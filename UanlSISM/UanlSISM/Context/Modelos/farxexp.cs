namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("farxexp")]
    public partial class farxexp
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string expediente { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal tot_rctas { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "numeric")]
        public decimal anio { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "numeric")]
        public decimal mes { get; set; }
    }
}
