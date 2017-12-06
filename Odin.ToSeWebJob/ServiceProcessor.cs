using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core;

namespace Odin.ToSeWebJob
{
    public class ServiceProcessor : IServiceProcessor
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceProcessor(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void ProcessService(string serviceId)
        {
            var service = _unitOfWork.Services.GetServiceById(serviceId);

        }
    }

    public interface IServiceProcessor
    {
        void ProcessService(string serviceId);
    }
}
