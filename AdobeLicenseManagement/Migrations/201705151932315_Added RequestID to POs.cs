namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedRequestIDtoPOs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PurchaseOrders", "RequestID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PurchaseOrders", "RequestID");
        }
    }
}
