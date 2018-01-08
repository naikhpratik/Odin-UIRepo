using Odin.Data.Core.Models;

namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInviteStatusToTransferee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "InviteStatus", c => c.String(defaultValue: InviteStatus.NotYetInvited));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "InviteStatus");
        }
    }
}
