using System.ComponentModel.DataAnnotations.Schema;

namespace Odin.Data.Core.Models
{
    public class Child
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public int Grade { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }

        [NotMapped]
        public string TempId { set; get; }
    }
}
