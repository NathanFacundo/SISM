namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FESTIVO")]
    public partial class FESTIVO
    {
        [Key]
        [Column(TypeName = "smalldatetime")]
        public DateTime FECHA { get; set; }

        [StringLength(40)]
        public string DESCRIPCION { get; set; }
    }
}
