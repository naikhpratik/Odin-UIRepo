using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Odin.Data.Persistence
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly ApplicationDbContext _context;

        public MessagesRepository(ApplicationDbContext context)
        {
            _context = context;
        }        

        public void Add(Message message)
        {
            _context.Messages.Add(message);
        }
        public IEnumerable<Message> GetMessagesByPropertyId(string id)
        {
            return _context.Messages
                .Where(a => a.HomeFindingPropertyId == id && a.Deleted == false).OrderByDescending(a => a.MessageDate)
                .ToList();
        }
        public Message GetMessageById(string id)
        {
            return _context.Messages
                .Where(a => a.Id.Equals(id))
                .SingleOrDefault<Message>(); 
        }
        public void Remove(Message message)
        {
            _context.Messages.Remove(message);
        }
    }
}
