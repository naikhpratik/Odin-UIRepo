using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IHomeFindingPropertyRepository
    {
        HomeFindingProperty GetHomeFindingPropertyById(string homeFindingPropertyId);
        HomeFindingProperty GetHomeFindingPropertyByPropertyId(string propertyId);
        IEnumerable<HomeFindingProperty> GetUpcomingHomeFindingPropertiesByHomeFindingId(string homeFindingId);
    }
}
