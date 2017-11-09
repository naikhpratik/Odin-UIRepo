using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

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

        public DepositType GetDepositType(string seValue)
        {
            return _context.DepositTypes.SingleOrDefault(d => d.SeValue == seValue);
        }
    }
}
