using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.ViewModels
{
    public class CitasViewModel
    {
        public int Id { get; set; }
        public string Medico { get; set; }
        public string Nombres { get; set; }
        public string Apaterno { get; set; }
        public string Amaterno { get; set; }
        public string tipo { get; set; }
        public string Fecha { get; set; }
        public DateTime fechanacimiento { get; set; }
        public string turno { get; set; }

        public string Numero_Medico { get; set; }
        public string NumEmp { get; set; }
        public string hr_consultorio { get; set; }
        public string estatus { get; set; }
        public string telefono { get; set; }
        public DateTime start { get; set; }
        public string Sexo { get; set; }

        //public string hora_turno() {
        //    var t = turno;

        //    if(turno != null)
        //    {
        //        //var hora = t.Substring(0, 2);
        //        var minutos = t.Substring(2, 3);
        //        return $"{minutos}";
        //    }

        //    return " ";

        //}

    }
}