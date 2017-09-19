using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Odin.Data.Core.Models;

namespace Odin.Data.Persistence.EntityConfigurations
{
    public class TransfereeConfiguration : EntityTypeConfiguration<Transferee>
    {
        public TransfereeConfiguration()
        {
            Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(64)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName,
                    new IndexAnnotation(new IndexAttribute("IX_Email") {IsUnique = true}));
        }
    }
}
