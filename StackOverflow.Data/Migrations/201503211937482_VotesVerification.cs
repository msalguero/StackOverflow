namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VotesVerification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Voter_Id = c.Guid(),
                        Question_Id = c.Guid(),
                        Answer_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Voter_Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .ForeignKey("dbo.Answers", t => t.Answer_Id)
                .Index(t => t.Voter_Id)
                .Index(t => t.Question_Id)
                .Index(t => t.Answer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Votes", "Answer_Id", "dbo.Answers");
            DropForeignKey("dbo.Votes", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Votes", "Voter_Id", "dbo.Accounts");
            DropIndex("dbo.Votes", new[] { "Answer_Id" });
            DropIndex("dbo.Votes", new[] { "Question_Id" });
            DropIndex("dbo.Votes", new[] { "Voter_Id" });
            DropTable("dbo.Votes");
        }
    }
}
