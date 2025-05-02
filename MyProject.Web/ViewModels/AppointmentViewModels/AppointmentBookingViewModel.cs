using Microsoft.AspNetCore.Mvc.Rendering;
using MyProject.Web.DTO;
using System.ComponentModel.DataAnnotations;

public class AppointmentBookingViewModel
{
    // Uzmanlık (specialization) listesi için
    public List<SelectListItem> Specializations { get; set; } = new();

    // Kullanıcının seçtiği uzmanlık
    [Display(Name = "Uzmanlık")]
    public int SelectedSpecializationId { get; set; }

    // Tarih seçimi
    [Display(Name = "Tarih")]
    [DataType(DataType.Date)]
    public DateTime SelectedDate { get; set; }

    // Doktorlar listesi (API'den gelir)
    public IEnumerable<DoctorDto> AvailableDoctors { get; set; } = new List<DoctorDto>();

    // Seçilen doktor
    [Display(Name = "Doktor")]
    public int SelectedDoctorId { get; set; }

    // Randevu saatleri (slotlar)
    public IEnumerable<AppointmentSlotDto> AvailableAppointmentSlots { get; set; } = new List<AppointmentSlotDto>();

    // Seçilen slot
    [Display(Name = "Randevu Saati")]
    public int SelectedAppointmentSlotId { get; set; }

    // Giriş yapmış hasta (gerekirse)
    public int PatientId { get; set; }
}
