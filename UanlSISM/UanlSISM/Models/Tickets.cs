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
    
    public partial class Tickets
    {
        public int id { get; set; }
        public string usuario_realiza { get; set; }
        public string descripcion { get; set; }
        public Nullable<int> area { get; set; }
        public string usuario_encargado { get; set; }
        public Nullable<System.DateTime> fecha_registro { get; set; }
        public Nullable<int> estado { get; set; }
        public Nullable<System.DateTime> fecha_cierre { get; set; }
        public Nullable<int> tipo { get; set; }
        public string ip_realiza { get; set; }
        public Nullable<System.DateTime> fecha_estimada { get; set; }
    }
}
