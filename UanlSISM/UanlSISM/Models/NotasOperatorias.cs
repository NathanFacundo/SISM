using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class NotasOperatorias
    {

        public DHABIENTES DHabiente { get; set; }
        public List<NotaOperatoria> NotasOperatoria { get; set; }
        public NotaOperatoria NotaOperatoria { get; set; }

    }
}