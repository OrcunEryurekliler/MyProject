using MyProject.Application.DTOs;

public class AppointmentApiClient : IAppointmentApiClient
{
    private readonly HttpClient _http;
    public AppointmentApiClient(HttpClient http) => _http = http;

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
    {
        var res = await _http.PostAsJsonAsync("api/appointment/create", dto);
        res.EnsureSuccessStatusCode();
        return await res.Content.ReadFromJsonAsync<AppointmentDto>();
    }
}