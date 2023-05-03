namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GL_ACCHARMST", newName: "GL_ACCHARTMST");
            DropPrimaryKey("dbo.GL_ACCHART");
            AddColumn("dbo.GL_ACCHART", "Accountcd", c => c.Long(nullable: false));
            AddColumn("dbo.GL_ACCHART", "Accountnm", c => c.String());
            AddPrimaryKey("dbo.GL_ACCHART", new[] { "Compid", "Headcd", "Accountcd" });
            DropColumn("dbo.GL_ACCHART", "Headnm");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GL_ACCHART", "Headnm", c => c.String());
            DropPrimaryKey("dbo.GL_ACCHART");
            DropColumn("dbo.GL_ACCHART", "Accountnm");
            DropColumn("dbo.GL_ACCHART", "Accountcd");
            AddPrimaryKey("dbo.GL_ACCHART", new[] { "Compid", "Headcd" });
            RenameTable(name: "dbo.GL_ACCHARTMST", newName: "GL_ACCHARMST");
        }
    }
}
