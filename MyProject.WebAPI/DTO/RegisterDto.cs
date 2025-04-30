using MyProject.Core.Enums;
using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required(ErrorMessage = "T.C. Kimlik Numarası zorunludur")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "T.C. Kimlik Numarası 11 haneli olmalıdır")]
    public string TCKN { get; set; }

    [Required(ErrorMessage = "İsim boş bırakılamaz")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email adresi boş bırakılamaz")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Cep telefonu zorunludur")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Cep telefonu 11 haneli olmalıdır")]
    public string Cellphone { get; set; }

    [Required(ErrorMessage = "Doğum tarihi gereklidir.")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Cinsiyet gereklidir.")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Medeni hal gereklidir.")]
    public MaritalStatus MaritalStatus { get; set; }

    [Required(ErrorMessage = "Şifre zorunludur.")]
    [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Profil tipi zorunludur.")]
    public string ProfileType { get; set; } // "Patient" veya "Doctor"
    
}
