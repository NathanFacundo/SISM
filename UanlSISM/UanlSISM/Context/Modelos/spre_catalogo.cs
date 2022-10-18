namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class spre_catalogo
    {
        [Key]
        public int id_pre_cat { get; set; }

        [StringLength(100)]
        public string desc_pre_cat { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tipo_pre_cat { get; set; }
    }
}
