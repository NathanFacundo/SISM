namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Interconsultas
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [StringLength(10)]
        public string especialidad { get; set; }

        public int? urgencia { get; set; }

        [Column(TypeName = "text")]
        public string observaciones { get; set; }

        public DateTime? fecha { get; set; }

        [StringLength(10)]
        public string num_exp { get; set; }

        [StringLength(10)]
        public string medico { get; set; }

        public int? num_inter { get; set; }
    }
}
