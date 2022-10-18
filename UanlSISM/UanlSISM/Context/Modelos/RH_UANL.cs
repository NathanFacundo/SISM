namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RH_UANL
    {
        [Key]
        [Column(Order = 0, TypeName = "numeric")]
        public decimal NO_PERSONAL { get; set; }

        [StringLength(13)]
        public string RFC { get; set; }

        [StringLength(13)]
        public string Expr1 { get; set; }

        [StringLength(13)]
        public string Expr2 { get; set; }

        [StringLength(18)]
        public string CURP { get; set; }

        [StringLength(18)]
        public string Expr3 { get; set; }

        [StringLength(18)]
        public string Expr4 { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(30)]
        public string A_PATERNO { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(30)]
        public string Expr5 { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(30)]
        public string Expr6 { get; set; }

        [StringLength(30)]
        public string A_MATERNO { get; set; }

        [StringLength(30)]
        public string Expr7 { get; set; }

        [StringLength(30)]
        public string Expr8 { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(30)]
        public string NOMBRE { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(30)]
        public string Expr9 { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(30)]
        public string Expr10 { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(1)]
        public string ESTADO_CIVIL { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(1)]
        public string Expr11 { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(1)]
        public string Expr12 { get; set; }

        [Key]
        [Column(Order = 10)]
        public DateTime FECHA_ANTIGUEDAD { get; set; }

        [Key]
        [Column(Order = 11)]
        public DateTime Expr13 { get; set; }

        [Key]
        [Column(Order = 12)]
        public DateTime Expr14 { get; set; }

        [Key]
        [Column(Order = 13)]
        public DateTime Expr15 { get; set; }

        [StringLength(1)]
        public string ESTATUS { get; set; }

        [StringLength(1)]
        public string Expr16 { get; set; }

        [StringLength(1)]
        public string Expr17 { get; set; }

        public DateTime? FECHA_BAJA { get; set; }

        public DateTime? Expr18 { get; set; }

        public DateTime? Expr19 { get; set; }

        public DateTime? Expr20 { get; set; }

        [StringLength(2)]
        public string DSM { get; set; }

        [StringLength(2)]
        public string Expr21 { get; set; }

        [StringLength(2)]
        public string Expr22 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? NO_PARAMETRO { get; set; }

        [StringLength(4)]
        public string AREA { get; set; }

        [StringLength(4)]
        public string Expr23 { get; set; }

        [StringLength(4)]
        public string Expr24 { get; set; }

        [StringLength(60)]
        public string DESC_AREA { get; set; }

        [StringLength(60)]
        public string Expr25 { get; set; }

        [StringLength(60)]
        public string Expr26 { get; set; }

        [StringLength(1)]
        public string TIPO_PUESTO { get; set; }

        [StringLength(1)]
        public string Expr27 { get; set; }

        [StringLength(1)]
        public string Expr28 { get; set; }

        [StringLength(20)]
        public string DESC_TIPO_PUESTO { get; set; }

        [StringLength(20)]
        public string Expr29 { get; set; }

        [StringLength(20)]
        public string Expr30 { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HORAS_TRABAJO { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? HORAS_CLASE { get; set; }

        [StringLength(1)]
        public string TIPO_CONTRATO { get; set; }

        [StringLength(1)]
        public string Expr31 { get; set; }

        [StringLength(1)]
        public string Expr32 { get; set; }

        public DateTime? FECHA_VENCIMIENTO { get; set; }

        public DateTime? Expr33 { get; set; }

        public DateTime? Expr34 { get; set; }

        public DateTime? Expr35 { get; set; }
    }
}
