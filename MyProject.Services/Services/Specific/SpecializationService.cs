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
    public class SpecializationService : Service<Specialization>, ISpecializationService
    {
        private readonly ISpecializationRepository _specRepository;
        public SpecializationService(ISpecializationRepository repository) : base(repository)
        {
            _specRepository = repository;
        }
    }
}
