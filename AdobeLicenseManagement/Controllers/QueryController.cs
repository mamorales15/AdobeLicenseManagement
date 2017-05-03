using AdobeLicenseManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;

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

    public class QueryController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Query
        public ActionResult Index()
        {
            QueryViewModel qvm = new QueryViewModel();
            qvm.SavedQueries = db.SavedQueries.ToList();
            return View(qvm);
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Run")]
        public ActionResult Run([Bind(Include = "Queries,SavedQueries,Query")] QueryViewModel qvm)
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
                    TempData["DangerOHMsg"] = "There was a problem running the query";
                    return RedirectToAction("Index");
                }
            }

            TempData["DangerOHMsg"] = "There was a problem running the query";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Save")]
        public ActionResult Save(string Query, string Description)
        {
            SavedQuery savQuery = new SavedQuery();
            // Check for valid Query. Must be SELECT Command, and only has a one semicolon at the end of the query
            if(Query.Length >= 6 && Query.Substring(0, 6) == "SELECT"
                && Query.Substring(0, Query.Length - 1) != ";" && Query.Substring(Query.Length, 1) == ";")
            {
                savQuery.Description = Description;
                db.SavedQueries.Add(savQuery);
                TempData["SuccessOHMsg"] = "Saved Query " + savQuery.SavedQueryID + " added";
            }
            else
            {
                TempData["DangerOHMsg"] = "Problem saving query in the database";
            }
            return RedirectToAction("Index");
        }
    }
}