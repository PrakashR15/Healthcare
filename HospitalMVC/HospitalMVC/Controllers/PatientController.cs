using HospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace HospitalFrontendMVC.Controllers
{
    public class PatientController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public PatientController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> BookAppointment()
        {
            await LoadDoctorsIntoViewBag();
            return View(new AppointmentDto { Date = DateTime.Now });
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(AppointmentDto dto)
        {
            dto.PatientID = HttpContext.Session.GetString("userId");

            if (
                string.IsNullOrEmpty(dto.PatientID) ||
                string.IsNullOrEmpty(dto.DoctorID) ||
                string.IsNullOrEmpty(dto.Reason) ||
                dto.Date == default || dto.Date < DateTime.Now)
            {
                ViewBag.Error = "All fields are required, and the date must be valid and in the future.";
                await LoadDoctorsIntoViewBag();
                return View(dto);
            }

            var client = _httpClientFactory.CreateClient();
            var baseUrl = _config["ApiSettings:BaseUrl"];
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/api/Appointment", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["Message"] = "Appointment booked successfully.";
                return RedirectToAction("AppointmentHistory");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            ViewBag.Error = $"Booking failed: {errorContent}";
            await LoadDoctorsIntoViewBag();
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> AppointmentHistory()
        {
            var patientId = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(patientId))
            {
                TempData["Error"] = "You must be logged in.";
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();
            var baseUrl = _config["ApiSettings:BaseUrl"];
            var resp = await client.GetAsync($"{baseUrl}/api/Appointment/patient/{patientId}");

            if (!resp.IsSuccessStatusCode)
            {
                ViewBag.Error = "Unable to retrieve appointments.";
                return View(new List<AppointmentDto>());
            }

            var json = await resp.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<AppointmentDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(list ?? new List<AppointmentDto>());
        }

        private async Task LoadDoctorsIntoViewBag()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _config["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}/api/Doctor");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var doctors = JsonSerializer.Deserialize<List<DoctorOption>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                ViewBag.Doctors = doctors;
            }
            else
            {
                ViewBag.Doctors = new List<DoctorOption>();
            }
        }
    }
}
