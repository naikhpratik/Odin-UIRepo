namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CleanUpPhotos : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Photos", name: "PropertyId", newName: "Property_Id");
            RenameIndex(table: "dbo.Photos", name: "IX_PropertyId", newName: "IX_Property_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Photos", name: "IX_Property_Id", newName: "IX_PropertyId");
            RenameColumn(table: "dbo.Photos", name: "Property_Id", newName: "PropertyId");
        }
    }
}
