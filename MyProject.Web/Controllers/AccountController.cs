using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Core.Entities;
using MyProject.Web.ViewModels.AccountViewModels;
using MyProject.Web.DTO;
using System.Net;
using Azure;

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
            if (User.Identity?.IsAuthenticated == true)
            {
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

            // API'ye login için DTO oluştur
            var loginDto = new
            {
                Email = model.Email,
                Password = model.Password
            };

            using var client = new HttpClient();
            // BURAYA API LOGIN URL'İNİ YAZ
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);


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

            // TOKEN'I SESSION'A YAZ
            HttpContext.Session.SetString("Token", result.Token);

            return RedirectToAction("Index", "Appointment");
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