using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Odin.Data.Core.Models;
using Odin.Data.Persistence;

namespace Odin.IntegrationTests
{
    [SetUpFixture]
    public class GlobalSetUp
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            MigrateDbToLatestVersion();

            Seed();
        }

        private static void MigrateDbToLatestVersion()
        {
            var configuration = new Odin.Data.Persistence.Migrations.Configuration();
            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }

        public void Seed()
        {
            var context = new ApplicationDbContext();

            if (context.Users.Any())
                return;

            context.Users.Add(new ApplicationUser { UserName = "user1", FirstName = "user1", Email = "user1@domain.com", PasswordHash = "-" });
            context.Users.Add(new ApplicationUser { UserName = "user2", FirstName = "user2", Email = "user1@domain.com", PasswordHash = "-" });
            context.SaveChanges();
        }
    }
}
