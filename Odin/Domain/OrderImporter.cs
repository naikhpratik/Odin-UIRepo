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
            var transferee = _unitOfWork.Transferees.GetTransfereeByEmail(orderDto.Transferee.Email);
            var consultant = _unitOfWork.Consultants.GetConsultantBySeContactUid(orderDto.Consultant.SeContactUid);
            var programManager = _unitOfWork.Managers.GetManagerBySeContactUid(orderDto.ProgramManager.SeContactUid);

            if (order == null)
            {
                order = _mapper.Map<OrderDto, Order>(orderDto);

                if (transferee == null)
                {
                    transferee = _mapper.Map<TransfereeDto, Transferee>(orderDto.Transferee);
                    _unitOfWork.Transferees.Add(transferee);
                }
                else
                {
                    _mapper.Map<TransfereeDto, Transferee>(orderDto.Transferee, transferee);
                    transferee.Orders.Add(order);
                    order.Transferee = transferee;
                }
            }
            else
            {
                _mapper.Map<OrderDto, Order>(orderDto, order);
            }

            order.Consultant = consultant;
            order.ProgramManager = programManager;

            _unitOfWork.Orders.Add(order);
            _unitOfWork.Complete();

        }
        
        
    }
}
