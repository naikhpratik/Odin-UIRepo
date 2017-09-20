namespace Odin.Data.Persistence.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransfereeInviteEnabledToOrders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TransfereeInviteEnabled", c => c.Boolean(nullable: false,
                annotations: new Dictionary<string, AnnotationValues>
                {
                    { 
                        "Default",
                        new AnnotationValues(oldValue: null, newValue: "False")
                    },
                }));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "TransfereeInviteEnabled",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "Default", "False" },
                });
        }
    }
}
