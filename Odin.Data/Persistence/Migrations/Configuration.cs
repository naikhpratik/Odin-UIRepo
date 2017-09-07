using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Odin.Data.Persistence.Migrations
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
            CreateTransfereeAndOrderData(context);
        }

        private void CreateTransfereeAndOrderData(ApplicationDbContext context)
        {
            if (!context.Transferees.Any())
            {
                var dsc = context.Users.First(u => u.LastName == "Emser");
                int count = 10;
                var transferees = TransfereeBuilder.New(count);
                var orders = OrderBuilder.New(count);

                for (int i = 0; i < count; i++)
                {
                    transferees[i].Orders.Add(orders[i]);
                    context.Transferees.Add(transferees[i]);
                    var consultantAssignment = new ConsultantAssignment() {Order = orders[i], ConsultantId = dsc.Id};
                    context.ConsultantAssignments.Add(consultantAssignment);
                }

                context.SaveChanges();
            }
        }

        private void CreateRolesAndUsers(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var consultantRole = roleManager.FindByName(UserRoles.Consultant);
            if (consultantRole == null)
            {
                consultantRole = new IdentityRole(UserRoles.Consultant);
                roleManager.Create(consultantRole);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var consultantUser = userManager.FindByName("austin.emser@dwellworks.com");
            if (consultantUser == null)
            {
                var newConsultant = new ApplicationUser()
                {
                    UserName = "austin.emser@dwellworks.com",
                    FirstName = "Austin",
                    LastName = "Emser",
                    Email = "austin.emser@dwellworks.com",
                    PhoneNumber = "4403184188"
                };
                userManager.Create(newConsultant, "Consultant5$");
                userManager.AddToRole(newConsultant.Id, UserRoles.Consultant);
            }

            var gscRole = roleManager.FindByName(UserRoles.GlobalSupplyChain);
            if (gscRole == null)
            {
                gscRole = new IdentityRole(UserRoles.GlobalSupplyChain);
                roleManager.Create(gscRole);
            }

            var gscUser = userManager.FindByName("aemser@dwellworks.com");
            if (gscUser == null)
            {
                var newGsc = new ApplicationUser()
                {
                    UserName = "aemser@dwellworks.com",
                    FirstName = "George",
                    LastName = "Gsc",
                    Email="aemser@dwellworks.com",
                    PhoneNumber = "2166824239"
                };
                userManager.Create(newGsc, "Global5$");
                userManager.AddToRole(newGsc.Id,UserRoles.GlobalSupplyChain);
            }
        }
    }
}
