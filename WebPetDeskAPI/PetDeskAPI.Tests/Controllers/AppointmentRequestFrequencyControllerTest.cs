using Microsoft.VisualStudio.TestTools.UnitTesting;
using PetDeskAPI.Controllers;

namespace PetDeskAPI.Tests.Controllers
{
    [TestClass]
    public class AppointmentRequestFrequencyControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            var controller = new AppointmentRequestFrequencyController();

            // Act
            var result = controller.Get();

            // Assert
            Assert.IsNotNull(result);

        }
    }
}
