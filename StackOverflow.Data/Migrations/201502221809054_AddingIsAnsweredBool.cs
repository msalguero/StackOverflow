namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIsAnsweredBool : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "IsAnswered", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "IsAnswered");
        }
    }
}
