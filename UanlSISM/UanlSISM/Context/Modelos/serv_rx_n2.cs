namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class serv_rx_n2
    {
        [Required]
        [StringLength(3)]
        public string tipo { get; set; }

        [Key]
        [StringLength(6)]
        public string codigo { get; set; }

        [Required]
        [StringLength(50)]
        public string descripcion { get; set; }

        public virtual serv_rx_n1 serv_rx_n1 { get; set; }
    }
}
