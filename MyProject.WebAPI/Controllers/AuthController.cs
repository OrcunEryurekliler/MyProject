using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyProject.Core.Entities;
using MyProject.WebAPI.DTO;

namespace MyProject.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        // ... diğer gerekli servisler

        public AuthController(
            IConfiguration config,
            UserManager<User> userManager,
            SignInManager<User> signInManager /*, ... */)
        {
            _config = config;
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
            var user = await _userManager.FindByEmailAsync(dto.Email) ?? await _userManager.FindByNameAsync(dto.Email); 
            if (user == null)
                return Unauthorized(new { message = "E-posta veya şifre yanlış." });
            
            // 2) Şifre kontrolü
            var passwordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return Unauthorized(new { message = "E-posta veya şifre yanlış." });
            var roles = await _userManager.GetRolesAsync(user);
            // 3) Token oluştur
            var token = GenerateJwtToken(user, roles);  // Kendi JWT oluşturma metodun

            // 4) Başarıyla token döndür
            return Ok(new LoginResultDto { Token = token });
        }

        // Örnek JWT üretme metodu (sadeleştirildi)
        private string GenerateJwtToken(User user, IList<string> roles)
        {
            // 1) Claim’leri oluştur
            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // 2) Security key ve SigningCredentials
            var key = new SymmetricSecurityKey(
                              Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3) Token parametreleri
            var expires = DateTime.UtcNow
                              .AddMinutes(
                                double.Parse(_config["Jwt:ExpiresInMinutes"] ?? "60"));
            var token = new JwtSecurityToken(
                              issuer: _config["Jwt:Issuer"],
                              audience: _config["Jwt:Audience"],
                              claims: claims,
                              expires: expires,
                              signingCredentials: creds);

            // 4) Token’ı string hâline getir
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

