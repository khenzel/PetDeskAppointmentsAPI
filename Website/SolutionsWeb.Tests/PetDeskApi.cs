using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SolutionsWeb.WebForms;
using static SolutionsWeb.classes.WebApi;
using static SolutionsWeb.classes.Cookies;

namespace SolutionsWeb.Tests
{
    [TestClass]
    public class PetDeskApi
    {
        private const string ApiUrl = "http://khenzel.info:8700/Assets/Projects/PetDeskAPI";

        [TestMethod]
        public void TestMethod1()
        {
            var token = GetToken(ApiUrl, "khenzel@hotmail.com", "Password123!");
            Assert.IsTrue(token.IndexOf("access_token") > 0);

            Assert.IsNotNull(classes.WebApi.CallApi(ApiUrl,token));
        }
    }
}
