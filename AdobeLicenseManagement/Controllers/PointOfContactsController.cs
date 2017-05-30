using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdobeLicenseManagement.Models;
using PagedList;

namespace AdobeLicenseManagement.Controllers
{
    [Authorize]
    public class PointOfContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PointOfContacts
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageSize)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.POCNameSortParam = String.IsNullOrEmpty(sortOrder) ? "pocName_desc" : sortOrder == "default" ? "pocName_desc" : "default";
            ViewBag.NotesSortParam = sortOrder == "notes_asc" ? "notes_desc" : "notes_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var pocs = from s in db.PointOfContacts
                                       select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                pocs = pocs.Where(s => s.POCName.Contains(searchString)
                                        || s.Notes.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "pocName_desc":
                    pocs = pocs.OrderByDescending(s => s.POCName);
                    break;
                case "notes_desc":
                    pocs = pocs.OrderByDescending(s => s.Notes);
                    break;
                case "notes_asc":
                    pocs = pocs.OrderBy(s => s.Notes);
                    break;
                default:
                    pocs = pocs.OrderBy(s => s.POCName);
                    break;
            }

            int? tentativePageSize = 10;
            if (pageSize != null && pageSize > 0)
            {
                tentativePageSize = pageSize;
                ViewBag.PageSize = pageSize;
            }
            int myPageSize = (int)tentativePageSize;
            int pageNumber = (page ?? 1);

            return View(pocs.ToPagedList(pageNumber, myPageSize));
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
        public ActionResult Create([Bind(Include = "POCName,Notes")] PointOfContact pointOfContact)
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
        public ActionResult Edit([Bind(Include = "POCName,Notes")] PointOfContact pointOfContact)
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
