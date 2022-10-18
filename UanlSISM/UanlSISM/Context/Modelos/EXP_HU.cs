namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EXP_HU
    {
        [StringLength(255)]
        public string PAC_REG { get; set; }

        [StringLength(255)]
        public string PAC_NOM { get; set; }

        public DateTime? PAC_FEC { get; set; }

        public DateTime? PAC_FDN { get; set; }

        [StringLength(255)]
        public string PAC_SEX { get; set; }

        [StringLength(255)]
        public string PAC_CIV { get; set; }

        [StringLength(255)]
        public string PAC_OCU { get; set; }

        [StringLength(255)]
        public string PAC_DOM { get; set; }

        [StringLength(255)]
        public string PAC_COL { get; set; }

        [StringLength(255)]
        public string PAC_CIU { get; set; }

        [StringLength(255)]
        public string PAC_EDO { get; set; }

        [StringLength(255)]
        public string PAC_TEL { get; set; }

        [StringLength(255)]
        public string PAC_CDP { get; set; }

        [StringLength(255)]
        public string PAC_EMP { get; set; }

        [StringLength(255)]
        public string PAC_EXP { get; set; }

        [StringLength(255)]
        public string PAC_NOT { get; set; }

        [StringLength(255)]
        public string PAC_RES { get; set; }

        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal PAC_ID { get; set; }

        [StringLength(8)]
        public string EXPEDIENTE { get; set; }

        [StringLength(6)]
        public string NUMAFIL { get; set; }

        public int? eliminar { get; set; }
    }
}
