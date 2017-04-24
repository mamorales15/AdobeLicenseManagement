namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PONochangedtostring : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PurchaseOrders", "PONo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PurchaseOrders", "PONo", c => c.Int(nullable: false));
        }
    }
}
