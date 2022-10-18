namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Alergias_Exp
    {
        public int id { get; set; }

        [StringLength(50)]
        public string medicamento { get; set; }

        [StringLength(50)]
        public string num_exp { get; set; }

        [StringLength(50)]
        public string medico { get; set; }

        public DateTime? fecha { get; set; }

        public int? estado { get; set; }
    }
}
