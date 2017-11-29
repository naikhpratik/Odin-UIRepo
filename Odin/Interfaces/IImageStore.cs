using Microsoft.WindowsAzure.Storage.Blob;
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
        string SaveImage(Stream stream);
        Task<string> SaveImageAsync(Stream stream);
        Uri UriFor(string imageId);

        ICloudBlob ImageBlobFor(string imageId);
        CloudBlobContainer GetImageContainer();
    }
}