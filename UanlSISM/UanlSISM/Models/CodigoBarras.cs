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
    
    public partial class CodigoBarras
    {
        public int Id { get; set; }
        public int Id_Sustancia { get; set; }
        public string Nombre_Comercial { get; set; }
        public string Codigo_Barra { get; set; }
        public short Contenido { get; set; }
        public bool Status { get; set; }
        public System.DateTime Fecha { get; set; }
        public Nullable<int> contenido_anterior { get; set; }
        public string codigobarra_anterior { get; set; }
    }
}
