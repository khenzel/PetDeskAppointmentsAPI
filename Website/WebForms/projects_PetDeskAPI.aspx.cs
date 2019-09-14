using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Reflection;
using Newtonsoft.Json;
using static SolutionsWeb.classes.WebApi;
using static SolutionsWeb.classes.Cookies;

//****************************************************************************************
//  projects_PetDeskAPI.aspx.cs Summary
//      - User Interface for the PetDesk Appointments WebAPI project
//****************************************************************************************
//  Usage 
//      -Three panels for user interaction:
//          - New User Registration
//              - Hits the API endpoint WebPetDeskAPI/api/Account/Register to create a
//                  new user account
//          - Existing User Login
//              - Hits the API endpoint WebPetDeskAPI/Token 
//              - Upon successful login:
//                  - cookie "PetDeskApiToken" is generated that carries the auth_token value
//                  - petDeskGetAppointments JSON service call is hit to refresh client data
//          - API Report Request
//              - Hits the two endpoints with the Bearer token authentication
//                  - WebPetDeskAPI/api/AppointmentRequestFrequency
//                  - WebPetDeskAPI/api/AppointmentTypeFrequency
//              - Reporting data is posted to a GridView for report display
//****************************************************************************************
//  History:
//   09/05/2019 Kevin Henzel                    Created
//****************************************************************************************

namespace SolutionsWeb
{
    #region enums
    public enum Panels
    {
        pnlCreateUser = 1,
        pnlLogin = 2,
        pnlApiRequest = 3,
        pnlGridView = 4
    }

    public enum ReportType
    {
        AppointmentRequestFrequency = 1,
        AppointmentTypeFrequency = 2
    }
    #endregion enums

    public partial class projects_PetDeskAPI : System.Web.UI.Page
    {
        private const string ApiUrl = "http://khenzel.info:8700/Assets/Projects/PetDeskAPI";

        #region DeSerilization Appointments
        public class AppointmentsRootobject
        {
            public string type { get; set; }
            public string value { get; set; }
            public int frequency { get; set; }
        }
        #endregion DeSerilization Appointments

        /// <summary>Handles the Load event of the Page control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Legacy user login page validation logic
                #region login validation
                if (Request.Cookies["SolutionsWebUserAuth"] == null)
                {
                    //does the cookie exist?
                    var response = base.Response;
                    Utility.Db.Log.Write(Utility.Db.Log.Level.Information, "projects_PetDeskAPI", "projects_PetDeskAPI load unauthorized: " + Utility.Variables.strUserID);

                    var uri = new Uri(Request.Url.AbsoluteUri);

                    var noLastSegment = string.Format("{0}://{1}", uri.Scheme, uri.Authority);

                    for (var i = 0; i < uri.Segments.Length - 2; i++)
                    {
                        noLastSegment += uri.Segments[i];
                    }

                    noLastSegment = noLastSegment.Trim("/".ToCharArray()); // remove trailing `/`

                    response.Redirect(noLastSegment + "../login_failed.aspx", false);
                }
                if (Request.Cookies["SolutionsWebUserAuth"].Value.Trim() == "")
                {
                    //is the user name valid?
                    var response = Response;
                    Utility.Db.Log.Write(Utility.Db.Log.Level.Information, "projects_PetDeskAPI", "projects_PetDeskAPI load unauthorized: " + Utility.Variables.strUserID);

                    var uri = new Uri(Request.Url.AbsoluteUri);

                    var noLastSegment = string.Format("{0}://{1}", uri.Scheme, uri.Authority);

                    for (var i = 0; i < uri.Segments.Length - 1; i++)
                    {
                        noLastSegment += uri.Segments[i];
                    }

                    noLastSegment = noLastSegment.Trim("/".ToCharArray()); // remove trailing `/`

                    response.Redirect(noLastSegment + "../login_failed.aspx", false);
                }
                #endregion login validation

                // Maintain scroll position on postback
                Page.MaintainScrollPositionOnPostBack = true;

                if (!IsPostBack)
                {
                    // Initial page load defaults                   
                    ShowPanel((int) Panels.pnlCreateUser);
                    dgvResults.Visible = false;
                    lblNewUserRegistgrationStatusText.Text = "";
                    lblExistingStatusText.Text = "";

                    // Check to see if the user is currently logged in by reading the session cookie
                    if (!string.IsNullOrEmpty(Cookies_Get("PetDeskApiToken")))
                    {
                        // User already has a session cookie. Refresh the JSON data by calling the service
                        const string redirect = "<script >window.open('petDeskGetAppointments.aspx');</script>";
                        Response.Write(redirect);

                        // Then direct the user to the API panel to gather his or her report
                        ShowPanel((int)Panels.pnlApiRequest);
                    }
                }
                
            }
            catch(Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error,
                    "projects_PetDeskAPI", "Page_Load", ex.ToString());

