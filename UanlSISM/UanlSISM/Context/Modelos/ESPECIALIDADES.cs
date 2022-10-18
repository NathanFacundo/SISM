namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ESPECIALIDADES
    {
        [StringLength(2)]
        public string CLAVE { get; set; }

        [StringLength(40)]
        public string DESCRIPCION { get; set; }

        [StringLength(76)]
        public string OBS_MED { get; set; }

        [StringLength(2)]
        public string GRUPO_DIR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FMSR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FMSB { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FVSR { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? FVSB { get; set; }

        [StringLength(2)]
        public string TIPO { get; set; }

        [Key]
        public int id_pre_esp { get; set; }

        [StringLength(50)]
        public string titulo { get; set; }

        [StringLength(1)]
        public string estado { get; set; }
    }
}
