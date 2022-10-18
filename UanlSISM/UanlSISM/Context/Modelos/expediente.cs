namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("expediente")]
    public partial class expediente
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string numemp { get; set; }

        [StringLength(1)]
        public string pariente { get; set; }

        [StringLength(18)]
        public string curp { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string medico { get; set; }

        [Key]
        [Column(Order = 2, TypeName = "smalldatetime")]
        public DateTime fecha { get; set; }

        [StringLength(7)]
        public string diagnostico { get; set; }

        [StringLength(7)]
        public string diagnostico2 { get; set; }

        [StringLength(7)]
        public string diagnostico3 { get; set; }

        [Column(TypeName = "ntext")]
        public string s_exp { get; set; }

        [Column(TypeName = "ntext")]
        public string o_exp { get; set; }

        [Column(TypeName = "ntext")]
        public string a_exp { get; set; }

        [Column(TypeName = "ntext")]
        public string p_exp { get; set; }

        [StringLength(3)]
        public string pesokg { get; set; }

        [StringLength(10)]
        public string pesomg { get; set; }

        [StringLength(3)]
        public string tallam { get; set; }

        [StringLength(10)]
        public string tallac { get; set; }

        [StringLength(6)]
        public string ta1 { get; set; }

        [StringLength(10)]
        public string ta2 { get; set; }

        [StringLength(1)]
        public string tipconsulta { get; set; }

        [StringLength(10)]
        public string emprealizo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? frealizo { get; set; }

        [StringLength(4)]
        public string turno { get; set; }

        [StringLength(2)]
        public string referido { get; set; }

        [StringLength(1)]
        public string referido_urg { get; set; }

        [StringLength(1)]
        public string incapacidad { get; set; }

        [StringLength(1)]
        public string laboratorio { get; set; }

        [StringLength(1)]
        public string receta { get; set; }

        [StringLength(2)]
        public string edd_anio { get; set; }

        [StringLength(2)]
        public string edd_mes { get; set; }

        [StringLength(5)]
        public string MED_TMP { get; set; }

        [Column(TypeName = "ntext")]
        public string ref_exp { get; set; }

        [Column(TypeName = "ntext")]
        public string c_ref_exp { get; set; }

        [StringLength(1)]
        public string ref_stat { get; set; }

        public float? talla_r { get; set; }

        public float? peso_r { get; set; }

        public float? temp_r { get; set; }

        [StringLength(3)]
        public string fresp { get; set; }

        [StringLength(3)]
        public string fcard { get; set; }

        [StringLength(2)]
        public string rxd { get; set; }

        [StringLength(3)]
        public string dstx { get; set; }

        public DateTime? hora_termino { get; set; }

        [StringLength(1)]
        public string tipconsulta2 { get; set; }

        [StringLength(1)]
        public string tipconsulta3 { get; set; }

        [StringLength(1)]
        public string consultadistancia { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ubicacion_realiza { get; set; }

        [StringLength(15)]
        public string ip_realiza { get; set; }

        [StringLength(2)]
        public string referido2 { get; set; }

        [StringLength(1)]
        public string referido_urg2 { get; set; }

        [Column(TypeName = "text")]
        public string ref_exp2 { get; set; }

        [StringLength(2)]
        public string referido3 { get; set; }

        [StringLength(1)]
        public string referido_urg3 { get; set; }

        [Column(TypeName = "text")]
        public string ref_exp3 { get; set; }

        [Column(TypeName = "text")]
        public string proxima_cita { get; set; }

        public DateTime? fecha_prox_cita { get; set; }

        [StringLength(4)]
        public string per_cefalico { get; set; }

        [StringLength(4)]
        public string masa_muscular { get; set; }

        [StringLength(4)]
        public string masa_grasa { get; set; }

        [StringLength(4)]
        public string porcentaje_grasa { get; set; }
    }
}
