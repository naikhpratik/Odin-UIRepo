using Odin.Data.Core.Dtos;

namespace Odin.Interfaces
{
    public interface IOrderImporter
    {
        void ImportOrder(OrderDto orderDto);
    }
}