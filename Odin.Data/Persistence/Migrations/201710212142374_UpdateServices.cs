namespace Odin.Data.Persistence.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UpdateServices : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Services", "Selected", c => c.Boolean(nullable: false));
            AddColumn("dbo.ServiceTypes", "Category", c => c.Int(nullable: false));

            //Sql("UPDATE ServiceTypes SET Category = 1 WHERE Id = 1");
            //Sql("UPDATE ServiceTypes SET Category = 2 WHERE Id = 2");
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServiceTypes", "Category");
            DropColumn("dbo.Services", "Selected");
        }
    }
}
