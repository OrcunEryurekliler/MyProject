using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTOs;
using MyProject.Application.Interfaces;
using MyProject.Core.Entities;
using MyProject.Core.Enums;
using MyProject.Web.ViewModels.AppointmentViewModels;
using MyProject.WebUI.ViewModels;
using System.Security.Claims;

namespace MyProject.Web.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IDoctorProfileService _doctorProfileService;
        private readonly IDoctorUnavailabilityService _doctorUnavailabilityService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IAppointmentApiClient _api;

        public AppointmentController(            
            IAppointmentService appointmentService,
            IDoctorProfileService profileService,
            IDoctorUnavailabilityService doctorUnavailabilityService,
            UserManager<User> userManager,
            IMapper mapper,
        IAppointmentApiClient api)
        {
            _appointmentService = appointmentService;
            _doctorProfileService = profileService;
            _userManager = userManager;
            _doctorUnavailabilityService = doctorUnavailabilityService;
            _api = api;
            _mapper = mapper;
        }

        // GET: /Appointment
        public async Task<IActionResult> Index()
        {
            // 1) Oturum açmış kullanıcıyı ve rollerini al
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();
            var roles = await _userManager.GetRolesAsync(user);
            var userId = int.Parse(_userManager.GetUserId(User));

            // 2) Doğru servis çağrısı
            IEnumerable<Appointment> entities;
            if (roles.Contains("Patient"))
                entities = await _appointmentService.GetAllAsyncByPatient(userId);
            else if (roles.Contains("Doctor"))
                entities = await _appointmentService.GetAllAsyncByDoctor(userId);
            else
                entities = await _appointmentService.GetAllAsync();
            var appointmentViewModels = _mapper.Map<IEnumerable<AppointmentViewModel>>(entities);
            // 3) Entity → ViewModel map
            

            return View(appointmentViewModels);
        }

        // GET: /Appointment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var a = await _appointmentService.GetAsync(id);
            if (a == null) return NotFound();

            var vm = new AppointmentDto
            {
                Id = a.Id,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Status = a.Status,
                DoctorProfileId = a.DoctorProfileId,
                PatientProfileId = a.PatientProfileId,
                DoctorFullName = a.DoctorProfile?.User.Name ?? "—",
                PatientFullName = a.PatientProfile?.User.Name ?? "—"
            };
            return View(vm);
        }

        
        public IActionResult Book()
        {
            ViewBag.Specializations = Enum.GetValues(typeof(Specialization))
                                          .Cast<Specialization>();
            return View(new AppointmentBookingViewModel
            {
                Date = DateTime.Today
            });
        }

        // POST: Form gönderildiğinde API'ye yönlendir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(AppointmentBookingViewModel vm)
        {
            var user = await _userManager.GetUserAsync(User);
            var patientProfileId = user.PatientProfile?.Id ?? 0; // Güvenli okuma

            if (!ModelState.IsValid)
            {
                ViewBag.Specializations = Enum.GetValues(typeof(Specialization))
                                              .Cast<Specialization>();
                return View(vm);
            }

            var dto = new CreateAppointmentDto
            {
                DoctorProfileId = vm.DoctorProfileId,
                StartTime = vm.Date.Date + vm.TimeSlot,
                DurationMinutes = vm.DurationMinutes,
                PatientProfileId = patientProfileId
                
            };

            await _api.CreateAsync(dto);
            return RedirectToAction(nameof(Book)); // veya Index
        }

    }
}
