namespace MyProject.Web.DTO
{
    public class AppointmentBookingDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int AppointmentSlotId { get; set; } // API, bu ID'yi kullanarak saat bilgisini alır
        public DateTime Date { get; set; }
    }
}
