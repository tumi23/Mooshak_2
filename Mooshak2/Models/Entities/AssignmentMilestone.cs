namespace Mooshak2.Models.Entities
{
	/// <summary>
	/// An AssignmentMilestone represents a part of an assignment.
	/// Each assignment may contain multiple milestones, where
	/// each milestone weight certain percentage of the final grade
	/// of the assignment.
	/// </summary>
	public class AssignmentMilestone
	{
		/// <summary>
		/// The database-generated unique ID of the milestone.
		/// </summary>
		public int ID { get; set; }

		/// <summary>
		/// A foreign key to the assignment.
		/// </summary>
		public int AssignmentID { get; set; }

		/// <summary>
		/// The title of the milestone. Example: "Part 1"
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Determines how much this milestone weights in the assignment.
		/// Example: if this milestone is 15% of the grade of the assignment,
		/// then this property contains the value 15.
		/// </summary>
		public int Weight { get; set; }
	}
}
