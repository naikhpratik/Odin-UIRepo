using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Odin.PropBotWebJob.Interfaces
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
