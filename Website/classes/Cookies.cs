using System;
using System.Linq;
using System.Web;
using Utility;

//****************************************************************************************
//  Cookies Summary
//      - Helper classes to get, set, and delete cookies to the browser
//****************************************************************************************
//  Usage 
//      - Cookies_Delete - removes/expires cookie by key
//      - Cookies_Get - retrieve cookie by key
//      - Cookies_Set - sets cookie by key, value, and expiration date
//****************************************************************************************
//  History:
//   09/05/2019 Kevin Henzel                    Created
//****************************************************************************************

namespace SolutionsWeb.classes
{
    public class Cookies
    {
        /// <summary>delete current cookie based on key.</summary>
        /// <param name="key">The key.</param>
        public static void Cookies_Delete(string key)
        {
            try
            {
                if (HttpContext.Current.Request.Cookies[key] == null) return;

                var responseCookie = HttpContext.Current.Response.Cookies[key];

                if (responseCookie != null) responseCookie.Value = "";

                var httpCookie = HttpContext.Current.Response.Cookies[key];

                if (httpCookie != null)
                    httpCookie.Expires = DateTime.Now.AddDays(-1);
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "Cookies", "Cookies_Delete: " + ex.Message, ex.ToString());
            }
        }

        /// <summary>Returns stored cookie value for sproc pull in GemericRules - spTeamComplianceGetCountsSearch.</summary>
        /// <param name="key">The key.</param>
        /// <returns>System.String.</returns>
        public static string Cookies_Get(string key)
        {
            try
            {
                var value = "";

                if (!HttpContext.Current.Request.Cookies.AllKeys.Contains(key)) return value;

                var requestCookie = HttpContext.Current.Request.Cookies[key];
                if (requestCookie != null)
                    value = requestCookie.Value;

                return value;
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "Cookies", "Cookies_Get: " + ex.Message, ex.ToString());
                return null;
            }
        }

        /// <summary>store cookie based off key, value, and expiration date.</summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="expires">The expires.</param>
        public static void Cookies_Set(string key, string value, DateTime expires)
        {
            try
            {
                var cookie = new HttpCookie(key, value) { Expires = expires };
                HttpContext.Current.Response.SetCookie(cookie);
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "Cookies", "Cookies_Set: " + ex.Message, ex.ToString());
            }
        }
    }
}