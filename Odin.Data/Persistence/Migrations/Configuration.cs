using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Odin.Data.Persistence.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private static readonly string _odinConsultantUserName = "odinconsultant@dwellworks.com";
        private static readonly string _odinGscUserName = "odingsc@dwellworks.com";
        private static readonly string _odinPmUserName = "odinpm@dwellworks.com";
        private static readonly string _odinEeUserName = "odinee@dwellworks.com";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistence\Migrations";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            SeedInitialRoles(context);
            SeedInitialUsers(context);
            CreateOrderData(context);
        }

        private void CreateOrderData(ApplicationDbContext context)
        {
            if (!context.Orders.Any())
            {
                var dsc = context.Consultants.First(u => u.UserName.Equals(_odinConsultantUserName));
                var pm = context.Managers.First(u => u.UserName.Equals(_odinPmUserName));

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

        private Transferee CreateRandomTransferee(ApplicationDbContext context)
        {
            var userStore = new UserStore<Transferee>(context);
            var userManager = new UserManager<Transferee>(userStore);

            var user = TransfereeBuilder.New();
            userManager.Create(user, "Transferee5$");
            return user;
        }

        private void SeedInitialUsers(ApplicationDbContext context)
        {
            var consultantStore = new UserStore<Consultant>(context);
            var consultantManager = new UserManager<Consultant>(consultantStore);

            var consultantUser = consultantManager.FindByName(_odinConsultantUserName);
            if (consultantUser == null)
            {
                var newConsultant = new Consultant()
                {
                    UserName = _odinConsultantUserName,
                    FirstName = "Odin",
                    LastName = "Consultant",
                    Email = _odinConsultantUserName,
                    PhoneNumber = "4403184188"
                };
                var result = consultantManager.Create(newConsultant, "OdinOdin5$");
                consultantManager.AddToRole(newConsultant.Id, UserRoles.Consultant);
            }


            var managerStore = new UserStore<Manager>(context);
            var managerManager = new UserManager<Manager>(managerStore);

            var gscUser = managerManager.FindByName(_odinGscUserName);
            if (gscUser == null)
            {
                var newGsc = new Manager()
                {
                    UserName = _odinGscUserName,
                    FirstName = "George",
                    LastName = "Gsc",
                    Email = _odinGscUserName,
                    PhoneNumber = "2166824239"
                };
                managerManager.Create(newGsc, "OdinOdin5$");
                managerManager.AddToRole(newGsc.Id, UserRoles.GlobalSupplyChain);
            }



            var pmUser = managerManager.FindByName(_odinPmUserName);
            if (pmUser == null)
            {
                var newPm = new Manager()
                {
                    UserName = _odinPmUserName,
                    FirstName = "Odin",
                    LastName = "Pm",
                    Email = _odinPmUserName,
                    PhoneNumber = "2166824239"
                };
                managerManager.Create(newPm, "OdinOdin5$");
                managerManager.AddToRole(newPm.Id, UserRoles.ProgramManager);
            }

            var transfereeStore = new UserStore<Transferee>(context);
            var transfereeManager = new UserManager<Transferee>(transfereeStore);

            var eeUser = transfereeManager.FindByName(_odinEeUserName);
            if (eeUser == null)
            {
                var newEe = new Transferee()
                {
                    UserName = _odinEeUserName,
                    Email = _odinEeUserName,
                    FirstName = "Odin",
                    LastName = "Ee",
                    PhoneNumber = "2166824239"
                };
                transfereeManager.Create(newEe, "OdinOdin5$");
                transfereeManager.AddToRole(newEe.Id, UserRoles.Transferee);
            }
        }

        private void SeedInitialRoles(ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var consultantRole = roleManager.FindByName(UserRoles.Consultant);
            if (consultantRole == null)
            {
                consultantRole = new IdentityRole(UserRoles.Consultant);
                roleManager.Create(consultantRole);
            }

            var gscRole = roleManager.FindByName(UserRoles.GlobalSupplyChain);
            if (gscRole == null)
            {
                gscRole = new IdentityRole(UserRoles.GlobalSupplyChain);
                roleManager.Create(gscRole);
            }

            var eeRole = roleManager.FindByName(UserRoles.Transferee);
            if (eeRole == null)
            {
                eeRole = new IdentityRole(UserRoles.Transferee);
                roleManager.Create(eeRole);
            }

            var pmRole = roleManager.FindByName(UserRoles.ProgramManager);
            if (pmRole == null)
            {
                pmRole = new IdentityRole(UserRoles.ProgramManager);
                roleManager.Create(pmRole);
            }
        }
    }
}
