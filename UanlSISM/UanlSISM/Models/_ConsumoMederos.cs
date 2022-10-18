using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class _ConsumoMederos
    {
        public int SustanciaId { get; set; }
        public string Clave { get; set; }
        public string Descripcion_21 { get; set; }
        public int cactual { get; set; }
        public decimal ConsumoMes { get; set; }
        public Nullable<int> Dia_1 { get; set; }
        public Nullable<int> Dia_2 { get; set; }
        public Nullable<int> Dia_3 { get; set; }
        public Nullable<int> Dia_4 { get; set; }
        public Nullable<int> Dia_5 { get; set; }
        public Nullable<int> Dia_6 { get; set; }
        public Nullable<int> Dia_7 { get; set; }
        public List<Tuple<string, Nullable<int>>> Ult6Meses { get; set; }
    }
}