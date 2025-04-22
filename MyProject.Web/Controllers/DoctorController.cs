using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyProject.Web.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
