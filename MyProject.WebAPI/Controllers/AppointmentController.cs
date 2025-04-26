using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTOs;
using MyProject.Application.Interfaces;
using MyProject.Core.Entities;

namespace MyProject.WebAPI.Controllers
{
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


        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var appointment = new Appointment
            {
                DoctorProfileId = dto.DoctorProfileId,
                PatientProfileId = 1, // Örnek: Login olan kullanıcıdan alınmalı ileride
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = dto.Status ?? "Pending"
            };

            var result = await _appointmentService.AddAsync(appointment);
            if (result)
                return Ok(new { message = "Randevu başarıyla oluşturuldu." });

            return BadRequest(new { message = "Randevu oluşturulamadı." });
        }
    }
}
