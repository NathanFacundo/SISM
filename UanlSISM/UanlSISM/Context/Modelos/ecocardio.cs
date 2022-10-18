namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ecocardio")]
    public partial class ecocardio
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(8)]
        public string EXPEDIENTE { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "smalldatetime")]
        public DateTime FECHA { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string SEXO { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(5)]
        public string MEDICO { get; set; }

        [StringLength(60)]
        public string MEDICO_REALIZA { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "numeric")]
        public decimal FOLIO { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(3)]
        public string EDAD { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(4)]
        public string PESO { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(4)]
        public string TALLA { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(8)]
        public string SC { get; set; }

        [StringLength(8)]
        public string FC { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(20)]
        public string ventana { get; set; }

        [StringLength(10)]
        public string dimvd { get; set; }

        [StringLength(10)]
        public string dimsd { get; set; }

        [StringLength(10)]
        public string dimss { get; set; }

        [StringLength(10)]
        public string dimcvid { get; set; }

        [StringLength(10)]
        public string dimcvis { get; set; }

        [StringLength(10)]
        public string dimpp4d { get; set; }

        [StringLength(10)]
        public string dimpp4s { get; set; }

        [StringLength(10)]
        public string dimrao { get; set; }

        [StringLength(10)]
        public string dimapao { get; set; }

        [StringLength(10)]
        public string dimauiz { get; set; }

        [StringLength(20)]
        public string motivo { get; set; }

        [StringLength(2)]
        public string FV01 { get; set; }

        [StringLength(2)]
        public string FV02 { get; set; }

        [StringLength(2)]
        public string FV03 { get; set; }

        [StringLength(2)]
        public string FV04 { get; set; }

        [StringLength(2)]
        public string FV05 { get; set; }

        [StringLength(2)]
        public string FV06 { get; set; }

        [StringLength(2)]
        public string FV07 { get; set; }

        [StringLength(2)]
        public string FV08 { get; set; }

        [StringLength(2)]
        public string FV09 { get; set; }

        [StringLength(2)]
        public string FV10 { get; set; }

        [StringLength(2)]
        public string FV11 { get; set; }

        [StringLength(2)]
        public string FV12 { get; set; }

        [StringLength(2)]
        public string FV13 { get; set; }

        [StringLength(2)]
        public string FV14 { get; set; }

        [StringLength(2)]
        public string FV15 { get; set; }

        [StringLength(2)]
        public string FV16 { get; set; }

        [StringLength(2)]
        public string FV17 { get; set; }

        [StringLength(2)]
        public string FV18 { get; set; }

        [StringLength(2)]
        public string FV19 { get; set; }

        [StringLength(2)]
        public string FV20 { get; set; }

        [StringLength(2)]
        public string FV21 { get; set; }

        [StringLength(2)]
        public string FV22 { get; set; }

        [StringLength(10)]
        public string FVVTD4 { get; set; }

        [StringLength(10)]
        public string FVVTD2 { get; set; }

        [StringLength(10)]
        public string FVVL { get; set; }

        [StringLength(10)]
        public string FVVTS4 { get; set; }

        [StringLength(10)]
        public string FVVTS2 { get; set; }

        [StringLength(10)]
        public string FVAF { get; set; }

        [StringLength(10)]
        public string FVFE4 { get; set; }

        [StringLength(10)]
        public string FVFE2 { get; set; }

        [StringLength(70)]
        public string MVMI { get; set; }

        [StringLength(70)]
        public string MVAO { get; set; }

        [StringLength(70)]
        public string MVPU { get; set; }

        [StringLength(70)]
        public string MVTR { get; set; }

        [StringLength(10)]
        public string MIVMM { get; set; }

        [StringLength(10)]
        public string MIGRMX { get; set; }

        [StringLength(10)]
        public string MIGRMD { get; set; }

        [StringLength(10)]
        public string MIPIE { get; set; }

        [StringLength(10)]
        public string MIPIA { get; set; }

        [StringLength(10)]
        public string MIAVM { get; set; }

        [StringLength(10)]
        public string MITIH { get; set; }

        [StringLength(10)]
        public string MITID { get; set; }

        [StringLength(10)]
        public string MIINS { get; set; }

        [StringLength(10)]
        public string MIVJIM { get; set; }

        [StringLength(10)]
        public string PUVPM { get; set; }

        [StringLength(10)]
        public string PUGRMX { get; set; }

        [StringLength(10)]
        public string PUGRMD { get; set; }

        [StringLength(10)]
        public string PUINS { get; set; }

        [StringLength(10)]
        public string PUTIPM { get; set; }

        [StringLength(10)]
        public string PUPDAP { get; set; }

        [StringLength(10)]
        public string AOVAM { get; set; }

        [StringLength(10)]
        public string AOGMX { get; set; }

        [StringLength(10)]
        public string AOGMD { get; set; }

        [StringLength(10)]
        public string AOINS { get; set; }

        [StringLength(10)]
        public string AOPINS { get; set; }

        [StringLength(10)]
        public string AOAVA { get; set; }

        [StringLength(10)]
        public string AOITV { get; set; }

        [StringLength(10)]
        public string TRVTMX { get; set; }

        [StringLength(10)]
        public string TRGRMX { get; set; }

        [StringLength(10)]
        public string TRGRMD { get; set; }

        [StringLength(10)]
        public string TRINS { get; set; }

        [StringLength(10)]
        public string TRVJIT { get; set; }

        [StringLength(10)]
        public string TRPAPU { get; set; }

        [StringLength(70)]
        public string MSHU { get; set; }

        [StringLength(70)]
        public string MSTRO { get; set; }

        [StringLength(70)]
        public string MSAVAI { get; set; }

        [StringLength(70)]
        public string MSAVAD { get; set; }

        [StringLength(10)]
        public string SMENG { get; set; }

        [StringLength(10)]
        public string SMCAL { get; set; }

        [StringLength(10)]
        public string SMPLI { get; set; }

        [StringLength(10)]
        public string SMAPS { get; set; }

        [Column(TypeName = "ntext")]
        public string HALLAZGO { get; set; }

        [Column(TypeName = "ntext")]
        public string DIAGNOSTICO { get; set; }

        [StringLength(10)]
        public string FVGAC { get; set; }

        [StringLength(110)]
        public string DXL1 { get; set; }

        [StringLength(110)]
        public string DXL2 { get; set; }

        [StringLength(110)]
        public string DXL3 { get; set; }

        [StringLength(110)]
        public string DXL4 { get; set; }

        [StringLength(110)]
        public string DXL5 { get; set; }

        [StringLength(110)]
        public string DXL6 { get; set; }

        [StringLength(110)]
        public string DXL7 { get; set; }

        [StringLength(110)]
        public string DXL8 { get; set; }

        [Column(TypeName = "text")]
        public string dx2 { get; set; }
    }
}
