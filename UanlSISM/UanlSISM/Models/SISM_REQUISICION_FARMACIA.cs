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
    
    public partial class SISM_REQUISICION_FARMACIA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SISM_REQUISICION_FARMACIA()
        {
            this.SISM_DETALLE_REQ_FARMACIA = new HashSet<SISM_DETALLE_REQ_FARMACIA>();
        }
    
        public int Id_Requisicion_Farm { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public string UsuarioRegistra { get; set; }
        public string Estatus { get; set; }
        public string ip_realiza { get; set; }
        public string EstatusContrato { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SISM_DETALLE_REQ_FARMACIA> SISM_DETALLE_REQ_FARMACIA { get; set; }
    }
}
