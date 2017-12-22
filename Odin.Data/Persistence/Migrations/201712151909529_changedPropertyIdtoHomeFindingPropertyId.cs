namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changedPropertyIdtoHomeFindingPropertyId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Messages", name: "PropertyId", newName: "HomeFindingPropertyId");
            RenameIndex(table: "dbo.Messages", name: "IX_PropertyId", newName: "IX_HomeFindingPropertyId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Messages", name: "IX_HomeFindingPropertyId", newName: "IX_PropertyId");
            RenameColumn(table: "dbo.Messages", name: "HomeFindingPropertyId", newName: "PropertyId");
        }
    }
}
