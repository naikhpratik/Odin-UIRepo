using Odin.Data.Core.Models;
using System.Linq;


namespace Odin.Data.Persistence
{
    public class ServiceTypesRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceTypesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ServiceType serviceType)
        {
            _context.ServiceTypes.Add(serviceType);
        }

        public ServiceType GetServiceType(int serviceTypeId)
        {
            return _context.ServiceTypes.FirstOrDefault<ServiceType>(s => s.Id == serviceTypeId);
        }
    }
}
