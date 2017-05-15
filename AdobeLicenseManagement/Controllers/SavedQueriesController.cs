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
    public class SavedQueriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SavedQueries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavedQuery savedQuery = db.SavedQueries.Find(id);
            if (savedQuery == null)
            {
                return HttpNotFound();
            }
            return View(savedQuery);
        }

        // GET: SavedQueries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavedQuery savedQuery = db.SavedQueries.Find(id);
            if (savedQuery == null)
            {
                return HttpNotFound();
            }
            return View(savedQuery);
        }

        // POST: SavedQueries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SavedQueryID,Description,Query")] SavedQuery savedQuery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(savedQuery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Query", null);
        }

        // GET: SavedQueries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SavedQuery savedQuery = db.SavedQueries.Find(id);
            if (savedQuery == null)
            {
                return HttpNotFound();
            }
            return View(savedQuery);
        }

        // POST: SavedQueries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SavedQuery savedQuery = db.SavedQueries.Find(id);
            db.SavedQueries.Remove(savedQuery);
            db.SaveChanges();
            return RedirectToAction("Index", "Query", null);
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
