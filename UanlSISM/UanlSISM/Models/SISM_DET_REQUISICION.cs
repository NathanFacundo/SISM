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
    
    public partial class SISM_DET_REQUISICION
    {
        public int Id_Detalle_Req { get; set; }
        public int Id_Requicision { get; set; }
        public int Id_Sustancia { get; set; }
        public Nullable<int> Cantidad { get; set; }
        public string Clave { get; set; }
        public string Descripcion { get; set; }
        public string Compendio { get; set; }
    
        public virtual SISM_REQUISICION SISM_REQUISICION { get; set; }
    }
}
