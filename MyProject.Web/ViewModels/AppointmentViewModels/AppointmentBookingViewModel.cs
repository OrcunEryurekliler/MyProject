using System.ComponentModel.DataAnnotations;

namespace MyProject.WebUI.ViewModels
{
    public class AppointmentBookingViewModel
    {
        [Required]
        public IEnumerable<string> Specializations { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int DoctorProfileId { get; set; }

        [Required]
        public TimeSpan TimeSlot { get; set; }

        public int DurationMinutes { get; set; } = 30;
    }
}
