namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CovidSolicitud")]
    public partial class CovidSolicitud
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

        public DateTime FechaSol { get; set; }

        public bool Vacunado { get; set; }

        public string MarcaVacuna { get; set; }

        public bool? EsquemaCompleto { get; set; }

        public DateTime? FechaUltimaDosis { get; set; }

        public bool? Fiebre { get; set; }

        public bool? Tos { get; set; }

        public bool? Anosmia { get; set; }

        public bool? Hipotension { get; set; }

        public bool? Odinofagia { get; set; }

        public bool? Mialgia { get; set; }

        public bool? FRM25 { get; set; }

        public bool? AlteracionConsciencia { get; set; }

        public bool? DolorCabeza { get; set; }

        public bool? DolorAbdominal { get; set; }

        public bool? Disnea { get; set; }

        public bool? Saturacion { get; set; }

        public bool? Alteraciones { get; set; }

        public bool? Diarrea { get; set; }

        public bool? Mareo { get; set; }

        [Required]
        public string Tipo { get; set; }

        public DateTime? FechaAgenda { get; set; }

        [StringLength(10)]
        public string UsuarioAgenda { get; set; }

        [StringLength(10)]
        public string UsuarioRealiza { get; set; }

        public DateTime? FechaRealiza { get; set; }

        public string NotaRealiza { get; set; }

        public bool Finalizado { get; set; }

        public bool Cancelado { get; set; }

        public bool NoPresento { get; set; }

        [StringLength(1)]
        public string Urgente { get; set; }

        public int? Resultado { get; set; }

        [StringLength(500)]
        public string NotaResultado { get; set; }

        public DateTime? FechaResultado { get; set; }

        [StringLength(50)]
        public string UsuarioResultado { get; set; }
    }
}
