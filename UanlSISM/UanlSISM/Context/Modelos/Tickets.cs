namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tickets
    {
        public int id { get; set; }

        [StringLength(50)]
        public string usuario_realiza { get; set; }

        [Column(TypeName = "text")]
        public string descripcion { get; set; }

        public int? area { get; set; }

        [StringLength(50)]
        public string usuario_encargado { get; set; }

        public DateTime? fecha_registro { get; set; }

        public int? estado { get; set; }

        public DateTime? fecha_cierre { get; set; }
    }
}
