namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate8 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.MCR_SCHEME");
            AlterColumn("dbo.MCR_SCHEME", "Schemeid", c => c.String());
            AlterColumn("dbo.MCR_SCHEME", "Schemecd", c => c.Long(nullable: false));
            AddPrimaryKey("dbo.MCR_SCHEME", new[] { "Compid", "Schemecd" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.MCR_SCHEME");
            AlterColumn("dbo.MCR_SCHEME", "Schemecd", c => c.Long());
            AlterColumn("dbo.MCR_SCHEME", "Schemeid", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.MCR_SCHEME", new[] { "Compid", "Schemeid" });
        }
    }
}
