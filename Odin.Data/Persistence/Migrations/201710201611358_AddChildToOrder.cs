namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChildToOrder : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Children",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Age = c.Int(nullable: false),
                        Grade = c.Int(nullable: false),
                        OrderId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .Index(t => t.OrderId);
            
            AddColumn("dbo.Orders", "SpouseName", c => c.String());
            AddColumn("dbo.Orders", "SpouseVisaType", c => c.String());
            AddColumn("dbo.Orders", "ChildrenEducationPreferences", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Children", "OrderId", "dbo.Orders");
            DropIndex("dbo.Children", new[] { "OrderId" });
            DropColumn("dbo.Orders", "ChildrenEducationPreferences");
            DropColumn("dbo.Orders", "SpouseVisaType");
            DropColumn("dbo.Orders", "SpouseName");
            DropTable("dbo.Children");
        }
    }
}
