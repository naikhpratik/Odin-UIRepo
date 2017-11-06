using System;
using System.IO;
using System.Threading.Tasks;

namespace Odin.Data.ServiceClients
{
    public interface IImageStore
    {
        Uri UriFor(string iamgeId);
        Task<string> SaveImage(Stream stream);
    }
}