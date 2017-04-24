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
    public class LicenseTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LicenseTypes
        public ActionResult Index()
        {
            return View(db.LicenseTypes.ToList());
        }

        // GET: LicenseTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicenseType licenseType = db.LicenseTypes.Find(id);
            if (licenseType == null)
            {
                return HttpNotFound();
            }
            return View(licenseType);
        }

        // GET: LicenseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LicenseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LicenseTypeID,LicenseTypeDesc")] LicenseType licenseType)
        {
            if (ModelState.IsValid)
            {
                db.LicenseTypes.Add(licenseType);
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "License Type " + licenseType.LicenseTypeDesc + " created";
                return RedirectToAction("Index");
            }

            return View(licenseType);
        }

        // GET: LicenseTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicenseType licenseType = db.LicenseTypes.Find(id);
            if (licenseType == null)
            {
                return HttpNotFound();
            }
            return View(licenseType);
        }

        // POST: LicenseTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LicenseTypeID,LicenseTypeDesc")] LicenseType licenseType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(licenseType).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "License Type " + licenseType.LicenseTypeDesc + " edited";
                return RedirectToAction("Index");
            }
            return View(licenseType);
        }

        // GET: LicenseTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicenseType licenseType = db.LicenseTypes.Find(id);
            if (licenseType == null)
            {
                return HttpNotFound();
            }
            return View(licenseType);
        }

        // POST: LicenseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LicenseType licenseType = db.LicenseTypes.Find(id);
            db.LicenseTypes.Remove(licenseType);
            db.SaveChanges();
            TempData["SuccessOHMsg"] = "License Type " + licenseType.LicenseTypeDesc + " deleted";
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
