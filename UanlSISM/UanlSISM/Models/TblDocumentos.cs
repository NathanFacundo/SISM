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
    
    public partial class TblDocumentos
    {
        public int IdDocumentacion { get; set; }
        public int Emp_NumEmp { get; set; }
        public string Carta { get; set; }
        public string Curriculum { get; set; }
        public string Acta { get; set; }
        public string INE { get; set; }
        public string DocRFC { get; set; }
        public string CURP { get; set; }
        public string Comprobante { get; set; }
        public string Formato { get; set; }
        public string Caratula { get; set; }
        public string Kardex { get; set; }
        public string Cedula { get; set; }
        public string Titulo { get; set; }
        public string CedulaEsp { get; set; }
        public string Constancia { get; set; }
        public string Opinion { get; set; }
        public string Propuesta { get; set; }
        public string Alta { get; set; }
        public string Examen { get; set; }
    
        public virtual TblEmpleado TblEmpleado { get; set; }
    }
}
