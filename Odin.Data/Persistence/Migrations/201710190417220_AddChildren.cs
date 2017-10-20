namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChildren : DbMigration
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
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Children", "OrderId", "dbo.Orders");
            DropIndex("dbo.Children", new[] { "OrderId" });
            DropTable("dbo.Children");
        }
    }
}
