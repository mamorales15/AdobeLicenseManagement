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
    public class EndUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: EndUsers
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageSize)
        {
            ViewBag.CurrentSort = sortOrder;

            ViewBag.UserNameSortParam = String.IsNullOrEmpty(sortOrder) ? "userName_desc" : sortOrder == "default" ? "userName_desc" : "default";
            ViewBag.EmailSortParam = sortOrder == "email_asc" ? "email_desc" : "email_asc";
            ViewBag.RmNoSortParam = sortOrder == "rmNo_asc" ? "rmNo_desc" : "rmNo_asc";
            ViewBag.TagSortParam = sortOrder == "tag_asc" ? "tag_desc" : "tag_asc";
            ViewBag.ComputerSerialSortParam = sortOrder == "computerSerial_asc" ? "computerSerial_desc" : "computerSerial_asc";
            ViewBag.ComputerNameSortParam = sortOrder == "computerName_asc" ? "computerName_desc" : "computerName_asc";
            ViewBag.AdobeIDSortParam = sortOrder == "adobeID_asc" ? "adobeID_desc" : "adobeID_asc";
            ViewBag.RequestIDSortParam = sortOrder == "requestID_asc" ? "requestID_desc" : "requestID_asc";
            ViewBag.BuildingNameSortParam = sortOrder == "buildingName_asc" ? "buildingName_desc" : "buildingName_asc";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var endUsers = from s in db.EndUsers
                                       select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                endUsers = endUsers.Where(s => s.EndUserID.ToString().Contains(searchString)
                                                                || s.UserName.ToString().Contains(searchString)
                                                                || s.Email.ToString().Contains(searchString)
                                                                || s.RmNo.ToString().Contains(searchString)
                                                                || s.Tag.ToString().Contains(searchString)
                                                                || s.ComputerSerial.ToString().Contains(searchString)
                                                                || s.ComputerName.ToString().Contains(searchString)
                                                                || s.AdobeID.ToString().Contains(searchString)
                                                                || s.Request.RequestID.ToString().Contains(searchString)
                                                                || s.Building.BuildingName.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "userName_desc":
                    endUsers = endUsers.OrderByDescending(s => s.UserName);
                    break;
                case "email_desc":
                    endUsers = endUsers.OrderByDescending(s => s.Email);
                    break;
                case "email_asc":
                    endUsers = endUsers.OrderBy(s => s.Email);
                    break;
                case "rmNo_desc":
                    endUsers = endUsers.OrderByDescending(s => s.RmNo);
                    break;
                case "rmNo_asc":
                    endUsers = endUsers.OrderBy(s => s.RmNo);
                    break;
                case "tag_desc":
                    endUsers = endUsers.OrderByDescending(s => s.Tag);
                    break;
                case "tag_asc":
                    endUsers = endUsers.OrderBy(s => s.Tag);
                    break;
                case "computerSerial_desc":
                    endUsers = endUsers.OrderByDescending(s => s.ComputerSerial);
                    break;
                case "computerSerial_asc":
                    endUsers = endUsers.OrderBy(s => s.ComputerSerial);
                    break;
                case "computerName_desc":
                    endUsers = endUsers.OrderByDescending(s => s.ComputerName);
                    break;
                case "computerName_asc":
                    endUsers = endUsers.OrderBy(s => s.ComputerName);
                    break;
                case "adobeID_desc":
                    endUsers = endUsers.OrderByDescending(s => s.AdobeID);
                    break;
                case "adobeID_asc":
                    endUsers = endUsers.OrderBy(s => s.AdobeID);
                    break;
                case "requestID_desc":
                    endUsers = endUsers.OrderByDescending(s => s.Request.RequestID);
                    break;
                case "requestID_asc":
                    endUsers = endUsers.OrderBy(s => s.Request.RequestID);
                    break;
                case "buildingName_desc":
                    endUsers = endUsers.OrderByDescending(s => s.Request.RequestID);
                    break;
                case "buildingName_asc":
                    endUsers = endUsers.OrderBy(s => s.Request.RequestID);
                    break;
                default:
                    endUsers = endUsers.OrderBy(s => s.UserName);
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

            return View(endUsers.ToPagedList(pageNumber, myPageSize));
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
