using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Interfaces
{
    interface IItineraryModelBuilder<TViewModel>
    {
        TViewModel Build(string orderId);
    }
}
