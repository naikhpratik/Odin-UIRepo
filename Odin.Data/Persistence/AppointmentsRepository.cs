using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;


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
    }
}
