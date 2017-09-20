using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IConsultantRepository
    {
        Consultant GetConsultantBySeContactUid(int seContactUid);
    }
}