namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate11 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MCR_COLLECT", "Branchid", c => c.Long(nullable: false));
            AlterColumn("dbo.MCR_COLLECT", "Fwid", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MCR_COLLECT", "Fwid", c => c.Long());
            AlterColumn("dbo.MCR_COLLECT", "Branchid", c => c.Long());
        }
    }
}
