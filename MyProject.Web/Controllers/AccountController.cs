using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyProject.Web.ViewModels.AccountViewModels;
using MyProject.Web.DTO;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MyProject.Web.Controllers
{
    public class AccountController : Controller
    {
        // Cookie entegrasyonu için
        private readonly HttpClient _httpClient;

        public AccountController(IHttpClientFactory clientFactory                                 )
        {
            _httpClient = clientFactory.CreateClient("ApiClient");
            
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

            
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", model); // API URL'ni güncelle

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Geçersiz giriş.");
                return View(model);
            }

            var loginResult = await response.Content.ReadFromJsonAsync<LoginResultDto>();
            if (loginResult == null || string.IsNullOrEmpty(loginResult.Token))
            {
                ModelState.AddModelError(string.Empty, "Sunucudan token alınamadı.");
                return View(model);
            }

            // 1. Token'ı decode et
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(loginResult.Token);
            
            var claims = token.Claims.ToList();

            // Cookie kimliği oluştur
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };



            // 2. Cookie yaz
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
            
            return RedirectToAction("Index", "Appointment");
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
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

    }
}
