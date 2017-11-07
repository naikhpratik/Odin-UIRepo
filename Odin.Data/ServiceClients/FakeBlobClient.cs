using System;

namespace Odin.Data.ServiceClients
{
    public class FakeBlobClient
    {
        private readonly Uri _cloudUri;
        private readonly string _accountName;
        private readonly string _storageKey;

        public FakeBlobClient(Uri cloudUri, string accountName, string storageKey)
        {
            _cloudUri = cloudUri;
            _accountName = accountName;
            _storageKey = storageKey;
        }
    }
}