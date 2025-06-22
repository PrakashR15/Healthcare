namespace HospitalMVC.DTO
{
    public class AppointmentDto
    {
        public int AppointmentID { get; set; }    
        public string PatientID { get; set; }
        public string DoctorID { get; set; }
        public string DoctorName { get; set; }      
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } = "Pending";
    }
}

