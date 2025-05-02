using System.ComponentModel.DataAnnotations;
using MyProject.Web.DTO;

namespace MyProject.WebUI.ViewModels
{
    public class AppointmentBookingViewModel
    {
        public int SelectedSpecializationId { get; set; }
        [Required]
        public IEnumerable<SpecializationDto> Specializations { get; set; }
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
