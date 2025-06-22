





using HospitalAPI.Data;
using HospitalAPI.DTOs;
using HospitalAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AppointmentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doctorExists = await _context.Users.AnyAsync(u => u.Id == dto.DoctorID && u.Role == "Doctor");
            var patientExists = await _context.Users.AnyAsync(u => u.Id == dto.PatientID && u.Role == "Patient");

            if (!doctorExists)
                return BadRequest("Invalid DoctorID.");
            if (!patientExists)
                return BadRequest("Invalid PatientID.");

            var appointment = new Appointment
            {
                DoctorID = dto.DoctorID,
                PatientID = dto.PatientID,
                Date = dto.Date,
                Reason = dto.Reason,
                Status = string.IsNullOrWhiteSpace(dto.Status) ? "Pending" : dto.Status
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment booked successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Select(a => new
                {
                    a.AppointmentID,
                    a.Date,
                    a.Reason,
                    a.Status,
                    DoctorName = a.Doctor != null ? a.Doctor.FullName : "N/A",
                    PatientName = a.Patient != null ? a.Patient.FullName : "N/A"
                })
                .ToListAsync();

            return Ok(appointments);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(string patientId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Doctor)
                .Where(a => a.PatientID == patientId)
                .Select(a => new
                {
                    a.AppointmentID,
                    a.Date,
                    a.Reason,
                    a.Status,
                    DoctorName = a.Doctor.FullName
                })
                .ToListAsync();

            return Ok(appointments);
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctor(string doctorId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient)
                .Where(a => a.DoctorID == doctorId)
                .Select(a => new
                {
                    a.AppointmentID,
                    a.Date,
                    a.Reason,
                    a.Status,
                    PatientName = a.Patient != null ? a.Patient.FullName : "N/A"
                })
                .ToListAsync();

            return Ok(appointments);
        }

        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);
            if (appt == null)
                return NotFound("Appointment not found.");

            if (appt.Status == "Accepted")
                return BadRequest("Appointment already accepted.");

            appt.Status = "Accepted";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment confirmed." });
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectAppointment(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);
            if (appt == null)
                return NotFound("Appointment not found.");

            if (appt.Status == "Cancelled")
                return BadRequest("Appointment already cancelled.");

            appt.Status = "Cancelled"; // or "Rejected" if that's your convention
            await _context.SaveChangesAsync();

            return Ok(new { message = "Appointment rejected." });
        }
    }
}
