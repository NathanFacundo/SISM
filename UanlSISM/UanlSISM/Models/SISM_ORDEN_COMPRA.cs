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
    
    public partial class SISM_ORDEN_COMPRA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SISM_ORDEN_COMPRA()
        {
            this.SISM_DETALLE_OC = new HashSet<SISM_DETALLE_OC>();
        }
    
        public int Id { get; set; }
        public string Clave { get; set; }
        public Nullable<int> Id_Requisicion { get; set; }
        public Nullable<int> Id_Proveedor { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<System.DateTime> FechaMod { get; set; }
        public string Forma_Pago { get; set; }
        public string Folio { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> UsuarioId { get; set; }
        public Nullable<bool> Cerrado { get; set; }
        public Nullable<int> Cuadro { get; set; }
        public string UsuarioNuevo { get; set; }
        public string IP_User { get; set; }
        public string NombreProveedor { get; set; }
        public Nullable<double> Total_OC { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SISM_DETALLE_OC> SISM_DETALLE_OC { get; set; }
    }
}
