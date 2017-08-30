using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDwellworks.Data.Core.Models
{
    public class Engagement
    {
        public DateTimeOffset? ScheduledDate;
        public DateTimeOffset? CompletedDate;
        public string Notes;
    }
}
