using System.Data;
using System.Web.Http;
using Utility;

namespace PetDeskAPI.Controllers
{
    [Authorize]
    public class AppointmentTypeFrequencyController : ApiController
    {
        private DataTable dtReturn = new DataTable();

        public AppointmentTypeFrequencyController()
        {
            var aryParameters = new[,] { { "@TypeFilter" }, { "2" } };
            var oAryType = new[] { SqlDbType.NVarChar };

            Db.Connection.EstablishDbConnection("usp_get_petdesk_appointments", dtReturn, aryParameters,
                oAryType, true, 60);
        }

        // GET: api/AppointmentDistribution
        public DataTable Get()
        {
            //return apptRequest;
            return dtReturn;
        }
    }
}
