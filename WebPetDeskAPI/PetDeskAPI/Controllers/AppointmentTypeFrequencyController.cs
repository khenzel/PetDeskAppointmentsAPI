using System.Data;
using System.Web.Http;
using PetDeskAPI.Models;

namespace PetDeskAPI.Controllers
{
    [Authorize]
    public class AppointmentTypeFrequencyController : ApiController
    {
        private DataTable dtAppointmentTypeFrequency = new DataTable();

        public AppointmentTypeFrequencyController()
        {
            // Constructor populates our data from the model for return to our Get API call
            var model = new AppointmentTypeFrequencyModels();
            dtAppointmentTypeFrequency = model.appointmentTypeFrequency;
        }

        // GET: api/AppointmentDistribution
        public DataTable Get()
        {
            //return appointment type data to the caller;
            return dtAppointmentTypeFrequency;
        }   
    }
}
