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
    
    public partial class Receta_Exp
    {
        public int Folio_Rcta { get; set; }
        public string Expediente { get; set; }
        public string Medico { get; set; }
        public System.DateTime Fecha { get; set; }
        public string Turno { get; set; }
        public string Regxdia { get; set; }
        public string Estado { get; set; }
        public string Dir_Ip { get; set; }
        public string imp_rcta { get; set; }
        public string Tipo { get; set; }
        public Nullable<decimal> Meses { get; set; }
        public Nullable<System.DateTime> Hora_Rcta { get; set; }
        public Nullable<System.DateTime> Hora_Imp { get; set; }
        public Nullable<int> UsuarioId { get; set; }
        public Nullable<System.DateTime> Fecha_Hora_Adjudicacion { get; set; }
        public Nullable<int> Usuario_Genero { get; set; }
        public Nullable<bool> Manual { get; set; }
        public Nullable<int> folio_rc { get; set; }
        public Nullable<System.DateTime> Fecha_Hora_Entrega { get; set; }
        public Nullable<int> TurnoFar { get; set; }
        public Nullable<int> UserFar { get; set; }
        public Nullable<System.DateTime> Fecha_Kiosco { get; set; }
        public Nullable<System.DateTime> hora_genera { get; set; }
        public Nullable<float> costo { get; set; }
        public Nullable<decimal> unidad_medica { get; set; }
        public string proxima_cita { get; set; }
        public Nullable<int> folio_rcta_hu { get; set; }
        public Nullable<System.DateTime> fecha_prox_cita { get; set; }
        public string medico_hu { get; set; }
    }
}
