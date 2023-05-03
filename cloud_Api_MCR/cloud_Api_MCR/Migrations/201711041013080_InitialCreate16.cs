namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MCR_SCHEME", "GlIncomehid", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MCR_SCHEME", "GlIncomehid");
        }
    }
}
