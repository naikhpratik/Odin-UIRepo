using System.Data.Entity.Migrations;

namespace MyDwellworks.Data.Persistence.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistance\Migrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {

        }
    }
}
