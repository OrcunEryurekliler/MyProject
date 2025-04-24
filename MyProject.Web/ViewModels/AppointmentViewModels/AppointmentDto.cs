using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.AppointmentViewModels
{
    public class AppointmentDto
    {
        // — Temel Kimlik —
        public int Id { get; set; }

        // — Zamanlama —
        [Display(Name = "Başlangıç Tarihi/Saati")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Bitiş Tarihi/Saati")]
        public DateTime EndTime { get; set; }

        // — Durum —
        [Display(Name = "Durum")]
        public string Status { get; set; } = string.Empty;

        // — İlişkili Profiller —
        [Display(Name = "Doktor")]
        public int DoctorProfileId { get; set; }
        public string DoctorFullName { get; set; } = string.Empty;

        [Display(Name = "Hasta")]
        public int PatientProfileId { get; set; }
        public string PatientFullName { get; set; } = string.Empty;
    }
}
