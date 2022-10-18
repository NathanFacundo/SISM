namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class lab_catalogo
    {
        [Key]
        [StringLength(4)]
        public string lab_id { get; set; }

        [Required]
        [StringLength(90)]
        public string lab_des { get; set; }

        [Required]
        [StringLength(50)]
        public string lab_nivel { get; set; }

        [Required]
        [StringLength(5)]
        public string lab_lab { get; set; }

        [Required]
        [StringLength(3)]
        public string lab_grupo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? lab_instrucciones { get; set; }
    }
}
