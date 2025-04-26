using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Application.Interfaces;
using MyProject.Application.Services.Generic;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;

namespace MyProject.Application.Services.Specific
{
    public class PatientProfileService : Service<PatientProfile>, IPatientProfileService
    {
        private readonly IPatientProfileRepository _patientProfileRepository;
        public PatientProfileService(IPatientProfileRepository patientProfileRepository) : base(patientProfileRepository)
        {
            _patientProfileRepository = patientProfileRepository;
        }
    }
}
