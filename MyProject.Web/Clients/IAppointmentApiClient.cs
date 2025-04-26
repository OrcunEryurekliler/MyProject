using MyProject.Application.DTOs;

public interface IAppointmentApiClient
{
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);
}