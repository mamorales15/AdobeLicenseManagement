using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AdobeLicenseManagement.Models;
using PagedList;
using System;

namespace AdobeLicenseManagement.Controllers
{
    [Authorize]
    public class ServiceDeskRequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ServiceDeskRequests
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageSize
)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.ServiceDeskRequestIDSortParam = String.IsNullOrEmpty(sortOrder) ? "serviceDeskRequestID_desc" : sortOrder == "default" ? "serviceDeskRequestID_desc" : "default";
            ViewBag.RequestIDSortParam = sortOrder == "requestID_asc" ? "requestID_desc" : "requestID_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var serviceDeskRequests = from s in db.ServiceDeskRequests.Include(i => i.Request)
                                       select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                serviceDeskRequests = serviceDeskRequests.Where(s => s.ServiceDeskRequestID.ToString().Contains(searchString)
                                                                || s.Request.RequestID.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "serviceDeskRequestID_desc":
                    serviceDeskRequests = serviceDeskRequests.OrderByDescending(s => s.ServiceDeskRequestID);
                    break;
                case "requestID_desc":
                    serviceDeskRequests = serviceDeskRequests.OrderByDescending(s => s.Request.RequestID);
                    break;
                case "requestID_asc":
                    serviceDeskRequests = serviceDeskRequests.OrderBy(s => s.Request.RequestID);
                    break;
                default:
                    serviceDeskRequests = serviceDeskRequests.OrderBy(s => s.ServiceDeskRequestID);
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

            return View(serviceDeskRequests.ToPagedList(pageNumber, myPageSize));
        }

        // GET: ServiceDeskRequests/Details/5
        [Authorize(Roles = "Owner, Administrator, Super User")]
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
        [Authorize(Roles = "Owner, Administrator")]
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
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create([Bind(Include = "ServiceDeskRequestID")] ServiceDeskRequest ServiceDeskRequest)
        {
            if (ModelState.IsValid)
            {
                db.ServiceDeskRequests.Add(ServiceDeskRequest);
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "ServiceDesk Request " + ServiceDeskRequest.ServiceDeskRequestID + " created";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem creating the ServiceDesk Request " + ServiceDeskRequest.ServiceDeskRequestID;
                    return RedirectToAction("Index");
                }
            }

            ViewBag.RequestID = new SelectList(db.Requests, "RequestID", "RequestID", ServiceDeskRequest.ServiceDeskRequestID);
            return View(ServiceDeskRequest);
        }

        // GET: ServiceDeskRequests/Edit/5
        [Authorize(Roles = "Owner, Administrator")]
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
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit([Bind(Include = "ServiceDeskRequestID,RequestID")] ServiceDeskRequestEditViewModel sdrEdit)
        {
            if (ModelState.IsValid)
            {
                ServiceDeskRequest sdr = db.ServiceDeskRequests.Find(sdrEdit.ServiceDeskRequestID);
                sdr.ServiceDeskRequestID = sdrEdit.ServiceDeskRequestID;
                if(sdr.Request != null)
                {
                    // Remove ServiceDesk Request from it's old Request's list
                    sdr.Request.ServiceDeskRequests.Remove(sdr);
                }
                // Get the new request
                sdr.Request = db.Requests.Find(sdrEdit.RequestID);
                // Add ServiceDesk Request to new Request's list
                sdr.Request.ServiceDeskRequests.Add(sdr);
                db.Entry(sdr).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "ServiceDesk Request " + sdrEdit.ServiceDeskRequestID + " edited";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem editing the ServiceDesk Request " + sdrEdit.ServiceDeskRequestID;
                    return RedirectToAction("Index");
                }
            }
            ViewBag.RequestID = new SelectList(db.Requests, "RequestID", "RequestID", sdrEdit.ServiceDeskRequestID);
            return View(sdrEdit);
        }

        // GET: ServiceDeskRequests/Delete/5
        [Authorize(Roles = "Owner, Administrator")]
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
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            ServiceDeskRequest ServiceDeskRequest = db.ServiceDeskRequests.Find(id);
            db.ServiceDeskRequests.Remove(ServiceDeskRequest);
            try
            {
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "ServiceDesk Request " + ServiceDeskRequest.ServiceDeskRequestID + " deleted";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["DangerOHMsg"] = "Problem deleting the ServiceDesk Request " + ServiceDeskRequest.ServiceDeskRequestID;
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
