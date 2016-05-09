namespace Mooshak2.DBL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AssignmentList")]
    public partial class AssignmentList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int courseId { get; set; }

        public int AssignmentId { get; set; }
    }
}
