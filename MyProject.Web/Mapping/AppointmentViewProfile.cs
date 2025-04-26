// MyProject.WebUI/Mapping/AppointmentProfile.cs
using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Web.ViewModels.AppointmentViewModels;

public class AppointmentViewProfile : Profile
{
    public AppointmentViewProfile()
    {
        // DTO → ViewModel eşlemesi
        CreateMap<AppointmentDto, AppointmentViewModel>();
    }
}
