namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Owner_Id = c.Guid(),
                        Answer_Id = c.Guid(),
                        Question_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Owner_Id)
                .ForeignKey("dbo.Answers", t => t.Answer_Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.Answer_Id)
                .Index(t => t.Question_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Comments", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.Comments", "Owner_Id", "dbo.Accounts");
            DropIndex("dbo.Comments", new[] { "Question_Id" });
            DropIndex("dbo.Comments", new[] { "Answer_Id" });
            DropIndex("dbo.Comments", new[] { "Owner_Id" });
            DropTable("dbo.Comments");
        }
    }
}
