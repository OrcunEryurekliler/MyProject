using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MyProject.Core.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "T.C. Kimlik Numarası zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "T.C. Kimlik Numarası 11 haneli olmalıdır")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "T.C. Kimlik Numarası sadece rakamlardan oluşmalı ve 11 hane olmalı")]
        public required string TCKN { get; set; }

        [Required(ErrorMessage = "İsim boş bırakılamaz")]
        public required string Name { get; set; }

        [Range(0, 100, ErrorMessage = "Yaş 0 ile 100 arasında olmalıdır")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email adresi boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public required string Email { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;

        [Required(ErrorMessage ="Cep telefonu zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Cep telefonu 11 haneli olmalıdır")]
        [RegularExpression(@"^05\d{9}$", ErrorMessage = "Cep telefonu 05 ile başlamalı ve 11 hane olmalı")]
        public required string Cellphone { get; set; }
            
        public required string Password { get; set; }

    }
}
