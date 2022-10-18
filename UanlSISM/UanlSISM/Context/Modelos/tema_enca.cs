namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tema_enca
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_tema { get; set; }

        [Required]
        [StringLength(80)]
        public string descripcion { get; set; }

        public int activo { get; set; }

        public int estado { get; set; }
    }
}
