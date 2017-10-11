namespace Odin.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SeedServiceTypes : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO ServiceTypes (Name) VALUES ('Initial/Pre-Arrival Consultation')");
            Sql("INSERT INTO ServiceTypes (Name) VALUES ('Welcome Packet')");
        }

        public override void Down()
        {
            Sql("DELETE FROM Genres WHERE Name IN ('Initial/Pre-Arrival Consultation','Welcome Packet')");
        }
    }
}
