﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Utility;

//****************************************************************************************
//  WebApi Summary
//      - Helper classes to manage web API calls for user account creation and authentication
//****************************************************************************************
//  Usage 
//      - CallApi - Calls the API with the Bearer Auth token provided (gathered from GetToken)
//      - GetToken - Gets the token for the authenticated User (email) and password
//      - Register - Creates a new user account in the API
//****************************************************************************************
//  History:
//   09/05/2019 Kevin Henzel                    Created
//****************************************************************************************

namespace SolutionsWeb.classes
{
    public class WebApi
    {
        #region serilization classes
        class RegisterModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
        }

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
        #endregion serilization classes

        /// <summary>Calls the API with the Bearer Auth token provided (gathered from GetToken).</summary>
        /// <param name="url">The URL.</param>
        /// <param name="token">The token.</param>
        /// <returns>System.String.</returns>
        public static string CallApi(string url, string token)
        {
            try
            {
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
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "WebApi", "CallApi: " + ex.Message, ex.ToString());
                return null;
            }
        }

        /// <summary>Gets the token for the authenticated User (email) and password.</summary>
        /// <param name="url">The URL.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
        public static string GetToken(string url, string userName, string password)
        {
            try
            {
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
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "WebApi", "GetToken: " + ex.Message, ex.ToString());
                return null;
            }
        }

        /// <summary>Creates a new user account in the API.</summary>
        /// <param name="url">The URL.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="confirmPassword">The confirm password.</param>
        /// <returns>
        ///   <c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Register(string url, string username, string password, string confirmPassword)
        {
            try
            {
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

                request.GetResponse();

                return true;
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "WebApi", "Register: " + ex.Message, ex.ToString());
                return false;
            }
        }
    }
}