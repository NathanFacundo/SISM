//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UanlSISM.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class sis_areas
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public sis_areas()
        {
            this.SOL_SERVICIO = new HashSet<SOL_SERVICIO>();
        }
    
        public string clave { get; set; }
        public string descripcion { get; set; }
        public string COORDINADOR { get; set; }
        public string EMAIL { get; set; }
        public string estatus { get; set; }
        public string subdireccion { get; set; }
        public string coordmedico { get; set; }
        public string emailcoordmed { get; set; }
        public string tipo_area { get; set; }
        public decimal id_area { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SOL_SERVICIO> SOL_SERVICIO { get; set; }
    }
}
