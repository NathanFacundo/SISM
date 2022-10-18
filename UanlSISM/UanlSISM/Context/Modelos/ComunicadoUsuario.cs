namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ComunicadoUsuario")]
    public partial class ComunicadoUsuario
    {
        public int id { get; set; }

        [StringLength(50)]
        public string usuario { get; set; }

        public int? id_comunicado { get; set; }

        public DateTime? fecha { get; set; }

        public int? estado { get; set; }
    }
}
