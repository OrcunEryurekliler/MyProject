using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTOs;
using MyProject.Application.Interfaces;
using MyProject.Core.Entities;
using MyProject.Core.Enums;
using MyProject.WebAPI.DTO;

namespace MyProject.WebAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsByDoctor(int doctorId, [FromQuery] DateTime date)
        {
            if (date == DateTime.MinValue)
            {
                return BadRequest(new { message = "Tarih geçerli değil." });
            }

            var appointments = await _appointmentService.GetAllByDoctorAndDateAsync(doctorId, date);

            if (!appointments.Any())
            {
                return NotFound(new { message = "Bu tarihte doktorun randevusu bulunamadı." });
            }

            return Ok(appointments);
        }

        // GET: api/appointment/available-doctors?specialization=Cardiology&date=2025-04-26
        [HttpGet("available-doctors")]
        public async Task<IActionResult> GetAvailableDoctors([FromQuery] Specialization specialization, [FromQuery] DateTime date)
        {
            try
            {
                var doctors = await _appointmentService.GetAvailableDoctorsAsync(specialization, date);
                var doctorDtos = doctors.Select(d => new AvailableDoctorDto
                {
                    Id = d.Id,
                    FullName = d.User.Name, // Mesela User üzerinden geliyor
                    SpecializationName = d.Specialization.ToString()
                }).ToList();
                return Ok(doctorDtos);
            }
            catch (Exception ex)
            {
                // Hata mesajını logla
                Console.WriteLine($"Error occurred while fetching doctors: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        // GET: api/appointment/available-timeslots?doctorId=5&date=2025-04-26
        [HttpGet("available-timeslots")]
        public async Task<IActionResult> GetAvailableTimeslots([FromQuery] int doctorId, [FromQuery] DateTime date)
        {
            var timeslots = await _appointmentService.GetAvailableTimeslotsAsync(doctorId, date);
            return Ok(timeslots);
        }


        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var patientId = await _appointmentService.GetPatientIdByUserId(userId);
            var appointment = new Appointment
            {
                DoctorProfileId = dto.DoctorProfileId,
                PatientProfileId = patientId,
                StartTime = dto.StartTime,
                EndTime = dto.StartTime.AddMinutes(dto.DurationMinutes), // <-- BURADA
                Status = dto.Status ?? "Pending"
            };

            var result = await _appointmentService.AddAsync(appointment);
            if (result)
                return Ok(new { message = "Randevu başarıyla oluşturuldu." });

            return BadRequest(new { message = "Randevu oluşturulamadı." });
        }

    }
}
