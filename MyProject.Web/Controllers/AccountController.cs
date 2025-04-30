using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.Interfaces;
using MyProject.Core.Entities;
using MyProject.Web.ViewModels.AccountViewModels;
using MyProject.Web.DTO;
using System.Net;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyProject.Web.Controllers
{

    public class AccountController : Controller
    {
        //Cookie entegrasyonu için
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IPatientProfileService _patientService;

        public AccountController(IHttpClientFactory clientFactory,
                                 UserManager<User> userManager,
                                 SignInManager<User> signInManager,
                                 IPatientProfileService patientService)
        {
            _httpClient = clientFactory.CreateClient("AuthApi");
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

            var response = await client.PostAsJsonAsync("https://localhost:7271/api/auth/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    ModelState.AddModelError("", "E-posta veya şifre yanlış.");
                else
                    ModelState.AddModelError("", "Sunucu hatası oluştu. Lütfen tekrar deneyin.");

                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResultDto>();
            if (result is null || string.IsNullOrEmpty(result.Token))
            {
                ModelState.AddModelError("", "Sunucudan geçerli token alınamadı.");
                return View(model);
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(result.Token);

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var nameIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            if (roleClaim == null || nameIdClaim == null)
            {
                ModelState.AddModelError("", "Kullanıcı bilgileri eksik.");
                return View(model);
            }

            // JWT'yi Cookie olarak kaydet (isteğe bağlı, API çağırmak için kullanabilirsin)
            Response.Cookies.Append("AccessToken", result.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            });

            // ASP.NET'e kullanıcı girişini bildir
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, nameIdClaim.Value),
        new Claim(ClaimTypes.Name, nameClaim?.Value ?? model.Email),
        new Claim(ClaimTypes.Email, emailClaim?.Value ?? model.Email),
        new Claim(ClaimTypes.Role, roleClaim.Value),
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Rol bazlı yönlendirme
            if (roleClaim.Value == "Doctor")
                return RedirectToAction("Dashboard", "Doctor");
            else if (roleClaim.Value == "Patient")
                return RedirectToAction("Index", "Appointment");
            else
                return RedirectToAction("Index", "Home");
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult PatientRegister()
        {
            // Eğer kullanıcı zaten giriş yaptıysa randevu sayfasına yönlendir
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToAction("Index", "Appointment");

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PatientRegister(RegisterPatientViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var registerDto = new
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                TCKN = model.TCKN,
                Cellphone = model.Cellphone,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender,
                MaritalStatus = model.MaritalStatus,
                BloodType = model.BloodType,
                profileType = model.profileType
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7271/api/auth/register", registerDto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError("", $"Kayıt başarısız: {errorMessage}");
                    return View(model);
                }

                var result = await response.Content.ReadFromJsonAsync<LoginResultDto>();
                if (result is null || string.IsNullOrEmpty(result.Token))
                {
                    ModelState.AddModelError("", "Sunucudan geçerli token alınamadı.");
                    return View(model);
                }

                // Token'ı al ve cookie olarak sakla
                Response.Cookies.Append("AccessToken", result.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddHours(1)
                });

                // Token'dan rolü al
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(result.Token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

                if (roleClaim?.Value == "Patient")
                    return RedirectToAction("Index", "Appointment");

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Hata oluştu: {ex.Message}");
                return View(model);
            }
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