using System;

namespace MyProject.Core.Entities
{
    public class DoctorUnavailability
    {
        public int Id { get; set; }

        public int DoctorProfileId { get; set; }
        public DoctorProfile DoctorProfile { get; set; }

        public DateTime OffWorkDate { get; set; } // Doktorun çalışamayacağı gün
        public string? Reason { get; set; } // Opsiyonel açıklama (yıllık izin, hastalık vs.)
    }
}
