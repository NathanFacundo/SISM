using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class Dental
    {
        public string clave { get; set; }
        public string medico { get; set; }
        public string pieza { get; set; }
        public string servicio { get; set; }
        public string sub_servicio { get; set; }
        public string nota { get; set; }
        public string diagnostico { get; set; }
        public string DesCorta { get; set; }
        public string nombre { get; set; }
        public string especialidad { get; set; }
        public DateTime fecha { get; set; }
        public int count { get; set; }
        public int telefonica { get; set; }
        public int presencial { get; set; }
    }
}