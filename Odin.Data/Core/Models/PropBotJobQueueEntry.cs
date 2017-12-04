using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class PropBotJobQueueEntry
    {
        public string OrderId { get; set; }
        public string Notes { get; set; }
        public string PropertyUrl { get; set; }
    }
}
