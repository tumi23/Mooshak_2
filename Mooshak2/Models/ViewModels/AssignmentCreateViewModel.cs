using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mooshak2.Models.ViewModels
{
    public class AssignmentCreateViewModel
    {
        public IEnumerable<Mooshak2.DBL.Assignment> Assignment { get; set; }
        public IEnumerable<Mooshak2.DBL.Course> Course { get; set; }
    }
}