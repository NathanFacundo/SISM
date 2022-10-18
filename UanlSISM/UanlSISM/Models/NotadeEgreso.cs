using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class NotadeEgreso
    {
        public DHABIENTES DHabiente { get; set; }

        public List<NotaEgreso> NotasEgreso { get; set; }
        public NotaEgreso NotaEgreso { get; set; }
    }
}