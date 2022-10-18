namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AFILIACION")]
    public partial class AFILIACION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AFILIACION()
        {
            AFILIADOS = new HashSet<AFILIADOS>();
        }

        [Key]
        [StringLength(2)]
        public string TIPOAFIL { get; set; }

        [StringLength(20)]
        public string DESCR { get; set; }

        [Column(TypeName = "ntext")]
        public string POLITICA { get; set; }

        [Column(TypeName = "ntext")]
        public string PROCEDIMIENTO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? tot_recetas { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_pre_contrato { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AFILIADOS> AFILIADOS { get; set; }
    }
}
