using Mooshak2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mooshak2.DBL;
using Mooshak2.Models;
using Microsoft.AspNet.Identity;

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
            string strCurrentUserName = User.Identity.GetUserName();
            List <Assignment> model = new List <Assignment>();
            var query = from assignment in db.Assignments
                        join course in db.Courses on assignment.courseId equals course.Id
                        join stdnt in db.StudentCourseLists on course.Id equals stdnt.courseId
                        select assignment.Name;
            foreach (var assignment in query)
            {
                model.Add(new Assignment() {Name = assignment});
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id.HasValue)
            {
                Assignment assignment = db.Assignments.Where(x => x.Id== id.Value).SingleOrDefault();
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
    }
}