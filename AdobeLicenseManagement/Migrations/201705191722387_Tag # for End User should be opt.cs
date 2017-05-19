namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagforEndUsershouldbeopt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PointOfContacts", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PointOfContacts", "Notes");
        }
    }
}
