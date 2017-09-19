namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTransfereeAndConsultantAssignmentEntities : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ConsultantAssignments", "ConsultantId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ConsultantAssignments", "OrderId", "dbo.Orders");
            DropIndex("dbo.ConsultantAssignments", new[] { "OrderId" });
            DropIndex("dbo.ConsultantAssignments", new[] { "ConsultantId" });
            DropIndex("dbo.Orders", new[] { "TransfereeId" });
            DropIndex("dbo.Orders", new[] { "ProgramManagerId" });
            DropIndex("dbo.Transferees", new[] { "Email" });
            Sql("delete from dbo.AspNetUserRoles");
            Sql("delete from dbo.AspNetRoles");
            Sql("delete from dbo.AspNetUsers");
            Sql("delete from dbo.Rents");
            Sql("delete from dbo.Orders");
            AddColumn("dbo.Orders", "ConsultantId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Orders", "TransfereeId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Orders", "ProgramManagerId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Orders", "TransfereeId");
            CreateIndex("dbo.Orders", "ProgramManagerId");
            CreateIndex("dbo.Orders", "ConsultantId");
            AddForeignKey("dbo.Orders", "ConsultantId", "dbo.AspNetUsers", "Id");
            DropTable("dbo.ConsultantAssignments");
            DropTable("dbo.Transferees");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Transferees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 64),
                        FirstName = c.String(),
                        LastName = c.String(),
                        SpouseName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConsultantAssignments",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                        ConsultantId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.OrderId, t.ConsultantId });
            
            DropForeignKey("dbo.Orders", "ConsultantId", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "ConsultantId" });
            DropIndex("dbo.Orders", new[] { "ProgramManagerId" });
            DropIndex("dbo.Orders", new[] { "TransfereeId" });
            AlterColumn("dbo.Orders", "ProgramManagerId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Orders", "TransfereeId", c => c.Int(nullable: false));
            DropColumn("dbo.Orders", "ConsultantId");
            CreateIndex("dbo.Transferees", "Email", unique: true);
            CreateIndex("dbo.Orders", "ProgramManagerId");
            CreateIndex("dbo.Orders", "TransfereeId");
            CreateIndex("dbo.ConsultantAssignments", "ConsultantId");
            CreateIndex("dbo.ConsultantAssignments", "OrderId");
            AddForeignKey("dbo.ConsultantAssignments", "OrderId", "dbo.Orders", "Id");
            AddForeignKey("dbo.ConsultantAssignments", "ConsultantId", "dbo.AspNetUsers", "Id");
        }
    }
}
