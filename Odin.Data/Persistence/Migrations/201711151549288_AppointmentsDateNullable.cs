namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AppointmentsDateNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "ScheduledDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "ScheduledDate", c => c.DateTime(nullable: false));
        }
    }
}
