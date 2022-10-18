using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class _TotalConsumo
    {
        public string Clave { get; set; }
        public string Descripcion_21 { get; set; }
        public Nullable<decimal> SEMAC { get; set; }
        public Nullable<decimal> MEDEROS { get; set; }
        public Nullable<decimal> UER { get; set; }
        public Nullable<int> ALMACEN_SM { get; set; }
        public Nullable<int> GONZALITOS { get; set; }
    }
}