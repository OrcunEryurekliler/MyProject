using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Core.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationService _service;
        public SpecializationController(ISpecializationService service)
        {
            _service = service;
        }
        // GET: api/<SpecializationsController>
        [HttpGet("all")]
        public async Task<IEnumerable<Specialization>> GetAll()
        {
            var allSpecializations = await _service.GetAllAsync();
            return allSpecializations;
        }

        
    }
}
