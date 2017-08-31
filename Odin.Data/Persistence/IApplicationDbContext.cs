﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;

namespace Odin.Data.Persistence
{
    public interface IApplicationDbContext
    {
        DbSet<Transferee> Transferees { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<Rent> Rents { get; set; }
        DbSet<ConsultantAssignment> ConsultantAssignments { get; set; }
    }
}