using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class PacientesHosp
    {
        public DHABIENTES DHabiente { get; set; }
        public AltaUrgencias AltaUrg { get; set; }
        public NotaQuirurgica Notaquir { get; set; }
    }
}