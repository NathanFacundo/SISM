namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dental_N1
    {
        [Key]
        [StringLength(2)]
        public string codigo { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        [StringLength(20)]
        public string especialidad { get; set; }
    }
}
