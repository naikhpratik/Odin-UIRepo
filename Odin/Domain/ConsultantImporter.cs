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
    public class ConsultantImporter : IConsultantImporter
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ConsultantImporter(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void ImportConsultants(ConsultantsDto consultantsDto)
        {
            foreach (var consultantDto in consultantsDto.Consultants)
            {
                var consultant = _unitOfWork.Consultants.GetConsultantBySeContactUid(consultantDto.SeContactUid);

                if (consultant == null)
                {
                    consultant = _mapper.Map<ConsultantImportDto, Consultant>(consultantDto);
                    _unitOfWork.Consultants.Add(consultant);
                    //TODO: Send activation email
                }
            }
            _unitOfWork.Complete();
        }
    }
}