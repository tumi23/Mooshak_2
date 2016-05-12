using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mooshak2.DAL;
using Mooshak2.Models;
using Mooshak2.Services;

namespace Mooshak2.Services
{
    public class UserService
    {
        private Context db = new Context();
        private CourseService cService = new CourseService();

        public  AddCourseViewModel GetDropDownListCourses()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            List<Course> course = cService.GetAllCourses();
            foreach (var item in course)
            {
                items.Add(new SelectListItem { Text = item.Name, Value = Convert.ToString(item.Id), Selected = true });
            }

            var model = new AddCourseViewModel();
            {
                model.ListCourses = items;
            }
            return model;
        }

        public List<StudentCourseList> GetAllStudentCourseList()
        {
            return db.StudentCourseList.ToList();
        }

        public List<StudentCourseList> GetAllStudentList()
        {
            return db.StudentCourseList.ToList();
        }

        public UserDetailViewModel GetUserDetailView(AspNetUser user)
        {
            List<Course> courses = new List<Course>();
            courses = GetStudentCoursesByUserName(user.UserName);
            UserDetailViewModel userModel = new UserDetailViewModel();
            userModel.user = user;
            userModel.courses = courses;

            return userModel;
        }

        public List<Course> GetStudentCoursesByUserName(string userName)
        {
            var query = from course in db.Course
                        join stdntCrsLst in db.StudentCourseList on course.Id equals stdntCrsLst.courseId
                        where (stdntCrsLst.UserName == userName)
                        select course;
            List<Course> courseList = new List<Course>();
            foreach (var course in query)
            {
                courseList.Add(new Course { Id = course.Id, Name = course.Name, Description = course.Description});
            }
            return courseList;
        }
    }
}