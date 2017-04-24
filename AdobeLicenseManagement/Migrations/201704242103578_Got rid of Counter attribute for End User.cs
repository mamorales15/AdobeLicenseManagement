namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GotridofCounterattributeforEndUser : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.EndUsers", "Counter");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EndUsers", "Counter", c => c.Int());
        }
    }
}
