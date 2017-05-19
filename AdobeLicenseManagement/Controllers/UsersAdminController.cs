using AdobeLicenseManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Mail;

namespace IdentitySample.Controllers
{
    [Authorize]
    public class UsersAdminController : Controller
    {
        public RoleStore<IdentityRole> roleStore;
        public RoleManager<IdentityRole> roleManager;
        public UserStore<ApplicationUser> userStore;
        public UserManager<ApplicationUser> userManager;
        ApplicationDbContext db = new ApplicationDbContext();

        public UsersAdminController()
        {
            roleStore = new RoleStore<IdentityRole>(db);
            roleManager = new RoleManager<IdentityRole>(roleStore);
            userStore = new UserStore<ApplicationUser>(db);
            userManager = new UserManager<ApplicationUser>(userStore);
        }

        //
        // GET: /Users/
        [Authorize(Roles = "Owner, Administrator")]
        public async Task<ActionResult> Index()
        {
            ViewData["Current User"] = User.Identity.GetUserName();
            return View(await userManager.Users.ToListAsync());
        }



        //
        // GET: /Users/Details/5
        [Authorize(Roles = "Owner, Administrator")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await userManager.FindByIdAsync(id);

            ViewBag.RoleNames = await userManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create/1
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create()
        {
            return View(new CreateUserViewModel()
            {
                RolesList = roleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public ActionResult Create([Bind(Include = "Email")] CreateUserViewModel createUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                // Create new user
                var user = new ApplicationUser();
                user.Email = createUser.Email;
                user.UserName = createUser.Email.Split('@')[0];
                string defaultPwd = "utep123#";    // Default password, users must change it

                /*
                // Send email to the new user
                string senderName = "Adobe License Management Administrator"
                string senderEmail = "adobeLicenseManagement@gmail.com";
                string senderEmailPassword = "YU9CEGobYi3w";

                var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
                string messageString =
                    "Your account for Adobe License Management has been created.\n" +
                    "Please log in with the password " + defaultPwd + " for your " +
                    "first time use.\n Please reset your password once you log in.";
                var message = new MailMessage();
                message.To.Add(new MailAddress(user.Email));
                message.From = new MailAddress(senderEmail);
                message.Subject = "Your Adobe License Management account has been created";
                message.Body = string.Format(body, senderName, senderEmail, messageString);
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = senderEmail,  // replace with valid value
                        Password = senderEmailPassword  // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
                */

                try
                {
                    var newUser = userManager.Create(user, defaultPwd);

                    // Add user to a role
                    if (newUser.Succeeded)
                    {
                        selectedRole = selectedRole ?? new string[] { };    // if selectedRole is not null use that, otherwise create new empty string
                        var result1 = userManager.AddToRole(user.Id, selectedRole[0]);
                    }

                    TempData["SuccessOHMsg"] = "User " + user.UserName + " created. An email has been sent to them with their first login password.";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem creating the User " + user.UserName;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        //
        // GET: /Users/Edit/1
        [Authorize(Roles = "Owner, Administrator")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userRoles = await userManager.GetRolesAsync(user.Id);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                RolesList = roleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public async Task<ActionResult> Edit([Bind(Include = "Email,UserName,Id")] EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                
                user.Email = editUser.Email;
                user.UserName = editUser.Email.Split('@')[0];

                var userRoles = await userManager.GetRolesAsync(user.Id);

                selectedRole = selectedRole ?? new string[] { };        // if selectedRole is not null use that, otherwise create new empty string

                var result = await userManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await userManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                TempData["SuccessOHMsg"] = user.UserName + " role changed to " + selectedRole[0];
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        [Authorize(Roles = "Owner, Administrator")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                TempData["SuccessOHMsg"] = "User " + user.UserName + " deleted";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}