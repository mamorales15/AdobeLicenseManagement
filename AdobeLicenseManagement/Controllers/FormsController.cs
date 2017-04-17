using AdobeLicenseManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdobeLicenseManagement.Controllers
{
    public class FormsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //
        // GET: /Forms/HelpDeskForm
        public ActionResult HelpDeskForm()
        {

            ViewBag.VIPList = new SelectList(db.VIPs.OrderBy(x => x.VIPID), "VIPID", "VIPName");
            ViewBag.LicenseTypeList = new SelectList(db.LicenseTypes.OrderBy(x => x.LicenseTypeID), "LicenseTypeID", "LicenseTypeDesc");
            ViewBag.ProductList = new SelectList(db.Products.OrderBy(x => x.ProductID), "ProductID", "ProductDesc");

            return View();
        }

        //
        // POST: /Forms/HelpDeskForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HelpDeskForm([Bind(Include = "SDReqID,VIPID,LicenseTypeID,ProductID,POC,Qty")] HelpDeskFormViewModel helpDeskForm)
        {
            ViewBag.VIPList = new SelectList(db.VIPs.OrderBy(x => x.VIPID), "VIPID", "VIPName");
            ViewBag.LicenseTypeList = new SelectList(db.LicenseTypes.OrderBy(x => x.LicenseTypeID), "LicenseTypeID", "LicenseTypeDesc");
            ViewBag.ProductList = new SelectList(db.Products.OrderBy(x => x.ProductID), "ProductID", "ProductDesc");

            if (ModelState.IsValid)
            {
                // Service Desk Request ID should not exist already
                if (db.ServiceDeskRequests.Any(o => o.ServiceDeskRequestID == helpDeskForm.SDReqID))
                {
                    ModelState.AddModelError("", "Service Desk Request ID exists already.");
                    return View();
                }

                // If Point of Contact doesn't already exist, create it.
                PointOfContact poc;
                if (!db.PointOfContacts.Any(o => o.POCName == helpDeskForm.POC))
                {
                    poc = new PointOfContact();
                    poc.POCName = helpDeskForm.POC;
                    db.PointOfContacts.Add(poc);
                    db.SaveChanges();
                }
                else
                {
                    poc = db.PointOfContacts.Find(helpDeskForm.POC);
                }

                // Get objects from tables
                VIP vip = db.VIPs.Find(helpDeskForm.VIPID);
                LicenseType licenseType = db.LicenseTypes.Find(helpDeskForm.LicenseTypeID);
                Product product = db.Products.Find(helpDeskForm.ProductID);

                // Create new Service Desk Request
                ServiceDeskRequest sdReq = new ServiceDeskRequest();
                sdReq.ServiceDeskRequestID = helpDeskForm.SDReqID;

                // Create new Purchase Order
                PurchaseOrder po = new PurchaseOrder();
                po.Qty = helpDeskForm.Qty;
                po.PODate = new DateTime(1753, 1, 1);       // Minimum date for SQL

                // Create new Request
                Request req = new Request();
                req.PurchaseOrder = po;
                req.VIP = vip;
                req.LicenseType = licenseType;
                req.Product = product;
                req.PointOfContact = poc;

                // Add Request to other models' virtual or ICollection
                sdReq.Request = req;
                po.Request = req;
                poc.Requests.Add(req);
                vip.Requests.Add(req);
                licenseType.Requests.Add(req);
                product.Requests.Add(req);

                // Add new records to database tables
                db.Requests.Add(req);   // Also adds Purchase Orders to table because of 1 to 1 relationship
                db.SaveChanges();
                db.ServiceDeskRequests.Add(sdReq);
                db.SaveChanges();
                
                return RedirectToAction("HelpDeskForm");
            }

            ModelState.AddModelError("", "Problem Submitting the form. Model state is not valid.");
            return View();
        }
    }
}