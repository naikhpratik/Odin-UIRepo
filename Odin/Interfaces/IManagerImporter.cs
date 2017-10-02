using Odin.Data.Core.Dtos;

namespace Odin.Interfaces
{
    public interface IManagerImporter
    {
        void ImportManagers(ManagersDto managerDto);
    }
}