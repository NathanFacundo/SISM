namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class sales
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Id_Grupo { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(2)]
        public string Clave_Sal { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Descripcion_Sal { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool Status_Sal { get; set; }
    }
}
