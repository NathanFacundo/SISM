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
    
    public partial class TblAcceso
    {
        public int IdAcceso { get; set; }
        public string Usuario { get; set; }
        public string Contraeña { get; set; }
        public string Token { get; set; }
        public int Emp_NumEmp { get; set; }
        public Nullable<int> IdDepartamento { get; set; }
        public Nullable<int> TipoUsuario { get; set; }
    
        public virtual TblEmpleado TblEmpleado { get; set; }
        public virtual TblDepartamento TblDepartamento { get; set; }
    }
}
