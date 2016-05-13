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

        [Required]
        [StringLength(100, ErrorMessage = "There must be a name for the assignment")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "There must be a Description for the assignment")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "There must be a Date of Assigned in DD/MM/YYYY HH:MM:SS Format")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "DateOfAssigned")]
        public DateTime DateOfAssigned { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "There must be a Date of Submittion in DD/MM/YYYY HH:MM:SS Format")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy hh:mm:ss}", ApplyFormatInEditMode = true)]
        [Display(Name = "DateOfSubmittion")]
        public DateTime DateOfSubmittion { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "There must be a List for Allowed Programming languages for the assignment")]
        [Display(Name = "AllowedProgrammingLanguage")]
        public string AllowedProgrammingLanguage { get; set; }

        public IEnumerable<SelectListItem> ListCourses { get; set; }
    }

    public class AssignmentIndexViewModel
	{
        public Assignment Assignments { get; set; }
        public Course Courses { get; set; }
	}

    public class AssignmentMilestoneViewModel
    {
        public string Title { get; set; }
    }

    public class AssignmentDetailViewModel
    {
        public Assignment Assignments  { get; set; }
        public List<Milestone> Milestones { get; set; }
    }

    public class MilestoneCreateViewModel
    {
        public int id { get; set; }
        public string Title { get; set; }
        public int Weight { get; set; }
        public int assignmentId { get; set; }
    }
}
