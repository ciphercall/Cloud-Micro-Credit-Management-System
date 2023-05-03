namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MCR_AREA",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Branchid = c.Long(nullable: false),
                        Areaid = c.Long(nullable: false),
                        Areanm = c.String(),
                        Authpnm = c.String(),
                        Fwid = c.Long(),
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
                .PrimaryKey(t => new { t.Compid, t.Branchid, t.Areaid });
            
            CreateTable(
                "dbo.MCR_BRANCH",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Branchid = c.Long(nullable: false),
                        Branchnm = c.String(),
                        Address = c.String(),
                        Contactno = c.String(),
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
                .PrimaryKey(t => new { t.Compid, t.Branchid });
            
            CreateTable(
                "dbo.MCR_COLLECT",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Transmy = c.String(nullable: false, maxLength: 128),
                        Transno = c.Long(nullable: false),
                        Transsl = c.Long(nullable: false),
                        Transdt = c.DateTime(),
                        Branchid = c.Long(),
                        Fwid = c.Long(),
                        Schemeid = c.String(),
                        Memberid = c.Long(),
                        Internid = c.Long(),
                        Amount = c.Decimal(precision: 18, scale: 2),
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
                .PrimaryKey(t => new { t.Compid, t.Transmy, t.Transno, t.Transsl });
            
            CreateTable(
                "dbo.MCR_MEMBER",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Macgid = c.Long(nullable: false),
                        Memberid = c.Long(nullable: false),
                        Membernm = c.String(),
                        Guardiannm = c.String(),
                        Mothernm = c.String(),
                        Addrpre = c.String(),
                        Addrper = c.String(),
                        Mobno1 = c.String(),
                        Mobno2 = c.String(),
                        Dob = c.DateTime(),
                        Age = c.String(),
                        Nationality = c.String(),
                        Nid = c.String(),
                        Religion = c.String(),
                        Occupation = c.String(),
                        Refnm = c.String(),
                        Refmid = c.Long(),
                        Refcno = c.String(),
                        Sharecno = c.String(),
                        Sharecdt = c.DateTime(),
                        Areaid = c.Long(),
                        Branchid = c.Long(),
                        Acopdt = c.DateTime(),
                        Accldt = c.DateTime(),
                        Status = c.String(),
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
                .PrimaryKey(t => new { t.Compid, t.Macgid, t.Memberid });
            
            CreateTable(
                "dbo.MCR_MNOMINEE",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Memberid = c.Long(nullable: false),
                        Nomineeid = c.Long(nullable: false),
                        Nomineenm = c.String(),
                        Guardiannm = c.String(),
                        Mothernm = c.String(),
                        Age = c.String(),
                        Relation = c.String(),
                        Npcnt = c.Decimal(precision: 18, scale: 2),
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
                .PrimaryKey(t => new { t.Compid, t.Memberid, t.Nomineeid });
            
            CreateTable(
                "dbo.MCR_MSCHEME",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Memberid = c.Long(nullable: false),
                        Schemesl = c.Long(nullable: false),
                        Schemeid = c.String(),
                        Internid = c.Long(),
                        Remarks = c.String(),
                        Status = c.String(),
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
                .PrimaryKey(t => new { t.Compid, t.Memberid, t.Schemesl });
            
            CreateTable(
                "dbo.MCR_SCHEME",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Schemeid = c.String(nullable: false, maxLength: 128),
                        Schemecd = c.Long(),
                        Schemetp = c.String(),
                        Glheadid = c.Long(),
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
                .PrimaryKey(t => new { t.Compid, t.Schemeid });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MCR_SCHEME");
            DropTable("dbo.MCR_MSCHEME");
            DropTable("dbo.MCR_MNOMINEE");
            DropTable("dbo.MCR_MEMBER");
            DropTable("dbo.MCR_COLLECT");
            DropTable("dbo.MCR_BRANCH");
            DropTable("dbo.MCR_AREA");
        }
    }
}
