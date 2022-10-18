namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NotaIngreso")]
    public partial class NotaIngreso
    {
        [Key]
        public int IngresoId { get; set; }

        public int OrdenFolio { get; set; }

        [Required]
        [StringLength(5)]
        public string Medico { get; set; }

        [Required]
        [StringLength(8)]
        public string NumEmp { get; set; }

        public DateTime Fecha { get; set; }

        [Required]
        [StringLength(6)]
        public string T { get; set; }

        [Required]
        [StringLength(10)]
        public string A { get; set; }

        public int Pulso { get; set; }

        public int Respiracion { get; set; }

        public decimal Temp { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string MotivoIngreso { get; set; }

        [Column(TypeName = "text")]
        public string ResumenInterrogatorio { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string ResLabGabinete { get; set; }

        [Required]
        [StringLength(20)]
        public string DiagnosticoPresuntivo { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string DiagnosticoDesc { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string PlanManejo { get; set; }

        [Column(TypeName = "text")]
        public string Pronostico { get; set; }

        [Required]
        [StringLength(5)]
        public string UsuarioCreacion { get; set; }
    }
}
