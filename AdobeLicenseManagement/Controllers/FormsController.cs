using AdobeLicenseManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AdobeLicenseManagement.Controllers
{
    [Authorize]
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
        public ActionResult HelpDeskForm([Bind(Include = "SDReqID,VIPID,LicenseTypeID,ProductID,POCName,POCNotes,Qty")] HelpDeskFormViewModel helpDeskForm)
        {
            ViewBag.VIPList = new SelectList(db.VIPs.OrderBy(x => x.VIPID), "VIPID", "VIPName");
            ViewBag.LicenseTypeList = new SelectList(db.LicenseTypes.OrderBy(x => x.LicenseTypeID), "LicenseTypeID", "LicenseTypeDesc");
            ViewBag.ProductList = new SelectList(db.Products.OrderBy(x => x.ProductID), "ProductID", "ProductDesc");

            if (ModelState.IsValid)
            {
                // ServiceDesk Request ID should not exist already
                if (db.ServiceDeskRequests.Any(o => o.ServiceDeskRequestID == helpDeskForm.SDReqID))
                {
                    ModelState.AddModelError("", "ServiceDesk Request ID exists already.");
                    return View();
                }

                // If Point of Contact doesn't already exist, create it.
                PointOfContact poc;
                if (!db.PointOfContacts.Any(o => o.POCName == helpDeskForm.POCName))
                {
                    poc = new PointOfContact();
                    poc.POCName = helpDeskForm.POCName;
                    poc.Notes = helpDeskForm.POCNotes;
                    db.PointOfContacts.Add(poc);
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "Point of Contact " + poc.POCName + " created";
                }
                else
                {
                    poc = db.PointOfContacts.Find(helpDeskForm.POCName);
                }

                // Get objects from tables
                VIP vip = db.VIPs.Find(helpDeskForm.VIPID);
                LicenseType licenseType = db.LicenseTypes.Find(helpDeskForm.LicenseTypeID);
                Product product = db.Products.Find(helpDeskForm.ProductID);

                // Create new ServiceDesk Request
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
                po.RequestID = req.RequestID;
                poc.Requests.Add(req);
                vip.Requests.Add(req);
                licenseType.Requests.Add(req);
                product.Requests.Add(req);

                // Add new records to database tables
                db.Requests.Add(req);   // Also adds Purchase Orders to table because of 1 to 1 relationship
                db.SaveChanges();
                db.ServiceDeskRequests.Add(sdReq);
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "ServiceDesk Request " + sdReq.ServiceDeskRequestID + ", Purchase Order " + po.PurchaseOrderID + ", and Request " + req.RequestID + " created";
                return RedirectToAction("HelpDeskForm");
            }

            ModelState.AddModelError("", "Problem Submitting the form. Model state is not valid.");
            return View();
        }

        //
        // GET: /Forms/EndUserForm
        public ActionResult EndUserForm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewData["request"] = id;
            ViewBag.BuildingList = new SelectList(db.Buildings.OrderBy(x => x.BuildingID), "BuildingID", "BuildingName");
            return View();
        }

        //
        // POST: /Forms/EndUserForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EndUserForm([Bind(Include = "RequestID,UserName,Email,BuildingID,RmNo,Tag,ComputerSerial,ComputerName,AdobeID")] EndUserFormViewModel endUserForm)
        {
            // TODO: Might want to change the dataTextField to another attribute of Request that is unique but also identifiable
            ViewBag.RequestList = new SelectList(db.Requests.OrderBy(x => x.RequestID), "RequestID", "RequestID");
            ViewBag.BuildingList = new SelectList(db.Buildings.OrderBy(x => x.BuildingID), "BuildingID", "BuildingName");

            if (ModelState.IsValid)
            {
                // Get objects from tables
                Request req = db.Requests.Find(endUserForm.RequestID);
                Building build = db.Buildings.Find(endUserForm.BuildingID);

                // Create new End User
                EndUser endUser = new EndUser();
                endUser.UserName = endUserForm.UserName;
                endUser.Email = endUserForm.Email;
                endUser.Building = build;
                endUser.RmNo = endUserForm.RmNo;
                endUser.Tag = endUserForm.Tag;
                endUser.ComputerSerial = endUserForm.ComputerSerial;
                endUser.ComputerName = endUserForm.ComputerName;
                endUser.AdobeID = endUserForm.AdobeID;      // Optional to include AdobeID right when the EndUser is created, but should be rare
                endUser.Request = req;
                db.EndUsers.Add(endUser);
                db.SaveChanges();

                TempData["SuccessOHMsg"] = "End User " + endUser.UserName + " created";
                return RedirectToAction("EndUserForm");
            }
            ViewData["request"] = endUserForm.RequestID;
            ModelState.AddModelError("", "Problem Submitting the form. Model state is not valid.");
            return View();
        }

        //
        // GET: /Forms/POForm
        public ActionResult POForm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewData["request"] = id;
            return View();
        }

        //
        // POST: /Forms/POForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult POForm([Bind(Include = "RequestID,Qty,PONo,PODate")] POFormViewModel purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                // PO ID and Request ID should be the same
                Request req = db.Requests.Find(purchaseOrder.RequestID);
                // Check if a PO exists already, if not create it
                PurchaseOrder po = db.PurchaseOrders.Find(purchaseOrder.RequestID);
                if (po == null)
                {
                    po = new PurchaseOrder();
                    po.PurchaseOrderID = purchaseOrder.RequestID;
                }
                po.Qty = purchaseOrder.Qty;
                po.PONo = purchaseOrder.PONo;
                po.PODate = purchaseOrder.PODate;

                db.PurchaseOrders.Add(po);
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "Purchase Order " + po.PurchaseOrderID + " created";
                    return RedirectToAction("Index", "Requests");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem creating the Purchase Order " + po.PurchaseOrderID;
                    return RedirectToAction("POForm");
                }
            }
            return RedirectToAction("POForm");
            /*
            if (ModelState.IsValid)
            {
                // Get objects from tables
                Request req = db.Requests.Find(purchaseOrder.RequestID);
                PurchaseOrder po = req.PurchaseOrder;
                if (po == null)
                    po = new PurchaseOrder();
                po.PONo = purchaseOrder.PONo;
                po.PODate = purchaseOrder.PODate;
                db.SaveChanges();

                TempData["SuccessOHMsg"] = "Purchase Order details added to the request " + req.RequestID;
                return RedirectToAction("POForm");
            }

            ModelState.AddModelError("", "Problem Submitting the form. Model state is not valid.");
            return View();
            */
        }

        //
        // GET: /Forms/AdobeIDForm
        public ActionResult AdobeIDForm()
        {
            ViewBag.EndUserList = new SelectList(db.EndUsers.OrderBy(x => x.UserName), "EndUserID", "UserName");

            return View();
        }

        //
        // POST: /Forms/AdobeIDForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdobeIDForm([Bind(Include = "EndUserID,AdobeID")] AdobeIDFormViewModel adobeIDForm)
        {
            ViewBag.EndUserList = new SelectList(db.EndUsers.OrderBy(x => x.UserName), "EndUserID", "UserName");

            if (ModelState.IsValid)
            {
                EndUser endUser = db.EndUsers.Find(adobeIDForm.EndUserID);  // Get object from table
                endUser.AdobeID = adobeIDForm.AdobeID;
                db.SaveChanges();

                TempData["SuccessOHMsg"] = "Adobe ID added to the end user " + endUser.UserName;
                return RedirectToAction("AdobeIDForm");
            }

            ModelState.AddModelError("", "Problem Submitting the form. Model state is not valid.");
            return View();
        }

        //
        // GET: /Forms/ServiceDeskForm
        public ActionResult ServiceDeskForm(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            ViewData["request"] = id;
            return View();
        }

        //
        // POST: /Forms/ServiceDeskForm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ServiceDeskForm([Bind(Include = "ServiceDeskRequestID,RequestID")] ServiceDeskFormViewModel serviceDeskForm)
        {
            if (ModelState.IsValid)
            {
                Request req = db.Requests.Find(serviceDeskForm.RequestID);  // Get object from table

                // Create new ServiceDesk Request
                ServiceDeskRequest sdreq = new ServiceDeskRequest();
                sdreq.ServiceDeskRequestID = serviceDeskForm.ServiceDeskRequestID;
                sdreq.Request = req;    // Might cause problems. Might already be handled in the line below.
                req.ServiceDeskRequests.Add(sdreq);
                db.SaveChanges();

                TempData["SuccessOHMsg"] = "Additional ServiceDesk Request added to request " + req.RequestID;
                return RedirectToAction("ServiceDeskForm");
            }

            ModelState.AddModelError("", "Problem Submitting the form. Model state is not valid.");
            return View();
        }
    }
}