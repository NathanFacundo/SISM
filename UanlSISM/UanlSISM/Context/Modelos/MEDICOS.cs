namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MEDICOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MEDICOS()
        {
            DetalleHorarioMedicos = new HashSet<DetalleHorarioMedicos>();
        }

        [StringLength(1)]
        public string Estatus { get; set; }

        [Key]
        [StringLength(5)]
        public string Numero { get; set; }

        [StringLength(20)]
        public string Apaterno { get; set; }

        [StringLength(20)]
        public string Amaterno { get; set; }

        [StringLength(30)]
        public string Nombre { get; set; }

        [StringLength(4)]
        public string Titulo { get; set; }

        [StringLength(2)]
        public string Consultorio { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FCancelini { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FCancelFin { get; set; }

        [StringLength(80)]
        public string ObsCancel { get; set; }

        [StringLength(2)]
        public string Recepcion { get; set; }

        [StringLength(20)]
        public string Cedula { get; set; }

        [StringLength(250)]
        public string HrLunes { get; set; }

        [StringLength(80)]
        public string ObLunes { get; set; }

        [StringLength(2)]
        public string pac_lun { get; set; }

        [StringLength(8)]
        public string hrlun { get; set; }

        [StringLength(250)]
        public string HrMartes { get; set; }

        [StringLength(80)]
        public string ObMartes { get; set; }

        [StringLength(2)]
        public string pac_mar { get; set; }

        [StringLength(8)]
        public string hrmar { get; set; }

        [StringLength(250)]
        public string HrMiercoles { get; set; }

        [StringLength(80)]
        public string ObMiercoles { get; set; }

        [StringLength(2)]
        public string pac_mie { get; set; }

        [StringLength(8)]
        public string hrmie { get; set; }

        [StringLength(250)]
        public string HrJueves { get; set; }

        [StringLength(80)]
        public string ObJueves { get; set; }

        [StringLength(2)]
        public string pac_jue { get; set; }

        [StringLength(8)]
        public string hrjue { get; set; }

        [StringLength(250)]
        public string HrViernes { get; set; }

        [StringLength(80)]
        public string ObViernes { get; set; }

        [StringLength(2)]
        public string pac_vie { get; set; }

        [StringLength(8)]
        public string hrvie { get; set; }

        [StringLength(250)]
        public string HrSabado { get; set; }

        [StringLength(80)]
        public string ObSabado { get; set; }

        [StringLength(2)]
        public string pac_sab { get; set; }

        [StringLength(8)]
        public string hrsab { get; set; }

        [StringLength(250)]
        public string HrDomingo { get; set; }

        [StringLength(80)]
        public string ObDomingo { get; set; }

        [StringLength(2)]
        public string pac_dom { get; set; }

        [StringLength(8)]
        public string hrdom { get; set; }

        [StringLength(5)]
        public string Socio1 { get; set; }

        [StringLength(5)]
        public string Socio2 { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FCubreIni { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? FCubreFin { get; set; }

        [StringLength(5)]
        public string CubreMed { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? hora { get; set; }

        public int? bloqueado { get; set; }

        [StringLength(2)]
        public string cveesp { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? fechab { get; set; }

        [StringLength(16)]
        public string USUARIO { get; set; }

        [StringLength(2)]
        public string CLAVE { get; set; }

        [StringLength(2)]
        public string TIP_HORARIO { get; set; }

        [StringLength(1)]
        public string Estatus_Lab { get; set; }

        [StringLength(2)]
        public string GRUPO_DIR { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? f2cancelini { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? f2cancelfin { get; set; }

        [StringLength(80)]
        public string obs2cancel { get; set; }

        [StringLength(10)]
        public string medico_grupo { get; set; }

        public int? accesoweb { get; set; }

        [StringLength(20)]
        public string consultorio_mt { get; set; }

        [StringLength(1)]
        public string sexo { get; set; }

        [StringLength(2)]
        public string cve_uni { get; set; }

        [StringLength(20)]
        public string cedula_esp { get; set; }

        [StringLength(100)]
        public string temporal { get; set; }

        public int? emp_numemp { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? unidad_medica { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? exap_max { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetalleHorarioMedicos> DetalleHorarioMedicos { get; set; }
    }
}
