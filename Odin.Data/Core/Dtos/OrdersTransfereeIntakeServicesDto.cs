using System.Collections.Generic;

namespace Odin.Data.Core.Dtos
{
    public class OrdersTransfereeIntakeServicesDto
    {
        public string Id { get; set; }
        public IEnumerable<OrdersTransfereeIntakeServiceDto> Services { get; set; }
    }
}
