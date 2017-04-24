namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangededTagnumbersbacktointstotestrangedataannotation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EndUsers", "Tag", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EndUsers", "Tag", c => c.String(nullable: false, maxLength: 6));
        }
    }
}
