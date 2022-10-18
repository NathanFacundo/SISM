namespace UanlSISM.Context.Modelos
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SolicitudLab")]
    public partial class SolicitudLab
    {
        [Key]
        public int Solicitud_Id { get; set; }

        [Required]
        [StringLength(8)]
        public string Paciente_Id { get; set; }

        [Required]
        [StringLength(45)]
        public string Paciente_Nombre { get; set; }

        [Required]
        [StringLength(45)]
        public string Paciente_Apellido_Paterno { get; set; }

        [Required]
        [StringLength(45)]
        public string Paciente_Apellido_Materno { get; set; }

        public int Paciente_Sexo { get; set; }

        public DateTime Paciente_Fecha_Nacimiento { get; set; }

        [Required]
        [StringLength(20)]
        public string Paciente_Telefono { get; set; }

        [Required]
        [StringLength(50)]
        public string Paciente_Correo { get; set; }

        [StringLength(10)]
        public string Paciente_Dependencia_Id { get; set; }

        [StringLength(10)]
        public string Paciente_Dependencia_Descripcion { get; set; }

        [Required]
        [StringLength(8)]
        public string Solicitud_Expediente { get; set; }

        [Required]
        [StringLength(5)]
        public string Solicitud_Medico_Id { get; set; }

        [Required]
        [StringLength(70)]
        public string Solicitud_Medico_Nombre { get; set; }

        [Required]
        [StringLength(7)]
        public string Solicitud_Diagnostico1_Id { get; set; }

        [Required]
        [StringLength(254)]
        public string Solicitud_Diagnostico1_Descripcion { get; set; }

        [Required]
        [StringLength(7)]
        public string Solicitud_Diagnostico2_Id { get; set; }

        [Required]
        [StringLength(254)]
        public string Solicitud_Diagnostico2_Descripcion { get; set; }

        [Required]
        [StringLength(7)]
        public string Solicitud_Diagnostico3_Id { get; set; }

        [Required]
        [StringLength(254)]
        public string Solicitud_Diagnostico3_Descripcion { get; set; }

        [Required]
        [StringLength(1)]
        public string Solicitud_Urgente { get; set; }

        [Required]
        [StringLength(2)]
        public string Solicitud_Consultorio { get; set; }

        [Required]
        [StringLength(10)]
        public string Solicitud_AreaMedica_Id { get; set; }

        [Required]
        [StringLength(45)]
        public string Solicitud_AreaMedica_Descripcion { get; set; }

        [Required]
        [StringLength(50)]
        public string Enterprise_Folio { get; set; }
    }
}
