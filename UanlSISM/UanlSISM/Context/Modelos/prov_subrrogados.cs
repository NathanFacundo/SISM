namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class prov_subrrogados
    {
        [Key]
        [StringLength(3)]
        public string clave { get; set; }

        [StringLength(80)]
        public string nombre { get; set; }

        [StringLength(30)]
        public string direccion { get; set; }

        [StringLength(30)]
        public string colonia { get; set; }

        [StringLength(30)]
        public string ciudad { get; set; }

        [StringLength(50)]
        public string horario { get; set; }

        [Column(TypeName = "text")]
        public string observaciones { get; set; }

        [StringLength(12)]
        public string telefono1 { get; set; }

        [StringLength(12)]
        public string telefono2 { get; set; }

        [StringLength(1)]
        public string estado { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_pre_prov { get; set; }
    }
}
