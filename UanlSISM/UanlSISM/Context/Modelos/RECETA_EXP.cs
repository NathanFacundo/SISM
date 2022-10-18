namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RECETA_EXP
    {
        [Key]
        public int FOLIO_RCTA { get; set; }

        [Required]
        [StringLength(8)]
        public string EXPEDIENTE { get; set; }

        [Required]
        [StringLength(5)]
        public string MEDICO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FECHA { get; set; }

        [Required]
        [StringLength(4)]
        public string TURNO { get; set; }

        [Required]
        [StringLength(2)]
        public string REGXDIA { get; set; }

        [StringLength(1)]
        public string estado { get; set; }

        [StringLength(15)]
        public string dir_ip { get; set; }

        [StringLength(1)]
        public string imp_rcta { get; set; }

        [StringLength(1)]
        public string tipo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? meses { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? hora_rcta { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? hora_imp { get; set; }

        public int? folio_rc { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unidad_medica { get; set; }
    }
}
