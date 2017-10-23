namespace Odin.Data.Core.Models
{
    public class Child : MobileTable
    {
        public string Name { get; set; }

        public int? Age { get; set; }

        public int? Grade { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }

    }
}
