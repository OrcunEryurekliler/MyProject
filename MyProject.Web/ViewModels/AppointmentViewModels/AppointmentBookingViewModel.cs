using System;
using System.ComponentModel.DataAnnotations;
using MyProject.Core.Enums;

namespace MyProject.WebUI.ViewModels
{
    public class AppointmentBookingViewModel
    {
        [Required]
        public Specialization? Specialization { get; set; }

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
