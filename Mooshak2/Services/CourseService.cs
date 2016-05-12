using Mooshak2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mooshak2.DAL;
using Mooshak2.Services;

namespace Mooshak2.Services
{
    public class CourseService
    {
        private Context db = new Context();

        private AssignmentsService aService = new AssignmentsService();

        public AssignmentCreateViewModel GetDropDownListCourses()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<Course> course = GetAllCourses();
            foreach (var item in course)
            {
                items.Add(new SelectListItem { Text = item.Name, Value = Convert.ToString(item.Id), Selected = true });
            }

            var model = new AssignmentCreateViewModel();
            {
                model.ListCourses = items;
            }
            return model;
        }

        public List<CourseStudentViewModel> GetCourseStudentList(int id)
        {
            List<CourseStudentViewModel> model = new List<CourseStudentViewModel>();
            var query = from user in db.AspNetUsers
                        join stdntCourseList in db.StudentCourseList on user.UserName equals stdntCourseList.UserName
                        join course in db.Course on stdntCourseList.courseId equals course.Id
                        where course.Id == id
                        select new CourseStudentViewModel {userId = user.Id, UserName = user.UserName, Email = user.Email };
            foreach (var student in query)
            {
                model.Add(new CourseStudentViewModel {userId = student.userId, UserName = student.UserName, Email = student.Email });
            }
            return model;
        }

        public void CourseDelete(int id)//Deletear sjálfum sér og StudentCourseList entryum tengd sjálfum sér og öllum assignments tengd sjálfum sér
        {
            Course course = db.Course.Find(id);
            List<Assignment> AssignmentsForDeletion = new List<Assignment>();
            AssignmentsForDeletion = GetAllAssignmentEntries(id);
            foreach(var assign in AssignmentsForDeletion)
            {
                aService.AssignmentDelete(assign.Id);
            }
            db.Course.Remove(course);
            db.SaveChanges();
        }

        public List<Assignment> GetAllAssignmentEntries(int courseId)
        {
            var query = from assign in db.Assignment
                        join assignList in db.AssignmentList on assign.Id equals assignList.AssignmentId
                        where (assignList.courseId == courseId)
                        select assign;
            List<Assignment> assignmentEntries = new List<Assignment>();
            foreach (var assign in query)
            {
                assignmentEntries.Add(new Assignment { Id = assign.Id, Name = assign.Name, Description = assign.Description, DateOfAssigned = assign.DateOfAssigned, DateOfSubmittion = assign.DateOfSubmittion, AllowedProgrammingLanguage = assign.AllowedProgrammingLanguage});
            }
            return assignmentEntries;
        }

        public List<Course> GetAllCourses()
        {
            return db.Course.ToList();
        }
    }
}