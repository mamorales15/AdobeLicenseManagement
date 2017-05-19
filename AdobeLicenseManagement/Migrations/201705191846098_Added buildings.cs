namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedbuildings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        BuildingID = c.Int(nullable: false, identity: true),
                        BuildingName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.BuildingID);
            
            AddColumn("dbo.EndUsers", "Building_BuildingID", c => c.Int());
            CreateIndex("dbo.EndUsers", "Building_BuildingID");
            AddForeignKey("dbo.EndUsers", "Building_BuildingID", "dbo.Buildings", "BuildingID");
            DropColumn("dbo.EndUsers", "Building");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EndUsers", "Building", c => c.String(maxLength: 50));
            DropForeignKey("dbo.EndUsers", "Building_BuildingID", "dbo.Buildings");
            DropIndex("dbo.EndUsers", new[] { "Building_BuildingID" });
            DropColumn("dbo.EndUsers", "Building_BuildingID");
            DropTable("dbo.Buildings");
        }
    }
}
