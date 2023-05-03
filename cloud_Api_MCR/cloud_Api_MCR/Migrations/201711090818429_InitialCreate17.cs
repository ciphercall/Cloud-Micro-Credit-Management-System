namespace cloud_Api_MCR.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ASL_PCONTACTS",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Compid = c.Long(nullable: false),
                        GroupId = c.Long(nullable: false),
                        Name = c.String(),
                        Email = c.String(),
                        MobNo1 = c.String(),
                        MobNo2 = c.String(),
                        Address = c.String(),
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
                .PrimaryKey(t => new { t.Id, t.Compid });
            
            CreateTable(
                "dbo.ASL_PEMAIL",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Compid = c.Long(nullable: false),
                        Transyy = c.Long(nullable: false),
                        TransNo = c.Long(nullable: false),
                        TransDt = c.DateTime(),
                        EmailId = c.String(maxLength: 100),
                        EmailSubject = c.String(),
                        BodyMsg = c.String(),
                        Status = c.String(maxLength: 7),
                        SentTm = c.DateTime(),
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
                .PrimaryKey(t => new { t.Id, t.Compid, t.Transyy, t.TransNo });
            
            CreateTable(
                "dbo.ASL_PGROUPS",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Compid = c.Long(nullable: false),
                        GroupId = c.Long(nullable: false),
                        GroupNm = c.String(),
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
                .PrimaryKey(t => new { t.Id, t.Compid, t.GroupId });
            
            CreateTable(
                "dbo.ASL_PSMS",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Compid = c.Long(nullable: false),
                        Transyy = c.Long(nullable: false),
                        TransNo = c.Long(nullable: false),
                        TransDt = c.DateTime(),
                        MobNo = c.String(maxLength: 13),
                        Language = c.String(maxLength: 3),
                        Message = c.String(maxLength: 160),
                        Status = c.String(maxLength: 7),
                        SentTm = c.DateTime(),
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
                .PrimaryKey(t => new { t.Id, t.Compid, t.Transyy, t.TransNo });
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ASL_PSMS");
            DropTable("dbo.ASL_PGROUPS");
            DropTable("dbo.ASL_PEMAIL");
            DropTable("dbo.ASL_PCONTACTS");
        }
    }
}
