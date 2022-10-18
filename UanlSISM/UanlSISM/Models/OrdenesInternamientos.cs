using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class OrdenesInternamientos
    {
        public DHABIENTES DHabiente { get; set; }
        public Orden_Int OrInternamiento { get; set; }
        public NotaQuirurgica Notaquir { get; set; }

    }
}