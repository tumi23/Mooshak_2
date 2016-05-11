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
    public class CourseService
    {
        private Context db = new Context();
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

        public List<Course> GetAllCourses()
        {
            return db.Course.ToList();
        }
    }
}