using Odin.Data.Core.Models;
using System.Collections.Generic;
using System.Linq;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class TransportationTypesRepository : ITransportationTypesRepository
    {
        private readonly IApplicationDbContext _context;

        public TransportationTypesRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TransportationType> GetTransportationTypes()
        {
            return _context.TransportationTypes.ToList();
        }
    }
}
