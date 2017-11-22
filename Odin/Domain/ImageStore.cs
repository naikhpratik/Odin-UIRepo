using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Odin.Data.Core;
using Odin.Data.Core.Models;
using Odin.Interfaces;

namespace Odin.Domain
{
    public class ImageStore : IImageStore
    {
        private readonly IUnitOfWork _unitOfWork;
        readonly CloudBlobClient _client;

        private const string StorageConnectionKey = "StorageConnectionString";
        private const string ContainerName = "images";

        public ImageStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting(StorageConnectionKey));

            _client = storageAccount.CreateCloudBlobClient();
        }

        public async Task<string> SaveImage(Stream stream, string propertyId)
        {
            var id = Guid.NewGuid().ToString();
            var container = _client.GetContainerReference(ContainerName);
            var blob = container.GetBlockBlobReference(id);
            await blob.UploadFromStreamAsync(stream);

            var urlStr = UriFor(id).AbsoluteUri;
            var photo = new Photo(propertyId, id, urlStr);
            _unitOfWork.Photos.Add(photo);
            _unitOfWork.Complete();

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
    }
}