using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.Core.Entities;
using MyProject.Core.Interfaces;
using MyProject.Infrastructure.Data.Repositories.Generic;

namespace MyProject.Infrastructure.Data.Repositories.Specific
{
    public class PatientProfileRepository : Repository<PatientProfile>, IPatientProfileRepository
    {
        
        public PatientProfileRepository(AppDbContext context) : base (context)
        {
            
        }
    }
}
