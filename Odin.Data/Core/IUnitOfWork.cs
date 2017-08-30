using MyDwellworks.Data.Core.Repositories;

namespace MyDwellworks.Data.Core
{
    public interface IUnitOfWork
    {
        IUsersRepository Users { get; }
        void Complete();
    }
}