using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.WebAPI.DTO;
using MyProject.Application.DTO;
using MyProject.Application.Interfaces;
using MyProject.Application.Helpers;

namespace MyProject.WebAPI.Controllers
{
    //[Authorize]
    [Route("api/appointment")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        // GET: api/appointment/available-doctors?specialization=Cardiology&date=2025-04-26
        [AllowAnonymous]
        [HttpGet("available-doctors")]
        public async Task<IActionResult> GetAvailableDoctors([FromQuery] int specializationId, [FromQuery] DateTime date)
        {
            try
            {
                var doctors = await _appointmentService.GetAvailableDoctorsAsync(specializationId, date);
                var doctorDtos = doctors.Select(d => new DoctorDto
                {
                    Id = d.Id,
                    FullName = d.FullName,
                    SpecializationName = d.SpecializationName.ToString()
                }).ToList();
                return Ok(doctorDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while fetching doctors: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }



        // GET /api/appointment/slots?doctorId=5&date=2025-05-02
        [HttpGet("slots")]
        //[Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAppointmentSlots([FromQuery] int doctorId,[FromQuery] DateTime date)
        {
            var slots = await _appointmentService.GetAvailableTimeslotsAsync(doctorId, date);
            return Ok(slots);
        }


    }
}
