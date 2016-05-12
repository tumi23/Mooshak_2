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
    }
}