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
    
    public partial class ReporteInventarios
    {
        public int Id { get; set; }
        public short Id_Grupo { get; set; }
        public short Id_Sal { get; set; }
        public short Id_Nivel { get; set; }
        public string clave_presentacion { get; set; }
        public string SobranteInv2022 { get; set; }
        public string Presentacion { get; set; }
        public string Clave { get; set; }
        public bool Status { get; set; }
        public string descripcion_21 { get; set; }
        public Nullable<int> id_grupo_21 { get; set; }
        public Nullable<int> Inv_Act_Farmacia { get; set; }
        public Nullable<int> Inv_Act_Almacen { get; set; }
        public Nullable<decimal> Inv_Act_CU { get; set; }
        public Nullable<decimal> Inv_Act_Mederos { get; set; }
        public Nullable<decimal> InventarioTotal { get; set; }
    }
}
