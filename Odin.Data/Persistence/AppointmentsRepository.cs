using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class AppointmentsRepository : IAppointmentsRepository
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsRepository(ApplicationDbContext context)
        {
            _context = context;
        }        

        public void Add(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
        }
        public IEnumerable<Appointment> GetAppointmentsByOrderId(string id)
        {
            return _context.Appointments
                .Where(a => a.OrderId == id)
                .ToList();
        }
    }
}
