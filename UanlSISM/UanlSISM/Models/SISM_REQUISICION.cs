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
    
    public partial class SISM_REQUISICION
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SISM_REQUISICION()
        {
            this.SISM_DET_REQUISICION = new HashSet<SISM_DET_REQUISICION>();
        }
    
        public int Id_Requicision { get; set; }
        public string Clave { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string Id_User { get; set; }
        public string IP_User { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<int> Estatus { get; set; }
        public string claveOLD { get; set; }
        public string EstatusContrato { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SISM_DET_REQUISICION> SISM_DET_REQUISICION { get; set; }
    }
}
