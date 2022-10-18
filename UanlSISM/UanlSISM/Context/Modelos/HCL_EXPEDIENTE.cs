namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HCL_EXPEDIENTE
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string EXPEDIENTE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string CODIGO { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "smalldatetime")]
        public DateTime fec_registro { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fec_padece { get; set; }

        [Column(TypeName = "ntext")]
        public string NOTA { get; set; }

        [StringLength(5)]
        public string medico { get; set; }
    }
}
