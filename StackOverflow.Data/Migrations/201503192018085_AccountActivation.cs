namespace StackOverflow.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountActivation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "LastName", c => c.String());
            AddColumn("dbo.Accounts", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "IsActive");
            DropColumn("dbo.Accounts", "LastName");
        }
    }
}
