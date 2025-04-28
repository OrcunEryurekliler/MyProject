using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Enums;

namespace MyProject.Core.Entities
{
    public class PatientProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        // Hasta'ya özel alanlar
        public BloodType BloodType { get; set; } = BloodType.OPozitif;
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
