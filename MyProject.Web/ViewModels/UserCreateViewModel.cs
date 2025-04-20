using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels
{
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "İsim boş bırakılamaz")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Yaş 0 ile 100 arasında olmalıdır")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email adresi boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cep telefonu zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Cep telefonu 11 haneli olmalıdır")]
        [RegularExpression(@"^05\d{9}$", ErrorMessage = "Cep telefonu 05 ile başlamalı ve 11 hane olmalı")]
        public string Cellphone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(16, MinimumLength = 8, ErrorMessage = "Şifre 8-16 karakter arasında olmalıdır")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "T.C. Kimlik Numarası zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "T.C. Kimlik Numarası 11 haneli olmalıdır")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "T.C. Kimlik Numarası sadece rakamlardan oluşmalı ve 11 hane olmalı")]
        public required string TCKN { get; set; }
        public int RoleId { get; set; } = 1; // Default olarak 1 (örneğin "Hasta")
    }
}
