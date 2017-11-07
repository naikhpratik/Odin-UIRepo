using System.Collections.Generic;

namespace Odin.Data.Core.Dtos
{
    public class OrdersTransfereeIntakeServicesDto
    {
        public OrdersTransfereeIntakeServicesDto()
        {
            Services = new List<OrdersTransfereeIntakeServiceDto>();
        }

        public string Id { get; set; }
        public IEnumerable<OrdersTransfereeIntakeServiceDto> Services { get; set; }
    }
}
