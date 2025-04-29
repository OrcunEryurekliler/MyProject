using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Core.Entities;
using MyProject.Web.ViewModels.AccountViewModels;
using MyProject.Web.DTO;
using System.Net;
using Azure;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace MyProject.Web.Controllers
{

    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IPatientProfileService _patientService;

        public AccountController(IHttpClientFactory clientFactory,
                                 IConfiguration config,
                                 UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IPatientProfileService patientService)
        {
            _httpClient = clientFactory.CreateClient("AuthApi");
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _patientService = patientService;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            // Kullanıcının zaten giriş yapıp yapmadığını kontrol et
            if (User.Identity?.IsAuthenticated == true)
            {
                // Kullanıcı zaten giriş yapmışsa, randevu sayfasına yönlendir
                return RedirectToAction("Index", "Appointment");
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginDto = new
            {
                Email = model.Email,
                Password = model.Password
            };

            using var client = new HttpClient();
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7271/api/auth/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    ModelState.AddModelError("", "E-posta veya şifre yanlış.");
                }
                else
                {
                    ModelState.AddModelError("", "Sunucu hatası oluştu. Lütfen tekrar deneyin.");
                }
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResultDto>();
            if (result is null || string.IsNullOrEmpty(result.Token))
            {
                ModelState.AddModelError("", "Sunucudan geçerli token alınamadı.");
                return View(model);
            }

            // Token'dan role'i oku
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(result.Token);
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (roleClaim == null)
            {
                ModelState.AddModelError("", "Rol bulunamadı.");
                return View(model);
            }
            // TOKEN'I SESSION'A YAZ
            HttpContext.Session.SetString("Token", result.Token);
            // KULLANICIYI LOGIN YAP
            var claims = new List<Claim>
            {
             new Claim(ClaimTypes.Name, model.Email),
             new Claim(ClaimTypes.Role, roleClaim.Value),
             new Claim("AccessToken", result.Token)
            };

            if (roleClaim.Value == "Doctor")
                return RedirectToAction("Dashboard", "Doctor");
            else if (roleClaim.Value == "Patient")
                return RedirectToAction("Index", "Appointment");
            else
                return RedirectToAction("Index", "Home");

        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}