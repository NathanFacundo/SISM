using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class NotasMedicoViewModel
    {
        public string NUMERO { get; set; }
        public string TITULO { get; set; }
        public string NOMBRESMEDICO { get; set; }
        public DateTime FECHACONSULTA { get; set; }
        public List<Med_Expediente> Lista { get; set; }
    }
}