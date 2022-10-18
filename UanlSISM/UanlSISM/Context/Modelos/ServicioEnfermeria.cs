namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ServicioEnfermeria")]
    public partial class ServicioEnfermeria
    {
        [Key]
        public int IdServicio { get; set; }

        [StringLength(20)]
        public string Clave { get; set; }

        public string Nombre { get; set; }

        public string Instruccciones { get; set; }
    }
}
