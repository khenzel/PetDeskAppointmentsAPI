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
            Assert.IsNotNull(result);

        }
    }
}
