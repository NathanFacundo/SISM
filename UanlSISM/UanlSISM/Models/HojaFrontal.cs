using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using WebGrease.Css.Ast.Selectors;
using System.Data;
using Microsoft.AspNet.Identity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;

namespace UanlSISM.Models
{
    public class HojaFrontal
    {
        public string NOMBRE { get; set; }
        public string COLONIA { get; set; }
        public string NUMEMP { get; set; }
        public string EXPE { get; set; }
        public string RFC { get; set; }
        public string DOMICILIO { get; set; }
        public string FINGRESO { get; set; }
        public string DEPENDENCIA { get; set; }
        public string TIPOAFIL { get; set; }

        public string NOTA_FECHA { get; set; }
        public string NOTA { get; set; }

        List<hfParientes> Item1 = new List<hfParientes>();

        public List<hfParientes> Parientes
        {
            get { return Item1; }
            set { Item1 = value; }
        }

        PopHp Itemp = new PopHp();

        public PopHp PopHp
        {
            get { return Itemp; }
            set { Itemp = value; }
        }


        List<historia> Item2 = new List<historia>();
        public List<historia> historia
        {
            get { return Item2; }
            set { Item2 = value; }
        }
        List<Medicamentos> Item3 = new List<Medicamentos>();
        public List<Medicamentos> Medicamentos
        {
            get { return Item3; }
            set { Item3 = value; }
        }


        List<Prestaciones> Item4 = new List<Prestaciones>();
        public List<Prestaciones> Prestaciones
        {
            get { return Item4; }
            set { Item4 = value; }
        }
    }

    public class Calendario
    {
        public string title { get; set; }
        public string url { get; set; }
        public string start { get; set; }
    }

    public class historia
    {
        public string ing_fecha { get; set; }
        public string ing_resum_interrog { get; set; }
        public string egr_dias { get; set; }
        public string egr_fecha { get; set; }
        public string egr_resumen_evo { get; set; }

    }

    public class Medicamentos
    {
        public string Medicamento { get; set; }
        public string Fecha { get; set; }

        public string ESPECIALIDAD { get; set; }
        public string CantSurtida { get; set; }
        public string Subrogar { get; set; }

    }

    public class Prestaciones
    {
        public string fecha { get; set; }
        public string servicio { get; set; }
        public string proveedor { get; set; }
        public string nota { get; set; }


    }
    public class hfParientes
    {
        public string NUMEMP { get; set; }
        public string PARIENTE { get; set; }
        public string NOMBRES { get; set; }
        public string APATERNO { get; set; }
        public string AMATERNO { get; set; }
        public string FNAC { get; set; }
        public string SEXO { get; set; }
        public string EDAD { get; set; }
        public string ANIOS { get; set; }
        public string MESES { get; set; }

    }

    public class PopHp
    {
        public string FechaIngreso { get; set; }
        public string EgresoFecha { get; set; }
        public string Motivo { get; set; }
        public string resumen_evo { get; set; }
        public string planTratamiento { get; set; }
        public string citaAbierta { get; set; }
        public string FactorRiesgo { get; set; }
        public string Lab { get; set; }
        public string Dias { get; set; }

        public string CitaEsp { get; set; }
        public string pronostico { get; set; }
        public string dx { get; set; }
        public string ing_explora_fis { get; set; }
        public string ing_exa_gab { get; set; }
        public string ing_exa_lab { get; set; }
        public string ing_tx_probable { get; set; }
        public string ing_indicaciones { get; set; }
        public string medico { get; set; }
        public string ing_peso { get; set; }
        public string ing_talla { get; set; }
        public string ing_frec_card { get; set; }
        public string ing_pres_art1 { get; set; }

        public string recNombre { get; set; }
        public string refNombre { get; set; }

    }


    public class Counsultahu
    {
        public string NUMEMP { get; set; }

        public string NOMBRES { get; set; }
        public string APATERNO { get; set; }
        public string AMATERNO { get; set; }
        public string SEXO { get; set; }
        public string FNAC { get; set; }
        public string FECALTA { get; set; }

