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
    public class LicenseTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: LicenseTypes
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Index()
        {
            return View(db.LicenseTypes.ToList());
        }

        // GET: LicenseTypes/Details/5
        [Authorize(Roles = "Owner, Administrator, Super User")]
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
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: LicenseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create([Bind(Include = "LicenseTypeID,LicenseTypeDesc")] LicenseType licenseType)
        {
            if (ModelState.IsValid)
            {
                db.LicenseTypes.Add(licenseType);
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "License Type " + licenseType.LicenseTypeDesc + " created";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem creating the License Type" + licenseType.LicenseTypeDesc;
                    return RedirectToAction("Index");
                }
            }

            return View(licenseType);
        }

        // GET: LicenseTypes/Edit/5
        [Authorize(Roles = "Owner, Administrator")]
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
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit([Bind(Include = "LicenseTypeID,LicenseTypeDesc")] LicenseType licenseType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(licenseType).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "License Type " + licenseType.LicenseTypeDesc + " edited";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem editing the License Type" + licenseType.LicenseTypeDesc;
                    return RedirectToAction("Index");
                }
            }
            return View(licenseType);
        }

        // GET: LicenseTypes/Delete/5
        [Authorize(Roles = "Owner, Administrator")]
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
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            LicenseType licenseType = db.LicenseTypes.Find(id);
            db.LicenseTypes.Remove(licenseType);
            try
            {
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "License Type " + licenseType.LicenseTypeDesc + " deleted";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["DangerOHMsg"] = "Problem deleting the License Type" + licenseType.LicenseTypeDesc;
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
