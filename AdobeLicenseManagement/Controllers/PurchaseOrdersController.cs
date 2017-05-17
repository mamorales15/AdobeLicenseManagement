using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AdobeLicenseManagement.Models;

namespace AdobeLicenseManagement.Controllers
{
    [Authorize]
    public class PurchaseOrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PurchaseOrders
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Index()
        {
            var purchaseOrders = db.PurchaseOrders.Include(p => p.Request);
            return View(purchaseOrders.ToList());
        }

        // GET: PurchaseOrders/Details/5
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Create
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create()
        {
            var query = from c in db.Requests
                        where c.PurchaseOrder == null
                        select c;
            ViewBag.PurchaseOrderID = new SelectList(query, "RequestID", "RequestID");
            //ViewBag.PurchaseOrderID = new SelectList(db.Requests, "RequestID", "RequestID");
            return View();
        }

        // POST: PurchaseOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create([Bind(Include = "PurchaseOrderID,Qty,PONo,PODate")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                db.PurchaseOrders.Add(purchaseOrder);
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "Purchase Order " + purchaseOrder.PurchaseOrderID + " created";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem creating the Purchase Order" + purchaseOrder.PurchaseOrderID;
                    return RedirectToAction("Index");
                }
            }
            var query = from c in db.Requests
                        where c.PurchaseOrder == null
                        select c;
            ViewBag.PurchaseOrderID = new SelectList(query, "RequestID", "RequestID", purchaseOrder.PurchaseOrderID);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Edit/5
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.PurchaseOrderID = new SelectList(db.Requests, "RequestID", "RequestID", purchaseOrder.PurchaseOrderID);
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit([Bind(Include = "PurchaseOrderID,Qty,PONo,PODate")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {

                db.Entry(purchaseOrder).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "Purchase Order " + purchaseOrder.PurchaseOrderID + " edited";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem editing the Purchase Order" + purchaseOrder.PurchaseOrderID;
                    return RedirectToAction("Index");
                }
            }
            ViewBag.PurchaseOrderID = new SelectList(db.Requests, "RequestID", "RequestID", purchaseOrder.PurchaseOrderID);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrders/Delete/5
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            if (purchaseOrder == null)
            {
                return HttpNotFound();
            }
            return View(purchaseOrder);
        }

        // POST: PurchaseOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchaseOrder purchaseOrder = db.PurchaseOrders.Find(id);
            db.PurchaseOrders.Remove(purchaseOrder);
            try
            {
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "Purchase Order " + purchaseOrder.PurchaseOrderID + " deleted";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["DangerOHMsg"] = "Problem deleting the Purchase Order" + purchaseOrder.PurchaseOrderID;
                return RedirectToAction("Index");
            }
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
