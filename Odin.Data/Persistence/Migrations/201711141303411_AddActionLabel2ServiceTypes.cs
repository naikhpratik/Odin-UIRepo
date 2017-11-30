namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActionLabel2ServiceTypes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServiceTypes", "ActionLabel", c => c.String());
            Sql("UPDATE ServiceTypes SET ActionLabel = 'Going to attend the ''Initial/Pre-Arrival Consultation''' WHERE Id = 1");
            Sql("UPDATE ServiceTypes SET ActionLabel = 'Receiving the ''Welcome Packet''' WHERE Id = 2");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceTypes", "ActionLabel");
        }
    }
}
