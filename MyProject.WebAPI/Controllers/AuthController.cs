using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyProject.Core.Entities;
using MyProject.WebAPI.DTO;

namespace MyProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        // ... diğer gerekli servisler

        public AuthController(
            UserManager<User> userManager,
            SignInManager<User> signInManager /*, ... */)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            // ...
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Gönderilen veri boş." });

            // 1) E-posta ile user’ı bul
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized(new { message = "E-posta veya şifre yanlış." });

            // 2) Şifre kontrolü
            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return Unauthorized(new { message = "E-posta veya şifre yanlış." });

            // 3) Token oluştur
            var token = GenerateJwtToken(user);  // Kendi JWT oluşturma metodun

            // 4) Başarıyla token döndür
            return Ok(new LoginResultDto { Token = token });
        }

        // Örnek JWT üretme metodu (sadeleştirildi)
        private string GenerateJwtToken(User user)
        {
            // burada JwtSecurityTokenHandler ile Claim’leri ayarla,
            // SigningCredentials kullanarak token’ı üret ve return et
            throw new NotImplementedException();
        }
    }
}

