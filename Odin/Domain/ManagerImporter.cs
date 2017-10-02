using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Interfaces;

namespace Odin.Domain
{
    public class ManagerImporter : IManagerImporter
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ManagerImporter(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void ImportManagers(ManagersDto managersDto)
        {
            foreach (var managerDto in managersDto.Managers)
            {
                var manager = _unitOfWork.Managers.GetManagerBySeContactUid(managerDto.SeContactUid);

                if (manager == null)
                {
                    manager = _mapper.Map<ManagerDto, Manager>(managerDto);
                    _unitOfWork.Managers.Add(manager, managerDto.Role);
                    //TODO: Send activation email
                }
            }

            _unitOfWork.Complete();
        }
    }
}