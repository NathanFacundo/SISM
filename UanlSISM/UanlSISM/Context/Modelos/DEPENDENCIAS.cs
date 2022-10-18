namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DEPENDENCIAS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DEPENDENCIAS()
        {
            AFILIADOS = new HashSet<AFILIADOS>();
            CITAS = new HashSet<CITAS>();
        }

        [Key]
        [StringLength(4)]
        public string CVEDEP { get; set; }

        [StringLength(100)]
        public string DESCR { get; set; }

        [StringLength(50)]
        public string DIRECCION { get; set; }

        [StringLength(40)]
        public string TELEFONOS { get; set; }

        [StringLength(40)]
        public string EMAIL { get; set; }

        [StringLength(40)]
        public string SITIOWEB { get; set; }

        [StringLength(3)]
        public string DEP_OLD { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AFILIADOS> AFILIADOS { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CITAS> CITAS { get; set; }
    }
}
