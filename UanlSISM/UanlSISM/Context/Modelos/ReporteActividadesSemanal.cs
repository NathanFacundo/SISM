namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ReporteActividadesSemanal")]
    public partial class ReporteActividadesSemanal
    {
        [Key]
        public int ReporteSemanalActividadId { get; set; }

        public int ActividadId { get; set; }

        public int Cantidad { get; set; }

        public int Semana { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        [StringLength(200)]
        public string UsuarioRealiza { get; set; }

        public DateTime FechaRealiza { get; set; }
    }
}
