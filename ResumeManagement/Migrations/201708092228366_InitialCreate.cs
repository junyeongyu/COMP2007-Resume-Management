namespace ResumeManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CVLetters",
                c => new
                    {
                        CVLetterId = c.String(nullable: false, maxLength: 128, defaultValueSql: "newid()"),
                        Name = c.String(nullable: false, maxLength: 255),
                        Content = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                        EditDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                    })
                .PrimaryKey(t => t.CVLetterId);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        PackageId = c.String(nullable: false, maxLength: 128, defaultValueSql: "newid()"),
                        Name = c.String(nullable: false, maxLength: 255),
                        ResumeId = c.String(nullable: false, maxLength: 128),
                        CVLetterId = c.String(nullable: false, maxLength: 128),
                        CreateDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                        EditDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                    })
                .PrimaryKey(t => t.PackageId)
                .ForeignKey("dbo.Resumes", t => t.ResumeId)
                .ForeignKey("dbo.CVLetters", t => t.CVLetterId)
                .Index(t => t.ResumeId)
                .Index(t => t.CVLetterId);
            
            CreateTable(
                "dbo.Resumes",
                c => new
                    {
                        ResumeId = c.String(nullable: false, maxLength: 128, defaultValueSql: "newid()"),
                        Name = c.String(nullable: false, maxLength: 255),
                        ApplicantName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 200),
                        Phone = c.String(nullable: false, maxLength: 20),
                        Address = c.String(nullable: false, maxLength: 200),
                        Summary = c.String(nullable: false, maxLength: 3000),
                        Skill = c.String(nullable: false, maxLength: 3000),
                        WorkExperience = c.String(nullable: false),
                        StudyExperience = c.String(nullable: false),
                        Certificate = c.String(nullable: false, maxLength: 800),
                        CreateDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                        EditDate = c.DateTime(nullable: false, defaultValueSql: "getutcdate()"),
                    })
                .PrimaryKey(t => t.ResumeId);   
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Packages", "CVLetterId", "dbo.CVLetters");
            DropForeignKey("dbo.Packages", "ResumeId", "dbo.Resumes");
            DropIndex("dbo.Packages", new[] { "CVLetterId" });
            DropIndex("dbo.Packages", new[] { "ResumeId" });
            DropTable("dbo.Resumes");
            DropTable("dbo.Packages");
            DropTable("dbo.CVLetters");
        }
    }
}
