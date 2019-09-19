using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Xml.Serialization;
using Utility;

//****************************************************************************************
//  petDeskGetAppointments Summary
//      - Hits PetDesk Appointments API, pulls appointment array 
//      - Stores appointmentType list and frequency to temp for AppointmentTypeFrequency API
//      - Stores requestedDateTimeOffset list and frequency to temp for AppointmentRequestFrequency API
//****************************************************************************************
//  Usage 
//      -Open validation for payload request. No user/password required.
//      -PetDesk Appointments API hit as follows:
//          -Retrieve Appointments: https://sampledata.petdesk.com/api/appointments
//      -Query Parameters: <NONE>
//      -API Return Format:
//          appointmentId               type
//          * appointmentType           type
//          createDateTime              type
//          * requestedDateTimeOffset   type
//          user:
//              userId                  type
//              firstName               type
//              lastName                type
//          animal:
//              animalId                type
//              firstName               type
//              species                 type
//              breed                   type
//
//          * signifies key fields we are processing in the request
//          all other fields are not in use for the target API's at this time
//
//      Sample JSON Return:
//            "appointmentId": 290318,
//            "appointmentType": "Other",
//            "createDateTime": "2018-11-28T22:57:33",
//            "requestedDateTimeOffset": "2018-12-01T08:00:00-08:00",
//            "user": {
//                "userId": 115066,
//                "firstName": "Tracey",
//                "lastName": "Polzin"
//            },
//            "animal": {
//                "animalId": 137900,
//                "firstName": "Jackson",
//                "species": "Dog",
//                "breed": "German Shepherd"
//            }
//****************************************************************************************
//  History:
//   09/05/2019 Kevin Henzel                    Created
//****************************************************************************************

//test string (debug enabled):
//http://khenzel.info:8700/WebForms/petDeskGetAppointments.aspx?debugmode=1


namespace SolutionsWeb.WebForms
{
    public partial class petDeskGetAppointments : System.Web.UI.Page
    {
        // query string container to indicate debug mode output via response.write
        private static string _debugmode;

        #region serilization classes
        public class AppointmentsRootobject
        {
            public int appointmentId { get; set; }
            public string appointmentType { get; set; }
            public object createDateTime { get; set; }
            public object requestedDateTimeOffset { get; set; }
            public UserData user { get; set; }
            public AnimalData animal { get; set; }
        }

        public class UserData
        {
            public int userId { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
        }

        public class AnimalData
        {
            public int animalId { get; set; }
            public string firstName { get; set; }
            public string species { get; set; }
            public string breed { get; set; }
        }
        #endregion DeSerilization PetDesk API Classes
        /// <summary>Handles the Load event of the Page control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Page URL Request Parameters
                _debugmode = Request.QueryString["debugmode"];
                #endregion Page URL Request Parameters
              
                // DebugMode text only displayed when query string "debugmode=1" is sent
                DebugModePrint("<h1><b>PetDesk Appointments Retrieval</b></h1><h2>***Debug Mode***</h2>");
                DebugModePrint("<p><b>Appointment retrieval initiated: </b></p>",true);

                // retrieve our appointments from the host API
                // return is an serialized object
                var appointments = RetrieveAppointments();

                DebugModePrint(SerializeObject(appointments));

                // Process the appointment types and requests to a DataTable with frequency counter
                var dtAppointments = AddRecordsToDataTable(appointments);

                DebugModePrint("<br><p><b>Appointments inserting into the database for retrieval.</b></p></br>",true);

                // DataTable is to be passed in via SPROC datatype param to insert to SolutionsWeb.dbo.tbl_PetDesk_Appointments_Frequency
                var dtResults = ExecuteProcedureWithDataTable(dtAppointments, 
                    "usp_petdesk_insert_appointments", "@appointmentsDataTable", "PetDeskAppointmentsDataTable");

                dgvResults.Visible = false;

                if (_debugmode == "1")
                {
                    // Pull records for display in debug mode
                    Db.Connection.EstablishDbConnection("usp_get_petdesk_appointments", dtResults, null, null, true, 60);

                    if (dtResults?.Rows.Count > 0)
                    {
                        dgvResults.Visible = true;
                        dgvResults.DataSource = dtResults;
                        dgvResults.DataBind();
                    }
                }

                DebugModePrint($"<br><p><b>{dtResults.Rows[0]["RowsInserted"]} records successfully inserted into the database.</b></p></br>",true);

                if (_debugmode == "1") return;

