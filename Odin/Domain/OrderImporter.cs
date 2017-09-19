using AutoMapper;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Interfaces;

namespace Odin.Domain
{
    public class OrderImporter : IOrderImporter
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderImporter(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void ImportOrder(OrderDto orderDto)
        {
            var order = _unitOfWork.Orders.GetOrderByTrackingId(orderDto.TrackingId);

            if (order == null)
            {
                order = _mapper.Map<OrderDto, Order>(orderDto);
            }
            else
            {
                _mapper.Map<OrderDto, Order>(orderDto, order);
            }

            
            // Upsert
            // Determine if consultant is new
            //      If new create user and start process of emailing DSC
            // Lookup Program Manager by secontactuid, assign PM, or throw back error pm does not exist
            // Lookup transferee by email, if new transferee add transferee record.
        }
    }
}
