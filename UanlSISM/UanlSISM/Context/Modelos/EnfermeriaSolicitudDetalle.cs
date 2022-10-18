namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("EnfermeriaSolicitudDetalle")]
    public partial class EnfermeriaSolicitudDetalle
    {
        [Key]
        public int IdDetalle { get; set; }

        public int SolicitudId { get; set; }

        public int IdServicio { get; set; }

        public DateTime FechaSol { get; set; }

        public DateTime FechaRealiza { get; set; }

        [Required]
        [StringLength(25)]
        public string UsuarioRealiza { get; set; }

        [Required]
        public string Descripcion { get; set; }
    }
}
