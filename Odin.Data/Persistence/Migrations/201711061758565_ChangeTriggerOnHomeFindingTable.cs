namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTriggerOnHomeFindingTable : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TRIGGER [dbo].[TR_dbo_Rents_InsertUpdateDelete] ON [dbo].[HomeFindings] AFTER INSERT, UPDATE, DELETE AS BEGIN UPDATE [dbo].[HomeFindings] SET [dbo].[HomeFindings].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) FROM INSERTED WHERE inserted.[Id] = [dbo].[HomeFindings].[Id] END");
        }
        
        public override void Down()
        {
            Sql("ALTER TRIGGER [dbo].[TR_dbo_Rents_InsertUpdateDelete] ON [dbo].[HomeFindings] AFTER INSERT, UPDATE, DELETE AS BEGIN UPDATE [dbo].[Rents] SET [dbo].[Rents].[UpdatedAt] = CONVERT(DATETIMEOFFSET, SYSUTCDATETIME()) FROM INSERTED WHERE inserted.[Id] = [dbo].[Rents].[Id] END");
        }
    }
}
