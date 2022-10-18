using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{  
    public class ServicioPrestaciones
    {
        public DHABIENTES DHabiente { get; set; }
        public string NUMEMP { get; set; }
        public string NUMAFIL { get; set; }
        public List<PrestacionesRayosX> Prestaciones { get; set; }
        public List<PrestacionesRayosX> HistorialPrestaciones { get; set; }

    }
}

public class PrestacionesRayosX
{
	public string codigo { get; set; }
	public string cantidad  { get; set; }
    public string numemp  { get; set; }
    public string medico  { get; set; }
    public DateTime fecha_sol { get; set; }
    public DateTime fecha_realiza  { get; set; }
    public string sexo { get; set; }
    public string edad  { get; set; }
    public string turno { get; set; }
    public string dx_sol { get; set; }
    public string proveedor { get; set; }
    public string prim_sub { get; set; }
    public string nota { get; set; }
    public string interp { get; set; }
    public string rea_interp { get; set; }
    public DateTime fec_interp { get; set; }
    public string realiza { get; set; }

    //serv_rx_n2

    public string rxcodigo { get; set; }
    public string tipo { get; set; }
    public string descripcion { get; set; }

}