using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IConsultantsRepository
    {
        Consultant GetConsultantBySeContactUid(int seContactUid);
        void Add(Consultant consultant);
    }
}