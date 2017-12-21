using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IMessagesRepository
    {
        void Add(Message message);
        void Remove(Message message);
        IEnumerable<Message> GetMessagesByPropertyId(string id);
        Message GetMessageById(string id);
    }
}