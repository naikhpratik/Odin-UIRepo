namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransfereeEmailUniqueIndex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Transferees", "Email", c => c.String(nullable: false, maxLength: 64));
            CreateIndex("dbo.Transferees", "Email", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Transferees", new[] { "Email" });
            AlterColumn("dbo.Transferees", "Email", c => c.String());
        }
    }
}
