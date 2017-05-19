namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TagforEndUsershouldbeopttry4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EndUsers", "Tag", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EndUsers", "Tag", c => c.Int(nullable: false));
        }
    }
}
