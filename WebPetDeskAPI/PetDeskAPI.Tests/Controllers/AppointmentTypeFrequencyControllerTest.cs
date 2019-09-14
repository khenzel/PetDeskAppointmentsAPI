using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetDeskAPI.Controllers;

namespace PetDeskAPI.Tests.Controllers
{
    [TestClass]
    public class AppointmentTypeFrequencyControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            var controller = new AppointmentTypeFrequencyController();

            // Act
            var result = controller.Get();

            // Assert
            // Test rows were returned from the query, and that result contains a return
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Rows.Count > 0);

        }
    }
}
