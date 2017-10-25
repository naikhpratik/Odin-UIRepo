namespace Odin.Data.Core.Models
{
    public class Pet : MobileTable
    {
        public string Type { get; set; }

        public string Breed { get; set; }

        public string Size { get; set; }

        public string OrderId { get; set; }

        public Order Order { get; set; }

        public void Delete()
        {
            Deleted = true;
        }
    }
}
