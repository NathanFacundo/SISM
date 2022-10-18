namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PARENTESCO")]
    public partial class PARENTESCO
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PARENTESCO()
        {
            CITAS = new HashSet<CITAS>();
            DHABIENTES = new HashSet<DHABIENTES>();
        }

        [Key]
        [StringLength(1)]
        public string PARIENTE { get; set; }

        [StringLength(20)]
        public string DESCR { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_pre_par { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CITAS> CITAS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DHABIENTES> DHABIENTES { get; set; }
    }
}
