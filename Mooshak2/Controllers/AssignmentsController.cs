using Mooshak2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mooshak2.DBL;
using Mooshak2.Models.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.SqlClient;

namespace Mooshak2.Controllers
{
    public class AssignmentsController : Controller
    {
		private AssignmentsService _service = new AssignmentsService();

        private Context db = new Context();

        // GET: Assignments
        [Authorize]
        public ActionResult Index()
        {
            List<Assignment> model = new List<Assignment>();
            model = _service.GetAssignmentsInCourse();
            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Assignment assignment = db.Assignments.Where(x => x.Id == id.Value).SingleOrDefault();
                if (assignment != null)
                {
                    Assignment model = new Assignment();
                    model.Id = assignment.Id;
                    model.Name = assignment.Name;
                    model.Description = assignment.Description;
                    model.DateOfSubmittion = assignment.DateOfSubmittion;
                    model.DateOfAssigned = assignment.DateOfAssigned;
                    model.AllowedProgrammingLanguage = assignment.AllowedProgrammingLanguage;
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        /*[Authorize(Roles = "Admin, Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Assignment model)
        {
            if (ModelState.IsValid)
            {
                var assign = new Assignment { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }*/

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        public ActionResult Details(int id)
        {
            Assignment model = new Assignment();
            model = _service.GetAssignmentByID(id);
            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Delete(int id)
        {
            Assignment model = new Assignment();
            model = _service.GetAssignmentByID(id);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Assignment assignment = db.Assignments.Find(id);
            db.Assignments.Remove(assignment);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}