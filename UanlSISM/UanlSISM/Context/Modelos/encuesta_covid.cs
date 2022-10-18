namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class encuesta_covid
    {
        [Key]
        [StringLength(8)]
        public string expediente { get; set; }

        [StringLength(5)]
        public string medico { get; set; }

        [StringLength(1)]
        public string veces_consulta { get; set; }

        [StringLength(1)]
        public string aislamiento { get; set; }

        [StringLength(1)]
        public string dif_respirar { get; set; }

        [StringLength(1)]
        public string int_requerido { get; set; }

        [StringLength(1)]
        public string oxi_requerido { get; set; }

        [StringLength(1)]
        public string vent_asistida { get; set; }

        [StringLength(1)]
        public string alta { get; set; }

        [StringLength(1)]
        public string fam_sintomas { get; set; }

        [StringLength(1)]
        public string derechohabiente { get; set; }

        public DateTime? inc_vence { get; set; }

        [Column(TypeName = "ntext")]
        public string comentarios { get; set; }

        public DateTime? fecha_enc { get; set; }

        public DateTime? fecha_pos { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? dias_prueba { get; set; }

        [StringLength(1)]
        public string gpo_riesgo { get; set; }

        [StringLength(1)]
        public string trabajo_ubica { get; set; }
    }
}
