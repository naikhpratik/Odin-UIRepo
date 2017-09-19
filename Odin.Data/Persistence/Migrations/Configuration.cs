using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Odin.Data.Persistence.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistence\Migrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            SeedInitialRolesAndUsers(context);
            CreateOrderData(context);
        }

        private void CreateOrderData(ApplicationDbContext context)
        {
            if (!context.Orders.Any())
            {
                var dsc = context.Users.First(u => u.LastName.Equals("Emser"));
                var pm = context.Users.First(u => u.UserName.Equals("odin-pm@dwellworks.com"));

                int count = 10;
                var orders = OrderBuilder.New(count);

                for (int i = 0; i < count; i++)
                {
                    orders[i].Consultant = dsc;
                    orders[i].ProgramManager = pm;
                    orders[i].Transferee = CreateRandomTransferee(context);
                    context.Orders.Add(orders[i]);
                }

                context.SaveChanges();
            }
        }

        private ApplicationUser CreateRandomTransferee(ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            var user = ApplicationUserBuilder.New();
            userManager.Create(user, "Transferee5$");
            userManager.AddToRole(user.Id, UserRoles.Transferee);
            return user;
        }

        private void SeedInitialRolesAndUsers(ApplicationDbContext context)
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

            var odinConsultantName = "odin-consultant@dwellworks.com";
            var consultantUser = userManager.FindByName(odinConsultantName);
            if (consultantUser == null)
            {
                var newConsultant = new ApplicationUser()
                {
                    UserName = odinConsultantName,
                    FirstName = "Odin",
                    LastName = "Consultant",
                    Email = odinConsultantName,
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

            var odinGscName = "odin-gsc@dwellworks.com";
            var gscUser = userManager.FindByName(odinGscName);
            if (gscUser == null)
            {
                var newGsc = new ApplicationUser()
                {
                    UserName = odinGscName,
                    FirstName = "George",
                    LastName = "Gsc",
                    Email= odinGscName,
                    PhoneNumber = "2166824239"
                };
                userManager.Create(newGsc, "Global5$");
                userManager.AddToRole(newGsc.Id,UserRoles.GlobalSupplyChain);
            }

            var pmRole = roleManager.FindByName(UserRoles.ProgramManager);
            if (pmRole == null)
            {
                pmRole = new IdentityRole(UserRoles.ProgramManager);
                roleManager.Create(pmRole);
            }

            var pmUser = userManager.FindByName("odin-pm@dwellworks.com");
            if (pmUser == null)
            {
                var newPm = new ApplicationUser()
                {
                    UserName = "odin-pm@dwellworks.com",
                    FirstName = "Odin",
                    LastName = "Pm",
                    Email = "odin-pm@dwellworks.com",
                    PhoneNumber = "2166824239"
                };
                userManager.Create(newPm, "OdinPm5$");
                userManager.AddToRole(newPm.Id, UserRoles.ProgramManager);
            }

            var eeRole = roleManager.FindByName(UserRoles.Transferee);
            if (eeRole == null)
            {
                eeRole = new IdentityRole(UserRoles.Transferee);
                roleManager.Create(eeRole);
            }

            var eeUser = userManager.FindByName("odin-ee@dwellworks.com");
            if (eeUser == null)
            {
                var newEe = new ApplicationUser()
                {
                    UserName = "odin-ee@dwellworks.com",
                    Email = "odin-ee@dwellworks.com",
                    FirstName = "Odin",
                    LastName = "Ee",
                    PhoneNumber = "2166824239"
                };
                userManager.Create(newEe, "OdinEe5$");
                userManager.AddToRole(newEe.Id, UserRoles.Transferee);
            }
        }
    }
}
