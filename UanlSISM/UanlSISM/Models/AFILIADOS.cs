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
    
    public partial class AFILIADOS
    {
        public string NUMEMP { get; set; }
        public string numemp2 { get; set; }
        public string NOMBRES { get; set; }
        public string APATERNO { get; set; }
        public string AMATERNO { get; set; }
        public string TIPOAFIL { get; set; }
        public string CVEDEP { get; set; }
        public Nullable<System.DateTime> FINGRESO { get; set; }
        public Nullable<System.DateTime> FVIGENCIA { get; set; }
        public string CURP { get; set; }
        public string PUESTO { get; set; }
        public string ACTVDES { get; set; }
        public string RFC { get; set; }
        public string DOMICILIO { get; set; }
        public string COLONIA { get; set; }
        public string TELEFONOS { get; set; }
        public string MOTBAJA { get; set; }
        public string SEXO { get; set; }
        public Nullable<System.DateTime> fnac { get; set; }
        public string DEP_OLD { get; set; }
        public string OCUPACION { get; set; }
        public Nullable<System.DateTime> FCREDENCIAL { get; set; }
        public Nullable<System.DateTime> FBAJA { get; set; }
        public string DESC_BAJA { get; set; }
        public string COLONIA_DESC { get; set; }
        public string CUIDAD { get; set; }
        public string NOMINA { get; set; }
        public string area { get; set; }
        public string emp_realizo { get; set; }
        public string RHUANL { get; set; }
        public string num_contrato { get; set; }
        public string status_rep { get; set; }
        public string TMP { get; set; }
        public string colonia_se { get; set; }
        public string num_tmp { get; set; }
        public string email { get; set; }
        public string plano_ubic { get; set; }
        public string coord_ubic { get; set; }
        public string tel_oficina { get; set; }
        public string tel_celular { get; set; }
        public string dersm { get; set; }
        public string pswd { get; set; }
        public string grupo_medico { get; set; }
        public string grupo_pediatra { get; set; }
        public string medicofam { get; set; }
        public string pediatra { get; set; }
    
        public virtual DEPENDENCIAS DEPENDENCIAS { get; set; }
    }
}
