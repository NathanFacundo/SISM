namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DHABIENTES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DHABIENTES()
        {
            CITAS = new HashSet<CITAS>();
        }

        [Key]
        [StringLength(8)]
        public string NUMEMP { get; set; }

        [StringLength(18)]
        public string CURP { get; set; }

        [StringLength(1)]
        public string PARIENTE { get; set; }

        [StringLength(45)]
        public string NOMBRES { get; set; }

        [StringLength(45)]
        public string APATERNO { get; set; }

        [StringLength(45)]
        public string AMATERNO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FNAC { get; set; }

        [StringLength(1)]
        public string SEXO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FECALTA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FVIGENCIA { get; set; }

        [StringLength(5)]
        public string CVEMED { get; set; }

        [StringLength(40)]
        public string EMAIL { get; set; }

        [StringLength(6)]
        public string NUMAFIL { get; set; }

        [StringLength(2)]
        public string OCUPACION { get; set; }

        [StringLength(2)]
        public string BAJA { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FBAJA { get; set; }

        [Column(TypeName = "text")]
        public string DESC_BAJA { get; set; }

        [StringLength(1)]
        public string ARCHIVO { get; set; }

        [StringLength(10)]
        public string EMP_REALIZO { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fec_foto { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fec_cred { get; set; }

        [StringLength(1)]
        public string status_rep { get; set; }

        [StringLength(1)]
        public string protocolo_vac { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fec_alta_cei { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fec_ing_cei { get; set; }

        [StringLength(2)]
        public string salon { get; set; }

        [StringLength(1)]
        public string num_cei { get; set; }

        [StringLength(15)]
        public string tel_oficina { get; set; }

        [StringLength(15)]
        public string tel_casa { get; set; }

        [StringLength(15)]
        public string tel_celular { get; set; }

        [StringLength(10)]
        public string fv_registro { get; set; }

        public DateTime? fv_fecha { get; set; }

        [Column(TypeName = "ntext")]
        public string fv_nota { get; set; }

        [StringLength(15)]
        public string fv_ip { get; set; }

        [Column(TypeName = "ntext")]
        public string FV_NOTA2 { get; set; }

        [StringLength(5)]
        public string medico { get; set; }

        [StringLength(2)]
        public string medico_grupo { get; set; }

        [StringLength(9)]
        public string REGHU { get; set; }

        [StringLength(5)]
        public string medico_dmhta { get; set; }

        [StringLength(8)]
        public string mat_uanl { get; set; }

        [StringLength(4)]
        public string esc_uanl { get; set; }

        [StringLength(1)]
        public string tipo_asign_med { get; set; }

        [StringLength(15)]
        public string tel_consulta { get; set; }

        [Column(TypeName = "text")]
        public string foto { get; set; }

        [StringLength(16)]
        public string credencial_exp { get; set; }

        public DateTime? fecha_credencial { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CITAS> CITAS { get; set; }

        public virtual PARENTESCO PARENTESCO { get; set; }
    }
}
