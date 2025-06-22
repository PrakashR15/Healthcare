using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.DTOs
{
    public class CreateAppointmentDto
    {
        [Required]
        public string DoctorID { get; set; }

        [Required]
        public string PatientID { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Reason { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
