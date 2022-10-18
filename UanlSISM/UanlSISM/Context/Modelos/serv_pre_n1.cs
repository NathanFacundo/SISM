namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class serv_pre_n1
    {
        [Key]
        [StringLength(3)]
        public string clave { get; set; }

        [StringLength(50)]
        public string descripcion { get; set; }
    }
}
