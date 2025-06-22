using Microsoft.AspNetCore.Mvc;
using HospitalAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoctorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetDoctors()
        {
            var doctors = _context.Users
                .Where(u => u.Role == "Doctor")
                .Select(u => new
                {
                    Id = u.Id,
                    FullName = u.FullName
                }).ToList();

            return Ok(doctors);
        }

        // ✅ NEW: Get all app
        [HttpGet("appointments/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctor(string doctorId)
        {
            var appointments = await _context.Appointments
                .Include(a => a.Patient) // Join with User table
                .Where(a => a.DoctorID == doctorId)
                .Select(a => new
                {
                    AppointmentID = a.AppointmentID,
                    PatientName = a.Patient.FullName, // ✅ get name from related user
                    Reason = a.Reason,
                    Status = a.Status,
                    Date = a.Date
                })
                .ToListAsync();

            return Ok(appointments);
        }

    }
}

