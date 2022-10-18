namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class receta_exp_crn
    {
        [Key]
        [Column(Order = 0)]
        public int folio_rc { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string medico_crea { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(5)]
        public string medico_ref { get; set; }

        [Key]
        [Column(Order = 3, TypeName = "date")]
        public DateTime fecha_crea { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int estado { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int platica_edu { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string lab_relev { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(50)]
        public string trat_actual { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "smalldatetime")]
        public DateTime fec_ini_sintomas { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string evento_adverso { get; set; }

        [Key]
        [Column(Order = 10)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int meses_surtir { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(8)]
        public string expediente { get; set; }

        [Key]
        [Column(Order = 12)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int policl { get; set; }

        [StringLength(1)]
        public string px_special { get; set; }

        public int? terminada { get; set; }

        [Column(TypeName = "text")]
        public string proxima_cita { get; set; }

        public float? costo { get; set; }

        public DateTime? fecha_kiosco { get; set; }

        public int? turnofar { get; set; }

        public DateTime? fecha_prox_cita { get; set; }
    }
}
