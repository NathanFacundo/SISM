namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DetalleHorarioMedicos
    {
        public int Id { get; set; }

        [Required]
        [StringLength(5)]
        public string MedicoId { get; set; }

        [Required]
        [StringLength(2)]
        public string TiposConsultaId { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFinal { get; set; }

        [StringLength(50)]
        public string EventPid { get; set; }

        public virtual MEDICOS MEDICOS { get; set; }

        public virtual TIPOSCONSULTA TIPOSCONSULTA { get; set; }
    }
}
