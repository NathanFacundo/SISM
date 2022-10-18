namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Directorio")]
    public partial class Directorio
    {
        public int Id { get; set; }

        [Column(TypeName = "text")]
        public string Nombre { get; set; }

        [Column(TypeName = "text")]
        public string Usuario { get; set; }

        [Column(TypeName = "text")]
        public string Departamento { get; set; }

        [Column(TypeName = "text")]
        public string Ubicacion { get; set; }

        [Column(TypeName = "text")]
        public string Puesto { get; set; }

        [Column(TypeName = "text")]
        public string NombreEquipo { get; set; }

        [Column(TypeName = "text")]
        public string IP { get; set; }

        [Column(TypeName = "text")]
        public string Tipo { get; set; }

        [Column(TypeName = "text")]
        public string MarcaCPU { get; set; }

        [Column(TypeName = "text")]
        public string ModeloCPU { get; set; }

        [Column(TypeName = "text")]
        public string SerieCPU { get; set; }

        [Column(TypeName = "text")]
        public string MarcaMonitor { get; set; }

        [Column(TypeName = "text")]
        public string ModeloMonitor { get; set; }

        [Column(TypeName = "text")]
        public string SerieMonitor { get; set; }

        [Column(TypeName = "text")]
        public string Office { get; set; }

        [Column(TypeName = "text")]
        public string ModeloImpresora { get; set; }

        [Column(TypeName = "text")]
        public string SerieImpresora { get; set; }

        [Column(TypeName = "text")]
        public string ConexionImpresora { get; set; }

        [Column(TypeName = "text")]
        public string ModeloTelefono { get; set; }

        [Column(TypeName = "text")]
        public string SerieTelefono { get; set; }

        [Column(TypeName = "text")]
        public string NumDirecto { get; set; }

        public int? Extension { get; set; }

        [Column(TypeName = "text")]
        public string Display { get; set; }
    }
}
