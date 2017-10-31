using Odin.Data.Core.Models;
using System.Collections.Generic;
using System.Linq;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class BrokerFeeTypesRepository : IBrokerFeeTypesRepository
    {
        private readonly IApplicationDbContext _context;

        public BrokerFeeTypesRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<BrokerFeeType> GetBorkerBrokerFeeTypes()
        {
            return _context.BrokerFeeTypes.ToList();
        }
    }
}
