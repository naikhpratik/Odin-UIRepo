using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
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

        public void ImportManager(ManagerDto managerDto)
        {
            
        }
    }
}