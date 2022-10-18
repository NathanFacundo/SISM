namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TBLSALES
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(2)]
        public string grupo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string clave { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }

        public virtual TBLGRUPOS TBLGRUPOS { get; set; }
    }
}
