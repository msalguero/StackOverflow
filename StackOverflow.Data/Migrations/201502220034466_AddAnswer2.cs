namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAnswer2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Accounts", "Answers");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "Answers", c => c.Int(nullable: false));
        }
    }
}
