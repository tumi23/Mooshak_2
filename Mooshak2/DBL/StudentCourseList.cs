namespace Mooshak2.DBL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentCourseList")]
    public partial class StudentCourseList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int userId { get; set; }

        public int courseId { get; set; }

        public virtual Course Course { get; set; }
    }
}
