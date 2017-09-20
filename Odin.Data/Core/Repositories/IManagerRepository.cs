using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IManagerRepository
    {
        Manager GetManagerBySeContactUid(int seContactUid);
    }
}