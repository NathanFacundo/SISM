using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class ReporteConsumo
    {
        //public string Clave { get; set; }
        //public int SustanciaId { get; set; }
        //public string Descripcion { get; set; }
        //public string Presentacion { get; set; }
        //public Nullable<int> InvDisp { get; set; }
        //public int Nivel { get; set; }
        //public string PromDiaFarm { get; set; }
        //public string PromDiaAlm { get; set; }
        //public string PromMensual { get; set; }
        //public string DiasConsumo { get; set; }
        //public Nullable<int> enero { get; set; }
        //public Nullable<int> febrero { get; set; }
        //public Nullable<int> marzo { get; set; }
        //public Nullable<int> abril { get; set; }
        //public Nullable<int> mayo { get; set; }
        //public Nullable<int> junio { get; set; }
        //public Nullable<int> julio { get; set; }
        //public Nullable<int> agosto { get; set; }
        //public Nullable<int> septiembre { get; set; }
        //public Nullable<int> octubre { get; set; }
        //public Nullable<int> noviembre { get; set; }
        //public Nullable<int> diciembre { get; set; }
        //public List<Tuple<string, Nullable<int>>> Ult6Meses { get; set; }
    }
    public class ReporteAbastecimeinto
    {
        public string Clave { get; set; }
        public int SustanciaId { get; set; }
        public string Descripcion_21 { get; set; }
        public int InvDisp { get; set; }
        public decimal Consumo_Mes { get; set; }
        public decimal Consumo_Dia { get; set; }
        public Nullable<int> DiasCobertura { get; set; }
        public string Semaforo { get; set; }
        //public Nullable<int> enero { get; set; }
        //public Nullable<int> febrero { get; set; }
        //public Nullable<int> marzo { get; set; }
        public Nullable<int> abril { get; set; }
        public Nullable<int> mayo { get; set; }
        public Nullable<int> junio { get; set; }
        //public Nullable<int> julio { get; set; }
        //public Nullable<int> agosto { get; set; }
        //public Nullable<int> septiembre { get; set; }
        //public Nullable<int> octubre { get; set; }
        //public Nullable<int> noviembre { get; set; }
        //public Nullable<int> diciembre { get; set; }
        public List<Requisicion> Requisiciones { get; set; }
        public List<OrdenCompra> OrdenesCompras { get; set; }
        public List<ValeEntrada> ValeEntrada { get; set; }
        public List<CodigoBarras> Productos { get; set; }
        public List<Tuple<string, Nullable<int>>> Ult6Meses { get; set; }
    }
    public class ReporteAbastecimeintoAlm
    {
        public string Clave { get; set; }
        public int SustanciaId { get; set; }
        public string Descripcion_21 { get; set; }
        public int InvDisp { get; set; }
        public decimal Consumo_Mes { get; set; }
        public decimal Consumo_Dia { get; set; }
        public Nullable<int> DiasCobertura { get; set; }
        public string Semaforo { get; set; }
        //public Nullable<int> enero { get; set; }
        //public Nullable<int> febrero { get; set; }
        //public Nullable<int> marzo { get; set; }
        public Nullable<int> abril { get; set; }
        public Nullable<int> mayo { get; set; }
        public Nullable<int> junio { get; set; }
        //public Nullable<int> julio { get; set; }
        //public Nullable<int> agosto { get; set; }
        //public Nullable<int> septiembre { get; set; }
        //public Nullable<int> octubre { get; set; }
        //public Nullable<int> noviembre { get; set; }
        //public Nullable<int> diciembre { get; set; }

        public List<Tuple<string, Nullable<int>>> Ult6Meses { get; set; }
    }
    public class Meses
    {
        //public string Item1 { get; set; }
        //public Nullable<int> Item2 { get; set; }
    }
    public class ReporteAdministrador
    {

    }
}