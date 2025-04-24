using AutoMapper;
using MyProject.Core.Entities;
using MyProject.Application.DTOs;

namespace MyProject.Application.Mappings
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<Appointment, AppointmentDto>().ReverseMap();
            CreateMap<Appointment, CreateAppointmentDto>().ReverseMap();
        }
    }
}
