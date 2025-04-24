using System.ComponentModel.DataAnnotations;

namespace MyProject.Web.ViewModels.AppointmentDTO
{
    public class CreateAppointmentDto
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        [Required]
        public int DoctorProfileId { get; set; }
    }
}
