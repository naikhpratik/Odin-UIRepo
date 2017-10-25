namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPetNotes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "PetNotes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "PetNotes");
        }
    }
}
