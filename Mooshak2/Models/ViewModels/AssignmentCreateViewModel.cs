using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mooshak2.DAL;
using System.Web.Mvc;

namespace Mooshak2.Models.ViewModels
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
        public decimal? FinalGrade { get; set; }

        public IEnumerable<SelectListItem> ListCourses { get; set; }
    }
}