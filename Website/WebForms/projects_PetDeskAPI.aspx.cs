using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using RestSharp;
using SolutionsWeb.classes;
using static SolutionsWeb.classes.WebApi;
using static SolutionsWeb.classes.Cookies;


namespace SolutionsWeb
{
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.Cookies["SolutionsWebUserAuth"] == null)
                {
                    //does the cookie exist?
                    var response = base.Response;
                    Utility.Db.Log.Write(Utility.Db.Log.Level.Information, "SolutionsWeb", "projects_PetDeskAPI load unathorized: " + Utility.Variables.strUserID);

                    var uri = new Uri(Request.Url.AbsoluteUri);

                    var noLastSegment = string.Format("{0}://{1}", uri.Scheme, uri.Authority);

                    for (int i = 0; i < uri.Segments.Length - 2; i++)
                    {
                        noLastSegment += uri.Segments[i];
                    }

                    noLastSegment = noLastSegment.Trim("/".ToCharArray()); // remove trailing `/`

                    response.Redirect(noLastSegment + "../login_failed.aspx", false);
                }
                if (Request.Cookies["SolutionsWebUserAuth"].Value.Trim() == "")
                {
                    //is the user name valid?
                    var response = base.Response;
                    Utility.Db.Log.Write(Utility.Db.Log.Level.Information, "SolutionsWeb", "projects_PetDeskAPI load unathorized: " + Utility.Variables.strUserID);

                    var uri = new Uri(Request.Url.AbsoluteUri);

                    var noLastSegment = string.Format("{0}://{1}", uri.Scheme, uri.Authority);

                    for (int i = 0; i < uri.Segments.Length - 1; i++)
                    {
                        noLastSegment += uri.Segments[i];
                    }

                    noLastSegment = noLastSegment.Trim("/".ToCharArray()); // remove trailing `/`

                    response.Redirect(noLastSegment + "../login_failed.aspx", false);
                }

                if (!IsPostBack)
                {
                    ShowPanel((int) Panels.pnlCreateUser);
                    dgvResults.Visible = false;
                    lblNewUserRegistgrationStatusText.Text = "";
                    lblExistingStatusText.Text = "";
                }
                
            }
            catch
            {
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
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "Unable to resolve DNS.", ex.ToString());
            }

            Utility.Db.Log.Write(Utility.Db.Log.Level.Information, "SolutionsWeb", "projects_PetDeskAPI page load by: " + Utility.Variables.strUserID, ecn);
        }

        public void AttemptLogin()
        {
            try
            {
                var token = GetUserInfo();

                lblExistingStatusText.Text = "Error: The user name or password is incorrect.";

                if (token.IndexOf("access_token") <= 0) return;

                // Login successful.
                // Refresh the JSON data with a service call
                const string redirect = "<script >window.open('petDeskGetAppointments.aspx');</script>";
                Response.Write(redirect);

                // forward to query panel
                ShowPanel((int)Panels.pnlApiRequest);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "AttemptLogin: " + ex.Message, ex.ToString());
            }
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            try
            {
                // Convert JSON deserialized object to datatable
                Type type = typeof(T);
                var properties = type.GetProperties();

                DataTable dataTable = new DataTable();
                foreach (PropertyInfo info in properties)
                {
                    dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
                }

                foreach (T entity in list)
                {
                    object[] values = new object[properties.Length];
                    for (int i = 0; i < properties.Length; i++)
                    {
                        values[i] = properties[i].GetValue(entity);
                    }

                    dataTable.Rows.Add(values);
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "CreateDataTable: " + ex.Message, ex.ToString());
                return null;
            }         
        }

        protected string GetUserInfo()
        {
            // Grabs token with provided URL, username, and password
            try
            {
                var token = GetToken(ApiUrl, txtExistingEmail.Text, txtExistingPassword.Text);

                // Store the token as a cookie and set it to expire in 24 hours
                Cookies_Set("PetDeskApiToken",token,DateTime.Now.AddDays(1));

                return token;
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "GetUserInfo: " + ex.Message, ex.ToString());
                return null;
            }
        }

        protected string GetReport(int reportType)
        {
            // Grabs the requested report - requires token has been populated with valid auth
            try
            {
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
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "GetReport: " + ex.Message, ex.ToString());
                return null;
            }

            //DataTable dt = (DataTable)JsonConvert.DeserializeObject(returnVal, (typeof(DataTable)));      

        }

        protected void runJsonService_Click(object sender, EventArgs e)
        {
            // Tests refresh hit to the server
            try
            {
                const string redirect = "<script >window.open('petDeskGetAppointments.aspx');</script>";
                Response.Write(redirect);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "runJsonService_Click: " + ex.Message, ex.ToString());
            }
        }

        protected void ShowPanel(int panelToExpose)
        {
            // Hide/expose indidcated panel group
            try
            {
                switch (panelToExpose)
                {
                    case 2: // pnlLogin
                        pnlCreateUser.Visible = false;
                        pnlLogin.Visible = true;
                        pnlApiRequest.Visible = false;
                        pnlGridView.Visible = false;
                        break;
                    case 3: // pnlApiRequest
                        pnlCreateUser.Visible = false;
                        pnlLogin.Visible = false;
                        pnlApiRequest.Visible = true;
                        pnlGridView.Visible = false;
                        break;
                    case 4: // pnlGridView + pnlApiRequest
                        pnlCreateUser.Visible = false;
                        pnlLogin.Visible = false;
                        pnlApiRequest.Visible = true;
                        pnlGridView.Visible = true;
                        break;
                    default: // pnlCreateUser (default)
                        pnlCreateUser.Visible = true;
                        pnlLogin.Visible = false;
                        pnlApiRequest.Visible = false;
                        pnlGridView.Visible = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "showPanel: " + ex.Message, ex.ToString());
            }
        }

        protected void btnExistingRegister_Click(object sender, EventArgs e)
        {
            try
            {
                ShowPanel((int)Panels.pnlCreateUser);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "btnExistingRegister_Click: " + ex.Message, ex.ToString());
            }
        }

        protected void btnNewLogin_Click(object sender, EventArgs e)
        {
            // Button to switch to Login panel from the New User Registration panel
            try
            {
                lblExistingStatusText.Text = "";
                ShowPanel((int)Panels.pnlLogin);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "btnNewLogin_Click: " + ex.Message, ex.ToString());
            }
        }

        protected void btnExistingLogin_Click(object sender, EventArgs e)
        {
            // Existing user login button
            try
            {
                AttemptLogin();
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "btnExistingLogin_Click: " + ex.Message, ex.ToString());
            }
        }
       
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Return user to login screen and log the user out
            try
            {
                // Kill the cookie token value on logout
                Cookies_Delete("PetDeskApiToken");

                // Send the user back to the create user screen
                ShowPanel((int)Panels.pnlCreateUser);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "btnLogout_Click: " + ex.Message, ex.ToString());
            }
        }

        protected void btnApiRequest_Click(object sender, EventArgs e)
        {
            // Attempts to process AppointmentRequestFrequency API Report to Grid
            try
            {
                var result = GetReport((int)ReportType.AppointmentRequestFrequency);

                // Convert result to a DataGridView to bind to our grid
                var objOrderList = JsonConvert.DeserializeObject<List<AppointmentsRootobject>>(result);
                var dtResults = CreateDataTable(objOrderList);

                dgvResults.Visible = true;
                dgvResults.DataSource = dtResults;
                dgvResults.DataBind();

                ShowPanel((int)Panels.pnlGridView);

            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "btnApiRequest_Click: " + ex.Message, ex.ToString());
            }
        }

        protected void btnApiType_Click(object sender, EventArgs e)
        {
            // Attempts to process AppointmentTypeFrequency API Report to Grid
            try
            {
                var result = GetReport((int)ReportType.AppointmentTypeFrequency);

                // Convert result to a DataGridView to bind to our grid
                var objOrderList = JsonConvert.DeserializeObject<List<AppointmentsRootobject>>(result);
                var dtResults = CreateDataTable(objOrderList);

                dgvResults.Visible = true;
                dgvResults.DataSource = dtResults;
                dgvResults.DataBind();

                ShowPanel((int)Panels.pnlGridView);
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "btnApiType_Click: " + ex.Message, ex.ToString());
            }
        }

        protected void btnNewRegister_Click(object sender, EventArgs e)
        {
            // New User Registration panel button to register a new user provided the user name (email) and password
            // Fields used to validate against are txtEmail, txtPassword, and txtPasswordConfirm
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
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "SolutionsWeb", "btnApiType_Click: " + ex.Message, ex.ToString());
            }
        }
    }
}