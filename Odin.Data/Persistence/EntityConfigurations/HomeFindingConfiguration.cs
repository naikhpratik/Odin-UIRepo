using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;

namespace Odin.Data.Persistence.EntityConfigurations
{
    public class HomeFindingConfiguration : EntityTypeConfiguration<HomeFinding>
    {
        public HomeFindingConfiguration()
        {
            HasKey(r => r.Id);

            HasRequired(r => r.Order)
                .WithOptional(o => o.HomeFinding)
                .WillCascadeOnDelete(false);

            HasMany(hf => hf.HomeFindingProperties).
                WithRequired(hfp => hfp.HomeFinding).
                WillCascadeOnDelete(true);
        }
    }
}
