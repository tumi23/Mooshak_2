namespace Mooshak2.DBL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Assignment")]
    public partial class Assignment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int courseId { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DateOfAssigned { get; set; }

        public DateTime DateOfSubmittion { get; set; }

        [Required]
        [StringLength(250)]
        public string AllowedProgrammingLanguage { get; set; }

        public decimal? FinalGrade { get; set; }

        public virtual Course Course { get; set; }
    }
}
