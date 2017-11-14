using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Azure.Mobile.Server.Tables;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Odin.Data.Persistence.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        private static readonly string _odinConsultantUserName = "odinconsultant@dwellworks.com";
        private static readonly string _odinGscUserName = "odingsc@dwellworks.com";
        private static readonly string _odinPmUserName = "odinpm@dwellworks.com";
        private static readonly string _odinEeUserName = "odinee@dwellworks.com";
        private static readonly string _odinAdminUserName = "odinadmin@dwellworks.com";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Persistence\Migrations";
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }

        protected override void Seed(ApplicationDbContext context)
        {
            SeedInitialRoles(context);
            SeedInitialUsers(context);
            CreateOrderData(context);
            PopulateSeContactUids(context);
            SeedNumberOfBathrooms(context);
            SeedHousingTypes(context);
            SeedAreaTypes(context);
            SeedTransportationTypes(context);
            SeedDepositTypes(context);
            SeedBrokerFeeTypes(context);
            SeedHomeFindingPropertiesIfNone(context);
        }

        private void PopulateSeContactUids(ApplicationDbContext context)
        {
            
            var users = context.Users.Where(u => !u.SeContactUid.HasValue);
            if (users.Any())
            {
                var maxSeContactUid = context.Users.OrderByDescending(u => u.SeContactUid).FirstOrDefault()?.SeContactUid ?? 1;
                foreach (var user in users)
                {
                    user.SeContactUid = ++maxSeContactUid;
                }
                context.SaveChanges();
            }
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
                    orders[i].Services.Add(new Service()
                    {
                        ServiceType = context.ServiceTypes.First()
                    });
                    orders[i].Services.Add(new Service()
                    {
                        ServiceType = context.ServiceTypes.OrderByDescending(st=>st.Id).First()
                    });
                    context.Orders.Add(orders[i]);

                    var homeFinding = HomeFindingBuilder.New();
                    homeFinding.Id = orders[i].Id;
                    context.HomeFindings.Add(homeFinding);

                    orders[i].Children.Add(ChildBuilder.New());
                    orders[i].Children.Add(ChildBuilder.New());
                    orders[i].Pets.Add(PetBuilder.New());
                    orders[i].Pets.Add(PetBuilder.New());
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
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);


            var adminUser = userManager.FindByName(_odinAdminUserName);
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser()
                {
                    UserName = _odinAdminUserName,
                    FirstName = "Odin",
                    LastName = "Admin",
                    Email = _odinAdminUserName,
                    PhoneNumber = "4403184188"
                };
                var result = userManager.Create(newAdmin, "OdinOdin5$");
                userManager.AddToRole(newAdmin.Id, UserRoles.Admin);
            }

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

            var adminRole = roleManager.FindByName(UserRoles.Admin);
            if (adminRole == null)
            {
                adminRole = new IdentityRole(UserRoles.Admin);
                roleManager.Create(adminRole);
            }
        }

        private void SeedNumberOfBathrooms(ApplicationDbContext context)
        {
            
            if (!context.NumberOfBathrooms.Any(n => n.Id == 1))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() {Id = 1, Name = "0",SeValue = "0"});
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 2))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 2, Name = "1/2", SeValue = "1/2" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 3))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 3, Name = "1", SeValue = "1"});
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 4))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 4, Name = "1 1/2", SeValue = "1 1/2" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 5))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 5, Name = "2", SeValue = "2"});
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 6))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 6, Name = "2 1/2", SeValue = "2 1/2" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 7))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 7, Name = "3", SeValue = "3" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 8))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 8, Name = "3 1/2", SeValue = "3 1/2" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 9))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 9, Name = "4", SeValue = "4" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 10))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 10, Name = "4 1/2", SeValue = "4 1/2" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 11))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 11, Name = "5", SeValue = "5" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 12))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 12, Name = "5 1/2", SeValue = "5 1/2" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 13))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 13, Name = "5+", SeValue = "5+" });
            }

        }

        private void SeedHousingTypes(ApplicationDbContext context)
        {

            if (!context.HousingTypes.Any(h => h.Id == 1))
            {
                context.HousingTypes.Add(new HousingType() { Id = 1, Name = "House", SeValue = "HOUSE" });
            }

            if (!context.HousingTypes.Any(h => h.Id == 2))
            {
                context.HousingTypes.Add(new HousingType() { Id = 2, Name = "Town House", SeValue = "TOWNRH" });
            }

            if (!context.HousingTypes.Any(h => h.Id == 3))
            {
                context.HousingTypes.Add(new HousingType() { Id = 3, Name = "Apartment", SeValue = "APARTM" });
            }

            if (!context.HousingTypes.Any(h => h.Id == 4))
            {
                context.HousingTypes.Add(new HousingType() { Id = 4, Name = "Condo", SeValue = "CONDOM" });
            }
        }

        private void SeedAreaTypes(ApplicationDbContext context)
        {

            if (!context.AreaTypes.Any(a => a.Id == 1))
            {
                context.AreaTypes.Add(new AreaType() { Id = 1, Name = "Suburban"});
            }

            if (!context.AreaTypes.Any(a => a.Id == 2))
            {
                context.AreaTypes.Add(new AreaType() { Id = 2, Name = "Urban" });
            }

            if (!context.AreaTypes.Any(a => a.Id == 3))
            {
                context.AreaTypes.Add(new AreaType() { Id = 3, Name = "Rural" });
            }
        }

        private void SeedTransportationTypes(ApplicationDbContext context)
        {
            if (!context.TransportationTypes.Any(t => t.Id == 1))
            {
                context.TransportationTypes.Add(new TransportationType() { Id = 1, Name = "Automobile",SeValue = "AUTOMO"});
            }

            if (!context.TransportationTypes.Any(t => t.Id == 2))
            {
                context.TransportationTypes.Add(new TransportationType() { Id = 2, Name = "Public Transportation", SeValue = "PUBLIC" });
            }
        }

        private void SeedDepositTypes(ApplicationDbContext context)
        {
            if (!context.DepositTypes.Any(d => d.Id == 1))
            {
                context.DepositTypes.Add(new DepositType() { Id = 1, Name = "Individual", SeValue = "INDIVI" });
            }

            if (!context.DepositTypes.Any(d => d.Id == 2))
            {
                context.DepositTypes.Add(new DepositType() { Id = 2, Name = "Company", SeValue = "COMPAN" });
            }
        }

        private void SeedBrokerFeeTypes(ApplicationDbContext context)
        {
            if (!context.BrokerFeeTypes.Any(b => b.Id == 1))
            {
                context.BrokerFeeTypes.Add(new BrokerFeeType() { Id = 1, Name = "Individual", SeValue = "INDIVI" });
            }

            if (!context.BrokerFeeTypes.Any(b => b.Id == 2))
            {
                context.BrokerFeeTypes.Add(new BrokerFeeType() { Id = 2, Name = "Company", SeValue = "COMPAN" });
            }
        }


        private void SeedHomeFindingPropertiesIfNone(ApplicationDbContext context)
        {
            if (!context.HomeFindingProperties.Any())
            {
                DbSet<HomeFinding> homeFindings = context.HomeFindings;
                foreach (HomeFinding hf in homeFindings)
                {
                    hf.HomeFindingProperties = HomeFindingPropertiesBuilder.New();
                }

                context.SaveChanges();
            }
        }

    }
}
