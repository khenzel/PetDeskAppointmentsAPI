using System.Data;
using System.Web.Http;
using PetDeskAPI.Models;

namespace PetDeskAPI.Controllers
{
    [Authorize]
    public class AppointmentRequestFrequencyController : ApiController
    {
        private DataTable dtAppointmentRequestFrequency = new DataTable();

        public AppointmentRequestFrequencyController()
        {
            // Constructor populates our data from the model for return to our Get API call
            var model = new AppointmentRequestFrequencyModels();
            dtAppointmentRequestFrequency = model.appointmentRequestFrequency;
        }

        // GET: api/AppointmentRequest
        public DataTable Get()
        {
            //return appointment request data to the caller
            return dtAppointmentRequestFrequency;
        }

    }
}
