using Odin.Data.Core.Repositories;

namespace Odin.Data.Core
{
    public interface IUnitOfWork
    {
        IUsersRepository Users { get; }
        IOrdersRepository Orders { get; }
        ITransfereesRepository Transferees { get; }
        IConsultantsRepository Consultants { get; }
        IManagersRepository Managers { get; }
        IServicesRepository Services { get; }
        IServiceTypesRepository ServiceTypes { get; }
        IAppointmentsRepository Appointments { get; }
        IChildrenRepository Children { get; }
        IPetsRepository Pets { get; }
        INumberOfBathroomsTypesRepository NumberOfBathrooms { get; }
        IHousingTypesRepository HousingTypes { get; }
        IAreaTypesRepository AreaTypes { get; }
        ITransportationTypesRepository TransportationTypes { get; }
        IDepositTypesRepository DepositTypes { get; }
        IBrokerFeeTypesRepository BrokerFeeTypes { get; }
        IPhotosRepository Photos { get; }
        void Complete();
    }
}
