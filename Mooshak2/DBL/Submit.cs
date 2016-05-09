namespace Mooshak2.DBL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Submit")]
    public partial class Submit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int userId { get; set; }

        public int assignId { get; set; }

        [Required]
        public string Code { get; set; }

        public DateTime DateOfSubmit { get; set; }
    }
}
