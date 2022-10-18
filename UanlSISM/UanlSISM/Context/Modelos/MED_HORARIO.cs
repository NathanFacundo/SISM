namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MED_HORARIO
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string CLAVE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string DESCRIPCION { get; set; }

        [StringLength(10)]
        public string DESC_CORTA { get; set; }
    }
}
