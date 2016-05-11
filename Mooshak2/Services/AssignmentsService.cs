using Mooshak2.Models;
using Mooshak2.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Mooshak2.DBL;

namespace Mooshak2.Services
{
	public class AssignmentsService
	{
		private Context db = new Context();

        public List<Assignment> GetAssignmentsInCourse()
        {
            var AssignList = GetAllAssignments();
            var CourseList = GetAllCourses();
            var StudentList = GetAllStudentList();

            List<Assignment> model = new List<Assignment>();
            var query = from assignment in AssignList
                        join course in CourseList on assignment.courseId equals course.Id
                        join stdnt in StudentList on course.Id equals stdnt.courseId
                        select new { assignment.Id, assignment.Name, assignment.Description, assignment.courseId };

            foreach (var assignment in query)
            {
                model.Add(new Assignment() { Name = assignment.Name, Id = assignment.Id, Description = assignment.Description, courseId = assignment.courseId });
            }
            return model;
        }

        public List<Assignment> GetAllAssignments()
        {
            return db.Assignments.ToList();
        }

        public List<Course> GetAllCourses()
        {
            return db.Courses.ToList();
        }

        public List<StudentCourseList> GetAllStudentList()
        {
            return db.StudentCourseLists.ToList();
        }

        public Assignment GetAssignmentByID(int assignmentID)
		{
			Assignment assignment = db.Assignments.SingleOrDefault(x => x.Id == assignmentID);
			return assignment;
		}

	}
}
