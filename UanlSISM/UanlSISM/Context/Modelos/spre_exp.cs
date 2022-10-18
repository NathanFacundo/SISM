namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class spre_exp
    {
        [Key]
        public int folio_pre { get; set; }

        [StringLength(5)]
        public string medico_crea { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_crea { get; set; }

        [StringLength(8)]
        public string expediente { get; set; }
    }
}
