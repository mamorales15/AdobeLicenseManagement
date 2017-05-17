namespace AdobeLicenseManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedallthebasemodelsTry3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EndUsers",
                c => new
                    {
                        EndUserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(),
                        Building = c.String(maxLength: 50),
                        RmNo = c.String(maxLength: 50),
                        Tag = c.Int(nullable: false),
                        ComputerSerial = c.String(maxLength: 50),
                        ComputerName = c.String(maxLength: 50),
                        Counter = c.Int(),
                        AdobeID = c.String(maxLength: 50),
                        Request_RequestID = c.Int(),
                    })
                .PrimaryKey(t => t.EndUserID)
                .ForeignKey("dbo.Requests", t => t.Request_RequestID)
                .Index(t => t.Request_RequestID);
            
            CreateTable(
                "dbo.Requests",
                c => new
                    {
                        RequestID = c.Int(nullable: false, identity: true),
                        LicenseType_LicenseTypeID = c.Int(),
                        PointOfContact_POCName = c.String(maxLength: 50),
                        Product_ProductID = c.Int(),
                        VIP_VIPID = c.Int(),
                    })
                .PrimaryKey(t => t.RequestID)
                .ForeignKey("dbo.LicenseTypes", t => t.LicenseType_LicenseTypeID)
                .ForeignKey("dbo.PointOfContacts", t => t.PointOfContact_POCName)
                .ForeignKey("dbo.Products", t => t.Product_ProductID)
                .ForeignKey("dbo.VIPs", t => t.VIP_VIPID)
                .Index(t => t.LicenseType_LicenseTypeID)
                .Index(t => t.PointOfContact_POCName)
                .Index(t => t.Product_ProductID)
                .Index(t => t.VIP_VIPID);
            
            CreateTable(
                "dbo.LicenseTypes",
                c => new
                    {
                        LicenseTypeID = c.Int(nullable: false, identity: true),
                        LicenseTypeDesc = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.LicenseTypeID);
            
            CreateTable(
                "dbo.PointOfContacts",
                c => new
                    {
                        POCName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.POCName);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductDesc = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.ProductID);
            
            CreateTable(
                "dbo.PurchaseOrders",
                c => new
                    {
                        PurchaseOrderID = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        PONo = c.Int(nullable: false),
                        PODate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PurchaseOrderID)
                .ForeignKey("dbo.Requests", t => t.PurchaseOrderID)
                .Index(t => t.PurchaseOrderID);
            
            CreateTable(
                "dbo.ServiceDeskRequests",
                c => new
                    {
                        ServiceDeskRequestID = c.Int(nullable: false),
                        Request_RequestID = c.Int(),
                    })
                .PrimaryKey(t => t.ServiceDeskRequestID)
                .ForeignKey("dbo.Requests", t => t.Request_RequestID)
                .Index(t => t.Request_RequestID);
            
            CreateTable(
                "dbo.VIPs",
                c => new
                    {
                        VIPID = c.Int(nullable: false, identity: true),
                        VIPName = c.String(nullable: false, maxLength: 50),
                        VIPNumber = c.String(nullable: false, maxLength: 50),
                        VIPRenewalDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.VIPID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Requests", "VIP_VIPID", "dbo.VIPs");
            DropForeignKey("dbo.ServiceDeskRequests", "Request_RequestID", "dbo.Requests");
            DropForeignKey("dbo.PurchaseOrders", "PurchaseOrderID", "dbo.Requests");
            DropForeignKey("dbo.Requests", "Product_ProductID", "dbo.Products");
            DropForeignKey("dbo.Requests", "PointOfContact_POCName", "dbo.PointOfContacts");
            DropForeignKey("dbo.Requests", "LicenseType_LicenseTypeID", "dbo.LicenseTypes");
            DropForeignKey("dbo.EndUsers", "Request_RequestID", "dbo.Requests");
            DropIndex("dbo.ServiceDeskRequests", new[] { "Request_RequestID" });
            DropIndex("dbo.PurchaseOrders", new[] { "PurchaseOrderID" });
            DropIndex("dbo.Requests", new[] { "VIP_VIPID" });
            DropIndex("dbo.Requests", new[] { "Product_ProductID" });
            DropIndex("dbo.Requests", new[] { "PointOfContact_POCName" });
            DropIndex("dbo.Requests", new[] { "LicenseType_LicenseTypeID" });
            DropIndex("dbo.EndUsers", new[] { "Request_RequestID" });
            DropTable("dbo.VIPs");
            DropTable("dbo.ServiceDeskRequests");
            DropTable("dbo.PurchaseOrders");
            DropTable("dbo.Products");
            DropTable("dbo.PointOfContacts");
            DropTable("dbo.LicenseTypes");
            DropTable("dbo.Requests");
            DropTable("dbo.EndUsers");
        }
    }
}
