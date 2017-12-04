using Odin.Data.Core.Models;
using Odin.Data.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System;

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
                .Where(a => a.OrderId == id && a.Deleted == false && a.ScheduledDate >= DateTime.Now)
                .ToList();
        }
        public Appointment GetAppointmentById(string id)
        {
            return _context.Appointments
                .Where(a => a.Id.Equals(id))
                .SingleOrDefault<Appointment>(); 
        }
        public void Remove(Appointment appointment)
        {
            _context.Appointments.Remove(appointment);
        }
    }
}
