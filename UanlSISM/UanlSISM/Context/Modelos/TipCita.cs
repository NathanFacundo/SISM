namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TipCita")]
    public partial class TipCita
    {
        [StringLength(2)]
        public string especialidad { get; set; }

        [Key]
        [StringLength(2)]
        public string clave { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        [Column(TypeName = "ntext")]
        public string desc_larga { get; set; }
    }
}
