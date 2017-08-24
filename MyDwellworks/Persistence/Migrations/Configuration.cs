using System.Data.Entity.Migrations;

namespace MyDwellworks.Persistence.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<MyDwellworks.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistance\Migrations";
        }

        protected override void Seed(MyDwellworks.Models.ApplicationDbContext context)
        {

        }
    }
}
