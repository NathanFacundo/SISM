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
    
    public partial class Tbl_DetalleOC
    {
        public int Id { get; set; }
        public int Id_OrdenCompra { get; set; }
        public Nullable<int> Id_CodigoBarrar { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public Nullable<double> PreUnit { get; set; }
        public Nullable<int> Obsequio { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<int> Id_Sustencia { get; set; }
        public Nullable<int> Pendiente { get; set; }
        public Nullable<double> Total { get; set; }
        public string Descripcion { get; set; }
        public string ClaveMedicamento { get; set; }
    
        public virtual Tbl_OrdenCompra Tbl_OrdenCompra { get; set; }
    }
}
