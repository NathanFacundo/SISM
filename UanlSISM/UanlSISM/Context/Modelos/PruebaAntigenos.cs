namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PruebaAntigenos
    {
        public int id { get; set; }

        public int? resultado { get; set; }

        [Column(TypeName = "text")]
        public string nota { get; set; }

        public DateTime? fecha { get; set; }

        [StringLength(50)]
        public string usuario { get; set; }

        [StringLength(50)]
        public string numexp { get; set; }
    }
}
