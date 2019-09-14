using System;
using System.Collections;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolutionsWeb.classes;
using SolutionsWeb.WebForms;

namespace SolutionsWeb.Tests
{
    [TestClass]
    public class PetDeskApiTest
    {
        private const string ApiUrl = "http://khenzel.info:8700/Assets/Projects/PetDeskAPI";
        private const string TestUserEmail = "khenzel@hotmail.com";
        private const string TestUserPassword = "Password123!";

        private static readonly Random Random = new Random();
        private static readonly object SyncLock = new object();
        /// <summary>Returns a randomized number to be used with random email.</summary>
        /// <param name="min">The minimum.</param>
        /// <param name="max">The maximum.</param>
        /// <returns>System.string.</returns>
        public static string RandomNumber(int min, int max)
        {
            // Calling Random.Next causes a static access issue
            // This static wrapper resolves access to test case generation
            lock (SyncLock)
            { // synchronize
                return Random.Next(min, max).ToString();
            }
        }

        [TestMethod]
        public void PetDeskApiTestMethod()
        {
            // JSON Service Tests: ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // 1. The JSON service is pulling records from the open API.
            //   a. Testing Expectation: when the call is made to the provided source API, records are
            //      retrieved with no authentication needed from the API.
            var rawAppointments = petDeskGetAppointments.RetrieveAppointments();
            var collection = (IList)rawAppointments;
            var recordCount = collection.Cast<object>().Count();

            Assert.AreNotEqual(recordCount, 0);

            // 2. The JSON service is performing appropriate calculations to the deserialized data and is
            //      inserting them to the database for retrieval.
            //   a. Testing Expectation: Expected frequency calculation is being performed on the source data.
            var dtProcessedAppointmentData = petDeskGetAppointments.AddRecordsToDataTable(rawAppointments);
            var results = dtProcessedAppointmentData.AsEnumerable().Where(myRow => myRow.Field<string>("Frequency") != "1");

            Assert.AreNotEqual(results.ToList().Count(), 0);

            //    b. Testing Expectation: The groomed data is being inserted to the database without error.
            var dtResults = petDeskGetAppointments.ExecuteProcedureWithDataTable(dtProcessedAppointmentData, 
                "usp_petdesk_insert_appointments", "@appointmentsDataTable", 
                "PetDeskAppointmentsDataTable");

            Assert.AreNotEqual(Convert.ToInt32(dtResults.Rows[0]["RowsInserted"].ToString()), 0);

            // WEB UI TESTS: ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

            // 3. The Web API is pulling records from the database for both endpoints.
            //    a. Testing Expectation: For both endpoints a record count of the return from the database
            //       is producing records.

            // Get user token - satisfies unit test case 5b.
            var token = WebApi.GetToken(ApiUrl, TestUserEmail, TestUserPassword);

            // Validate that token has been returned
            Assert.IsTrue(token.IndexOf("access_token") > 0);

            // AppointmentRequestFrequency check - on success payload will include type field "apptRequest"
            var apiReturn = WebApi.CallApi(ApiUrl + "/api/AppointmentRequestFrequency", token);
            Assert.IsTrue(apiReturn.IndexOf("apptRequest") > 0);
            // AppointmentTypeFrequency check - on success payload will include type field "apptType"
            apiReturn = WebApi.CallApi(ApiUrl + "/api/AppointmentTypeFrequency", token);
            Assert.IsTrue(apiReturn.IndexOf("apptType") > 0);

            // 4. The Web UI is allowing users to properly create new accounts.
            //    a. Testing Expectation: When a user enters an invalid email format a rejection occurs.

            var isSuccess = WebApi.Register(ApiUrl, "bademailformat", TestUserPassword, 
                TestUserPassword);

            Assert.IsFalse(isSuccess);

            //    b. Testing Expectation: When a user enters an invalid password format (requires one
            //       upper case, one lower case, one numeric, and one symbol) a rejection occurs.

            isSuccess = WebApi.Register(ApiUrl, TestUserEmail, "password123!",
                "password123!");

            // No upper case test
            Assert.IsFalse(isSuccess);

            isSuccess = WebApi.Register(ApiUrl, TestUserEmail, "PASSWORD123!",
                "PASSWORD123!");

            // No lower case test
            Assert.IsFalse(isSuccess);

            isSuccess = WebApi.Register(ApiUrl, TestUserEmail, "Password!",
                "Password!");

            // No numeric test
            Assert.IsFalse(isSuccess);

            isSuccess = WebApi.Register(ApiUrl, TestUserEmail, "Password123",
                "Password123");

            // No symbol test
            Assert.IsFalse(isSuccess);

            //    c. Testing Expectation: When a user enters an invalid confirmation password a
            //       rejection occurs.

            isSuccess = WebApi.Register(ApiUrl, TestUserEmail, "Password123!",
                "Password321!");

            Assert.IsFalse(isSuccess);

            //    d. Testing Expectation: When a user attempts to create a duplicate user already existing
            //       in the database, an error occurs.

            isSuccess = WebApi.Register(ApiUrl, TestUserEmail, TestUserPassword,
                TestUserPassword);

            Assert.IsFalse(isSuccess);

            //    e. Testing Expectation: When all above requirements are met, the user can successfully
            //       create an account.

            // Generate a random email to ensure there is no collision on create
            isSuccess = WebApi.Register(ApiUrl, $"{RandomNumber(1000,999999999)}@UnitTestCase.Com", TestUserPassword,
                TestUserPassword);

            Assert.IsTrue(isSuccess);

            //  5. The Web UI Allows users to log in successfully.
            //     a. Testing Expectation: When the user improperly validates, login is rejected.

            var invalidToken = WebApi.GetToken(ApiUrl, TestUserEmail, "incorrectPassword");

            // Validate that token has been returned
            Assert.IsFalse(invalidToken.IndexOf("access_token") > 0);

            //    b. Testing Expectation: When the user properly validates, login is granted.

            // Handled above in token grab

            //    c. Testing Expectation: Upon successful login, the user is passed to the JSON service for
            //       data refresh.

            // This is tested during process flow

            //    d. Testing Expectation: Upon successful login, the access_token is stored as a cookie to
            //       the user’s browser under key “PetDeskApiToken”.

            // Retrieving a cookie only works with browser interaction. Must be validated during run-time.

            //    e. Testing Expectation: When the user logs out, the cookie is removed from the browser.

            // Retrieving a cookie only works with browser interaction. Must be validated during run-time.

            //    f. Testing Expectation: When the user requests a report from the API Report Request
            //       panel, the user is successfully authenticated against the API to produce a report.

            // This is validated in part by 3a. Remainder of validation will be through the UI.
        }
    }
}
