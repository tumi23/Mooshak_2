namespace Mooshak2.DAL
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

        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime DateOfAssigned { get; set; }

        public DateTime DateOfSubmittion { get; set; }

        [Required]
        [StringLength(250)]
        public string AllowedProgrammingLanguage { get; set; }
    }
}
