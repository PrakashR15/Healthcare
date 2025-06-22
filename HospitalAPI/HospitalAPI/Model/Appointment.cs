using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Providers.Entities;

using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Model
{
    public class Appointment
    {
        public int AppointmentID { get; set; }

        public string DoctorID { get; set; }
        public AppUser Doctor { get; set; }

        public string PatientID { get; set; }
        public AppUser Patient { get; set; }

        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }
    }

}
