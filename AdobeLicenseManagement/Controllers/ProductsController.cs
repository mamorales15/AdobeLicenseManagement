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
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Products/Details/5
        [Authorize(Roles = "Owner, Administrator, Super User")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create([Bind(Include = "ProductID,ProductDesc")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "Product " + product.ProductDesc + " created";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem creating the Product " + product.ProductDesc;
                    return RedirectToAction("Index");
                }
            }

            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Edit([Bind(Include = "ProductID,ProductDesc")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "Product " + product.ProductDesc + " edited";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem editing the Product " + product.ProductDesc;
                    return RedirectToAction("Index");
                }
            }
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            try
            {
                db.SaveChanges();
                TempData["SuccessOHMsg"] = "Product " + product.ProductDesc + " deleted";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["DangerOHMsg"] = "Problem deleting the Product " + product.ProductDesc;
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