        public string FBAJA { get; set; }
        public string EDAD { get; set; }

    }

    public class DalHojaFrontal
    {
        public bool Llamar(string v_num, string v_med, string ip_realiza)
        {
            SqlConnection cnn = null;
            SqlConnection cnn2 = null;

            List<Counsultahu> Lista = new List<Counsultahu>();

            try
            {
                var context = new SMDEVEntities14();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = " "; 

                cmdTxt = "select recepcion_mt, num_cons from [INVENTARIO].[dbo].[INVENTARIO] where (DIRECCION_IP ='" + ip_realiza + "')";

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                string estado = "";
                string num_cons = "";

                while (lector.Read())
                { 
                    estado = VerifStringSql(lector, "recepcion_mt");
                    num_cons = VerifStringSql(lector, "num_cons");
                }

                lector.Close();

                var context1 = new SMDEVEntities16();
                var connection1 = context1.Database.Connection;
                string conStr1 = connection1.ConnectionString;

                cnn2 = new SqlConnection(conStr1);

                cnn2.Open();


                cmdTxt = "  update CITAS SET hr_llamado = CONVERT(VARCHAR(5),getdate(),108), recepcion_mt='"+estado+ "'  where regxdia='" + v_num + "' and  CITAS.MEDICO = '" + v_med + "' AND CITAS.FECHA = CAST(CAST(GETDATE() AS DATE) AS SMALLDATETIME); update MEDICOS SET consultorio_mt='" + num_cons + "'  where   Numero='" + v_med + "'";
                //cmdTxt = "  update MEDICOS SET consultorio_mt='"+ num_cons + "'  where   Numero='" + v_med + "'";
                cmm = null;
                cmm = new SqlCommand(cmdTxt, cnn2);
                cmm.ExecuteNonQuery();



                return true; 
            }
            catch (Exception)
            {

                throw;

            }
            finally
            {
                cnn.Close();
                cnn2.Close();


            }
        }

