namespace HospitalMVC.Models
{
    public class BookAppointmentViewModel
    {
        public AppointmentDto Appointment { get; set; } = new AppointmentDto();
        public List<DoctorDto> Doctors { get; set; } = new List<DoctorDto>();
    }
}
