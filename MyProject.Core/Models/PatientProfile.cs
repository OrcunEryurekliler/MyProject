using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Core.Models
{
    public class PatientProfile
    {
        [Key]
        public int UserId { get; set; }

        public User User { get; set; }

        // Hasta'ya özel alanlar
        public string BloodType { get; set; }
    }
}
