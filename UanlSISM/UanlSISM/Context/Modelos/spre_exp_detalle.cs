namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class spre_exp_detalle
    {
        [Column(TypeName = "numeric")]
        public decimal? folio_pre { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? id_pre_cat { get; set; }

        [StringLength(100)]
        public string indicaciones { get; set; }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }
    }
}
