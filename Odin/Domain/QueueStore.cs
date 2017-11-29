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
        private readonly CloudQueueClient _queueClient;
        private const string StorageConnectionKey = "StorageConnectionString";
        private const string OdinToSeQueueName = "odintose";
        private const string PropBotQueueName = "propbotqueue";

        public QueueStore()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting(StorageConnectionKey));
            _queueClient = storageAccount.CreateCloudQueueClient();
            var odinToSeQueue = _queueClient.GetQueueReference(OdinToSeQueueName);
            odinToSeQueue.CreateIfNotExists();
            var propBotQueue = _queueClient.GetQueueReference(PropBotQueueName);
            propBotQueue.CreateIfNotExists();
        }

        public void Add(OdinToSeQueueEntry entry)
        {
            var queue = _queueClient.GetQueueReference(OdinToSeQueueName);
            CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(entry));
            queue.AddMessage(message);
        }

        public void Add(PropBotJobQueueEntry entry)
        {
            var queue = _queueClient.GetQueueReference(PropBotQueueName);
            CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(entry));
            queue.AddMessage(message);
        }
    }
}