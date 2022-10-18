namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class citas_sust
    {
        [StringLength(5)]
        public string medico_titular { get; set; }

        [StringLength(1)]
        public string grupo { get; set; }

        [StringLength(5)]
        public string medico_sust { get; set; }

        [Column(TypeName = "date")]
        public DateTime? fecha { get; set; }

        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal id { get; set; }

        public DateTime? fecha_realizo { get; set; }

        [StringLength(15)]
        public string emp_realizo { get; set; }

        [StringLength(1)]
        public string agenda_remplazo { get; set; }
    }
}
