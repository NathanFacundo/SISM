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
    
    public partial class Lab_exp
    {
        public int Folio_lab { get; set; }
        public string medico_crea { get; set; }
        public System.DateTime fecha_crea { get; set; }
        public int estado { get; set; }
        public string observaciones { get; set; }
        public string expediente { get; set; }
        public string urgente { get; set; }
        public Nullable<decimal> folio_lab2 { get; set; }
        public Nullable<System.DateTime> fecha_muestra { get; set; }
        public Nullable<System.TimeSpan> hora_muestra { get; set; }
        public string emp_muestra { get; set; }
        public Nullable<decimal> folio_consec { get; set; }
        public string ip_muestra { get; set; }
        public Nullable<decimal> folio_semac { get; set; }
        public Nullable<decimal> folio_mederos { get; set; }
    }
}
