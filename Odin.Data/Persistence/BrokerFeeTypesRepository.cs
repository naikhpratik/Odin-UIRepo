using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class BrokerFeeTypesRepository : IBrokerFeeTypesRepository
    {
        private readonly IApplicationDbContext _context;

        public BrokerFeeTypesRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BrokerFeeType> GetBrokerFeeTypes()
        {
            return _context.BrokerFeeTypes.ToList();
        }

        public BrokerFeeType GetBrokerFeeType(string seValue)
        {
            return _context.BrokerFeeTypes.SingleOrDefault(b => b.SeValue == seValue);
        }
    }
}
