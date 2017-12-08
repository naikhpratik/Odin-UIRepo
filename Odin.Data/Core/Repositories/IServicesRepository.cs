using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IServicesRepository
    {
        void Add(Service service);
        IEnumerable<Service> GetServicesByOrderId(string id);
        Service GetServiceById(string id);
    }
    
}
