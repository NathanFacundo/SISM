namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Query")]
    public partial class Query
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(200)]
        public string Descripcion { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string Departamento { get; set; }

        [StringLength(100)]
        public string Ubicacion { get; set; }
    }
}
