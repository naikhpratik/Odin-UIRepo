using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Linq;


namespace Odin.Data.Persistence
{
    public class ServiceTypesRepository : IServiceTypesRepository
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

        public IEnumerable<ServiceType> GetByServiceCategories(IEnumerable<ServiceCategory> cat)
        {
            return _context.ServiceTypes.Where(st => cat.Contains(st.Category)).ToList();
        }

        public IEnumerable<ServiceType> GetPossibleServiceTypes(IEnumerable<ServiceCategory> cat,IEnumerable<int> existingServiceTypeIds)
        {
            return _context.ServiceTypes.Where(st =>
                cat.Contains(st.Category) && !existingServiceTypeIds.Contains(st.Id)).ToList();
        }
    }
}
