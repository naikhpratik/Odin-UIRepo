using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class AreaTypesRepository : IAreaTypesRepository
    {
        private readonly IApplicationDbContext _context;

        public AreaTypesRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AreaType> GetAreaTypesList()
        {
            return _context.AreaTypes.ToList();
        }
    }
}
