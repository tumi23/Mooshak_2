using Mooshak2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Mooshak2.DAL;
using System.Web.UI.WebControls;
using Mooshak2.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.Security.Principal;

namespace Mooshak2.Controllers
{
    public class AssignmentsController : Controller
    {
		private AssignmentsService aService = new AssignmentsService();
        private CourseService cService = new CourseService();

        private Context db = new Context();

        // GET: Assignments
        [Authorize]
        public ActionResult Index()
        {
            return View(aService.GetAssignmentAndCourse());
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Assignment assignment = db.Assignment.Where(x => x.Id == id.Value).SingleOrDefault();
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

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Create()
        {
            return View(cService.GetDropDownListCourses());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Create(AssignmentCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                AssignmentCreateViewModel returnModel = new AssignmentCreateViewModel();
                returnModel = cService.GetDropDownListCourses();
                ModelState.AddModelError("", "All fields aren't filled out.");
                return View(returnModel);
            }
                var count = aService.GetAllAssignments().LastOrDefault();
                int id;
                if (count == null)
                    id = 0;
                else
                    id = count.Id + 1;
                db.Assignment.Add(new Assignment
                {
                    Id =  id,
                    Name = model.Name,
                    Description = model.Description,
                    DateOfAssigned = model.DateOfAssigned,
                    DateOfSubmittion = model.DateOfSubmittion,
                    AllowedProgrammingLanguage = model.AllowedProgrammingLanguage
                });
                db.AssignmentList.Add(new AssignmentList
                {
                    courseId = Convert.ToInt32(model.courseId),
                    AssignmentId = id
                });
                db.SaveChanges();
                return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult CreateMilestone(MilestoneCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                model = aService.CreateMilestone(model);
                aService.CreateMilestoneList(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult CreateMilestone(int id)
        {
            MilestoneCreateViewModel model = new MilestoneCreateViewModel();
            model.assignmentId = id;
            return View(model);
        }


        [Authorize]
        public ActionResult Details(int id)
        {
            AssignmentDetailViewModel model = new AssignmentDetailViewModel();
            model.Assignments = aService.GetAssignmentByID(id);
            model.Milestones = aService.GetAllMilestonesByAssignId(id);
            return View(model);
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult Delete(int id)
        {
            AssignmentDetailViewModel model = new AssignmentDetailViewModel();
            model.Assignments = aService.GetAssignmentByID(id);
            model.Milestones = aService.GetAllMilestonesByAssignId(id);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            aService.AssignmentDelete(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin, Teacher")]
        public ActionResult DeleteMilestone(int id)
        {
            Milestone model = new Milestone();
            model = aService.GetMilestoneById(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMilestone(Milestone model)
        {
            aService.MilestoneDelete(model.Id);
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Submit(int id)
        {
            Submit submit = new Submit();
            submit.MilestoneId = id;
            return View(submit);
        }

        /*[HttpPost]
        public ActionResult Submit(HttpPostedFileBase file)
        {
            string userName = User.Identity.GetUserName();
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = "C:\\Submits\\" + userName + "\\" + fileName;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                file.SaveAs(path);
            }
            //aService.RunCompilerCPlusPLus(userName);
            ViewBag.Message = "Upload successful";
            return RedirectToAction("Index");
        }*/

        [HttpPost]
        public ActionResult Submit()
        {
            string userName = User.Identity.GetUserName();
            aService.RunCompilerCPlusPLus(userName);
            ViewBag.Message = "Upload successful";
            return RedirectToAction("Index");
        }


    }
}