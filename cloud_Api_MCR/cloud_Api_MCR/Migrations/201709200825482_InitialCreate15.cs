namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MCR_MSCHEME", "Schemeefdt", c => c.DateTime());
            AddColumn("dbo.MCR_MSCHEME", "Schemeetdt", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MCR_MSCHEME", "Schemeetdt");
            DropColumn("dbo.MCR_MSCHEME", "Schemeefdt");
        }
    }
}
