namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class REP_EPI
    {
        [StringLength(3)]
        public string POS_REP { get; set; }

        [Key]
        [StringLength(3)]
        public string CVE_EPI { get; set; }

        [StringLength(50)]
        public string DESC_DX { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R01 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R14 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R59 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R1014 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R1519 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R2024 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R2544 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R4549 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R5059 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R6064 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? R65 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IGN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? TOTAL { get; set; }
    }
}
