namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("InventarioFarmacia")]
    public partial class InventarioFarmacia
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Inv_Sal { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Inv_Act { get; set; }

        public int? ManejoDisponible { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(6)]
        public string Clave { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string Descripcion_Grupo { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string Descripcion_Sal { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string Presentacion { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string Clave_Nivel { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(50)]
        public string Descripcion_Nivel { get; set; }
    }
}
