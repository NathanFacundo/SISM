namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RECETA_EXP_2
    {
        [Key]
        [Column(Order = 0)]
        public int Folio_Rcta { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(8)]
        public string Expediente { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(5)]
        public string Medico { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime Fecha { get; set; }

        [StringLength(4)]
        public string Turno { get; set; }

        [StringLength(2)]
        public string Regxdia { get; set; }

        [StringLength(1)]
        public string Estado { get; set; }

        [StringLength(15)]
        public string Dir_Ip { get; set; }

        [StringLength(1)]
        public string imp_rcta { get; set; }

        [StringLength(1)]
        public string Tipo { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Meses { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Hora_Rcta { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Hora_Imp { get; set; }

        public int? UsuarioId { get; set; }

        public DateTime? Fecha_Hora_Adjudicacion { get; set; }

        public int? Usuario_Genero { get; set; }

        public bool? Manual { get; set; }

        public int? folio_rc { get; set; }
    }
}
