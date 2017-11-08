using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using Odin.Data.Core.Models;
using Odin.Interfaces;

namespace Odin.Domain
{
    public class CloudQueueStore : ICloudQueueStore
    {
        private readonly CloudQueue _queue;

        public CloudQueueStore(string storageConnectionKey, string queueName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting(storageConnectionKey));
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();
            _queue = queue;
        }

        public void Add(QueueEntry entry)
        {
            CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(entry));
            _queue.AddMessage(message);
        }
        
    }
}