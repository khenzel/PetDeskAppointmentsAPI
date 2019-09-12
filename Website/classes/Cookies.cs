using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolutionsWeb.classes
{
    public class Cookies
    {
        public static void Cookies_Set(string key, string value, DateTime expires)
        {
            //store cookie based off key, value, and expiration date
            var cookie = new HttpCookie(key, value) { Expires = expires };
            HttpContext.Current.Response.SetCookie(cookie);
        }


        public static string Cookies_Get(string key)
        {
            //Returns stored cookie value for sproc pull in GemericRules - spTeamComplianceGetCountsSearch
            var value = "";

            if (!HttpContext.Current.Request.Cookies.AllKeys.Contains(key)) return value;

            var requestCookie = HttpContext.Current.Request.Cookies[key];
            if (requestCookie != null)
                value = requestCookie.Value;

            return value;
        }

        public static void Cookies_Delete(string key)
        {
            //delete current cookie based on key
            if (HttpContext.Current.Request.Cookies[key] == null) return;

            var responseCookie = HttpContext.Current.Response.Cookies[key];

            if (responseCookie != null) responseCookie.Value = "";

            var httpCookie = HttpContext.Current.Response.Cookies[key];

            if (httpCookie != null)
                httpCookie.Expires = DateTime.Now.AddDays(-1);
        }
    }
}