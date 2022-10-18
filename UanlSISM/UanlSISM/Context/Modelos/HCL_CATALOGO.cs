namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HCL_CATALOGO
    {
        [Key]
        [StringLength(5)]
        public string codigo { get; set; }

        [StringLength(5)]
        public string CODIGO2 { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        [Column(TypeName = "text")]
        public string desc_larga { get; set; }

        [StringLength(1)]
        public string sol_fecha { get; set; }

        [StringLength(1)]
        public string sol_nota { get; set; }

        [StringLength(1)]
        public string tipo { get; set; }

        [StringLength(1)]
        public string activo { get; set; }

        [StringLength(1)]
        public string cod_default { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? lineas { get; set; }
    }
}
