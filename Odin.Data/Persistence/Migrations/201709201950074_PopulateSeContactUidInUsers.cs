using System.Linq;

namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateSeContactUidInUsers : DbMigration
    {
        public override void Up()
        {
            var context = new ApplicationDbContext();
            var users = context.Users.ToList();

            for (int i = 0; i < users.Count; i++)
                users[i].SeContactUid = i+1;

            context.SaveChanges();
        }
        
        public override void Down()
        {
        }
    }
}