        public List<Counsultahu> BuscarHU(string v_NUMEMP, string v_nombres, string v_apellido, string v_apellidoM)
        {
            SqlConnection cnn = null;

            List<Counsultahu> Lista = new List<Counsultahu>();

            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = "SELECT NUMEMP , NOMBRES,APATERNO ,AMATERNO,SEXO,convert(char, FNAC, 103) FNAC,floor((cast(datediff(day, FNAC, getdate()) as float)/365-(floor(cast(datediff(day, FNAC, getdate()) as float)/365)))*12) AS 'meses', floor(cast(datediff(day, FNAC, getdate()) as float)/365) AS 'años' ,convert(char, FECALTA, 103) FECALTA,DHA_BAJA.DESCRIPCION  FBAJA  FROM  DHABIENTES  left join  DHA_BAJA on  DHABIENTES.BAJA= DHA_BAJA.CLAVE";
                cmdTxt += "   WHERE FVIGENCIA != '1900-01-01T00:00:00' AND NUMAFIL IS NOT NULL AND IsNull(BAJA,'0') <> '01' AND IsNull(BAJA,'0') <> '03'  AND   (NUMEMP like '" + v_NUMEMP + "%' or  ''='" + v_NUMEMP + "'  ) and ( NOMBRES like '%" + v_nombres.ToUpper() + "%' or ''='" + v_nombres.ToUpper() + "'  ) and ( APATERNO like '%" + v_apellido.ToUpper() + "%'  or ''='" + v_apellido.ToUpper() + "' ) and  (AMATERNO like '%" + v_apellidoM.ToUpper() + "%' or ''='" + v_apellidoM.ToUpper() + "'  )  ";

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Counsultahu entidad = new Counsultahu();

                while (lector.Read())
                {
                    var v_nac = "";
                    if (VerifStringSql(lector, "FNAC") == "")
                    {
                        v_nac = "1900";

                        //var v_mes = VerifStringSql(lector, "FNAC").Substring(3, 2);
                        //var v_anio = VerifStringSql(lector, "FNAC").Substring(6, 4);

                        //if (v_anio != "1900")
                        //{
                        //    DateTime now = DateTime.Now;

                        //    string fechaactual = now.Date.ToString("dd/MM/yyyy");
                        //    v_nac = (Convert.ToInt32(now.Date.ToString("dd/MM/yyyy").Substring(6, 4)) - Convert.ToInt32(v_anio)).ToString() + " - " + (Convert.ToInt32(now.Date.ToString("dd/MM/yyyy").Substring(3, 2)) - Convert.ToInt32(v_mes)).ToString();

                        //}

                    }
                    else
                    {
                        v_nac = VerifStringSql(lector, "FNAC");
                    }

                    /*var nacimiento = "";
                    if (VerifStringSql(lector, "FNAC") != "")
                    {
                        var today = DateTime.Today;
                        var fnac = VerifStringSql(lector, "FNAC");
                        var fechaNac = Convert.ToDateTime(fnac);
                        DateTime fecha_nac = (DateTime)fechaNac;

                        var edad = today.Year - fecha_nac.Year;
                        var meses1 = today.Month - fecha_nac.Month;
                        var meses = 0;
                        if (meses1 >= 0)
                        {
                            meses = (today.Month - fecha_nac.Month);
                        }
                        else
                        {
                            meses = (today.Month - fecha_nac.Month) + 12;
                        }

                        //var meses = (today.Month - fecha_nac.Month) + 12;
                        var edadString = edad.ToString();
                        var mesesString = meses.ToString();

                        nacimiento = edadString + " - " + mesesString
                        //System.Diagnostics.Debug.WriteLine(v_nac);
                    }*/

                    entidad = new Counsultahu();
                    entidad.FNAC = v_nac;
                    entidad.EDAD = VerifStringSql(lector, "años") +" años y "+ VerifStringSql(lector, "meses")+" meses";
                    entidad.AMATERNO = VerifStringSql(lector, "AMATERNO");
                    entidad.APATERNO = VerifStringSql(lector, "APATERNO");
                    entidad.FBAJA = VerifStringSql(lector, "FBAJA");
                    entidad.FECALTA = VerifStringSql(lector, "FECALTA");
                    entidad.NOMBRES = VerifStringSql(lector, "NOMBRES");
                    entidad.NUMEMP = VerifStringSql(lector, "NUMEMP");
                    entidad.SEXO = VerifStringSql(lector, "SEXO");

                    Lista.Add(entidad);


                }

                lector.Close();

                return Lista;
            }
            catch (Exception)
            {

                throw;

            }
            finally
            {
                cnn.Close();


            }
        }

