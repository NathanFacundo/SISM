using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class CredencialDigital
    {
        public DHABIENTES DHabiente { get; set; }
        public string NUMEMP { get; set; }
        public string NUMAFIL { get; set; }
        public string NOMBRES { get; set; }
        public string APATERNO { get; set; }
        public string AMATERNO { get; set; }
        public DateTime? FNAC { get; set; }
        public string foto { get; set; }
        public string fondo { get; set; }

        public AFILIADOS afiliados { get; set; }
    }
}