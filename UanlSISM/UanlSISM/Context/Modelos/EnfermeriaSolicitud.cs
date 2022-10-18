namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EnfermeriaSolicitud")]
    public partial class EnfermeriaSolicitud
    {
        [Key]
        public int SolicitudId { get; set; }

        [Required]
        [StringLength(8)]
        public string NumEmp { get; set; }

        [Required]
        [StringLength(5)]
        public string Medico { get; set; }

        [Required]
        public string Nota { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FechaSol { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FechaAgenda { get; set; }

        [Required]
        [StringLength(10)]
        public string UsuarioAgenda { get; set; }

        [Required]
        [StringLength(10)]
        public string UsuarioRealiza { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime FechaRealiza { get; set; }

        [Required]
        public string NotaRealiza { get; set; }

        public bool Finalizado { get; set; }

        public bool Cancelado { get; set; }

        public bool NoPresento { get; set; }

        [Required]
        [StringLength(1)]
        public string Urgente { get; set; }
    }
}
