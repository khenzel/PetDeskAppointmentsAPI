using System;
using System.Data;
using Utility;

namespace PetDeskAPI.Models
{
    public class AppointmentTypeFrequencyModels
    {
        public DataTable appointmentTypeFrequency { get; set; } = new DataTable();

        public AppointmentTypeFrequencyModels()
        {
            // Constructor pulls the appointment type data from our database connection
            try
            {
                var aryParameters = new[,] { { "@TypeFilter" }, { "2" } };

                Db.Connection.EstablishDbConnection("usp_get_petdesk_appointments", appointmentTypeFrequency, aryParameters,
                    null, true, 60);

                Db.Log.Write(Db.Log.Level.Information, "PetDeskAPI",
                    $"AppointmentTypeFrequencyModels: {appointmentTypeFrequency.Rows.Count} records returned");
            }
            catch (Exception ex)
            {
                Db.Log.Write(Db.Log.Level.Error, "PetDeskAPI", "AppointmentTypeFrequencyModels: " + ex.Message, ex.ToString());
            }
        }        
    }
}