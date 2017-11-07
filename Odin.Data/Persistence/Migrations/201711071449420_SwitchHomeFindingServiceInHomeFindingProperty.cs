namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwitchHomeFindingServiceInHomeFindingProperty : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.HomeFindingProperties", name: "HomeFindingService_Id", newName: "HomeFinding_Id");
            RenameIndex(table: "dbo.HomeFindingProperties", name: "IX_HomeFindingService_Id", newName: "IX_HomeFinding_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.HomeFindingProperties", name: "IX_HomeFinding_Id", newName: "IX_HomeFindingService_Id");
            RenameColumn(table: "dbo.HomeFindingProperties", name: "HomeFinding_Id", newName: "HomeFindingService_Id");
        }
    }
}
