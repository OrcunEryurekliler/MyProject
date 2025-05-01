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
    public class SpecializationsController : ControllerBase
    {
        private readonly ISpecializationService _service;
        public SpecializationsController(ISpecializationService service)
        {
            _service = service;
        }
        // GET: api/<SpecializationsController>
        [HttpGet]
        public async Task<IEnumerable<Specialization>> GetAllSpecializations()
        {
            var allSpecializations = await _service.GetAllAsync();
            return allSpecializations;
        }

        
    }
}