                // If not in debug mode, this auto closes the page after the service request is completed
                ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "petDeskGetAppointments",
                    "Page_Load: " + ex.Message);
            }           
        }

        /// <summary>Convert the incoming object into a DataTable containing the following structure:
        /// Type = apptType / apptRequest
        /// Value = apptType text / <month>-<year> converted from DateTime
        /// Frequency = # of occurrences the value has been identified</summary>
        /// <param name="appointments">The appointments.</param>
        /// <returns>DataTable.</returns>
        public static DataTable AddRecordsToDataTable(object appointments)
        {
            try
            {
                var dtAppointments = new DataTable();
                dtAppointments.Clear();
                dtAppointments.Columns.Add("Type");
                dtAppointments.Columns.Add("Value");
                dtAppointments.Columns.Add("Frequency");

                foreach (var o in (IEnumerable)appointments)
                {

                    var appointmentType = ((AppointmentsRootobject)o)?.appointmentType;
                    var appointmentRequest = Convert.ToDateTime(((AppointmentsRootobject)o)?.requestedDateTimeOffset);
                    var appointmentRequestMonthYear = $"{appointmentRequest.Month}-{appointmentRequest.Year}";                    
                    var countRequest = GetMatchingDataTableRow(dtAppointments, appointmentRequestMonthYear);

                    // Re-define types based off comma delimiter for multi-selection list items.
                    var appointmentTypes = appointmentType?.Split(',').Select(x => x.Trim()).ToArray();

                    if (appointmentTypes != null)
                        foreach (var appointment in appointmentTypes)
                        {
                            var countType = GetMatchingDataTableRow(dtAppointments, appointmentType);

                            // Process the appointment types. If a comma separated value is detected, treat it as an 
                            // additional row for frequency counting.
                            if (countType > 0)
                            {
                                // Here we have a matching segment for our type value field. Iterate the frequency for this field and move on.
                                dtAppointments.Rows[countType]["Frequency"] =
                                    Convert.ToInt32(dtAppointments.Rows[countType]["Frequency"]) + 1;
                            }
                            else
                            {
                                var r = dtAppointments.NewRow();

                                r["Type"] = "apptType";
                                r["Value"] = appointment;
                                r["Frequency"] = "1";

                                dtAppointments.Rows.Add(r);
                            }
                        }

                    if (countRequest > 0)
                    {
                        // Here we have a matching segment for our request value field. Iterate the frequency for this field and move on.
                        dtAppointments.Rows[countRequest]["Frequency"] = Convert.ToInt32(dtAppointments.Rows[countRequest]["Frequency"]) + 1;
                    }
                    else
                    {
                        var r = dtAppointments.NewRow();
                        r["Type"] = "apptRequest";
                        r["Value"] = appointmentRequestMonthYear;
                        r["Frequency"] = "1";
                        dtAppointments.Rows.Add(r);
                    }
                }

                return dtAppointments;
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "petDeskGetAppointments",
                    "AddRecordToDataTable: " + ex.Message);
                return null;
            }
        }

        /// <summary>Print out to the interface if the debugmode flag is set
        /// Optionally post as information to the application log table</summary>
        /// <param name="printMessage">The print message.</param>
        /// <param name="logToDatabase">if set to <c>true</c> [log to database].</param>
        private void DebugModePrint(string printMessage, bool logToDatabase = false)
        {
            try
            {
                if (_debugmode == "1")
                    Response.Write(printMessage);

                if (logToDatabase)
                    Db.Log.Write(Db.Log.Level.Information, "petDeskGetAppointments", printMessage);
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "petDeskGetAppointments",
                    "DebugModePrint: " + ex.Message);
            }
        }
      
        /// <summary>Passes data table payload in to the SQL SPROC as a parameter for storage.</summary>
        /// <param name="dt">generate an upload CSV structured for file upload.</param>
        /// <param name="storedProcedureName">Name of the stored procedure.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="dataTypeName">Name of the data type.</param>
        /// <returns>DataTable.</returns>
        public static DataTable ExecuteProcedureWithDataTable(DataTable dt, string storedProcedureName, string parameterName, string dataTypeName)
        {
            try
            {
                var dtResult = new DataTable();

                using (var connection = new SqlConnection(Db.Connection.GetConnectionString()))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = storedProcedureName;
                        command.CommandType = CommandType.StoredProcedure;

                        var parameter = command.Parameters
                            .AddWithValue(parameterName, dt);

                        parameter.SqlDbType = SqlDbType.Structured;
                        parameter.TypeName = $"dbo.{dataTypeName}";

                        var da = new SqlDataAdapter { SelectCommand = command };

                        da.Fill(dtResult);

                        return dtResult;
                    }
                }
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "petDeskGetAppointments",
                    "ExecuteProcedureWithDataTable: " + ex.Message);
                return null;
            }        
        }

        /// <summary>Returns the row index of the matching string value from the data table.</summary>
        /// <param name="dt">The dt.</param>
        /// <param name="value">The value.</param>
        /// <returns>System.Int32.</returns>
        private static int GetMatchingDataTableRow(DataTable dt, string value)
        {
            try
            {
                var result = dt.Select($"Value = '{value}'");

                return result.Length > 0 ? dt.Rows.IndexOf(result[0]) : 0;
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "petDeskGetAppointments",
                    "GetMatchingDataTableRow: " + ex.Message);
                return 0;
            }
        }

        /// <summary>Return all Appointments for PetDesk via JSON Deserialization from the target host.</summary>
        /// <returns>System.Object.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static object RetrieveAppointments()
        {
            try
            {
                const string url = "https://sampledata.petdesk.com/api/appointments";
                var webRequest = (HttpWebRequest)WebRequest.Create(url);

                webRequest.Method = "GET";
                webRequest.ContentType = "application/json";
                webRequest.ContentLength = 0;

                var webResponse = (HttpWebResponse)webRequest.GetResponse();

                if (webResponse.GetResponseStream() == null) return null;

                using (var reader = new StreamReader(webResponse.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    var streamRead = reader.ReadToEnd();
                    reader.Close();

                    var js = new JavaScriptSerializer();
                    var appointmentObj = js.Deserialize<List<AppointmentsRootobject>>(streamRead);

                    return appointmentObj;
                }
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "petDeskGetAppointments",
                    "RetrieveAppointments: " + ex.Message);
                return null;
            }
        }

        /// <summary>Converts the object and its members to a write-able string for view.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toSerialize">To serialize.</param>
        /// <returns>System.String.</returns>
        private static string SerializeObject<T>(T toSerialize)
        {
            try
            {              
                var xmlSerializer = new XmlSerializer(toSerialize.GetType());

                using (var textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, toSerialize);
                    return textWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "petDeskGetAppointments",
                    "SerializeObject: " + ex.Message);
                return null;
            }          
        }      
    }
}
