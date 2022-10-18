namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class EMP_JHG
    {
        [Key]
        [StringLength(5)]
        public string NUM_EMP { get; set; }

        [Required]
        [StringLength(50)]
        public string NOM_EMP { get; set; }

        [Required]
        [StringLength(50)]
        public string DIR_EMP { get; set; }

        [Required]
        [StringLength(10)]
        public string TEL_EMP { get; set; }

        [Required]
        [StringLength(10)]
        public string SDO_EMP { get; set; }
    }
}
