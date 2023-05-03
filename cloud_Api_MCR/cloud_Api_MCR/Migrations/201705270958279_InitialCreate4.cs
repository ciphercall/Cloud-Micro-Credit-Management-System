namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.GL_STRANS", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.GL_STRANS", "Amount", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
