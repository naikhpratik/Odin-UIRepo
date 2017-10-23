using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IChildrenRepository
    {
        void Remove(Child child);
        Child GetChildById(string id);
    }
}