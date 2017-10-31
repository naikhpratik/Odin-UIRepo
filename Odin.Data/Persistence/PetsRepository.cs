using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class PetsRepository : IPetsRepository
    {
        private readonly IApplicationDbContext _context;

        public PetsRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public Pet GetPetFor(string userId, string id)
        {
            return _context.Pets.SingleOrDefault(p => p.Id == id && p.Order.ConsultantId == userId);
        }
    }
}
