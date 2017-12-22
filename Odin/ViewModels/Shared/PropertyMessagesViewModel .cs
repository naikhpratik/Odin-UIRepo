using System;
using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.ViewModels.Shared
{
    public class PropertyMessagesViewModel
    {
        public string Id { get; set; }
        public IEnumerable<Message> messages { get; set; }
        public DateTime? latest { get; set; }
    }
}