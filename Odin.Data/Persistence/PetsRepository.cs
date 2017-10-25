using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class PetsRepository : IPetsRepository
    {
        private readonly IApplicationDbContext _context;

        public PetsRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public Pet GetChildById(string id)
        {
            return _context.Pets.SingleOrDefault(c => c.Id == id);
        }
    }
}
