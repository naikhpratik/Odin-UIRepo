using System;
using AutoMapper;
using Odin.Data.Core;
using Odin.Data.Core.Dtos;
using Odin.Data.Core.Models;
using Odin.Interfaces;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;

namespace Odin.Domain
{
    public class ManagerImporter : IManagerImporter
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAccountHelper _accountHelper;
        private readonly TelemetryClient _telemetryClient;

        public ManagerImporter(IUnitOfWork unitOfWork, IMapper mapper, IAccountHelper accountHelper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _accountHelper = accountHelper;
            _telemetryClient = new TelemetryClient();
        }

        public async Task ImportManagers(ManagersDto managersDto)
        {
            foreach (var managerDto in managersDto.Managers)
            {
                try
                {
                    var manager = _unitOfWork.Managers.GetManagerBySeContactUid(managerDto.SeContactUid);

                    if (manager == null)
                    {
                        manager = _mapper.Map<ManagerDto, Manager>(managerDto);
                        _unitOfWork.Managers.Add(manager, managerDto.Role);
                        await _accountHelper.SendEmailCreateTokenAsync(manager.Id);
                    }
                }
                // Track the exception but keep processing records.
                catch (Exception e)
                {
                    _telemetryClient.TrackException(e);
                }
            }

            _unitOfWork.Complete();
        }
    }
}