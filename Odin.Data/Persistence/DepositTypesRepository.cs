using Odin.Data.Core.Models;
using System.Collections.Generic;
using System.Linq;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class DepositTypesRepository : IDepositTypesRepository
    {
        private readonly IApplicationDbContext _context;

        public DepositTypesRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<DepositType> GetDepositTypesList()
        {
            return _context.DepositTypes.ToList();
        }
    }
}
