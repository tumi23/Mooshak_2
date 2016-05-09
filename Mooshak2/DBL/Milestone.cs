namespace Mooshak2.DBL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Milestone")]
    public partial class Milestone
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Milestone()
        {
            MilestoneLists = new HashSet<MilestoneList>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int AssignmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public int? Weight { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MilestoneList> MilestoneLists { get; set; }
    }
}
