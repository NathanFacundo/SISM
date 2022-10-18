namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NotaEgreso")]
    public partial class NotaEgreso
    {
        [Key]
        public int EgresoId { get; set; }

        public int OrdenFolio { get; set; }

        public int OperatoriaId { get; set; }

        [Required]
        [StringLength(5)]
        public string Medico { get; set; }

        [Required]
        [StringLength(8)]
        public string NumEmp { get; set; }

        [Required]
        [StringLength(20)]
        public string DiagnosticoFinal { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string DiagnosticoDesc { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ResumenClinico { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ProcedimientoRealizado { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ProcProblemasPendientes { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Medicacion { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string RecomendacionesIncapacidad { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Pronostico { get; set; }

        [Required]
        [StringLength(5)]
        public string UsuarioCreacion { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime FechaEgreso { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
