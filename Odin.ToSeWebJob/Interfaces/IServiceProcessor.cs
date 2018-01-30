using System.IO;
using System.Threading.Tasks;

namespace Odin.ToSeWebJob.Interfaces
{
    public interface IServiceProcessor
    {
        Task<string> ProcessService(string serviceId, TextWriter log);
    }
}