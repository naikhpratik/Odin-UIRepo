using Odin.Data.Core.Models;

namespace Odin.Data.Core.Repositories
{
    public interface IAppointmentsRepository
    {
        void Add(Appointment appointment);
    }
}