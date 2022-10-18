namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SOLICITUD_LAB
    {
        [Key]
        [Column(TypeName = "numeric")]
        public decimal FOLIO { get; set; }

        [StringLength(8)]
        public string EXPEDIENTE { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FECHA_SOL { get; set; }

        [StringLength(5)]
        public string MEDICO { get; set; }

        [StringLength(1)]
        public string INDICACIONES { get; set; }
    }
}
