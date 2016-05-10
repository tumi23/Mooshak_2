using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooshak2.Models.ViewModels
{
	public class AssignmentViewModel
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CourseName { get; set; }

        public List<AssignmentMilestoneViewModel> Milestones { get; set; }
	}
}
