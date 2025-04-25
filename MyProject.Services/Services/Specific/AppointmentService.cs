using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyProject.Application.DTOs;
using MyProject.Application.Interfaces;
using MyProject.Application.Services.Generic;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

namespace MyProject.Application.Services.Specific
{
    public class AppointmentService: Service<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorUnavailabilityRepository _doctorUnavailabilityRepository;
        private readonly IMapper _mapper;
        public AppointmentService(IAppointmentRepository repository,
                                  IDoctorUnavailabilityRepository doctorUnavailabilityRepository,
                                  IMapper mapper) : base(repository)
        {
            _appointmentRepository = repository;
            _doctorUnavailabilityRepository = doctorUnavailabilityRepository;
            _mapper = mapper;
        }


        public async Task<IEnumerable<Appointment>> GetAllAsyncByPatient(int Id)
        {
            return await _appointmentRepository.GetAllAsyncByPatient(Id);
        }
        public async Task<IEnumerable<Appointment>> GetAllAsyncByDoctor(int Id)
        {
            return await _appointmentRepository.GetAllAsyncByDoctor(Id);
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllByPatientDtoAsync(int patientId)
        {
            var entities = await _appointmentRepository.GetAllAsyncByPatient(patientId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(entities);
            
        }

        

        public async Task<AppointmentDto> CreateAppointmentDtoAsync(CreateAppointmentDto dto, int patientId)
        {
            // 1. İstenen slot hâlâ müsait mi?
            var available = await GetAvailableTimeSlotsAsync(dto.DoctorProfileId, dto.StartTime);
            if (!available.Contains(dto.StartTime.TimeOfDay))
                throw new InvalidOperationException("Seçilen saat dolu veya doktor o saatte müsait değil.");

            // 2. Entity’yi oluştur
            var appointment = new Appointment
            {
                DoctorProfileId = dto.DoctorProfileId,
                PatientProfileId = patientId,
                StartTime = dto.StartTime.Date.Add(dto.StartTime.TimeOfDay),
                EndTime = dto.StartTime.Date.Add(dto.StartTime.TimeOfDay).AddMinutes(dto.DurationMinutes),
                Status = dto.Status ?? "Beklemede"
            };

            // 3. Kaydet
            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveChangesAsync();

            // 4. DTO’ya çevirip dön
            return _mapper.Map<AppointmentDto>(appointment);
        }

    // Mevcut GetAvailableTimeSlotsAsync ve diğer metodlar…
      

        public async Task<IEnumerable<AppointmentDto>> GetAllByDoctorDtoAsync(int doctorId)
        {
            var entities = await _appointmentRepository.GetAllAsyncByPatient(doctorId);
            return _mapper.Map<IEnumerable<AppointmentDto>>(entities);
        }

        public async Task<IEnumerable<TimeSpan>> GetAvailableTimeSlotsAsync(int doctorId, DateTime date)
        {
            var allSlots = new List<TimeSpan>();

            // Sabah: 09:00 - 12:30
            for (var time = new TimeSpan(9, 0, 0); time < new TimeSpan(12, 30, 0); time = time.Add(TimeSpan.FromMinutes(30)))
            {
                allSlots.Add(time);
            }

            // Öğle: 13:30 - 17:30
            for (var time = new TimeSpan(13, 30, 0); time < new TimeSpan(17, 30, 0); time = time.Add(TimeSpan.FromMinutes(30)))
            {
                allSlots.Add(time);
            }

            // Unavailable gün kontrolü
            var isUnavailable = await _doctorUnavailabilityRepository
                .AnyAsync(x => x.DoctorProfileId == doctorId && x.OffWorkDate.Date == date.Date);

            if (isUnavailable)
                return Enumerable.Empty<TimeSpan>();

            // O güne ait alınmış randevular
            var takenSlots = await _appointmentRepository.GetTakenSlotsAsync(doctorId, date);
            


            // Boş slotlar
            return allSlots.Where(slot => !takenSlots.Contains(slot));
        }
    }
}
