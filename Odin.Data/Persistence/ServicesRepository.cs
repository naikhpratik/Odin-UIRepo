using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

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
        public IEnumerable<Service> GetServicesByOrderId(string id)
        {
            return _context.Services
                .Where(s => s.OrderId == id && s.CompletedDate.HasValue == false)
                .Include(s => s.ServiceType)
                .ToList();
        }
    }
}
