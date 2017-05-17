using AdobeLicenseManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Reflection;
using System.Net;

namespace AdobeLicenseManagement.Controllers
{
    /*
     * A class that will allow multiple buttons for the query textarea
     * Reference: http://stackoverflow.com/questions/442704/how-do-you-handle-multiple-submit-buttons-in-asp-net-mvc-framework
     */
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MultipleButtonAttribute : ActionNameSelectorAttribute
    {
        public string Name { get; set; }
        public string Argument { get; set; }

        public override bool IsValidName(ControllerContext controllerContext, string actionName, MethodInfo methodInfo)
        {
            var isValidName = false;
            var keyValue = string.Format("{0}:{1}", Name, Argument);
            var value = controllerContext.Controller.ValueProvider.GetValue(keyValue);

            if (value != null)
            {
                controllerContext.Controller.ControllerContext.RouteData.Values[Name] = Argument;
                isValidName = true;
            }

            return isValidName;
        }
    }

    [Authorize(Roles = "Owner, Administrator, Super User")]
    public class QueryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Test()
        {
            return View();
        }

        // GET: Query
        public ActionResult Index([Bind(Include = "Queries,SavedQueries,Query,Description")] QueryViewModel qvm)
        {
            if(qvm.SavedQueries == null)
            {

                qvm = new QueryViewModel();
                qvm.SavedQueries = db.SavedQueries.ToList();
            }
            return View(qvm);
        }

        // GET: SavedQueryRan/5
        // This function is used to translate an ID into a QVM that can be passed to the Run action
        public ActionResult SavedQueryRan(int? id)
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
            QueryViewModel qvm = new QueryViewModel();
            qvm.SavedQueries = db.SavedQueries.ToList();
            qvm.Query = savedQuery.Query;
            return Run(qvm);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Run")]
        [ValidateAntiForgeryToken]
        public ActionResult Run([Bind(Include = "Queries,SavedQueries,Query,Description")] QueryViewModel qvm)
        {
            

            if(qvm.Query == null)
            {
                TempData["DangerOHMsg"] = "There was a problem running the query";
                return RedirectToAction("Index");
            }
            if (qvm.Query.Length >= 6 && qvm.Query.Substring(0, 6) == "SELECT"
                && qvm.Query.Substring(0, qvm.Query.Length - 1) != ";" && qvm.Query.Substring(qvm.Query.Length - 1, 1) == ";")
            {
                try
                {
                    IEnumerable<QueryDTOViewModel> data = db.Database.SqlQuery<QueryDTOViewModel>(qvm.Query);
                    qvm.Queries = data.ToList();
                    qvm.SavedQueries = db.SavedQueries.ToList();
                    TempData["SuccessOHMsg"] = "The Query was successfully ran";
                    return View("Index", qvm);
                }
                catch (Exception e)
                {
                    TempData["DangerOHMsg"] = e.ToString();
                    //TempData["DangerOHMsg"] = "There was a problem running the query";
                    return RedirectToAction("Index", qvm);
                }
            }

            TempData["DangerOHMsg"] = "There was a problem with the format of the query";
            return RedirectToAction("Index", qvm);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Save")]
        [ValidateAntiForgeryToken]
        public ActionResult Save([Bind(Include = "Queries,SavedQueries,Query,Description")] QueryViewModel qvm)
        {
            SavedQuery savQuery = new SavedQuery();
            // Check for valid Query. Must be SELECT Command, and only has a one semicolon at the end of the query
            string Query = qvm.Query;
            if(Query.Length >= 6 && Query.Substring(0, 6) == "SELECT"
                && Query.Substring(0, Query.Length - 1) != ";" && Query.Substring(Query.Length - 1, 1) == ";")
            {
                savQuery.Query = Query;
                savQuery.Description = qvm.Description;
                savQuery.CreationDate = DateTime.Today;
                db.SavedQueries.Add(savQuery);
                try
                {
                    db.SaveChanges();
                    TempData["SuccessOHMsg"] = "Saved Query " + savQuery.SavedQueryID + " added";
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem saving query in the database. Please remember to enter a description before saving.";
                    return RedirectToAction("Index", qvm);
                }
            }
            else
            {
                TempData["DangerOHMsg"] = "There was a problem with the format of the query";
                return RedirectToAction("Index", qvm);
            }
            return Run(qvm);
        }
    }
}