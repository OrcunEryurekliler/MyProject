using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Core.Entities;
using MyProject.Web.ViewModels.AppointmentViewModels;
using System.Security.Claims;

namespace MyProject.Web.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        private readonly UserManager<User> _userManager;

        public AppointmentController(
            IAppointmentService appointmentService,
            UserManager<User> userManager)
        {
            _appointmentService = appointmentService;
            _userManager = userManager;
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

            // 3) Entity → ViewModel map
            var vms = entities.Select(a => new AppointmentViewModel
            {
                Id = a.Id,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Status = a.Status,
                DoctorProfileId = a.DoctorProfileId,
                PatientProfileId = a.PatientProfileId,
                DoctorFullName = a.DoctorProfile?.User.Name ?? "—",
                PatientFullName = a.PatientProfile?.User.Name ?? "—"
            });

            return View(vms);
        }

        // GET: /Appointment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var a = await _appointmentService.GetAsync(id);
            if (a == null) return NotFound();

            var vm = new AppointmentViewModel
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

        // GET: /Appointment/Create
        [HttpGet]
        public IActionResult Create()
        {
            // Eğer dropdown’lar lazımsa ek veri aktarabilirsin (ör. TempData, ViewBag)
            return View(new AppointmentViewModel
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddMinutes(30),
                Status = "Beklemede"
            });
        }

        // POST: /Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // ViewModel → Entity map
            var entity = new Appointment
            {
                StartTime = vm.StartTime,
                EndTime = vm.EndTime,
                Status = vm.Status,
                DoctorProfileId = vm.DoctorProfileId,
                PatientProfileId = vm.PatientProfileId
            };

            await _appointmentService.AddAsync(entity);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Appointment/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var a = await _appointmentService.GetAsync(id);
            if (a == null) return NotFound();

            var vm = new AppointmentViewModel
            {
                Id = a.Id,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Status = a.Status,
                DoctorProfileId = a.DoctorProfileId,
                PatientProfileId = a.PatientProfileId,
                DoctorFullName = a.DoctorProfile?.User.Name,
                PatientFullName = a.PatientProfile?.User.Name
            };
            return View(vm);
        }

        // POST: /Appointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppointmentViewModel vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            var entity = new Appointment
            {
                Id = vm.Id,
                StartTime = vm.StartTime,
                EndTime = vm.EndTime,
                Status = vm.Status,
                DoctorProfileId = vm.DoctorProfileId,
                PatientProfileId = vm.PatientProfileId
            };

            await _appointmentService.UpdateAsync(entity);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Appointment/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var a = await _appointmentService.GetAsync(id);
            if (a == null) return NotFound();

            var vm = new AppointmentViewModel
            {
                Id = a.Id,
                StartTime = a.StartTime,
                EndTime = a.EndTime,
                Status = a.Status,
                DoctorFullName = a.DoctorProfile?.User.Name,
                PatientFullName = a.PatientProfile?.User.Name
            };
            return View(vm);
        }

        // POST: /Appointment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _appointmentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
