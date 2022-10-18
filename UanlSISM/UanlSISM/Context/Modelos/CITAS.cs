namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CITAS
    {
        [StringLength(8)]
        public string NumEmp { get; set; }

        [StringLength(1)]
        public string Pariente { get; set; }

        [StringLength(18)]
        public string Curp { get; set; }

        [StringLength(4)]
        public string Dependencia { get; set; }

        [StringLength(1)]
        public string Sexo { get; set; }

        [StringLength(2)]
        public string Edad { get; set; }

        [StringLength(5)]
        public string Medico { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? Fecha { get; set; }

        [StringLength(4)]
        public string turno { get; set; }

        [StringLength(2)]
        public string Tipo { get; set; }

        [StringLength(30)]
        public string Estudios { get; set; }

        [StringLength(7)]
        public string Diagnostico { get; set; }

        [StringLength(5)]
        public string CanalMed { get; set; }

        [StringLength(16)]
        public string EmpRealizo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FRealizo { get; set; }

        public short? REGXDIA { get; set; }

        public DateTime? hr_recepcion { get; set; }

        [StringLength(5)]
        public string hora_recepcion { get; set; }

        [StringLength(1)]
        public string normalsub { get; set; }

        [StringLength(1)]
        public string tratamiento { get; set; }

        [StringLength(1)]
        public string asist_cons { get; set; }

        [StringLength(3)]
        public string ta1 { get; set; }

        [StringLength(3)]
        public string ta2 { get; set; }

        [StringLength(4)]
        public string temperatura { get; set; }

        [StringLength(7)]
        public string peso { get; set; }

        [StringLength(4)]
        public string talla { get; set; }

        [StringLength(2)]
        public string fresp { get; set; }

        [StringLength(3)]
        public string fcard { get; set; }

        [StringLength(10)]
        public string enf_realizo { get; set; }

        public float? temp_R { get; set; }

        public float? peso_r { get; set; }

        public float? talla_r { get; set; }

        [StringLength(3)]
        public string dstx { get; set; }

        [StringLength(1)]
        public string tipo_reg { get; set; }

        [StringLength(5)]
        public string hr_llamado { get; set; }

        [StringLength(5)]
        public string hr_consultorio { get; set; }

        [StringLength(2)]
        public string recepcion_mt { get; set; }

        [StringLength(1)]
        public string urg_real { get; set; }

        [StringLength(1)]
        public string cita_confirm { get; set; }

        [StringLength(50)]
        public string cambiorealiza { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? examen_ppncl { get; set; }

        public DateTime? fecha_kiosco { get; set; }

        public DateTime? fecha_tarde { get; set; }

        public bool? NoContesto { get; set; }

        public bool? SeCortoComunicacion { get; set; }

        public bool? Otros { get; set; }

        [StringLength(50)]
        public string no_contesto { get; set; }

        public int Id { get; set; }

        public virtual DEPENDENCIAS DEPENDENCIAS { get; set; }

        public virtual DHABIENTES DHABIENTES { get; set; }

        public virtual DIAGNOSTICOS DIAGNOSTICOS { get; set; }

        public virtual PARENTESCO PARENTESCO { get; set; }

        public virtual TIPOSCONSULTA TIPOSCONSULTA { get; set; }
    }
}
