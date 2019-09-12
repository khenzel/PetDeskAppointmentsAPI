using System.Data;
using System.Web.Http;
using Utility;

namespace PetDeskAPI.Controllers
{
    [Authorize]
    public class AppointmentRequestFrequencyController : ApiController
    {
        private DataTable dtReturn = new DataTable();

        public AppointmentRequestFrequencyController()
        {
            var aryParameters = new[,] { { "@TypeFilter" }, { "1" } };
            var oAryType = new[] { SqlDbType.NVarChar };

            Db.Connection.EstablishDbConnection("usp_get_petdesk_appointments", dtReturn, aryParameters,
                oAryType, true, 60);
        }

        // GET: api/AppointmentRequest
        public DataTable Get()
        {
            //return apptRequest;
            return dtReturn;
        }

    }
}
