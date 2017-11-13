using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace Odin.Data.Persistence.EntityConfigurations
{
    public class HomeFindingPropertiesConfiguration : EntityTypeConfiguration<HomeFindingProperty>
    {
        public HomeFindingPropertiesConfiguration()
        {           
            HasRequired(hfp => hfp.Property)
                .WithMany()
                .WillCascadeOnDelete(true);
        }
    }
}
