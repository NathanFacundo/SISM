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
    
    public partial class InvFarm
    {
        public int Id { get; set; }
        public short InvFarmId { get; set; }
        public Nullable<System.DateTime> Inv_Fecha { get; set; }
        public Nullable<int> Id_Sustancia { get; set; }
        public Nullable<int> Inv_Ini { get; set; }
        public int Inv_Ent { get; set; }
        public int Inv_Sal { get; set; }
        public int Inv_Act { get; set; }
        public Nullable<int> Inv_Reorden { get; set; }
        public Nullable<int> ManejoDisponible { get; set; }
        public string Usuario_Registra { get; set; }
    }
}
