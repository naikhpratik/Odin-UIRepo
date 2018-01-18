namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProgramNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ProgramNotes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ProgramNotes");
        }
    }
}
