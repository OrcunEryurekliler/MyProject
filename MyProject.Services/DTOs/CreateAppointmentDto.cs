    using System.ComponentModel.DataAnnotations;

    namespace MyProject.Application.DTOs
    {
        public class CreateAppointmentDto
        {
            [Required]
            public DateTime StartTime { get; set; }
            [Required]
            public DateTime EndTime { get; set; }
            [Required]
            public int DoctorProfileId { get; set; }
            [Required]
            public int PatientProfileId { get; set; }
            public int DurationMinutes { get; set; } = 30;
            public string? Status { get; set; }

        }
    }
