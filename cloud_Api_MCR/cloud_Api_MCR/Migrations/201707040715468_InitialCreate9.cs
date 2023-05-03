namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate9 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MCR_SCHEME", "Glheadid", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MCR_SCHEME", "Glheadid", c => c.Long());
        }
    }
}
