namespace Mooshak2.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MilestoneGradeList")]
    public partial class MilestoneGradeList
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int? userId { get; set; }

        public int? milestoneId { get; set; }

        public decimal? grade { get; set; }
    }
}
