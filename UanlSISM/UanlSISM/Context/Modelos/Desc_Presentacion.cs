namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Desc_Presentacion
    {
        public int ID { get; set; }

        public string Presentacion_Desc { get; set; }

        [StringLength(25)]
        public string Tipo { get; set; }
    }
}
