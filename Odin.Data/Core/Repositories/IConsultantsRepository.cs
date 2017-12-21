using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IConsultantsRepository
    {
        Consultant GetConsultantBySeContactUid(int seContactUid);
        Consultant GetConsultantById(string id);
        void Add(Consultant consultant);
    }
}