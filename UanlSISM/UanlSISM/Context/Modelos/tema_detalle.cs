namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tema_detalle
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_tema { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_detalle { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        [StringLength(50)]
        public string imagen { get; set; }

        public string desc_detalle { get; set; }

        public int? estado { get; set; }
    }
}
