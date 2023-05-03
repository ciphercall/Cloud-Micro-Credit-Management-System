namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate13 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MCR_MLOAN", "Memberid", c => c.Long(nullable: false));
            AlterColumn("dbo.MCR_MLOAN", "Internid", c => c.Long(nullable: false));
            AlterColumn("dbo.MCR_MLOAN", "Schemeefdt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MCR_MLOAN", "Schemeetdt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MCR_MLOAN", "Schemeetdt", c => c.DateTime());
            AlterColumn("dbo.MCR_MLOAN", "Schemeefdt", c => c.DateTime());
            AlterColumn("dbo.MCR_MLOAN", "Internid", c => c.Long());
            AlterColumn("dbo.MCR_MLOAN", "Memberid", c => c.Long());
        }
    }
}
