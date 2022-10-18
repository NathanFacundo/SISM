namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NotaOperatoria")]
    public partial class NotaOperatoria
    {
        [Key]
        public int OperatoriaId { get; set; }

        public int OrdenFolio { get; set; }

        public int? PreOperatoriaId { get; set; }

        [Required]
        [StringLength(5)]
        public string Medico { get; set; }

        [Required]
        [StringLength(8)]
        public string NumEmp { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ResumenClinico { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ResEstudiosDiagnosticos { get; set; }

        [Required]
        [StringLength(20)]
        public string DiagnosticoIngreso { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string DiagnosticoIngresoDesc { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string PlanTerCirujiaPlaneada { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string RiesgoQuirurgico { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string CirujiaRealizada { get; set; }

        [Required]
        [StringLength(20)]
        public string DiagnosticoEgreso { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string DiagnosticoEgresoDesc { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string TipoAnestecia { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string DescTecnicaQuirurgicaTerapeutica { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Hallazgos { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Incidentes { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Sangrado { get; set; }

        public int? MotivoEgreso { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string EstadoActualProblemasClinicos { get; set; }

        [Column(TypeName = "text")]
        public string PlanManejo { get; set; }

        [Column(TypeName = "text")]
        public string Pronostico { get; set; }

        [Required]
        [StringLength(5)]
        public string UsuarioCreacion { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}
