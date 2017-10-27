using Odin.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Odin.Data.Persistence.EntityConfigurations
{
    public class LeaseConfiguration : EntityTypeConfiguration<Lease>
    {
        public LeaseConfiguration()
        {
            HasKey(l => l.Id);

            HasRequired(r => r.Order)
                .WithOptional(o => o.Lease)
                .WillCascadeOnDelete(false);

        }
    }
}
