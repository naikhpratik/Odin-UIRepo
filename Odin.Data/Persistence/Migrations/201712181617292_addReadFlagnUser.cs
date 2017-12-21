namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addReadFlagnUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "AuthorId", c => c.String());
            AddColumn("dbo.Messages", "IsRead", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "IsRead");
            DropColumn("dbo.Messages", "AuthorId");
        }
    }
}
