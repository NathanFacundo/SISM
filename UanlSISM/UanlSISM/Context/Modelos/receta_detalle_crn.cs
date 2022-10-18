namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class receta_detalle_crn
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int folio_rc { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(6)]
        public string codigo { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int cantidad { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string dosis { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(100)]
        public string indicaciones { get; set; }

        [StringLength(1)]
        public string farext { get; set; }

        public float? costo { get; set; }
    }
}
