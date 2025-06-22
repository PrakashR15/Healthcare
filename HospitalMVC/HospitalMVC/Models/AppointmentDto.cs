namespace HospitalMVC.Models
{
    public class AppointmentDto
    {
        public int AppointmentID { get; set; }
        public DateTime Date { get; set; }
        public string DoctorID { get; set; }     
        public string PatientID { get; set; }  
        public string PatientName { get; set; } 
        public string Reason { get; set; }
        public string Status { get; set; }      
        public string DoctorName { get; set; }   
    }

    public class DoctorOption
    {
        public string Id { get; set; }
        public string FullName { get; set; }
    }
}


