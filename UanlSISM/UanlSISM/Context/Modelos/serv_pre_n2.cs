namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class serv_pre_n2
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string tipo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(6)]
        public string codigo { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }
    }
}
