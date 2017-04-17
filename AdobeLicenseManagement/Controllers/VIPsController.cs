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
    public class VIPsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VIPs
        public ActionResult Index()
        {
            return View(db.VIPs.ToList());
        }

        // GET: VIPs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VIP vIP = db.VIPs.Find(id);
            if (vIP == null)
            {
                return HttpNotFound();
            }
            return View(vIP);
        }

        // GET: VIPs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VIPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VIPID,VIPName,VIPNumber,VIPRenewalDate")] VIP vIP)
        {
            if (ModelState.IsValid)
            {
                db.VIPs.Add(vIP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vIP);
        }

        // GET: VIPs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VIP vIP = db.VIPs.Find(id);
            if (vIP == null)
            {
                return HttpNotFound();
            }
            return View(vIP);
        }

        // POST: VIPs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VIPID,VIPName,VIPNumber,VIPRenewalDate")] VIP vIP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vIP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vIP);
        }

        // GET: VIPs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VIP vIP = db.VIPs.Find(id);
            if (vIP == null)
            {
                return HttpNotFound();
            }
            return View(vIP);
        }

        // POST: VIPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VIP vIP = db.VIPs.Find(id);
            db.VIPs.Remove(vIP);
            db.SaveChanges();
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
