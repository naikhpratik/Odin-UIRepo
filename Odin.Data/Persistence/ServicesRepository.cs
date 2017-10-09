using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;

namespace Odin.Data.Persistence
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly ApplicationDbContext _context;

        public ServicesRepository(ApplicationDbContext context)
        {
            _context = context;
        }        

        public void Add(Service service)
        {
            _context.Services.Add(service);
        }
    }
}
