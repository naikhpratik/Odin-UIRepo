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
            var consultantId = _unitOfWork.Consultants.GetConsultantBySeContactUid(orderDto.Consultant.SeContactUid).Id;
            var programManagerId = _unitOfWork.Managers.GetManagerBySeContactUid(orderDto.ProgramManager.SeContactUid).Id;

            if (order == null)
            {
                order = _mapper.Map<OrderDto, Order>(orderDto);

                //Add new services
                foreach (var serviceDto in orderDto.Services)
                {
                    if (!order.HasService(serviceDto.ServiceTypeId))
                    {
                        var newService = _mapper.Map<ServiceDto, Service>(serviceDto);
                        order.Services.Add(newService);
                    }
                }

                if (transferee == null)
                {
                    transferee = _mapper.Map<TransfereeDto, Transferee>(orderDto.Transferee);
                    _unitOfWork.Transferees.Add(transferee);
                }                

                _unitOfWork.Orders.Add(order);
            }
            else
            {
                _mapper.Map<OrderDto, Order>(orderDto, order);

                //Add new services
                foreach (var serviceDto in orderDto.Services)
                {
                    if (!order.HasService(serviceDto.ServiceTypeId))
                    {
                        var newService = _mapper.Map<ServiceDto, Service>(serviceDto);
                        order.Services.Add(newService);
                    }
                }

                if (transferee == null)
                {
                    transferee = _mapper.Map<TransfereeDto, Transferee>(orderDto.Transferee);
                    _unitOfWork.Transferees.Add(transferee);
                }
            }

            order.TransfereeId = transferee.Id;
            order.ConsultantId = consultantId;
            order.ProgramManagerId = programManagerId;

            _unitOfWork.Complete();
            
        }
        
        
    }
}
