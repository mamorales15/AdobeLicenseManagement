namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovelengthlimitonSavedQueriesDescription : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SavedQueries", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SavedQueries", "Description", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
