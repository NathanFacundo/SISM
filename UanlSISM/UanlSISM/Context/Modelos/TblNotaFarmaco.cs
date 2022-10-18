namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TblNotaFarmaco")]
    public partial class TblNotaFarmaco
    {
        [Key]
        public int IdNota { get; set; }

        public string NotaFarmaco { get; set; }

        public DateTime? FechaNotaFarmaco { get; set; }

        public string MedicoRealizo { get; set; }

        public string IP_Realizo { get; set; }

        public string NUMEMP { get; set; }

        public int? Riesgo { get; set; }
    }
}
