using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Models
{
    public class HomeFindingProperty : MobileTable
    {
        public Property Property { get; set; }
        public Rent HomeFindingService { get; set; }
    }
}
