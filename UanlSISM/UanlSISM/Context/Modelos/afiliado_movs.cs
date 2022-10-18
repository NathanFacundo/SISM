namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class afiliado_movs
    {
        [Key]
        [Column(Order = 0, TypeName = "smalldatetime")]
        public DateTime fecha_realizo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string emp_realizo { get; set; }

        [StringLength(20)]
        public string ip_realizo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(250)]
        public string descripcion { get; set; }

        [StringLength(6)]
        public string numemp { get; set; }

        [StringLength(6)]
        public string num_anterior { get; set; }
    }
}
