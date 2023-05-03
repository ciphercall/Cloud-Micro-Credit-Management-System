namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate6 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MCR_MASTER",
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
                        Memberid = c.Long(),
                        Schemeid = c.String(),
                        Internid = c.Long(),
                        Debitamt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Creditamt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remarks = c.String(),
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
                "dbo.MCR_MLOAN",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Transmy = c.String(nullable: false, maxLength: 128),
                        Transno = c.Long(nullable: false),
                        Transdt = c.DateTime(nullable: false),
                        Schemeid = c.String(),
                        Memberid = c.Long(),
                        Internid = c.Long(),
                        Loanamt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pchargert = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Pchargeamt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Schargert = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Schargeamt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Disburseamt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Riskfundrt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Riskfundamt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Collectamt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Interestrt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Schemeiqty = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Schemeefdt = c.DateTime(),
                        Schemeetdt = c.DateTime(),
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
                .PrimaryKey(t => new { t.Compid, t.Transmy, t.Transno });
            
            CreateTable(
                "dbo.MCR_MTRANS",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Transtp = c.String(nullable: false, maxLength: 128),
                        Transmy = c.String(nullable: false, maxLength: 128),
                        Transno = c.Long(nullable: false),
                        Transsl = c.Long(nullable: false),
                        Transdt = c.DateTime(nullable: false),
                        Transfor = c.String(),
                        Transmode = c.String(),
                        Glheadid = c.Long(),
                        Schemeid = c.String(),
                        Memberid = c.Long(),
                        Internid = c.Long(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                .PrimaryKey(t => new { t.Compid, t.Transtp, t.Transmy, t.Transno, t.Transsl });
            
            AlterColumn("dbo.MCR_COLLECT", "Transdt", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MCR_COLLECT", "Transdt", c => c.DateTime());
            DropTable("dbo.MCR_MTRANS");
            DropTable("dbo.MCR_MLOAN");
            DropTable("dbo.MCR_MASTER");
        }
    }
}
