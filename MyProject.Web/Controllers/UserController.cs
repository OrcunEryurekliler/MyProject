using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyProject.Core.Models;
using MyProject.Services.Implementations;
using MyProject.Services.Interfaces;
using MyProject.Web.ViewModels;

namespace MyProject.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // Kullanıcı eklemek için formu gösteriyoruz
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Age = model.Age,
                    Cellphone = model.Cellphone,
                    Password = model.Password,
                    TCKN = model.TCKN,
                    RoleId = 1
                };

                try
                {
                    await _userService.CreateAsync(user);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException?.Message); // detay burada
                    throw; // logladıktan sonra tekrar fırlatabilirsin
                }


            }

            return View(model);
        }


        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserEditViewModel userEditViewModel = new UserEditViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Age = user.Age,
                Cellphone = user.Cellphone,
                Password = user.Password,
                TCKN = user.TCKN,
                RoleId = 1
            };

            return View(userEditViewModel); // Güncelleme formunu doldurmak için kullanıcıyı View'a gönderiyoruz
        }

        // POST: User/Edit/5


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserEditViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
                return Challenge();  // veya RedirectToAction("Login", "Account");

            // 1. URL’den gelen id ile formdaki gizli model.Id eşleşiyor mu?
            if (id != model.Id)
                return BadRequest();

            // 2. Oturum açmış kullanıcının ID'sini claim'ten al
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out var loggedInUserId))
                return BadRequest("Geçersiz kullanıcı kimliği.");

            // 3. Sadece kendi verisini düzenleyebilsin
            if (loggedInUserId != model.Id)
                return Forbid();

            // 4. Form doğrulama
            if (!ModelState.IsValid)
                return View(model);

            // 5. Güncelleme için domain modeli oluştur
            var user = new User
            {
                Id = model.Id,
                Name = model.Name,
                Email = model.Email,
                Age = model.Age,
                Cellphone = model.Cellphone,
                Password = model.Password,
                TCKN = model.TCKN,
                RoleId = 1
            };

            // 6. Servisi çağır
            await _userService.UpdateAsync(user);
            return RedirectToAction("Index");
        }


        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user); // Silme onayı için kullanıcıyı View'a gönderiyoruz
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _userService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
