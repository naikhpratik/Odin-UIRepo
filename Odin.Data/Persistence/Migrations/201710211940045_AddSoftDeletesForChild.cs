namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddSoftDeletesForChild : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Children", "Deleted", c => c.Boolean(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: "Deleted", newValue: null)
                    },
                }));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Children", "Deleted", c => c.Boolean(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "ServiceTableColumn",
                        new AnnotationValues(oldValue: null, newValue: "Deleted")
                    },
                }));
        }
    }
}
