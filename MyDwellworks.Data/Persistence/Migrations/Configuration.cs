using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MyDwellworks.Data.Core.Models;

namespace MyDwellworks.Data.Persistence.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistence\Migrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            CreateRolesAndUsers(context);
        }

        private void CreateRolesAndUsers(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var consultantRole = roleManager.FindByName("Consultant");
            if (consultantRole == null)
            {
                consultantRole = new IdentityRole("Consultant");
                roleManager.Create(consultantRole);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var consultantUser = userManager.FindByName("testconsultant");
            if (consultantUser == null)
            {
                var newConsultant = new ApplicationUser()
                {
                    UserName = "testconsultant",
                    FirstName = "Test",
                    LastName = "Consultant",
                    Email = "austin.emser@dwellworks.com",
                    PhoneNumber = "4403184188"
                };
                userManager.Create(newConsultant, "Consultant5$");
                userManager.AddToRole(newConsultant.Id, "Consultant");
            }
        }
    }
}
