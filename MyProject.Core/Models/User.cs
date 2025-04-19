using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Core.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim boş bırakılamaz")]
        public required string Name { get; set; }

        [Range(18, 100, ErrorMessage = "Yaş 18 ile 100 arasında olmalıdır")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Email adresi boş bırakılamaz")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        public required string Email { get; set; } 

    }
}
