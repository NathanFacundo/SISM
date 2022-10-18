using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class ProcAmbulatorios
    {
        public bool NotaIngreso { get; set; }
        public bool NotaPreop { get; set; }
        public bool NotaOp { get; set; }
        public bool NotaEgreso { get; set; }
    }
}