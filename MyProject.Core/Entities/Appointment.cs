namespace MyProject.Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        // Foreign key: Hangi hasta aldı
        public int PatientProfileId { get; set; }
        public PatientProfile PatientProfile { get; set; }

        // Foreign key: Hangi doktordan aldı
        public int DoctorProfileId { get; set; }
        public DoctorProfile DoctorProfile { get; set; }

        // Randevu zamanları
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        // İsteğe bağlı: Durum (Onaylandı, Beklemede, İptal)
        public string Status { get; set; }
    }

}
