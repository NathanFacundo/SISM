using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public partial class SISM_DET_REQUISICION
    {
        public short CANTIDAD_NUEVA { get; set; }
        public float PREUNIT_NUEVA { get; set; }
        public float TOTAL_NUEVA { get; set; }
        public bool CB_ELIMINAR { get; set; }
    }
}
