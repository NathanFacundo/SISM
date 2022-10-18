namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ActividadesDepartamento")]
    public partial class ActividadesDepartamento
    {
        [Key]
        public int ActividadId { get; set; }

        [Required]
        [StringLength(200)]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(200)]
        public string Departamento { get; set; }

        [StringLength(100)]
        public string Ubicacion { get; set; }
    }
}
