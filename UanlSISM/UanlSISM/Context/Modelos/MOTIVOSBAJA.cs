namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MOTIVOSBAJA")]
    public partial class MOTIVOSBAJA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MOTIVOSBAJA()
        {
            AFILIADOS = new HashSet<AFILIADOS>();
        }

        [Key]
        [StringLength(2)]
        public string MOTBAJA { get; set; }

        [StringLength(40)]
        public string DESCR { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AFILIADOS> AFILIADOS { get; set; }
    }
}
