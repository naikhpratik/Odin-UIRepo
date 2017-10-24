using System.Collections.Generic;

namespace Odin.Data.Core.Dtos
{
    public class OrdersTransfereeIntakeFamilyDto
    {
        public OrdersTransfereeIntakeFamilyDto()
        {
            Children = new List<ChildDto>();
        }

        public string Id { get; set; }
        public string SpouseName { get; set; }
        public string SpouseVisaType { get; set; }
        public IEnumerable<ChildDto> Children { get; set; }
        public string ChildrenEducationPreferences { get; set; }
    }
}
