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
    
    public partial class OrdenCompra
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrdenCompra()
        {
            this.DetalleOC = new HashSet<DetalleOC>();
        }
    
        public int Id { get; set; }
        public string clave { get; set; }
        public Nullable<int> Id_Requisicion { get; set; }
        public int Id_Proveedor { get; set; }
        public Nullable<System.DateTime> Fecha { get; set; }
        public Nullable<System.DateTime> FecMod { get; set; }
        public string Forma_Pago { get; set; }
        public string Folio { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> UsuarioId { get; set; }
        public bool Cerrado { get; set; }
        public Nullable<int> Cuadro { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleOC> DetalleOC { get; set; }
    }
}
