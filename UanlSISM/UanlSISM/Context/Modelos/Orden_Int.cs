namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orden_Int
    {
        [Key]
        public int id_orden { get; set; }

        [StringLength(50)]
        public string numemp { get; set; }

        [StringLength(50)]
        public string medico { get; set; }

        public DateTime? fecha_internamiento { get; set; }

        [StringLength(50)]
        public string proveedor { get; set; }

        [Column(TypeName = "text")]
        public string estudios { get; set; }

        [Column(TypeName = "text")]
        public string motivo { get; set; }

        [Column(TypeName = "text")]
        public string indicaciones { get; set; }

        [StringLength(50)]
        public string diagnostico1 { get; set; }

        [StringLength(50)]
        public string diagnostico2 { get; set; }

        [StringLength(50)]
        public string diagnostico3 { get; set; }

        public DateTime? fecha_registro { get; set; }

        [StringLength(50)]
        public string folio { get; set; }

        public DateTime? fecha_alta { get; set; }

        public int? estatus { get; set; }

        [Column(TypeName = "text")]
        public string comentarios { get; set; }

        public int? tipo { get; set; }

        [Column(TypeName = "text")]
        public string resumen_medico { get; set; }

        public int? enviar_a { get; set; }
    }
}
