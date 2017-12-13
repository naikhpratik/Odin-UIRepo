using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IHomeFindingPropertyRepository
    {
        HomeFindingProperty GetHomeFindingPropertyById(string homeFindingPropertyId);
        IEnumerable<HomeFindingProperty> GetUpcomingHomeFindingPropertiesByHomeFindingId(string homeFindingId);
    }
}
