namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Lab_detalle
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Folio_lab { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal id_indicaciones { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(4)]
        public string id_servicio { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(5)]
        public string id_lab { get; set; }
    }
}
