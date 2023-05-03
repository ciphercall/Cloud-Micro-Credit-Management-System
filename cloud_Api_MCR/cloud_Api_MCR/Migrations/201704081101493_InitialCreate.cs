namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ASL_COMPANY",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Compnm = c.String(),
                        Address = c.String(),
                        Address2 = c.String(),
                        Contactno = c.String(),
                        Emailid = c.String(),
                        Webid = c.String(),
                        Status = c.String(),
                        Emailidp = c.String(),
                        Emailpwp = c.String(),
                        Smssendernm = c.String(),
                        Smsidp = c.String(),
                        Smspwp = c.String(),
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
                .PrimaryKey(t => t.Compid);
            
            CreateTable(
                "dbo.ASL_DELETE",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Compid = c.Long(nullable: false),
                        Userid = c.Long(nullable: false),
                        Delslno = c.Long(nullable: false),
                        Deldate = c.DateTime(),
                        Deltime = c.String(),
                        Delipno = c.String(),
                        Delltude = c.String(),
                        Tableid = c.String(),
                        Deldata = c.String(),
                        Userpc = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ASL_LOG",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Compid = c.Long(nullable: false),
                        Userid = c.Long(nullable: false),
                        Logtype = c.String(),
                        Logslno = c.Long(nullable: false),
                        Logdate = c.DateTime(),
                        Logtime = c.String(),
                        Logipno = c.String(),
                        Logltude = c.String(),
                        Tableid = c.String(),
                        Logdata = c.String(),
                        Userpc = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ASL_MENU",
                c => new
                    {
                        Moduleid = c.String(nullable: false, maxLength: 128),
                        Menutp = c.String(nullable: false, maxLength: 128),
                        Menuid = c.String(nullable: false, maxLength: 128),
                        Serial = c.Long(nullable: false),
                        Menunm = c.String(),
                        Actionname = c.String(),
                        Controllername = c.String(),
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
                .PrimaryKey(t => new { t.Moduleid, t.Menutp, t.Menuid });
            
            CreateTable(
                "dbo.ASL_MENUMST",
                c => new
                    {
                        Moduleid = c.String(nullable: false, maxLength: 128),
                        Modulenm = c.String(),
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
                .PrimaryKey(t => t.Moduleid);
            
            CreateTable(
                "dbo.ASL_ROLE",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Userid = c.Long(nullable: false),
                        Moduleid = c.String(nullable: false, maxLength: 128),
                        Menutp = c.String(nullable: false, maxLength: 128),
                        Menuid = c.String(nullable: false, maxLength: 128),
                        Serial = c.Long(nullable: false),
                        Status = c.String(),
                        Insertr = c.String(),
                        Updater = c.String(),
                        Deleter = c.String(),
                        Menuname = c.String(),
                        Actionname = c.String(),
                        Controllername = c.String(),
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
                .PrimaryKey(t => new { t.Compid, t.Userid, t.Moduleid, t.Menutp, t.Menuid });
            
            CreateTable(
                "dbo.ASL_TOKEN",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Userid = c.Long(nullable: false),
                        Token = c.String(),
                        ExpireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Compid, t.Userid });
            
            CreateTable(
                "dbo.ASL_USERCO",
                c => new
                    {
                        Compid = c.Long(nullable: false),
                        Userid = c.Long(nullable: false),
                        Usernm = c.String(),
                        Deptnm = c.String(),
                        Optp = c.String(),
                        Address = c.String(),
                        Mobno = c.String(),
                        Emailid = c.String(),
                        Loginby = c.String(),
                        Loginid = c.String(),
                        Loginpw = c.String(),
                        Timefr = c.String(),
                        Timeto = c.String(),
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
                .PrimaryKey(t => new { t.Compid, t.Userid });
            
            CreateTable(
                "dbo.GL_ACCHARMST",
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.GL_ACCHARMST");
            DropTable("dbo.ASL_USERCO");
            DropTable("dbo.ASL_TOKEN");
            DropTable("dbo.ASL_ROLE");
            DropTable("dbo.ASL_MENUMST");
            DropTable("dbo.ASL_MENU");
            DropTable("dbo.ASL_LOG");
            DropTable("dbo.ASL_DELETE");
            DropTable("dbo.ASL_COMPANY");
        }
    }
}