                Response.Redirect("~/404.aspx");
            }

            var ecn = "";
            try
            {
                //string[] computer_name = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName.Split(new Char[] { '.' });
                ecn = Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName;
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, 
                    "projects_PetDeskAPI", "Unable to resolve DNS.", ex.ToString());
            }

            Utility.Db.Log.Write(Utility.Db.Log.Level.Information, 
                "projects_PetDeskAPI", "projects_PetDeskAPI page load by: " + Utility.Variables.strUserID, ecn);
        }

        /// <summary>Attempt to Login to the API using bearer token with validation of email and password</summary>
        /// <param name="email">user email address</param>
        /// <param name="password">user password</param>
        public void AttemptLogin(string email, string password)
        {
            try
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Information, 
                    "projects_PetDeskAPI", "AttemptLogin : " + Utility.Variables.strUserID);

                // Populate token with API call
                var token = GetUserInfo(email, password);

                // OWASP Standards dictate that help text does not indicate if a user exists in the system upon unsuccessful login.
                // Instead, a default failure message should be displayed.
                lblExistingStatusText.Text = "Error: Unable to log in. Please try again.";

                // Abort if access token was not returned
                if (token.IndexOf("access_token") <= 0) return;

                // Login successful.
                // Refresh the JSON data with a service call
                // This is processed as a hit to the petDeskGetAppointments service page opened up in a new tab.
                // After the service has processed, the tab is closed and the users are directed to the Api Request
                // panel to gather their reports.
                const string redirect = "<script >window.open('petDeskGetAppointments.aspx');</script>";
                Response.Write(redirect);

                // forward to the api request panel to request reports
                ShowPanel((int)Panels.pnlApiRequest);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, 
                    "projects_PetDeskAPI", "AttemptLogin: " + ex.Message, ex.ToString());
            }
        }

        /// <summary>Attempts to process AppointmentRequestFrequency API Report to Grid.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnApiRequest_Click(object sender, EventArgs e)
        {
            try
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Information,
                    "projects_PetDeskAPI", "btnApiRequest_Click : " + Utility.Variables.strUserID);

                ProcessApiReportRequest((int)ReportType.AppointmentRequestFrequency);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error,
                    "projects_PetDeskAPI", "btnApiRequest_Click: " + ex.Message, ex.ToString());
            }
        }

        /// <summary>Attempts to process AppointmentTypeFrequency API Report to Grid.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnApiType_Click(object sender, EventArgs e)
        {
            try
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Information,
                    "projects_PetDeskAPI", "btnApiType_Click : " + Utility.Variables.strUserID);

                ProcessApiReportRequest((int)ReportType.AppointmentTypeFrequency);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error,
                    "projects_PetDeskAPI", "btnApiType_Click: " + ex.Message, ex.ToString());
            }
        }

        /// <summary>Handles the Click event of the btnExistingRegister control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnExistingRegister_Click(object sender, EventArgs e)
        {
            try
            {
                ShowPanel((int)Panels.pnlCreateUser);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error,
                    "projects_PetDeskAPI", "btnExistingRegister_Click: " + ex.Message, ex.ToString());
            }
        }

        /// <summary>Handles the Click event of the btnExistingLogin control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnExistingLogin_Click(object sender, EventArgs e)
        {
            try
            {
                AttemptLogin(txtExistingEmail.Text, txtExistingPassword.Text);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error,
                    "projects_PetDeskAPI", "btnExistingLogin_Click: " + ex.Message, ex.ToString());
            }
        }

        /// <summary>Button to switch to Login panel from the New User Registration panel.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnNewLogin_Click(object sender, EventArgs e)
        {
            try
            {
                lblExistingStatusText.Text = "";
                ShowPanel((int)Panels.pnlLogin);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error,
                    "projects_PetDeskAPI", "btnNewLogin_Click: " + ex.Message, ex.ToString());
            }
        }

        /// <summary>New User Registration panel button to register a new user provided the user name (email) and password.
        /// Fields used to validate against are txtEmail, txtPassword, and txtPasswordConfirm.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnNewRegister_Click(object sender, EventArgs e)
        {
            try
            {
                var isSuccess = Register(ApiUrl, txtEmail.Text, txtPassword.Text, txtPasswordConfirm.Text);

                lblNewUserRegistgrationStatusText.Text = "";

                if (isSuccess)
                {
                    ShowPanel((int)Panels.pnlLogin);
                }
                else
                {
                    lblNewUserRegistgrationStatusText.Text =
                        "Error : Unable to create user. Please ensure you are following proper email/password conventions.";
                }
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error,
                    "projects_PetDeskAPI", "btnApiType_Click: " + ex.Message, ex.ToString());
            }
        }

        /// <summary>Return user to login screen and log the user out.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // Kill the cookie token value on logout
                Cookies_Delete("PetDeskApiToken");

                // Send the user back to the create user screen
                ShowPanel((int)Panels.pnlCreateUser);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error,
                    "projects_PetDeskAPI", "btnLogout_Click: " + ex.Message, ex.ToString());
            }
        }

        /// <summary>Creates the data table.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">The list.</param>
        /// <returns>DataTable.</returns>
        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            try
            {
                var type = typeof(T);
                var properties = type.GetProperties();

                var dataTable = new DataTable();
                foreach (var info in properties)
                {
                    dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
                }

                foreach (var entity in list)
                {
                    var values = new object[properties.Length];
                    for (var i = 0; i < properties.Length; i++)
                    {
                        values[i] = properties[i].GetValue(entity);
                    }

                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, 
                    "projects_PetDeskAPI", "CreateDataTable: " + ex.Message, ex.ToString());
                return null;
            }         
        }

        /// <summary>Gets the user information.</summary>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
        protected string GetUserInfo(string email, string password)
        {
            try
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Information, 
                    "projects_PetDeskAPI", "GetUserInfo : " + Utility.Variables.strUserID);

                // Grab the token from the API
                var token = GetToken(ApiUrl, email, password);

                // Store the token as a cookie and set it to expire in 24 hours
                Cookies_Set("PetDeskApiToken",token,DateTime.Now.AddDays(1));

                return token;
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, 
                    "projects_PetDeskAPI", "GetUserInfo: " + ex.Message, ex.ToString());
                return null;
            }
        }

        /// <summary>Grabs the requested report - requires token has been populated with valid auth.</summary>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>System.String.</returns>
        protected static string GetReport(int reportType)
        {
            try
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Information, 
                    "projects_PetDeskAPI", "GetReport : " + Utility.Variables.strUserID);

                var returnVal = "";
                var token = Cookies_Get("PetDeskApiToken");
                
                switch (reportType)
                {
                    case 1: // AppointmentRequestFrequency
                        returnVal = CallApi(ApiUrl + "/api/AppointmentRequestFrequency", token);
                        break;
                    case 2: // AppointmentTypeFrequency
                        returnVal = CallApi(ApiUrl + "/api/AppointmentTypeFrequency", token);
                        break;
                }

                return returnVal;
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, 
                    "projects_PetDeskAPI", "GetReport: " + ex.Message, ex.ToString());
                return null;
            }
        }

        /// <summary>Processes the API report request.</summary>
        /// <param name="reportType">Type of the report.</param>
        /// <returns>DataTable.</returns>
        public DataTable ProcessApiReportRequest(int reportType)
        {
            try
            {
                var result = GetReport(reportType);

                // Convert result to a DataGridView to bind to our grid
                var objOrderList = JsonConvert.DeserializeObject<List<AppointmentsRootobject>>(result);
                var dtResults = CreateDataTable(objOrderList);

                dgvResults.Visible = true;
                dgvResults.DataSource = dtResults;
                dgvResults.DataBind();
                lblGridViewHeader.Text = "Appointment Requests Received Per Month :";

                ShowPanel((int)Panels.pnlGridView);

                return dtResults;
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "projects_PetDeskAPI",
                    "ProcessApiReportRequest: " + ex.Message, ex.ToString());
                return null;
            }
        }

        /// <summary>Handles the Click event of the runJsonService control.</summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void runJsonService_Click(object sender, EventArgs e)
        {
            try
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Information, 
                    "projects_PetDeskAPI", "runJsonService_Click : " + Utility.Variables.strUserID);

                const string redirect = "<script >window.open('petDeskGetAppointments.aspx');</script>";
                Response.Write(redirect);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, 
                    "projects_PetDeskAPI", "runJsonService_Click: " + ex.Message, ex.ToString());
            }
        }

        /// <summary>Hide/expose indicated panel group.</summary>
        /// <param name="panelToExpose">The panel to expose.</param>
        protected void ShowPanel(int panelToExpose)
        {
            try
            {
                pnlCreateUser.Visible = false;
                pnlLogin.Visible = false;
                pnlApiRequest.Visible = false;
                pnlGridView.Visible = false;

                switch (panelToExpose)
                {
                    case 2: // pnlLogin
                        pnlLogin.Visible = true;
                        break;
                    case 3: // pnlApiRequest
                        pnlApiRequest.Visible = true;
                        break;
                    case 4: // pnlGridView + pnlApiRequest
                        pnlApiRequest.Visible = true;
                        pnlGridView.Visible = true;
                        break;
                    default: // pnlCreateUser (default)
                        pnlCreateUser.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, 
                    "projects_PetDeskAPI", "showPanel: " + ex.Message, ex.ToString());
            }
        }         
    }
}