using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Odin.Data.Core.Models
{
    public class HomeFindingProperty : MobileTable
    {
        public HomeFindingProperty()
        {
            Messages = new Collection<Message>();
        }
        public Property Property { get; set; }
        public HomeFinding HomeFinding { get; set; }
        public bool? Liked { get; set; }
        public DateTime? ViewingDate { get; set; }
        public virtual ICollection<Message> Messages { get; set; }
        public bool? selected { get; set; }
    }
}