        public List<Counsultahu> BuscarHUHistorial(string v_NUMEMP, string v_nombres, string v_apellido, string v_apellidoM, string v_medico)
        {
            SqlConnection cnn = null;

            List<Counsultahu> Lista = new List<Counsultahu>();

            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = "SELECT   distinct hab.NUMEMP , NOMBRES,APATERNO ,AMATERNO,SEXO,convert(char, FNAC, 103) FNAC,floor((cast(datediff(day, FNAC, getdate()) as float)/365-(floor(cast(datediff(day, FNAC, getdate()) as float)/365)))*12) AS 'meses', floor(cast(datediff(day, FNAC, getdate()) as float)/365) AS 'años', exp.hora_termino FECALTA,exp.hora_termino FBAJA  FROM  DHABIENTES hab ,  expediente exp   ";
                cmdTxt += "   WHERE hab.FVIGENCIA != '1900-01-01T00:00:00' and NUMAFIL IS NOT NULL AND IsNull(BAJA,'0') <> '01' AND IsNull(BAJA,'0') <> '03'  and   convert(char, exp.hora_termino, 103) = convert(char, GETDATE(), 103)  AND      hab.NUMEMP = exp.numemp  and exp.medico ='" + v_medico + "' ";

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Counsultahu entidad = new Counsultahu();

                while (lector.Read())
                {
                    var v_nac = "";
                    if (VerifStringSql(lector, "FNAC") == "")
                    {

                        v_nac = "1900";
                        //var v_mes = VerifStringSql(lector, "FNAC").Substring(3, 2);
                        //var v_anio = VerifStringSql(lector, "FNAC").Substring(6, 4);

                        //if (v_anio != "1900")
                        //{
                        //    DateTime now = DateTime.Now;

                        //    string fechaactual = now.Date.ToString("dd/MM/yyyy");
                        //    v_nac = (Convert.ToInt32(now.Date.ToString("dd/MM/yyyy").Substring(6, 4)) - Convert.ToInt32(v_anio)).ToString() + " - " + (Convert.ToInt32(now.Date.ToString("dd/MM/yyyy").Substring(3, 2)) - Convert.ToInt32(v_mes)).ToString();

                        //}

                    }
                    else
                    {
                        v_nac = VerifStringSql(lector, "FNAC");
                    }


                    /*var nacimiento = "";
                    if (VerifStringSql(lector, "FNAC") != "")
                    {
                        var today = DateTime.Today;
                        var fnac = VerifStringSql(lector, "FNAC");
                        var fechaNac = Convert.ToDateTime(fnac);
                        DateTime fecha_nac = (DateTime)fechaNac;

                        var edad = today.Year - fecha_nac.Year;
                        var meses1 = today.Month - fecha_nac.Month;
                        var meses = 0;
                        if (meses1 >= 0)
                        {
                            meses = (today.Month - fecha_nac.Month);
                        }
                        else
                        {
                            meses = (today.Month - fecha_nac.Month) + 12;
                        }

                        //var meses = (today.Month - fecha_nac.Month) + 12;
                        var edadString = edad.ToString();
                        var mesesString = meses.ToString();

                        nacimiento = edadString + " - " + mesesString;

                        //System.Diagnostics.Debug.WriteLine(v_nac);
                    }*/

                    //System.Diagnostics.Debug.WriteLine(v_nac);

                    entidad = new Counsultahu();
                    entidad.FNAC = v_nac;
                    entidad.EDAD = VerifStringSql(lector, "años") + "años y" + VerifStringSql(lector, "meses")+"meses";
                    entidad.AMATERNO = VerifStringSql(lector, "AMATERNO");
                    entidad.APATERNO = VerifStringSql(lector, "APATERNO");
                    entidad.FBAJA = VerifStringSql(lector, "FBAJA");
                    entidad.FECALTA = VerifStringSql(lector, "FECALTA");
                    entidad.NOMBRES = VerifStringSql(lector, "NOMBRES");
                    entidad.NUMEMP = VerifStringSql(lector, "NUMEMP");
                    entidad.SEXO = VerifStringSql(lector, "SEXO");

                    Lista.Add(entidad);


                }

                lector.Close();

                return Lista;
            }
            catch (Exception)
            {

                throw;

            }
            finally
            {
                cnn.Close();


            }
        }

        public HojaFrontal BuscarTitular(string v_NUMEMP)

        {
            SqlConnection cnn = null;
            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                if (string.IsNullOrEmpty(v_NUMEMP))
                {
                    v_NUMEMP = "062759";
                }
                string cmdTxt = "  SELECT top 1 cc.DESCRIPCION COLONIA,   CONCAT(DH.APATERNO,' ',DH.AMATERNO ,' ',DH.NOMBRES )   NOMBRE, afd.NUMEMP, RFC, DOMICILIO,  convert(char, FINGRESO, 103)  FINGRESO, dep.DESCR DEPENDENCIA, afc.DESCR TIPOAFIL ";
                cmdTxt += "    FROM DHABIENTES   DH    left join AFILIADOS afd on afd.NUMEMP = DH.NUMAFIL and DH.FVIGENCIA != '1900-01-01T00:00:00'  left join  AFILIACION afc on  afc.TIPOAFIL = afd.TIPOAFIL   left join  DEPENDENCIAS dep  on dep.CVEDEP = afd.CVEDEP   left join   col_afil cc  on cc.clave = afd.colonia    where dh.NUMEMP='" + v_NUMEMP + "'  ";

                //cmdTxt = "  SELECT top 1 cc.DESCRIPCION COLONIA, afd.NUMEMP, RFC, DOMICILIO,  convert(char, FINGRESO, 103)  FINGRESO, dep.DESCR DEPENDENCIA, afc.DESCR TIPOAFIL ";
                //cmdTxt += "   FROM AFILIADOS afd, AFILIACION afc, DEPENDENCIAS dep  ,  col_afil cc ,DHABIENTES   DH";
                //cmdTxt += "   where DH.FVIGENCIA != '1900-01-01T00:00:00' and cc.clave = afd.colonia  and afc.TIPOAFIL = afd.TIPOAFIL and dep.CVEDEP = afd.CVEDEP and  afd.NUMEMP = DH.NUMAFIL and dh.NUMEMP='" + v_NUMEMP + "' ";


                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                HojaFrontal entidad = new HojaFrontal();

                while (lector.Read())
                {
                    entidad = new HojaFrontal();
                    entidad.COLONIA = VerifStringSql(lector, "COLONIA");
                    entidad.DEPENDENCIA = VerifStringSql(lector, "DEPENDENCIA");
                    entidad.DOMICILIO = VerifStringSql(lector, "DOMICILIO");
                    entidad.FINGRESO = VerifStringSql(lector, "FINGRESO");
                    entidad.NUMEMP = VerifStringSql(lector, "NUMEMP");
                    entidad.EXPE = v_NUMEMP;
                    entidad.RFC = VerifStringSql(lector, "RFC");
                    entidad.TIPOAFIL = VerifStringSql(lector, "TIPOAFIL");
                    entidad.NOMBRE = VerifStringSql(lector, "NOMBRE");


                }

                lector.Close();

                if (!string.IsNullOrEmpty(v_NUMEMP))
                {
                    // familiares

                    cmdTxt = " SELECT NUMEMP, PA.DESCR   PARIENTE  ,NOMBRES ,APATERNO ,AMATERNO , convert(char, FNAC, 103)  FNAC ,SEXO,convert(char, FNAC, 103) FNAC,cast(floor((cast(datediff(day, FNAC, getdate()) as float)/365-(floor(cast(datediff(day, FNAC, getdate()) as float)/365)))*12)as varchar) AS 'meses',cast(floor(cast(datediff(day, FNAC, getdate()) as float)/365)as varchar) AS 'anios'";
                    cmdTxt += " FROM DHABIENTES   DH,  PARENTESCO PA WHERE  DH.FVIGENCIA != '1900-01-01T00:00:00' and dh.PARIENTE=PA.PARIENTE and  DH.NUMEMP='" + v_NUMEMP + "' ";
                    // cmdTxt += " FROM DHABIENTES   DH,  PARENTESCO PA WHERE DH.FVIGENCIA != '1900-01-01T00:00:00' AND  dh.PARIENTE=PA.PARIENTE and DH.NUMAFIL = '" + entidad.NUMEMP + "' ";

                    cmm = null;
                    cmm = new SqlCommand(cmdTxt, cnn);

                    lector = null;
                    lector = cmm.ExecuteReader();

                    //System.Diagnostics.Debug.WriteLine(VerifStringSql(lector, "FNAC"));

                    

                    //System.Diagnostics.Debug.WriteLine(nacimiento);


                    List<hfParientes> hfp = new List<hfParientes>();
                    while (lector.Read())
                    {
                        hfParientes entidad2 = new hfParientes();


                        entidad2.AMATERNO = VerifStringSql(lector, "AMATERNO");
                        entidad2.APATERNO = VerifStringSql(lector, "APATERNO");
                        entidad2.FNAC = VerifStringSql(lector, "FNAC");
                        entidad2.NOMBRES = VerifStringSql(lector, "NOMBRES");
                        entidad2.NUMEMP = VerifStringSql(lector, "NUMEMP");

                        entidad2.PARIENTE = VerifStringSql(lector, "PARIENTE");
                        entidad2.SEXO = VerifStringSql(lector, "SEXO");
                        entidad2.ANIOS = VerifStringSql(lector, "ANIOS");
                        entidad2.MESES = VerifStringSql(lector, "MESES");
                        //entidad2.EDAD = nacimiento;
                        hfp.Add(entidad2);
                    }

                    lector.Close();

                    entidad.Parientes = hfp;




                    // historia

                    cmdTxt = " select NUMEMP,convert(char, ing_fecha, 103)  ing_fecha,ing_resum_interrog, egr_dias, convert(char, egr_fecha, 103) egr_fecha, egr_resumen_evo   ";
                    cmdTxt += " from  expediente_hu  expe , DHABIENTES   DH  where    DH.NUMEMP='" + v_NUMEMP + "'   and expe.expediente =NUMEMP  ";
                    cmm = null;
                    cmm = new SqlCommand(cmdTxt, cnn);

                    lector = null;
                    lector = cmm.ExecuteReader();

                    List<historia> listaHitoria = new List<historia>();
                    while (lector.Read())
                    {
                        historia entidad3 = new historia();


                        entidad3.egr_dias = VerifStringSql(lector, "egr_dias");
                        entidad3.egr_fecha = VerifStringSql(lector, "egr_fecha");
                        entidad3.egr_resumen_evo = VerifStringSql(lector, "egr_resumen_evo");
                        entidad3.ing_resum_interrog = VerifStringSql(lector, "ing_resum_interrog");
                        entidad3.ing_fecha = VerifStringSql(lector, "ing_fecha");

                        listaHitoria.Add(entidad3);
                    }

                    lector.Close();

                    entidad.historia = listaHitoria;



                    // NOTAS

                    cmdTxt = "  SELECT convert(char,fv_fecha, 103)  fv_fecha, fv_nota   ";
                    cmdTxt += "  FROM DHABIENTES   DH,  PARENTESCO PA WHERE  dh.PARIENTE=PA.PARIENTE   and   DH.NUMEMP='" + v_NUMEMP + "' and dh.FVIGENCIA != '1900-01-01T00:00:00'   ";
                    // cmdTxt += "  FROM DHABIENTES   DH,  PARENTESCO PA WHERE dh.FVIGENCIA != '1900-01-01T00:00:00' and  dh.PARIENTE=PA.PARIENTE   and DH.NUMEMP = '" + entidad.EXPE + "' ";

                    cmm = null;
                    cmm = new SqlCommand(cmdTxt, cnn);

                    lector = null;
                    lector = cmm.ExecuteReader();

                    while (lector.Read())
                    {
                        entidad.NOTA_FECHA = VerifStringSql(lector, "fv_fecha");
                        entidad.NOTA = VerifStringSql(lector, "fv_nota");
                    }

                    lector.Close();


                    // Medicamentos

                    cmdTxt = " SELECT concat( inv.descripcion_sal,' ', inv.presentacion) Medicamento, convert(char, expe.Fecha, 103) Fecha,  esp.DESCRIPCION ESPECIALIDAD,    re.CantSurtida, re.Subrogar  ";
                    cmdTxt += " FROM receta_exp_2 expe, inventariofarmacia inv , receta_detalle_2 re , ESPECIALIDADES esp, DHABIENTES   DH  ";
                    cmdTxt += " WHERE   expe.folio_rcta= re.folio_rcta and esp.CLAVE =  SUBSTRING(expe.Medico,1,2) ";
                    cmdTxt += " AND inv.Clave= re.Codigo and   expe.expediente = NUMEMP  and   DH.NUMEMP='" + v_NUMEMP + "'  ";

                    cmdTxt += " and  expe.Fecha_Hora_Adjudicacion is not null and year (expe.Fecha) = year( GETDATE())   and month (expe.Fecha) > month( GETDATE()) -3   order by  expe.Fecha desc";

                    cmm = null;
                    cmm = new SqlCommand(cmdTxt, cnn);

                    lector = null;
                    lector = cmm.ExecuteReader();

                    List<Medicamentos> listaMedicamentos = new List<Medicamentos>();
                    while (lector.Read())
                    {
                        Medicamentos entidad4 = new Medicamentos();


                        entidad4.CantSurtida = VerifStringSql(lector, "CantSurtida");
                        entidad4.ESPECIALIDAD = VerifStringSql(lector, "ESPECIALIDAD");
                        entidad4.Fecha = VerifStringSql(lector, "Fecha");
                        entidad4.Medicamento = VerifStringSql(lector, "Medicamento");
                        entidad4.Subrogar = VerifStringSql(lector, "Subrogar");

                        listaMedicamentos.Add(entidad4);
                    }

                    lector.Close();

                    entidad.Medicamentos = listaMedicamentos;


                    // Prestaciones

                    cmdTxt = " select convert(char, sp.fecha_SOL, 103)   fecha, s.descripcion servicio, p.nombre proveedor , sp.nota  ";
                    cmdTxt += " from  serv_pre_n2 s , servicio_prestaciones sp, prov_subrrogados  p  , DHABIENTES   DH  ";
                    cmdTxt += "   where  s.codigo=sp.codigo and sp.proveedor= p.clave         and year ( sp.fecha_SOL) >  year( GETDATE()) -2  ";
                    cmdTxt += "    and      sp.numemp = DH .NUMEMP  and   DH.NUMEMP='" + v_NUMEMP + "'    ORDER BY sp.fecha_SOL DESC  ";

                    cmm = null;
                    cmm = new SqlCommand(cmdTxt, cnn);

                    lector = null;
                    lector = cmm.ExecuteReader();

                    List<Prestaciones> listaPrestaciones = new List<Prestaciones>();
                    while (lector.Read())
                    {
                        Prestaciones entidad5 = new Prestaciones();
                        entidad5.fecha = VerifStringSql(lector, "fecha");
                        entidad5.nota = VerifStringSql(lector, "nota");
                        entidad5.proveedor = VerifStringSql(lector, "proveedor");
                        entidad5.servicio = VerifStringSql(lector, "servicio");

                        listaPrestaciones.Add(entidad5);
                    }

                    lector.Close();

                    entidad.Prestaciones = listaPrestaciones;




                    // popHp

                    cmdTxt = " select  CONCAT(med.Nombre,' ',med.Apaterno,' ',med.Amaterno)   medico,  rec.descripcion recNombre, ref.descripcion refNombre,  ing_peso,ing_talla,ing_frec_card,ing_pres_art1, ing_indicaciones, ing_dx_probable,ing_exa_lab,ing_exa_gab, ing_explora_fis, DATEDIFF(DAY, ing_fecha  ,egr_fecha) Dias, convert(char,ing_fecha, 103)    FechaIngreso, convert(char,egr_fecha, 103)  EgresoFecha, egr_motivo Motivo, ing_resum_interrog  resumen_evo , ing_plan_tx planTratamiento, egr_cita_abierta citaAbierta, egr_fact_riesgo FactorRiesgo, ing_tx_antes Lab, egr_cita_esp CitaEsp,egr_pronostico pronostico, '' dx ";
                    cmdTxt += "    from expediente_hu     expe  left join hu_recibe  rec on  ing_unidad_rec= rec.clave    left join hu_refiere  ref  on ing_unidad_rec= ref.clave  left join   MEDICOS med on med.Numero=egr_medico  where    expe.expediente = '" + v_NUMEMP + "'    ";

                    cmm = null;
                    cmm = new SqlCommand(cmdTxt, cnn);

                    lector = null;
                    lector = cmm.ExecuteReader();

                    PopHp entidad6 = new PopHp();
                    while (lector.Read())
                    {
                        entidad6.Dias = VerifStringSql(lector, "Dias");
                        entidad6.citaAbierta = VerifStringSql(lector, "citaAbierta");
                        entidad6.CitaEsp = VerifStringSql(lector, "CitaEsp");
                        entidad6.dx = VerifStringSql(lector, "dx");
                        entidad6.EgresoFecha = VerifStringSql(lector, "EgresoFecha");
                        entidad6.FactorRiesgo = VerifStringSql(lector, "FactorRiesgo");
                        entidad6.FechaIngreso = VerifStringSql(lector, "FechaIngreso");
                        entidad6.Lab = VerifStringSql(lector, "Lab");
                        entidad6.Motivo = VerifStringSql(lector, "Motivo");
                        entidad6.planTratamiento = VerifStringSql(lector, "planTratamiento");
                        entidad6.pronostico = VerifStringSql(lector, "pronostico");
                        entidad6.resumen_evo = VerifStringSql(lector, "resumen_evo");

                        entidad6.ing_explora_fis = VerifStringSql(lector, "ing_explora_fis");
                        entidad6.ing_exa_lab = VerifStringSql(lector, "ing_exa_lab");
                        entidad6.ing_exa_gab = VerifStringSql(lector, "ing_exa_gab");
                        entidad6.ing_tx_probable = VerifStringSql(lector, "ing_dx_probable");
                        entidad6.ing_indicaciones = VerifStringSql(lector, "ing_indicaciones");

                        entidad6.ing_peso = VerifStringSql(lector, "ing_peso");
                        entidad6.ing_talla = VerifStringSql(lector, "ing_talla");
                        entidad6.ing_frec_card = VerifStringSql(lector, "ing_frec_card");
                        entidad6.ing_pres_art1 = VerifStringSql(lector, "ing_pres_art1");

                        entidad6.recNombre = VerifStringSql(lector, "recNombre");
                        entidad6.refNombre = VerifStringSql(lector, "refNombre");
                        entidad6.medico = VerifStringSql(lector, "medico");



                    }

                    lector.Close();

                    entidad.PopHp = entidad6;

                }
                return entidad;

            }
            catch (Exception)
            {
                throw;

            }
            finally
            {
                cnn.Close();
            }

        }

        public List<Calendario> BuscarCalendario(string v_MED)
        {
            SqlConnection cnn = null;

            List<Calendario> Lista = new List<Calendario>();

            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = " SELECT     concat(convert(char(10), CITAS.FECHA, 126) ,'T', SUBSTRING(CITAS.turno,1,2),':' , SUBSTRING(CITAS.turno,3,2),':00') turno,  citas.numemp, concat( dh.NOMBRES,' ',dh.APATERNO,' ',dh.AMATERNO) Nombre ";
                cmdTxt += "  FROM CITAS   left join tiposconsulta tc on tc.Clave= CITAS.Tipo  left join DHABIENTES dh on dh.NUMEMP=CITAS.numemp and FVIGENCIA != '1900-01-01T00:00:00' AND NUMAFIL IS NOT NULL AND IsNull(BAJA,'0') <> '01' AND IsNull(BAJA,'0') <> '03'  ";
                cmdTxt +=  " WHERE CITAS.MEDICO = '"+ v_MED+ "' AND  CITAS.FECHA >  CAST(CAST(GETDATE() -90 AS DATE) AS SMALLDATETIME)    and   CITAS.FECHA  < CAST(CAST(GETDATE() +30 AS DATE) AS SMALLDATETIME)";
                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Calendario entidad = new Calendario();

                string url = "/ServiciosMedicos/SOAP/";
                while (lector.Read())
                {
                  
                    entidad = new Calendario();
           
                    entidad.title= VerifStringSql(lector, "Nombre");
                    entidad.url= "/ServiciosMedicos/SOAP/"+ VerifStringSql(lector, "numemp");
                    entidad.start = VerifStringSql(lector, "turno");
                   

                    Lista.Add(entidad);


                }

                lector.Close();

                return Lista;
            }
            catch (Exception)
            {

                throw;

            }
            finally
            {
                cnn.Close();


            }
        }


        public static Int32 VeriIntSql(SqlDataReader lector, string s)
        {

            if (lector[s] == DBNull.Value)
            {

                return 0;
            }
            else
            {

                return Convert.ToInt32(lector[s]);

            }


        }


        public static string VerifStringSql(SqlDataReader lector, string s)
        {

            if (lector[s] == DBNull.Value)
            {

                return "";
            }
            else
            {

                return Convert.ToString(lector[s]);

            }


        }
    }
}