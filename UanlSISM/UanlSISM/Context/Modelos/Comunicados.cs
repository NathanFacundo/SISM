namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comunicados
    {
        public int id { get; set; }

        [Column(TypeName = "text")]
        public string aviso { get; set; }

        public DateTime? fecha { get; set; }

        public int? estado { get; set; }
    }
}
