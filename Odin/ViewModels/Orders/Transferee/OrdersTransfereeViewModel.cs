using Odin.ViewModels.Shared;

namespace Odin.ViewModels.Orders.Transferee
{
    public class OrdersTransfereeViewModel
    {
        public int Id { get; set; }

        public string DestinationCity { get; set; }
        public string DestinationState { get; set; }
        public string DestinationCountry { get; set; }

        public bool IsRush { get; set; }
        public bool IsVip { get; set; }

        public TransfereeViewModel Transferee { get; set; }
    }
}