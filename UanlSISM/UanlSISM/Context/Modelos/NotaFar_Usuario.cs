namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class NotaFar_Usuario
    {
        public int id { get; set; }

        public int? id_nota { get; set; }

        [StringLength(50)]
        public string usuario { get; set; }

        public DateTime? fecha { get; set; }

        public int? estado { get; set; }
    }
}
