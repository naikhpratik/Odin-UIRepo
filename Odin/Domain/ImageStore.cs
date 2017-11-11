using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Odin.Interfaces;

namespace Odin.Domain
{
    public class ImageStore : IImageStore
    {
        readonly CloudBlobClient _client;

        private const string StorageConnectionKey = "StorageConnectionString";
        private const string ContainerName = "images";

        public ImageStore()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting(StorageConnectionKey));

            _client = storageAccount.CreateCloudBlobClient();
        }

        public async Task<string> SaveImage(Stream stream)
        {
            var id = Guid.NewGuid().ToString();
            var container = _client.GetContainerReference(ContainerName);
            var blob = container.GetBlockBlobReference(id);
            await blob.UploadFromStreamAsync(stream);
            return id;
        }

        public Uri UriFor(string imageId)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.Now.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.Now.AddMinutes(30)
            };
            var container = _client.GetContainerReference(ContainerName);
            var blob = container.GetBlockBlobReference(imageId);
            var sasToken = blob.GetSharedAccessSignature(sasPolicy);

            // Using storage emulator
            if (_client.BaseUri.IsLoopback)
            {
                return new Uri($"{_client.BaseUri}/{ContainerName}/{imageId}");
            }

            return new Uri($"{_client.BaseUri}{ContainerName}/{imageId}{sasToken}");
        }
    }
}