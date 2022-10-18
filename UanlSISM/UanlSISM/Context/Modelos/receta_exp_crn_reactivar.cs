namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class receta_exp_crn_reactivar
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int folio_rc { get; set; }

        [StringLength(5)]
        public string medico_crea { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_crea { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fec_ini_sintomas { get; set; }

        public int? meses_surtir { get; set; }

        [StringLength(8)]
        public string expediente { get; set; }

        public int? terminada { get; set; }
    }
}
