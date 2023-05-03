namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GL_ACCHART",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Headcd = c.Long(nullable: false),
                        Headtp = c.Int(nullable: false),
                        Headnm = c.String(),
                        Remarks = c.String(),
                        Userpc = c.String(),
                        Insuserid = c.Long(),
                        Instime = c.DateTime(),
                        Insipno = c.String(),
                        Insltude = c.String(),
                        Upduserid = c.Long(),
                        Updtime = c.DateTime(),
                        Updipno = c.String(),
                        Updltude = c.String(),
                    })
                .PrimaryKey(t => new { t.Compid, t.Headcd });
            
            CreateTable(
                "dbo.GL_COSTPMST",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Costcid = c.Long(nullable: false),
                        Costcnm = c.String(),
                        Userpc = c.String(),
                        Insuserid = c.Long(),
                        Instime = c.DateTime(),
                        Insipno = c.String(),
                        Insltude = c.String(),
                        Upduserid = c.Long(),
                        Updtime = c.DateTime(),
                        Updipno = c.String(),
                        Updltude = c.String(),
                    })
                .PrimaryKey(t => new { t.Compid, t.Costcid });
            
            CreateTable(
                "dbo.GL_COSTPOOL",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Costcid = c.Long(nullable: false),
                        Costpid = c.Long(nullable: false),
                        Costpnm = c.String(),
                        Costpsid = c.String(),
                        Remarks = c.String(),
                        Userpc = c.String(),
                        Insuserid = c.Long(),
                        Instime = c.DateTime(),
                        Insipno = c.String(),
                        Insltude = c.String(),
                        Upduserid = c.Long(),
                        Updtime = c.DateTime(),
                        Updipno = c.String(),
                        Updltude = c.String(),
                    })
                .PrimaryKey(t => new { t.Compid, t.Costcid, t.Costpid });
            
            CreateTable(
                "dbo.GL_MASTER",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Transtp = c.String(nullable: false, maxLength: 128),
                        Transmy = c.String(nullable: false, maxLength: 128),
                        Transno = c.Long(nullable: false),
                        Transsl = c.Long(nullable: false),
                        Transdt = c.DateTime(nullable: false),
                        Transdrcr = c.String(),
                        Transfor = c.String(),
                        Transmode = c.String(),
                        Costpid = c.Long(),
                        Creditcd = c.Long(),
                        Debitcd = c.Long(),
                        Chequeno = c.String(),
                        Chequedt = c.DateTime(),
                        Remarks = c.String(),
                        Debitamt = c.Decimal(precision: 18, scale: 2),
                        Creditamt = c.Decimal(precision: 18, scale: 2),
                        Tableid = c.String(),
                        Userpc = c.String(),
                        Insuserid = c.Long(),
                        Instime = c.DateTime(),
                        Insipno = c.String(),
                        Insltude = c.String(),
                        Upduserid = c.Long(),
                        Updtime = c.DateTime(),
                        Updipno = c.String(),
                        Updltude = c.String(),
                    })
                .PrimaryKey(t => new { t.Compid, t.Transtp, t.Transmy, t.Transno, t.Transsl });
            
            CreateTable(
                "dbo.GL_STRANS",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Transtp = c.String(nullable: false, maxLength: 128),
                        Transmy = c.String(nullable: false, maxLength: 128),
                        Transno = c.Long(nullable: false),
                        Transdt = c.DateTime(nullable: false),
                        Transfor = c.String(),
                        Transmode = c.String(),
                        Costpid = c.Long(),
                        Creditcd = c.Long(),
                        Debitcd = c.Long(),
                        Chequeno = c.String(),
                        Chequedt = c.DateTime(),
                        Remarks = c.String(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Chqpayto = c.String(),
                        Userpc = c.String(),
                        Insuserid = c.Long(),
                        Instime = c.DateTime(),
                        Insipno = c.String(),
                        Insltude = c.String(),
                        Upduserid = c.Long(),
                        Updtime = c.DateTime(),
                        Updipno = c.String(),
                        Updltude = c.String(),
                    })
                .PrimaryKey(t => new { t.Compid, t.Transtp, t.Transmy, t.Transno });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GL_STRANS");
            DropTable("dbo.GL_MASTER");
            DropTable("dbo.GL_COSTPOOL");
            DropTable("dbo.GL_COSTPMST");
            DropTable("dbo.GL_ACCHART");
        }
    }
}
