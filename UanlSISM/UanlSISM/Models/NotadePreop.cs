using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class NotadePreop
    {
        public DHABIENTES DHabiente { get; set; }

        public List<NotaPreoperatoria> NotasPreoperatoria { get; set; }
        public NotaPreoperatoria NotaPreoperatoria { get; set; }
    }
}