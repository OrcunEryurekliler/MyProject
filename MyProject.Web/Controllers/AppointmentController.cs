using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Web.ViewModels.AppointmentViewModels;
using MyProject.WebUI.ViewModels;
using System.Security.Claims;

namespace MyProject.Web.Controllers
{
    //[Authorize]
    public class AppointmentController : Controller
    {

        private readonly HttpClient _httpClient;

        public AppointmentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ApiClient");
        }


        // GET: /Appointment
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var response = await _httpClient.GetAsync($"api/appointments/user/{userId}");

            if (!response.IsSuccessStatusCode)
                return View(new List<AppointmentViewModel>());

            var appointments = await response.Content.ReadFromJsonAsync<List<AppointmentViewModel>>();

            return View(appointments);
        }


        // GET: /Appointment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return View();
        }

        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> Book()
        {
            return View();
        }

        // POST: Form gönderildiğinde API'ye yönlendir

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Booking(AppointmentBookingViewModel vm)
        {
            var doctors = _httpClient.GetAsync
            return View();
        }

    }
}
