using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class EnfermeriaAgendar
    {
        public DHABIENTES DHABIENTES { get; set; }

        public EnfermeriaSolicitud EnfermeriaSolicitud { get; set; }

        public List< EnfermeriaSolicitudDetalle> EnfermeriaSolicitudDetalle { get; set; }
    }
}