using HospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HospitalFrontendMVC.Controllers             
{
    public class DoctorController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public DoctorController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> MyAppointments()
        {
            var doctorId = HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(doctorId))
            {
                TempData["Error"] = "Doctor ID not found in session.";
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();
            var baseUrl = _config["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}/api/Appointment/doctor/{doctorId}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load appointments.";
                return View(new List<AppointmentDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var appointments = JsonSerializer.Deserialize<List<AppointmentDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _config["ApiSettings:BaseUrl"];

            var response = await client.PutAsync($"{baseUrl}/api/Appointment/{id}/confirm", null);
            var text = await response.Content.ReadAsStringAsync();

            TempData["Error"] = response.IsSuccessStatusCode
                ? null
                : $"Error {response.StatusCode}: {text}";

            return RedirectToAction("MyAppointments");
        }

        [HttpPost]
        public async Task<IActionResult> RejectAppointment(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _config["ApiSettings:BaseUrl"];

            var response = await client.PutAsync($"{baseUrl}/api/Appointment/{id}/reject", null); // Ensure this endpoint exists in your API
            var text = await response.Content.ReadAsStringAsync();

            TempData["Error"] = response.IsSuccessStatusCode
                ? null
                : $"Error {response.StatusCode}: {text}";

            return RedirectToAction("MyAppointments");
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _config["ApiSettings:BaseUrl"];
            var response = await client.GetAsync($"{baseUrl}/api/User/doctors");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Failed to load doctors.";
                return View(new List<DoctorDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var doctors = JsonSerializer.Deserialize<List<DoctorDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(doctors);
        }
    }
}
