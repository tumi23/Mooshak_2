using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Mooshak2.DAL;
using System.Web.Mvc;
using System;

namespace Mooshak2.Models
{
    public class AssignmentCreateViewModel
    {
        public int Id { get; set; }
        public string courseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateOfAssigned { get; set; }
        public DateTime DateOfSubmittion { get; set; }
        public string AllowedProgrammingLanguage { get; set; }

        public IEnumerable<SelectListItem> ListCourses { get; set; }
    }

    public class AssignmentViewModel
	{
        public Assignment Assignments { get; set; }
        public Course Courses { get; set; }
	}

    public class AssignmentMilestoneViewModel
    {
        public string Title { get; set; }
    }
}
