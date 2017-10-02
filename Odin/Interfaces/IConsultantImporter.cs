using System.Threading.Tasks;
using Odin.Data.Core.Dtos;

namespace Odin.Interfaces
{
    public interface IConsultantImporter
    {
        Task ImportConsultants(ConsultantsDto consultantsDto);
    }
}