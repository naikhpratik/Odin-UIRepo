using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Data.Entity;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class HomeFindingPropertyRepository : IHomeFindingPropertyRepository
    {
        private readonly IApplicationDbContext _context;

        public HomeFindingPropertyRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        public HomeFindingProperty GetHomeFindingPropertyById(string homeFindingPropertyId)
        {
            return _context.HomeFindingProperties
                .Where(hfp => hfp.Id.Equals(homeFindingPropertyId))
                .Include(hfp => hfp.Property.Photos)
                .SingleOrDefault<HomeFindingProperty>();
        }
    }
}
