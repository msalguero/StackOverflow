namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProfileUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "RegistrationDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Accounts", "LastProfileViewDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Accounts", "ProfileViews", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "ProfileViews");
            DropColumn("dbo.Accounts", "LastProfileViewDate");
            DropColumn("dbo.Accounts", "RegistrationDate");
        }
    }
}
