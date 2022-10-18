namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ARTICULOS
    {
        [Key]
        [StringLength(5)]
        public string codigo { get; set; }

        [StringLength(2)]
        public string cve_grupo { get; set; }

        [StringLength(2)]
        public string cve_sal { get; set; }

        [StringLength(2)]
        public string cve_pre { get; set; }

        [StringLength(50)]
        public string miligramos { get; set; }

        [StringLength(50)]
        public string mililitros { get; set; }

        [StringLength(50)]
        public string inicio { get; set; }

        [StringLength(50)]
        public string entradas { get; set; }

        [StringLength(50)]
        public string salidas { get; set; }

        [StringLength(50)]
        public string actual { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? frealizo { get; set; }

        [StringLength(50)]
        public string emprealizo { get; set; }

        [StringLength(50)]
        public string horarealizo { get; set; }

        [StringLength(1)]
        public string cuadrobasico { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        [Column(TypeName = "money")]
        public decimal? costo { get; set; }

        public virtual TBLGRUPOS TBLGRUPOS { get; set; }
    }
}
