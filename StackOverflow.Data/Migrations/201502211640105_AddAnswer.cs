namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnswer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Votes = c.Int(nullable: false),
                        Description = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Correct = c.Boolean(nullable: false),
                        Owner_Id = c.Guid(),
                        Question_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Accounts", t => t.Owner_Id)
                .ForeignKey("dbo.Questions", t => t.Question_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.Question_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Answers", "Question_Id", "dbo.Questions");
            DropForeignKey("dbo.Answers", "Owner_Id", "dbo.Accounts");
            DropIndex("dbo.Answers", new[] { "Question_Id" });
            DropIndex("dbo.Answers", new[] { "Owner_Id" });
            DropTable("dbo.Answers");
        }
    }
}
