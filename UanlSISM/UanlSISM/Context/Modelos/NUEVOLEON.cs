namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NUEVOLEON")]
    public partial class NUEVOLEON
    {
        [Key]
        [StringLength(5)]
        public string clave_col { get; set; }

        public double? d_codigo { get; set; }

        [StringLength(255)]
        public string d_asenta { get; set; }

        [StringLength(255)]
        public string d_tipo_asenta { get; set; }

        [StringLength(255)]
        public string d_mnpio { get; set; }

        [StringLength(255)]
        public string d_estado { get; set; }

        [StringLength(255)]
        public string d_ciudad { get; set; }

        public double? d_CP { get; set; }

        public double? c_estado { get; set; }

        public double? c_oficina { get; set; }

        public double? c_CP { get; set; }

        public double? c_tipo_asenta { get; set; }

        public double? c_mnpio { get; set; }

        [StringLength(2)]
        public string clave_unidad { get; set; }
    }
}
