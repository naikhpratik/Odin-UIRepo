// <auto-generated />
namespace Odin.Data.Persistence.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.1.3-40302")]
    public sealed partial class AddCreatedByToNotifications : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddCreatedByToNotifications));
        
        string IMigrationMetadata.Id
        {
            get { return "201801161919330_AddCreatedByToNotifications"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
