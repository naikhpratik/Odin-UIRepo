using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Odin.Data.Core.Dtos
{
    public class OrdersTransfereeDetailsServicesDto
    {
        public string Id { get; set; }
        public IEnumerable<OrdersTransfereeDetailsServiceDto> Services { get; set; }
    }
}
