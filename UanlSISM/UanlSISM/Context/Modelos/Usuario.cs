namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usuario")]
    public partial class Usuario
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UsuarioId { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(10)]
        public string Usu_User { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(10)]
        public string Usu_Pass { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string Usu_Nombre { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime Usu_FecAlt { get; set; }

        public byte? RolId { get; set; }

        [Key]
        [Column(Order = 5)]
        public bool Usu_Status { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool Baja { get; set; }
    }
}
