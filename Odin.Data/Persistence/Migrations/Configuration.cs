using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Azure.Mobile.Server.Tables;
using Odin.Data.Builders;
using Odin.Data.Core.Models;
using System;
using System.Collections.Generic;
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
            //Note for Seeding types:  byte primary keys can be specified during insert.
            //int primary keys can't; no identity insert, just autoincrementing.
            SeedNumberOfBathrooms(context);
            SeedHousingTypes(context);
            SeedAreaTypes(context);
            SeedTransportationTypes(context);
            SeedDepositTypes(context);
            SeedBrokerFeeTypes(context);
            SeedServiceTypes(context);

            SeedInitialRoles(context);
            SeedInitialUsers(context);
            CreateOrderData(context);
            PopulateSeContactUids(context);
            SeedHomeFindingPropertiesIfNone(context);
            CreateOrderWithTransferee(context);
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
                        ServiceType = context.ServiceTypes.First(),
                        Selected = true
                    });
                    orders[i].Services.Add(new Service()
                    {
                        ServiceType = context.ServiceTypes.OrderByDescending(st => st.Id).First(),
                        Selected = true
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

        private void CreateOrderWithTransferee(ApplicationDbContext context)
        {
            if (!context.Orders.Any(o => o.Transferee.Email == "eeWithOrder@dwellworks.com"))
            {
                var dsc = context.Consultants.First(u => u.UserName.Equals(_odinConsultantUserName));
                var pm = context.Managers.First(u => u.UserName.Equals(_odinPmUserName));

                int count = 1;
                var orders = OrderBuilder.New(count);
               
                var userStore = new UserStore<Transferee>(context);
                var userManager = new UserManager<Transferee>(userStore);

                Transferee user;
                if (!context.Users.Any(u => u.Email == "eeWithOrder@dwellworks.com"))
                {
                    user = TransfereeBuilder.New();
                    user.Email = "eeWithOrder@dwellworks.com";
                    user.UserName = "eeWithOrder@dwellworks.com";
                    userManager.Create(user, "OdinOdin5$");
                    var transfereeStore = new UserStore<Transferee>(context);
                    var transfereeManager = new UserManager<Transferee>(transfereeStore);
                    transfereeManager.AddToRole(user.Id, UserRoles.Transferee);
                }
                else
                {
                    user = context.Transferees.Single<Transferee>(t => t.Email == "eeWithOrder@dwellworks.com");
                }

                orders[0].TrackingId = Guid.NewGuid().ToString().Substring(0,20);
                orders[0].Transferee = user;
                orders[0].Consultant = dsc;
                orders[0].ProgramManager = pm;
                orders[0].Services.Add(new Service()
                {
                    ServiceType = context.ServiceTypes.First(),
                    Selected = true
                });
                orders[0].Services.Add(new Service()
                {
                    ServiceType = context.ServiceTypes.OrderByDescending(st => st.Id).First(),
                    Selected = true
                });
                context.Orders.Add(orders[0]);

                var homeFinding = HomeFindingBuilder.New();
                homeFinding.Id = orders[0].Id;
                context.HomeFindings.Add(homeFinding);

                orders[0].Children.Add(ChildBuilder.New());
                orders[0].Children.Add(ChildBuilder.New());
                orders[0].Pets.Add(PetBuilder.New());
                orders[0].Pets.Add(PetBuilder.New());              
               
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
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 1, Name = "0", SeValue = "0" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 2))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 2, Name = "1/2", SeValue = "1/2" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 3))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 3, Name = "1", SeValue = "1" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 4))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 4, Name = "1 1/2", SeValue = "1 1/2" });
            }

            if (!context.NumberOfBathrooms.Any(n => n.Id == 5))
            {
                context.NumberOfBathrooms.Add(new NumberOfBathroomsType() { Id = 5, Name = "2", SeValue = "2" });
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

            context.SaveChanges();
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

            context.SaveChanges();
        }

        private void SeedAreaTypes(ApplicationDbContext context)
        {

            if (!context.AreaTypes.Any(a => a.Id == 1))
            {
                context.AreaTypes.Add(new AreaType() { Id = 1, Name = "Suburban" });
            }

            if (!context.AreaTypes.Any(a => a.Id == 2))
            {
                context.AreaTypes.Add(new AreaType() { Id = 2, Name = "Urban" });
            }

            if (!context.AreaTypes.Any(a => a.Id == 3))
            {
                context.AreaTypes.Add(new AreaType() { Id = 3, Name = "Rural" });
            }

            context.SaveChanges();
        }

        private void SeedTransportationTypes(ApplicationDbContext context)
        {
            if (!context.TransportationTypes.Any(t => t.Id == 1))
            {
                context.TransportationTypes.Add(new TransportationType() { Id = 1, Name = "Automobile", SeValue = "AUTOMO" });
            }

            if (!context.TransportationTypes.Any(t => t.Id == 2))
            {
                context.TransportationTypes.Add(new TransportationType() { Id = 2, Name = "Public Transportation", SeValue = "PUBLIC" });
            }

            context.SaveChanges();
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

            context.SaveChanges();
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

            context.SaveChanges();
        }

        private void SeedServiceTypes(ApplicationDbContext context)
        {
            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Initial/Pre-Arrival Consultation".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Initial/Pre-Arrival Consultation",
                    Category = ServiceCategory.InitialConsultation,
                    SortOrder = 1,
                    Default = (int)DefaultType.Domestic + (int)DefaultType.International,
                    ActionLabel = "Going to attend the 'Initial/Pre-Arrival Consultation'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Initial/Pre-Arrival Consultation".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 1;
                    type.Default = (int)DefaultType.Domestic + (int)DefaultType.International;

                }
                type.ActionLabel = "Going to attend the 'Initial/Pre-Arrival Consultation'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Welcome Packet".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Welcome Packet",
                    Category = ServiceCategory.WelcomePacket,
                    SortOrder = 2,
                    Default = (int)DefaultType.Domestic + (int)DefaultType.International,
                    ActionLabel = "Receiving the 'Welcome Packet'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Welcome Packet".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 2;
                    type.Default = (int)DefaultType.Domestic + (int)DefaultType.International;

                }
                type.ActionLabel = "Receiving the 'Welcome Packet'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Social Security Registration".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Social Security Registration",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 5,
                    Default = (int)DefaultType.International,
                    ActionLabel = "Going to register for 'Social Security Registration'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Social Security Registration".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 5;
                    type.Default = (int)DefaultType.International;

                }
                type.ActionLabel = "Going to register for 'Social Security Registration'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Colleges/Universities".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Colleges/Universities",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 19,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Going to explore 'Colleges/Universities'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Colleges/Universities".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 19;
                    type.Default = (int)DefaultType.No;

                }
                type.ActionLabel = "Going to explore 'Colleges/Universities'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Language Assistance".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Language Assistance",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 20,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Going to receive 'Language Assistance'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Language Assistance".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 20;
                    type.Default = (int)DefaultType.No;

                }
                type.ActionLabel = "Going to receive 'Language Assistance'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Telephone systems".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Telephone systems",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 21,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Going to compare 'Telephone systems'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Telephone systems".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 21;
                    type.Default = (int)DefaultType.No;

                }
                type.ActionLabel = "Going to compare 'Telephone systems'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Utility hook-up".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Utility hook-up",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 22,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Going to work on 'Utility hook-up'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Utility hook-up".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 22;
                    type.Default = (int)DefaultType.No;

                }
                type.ActionLabel = "Going to work on 'Utility hook-up'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Internet service providers".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Internet service providers",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 23,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Researching available 'Internet service providers'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Internet service providers".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 23;
                    type.Default = (int)DefaultType.No;

                }
                type.ActionLabel = "Researching available 'Internet service providers'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Furniture Rental".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Furniture Rental",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 24,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Going to look into 'Furniture Rental'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Furniture Rental".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 24;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Going to look into 'Furniture Rental'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Furniture Purchase".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Furniture Purchase",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 25,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Going to complete 'Furniture Purchase'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Furniture Purchase".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 25;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Going to complete 'Furniture Purchase'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Appliances(compatibility / purchase)".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Appliances(compatibility / purchase)",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 26,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Going to shop for 'Appliances(compatibility / purchase)'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Appliances(compatibility / purchase)".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 26;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Going to shop for 'Appliances(compatibility / purchase)'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Driving/auto info (licensing / driving schools)".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Driving/auto info (licensing / driving schools)",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 7,
                    Default = (int)DefaultType.International,
                    ActionLabel = "Going to look into 'Driving/auto info (licensing / driving schools)'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Driving/auto info (licensing / driving schools)".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 7;
                    type.Default = (int)DefaultType.International;
                }
                type.ActionLabel = "Going to look into 'Driving/auto info (licensing / driving schools)'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Transportation options".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Transportation options",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 27,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Going to look into 'Transportation options'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Transportation options".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 27;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Going to look into 'Transportation options'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Homeowner's services".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Homeowner's services",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 28,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Exploring various 'Homeowner's services'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Homeowner's services".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 28;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Exploring various 'Homeowner's services'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Insurance (auto / renter / homeowner)".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Insurance (auto / renter / homeowner)",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 29,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Shopping for 'Insurance (auto / renter / homeowner)'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Insurance (auto / renter / homeowner)".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 29;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Shopping for 'Insurance (auto / renter / homeowner)'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Medical/dental information".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Medical/dental information",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 30,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Researching 'Medical/dental information'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Medical/dental information".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 30;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Researching 'Medical/dental information'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Money issues/banking".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Money issues/banking",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 6,
                    Default = (int)DefaultType.International,
                    ActionLabel = "Looking into and resolving 'Money issues/banking'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Money issues/banking".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 6;
                    type.Default = (int)DefaultType.International;
                }
                type.ActionLabel = "Looking into and resolving 'Money issues/banking'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Mail services".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Mail services",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 31,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Finding 'Mail services'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Mail services".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 31;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Finding 'Mail services'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Domestic services(maid / cook / driver)".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Domestic services(maid / cook / driver)",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 32,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Finding and hiring 'Domestic services(maid / cook / driver)'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Domestic services(maid / cook / driver)".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 32;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Finding and hiring 'Domestic services(maid / cook / driver)'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Pets (care/licensing/etc.)".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Pets (care/licensing/etc.)",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 33,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Finding 'Pets (care/licensing/etc.)'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Pets (care/licensing/etc.)".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 33;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Finding 'Pets (care/licensing/etc.)'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Childcare".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Childcare",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 34,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Researching and finding 'Childcare'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Childcare".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 34;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Researching and finding 'Childcare'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Eldercare".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Eldercare",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 35,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Researching and finding convenient 'Eldercare'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Eldercare".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 35;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Researching and finding convenient 'Eldercare'";
            }

            /*Duplicate Fix*/
            if (context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Area Orientation/Overview".ToUpper()))
            {
                List<ServiceType> types = context.ServiceTypes.Where(s => s.Name.Trim().ToUpper() == "Area Orientation/Overview".ToUpper()).ToList();
                foreach (var type in types)
                {
                    context.ServiceTypes.Remove(type);
                }
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Area Orientation / Overview".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Area Orientation / Overview",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 3,
                    Default = (int)DefaultType.Domestic + (int)DefaultType.International,
                    ActionLabel = "Attending a briefing on 'Area Orientation / Overview'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Area Orientation / Overview".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 3;
                    type.Default = (int)DefaultType.Domestic + (int)DefaultType.International;
                }
                type.ActionLabel = "Attending a briefing on 'Area Orientation / Overview'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Housing/Neighborhoods".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Housing/Neighborhoods",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 8,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Researching and visiting 'Housing/Neighborhoods'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Housing/Neighborhoods".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 8;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Researching and visiting 'Housing/Neighborhoods'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Religious worship".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Religious worship",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 9,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Finding places for 'Religious worship'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Religious worship".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 9;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Finding places for 'Religious worship'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Shopping information".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Shopping information",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 10,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Researching 'Shopping information'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Shopping information".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 10;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Researching 'Shopping information'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Restaurants".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Restaurants",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 11,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Locating 'Restaurants'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Restaurants".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 11;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Locating 'Restaurants'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Arts and leisure facilities".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Arts and leisure facilities",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 12,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Locating local 'Arts and leisure facilities'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Arts and leisure facilities".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 12;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Locating local 'Arts and leisure facilities'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Clubs (social / health / recreational)".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Clubs (social / health / recreational)",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 13,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Locating local 'Clubs (social / health / recreational)'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Clubs (social / health / recreational)".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 13;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Locating local 'Clubs (social / health / recreational)'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Sports information".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Sports information",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 14,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Aquiring 'Sports information'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Sports information".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 14;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Aquiring 'Sports information'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Volunteer associations".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Volunteer associations",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 15,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Investigating 'Volunteer associations'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Volunteer associations".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 15;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Investigating 'Volunteer associations'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Library".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Library",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 16,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Locating the local 'Library'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Library".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 16;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Locating the local 'Library'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Emergency/police/fire".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Emergency/police/fire",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 17,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Locating the local 'Emergency/police/fire'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Emergency/police/fire".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 17;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Locating the local 'Emergency/police/fire'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Local/Regional/Government Holidays".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Local/Regional/Government Holidays",
                    Category = ServiceCategory.AreaOrientation,
                    SortOrder = 18,
                    Default = (int)DefaultType.No,
                    ActionLabel = "Marking the 'Local/Regional/Government Holidays'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Local/Regional/Government Holidays".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 18;
                    type.Default = (int)DefaultType.No;
                }
                type.ActionLabel = "Marking the 'Local/Regional/Government Holidays'";
            }

            if (!context.ServiceTypes.Any(s => s.Name.Trim().ToUpper() == "Settling In / Overview".ToUpper()))
            {
                context.ServiceTypes.Add(new ServiceType()
                {
                    Name = "Settling In / Overview",
                    Category = ServiceCategory.SettlingIn,
                    SortOrder = 4,
                    Default = (int)DefaultType.Domestic + (int)DefaultType.International,
                    ActionLabel = "Looking into 'Settling In / Overview'"
                });
            }
            else
            {
                ServiceType type = context.ServiceTypes.SingleOrDefault(s => s.Name.Trim().ToUpper() == "Settling In / Overview".ToUpper());
                if (type.SortOrder == 0)
                {
                    type.SortOrder = 4;
                    type.Default = (int)DefaultType.Domestic + (int)DefaultType.International;
                }
                type.ActionLabel = "Looking into 'Settling In / Overview'";
            }

            context.SaveChanges();
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
