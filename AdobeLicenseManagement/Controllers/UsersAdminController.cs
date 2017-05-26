using AdobeLicenseManagement.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net.Mail;
using AdobeLicenseManagement;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Security.Cryptography;
using System.Text;

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
        
        // POST: /Users/Create/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Owner, Administrator")]
        public async Task<ActionResult> CreateAsync([Bind(Include = "Email")] CreateUserViewModel createUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                // Create new user
                var user = new ApplicationUser();
                user.Email = createUser.Email;
                user.UserName = createUser.Email.Split('@')[0];
                //string defaultPwd = GetUniqueKey(8);    // Default password, users must change it
                string defaultPwd = "utep#123";     // Default password, users must change it

                /* Used when sending email to new user created. I removed this for simplicity
                var callbackUrl = Url.Action("Login", "Account", null, protocol: Request.Url.Scheme);
                string message = "An account for Adobe License Management has been created for you by " + User.Identity.GetUserName()
                    + ".<br />Your username is <strong>" + user.UserName + "</strong> and your default password is <strong>" + defaultPwd
                    + "</strong>.<br />Please change the password once you log in by clicking on your username in the top-right and then clicking reset password"
                    + ".<br />Here is a link to the <a href=\"" + callbackUrl + "\">Adobe License Management</a> website. ";
                await SendEmail(user.Email, "Adobe License Management account created", message);
                */

                try
                {
                    var newUser = userManager.Create(user, defaultPwd);

                    // Add user to a role
                    if (newUser.Succeeded)
                    {
                        if(selectedRole != null)
                        {
                            selectedRole = selectedRole ?? new string[] { };    // if selectedRole is not null use that, otherwise create new empty string
                            var result1 = userManager.AddToRole(user.Id, selectedRole[0]);
                        }

                        // Used when sending email to new user created. I removed this for simplicity
                        //TempData["SuccessOHMsg"] = "User " + user.UserName + " created. An email has been sent to them with their first login password." + "Please remind them to check their spam/junk folder.";
                        TempData["SuccessOHMsg"] = "User " + user.UserName + " created. Please let them know that their temporary password is <b>" + defaultPwd + "</b> and they must change it after they log in";
                    }
                    else
                    {
                        TempData["DangerOHMsg"] = "Problem creating the User " + user.UserName;
                    }
                }
                catch
                {
                    TempData["DangerOHMsg"] = "Problem creating the User " + user.UserName;
                }
            }

            return RedirectToAction("Index");
        }

        /* Used this to send an email to the user created with a random temporary password and a link to the website
         * I removed it because I needed to include an API key and it complicated things too much.
         * If you would like to implement this, please refer to https://docs.microsoft.com/en-us/aspnet/identity/overview/features-api/account-confirmation-and-password-recovery-with-aspnet-identity
        // Generates a random alphanumeric string of size maxSize
        // Reference: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings-in-c
        private string GetUniqueKey(int size)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        
        private async Task SendEmail(string destination, string subject, string message)
        {
            // The apiKey needs to be saved in an external file that is not uploaded to cloud server (i.e., Github)
            // For more info, please see https://docs.microsoft.com/en-us/aspnet/identity/overview/features-api/best-practices-for-deploying-passwords-and-other-sensitive-data-to-aspnet-and-azure
            var apiKey = "";       // Remove later
            System.Configuration.Configuration rootWebConfig1 =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(null);
            if (rootWebConfig1.AppSettings.Settings.Count > 0)
            {
                apiKey = rootWebConfig1.AppSettings.Settings["SendGridAPIKey"].Value;
            }

            var client = new SendGridClient(apiKey);
            var myMessage = new SendGridMessage();
            myMessage.SetFrom(new EmailAddress("mamorales15@miners.utep.edu", "Adobe License Management"));
            myMessage.AddTo(destination);
            myMessage.SetSubject(subject);
            myMessage.AddContent(MimeType.Text, message);
            myMessage.AddContent(MimeType.Html, message);
            var response = await client.SendEmailAsync(myMessage);
        }
        */

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