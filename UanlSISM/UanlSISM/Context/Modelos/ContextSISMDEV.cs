using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace UanlSISM.Context.Modelos
{
    public partial class ContextSISMDEV : DbContext
    {
        public ContextSISMDEV()
            : base("name=ContextoSMDEV")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<ActividadesDepartamento> ActividadesDepartamento { get; set; }
        public virtual DbSet<AFILIACION> AFILIACION { get; set; }
        public virtual DbSet<AFILIADOS> AFILIADOS { get; set; }
        public virtual DbSet<Alergias_Exp> Alergias_Exp { get; set; }
        public virtual DbSet<ARTICULOS> ARTICULOS { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<CITAS> CITAS { get; set; }
        public virtual DbSet<Citas_Medicos_Externos> Citas_Medicos_Externos { get; set; }
        public virtual DbSet<Comunicados> Comunicados { get; set; }
        public virtual DbSet<ComunicadoUsuario> ComunicadoUsuario { get; set; }
        public virtual DbSet<CovidSolicitud> CovidSolicitud { get; set; }
        public virtual DbSet<Dental_N1> Dental_N1 { get; set; }
        public virtual DbSet<DEPENDENCIAS> DEPENDENCIAS { get; set; }
        public virtual DbSet<Desc_Presentacion> Desc_Presentacion { get; set; }
        public virtual DbSet<DetalleHorarioMedicos> DetalleHorarioMedicos { get; set; }
        public virtual DbSet<DHABIENTES> DHABIENTES { get; set; }
        public virtual DbSet<DIAGNOSTICOS> DIAGNOSTICOS { get; set; }
        public virtual DbSet<dtproperties> dtproperties { get; set; }
        public virtual DbSet<EMP_JHG> EMP_JHG { get; set; }
        public virtual DbSet<encuesta_covid> encuesta_covid { get; set; }
        public virtual DbSet<encuesta_covid_segunda> encuesta_covid_segunda { get; set; }
        public virtual DbSet<EnfermeriaSolicitud> EnfermeriaSolicitud { get; set; }
        public virtual DbSet<EnfermeriaSolicitudDetalle> EnfermeriaSolicitudDetalle { get; set; }
        public virtual DbSet<FESTIVO> FESTIVO { get; set; }
        public virtual DbSet<HCL_CATALOGO> HCL_CATALOGO { get; set; }
        public virtual DbSet<Interconsultas> Interconsultas { get; set; }
        public virtual DbSet<lab_catalogo> lab_catalogo { get; set; }
        public virtual DbSet<MEDICOS> MEDICOS { get; set; }
        public virtual DbSet<MONITOR> MONITOR { get; set; }
        public virtual DbSet<MOTIVOSBAJA> MOTIVOSBAJA { get; set; }
        public virtual DbSet<NotaEgreso> NotaEgreso { get; set; }
        public virtual DbSet<NotaFar_Usuario> NotaFar_Usuario { get; set; }
        public virtual DbSet<NotaIngreso> NotaIngreso { get; set; }
        public virtual DbSet<NotaOperatoria> NotaOperatoria { get; set; }
        public virtual DbSet<NotaPreoperatoria> NotaPreoperatoria { get; set; }
        public virtual DbSet<NUEVOLEON> NUEVOLEON { get; set; }
        public virtual DbSet<Orden_Int> Orden_Int { get; set; }
        public virtual DbSet<Orden_Int_Enviar> Orden_Int_Enviar { get; set; }
        public virtual DbSet<PARENTESCO> PARENTESCO { get; set; }
        public virtual DbSet<prov_subrrogados> prov_subrrogados { get; set; }
        public virtual DbSet<PruebaAntigenos> PruebaAntigenos { get; set; }
        public virtual DbSet<PUESTOS> PUESTOS { get; set; }
        public virtual DbSet<RECETA_EXP> RECETA_EXP { get; set; }
        public virtual DbSet<receta_exp_crn_reactivar> receta_exp_crn_reactivar { get; set; }
        public virtual DbSet<REP_EPI> REP_EPI { get; set; }
        public virtual DbSet<ReporteActividadesSemanal> ReporteActividadesSemanal { get; set; }
        public virtual DbSet<serv_rx_n1> serv_rx_n1 { get; set; }
        public virtual DbSet<serv_rx_n2> serv_rx_n2 { get; set; }
        public virtual DbSet<SolicitudLab> SolicitudLab { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TBLGRUPOS> TBLGRUPOS { get; set; }
        public virtual DbSet<TblNotaFarmaco> TblNotaFarmaco { get; set; }
        public virtual DbSet<TBLSALES> TBLSALES { get; set; }
        public virtual DbSet<tema_enca> tema_enca { get; set; }
        public virtual DbSet<Tickets> Tickets { get; set; }
        public virtual DbSet<TipCita> TipCita { get; set; }
        public virtual DbSet<TIPOSCONSULTA> TIPOSCONSULTA { get; set; }
        public virtual DbSet<afiliado_movs> afiliado_movs { get; set; }
        public virtual DbSet<citas_sust> citas_sust { get; set; }
        public virtual DbSet<Directorio> Directorio { get; set; }
        public virtual DbSet<ecocardio> ecocardio { get; set; }
        public virtual DbSet<ESPECIALIDADES> ESPECIALIDADES { get; set; }
        public virtual DbSet<EXP_HU> EXP_HU { get; set; }
        public virtual DbSet<expediente> expediente { get; set; }
        public virtual DbSet<expediente_hta> expediente_hta { get; set; }
        public virtual DbSet<FAR_FILA> FAR_FILA { get; set; }
        public virtual DbSet<farxexp> farxexp { get; set; }
        public virtual DbSet<HCL_EXPEDIENTE> HCL_EXPEDIENTE { get; set; }
        public virtual DbSet<Lab_detalle> Lab_detalle { get; set; }
        public virtual DbSet<Lab_exp> Lab_exp { get; set; }
        public virtual DbSet<labinstrucciones> labinstrucciones { get; set; }
        public virtual DbSet<MED_HORARIO> MED_HORARIO { get; set; }
        public virtual DbSet<Query> Query { get; set; }
        public virtual DbSet<receta_detalle_crn> receta_detalle_crn { get; set; }
        public virtual DbSet<receta_exp_crn> receta_exp_crn { get; set; }
        public virtual DbSet<Resultados> Resultados { get; set; }
        public virtual DbSet<sdp_tramites> sdp_tramites { get; set; }
        public virtual DbSet<serv_pre_n1> serv_pre_n1 { get; set; }
        public virtual DbSet<serv_pre_n2> serv_pre_n2 { get; set; }
        public virtual DbSet<ServicioEnfermeria> ServicioEnfermeria { get; set; }
        public virtual DbSet<SOLICITUD_LAB> SOLICITUD_LAB { get; set; }
        public virtual DbSet<spre_catalogo> spre_catalogo { get; set; }
        public virtual DbSet<spre_exp> spre_exp { get; set; }
        public virtual DbSet<spre_exp_detalle> spre_exp_detalle { get; set; }
        public virtual DbSet<spre_permiso> spre_permiso { get; set; }
        public virtual DbSet<tema_detalle> tema_detalle { get; set; }
        public virtual DbSet<InventarioFarmacia> InventarioFarmacia { get; set; }
        public virtual DbSet<InventarioFarmaciaNiveles> InventarioFarmaciaNiveles { get; set; }
        public virtual DbSet<Receta_Detalle_2> Receta_Detalle_2 { get; set; }
        public virtual DbSet<RECETA_EXP_2> RECETA_EXP_2 { get; set; }
        public virtual DbSet<RH_UANL> RH_UANL { get; set; }
        public virtual DbSet<sales> sales { get; set; }
        public virtual DbSet<Sustancia> Sustancia { get; set; }
        public virtual DbSet<Usuario> Usuario { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActividadesDepartamento>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<ActividadesDepartamento>()
                .Property(e => e.Departamento)
                .IsUnicode(false);

            modelBuilder.Entity<ActividadesDepartamento>()
                .Property(e => e.Ubicacion)
                .IsUnicode(false);

            modelBuilder.Entity<AFILIACION>()
                .Property(e => e.tot_recetas)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AFILIACION>()
                .HasMany(e => e.AFILIADOS)
                .WithOptional(e => e.AFILIACION)
                .WillCascadeOnDelete();

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.OCUPACION)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.DESC_BAJA)
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.emp_realizo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.RHUANL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.num_contrato)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.status_rep)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.TMP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.colonia_se)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.tel_oficina)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.tel_celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.pswd)
                .IsFixedLength();

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.grupo_medico)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.grupo_pediatra)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.medicofam)
                .IsFixedLength();

            modelBuilder.Entity<AFILIADOS>()
                .Property(e => e.pediatra)
                .IsFixedLength();

            modelBuilder.Entity<Alergias_Exp>()
                .Property(e => e.medicamento)
                .IsUnicode(false);

            modelBuilder.Entity<Alergias_Exp>()
                .Property(e => e.num_exp)
                .IsUnicode(false);

            modelBuilder.Entity<Alergias_Exp>()
                .Property(e => e.medico)
                .IsUnicode(false);

            modelBuilder.Entity<ARTICULOS>()
                .Property(e => e.costo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.hora_recepcion)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.normalsub)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.tratamiento)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.asist_cons)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.ta1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.ta2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.temperatura)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.peso)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.talla)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.fresp)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.fcard)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.enf_realizo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.dstx)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.tipo_reg)
                .IsFixedLength();

            modelBuilder.Entity<CITAS>()
                .Property(e => e.hr_llamado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.hr_consultorio)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.recepcion_mt)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.urg_real)
                .IsFixedLength();

            modelBuilder.Entity<CITAS>()
                .Property(e => e.cita_confirm)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.cambiorealiza)
                .IsFixedLength();

            modelBuilder.Entity<CITAS>()
                .Property(e => e.examen_ppncl)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CITAS>()
                .Property(e => e.no_contesto)
                .IsUnicode(false);

            modelBuilder.Entity<Citas_Medicos_Externos>()
                .Property(e => e.medico)
                .IsUnicode(false);

            modelBuilder.Entity<Citas_Medicos_Externos>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Comunicados>()
                .Property(e => e.aviso)
                .IsUnicode(false);

            modelBuilder.Entity<ComunicadoUsuario>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<CovidSolicitud>()
                .Property(e => e.NotaResultado)
                .IsUnicode(false);

            modelBuilder.Entity<CovidSolicitud>()
                .Property(e => e.UsuarioResultado)
                .IsUnicode(false);

            modelBuilder.Entity<Dental_N1>()
                .Property(e => e.codigo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Dental_N1>()
                .Property(e => e.descripcion)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Dental_N1>()
                .Property(e => e.especialidad)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DEPENDENCIAS>()
                .HasMany(e => e.AFILIADOS)
                .WithOptional(e => e.DEPENDENCIAS)
                .WillCascadeOnDelete();

            modelBuilder.Entity<DEPENDENCIAS>()
                .HasMany(e => e.CITAS)
                .WithOptional(e => e.DEPENDENCIAS)
                .HasForeignKey(e => e.Dependencia)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Desc_Presentacion>()
                .Property(e => e.Presentacion_Desc)
                .IsUnicode(false);

            modelBuilder.Entity<Desc_Presentacion>()
                .Property(e => e.Tipo)
                .IsUnicode(false);

            modelBuilder.Entity<DetalleHorarioMedicos>()
                .Property(e => e.EventPid)
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.OCUPACION)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.BAJA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.DESC_BAJA)
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.ARCHIVO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.EMP_REALIZO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.status_rep)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.protocolo_vac)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.salon)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.num_cei)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.tel_oficina)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.tel_casa)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.tel_celular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.fv_registro)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.fv_ip)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.medico)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.medico_grupo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.REGHU)
                .IsFixedLength();

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.medico_dmhta)
                .IsFixedLength();

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.mat_uanl)
                .IsFixedLength();

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.esc_uanl)
                .IsFixedLength();

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.tipo_asign_med)
                .IsFixedLength();

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.tel_consulta)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.foto)
                .IsUnicode(false);

            modelBuilder.Entity<DHABIENTES>()
                .Property(e => e.credencial_exp)
                .IsUnicode(false);

            modelBuilder.Entity<DIAGNOSTICOS>()
                .HasMany(e => e.CITAS)
                .WithOptional(e => e.DIAGNOSTICOS)
                .HasForeignKey(e => e.Diagnostico)
                .WillCascadeOnDelete();

            modelBuilder.Entity<dtproperties>()
                .Property(e => e.property)
                .IsUnicode(false);

            modelBuilder.Entity<dtproperties>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<EMP_JHG>()
                .Property(e => e.NUM_EMP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EMP_JHG>()
                .Property(e => e.NOM_EMP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EMP_JHG>()
                .Property(e => e.DIR_EMP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EMP_JHG>()
                .Property(e => e.TEL_EMP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<EMP_JHG>()
                .Property(e => e.SDO_EMP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.expediente)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.medico)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.veces_consulta)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.aislamiento)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.dif_respirar)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.int_requerido)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.oxi_requerido)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.vent_asistida)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.alta)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.fam_sintomas)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.derechohabiente)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.dias_prueba)
                .HasPrecision(18, 0);

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.gpo_riesgo)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid>()
                .Property(e => e.trabajo_ubica)
                .IsFixedLength();

            modelBuilder.Entity<encuesta_covid_segunda>()
                .Property(e => e.nombre)
                .IsUnicode(false);

            modelBuilder.Entity<encuesta_covid_segunda>()
                .Property(e => e.expediente)
                .IsUnicode(false);

            modelBuilder.Entity<encuesta_covid_segunda>()
                .Property(e => e.antecedentes_medicos)
                .IsUnicode(false);

            modelBuilder.Entity<encuesta_covid_segunda>()
                .Property(e => e.notas)
                .IsUnicode(false);

            modelBuilder.Entity<FESTIVO>()
                .Property(e => e.DESCRIPCION)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HCL_CATALOGO>()
                .Property(e => e.desc_larga)
                .IsUnicode(false);

            modelBuilder.Entity<HCL_CATALOGO>()
                .Property(e => e.sol_fecha)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HCL_CATALOGO>()
                .Property(e => e.sol_nota)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HCL_CATALOGO>()
                .Property(e => e.tipo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HCL_CATALOGO>()
                .Property(e => e.activo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HCL_CATALOGO>()
                .Property(e => e.cod_default)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<HCL_CATALOGO>()
                .Property(e => e.lineas)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Interconsultas>()
                .Property(e => e.especialidad)
                .IsFixedLength();

            modelBuilder.Entity<Interconsultas>()
                .Property(e => e.observaciones)
                .IsUnicode(false);

            modelBuilder.Entity<Interconsultas>()
                .Property(e => e.num_exp)
                .IsFixedLength();

            modelBuilder.Entity<Interconsultas>()
                .Property(e => e.medico)
                .IsFixedLength();

            modelBuilder.Entity<lab_catalogo>()
                .Property(e => e.lab_instrucciones)
                .HasPrecision(18, 0);

            modelBuilder.Entity<MEDICOS>()
                .Property(e => e.medico_grupo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<MEDICOS>()
                .Property(e => e.consultorio_mt)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<MEDICOS>()
                .Property(e => e.sexo)
                .IsFixedLength();

            modelBuilder.Entity<MEDICOS>()
                .Property(e => e.cve_uni)
                .IsFixedLength();

            modelBuilder.Entity<MEDICOS>()
                .Property(e => e.cedula_esp)
                .IsFixedLength();

            modelBuilder.Entity<MEDICOS>()
                .Property(e => e.unidad_medica)
                .HasPrecision(18, 0);

            modelBuilder.Entity<MEDICOS>()
                .Property(e => e.exap_max)
                .HasPrecision(18, 0);

            modelBuilder.Entity<MEDICOS>()
                .HasMany(e => e.DetalleHorarioMedicos)
                .WithRequired(e => e.MEDICOS)
                .HasForeignKey(e => e.MedicoId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MOTIVOSBAJA>()
                .HasMany(e => e.AFILIADOS)
                .WithOptional(e => e.MOTIVOSBAJA)
                .WillCascadeOnDelete();

            modelBuilder.Entity<NotaEgreso>()
                .Property(e => e.DiagnosticoDesc)
                .IsUnicode(false);

            modelBuilder.Entity<NotaEgreso>()
                .Property(e => e.ResumenClinico)
                .IsUnicode(false);

            modelBuilder.Entity<NotaEgreso>()
                .Property(e => e.ProcedimientoRealizado)
                .IsUnicode(false);

            modelBuilder.Entity<NotaEgreso>()
                .Property(e => e.ProcProblemasPendientes)
                .IsUnicode(false);

            modelBuilder.Entity<NotaEgreso>()
                .Property(e => e.Medicacion)
                .IsUnicode(false);

            modelBuilder.Entity<NotaEgreso>()
                .Property(e => e.RecomendacionesIncapacidad)
                .IsUnicode(false);

            modelBuilder.Entity<NotaEgreso>()
                .Property(e => e.Pronostico)
                .IsUnicode(false);

            modelBuilder.Entity<NotaFar_Usuario>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<NotaIngreso>()
                .Property(e => e.A)
                .IsFixedLength();

            modelBuilder.Entity<NotaIngreso>()
                .Property(e => e.Temp)
                .HasPrecision(18, 0);

            modelBuilder.Entity<NotaIngreso>()
                .Property(e => e.MotivoIngreso)
                .IsUnicode(false);

            modelBuilder.Entity<NotaIngreso>()
                .Property(e => e.ResumenInterrogatorio)
                .IsUnicode(false);

            modelBuilder.Entity<NotaIngreso>()
                .Property(e => e.ResLabGabinete)
                .IsUnicode(false);

            modelBuilder.Entity<NotaIngreso>()
                .Property(e => e.DiagnosticoDesc)
                .IsUnicode(false);

            modelBuilder.Entity<NotaIngreso>()
                .Property(e => e.PlanManejo)
                .IsUnicode(false);

            modelBuilder.Entity<NotaIngreso>()
                .Property(e => e.Pronostico)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.ResumenClinico)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.ResEstudiosDiagnosticos)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.DiagnosticoIngresoDesc)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.PlanTerCirujiaPlaneada)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.RiesgoQuirurgico)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.CirujiaRealizada)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.DiagnosticoEgresoDesc)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.TipoAnestecia)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.DescTecnicaQuirurgicaTerapeutica)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.Hallazgos)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.Incidentes)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.Sangrado)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.EstadoActualProblemasClinicos)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.PlanManejo)
                .IsUnicode(false);

            modelBuilder.Entity<NotaOperatoria>()
                .Property(e => e.Pronostico)
                .IsUnicode(false);

            modelBuilder.Entity<NotaPreoperatoria>()
                .Property(e => e.DiagnosticoDesc)
                .IsUnicode(false);

            modelBuilder.Entity<NotaPreoperatoria>()
                .Property(e => e.PlanesCuidados)
                .IsUnicode(false);

            modelBuilder.Entity<NotaPreoperatoria>()
                .Property(e => e.PlanQuirurgico)
                .IsUnicode(false);

            modelBuilder.Entity<NotaPreoperatoria>()
                .Property(e => e.FactoresRiesgo)
                .IsUnicode(false);

            modelBuilder.Entity<NotaPreoperatoria>()
                .Property(e => e.RiesgoQuirurgico)
                .IsUnicode(false);

            modelBuilder.Entity<NotaPreoperatoria>()
                .Property(e => e.Pronostico)
                .IsUnicode(false);

            modelBuilder.Entity<NUEVOLEON>()
                .Property(e => e.clave_col)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NUEVOLEON>()
                .Property(e => e.clave_unidad)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Orden_Int>()
                .Property(e => e.estudios)
                .IsUnicode(false);

            modelBuilder.Entity<Orden_Int>()
                .Property(e => e.motivo)
                .IsUnicode(false);

            modelBuilder.Entity<Orden_Int>()
                .Property(e => e.indicaciones)
                .IsUnicode(false);

            modelBuilder.Entity<Orden_Int>()
                .Property(e => e.comentarios)
                .IsUnicode(false);

            modelBuilder.Entity<Orden_Int>()
                .Property(e => e.resumen_medico)
                .IsUnicode(false);

            modelBuilder.Entity<Orden_Int_Enviar>()
                .Property(e => e.ubicacion)
                .IsFixedLength();

            modelBuilder.Entity<PARENTESCO>()
                .HasMany(e => e.CITAS)
                .WithOptional(e => e.PARENTESCO)
                .WillCascadeOnDelete();

            modelBuilder.Entity<PARENTESCO>()
                .HasMany(e => e.DHABIENTES)
                .WithOptional(e => e.PARENTESCO)
                .WillCascadeOnDelete();

            modelBuilder.Entity<prov_subrrogados>()
                .Property(e => e.observaciones)
                .IsUnicode(false);

            modelBuilder.Entity<PruebaAntigenos>()
                .Property(e => e.nota)
                .IsUnicode(false);

            modelBuilder.Entity<PruebaAntigenos>()
                .Property(e => e.usuario)
                .IsUnicode(false);

            modelBuilder.Entity<PruebaAntigenos>()
                .Property(e => e.numexp)
                .IsUnicode(false);

            modelBuilder.Entity<PUESTOS>()
                .HasMany(e => e.AFILIADOS)
                .WithOptional(e => e.PUESTOS)
                .WillCascadeOnDelete();

            modelBuilder.Entity<RECETA_EXP>()
                .Property(e => e.EXPEDIENTE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP>()
                .Property(e => e.MEDICO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP>()
                .Property(e => e.TURNO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP>()
                .Property(e => e.REGXDIA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP>()
                .Property(e => e.estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP>()
                .Property(e => e.dir_ip)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP>()
                .Property(e => e.imp_rcta)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP>()
                .Property(e => e.tipo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP>()
                .Property(e => e.meses)
                .HasPrecision(18, 0);

            modelBuilder.Entity<RECETA_EXP>()
                .Property(e => e.unidad_medica)
                .HasPrecision(18, 0);

            modelBuilder.Entity<receta_exp_crn_reactivar>()
                .Property(e => e.medico_crea)
                .IsFixedLength();

            modelBuilder.Entity<receta_exp_crn_reactivar>()
                .Property(e => e.expediente)
                .IsFixedLength();

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.POS_REP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.DESC_DX)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R01)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R14)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R59)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R1014)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R1519)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R2024)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R2544)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R4549)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R5059)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R6064)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.R65)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.IGN)
                .HasPrecision(18, 0);

            modelBuilder.Entity<REP_EPI>()
                .Property(e => e.TOTAL)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ReporteActividadesSemanal>()
                .Property(e => e.UsuarioRealiza)
                .IsUnicode(false);

            modelBuilder.Entity<serv_rx_n1>()
                .HasMany(e => e.serv_rx_n2)
                .WithRequired(e => e.serv_rx_n1)
                .HasForeignKey(e => e.tipo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SolicitudLab>()
                .Property(e => e.Paciente_Dependencia_Id)
                .IsFixedLength();

            modelBuilder.Entity<SolicitudLab>()
                .Property(e => e.Paciente_Dependencia_Descripcion)
                .IsFixedLength();

            modelBuilder.Entity<TBLGRUPOS>()
                .HasMany(e => e.ARTICULOS)
                .WithOptional(e => e.TBLGRUPOS)
                .HasForeignKey(e => e.cve_grupo)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TBLGRUPOS>()
                .HasMany(e => e.TBLSALES)
                .WithRequired(e => e.TBLGRUPOS)
                .HasForeignKey(e => e.grupo);

            modelBuilder.Entity<TblNotaFarmaco>()
                .Property(e => e.NotaFarmaco)
                .IsUnicode(false);

            modelBuilder.Entity<TblNotaFarmaco>()
                .Property(e => e.MedicoRealizo)
                .IsUnicode(false);

            modelBuilder.Entity<TblNotaFarmaco>()
                .Property(e => e.IP_Realizo)
                .IsUnicode(false);

            modelBuilder.Entity<TblNotaFarmaco>()
                .Property(e => e.NUMEMP)
                .IsUnicode(false);

            modelBuilder.Entity<tema_enca>()
                .Property(e => e.descripcion)
                .IsFixedLength();

            modelBuilder.Entity<Tickets>()
                .Property(e => e.usuario_realiza)
                .IsUnicode(false);

            modelBuilder.Entity<Tickets>()
                .Property(e => e.descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Tickets>()
                .Property(e => e.usuario_encargado)
                .IsUnicode(false);

            modelBuilder.Entity<TipCita>()
                .Property(e => e.especialidad)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TipCita>()
                .Property(e => e.clave)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TipCita>()
                .Property(e => e.descripcion)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<TIPOSCONSULTA>()
                .HasMany(e => e.CITAS)
                .WithOptional(e => e.TIPOSCONSULTA)
                .HasForeignKey(e => e.Tipo)
                .WillCascadeOnDelete();

            modelBuilder.Entity<TIPOSCONSULTA>()
                .HasMany(e => e.DetalleHorarioMedicos)
                .WithRequired(e => e.TIPOSCONSULTA)
                .HasForeignKey(e => e.TiposConsultaId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<afiliado_movs>()
                .Property(e => e.emp_realizo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<afiliado_movs>()
                .Property(e => e.ip_realizo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<afiliado_movs>()
                .Property(e => e.numemp)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<afiliado_movs>()
                .Property(e => e.num_anterior)
                .IsFixedLength();

            modelBuilder.Entity<citas_sust>()
                .Property(e => e.medico_titular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<citas_sust>()
                .Property(e => e.grupo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<citas_sust>()
                .Property(e => e.medico_sust)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<citas_sust>()
                .Property(e => e.id)
                .HasPrecision(18, 0);

            modelBuilder.Entity<citas_sust>()
                .Property(e => e.emp_realizo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<citas_sust>()
                .Property(e => e.agenda_remplazo)
                .IsFixedLength();

            modelBuilder.Entity<Directorio>()
                .Property(e => e.Nombre)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.Usuario)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.Departamento)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.Ubicacion)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.Puesto)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.NombreEquipo)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.IP)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.Tipo)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.MarcaCPU)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.ModeloCPU)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.SerieCPU)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.MarcaMonitor)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.ModeloMonitor)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.SerieMonitor)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.Office)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.ModeloImpresora)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.SerieImpresora)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.ConexionImpresora)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.ModeloTelefono)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.SerieTelefono)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.NumDirecto)
                .IsUnicode(false);

            modelBuilder.Entity<Directorio>()
                .Property(e => e.Display)
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.EXPEDIENTE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.SEXO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MEDICO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MEDICO_REALIZA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FOLIO)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.EDAD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.PESO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.TALLA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.SC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.ventana)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dimvd)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dimsd)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dimss)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dimcvid)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dimcvis)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dimpp4d)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dimpp4s)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dimrao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dimapao)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dimauiz)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.motivo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV01)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV02)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV03)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV04)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV05)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV06)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV07)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV08)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV09)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV10)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV11)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV12)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV13)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV14)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV15)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV16)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV17)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV18)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV19)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV20)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV21)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FV22)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FVVTD4)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FVVTD2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FVVL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FVVTS4)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FVVTS2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FVAF)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FVFE4)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FVFE2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MVMI)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MVAO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MVPU)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MVTR)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MIVMM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MIGRMX)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MIGRMD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MIPIE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MIPIA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MIAVM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MITIH)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MITID)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MIINS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MIVJIM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.PUVPM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.PUGRMX)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.PUGRMD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.PUINS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.PUTIPM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.PUPDAP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.AOVAM)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.AOGMX)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.AOGMD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.AOINS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.AOPINS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.AOAVA)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.AOITV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.TRVTMX)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.TRGRMX)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.TRGRMD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.TRINS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.TRVJIT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.TRPAPU)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MSHU)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MSTRO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MSAVAI)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.MSAVAD)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.SMENG)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.SMCAL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.SMPLI)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.SMAPS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.FVGAC)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.DXL1)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.DXL2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.DXL3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.DXL4)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.DXL5)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.DXL6)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.DXL7)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.DXL8)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ecocardio>()
                .Property(e => e.dx2)
                .IsUnicode(false);

            modelBuilder.Entity<ESPECIALIDADES>()
                .Property(e => e.FMSR)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ESPECIALIDADES>()
                .Property(e => e.FMSB)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ESPECIALIDADES>()
                .Property(e => e.FVSR)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ESPECIALIDADES>()
                .Property(e => e.FVSB)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ESPECIALIDADES>()
                .Property(e => e.TIPO)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ESPECIALIDADES>()
                .Property(e => e.titulo)
                .IsFixedLength();

            modelBuilder.Entity<ESPECIALIDADES>()
                .Property(e => e.estado)
                .IsFixedLength();

            modelBuilder.Entity<EXP_HU>()
                .Property(e => e.PAC_ID)
                .HasPrecision(18, 0);

            modelBuilder.Entity<EXP_HU>()
                .Property(e => e.EXPEDIENTE)
                .IsFixedLength();

            modelBuilder.Entity<EXP_HU>()
                .Property(e => e.NUMAFIL)
                .IsFixedLength();

            modelBuilder.Entity<expediente>()
                .Property(e => e.pesomg)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.tallac)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.ta2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.turno)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.referido)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.referido_urg)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.incapacidad)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.laboratorio)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.receta)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.edd_anio)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.edd_mes)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.MED_TMP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.ref_stat)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.fresp)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.fcard)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.rxd)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.dstx)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.consultadistancia)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.ubicacion_realiza)
                .HasPrecision(18, 0);

            modelBuilder.Entity<expediente>()
                .Property(e => e.ip_realiza)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.referido2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.referido_urg2)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.ref_exp2)
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.referido3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.referido_urg3)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.ref_exp3)
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.proxima_cita)
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.per_cefalico)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.masa_muscular)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.masa_grasa)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<expediente>()
                .Property(e => e.porcentaje_grasa)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<farxexp>()
                .Property(e => e.expediente)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<farxexp>()
                .Property(e => e.tot_rctas)
                .HasPrecision(18, 0);

            modelBuilder.Entity<farxexp>()
                .Property(e => e.anio)
                .HasPrecision(18, 0);

            modelBuilder.Entity<farxexp>()
                .Property(e => e.mes)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Lab_detalle>()
                .Property(e => e.id_indicaciones)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Lab_exp>()
                .Property(e => e.medico_crea)
                .IsFixedLength();

            modelBuilder.Entity<Lab_exp>()
                .Property(e => e.expediente)
                .IsFixedLength();

            modelBuilder.Entity<Lab_exp>()
                .Property(e => e.folio_lab2)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Lab_exp>()
                .Property(e => e.emp_muestra)
                .IsFixedLength();

            modelBuilder.Entity<Lab_exp>()
                .Property(e => e.folio_consec)
                .HasPrecision(18, 0);

            modelBuilder.Entity<labinstrucciones>()
                .Property(e => e.id)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Query>()
                .Property(e => e.Descripcion)
                .IsUnicode(false);

            modelBuilder.Entity<Query>()
                .Property(e => e.Departamento)
                .IsUnicode(false);

            modelBuilder.Entity<Query>()
                .Property(e => e.Ubicacion)
                .IsUnicode(false);

            modelBuilder.Entity<receta_detalle_crn>()
                .Property(e => e.codigo)
                .IsFixedLength();

            modelBuilder.Entity<receta_detalle_crn>()
                .Property(e => e.dosis)
                .IsFixedLength();

            modelBuilder.Entity<receta_detalle_crn>()
                .Property(e => e.indicaciones)
                .IsFixedLength();

            modelBuilder.Entity<receta_detalle_crn>()
                .Property(e => e.farext)
                .IsFixedLength();

            modelBuilder.Entity<receta_exp_crn>()
                .Property(e => e.medico_crea)
                .IsFixedLength();

            modelBuilder.Entity<receta_exp_crn>()
                .Property(e => e.medico_ref)
                .IsFixedLength();

            modelBuilder.Entity<receta_exp_crn>()
                .Property(e => e.expediente)
                .IsFixedLength();

            modelBuilder.Entity<receta_exp_crn>()
                .Property(e => e.px_special)
                .IsFixedLength();

            modelBuilder.Entity<receta_exp_crn>()
                .Property(e => e.proxima_cita)
                .IsUnicode(false);

            modelBuilder.Entity<Resultados>()
                .Property(e => e.OCUPACION)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Resultados>()
                .Property(e => e.DESC_BAJA)
                .IsUnicode(false);

            modelBuilder.Entity<Resultados>()
                .Property(e => e.emp_realizo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Resultados>()
                .Property(e => e.RHUANL)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Resultados>()
                .Property(e => e.num_contrato)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<sdp_tramites>()
                .Property(e => e.expediente)
                .IsFixedLength();

            modelBuilder.Entity<sdp_tramites>()
                .Property(e => e.id_tramite)
                .HasPrecision(18, 0);

            modelBuilder.Entity<sdp_tramites>()
                .Property(e => e.anio)
                .HasPrecision(18, 0);

            modelBuilder.Entity<sdp_tramites>()
                .Property(e => e.id_consec)
                .HasPrecision(18, 0);

            modelBuilder.Entity<sdp_tramites>()
                .Property(e => e.realiza)
                .IsFixedLength();

            modelBuilder.Entity<sdp_tramites>()
                .Property(e => e.conyugue)
                .IsFixedLength();

            modelBuilder.Entity<sdp_tramites>()
                .Property(e => e.estudio)
                .IsFixedLength();

            modelBuilder.Entity<sdp_tramites>()
                .Property(e => e.medico)
                .IsFixedLength();

            modelBuilder.Entity<SOLICITUD_LAB>()
                .Property(e => e.FOLIO)
                .HasPrecision(18, 0);

            modelBuilder.Entity<spre_catalogo>()
                .Property(e => e.desc_pre_cat)
                .IsFixedLength();

            modelBuilder.Entity<spre_catalogo>()
                .Property(e => e.tipo_pre_cat)
                .HasPrecision(18, 0);

            modelBuilder.Entity<spre_exp>()
                .Property(e => e.medico_crea)
                .IsFixedLength();

            modelBuilder.Entity<spre_exp>()
                .Property(e => e.expediente)
                .IsFixedLength();

            modelBuilder.Entity<spre_exp_detalle>()
                .Property(e => e.folio_pre)
                .HasPrecision(18, 0);

            modelBuilder.Entity<spre_exp_detalle>()
                .Property(e => e.id_pre_cat)
                .HasPrecision(18, 0);

            modelBuilder.Entity<spre_exp_detalle>()
                .Property(e => e.indicaciones)
                .IsFixedLength();

            modelBuilder.Entity<spre_exp_detalle>()
                .Property(e => e.id)
                .HasPrecision(18, 0);

            modelBuilder.Entity<spre_permiso>()
                .Property(e => e.id_pre_cat)
                .HasPrecision(18, 0);

            modelBuilder.Entity<spre_permiso>()
                .Property(e => e.id_pre_tab)
                .HasPrecision(18, 0);

            modelBuilder.Entity<spre_permiso>()
                .Property(e => e.id_pre_reg)
                .HasPrecision(18, 0);

            modelBuilder.Entity<tema_detalle>()
                .Property(e => e.imagen)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<InventarioFarmacia>()
                .Property(e => e.Clave)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<InventarioFarmacia>()
                .Property(e => e.Clave_Nivel)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<InventarioFarmaciaNiveles>()
                .Property(e => e.Clave)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<InventarioFarmaciaNiveles>()
                .Property(e => e.Clave_Nivel)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Receta_Detalle_2>()
                .Property(e => e.Codigo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Receta_Detalle_2>()
                .Property(e => e.Dosis)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Receta_Detalle_2>()
                .Property(e => e.Indicaciones)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Receta_Detalle_2>()
                .Property(e => e.Estatus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Receta_Detalle_2>()
                .Property(e => e.Subrogar)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Receta_Detalle_2>()
                .Property(e => e.dep_sub)
                .IsUnicode(false);

            modelBuilder.Entity<Receta_Detalle_2>()
                .Property(e => e.no_surtida_razon)
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP_2>()
                .Property(e => e.Turno)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP_2>()
                .Property(e => e.Regxdia)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP_2>()
                .Property(e => e.Estado)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP_2>()
                .Property(e => e.Dir_Ip)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP_2>()
                .Property(e => e.imp_rcta)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP_2>()
                .Property(e => e.Tipo)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<RECETA_EXP_2>()
                .Property(e => e.Meses)
                .HasPrecision(18, 0);

            modelBuilder.Entity<RH_UANL>()
                .Property(e => e.NO_PERSONAL)
                .HasPrecision(6, 0);

            modelBuilder.Entity<RH_UANL>()
                .Property(e => e.NO_PARAMETRO)
                .HasPrecision(3, 0);

            modelBuilder.Entity<RH_UANL>()
                .Property(e => e.HORAS_TRABAJO)
                .HasPrecision(2, 0);

            modelBuilder.Entity<RH_UANL>()
                .Property(e => e.HORAS_CLASE)
                .HasPrecision(2, 0);

            modelBuilder.Entity<sales>()
                .Property(e => e.Clave_Sal)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Sustancia>()
                .Property(e => e.clave_presentacion)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Sustancia>()
                .Property(e => e.Clave)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Sustancia>()
                .Property(e => e.Consultorio)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Usu_User)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Usu_Pass)
                .IsUnicode(false);

            modelBuilder.Entity<Usuario>()
                .Property(e => e.Usu_Nombre)
                .IsUnicode(false);
        }
    }
}
