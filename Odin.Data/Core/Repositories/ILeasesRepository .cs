using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface ILeasesRepository
    {
        void Add(Lease lease);
        void Remove(Lease lease);
        Lease GetLeaseByPropertyId(string id);
        Lease GetLeaseById(string id);
    }
}