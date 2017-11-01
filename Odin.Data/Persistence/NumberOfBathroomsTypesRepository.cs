using Odin.Data.Core.Models;
using System.Collections.Generic;
using System.Linq;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class NumberOfBathroomsTypesRepository : INumberOfBathroomsTypesRepository
    {
        private readonly IApplicationDbContext _context;

        public NumberOfBathroomsTypesRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<NumberOfBathroomsType> GetNumberOfBathroomsList()
        {
            return _context.NumberOfBathrooms.ToList();
        }
    }
}
