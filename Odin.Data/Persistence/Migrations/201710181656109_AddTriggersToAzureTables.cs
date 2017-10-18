namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTriggersToAzureTables : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE TRIGGER [dbo].[TR_dbo_Orders_InsertUpdateDelete] ON [dbo].[Orders] AFTER INSERT, UPDATE, DELETE AS BEGIN UPDATE [dbo].[Orders] SET [dbo].[Orders].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) FROM INSERTED WHERE inserted.[Id] = [dbo].[Orders].[Id] END");
            Sql("ALTER TABLE [dbo].[Orders] ENABLE TRIGGER [TR_dbo_Orders_InsertUpdateDelete]");
            Sql("CREATE TRIGGER [dbo].[TR_dbo_Rents_InsertUpdateDelete] ON [dbo].[Rents] AFTER INSERT, UPDATE, DELETE AS BEGIN UPDATE [dbo].[Rents] SET [dbo].[Rents].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) FROM INSERTED WHERE inserted.[Id] = [dbo].[Rents].[Id] END");
            Sql("ALTER TABLE [dbo].[Rents] ENABLE TRIGGER [TR_dbo_Rents_InsertUpdateDelete]");
            Sql("CREATE TRIGGER [dbo].[TR_dbo_Services_InsertUpdateDelete] ON [dbo].[Services] AFTER INSERT, UPDATE, DELETE AS BEGIN UPDATE [dbo].[Services] SET [dbo].[Services].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) FROM INSERTED WHERE inserted.[Id] = [dbo].[Services].[Id] END");
            Sql("ALTER TABLE [dbo].[Services] ENABLE TRIGGER [TR_dbo_Services_InsertUpdateDelete]");
        }
        
        public override void Down()
        {
            Sql("DROP TRIGGER [dbo].[TR_dbo_Orders_InsertUpdateDelete];");
            Sql("DROP TRIGGER [dbo].[TR_dbo_Rents_InsertUpdateDelete];");
            Sql("DROP TRIGGER [dbo].[TR_dbo_Services_InsertUpdateDelete];");
        }
    }
}
