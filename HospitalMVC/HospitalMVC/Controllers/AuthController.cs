using HospitalMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace HospitalFrontendMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _config["ApiSettings:BaseUrl"];
            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/api/Auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Invalid login credentials.";
                return View(dto);
            }

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<LoginResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (result == null || string.IsNullOrEmpty(result.UserId))
            {
                ViewBag.Error = "Invalid response from API.";
                return View(dto);
            }

            HttpContext.Session.SetString("userId", result.UserId);
            HttpContext.Session.SetString("email", dto.Email);
            HttpContext.Session.SetString("role", result.Role);

            if (result.Role == "Doctor")
            {
                HttpContext.Session.SetString("doctorId", result.UserId); 
                return RedirectToAction("MyAppointments", "Doctor");
            }

            if (result.Role == "Patient")
            {
                return RedirectToAction("BookAppointment", "Patient");
            }

            ViewBag.Error = "Unknown role.";
            return View(dto);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _config["ApiSettings:BaseUrl"];

            var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{baseUrl}/api/Auth/register", content);

            if (response.IsSuccessStatusCode)
            {
                TempData["message"] = "Registration successful. Please login.";
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Registration failed.";
            return View(dto);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Welcome()
        {
            ViewBag.Message = TempData["message"];
            return View();
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }

    }
}


