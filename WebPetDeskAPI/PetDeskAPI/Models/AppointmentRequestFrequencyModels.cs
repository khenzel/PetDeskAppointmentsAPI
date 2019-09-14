using System;
using System.Data;
using Utility;

namespace PetDeskAPI.Models
{
    public class AppointmentRequestFrequencyModels
    {
        public DataTable appointmentRequestFrequency { get; set; } = new DataTable();

        public AppointmentRequestFrequencyModels()
        {
            // Constructor pulls the appointment Request data from our database connection
            try
            {
                var aryParameters = new[,] { { "@TypeFilter" }, { "1" } };

                Db.Connection.EstablishDbConnection("usp_get_petdesk_appointments", appointmentRequestFrequency, aryParameters,
                    null, true, 60);

                Db.Log.Write(Db.Log.Level.Information, "PetDeskAPI",
                    $"AppointmentRequestFrequencyModels: {appointmentRequestFrequency.Rows.Count} records returned");
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "PetDeskAPI", "AppointmentRequestFrequencyModels: " + ex.Message, ex.ToString());
            }
        }
    }
}