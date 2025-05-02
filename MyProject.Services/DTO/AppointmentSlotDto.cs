namespace MyProject.Application.DTO
{
    public class AppointmentSlotDto
    {
        public int Id { get; set; } // İstersen zamanı string olarak doğrudan verebilirsin

        public TimeOnly SlotTime { get; set; } // Örn: "09:00", "09:30"

        public bool IsAvailable { get; set; } // View'da doluysa kırmızı yapabilirsin
    }

}
