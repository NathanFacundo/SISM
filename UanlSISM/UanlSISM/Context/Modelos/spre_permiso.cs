namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class spre_permiso
    {
        [Key]
        public int id_pre_per { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? id_pre_cat { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? id_pre_tab { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? id_pre_reg { get; set; }
    }
}
