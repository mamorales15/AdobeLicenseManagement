namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedQuerymodelsTry3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SavedQueries",
                c => new
                    {
                        SavedQueryID = c.Int(nullable: false, identity: true),
                        Description = c.String(nullable: false, maxLength: 50),
                        Query = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.SavedQueryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.SavedQueries");
        }
    }
}
