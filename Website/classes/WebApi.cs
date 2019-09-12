using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace SolutionsWeb.classes
{
    public class WebApi
    {
        class Token
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
            public string userName { get; set; }
            [JsonProperty(".issued")]
            public string issued { get; set; }
            [JsonProperty(".expires")]
            public string expires { get; set; }
        }

        public static string GetToken(string url, string userName, string password)
        {
            // Gets the token for the authenticated User (email) and password 
            var pairs = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>( "grant_type", "password" ),
                new KeyValuePair<string, string>( "username", userName ),
                new KeyValuePair<string, string> ( "Password", password )
            };
            var content = new FormUrlEncodedContent(pairs);
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                var response = client.PostAsync(url + "/Token", content).Result;
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        public static string CallApi(string url, string token)
        {
            // Calls the API with the Bearer Auth token provided (gathered from GetToken)
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                if (!string.IsNullOrWhiteSpace(token))
                {
                    var t = JsonConvert.DeserializeObject<Token>(token);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + t.access_token);
                }
                var response = client.GetAsync(url).Result;

                return response.Content.ReadAsStringAsync().Result;
            }
        }

        class RegisterModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

        public static bool Register(string url, string username, string password, string confirmPassword)
        {
            // Creates a new user account in the API
            var model = new RegisterModel
            {
                ConfirmPassword = confirmPassword,
                Password = password,
                Email = username
            };

            var request = (HttpWebRequest)WebRequest.Create(url + "/api/Account/Register");
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "application/json";
            var json = JsonConvert.SerializeObject(model);
            var bytes = Encoding.UTF8.GetBytes(json);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            try
            {
                request.GetResponse();

                return true;
            }
            catch (Exception ex)
            {
                Utility.Db.Log.Write(Utility.Db.Log.Level.Error, "WebApi", "Register: " + ex.Message, ex.ToString());
                return false;
            }
        }
    }
}