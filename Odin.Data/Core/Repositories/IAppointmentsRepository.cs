using Odin.Data.Core.Models;
using System.Collections.Generic;

namespace Odin.Data.Core.Repositories
{
    public interface IAppointmentsRepository
    {
        void Add(Appointment appointment);
        IEnumerable<Appointment> GetAppointmentsByOrderId(string id);
        Appointment GetAppointmentById(string id);
    }
}