using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class Citas_aux
    {
        public string fecha { get; set; }


        public List<Citas> Lista { get; set; } = new List<Citas>();
    }
    public class Citas
    {
        public string title { get; set; }
        public string color { get; set; }
        public string url { get; set; }
        public string tipo { get; set; }
        public string hora_recepcion { get; set; }
        public string cita_confirm { get; set; }

        public string hr_consultorio { get; set; }
        public string hr_llamado { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string NumEmp { get; set; }
        public string Medico { get; set; }
        public string Medico2 { get; set; }
        public string Fecha { get; set; }
        public string turno { get; set; }
        public string turno2 { get; set; }
        public string Nombres { get; set; }
        public string Apaterno { get; set; }
        public string Amaterno { get; set; }
        public string Numero_Medico { get; set; }
        public string sexo { get; set; }
        public DateTime fnac { get; set; }
        public int modificar { get; set; }
        public Int16 regxdia { get; set; }
        public DateTime frealizo { get; set; }
        public String frealizoStr { get; set; }
        public string emprealizo { get; set; }
        public string canalmed { get; set; }
        public string telefono { get; set; }
        public string cambiorealiza { get; set; }
        public DateTime? fecha_kiosco { get; set; }
        public DateTime? fecha_tarde { get; set; }
        public bool NoContesto { get; set; }
        public bool SeCortoComunicacion { get; set; }
        public bool Otros { get; set; }
        public string pruebacampoeliminar { get; set; }

        public string no_contesto { get; set; }
        public string estatus { get; set; }
        public int id { get; set; }
        public int count { get; set; }
        public string especialidad { get; set; }
        public string ip_kiosco { get; set; }

    }

}