using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MyProject.Core.Models
{
    public enum Gender
    {
        Erkek,
        Kadın,
        Diğer
    }

    public enum MaritalStatus
    {
        Bekar,
        Evli,
        Boşanmış,
        Dul
    }

    public class User : IdentityUser<int>
    {
        [Required(ErrorMessage = "T.C. Kimlik Numarası zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "T.C. Kimlik Numarası 11 haneli olmalıdır")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "T.C. Kimlik Numarası sadece rakamlardan oluşmalı ve 11 hane olmalı")]
        public string TCKN { get; set; }

        [Required(ErrorMessage = "İsim boş bırakılamaz")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email adresi boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Cep telefonu zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Cep telefonu 11 haneli olmalıdır")]
        [RegularExpression(@"^05\d{9}$", ErrorMessage = "Cep telefonu 05 ile başlamalı ve 11 hane olmalı")]
        public string Cellphone { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [Required(ErrorMessage = "Doğum tarihi gereklidir.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Cinsiyet gereklidir.")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Medeni hal gereklidir.")]
        public MaritalStatus MaritalStatus { get; set; }

        // Yaş, doğum tarihinden hesaplanabilir
        public int Age => DateTime.Today.Year - DateOfBirth.Year -
                          (DateTime.Today.DayOfYear < DateOfBirth.DayOfYear ? 1 : 0);
        public DoctorProfile DoctorProfile { get; set; }
        public PatientProfile PatientProfile { get; set; }


    }
}
