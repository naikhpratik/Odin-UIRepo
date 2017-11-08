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
    /// <summary>
    /// Adding items to the queue store will stage them to be pushed back to service engine.
    /// </summary>
    public class QueueStore : IQueueStore
    {
        private readonly CloudQueue _queue;
        private const string StorageConnectionKey = "StorageConnectionString";
        private const string QueueName = "odintose";

        public QueueStore()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting(StorageConnectionKey));
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference(QueueName);
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