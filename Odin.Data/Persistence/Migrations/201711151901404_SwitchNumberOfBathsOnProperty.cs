namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwitchNumberOfBathsOnProperty : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Properties", "NumberOfBathrooms_Id", "dbo.NumberOfBathroomsTypes");
            DropIndex("dbo.Properties", new[] { "NumberOfBathrooms_Id" });
            AddColumn("dbo.Properties", "NumberOfBathrooms", c => c.Decimal(precision: 18, scale: 2));
            DropColumn("dbo.Properties", "NumberOfBathrooms_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Properties", "NumberOfBathrooms_Id", c => c.Byte());
            DropColumn("dbo.Properties", "NumberOfBathrooms");
            CreateIndex("dbo.Properties", "NumberOfBathrooms_Id");
            AddForeignKey("dbo.Properties", "NumberOfBathrooms_Id", "dbo.NumberOfBathroomsTypes", "Id");
        }
    }
}
