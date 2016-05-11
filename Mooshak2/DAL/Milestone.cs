namespace Mooshak2.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Milestone")]
    public partial class Milestone
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int AssignmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public int? Weight { get; set; }
    }
}
