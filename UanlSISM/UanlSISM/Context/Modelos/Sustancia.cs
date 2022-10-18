namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sustancia")]
    public partial class Sustancia
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Id_Grupo { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Id_Sal { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short Id_Nivel { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(2)]
        public string clave_presentacion { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string Presentacion { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(6)]
        public string Clave { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool Status { get; set; }

        [StringLength(1)]
        public string Consultorio { get; set; }

        public float? cto_promedio { get; set; }
    }
}
