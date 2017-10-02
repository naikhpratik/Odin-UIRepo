using System.Threading.Tasks;
using Odin.Data.Core.Dtos;

namespace Odin.Interfaces
{
    public interface IManagerImporter
    {
        Task ImportManagers(ManagersDto managerDto);
    }
}