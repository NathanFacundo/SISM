namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Lab_exp
    {
        [Key]
        [Column(Order = 0)]
        public int Folio_lab { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string medico_crea { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "date")]
        public DateTime fecha_crea { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int estado { get; set; }

        public string observaciones { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(8)]
        public string expediente { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(1)]
        public string urgente { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? folio_lab2 { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha_muestra { get; set; }

        public TimeSpan? hora_muestra { get; set; }

        [StringLength(20)]
        public string emp_muestra { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? folio_consec { get; set; }
    }
}
