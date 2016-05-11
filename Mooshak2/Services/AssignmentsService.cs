using Mooshak2.Models;
using Mooshak2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mooshak2.DAL;
using Mooshak2.Services;

namespace Mooshak2.Services
{
	public class AssignmentsService
	{
		private Context db = new Context();
        private CourseService cService = new CourseService();

        public List<Assignment> GetAssignmentsInCourse()
        {
            var AssignList = GetAllAssignments();
            var CourseList = cService.GetAllCourses();
            var StudentList = GetAllStudentList();

            List<Assignment> model = new List<Assignment>();
            var query = from assignment in AssignList
                        orderby assignment.Id descending
                        select new { assignment.Id, assignment.Name, assignment.Description };
            /*var query = from assignment in AssignList
                        join course in CourseList on assignment.courseId equals course.Id
                        join stdnt in StudentList on course.Id equals stdnt.courseId
                        select new { assignment.Id, assignment.Name, assignment.Description, assignment.courseId };*/

            foreach (var assignment in query)
            {
                model.Add(new Assignment() { Name = assignment.Name, Id = assignment.Id, Description = assignment.Description });
            }
            return model;
        }

        public List<Assignment> GetAllAssignments()
        {
            return db.Assignment.ToList();
        }

        public List<StudentCourseList> GetAllStudentList()
        {
            return db.StudentCourseList.ToList();
        }

        public Assignment GetAssignmentByID(int assignmentID)
		{
			Assignment assignment = db.Assignment.SingleOrDefault(x => x.Id == assignmentID);
			return assignment;
		}

	}
}
