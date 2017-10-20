namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteOriginalChildTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Children", "OrderId", "dbo.Orders");
            DropIndex("dbo.Children", new[] { "OrderId" });
            DropTable("dbo.Children");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Children", "OrderId");
            AddForeignKey("dbo.Children", "OrderId", "dbo.Orders", "Id");
        }
    }
}
