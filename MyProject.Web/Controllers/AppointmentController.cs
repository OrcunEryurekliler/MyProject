using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyProject.Web.DTO;
using MyProject.Web.ViewModels.AppointmentViewModels;
using MyProject.Web.ViewModels;
using System.Collections.Generic;
using System.Security.Claims;

namespace MyProject.Web.Controllers
{
    [Authorize]
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
        [HttpGet]
        public async Task<IActionResult> Book()
        {
            var specializations = await _httpClient.GetFromJsonAsync<IEnumerable<SpecializationDto>>("api/specialization/all");

            var vm = new AppointmentBookingViewModel();
            // ViewModel'e ekle
            vm.Specializations = specializations.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            // İlk başta SelectedSpecializationId boş olacak
            vm.SelectedSpecializationId = 0;
            vm.SelectedDate = DateTime.Now;

            return View(vm); // Book view'ını döndür
        }

        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> Book(AppointmentBookingViewModel vm)
        {
            if (vm.SelectedSpecializationId == 0 || vm.SelectedDate == null)
            {
                // Hatalı giriş kontrolü
                ModelState.AddModelError("", "Lütfen geçerli bir uzmanlık ve tarih seçin.");
                return View(vm); // Hata durumunda view'ı tekrar göster
            }

            // Seçilen uzmanlık ve tarihe göre doktorları getir
            var response = await _httpClient.GetAsync($"api/appointment/available-doctors?specializationId={vm.SelectedSpecializationId}&date={vm.SelectedDate:yyyy-MM-dd}");

            if (response.IsSuccessStatusCode)
            {
                vm.AvailableDoctors = await response.Content.ReadFromJsonAsync<IEnumerable<DoctorDto>>();
            }
            else
            {
                ModelState.AddModelError("", "Doktorlar getirilemedi.");
            }

            // Seçilen uzmanlıkları yeniden çek
            var specializations = await _httpClient.GetFromJsonAsync<IEnumerable<SpecializationDto>>("api/specialization/all");
            vm.Specializations = specializations.Select(s => new SelectListItem
            {
                Value = s.Id.ToString(),
                Text = s.Name
            }).ToList();

            return View(vm); // Doktorları ve hata mesajlarını gösteren view'ı döndür
        }

        // slotları ajax ile getirmek istersen:
        [Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> Slots(int doctorId, DateTime date)
        {
            
            var slots = await _httpClient
                .GetFromJsonAsync<IEnumerable<AppointmentSlotDto>>(
                    $"api/appointment/slots?doctorId={doctorId}&date={date:yyyy-MM-dd}");
            if(slots == null)
            {
                return Json(slots); 
            }
            return Json(slots);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> Create()
    }
}

