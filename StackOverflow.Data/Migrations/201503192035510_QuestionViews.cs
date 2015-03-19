namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionViews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Views", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Views");
        }
    }
}
