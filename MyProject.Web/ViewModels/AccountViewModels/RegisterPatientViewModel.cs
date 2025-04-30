using System;
using System.ComponentModel.DataAnnotations;
using MyProject.Core.Enums;

public class RegisterPatientViewModel
{
    [Required(ErrorMessage = "İsim zorunludur")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email adresi zorunludur")]
    [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Şifre zorunludur")]
    [StringLength(100, ErrorMessage = "Şifre en az 6 karakter uzunluğunda olmalıdır", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "T.C. Kimlik Numarası zorunludur")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "T.C. Kimlik Numarası 11 haneli olmalıdır")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "T.C. Kimlik Numarası sadece rakamlardan oluşmalı ve 11 hane olmalı")]
    public string TCKN { get; set; }

    [Required(ErrorMessage = "Cep telefonu zorunludur")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "Cep telefonu 11 haneli olmalıdır")]
    [RegularExpression(@"^05\d{9}$", ErrorMessage = "Cep telefonu 05 ile başlamalı ve 11 hane olmalı")]
    public string Cellphone { get; set; }

    [Required(ErrorMessage = "Doğum tarihi zorunludur")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Cinsiyet zorunludur")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Medeni hal zorunludur")]
    public MaritalStatus MaritalStatus { get; set; }

    [Required(ErrorMessage = "Kan grubu zorunludur")]
    public BloodType BloodType { get; set; }
    public string profileType { get; set; } = "Patient";
}
