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
}