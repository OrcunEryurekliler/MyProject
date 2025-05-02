using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Application.Services.Specific;
using MyProject.Core.Entities;
using MyProject.WebAPI.DTO;

namespace MyProject.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorProfileService _doctorService;
        private readonly IAppointmentService _appointmentService;
        public DoctorController(IDoctorProfileService doctorProfileService,
                                IAppointmentService appointmentService)
        {
            _doctorService = doctorProfileService;
            _appointmentService = appointmentService;
        }
    }
}
