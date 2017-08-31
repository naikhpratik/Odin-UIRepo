﻿using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;

namespace Odin.Data.Persistence.EntityConfigurations
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            HasRequired(o => o.Transferee)
                .WithMany(t => t.Orders)
                .WillCascadeOnDelete(false);

            HasMany(o => o.Consultants)
                .WithRequired(ca => ca.Order)
                .WillCascadeOnDelete(false);

        }
    }
}