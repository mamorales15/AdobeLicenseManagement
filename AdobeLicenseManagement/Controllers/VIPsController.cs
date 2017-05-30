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
    public class VIPsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VIPs
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageSize)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.VIPNameSortParam = String.IsNullOrEmpty(sortOrder) ? "vipName_desc" : sortOrder == "default" ? "vipName_desc" : "default";
            ViewBag.VIPNumberSortParam = sortOrder == "vipNumber_asc" ? "vipNumber_desc" : "vipNumber_asc";
            ViewBag.VIPRenewalDateSortParam = sortOrder == "vipRenewalDate_asc" ? "vipRenewalDate_desc" : "vipRenewalDate_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var vips = from s in db.VIPs
                                select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                vips = vips.Where(s => s.VIPName.Contains(searchString)
                                    || s.VIPNumber.ToString().Contains(searchString)
                                    || s.VIPRenewalDate.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "vipName_desc":
                    vips = vips.OrderByDescending(s => s.VIPName);
                    break;
                case "vipNumber_desc":
                    vips = vips.OrderByDescending(s => s.VIPNumber);
                    break;
                case "vipNumber_asc":
                    vips = vips.OrderBy(s => s.VIPNumber);
                    break;
                case "vipRenewalDate_desc":
                    vips = vips.OrderByDescending(s => s.VIPRenewalDate);
                    break;
                case "vipRenewalDate_asc":
                    vips = vips.OrderBy(s => s.VIPRenewalDate);
                    break;
                default:
                    vips = vips.OrderBy(s => s.VIPName);
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

            return View(vips.ToPagedList(pageNumber, myPageSize));
        }

        // GET: VIPs/Details/5
        [Authorize(Roles = "Owner, Administrator, Super User")]
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
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: VIPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create([Bind(Include = "VIPID,VIPName,VIPNumber,VIPRenewalDate")] VIP vip)
        {
            if (ModelState.IsValid)
            {
                db.VIPs.Add(vip);
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "VIP " + vip.VIPName + " created";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem creating the VIP " + vip.VIPName;
                    return RedirectToAction("Index");
                }
            }

            return View(vip);
        }

        // GET: VIPs/Edit/5
        [Authorize(Roles = "Owner, Administrator")]
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
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit([Bind(Include = "VIPID,VIPName,VIPNumber,VIPRenewalDate")] VIP vip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vip).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "VIP " + vip.VIPName + " edited";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem editing the VIP " + vip.VIPName;
                    return RedirectToAction("Index");
                }
            }
            return View(vip);
        }

        // GET: VIPs/Delete/5
        [Authorize(Roles = "Owner, Administrator")]
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
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            VIP vip = db.VIPs.Find(id);
            db.VIPs.Remove(vip);
            try
            {
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "VIP " + vip.VIPName + " deleted";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["DangerOHMsg"] = "Problem deleting the VIP " + vip.VIPName;
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
