using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IAreaTypesRepository
    {
        IEnumerable<AreaType> GetAreaTypesList();
    }
}