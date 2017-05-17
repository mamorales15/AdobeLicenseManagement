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
    public class PointOfContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PointOfContacts
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Index()
        {
            return View(db.PointOfContacts.ToList());
        }

        // GET: PointOfContacts/Details/5
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Details(string id)
        {
            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PointOfContact pointOfContact = db.PointOfContacts.Find(id);
            if (pointOfContact == null)
            {
                return HttpNotFound();
            }
            return View(pointOfContact);
        }

        // GET: PointOfContacts/Create
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PointOfContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create([Bind(Include = "POCName")] PointOfContact pointOfContact)
        {
            if (ModelState.IsValid)
            {
                db.PointOfContacts.Add(pointOfContact);
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "Point of Contact " + pointOfContact.POCName + " created";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem creating the Point of Contact " + pointOfContact.POCName;
                    return RedirectToAction("Index");
                }
            }

            return View(pointOfContact);
        }

        // GET: PointOfContacts/Edit/5
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit(string id)
        {
            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PointOfContact pointOfContact = db.PointOfContacts.Find(id);
            if (pointOfContact == null)
            {
                return HttpNotFound();
            }
            return View(pointOfContact);
        }

        // POST: PointOfContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit([Bind(Include = "POCName")] PointOfContact pointOfContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pointOfContact).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "Point of Contact " + pointOfContact.POCName + " edited";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem editing the Point of Contact " + pointOfContact.POCName;
                    return RedirectToAction("Index");
                }
            }
            return View(pointOfContact);
        }

        // GET: PointOfContacts/Delete/5
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Delete(string id)
        {
            if (id == "")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PointOfContact pointOfContact = db.PointOfContacts.Find(id);
            if (pointOfContact == null)
            {
                return HttpNotFound();
            }
            return View(pointOfContact);
        }

        // POST: PointOfContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult DeleteConfirmed(string id)
        {
            PointOfContact pointOfContact = db.PointOfContacts.Find(id);
            db.PointOfContacts.Remove(pointOfContact);
            try
            {
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "Point of Contact " + pointOfContact.POCName + " deleted";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["DangerOHMsg"] = "Problem deleting the Point of Contact " + pointOfContact.POCName;
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
