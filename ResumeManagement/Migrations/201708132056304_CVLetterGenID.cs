namespace ResumeManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CVLetterGenID : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Packages", "CVLetterId", "dbo.CVLetters");
            DropPrimaryKey("dbo.CVLetters");
            AlterColumn("dbo.CVLetters", "CVLetterId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.CVLetters", "CVLetterId");
            AddForeignKey("dbo.Packages", "CVLetterId", "dbo.CVLetters", "CVLetterId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Packages", "CVLetterId", "dbo.CVLetters");
            DropPrimaryKey("dbo.CVLetters");
            AlterColumn("dbo.CVLetters", "CVLetterId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.CVLetters", "CVLetterId");
            AddForeignKey("dbo.Packages", "CVLetterId", "dbo.CVLetters", "CVLetterId");
        }
    }
}
