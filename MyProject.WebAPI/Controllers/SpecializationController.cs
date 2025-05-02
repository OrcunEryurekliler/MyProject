using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Application.Services.Specific;
using MyProject.Core.Entities;
using MyProject.WebAPI.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;
        public SpecializationController(ISpecializationService service)
        {
            _specializationService = service;
        }
        // GET: api/<SpecializationsController>
        
        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var list = await _specializationService.GetAllAsync();
            var dto = list.Select(s => new SpecializationDto
            {
                Id = s.Id,
                Name = s.Name
            });
            return Ok(dto);
        }


    }
}
