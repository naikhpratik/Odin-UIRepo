using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IAccountHelper _accountHelper;

        public ConsultantImporter(IUnitOfWork unitOfWork, IMapper mapper, IAccountHelper accountHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountHelper = accountHelper;
        }

        public async Task ImportConsultants(ConsultantsDto consultantsDto)
        {
            foreach (var consultantDto in consultantsDto.Consultants)
            {
                var consultant = _unitOfWork.Consultants.GetConsultantBySeContactUid(consultantDto.SeContactUid);

                if (consultant == null)
                {
                    consultant = _mapper.Map<ConsultantImportDto, Consultant>(consultantDto);
                    _unitOfWork.Consultants.Add(consultant);
                    await _accountHelper.SendEmailConfirmationTokenAsync(consultant.Id);
                }
            }
            _unitOfWork.Complete();
        }
    }
}