using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IServiceTypesRepository
    {
        void Add(ServiceType serviceType);
        IEnumerable<ServiceType> GetByServiceCategories(IEnumerable<ServiceCategory> cat);
        ServiceType GetServiceType(int serviceTypeId);

        IEnumerable<ServiceType> GetPossibleServiceTypes(IEnumerable<ServiceCategory> cat,
            IEnumerable<int> existingServiceTypeIds);
    }
}