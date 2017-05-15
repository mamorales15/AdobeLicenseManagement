namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreationdatetoSavedQuery : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SavedQueries", "CreationDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SavedQueries", "CreationDate");
        }
    }
}
