using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Odin.Interfaces
{
    public interface IImageStore
    {
        Task<string> SaveImage(Stream stream, string propertyId);
        Uri UriFor(string imageId);
    }
}