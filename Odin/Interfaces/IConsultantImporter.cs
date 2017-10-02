using Odin.Data.Core.Dtos;

namespace Odin.Interfaces
{
    public interface IConsultantImporter
    {
        void ImportConsultants(ConsultantsDto consultantsDto);
    }
}