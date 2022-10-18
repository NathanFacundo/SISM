namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Receta_Detalle_2
    {
        public int? Folio_Rcta { get; set; }

        [Key]
        [StringLength(6)]
        public string Codigo { get; set; }

        public int? Cantidad { get; set; }

        public int? CantSurtida { get; set; }

        [StringLength(100)]
        public string Dosis { get; set; }

        [StringLength(100)]
        public string Indicaciones { get; set; }

        [StringLength(1)]
        public string Estatus { get; set; }

        [StringLength(1)]
        public string Subrogar { get; set; }

        [StringLength(50)]
        public string dep_sub { get; set; }

        [StringLength(50)]
        public string no_surtida_razon { get; set; }
    }
}
