using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Core.Models
{
    public class DoctorProfile
    {
        [Key]
        public int UserId { get; set; }

        public User User { get; set; }

        public string Specialty { get; set; }
        public string DiplomaNumber { get; set; }

        // Doktor'a özel ilişkiler (örn. Randevular)
    }

}
