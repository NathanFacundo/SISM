namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NotaPreoperatoria")]
    public partial class NotaPreoperatoria
    {
        [Key]
        public int PreOperatoriaId { get; set; }

        public int IngresoId { get; set; }

        public int OrdenFolio { get; set; }

        [Required]
        [StringLength(5)]
        public string Medico { get; set; }

        [Required]
        [StringLength(8)]
        public string NumEmp { get; set; }

        [StringLength(20)]
        public string Diagnostico { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string DiagnosticoDesc { get; set; }

        [Column(TypeName = "text")]
        public string PlanesCuidados { get; set; }

        [Column(TypeName = "text")]
        public string PlanQuirurgico { get; set; }

        [Column(TypeName = "text")]
        public string FactoresRiesgo { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string RiesgoQuirurgico { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Pronostico { get; set; }

        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(5)]
        public string UsuarioCreacion { get; set; }
    }
}
