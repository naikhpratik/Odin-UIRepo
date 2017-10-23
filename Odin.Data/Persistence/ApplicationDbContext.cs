using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Azure.Mobile.Server.Tables;
using Odin.Data.Core.Models;
using Odin.Data.Persistence.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;

namespace Odin.Data.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Consultant> Consultants { get; set; }
        public virtual DbSet<Transferee> Transferees { get; set; }
        public virtual DbSet<Child> Children { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new RentConfiguration());

            //modelBuilder.Entity<Child>()
            //    .Map(m => m.Requires("Deleted").HasValue(false))
            //    .Ignore(m => m.Deleted);

            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));

            base.OnModelCreating(modelBuilder);
        }

        //Used for soft deletes. Source: https://putshello.wordpress.com/2014/08/20/entity-framework-soft-deletes-are-easy/

        //public override int SaveChanges()
        //{
        //    foreach (var entry in ChangeTracker.Entries().Where(e => e.Entity is MobileTable && ((MobileTable)e.Entity).Deleted).ToList())
        //    {
        //        SoftDelete(entry);
        //    }

        //    return base.SaveChanges();
        //}

        private void SoftDelete(DbEntityEntry entry)
        {
            Type entryEntityType = entry.Entity.GetType();

            string tableName = GetTableName(entryEntityType);
            string primaryKeyName = GetPrimaryKeyName(entryEntityType);

            string sql =
                string.Format(
                    "UPDATE {0} SET Deleted = 1 WHERE {1} = @id",
                    tableName, primaryKeyName);

            Database.ExecuteSqlCommand(
                sql,
                new SqlParameter("@id", entry.OriginalValues[primaryKeyName]));
        }

        private static Dictionary<Type, EntitySetBase> _mappingCache = new Dictionary<Type, EntitySetBase>();

        private string GetTableName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);

            return string.Format("[{0}].[{1}]",
                es.MetadataProperties["Schema"].Value,
                es.MetadataProperties["Table"].Value);
        }

        private string GetPrimaryKeyName(Type type)
        {
            EntitySetBase es = GetEntitySet(type);

            return es.ElementType.KeyMembers[0].Name;
        }

        private EntitySetBase GetEntitySet(Type type)
        {
            if (!_mappingCache.ContainsKey(type))
            {
                ObjectContext octx = ((IObjectContextAdapter)this).ObjectContext;

                string typeName = ObjectContext.GetObjectType(type).Name;

                var es = octx.MetadataWorkspace
                    .GetItemCollection(DataSpace.SSpace)
                    .GetItems<EntityContainer>()
                    .SelectMany(c => c.BaseEntitySets
                        .Where(e => e.Name == typeName))
                    .FirstOrDefault();

                if (es == null)
                    throw new ArgumentException("Entity type not found in GetTableName", typeName);

                _mappingCache.Add(type, es);
            }

            return _mappingCache[type];
        }
    }
}