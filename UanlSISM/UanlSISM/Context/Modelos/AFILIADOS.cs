namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class AFILIADOS
    {
        [Key]
        [StringLength(6)]
        public string NUMEMP { get; set; }

        [StringLength(6)]
        public string numemp2 { get; set; }

        [StringLength(45)]
        public string NOMBRES { get; set; }

        [StringLength(45)]
        public string APATERNO { get; set; }

        [StringLength(45)]
        public string AMATERNO { get; set; }

        [StringLength(2)]
        public string TIPOAFIL { get; set; }

        [StringLength(4)]
        public string CVEDEP { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FINGRESO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FVIGENCIA { get; set; }

        [StringLength(18)]
        public string CURP { get; set; }

        [StringLength(3)]
        public string PUESTO { get; set; }

        [StringLength(50)]
        public string ACTVDES { get; set; }

        [StringLength(13)]
        public string RFC { get; set; }

        [StringLength(72)]
        public string DOMICILIO { get; set; }

        [StringLength(5)]
        public string COLONIA { get; set; }

        [StringLength(40)]
        public string TELEFONOS { get; set; }

        [StringLength(2)]
        public string MOTBAJA { get; set; }

        [StringLength(1)]
        public string SEXO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fnac { get; set; }

        [StringLength(3)]
        public string DEP_OLD { get; set; }

        [StringLength(2)]
        public string OCUPACION { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FCREDENCIAL { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FBAJA { get; set; }

        [Column(TypeName = "text")]
        public string DESC_BAJA { get; set; }

        [StringLength(50)]
        public string COLONIA_DESC { get; set; }

        [StringLength(2)]
        public string CUIDAD { get; set; }

        [StringLength(1)]
        public string NOMINA { get; set; }

        [StringLength(4)]
        public string area { get; set; }

        [StringLength(10)]
        public string emp_realizo { get; set; }

        [StringLength(1)]
        public string RHUANL { get; set; }

        [StringLength(6)]
        public string num_contrato { get; set; }

        [StringLength(1)]
        public string status_rep { get; set; }

        [StringLength(2)]
        public string TMP { get; set; }

        [StringLength(255)]
        public string colonia_se { get; set; }

        [StringLength(6)]
        public string num_tmp { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(5)]
        public string plano_ubic { get; set; }

        [StringLength(5)]
        public string coord_ubic { get; set; }

        [StringLength(15)]
        public string tel_oficina { get; set; }

        [StringLength(15)]
        public string tel_celular { get; set; }

        [StringLength(2)]
        public string dersm { get; set; }

        [StringLength(20)]
        public string pswd { get; set; }

        [StringLength(1)]
        public string grupo_medico { get; set; }

        [StringLength(1)]
        public string grupo_pediatra { get; set; }

        [StringLength(6)]
        public string medicofam { get; set; }

        [StringLength(6)]
        public string pediatra { get; set; }

        public virtual AFILIACION AFILIACION { get; set; }

        public virtual DEPENDENCIAS DEPENDENCIAS { get; set; }

        public virtual MOTIVOSBAJA MOTIVOSBAJA { get; set; }

        public virtual PUESTOS PUESTOS { get; set; }
    }
}
