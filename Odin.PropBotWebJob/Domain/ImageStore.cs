using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Odin.PropBotWebJob.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Odin.PropBotWebJob.Domain
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

        public CloudBlobContainer GetImageContainer()
        {
            return _client.GetContainerReference(ContainerName);
        }

        public string SaveImage(Stream stream)
        {
            var id = Guid.NewGuid().ToString();
            var container = _client.GetContainerReference(ContainerName);
            var blob = container.GetBlockBlobReference(id);
            blob.UploadFromStream(stream);

            return id;
        }

        public async Task<string> SaveImageAsync(Stream stream)
        {
            var id = Guid.NewGuid().ToString();
            var container = _client.GetContainerReference(ContainerName);
            var blob = container.GetBlockBlobReference(id);
            await blob.UploadFromStreamAsync(stream);

            return id;
        }

        public Uri PrivateUriFor(string imageId)
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

            // Using storage emulator.
            if (_client.BaseUri.IsLoopback)
            {
                return new Uri($"{_client.BaseUri}/{ContainerName}/{imageId}");
            }

            return new Uri($"{_client.BaseUri}{ContainerName}/{imageId}{sasToken}");
        }

        public Uri UriFor(string imageId)
        {
            // Using storage emulator
            if (_client.BaseUri.IsLoopback)
            {
                return new Uri($"{_client.BaseUri}/{ContainerName}/{imageId}");
            }

            return new Uri($"{_client.BaseUri}{ContainerName}/{imageId}");
        }

        public ICloudBlob ImageBlobFor(string imageId)
        {
            return _client.GetBlobReferenceFromServer(UriFor(imageId));
        }
    }
}
