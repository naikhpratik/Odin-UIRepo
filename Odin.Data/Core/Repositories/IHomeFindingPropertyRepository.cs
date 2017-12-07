using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IHomeFindingPropertyRepository
    {
        HomeFindingProperty GetHomeFindingPropertyById(string homeFindingPropertyId);
    }
}
