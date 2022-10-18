namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DIAGNOSTICOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DIAGNOSTICOS()
        {
            CITAS = new HashSet<CITAS>();
        }

        [Key]
        [StringLength(7)]
        public string Clave { get; set; }

        [StringLength(70)]
        public string DesCorta { get; set; }

        [StringLength(254)]
        public string DescCompleta { get; set; }

        [StringLength(2)]
        public string Grupo { get; set; }

        [StringLength(50)]
        public string Especialidad { get; set; }

        [StringLength(1)]
        public string Estatus { get; set; }

        [StringLength(7)]
        public string clave2 { get; set; }

        [StringLength(1)]
        public string dental { get; set; }

        [StringLength(1)]
        public string estatus_exp { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CITAS> CITAS { get; set; }
    }
}
