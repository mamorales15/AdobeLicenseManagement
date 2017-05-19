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
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Index()
        {
            return View(db.EndUsers.ToList());
        }

        // GET: EndUsers/Details/5
        [Authorize(Roles = "Owner, Administrator, Super User")]
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
        [Authorize(Roles = "Owner, Administrator")]
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
            if (endUser.Request != null)
                endUserVM.BuildingID = endUser.Building.BuildingID;
            endUserVM.ComputerName = endUser.ComputerName;
            endUserVM.ComputerSerial = endUser.ComputerSerial;
            endUserVM.Email = endUser.Email;
            endUserVM.EndUserID = endUser.EndUserID;
            if(endUser.Request != null)
                endUserVM.RequestID = endUser.Request.RequestID;
            endUserVM.RmNo = endUser.RmNo;
            endUserVM.Tag = endUser.Tag;
            endUserVM.UserName = endUser.UserName;

            // TODO: Might want to change the dataTextField to another attribute of Request that is unique but also identifiable
            ViewBag.RequestList = new SelectList(db.Requests.OrderBy(x => x.RequestID), "RequestID", "RequestID");
            ViewBag.BuildingList = new SelectList(db.Buildings.OrderBy(x => x.BuildingID), "BuildingID", "BuildingName");
            return View(endUserVM);
        }

        // POST: EndUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit([Bind(Include = "EndUserID,UserName,Email,BuildingID,RmNo,Tag,ComputerSerial,ComputerName,AdobeID,RequestID")] EndUserViewModel endUserVM)
        {
            if (ModelState.IsValid)
            {
                EndUser endUser = db.EndUsers.Find(endUserVM.EndUserID);
                endUser.UserName = endUserVM.UserName;
                endUser.Email = endUserVM.Email;
                Building build = db.Buildings.Find(endUserVM.BuildingID);
                endUser.Building = build;
                endUser.RmNo = endUserVM.RmNo;
                endUser.Tag = endUserVM.Tag;
                endUser.ComputerSerial = endUserVM.ComputerSerial;
                endUser.ComputerName = endUserVM.ComputerName;
                endUser.AdobeID = endUserVM.AdobeID;
                Request req = db.Requests.Find(endUserVM.RequestID);
                endUser.Request = req;
                db.Entry(endUser).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "End User " + endUser.UserName + " edited";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem editing the End User" + endUser.UserName;
                    return RedirectToAction("Index");
                }
            }

            // TODO: Might want to change the dataTextField to another attribute of Request that is unique but also identifiable
            ViewBag.RequestList = new SelectList(db.Requests.OrderBy(x => x.RequestID), "RequestID", "RequestID");
            ViewBag.BuildingList = new SelectList(db.Buildings.OrderBy(x => x.BuildingID), "BuildingID", "BuildingName");
            return View(endUserVM);
        }

        // GET: EndUsers/Delete/5
        [Authorize(Roles = "Owner, Administrator")]
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
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            EndUser endUser = db.EndUsers.Find(id);
            db.EndUsers.Remove(endUser);
            try
            {
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "End User " + endUser.UserName + " deleted";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["DangerOHMsg"] = "Problem deleting the End User" + endUser.UserName;
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
