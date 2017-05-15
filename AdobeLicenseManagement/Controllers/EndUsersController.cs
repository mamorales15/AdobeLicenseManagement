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
    public class EndUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EndUsers
        [Authorize(Roles = "Owner, Administrator, SuperUser")]
        public ActionResult Index()
        {
            return View(db.EndUsers.ToList());
        }

        // GET: EndUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUser endUser = db.EndUsers.Find(id);
            if (endUser == null)
            {
                return HttpNotFound();
            }
            return View(endUser);
        }

        // GET: EndUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUser endUser = db.EndUsers.Find(id);

            // !!!TODO!!! Make a EndUserViewModel out of an EndUser. So that you can pass an EndUserViewModel to the View.
            // The View requires an EndUserViewModel because I am asking for the RequestID which is in the EndUserViewModel
            // EndUser only has a virtual Request which makes it difficult to create a dropdownlist (I think). Not really sure.

            if (endUser == null)
            {
                return HttpNotFound();
            }

            EndUserViewModel endUserVM = new EndUserViewModel();
            endUserVM.AdobeID = endUser.AdobeID;
            endUserVM.Building = endUser.Building;
            endUserVM.ComputerName = endUser.ComputerName;
            endUserVM.ComputerSerial = endUser.ComputerSerial;
            endUserVM.Email = endUser.Email;
            endUserVM.EndUserID = endUser.EndUserID;
            endUserVM.RequestID = endUser.Request.RequestID;
            endUserVM.RmNo = endUser.RmNo;
            endUserVM.Tag = endUser.Tag;
            endUserVM.Username = endUser.Username;

            // TODO: Might want to change the dataTextField to another attribute of Request that is unique but also identifiable
            ViewBag.RequestList = new SelectList(db.Requests.OrderBy(x => x.RequestID), "RequestID", "RequestID");

            return View(endUserVM);
        }

        // POST: EndUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EndUserID,Username,Email,Building,RmNo,Tag,ComputerSerial,ComputerName,AdobeID,RequestID")] EndUserViewModel endUserVM)
        {
            if (ModelState.IsValid)
            {
                EndUser endUser = db.EndUsers.Find(endUserVM.EndUserID);
                endUser.Username = endUserVM.Username;
                endUser.Email = endUserVM.Email;
                endUser.Building = endUserVM.Building;
                endUser.RmNo = endUserVM.RmNo;
                endUser.Tag = endUserVM.Tag;
                endUser.ComputerSerial = endUserVM.ComputerSerial;
                endUser.ComputerName = endUserVM.ComputerName;
                endUser.AdobeID = endUserVM.AdobeID;
                Request req = db.Requests.Find(endUserVM.RequestID);
                endUser.Request = req;
                db.Entry(endUser).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "End User " + endUser.Username + " edited";
                return RedirectToAction("Index");
            }

            // TODO: Might want to change the dataTextField to another attribute of Request that is unique but also identifiable
            ViewBag.RequestList = new SelectList(db.Requests.OrderBy(x => x.RequestID), "RequestID", "RequestID");

            return View(endUserVM);
        }

        // GET: EndUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EndUser endUser = db.EndUsers.Find(id);
            if (endUser == null)
            {
                return HttpNotFound();
            }
            return View(endUser);
        }

        // POST: EndUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EndUser endUser = db.EndUsers.Find(id);
            db.EndUsers.Remove(endUser);
            db.SaveChanges();
            TempData["SuccessOHMsg"] = "End User " + endUser.Username + " deleted";
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
