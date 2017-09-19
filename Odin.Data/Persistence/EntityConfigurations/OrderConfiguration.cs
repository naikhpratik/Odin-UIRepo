using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
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
                .WithMany()
                .HasForeignKey(o => o.TransfereeId)
                .WillCascadeOnDelete(false);

            HasRequired(o => o.ProgramManager)
                .WithMany()
                .HasForeignKey(o => o.ProgramManagerId)
                .WillCascadeOnDelete(false);

            HasRequired(o => o.Consultant)
                .WithMany()
                .HasForeignKey(o => o.ConsultantId)
                .WillCascadeOnDelete(false);

            Property(o => o.TrackingId)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_TrackingID") {IsUnique = true}));
        }
    }
}
