using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.ServiceClients
{
    public class ImageStore : IImageStore
    {
        private FakeBlobClient _client;
        private Uri _cloudUri;

        public ImageStore(string storageKey, string accountName, Uri cloudUri)
        {
            _cloudUri = cloudUri;
            _client = new FakeBlobClient(cloudUri, accountName,storageKey);
        }

        public async Task<string> SaveImage(Stream stream)
        {

            return "Test";
        }

        public Uri UriFor(string iamgeId)
        {
            
            return new Uri("test");    
        }
    }
}
