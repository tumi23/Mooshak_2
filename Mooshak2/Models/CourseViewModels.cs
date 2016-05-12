using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mooshak2.DAL;

namespace Mooshak2.Models
{
    public class CourseStudentViewModel
    {
        public string userId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class CourseDeleteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<CourseStudentViewModel> studentList { get; set; }

        public List<Assignment> assignments { get; set; }
    }

}