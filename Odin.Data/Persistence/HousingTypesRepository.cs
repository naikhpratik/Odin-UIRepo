using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class HousingTypesRepository : IHousingTypesRepository
    {
        private readonly IApplicationDbContext _context;

        public HousingTypesRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<HousingType> GetHousingTypesList()
        {
            return _context.HousingTypes.ToList();
        }
    }
}
