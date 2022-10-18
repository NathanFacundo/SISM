namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FAR_FILA
    {
        public DateTime? FECHA { get; set; }

        public int? FILA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fecha2 { get; set; }

        public DateTime? hora { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? fecha3 { get; set; }

        public int id { get; set; }
    }
}
