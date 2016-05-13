namespace Mooshak2.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AssignmentGradeList")]
    public partial class AssignmentGradeList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [StringLength(250)]
        public string UserName { get; set; }

        public int? assignmentId { get; set; }

        public decimal? grade { get; set; }
    }
}
