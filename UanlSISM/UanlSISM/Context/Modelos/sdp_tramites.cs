namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sdp_tramites
    {
        public int id { get; set; }

        [StringLength(8)]
        public string expediente { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? id_tramite { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? anio { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? id_consec { get; set; }

        [StringLength(10)]
        public string realiza { get; set; }

        [StringLength(100)]
        public string conyugue { get; set; }

        [StringLength(1000)]
        public string diagnostico { get; set; }

        [StringLength(1)]
        public string estudio { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fechaest1 { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fechaest2 { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fechaest3 { get; set; }

        [StringLength(6)]
        public string medico { get; set; }
    }
}
