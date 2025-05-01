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
    public class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
    {
        private readonly AppDbContext _appDbContext;
        public SpecializationRepository(AppDbContext dbContext) : base(dbContext)
        {
            _appDbContext = dbContext;
        }
    }
}
