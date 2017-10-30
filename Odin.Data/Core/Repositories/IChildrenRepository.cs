using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IChildrenRepository
    {
        Child GetChildById(string id);
    }
}