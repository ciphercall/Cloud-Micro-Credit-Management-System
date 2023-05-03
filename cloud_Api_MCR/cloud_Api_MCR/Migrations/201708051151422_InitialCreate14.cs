namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MCR_MTRANS", "Schemeid2", c => c.String());
            AddColumn("dbo.MCR_MTRANS", "Memberid2", c => c.Long());
            AddColumn("dbo.MCR_MTRANS", "Internid2", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MCR_MTRANS", "Internid2");
            DropColumn("dbo.MCR_MTRANS", "Memberid2");
            DropColumn("dbo.MCR_MTRANS", "Schemeid2");
        }
    }
}
