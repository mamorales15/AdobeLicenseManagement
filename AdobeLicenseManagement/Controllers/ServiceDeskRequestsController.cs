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
    public class ServiceDeskRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ServiceDeskRequests
        public ActionResult Index()
        {
            var ServiceDeskRequests = db.ServiceDeskRequests.Include(i => i.Request);
            return View(ServiceDeskRequests.ToList());
        }

        // GET: ServiceDeskRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceDeskRequest ServiceDeskRequest = db.ServiceDeskRequests.Find(id);
            if (ServiceDeskRequest == null)
            {
                return HttpNotFound();
            }
            return View(ServiceDeskRequest);
        }

        // GET: ServiceDeskRequests/Create
        public ActionResult Create()
        {
            ViewBag.RequestID = new SelectList(db.Requests, "RequestID", "RequestID");
            return View();
        }

        // POST: ServiceDeskRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServiceDeskRequestID")] ServiceDeskRequest ServiceDeskRequest)
        {
            if (ModelState.IsValid)
            {
                db.ServiceDeskRequests.Add(ServiceDeskRequest);
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "Service Desk Request " + ServiceDeskRequest.ServiceDeskRequestID + " created";
                return RedirectToAction("Index");
            }

            ViewBag.RequestID = new SelectList(db.Requests, "RequestID", "RequestID", ServiceDeskRequest.ServiceDeskRequestID);
            return View(ServiceDeskRequest);
        }

        // GET: ServiceDeskRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceDeskRequest ServiceDeskRequest = db.ServiceDeskRequests.Find(id);
            if (ServiceDeskRequest == null)
            {
                return HttpNotFound();
            }
            ServiceDeskRequestEditViewModel sdrEdit = new ServiceDeskRequestEditViewModel();
            sdrEdit.ServiceDeskRequestID = ServiceDeskRequest.ServiceDeskRequestID;
            if(ServiceDeskRequest.Request != null)
            {
                sdrEdit.RequestID = ServiceDeskRequest.Request.RequestID;
            }
            ViewBag.RequestList = new SelectList(db.Requests.OrderBy(x => x.RequestID), "RequestID", "RequestID");
            return View(sdrEdit);
        }

        // POST: ServiceDeskRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServiceDeskRequestID,RequestID")] ServiceDeskRequestEditViewModel sdrEdit)
        {
            if (ModelState.IsValid)
            {
                ServiceDeskRequest sdr = db.ServiceDeskRequests.Find(sdrEdit.ServiceDeskRequestID);
                sdr.ServiceDeskRequestID = sdrEdit.ServiceDeskRequestID;
                if(sdr.Request != null)
                {
                    // Remove Service Desk Request from it's old Request's list
                    sdr.Request.ServiceDeskRequests.Remove(sdr);
                }
                // Get the new request
                sdr.Request = db.Requests.Find(sdrEdit.RequestID);
                // Add Service Desk Request to new Request's list
                sdr.Request.ServiceDeskRequests.Add(sdr);
                db.Entry(sdr).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "Service Desk Request " + sdrEdit.ServiceDeskRequestID + " edited";
                return RedirectToAction("Index");
            }
            ViewBag.RequestID = new SelectList(db.Requests, "RequestID", "RequestID", sdrEdit.ServiceDeskRequestID);
            return View(sdrEdit);
        }

        // GET: ServiceDeskRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ServiceDeskRequest ServiceDeskRequest = db.ServiceDeskRequests.Find(id);
            if (ServiceDeskRequest == null)
            {
                return HttpNotFound();
            }
            return View(ServiceDeskRequest);
        }

        // POST: ServiceDeskRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceDeskRequest ServiceDeskRequest = db.ServiceDeskRequests.Find(id);
            db.ServiceDeskRequests.Remove(ServiceDeskRequest);
            db.SaveChanges();
            TempData["SuccessOHMsg"] = "Service Desk Request " + ServiceDeskRequest.ServiceDeskRequestID + " deleted";
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
