using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdobeLicenseManagement.Models;

namespace AdobeLicenseManagement.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Requests
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Index()
        {
            var requests = db.Requests.Include(r => r.PurchaseOrder);
            return View(requests.ToList());
        }

        // GET: Requests/Details/5
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Details(int? id)
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
            return View(request);
        }

        // GET: Requests/Edit/5
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit(int? id)
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

            RequestEditViewModel reqEdit = new RequestEditViewModel();
            reqEdit.RequestID = request.RequestID;
            reqEdit.PurchaseOrderID = request.PurchaseOrder.PurchaseOrderID;
            reqEdit.VIPID = request.VIP.VIPID;
            reqEdit.LicenseTypeID = request.LicenseType.LicenseTypeID;
            reqEdit.ProductID = request.Product.ProductID;
            reqEdit.POCName = request.PointOfContact.POCName;
            
            ViewBag.VIPList = new SelectList(db.VIPs.OrderBy(x => x.VIPID), "VIPID", "VIPName");
            ViewBag.LicenseTypeList = new SelectList(db.LicenseTypes.OrderBy(x => x.LicenseTypeID), "LicenseTypeID", "LicenseTypeDesc");
            ViewBag.ProductList = new SelectList(db.Products.OrderBy(x => x.ProductID), "ProductID", "ProductDesc");
               
            return View(reqEdit);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit([Bind(Include = "RequestID,PurchaseOrderID,VIPID,LicenseTypeID,ProductID,POCName")] RequestEditViewModel reqEdit)
        {
            if (ModelState.IsValid)
            {
                Request req = db.Requests.Find(reqEdit.RequestID);

                // If there is a difference in the VIPs
                if (req.VIP.VIPID != reqEdit.VIPID)
                {
                    // Assumes that the VIP already exists because it came from a drop down list
                    VIP vip = db.VIPs.Find(reqEdit.VIPID);

                    // Remove request from old VIP
                    req.VIP.Requests.Remove(req);

                    // Add request to new VIP
                    vip.Requests.Add(req);

                    // Point the request to the new VIP
                    req.VIP = vip;
                }

                // If there is a difference in the LicenseTypes
                if (req.LicenseType.LicenseTypeID != reqEdit.LicenseTypeID)
                {
                    // Assumes that the LicenseType already exists because it came from a drop down list
                    LicenseType licenseType = db.LicenseTypes.Find(reqEdit.LicenseTypeID);

                    // Remove request from old VIP
                    req.LicenseType.Requests.Remove(req);

                    // Add request to new VIP
                    licenseType.Requests.Add(req);

                    // Point the request to the new VIP
                    req.LicenseType = licenseType;
                }

                // If there is a difference in the Products
                if (req.Product.ProductID != reqEdit.ProductID)
                {
                    // Assumes that the VIP already exists because it came from a drop down list
                    Product product = db.Products.Find(reqEdit.ProductID);

                    // Remove request from old VIP
                    req.Product.Requests.Remove(req);

                    // Add request to new VIP
                    product.Requests.Add(req);

                    // Point the request to the new VIP
                    req.Product = product;
                }

                // If there is a difference in the Point of Contacts
                if (req.PointOfContact.POCName != reqEdit.POCName)
                {
                    // Assumes that the VIP already exists because it came from a drop down list
                    //VIP vip = db.VIPs.Find(reqEdit.VIPID);

                    PointOfContact poc;

                    // If the Point of Contact already exists
                    if (db.PointOfContacts.Any(o => o.POCName == reqEdit.POCName))
                    {
                        poc = db.PointOfContacts.Find(reqEdit.POCName);
                    }
                    else
                    {
                        // Create new Point of Contact
                        poc = new PointOfContact();
                        poc.POCName = reqEdit.POCName;
                    }

                    // Remove request from old Point of Contact
                    req.PointOfContact.Requests.Remove(req);

                    // Add request to new Point of Contact
                    poc.Requests.Add(req);

                    // Point the request to the new Point of Contact
                    req.PointOfContact = poc;
                }

                db.Entry(req).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "Request " + req.RequestID + " edited";
                return RedirectToAction("Index");
            }
            
            ViewBag.VIPList = new SelectList(db.VIPs.OrderBy(x => x.VIPID), "VIPID", "VIPName");
            ViewBag.LicenseTypeList = new SelectList(db.LicenseTypes.OrderBy(x => x.LicenseTypeID), "LicenseTypeID", "LicenseTypeDesc");
            ViewBag.ProductList = new SelectList(db.Products.OrderBy(x => x.ProductID), "ProductID", "ProductDesc");

            return View(reqEdit);
        }

        // GET: Requests/Delete/5
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Delete(int? id)
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
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
            db.SaveChanges();
            TempData["SuccessOHMsg"] = "Request " + request.RequestID + " deleted";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
