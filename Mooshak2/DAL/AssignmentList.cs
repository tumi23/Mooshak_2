namespace Mooshak2.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AssignmentList")]
    public partial class AssignmentList
    {
        public int Id { get; set; }

        public int courseId { get; set; }

        public int AssignmentId { get; set; }
    }
}
