using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
  public  class DalMisMedicos
    {

        #region BuscarEspMedicoActivo
        //CG - 20210616 - BuscarEspMedicoActivo
        public List<Med_Expediente> BuscarEspMedicoActivo(string pClaveMedico, string cboMEDICOS, DateTime TxtFecha)
        {
            
            SqlConnection cnn = null;
            String ParamQuery1 = ((pClaveMedico != string.Empty) ? pClaveMedico : cboMEDICOS);
            var ParamQuery2 = string.Format("{0:yyyy-MM-ddT00:00:00}", TxtFecha);

            List<Med_Expediente> lista = new List<Med_Expediente>();
            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;
                cnn = new SqlConnection(conStr2);
                cnn.Open();

                string cmdTxt = " SELECT ";
                cmdTxt += " MD.NUMERO, CONCAT(MD.APATERNO, ', ', MD.AMATERNO, ', ', MD.NOMBRE) NOMBRES, MD.TITULO, ";
                cmdTxt += " EX.fecha, EX.numemp, EX.medico, EX.edd_anio, EX.edd_mes, EX.peso_r, ";
                cmdTxt += " EX.talla_r,EX.ta1, EX.ta2, EX.temp_r, EX.fresp,DIAGN.DesCorta as Diagnostico, ";
                cmdTxt += " DIAGN2.DesCorta as Diagnostico2, DIAGN3.DesCorta as Diagnostico3, EX.referido, EX.referido_urg, ";
                cmdTxt += " EX.tipconsulta, EX.s_exp, EX.o_exp, EX.p_exp, EX.a_exp, ";
                cmdTxt += " DH.CURP, DH.APATERNO, DH.AMATERNO, DH.NOMBRES AS NOMBRESDH, DH.FNAC ";
                cmdTxt += " FROM MEDICOS MD ";
                cmdTxt += " LEFT JOIN EXPEDIENTE EX ";
                cmdTxt += " ON EX.medico = MD.Numero ";
                cmdTxt += " AND EX.FECHA = '" + ParamQuery2 + "' ";
                cmdTxt += " LEFT JOIN DHABIENTES DH ";
                cmdTxt += " ON DH.NUMEMP = EX.NUMEMP ";
                cmdTxt += " LEFT JOIN  DIAGNOSTICOS DIAGN ";
                cmdTxt += " ON EX.diagnostico = DIAGN.Clave ";
                cmdTxt += " LEFT JOIN  DIAGNOSTICOS DIAGN2 ";
                cmdTxt += " ON EX.diagnostico2 = DIAGN2.Clave ";
                cmdTxt += " LEFT JOIN  DIAGNOSTICOS DIAGN3 ";
                cmdTxt += " ON EX.diagnostico3 = DIAGN3.Clave ";
                cmdTxt += " WHERE MD.NUMERO = '" + ParamQuery1.Trim() + "' ";
                cmdTxt += " ORDER BY EX.numemp DESC ";
                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Med_Expediente Entidad = null;
                string cmdTxt1 = String.Empty;

                while (lector.Read())
                {
                    Entidad = new Med_Expediente();
                    Entidad.NOMBRESMEDICO = VerifStringSql(lector, "NOMBRES");
                    Entidad.TITULO = VerifStringSql(lector, "TITULO");
                    Entidad.NUMEMP = VerifStringSql(lector, "NUMEMP");
                    Entidad.ANIO = VerifStringSql(lector, "edd_anio");
                    Entidad.MES = VerifStringSql(lector, "edd_mes");
                    Entidad.PESO = VerifStringSql(lector, "peso_r");
                    Entidad.TALLA = VerifStringSql(lector, "talla_r");
                    Entidad.PRES_ART1 = VerifStringSql(lector, "ta1");
                    Entidad.PRES_ART1 = VerifStringSql(lector, "ta2");
                    Entidad.DHAPATERNO = VerifStringSql(lector, "APATERNO");
                    Entidad.DHAMATERNO = VerifStringSql(lector, "AMATERNO");
                    Entidad.DHNOMBRES = VerifStringSql(lector, "NOMBRESDH");
                    Entidad.FNAC = VerifStringSql(lector, "FNAC");
                    if (Entidad.FNAC != "")
                    {
                      Entidad.FNAC = Edad.CalcularEdad(Convert.ToDateTime(Entidad.FNAC)).ToString();
                    }

                    Entidad.SUBJETIVO = VerifStringSql(lector, "s_exp");
                    Entidad.OBJETIVO = VerifStringSql(lector, "o_exp");
                    Entidad.DIAGNOSTICO = VerifStringSql(lector, "diagnostico");
                    Entidad.DIAGNOSTICO2 = VerifStringSql(lector, "diagnostico2");
                    Entidad.DIAGNOSTICO3 = VerifStringSql(lector, "diagnostico3");
                    Entidad.PLAN = VerifStringSql(lector, "p_exp");

                    Entidad.RECETA = VerifStringSql(lector, "a_exp");
                    lista.Add(Entidad);
                }

                lector.Close();

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cnn.Close();
            }
        }
        #endregion BuscarEspMedicoActivo


        public List<Medicos_especialidad> BuscarEspMedico(string TipoBaja, string TipoEsp)
        {
            SqlConnection cnn = null;


            List<Medicos_especialidad> lista = new List<Medicos_especialidad>();
            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = "SELECT  NUMERO, CONCAT(APATERNO,', ',AMATERNO,', ',NOMBRE) NOMBRES,   ";
                cmdTxt += " CASE  WHEN len(HRLUNES) >0  THEN  CONCAT( SUBSTRING(LEFT(HRLUNES, 4),1,2),':',SUBSTRING(LEFT(HRLUNES, 4),3,2),'-', SUBSTRING(right(HRLUNES, 7),1,2) ,':',SUBSTRING(right(HRLUNES, 7),3,2)) ELSE '-'   END as LUNES ,  ";

                cmdTxt += "  CASE  WHEN len(HRmartes) >0  THEN  CONCAT( SUBSTRING(LEFT(HRmartes, 4),1,2),':',SUBSTRING(LEFT(HRmartes, 4),3,2),'-', SUBSTRING(right(HRmartes, 7),1,2) ,':',SUBSTRING(right(HRmartes, 7),3,2)) ELSE '-'   END as MARTES ,  ";
                cmdTxt += "  CASE  WHEN len(HRmiercoles) >0  THEN  CONCAT( SUBSTRING(LEFT(HRmiercoles, 4),1,2),':',SUBSTRING(LEFT(HRmiercoles, 4),3,2),'-', SUBSTRING(right(HRmiercoles, 7),1,2) ,':',SUBSTRING(right(HRmiercoles, 7),3,2)) ELSE '-'   END as MIERCOLES, ";
                cmdTxt += "   CASE  WHEN len(HRjueves) >0  THEN  CONCAT( SUBSTRING(LEFT(HRjueves, 4),1,2),':',SUBSTRING(LEFT(HRjueves, 4),3,2),'-', SUBSTRING(right(HRjueves, 7),1,2) ,':',SUBSTRING(right(HRjueves, 7),3,2)) ELSE '-'   END as JUEVES , ";
                cmdTxt += " CASE  WHEN len(HRviernes) >0  THEN  CONCAT( SUBSTRING(LEFT(HRviernes, 4),1,2),':',SUBSTRING(LEFT(HRviernes, 4),3,2),'-', SUBSTRING(right(HRviernes, 7),1,2) ,':',SUBSTRING(right(HRviernes, 7),3,2)) ELSE '-'   END as VIERNES  ";
              
                cmdTxt += "  FROM MEDICOS WHERE LEFT(MEDICOS.NUMERO,2) = '" + TipoEsp.Trim() + "'   ";


                if (TipoBaja=="true") {
                    cmdTxt += "  and estatus='1' order by numero";
                }
                else
                {
                    cmdTxt += "  and estatus='0' order by numero";
                }
                

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Medicos_especialidad  entidad = new Medicos_especialidad();

                while (lector.Read())
                {

                    entidad = new Medicos_especialidad();
                    entidad.NUMERO = VerifStringSql(lector, "NUMERO");
                    entidad.NOMBRES = VerifStringSql(lector, "NOMBRES");
                    entidad.LUNES= VerifStringSql(lector, "LUNES");
                    entidad.MARTES = VerifStringSql(lector, "MARTES");
                    entidad.MIERCOLES = VerifStringSql(lector, "MIERCOLES");
                    entidad.JUEVES = VerifStringSql(lector, "JUEVES");
                    entidad.VIERNES = VerifStringSql(lector, "VIERNES");
                    lista.Add(entidad);
                }

                lector.Close();

                return lista;
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


        public Medicos BuscarMedico(string v_num)
        {
            SqlConnection cnn = null;

       

            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = " ";
                


                cmdTxt += "    SELECT   NUMERO, APATERNO, AMATERNO, NOMBRE, MEDICOS.TITULO, DESCRIPCION ESPECIALIDAD, CONSULTORIO, convert(char, FCANCELINI, 103)  FCANCELINI,convert(char, FCANCELFIN, 103) FCANCELFIN,OBSCANCEL,RECEPCION,CEDULA,SOCIO1,SOCIO2,convert(char, FCUBREINI, 103) FCUBREINI,convert(char, FCUBREFIN, 103) FCUBREFIN,CUBREMED      ";
                cmdTxt += "   ,  (SELECT DISTINCT   ((YEAR(GETDATE()) - YEAR(Emp_FechaIngreso)) / 5) * 4 - (select ISNULL(sum(convert(int,Kar_DIA)),0) from SISM.dbo.TblKardex where Emp_NumEmp = SISM.dbo.TblEmpleado.Emp_NumEmp and Kar_STA in( 'A','P') And year(convert(datetime,Kar_INI )) = year( getdate()) and ClaveMov = 7 )  FROM SISM.dbo.TblEmpleado inner join SISM.dbo.TblKardex 	on  SISM.dbo.TblEmpleado.Emp_NumEmp = TblKardex.Emp_NumEmp 	WHERE 	TblEmpleado.Emp_NumEmp=SMDEV.dbo.MEDICOS.emp_numemp  and IdRegimen IN (1,2)  AND  Emp_Estatus = 1 	AND ClaveMov = 7 	AND Kar_STA IN ('A','P')) AS Quinquenio ";
                cmdTxt += "   , ( 	select 	10 - ISNULL((SELECT SUM(CONVERT(INT,Kar_DIA))  FROM SISM.dbo.TblKardex 	WHERE SISM.dbo.TblKardex.Emp_NumEmp = SMDEV.dbo.MEDICOS.emp_numemp 	AND Kar_STA = 'A'  AND ClaveMov = 9 and MONTH(convert(datetime,  Kar_INI)) >= 7 and MONTH(convert(datetime,  Kar_INI)) <= 12 AND YEAR (CONVERT(DATETIME,Kar_INI)) = YEAR(GETDATE())),0))  AS DIAS_RESTANTES_2    ";
                cmdTxt += "  ,(SELECT   RR.Regimen from  SISM.dbo.TblEmpleado AS PP, SISM.dbo.TblRegimen RR where PP.Emp_NumEmp = SMDEV.dbo.MEDICOS.emp_numemp and RR.IdRegimen = PP.IdRegimen) Tipo_Empleado  ";
                cmdTxt += "  , (SELECT top 1 pp.Emp_CURP from  SISM.dbo.TblEmpleado AS PP where PP.Emp_NumEmp = SMDEV.dbo.MEDICOS.emp_numemp ) CURP ";
                cmdTxt += "   ,  ( select DISTINCT ((YEAR(GETDATE()) - YEAR(Emp_FechaIngreso)) / 5) * 4 - (select ISNULL(sum(convert(int,Kar_DIA)),0) from SISM.dbo.TblKardex where Emp_NumEmp = TblEmpleado.Emp_NumEmp and Kar_STA in( 'A','P') And year(convert(datetime,Kar_INI )) = year( getdate()) and ClaveMov = 7 ) AS 'Dias Disponibles'   from  SISM.dbo.TblKardex inner join  SISM.dbo.TblEmpleado on  TblKardex.Emp_NumEmp =   TblEmpleado.Emp_NumEmp  WHERE  TblEmpleado.Emp_NumEmp=SMDEV.dbo.MEDICOS.emp_numemp ) AS Dias_Disponibles   ";

                cmdTxt += "   FROM SMDEV.dbo.MEDICOS left join  SMDEV.dbo.ESPECIALIDADES on  SMDEV.dbo.ESPECIALIDADES.CLAVE = CONVERT(VARCHAR(2), MEDICOS.NUMERO)   WHERE NUMERO = '" + v_num.Trim() + "'";

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Medicos entidad = new Medicos();

                while (lector.Read())
                {
                    
                    entidad = new Medicos();
                    entidad.NUMERO = VerifStringSql(lector, "NUMERO");
                    entidad.AMATERNO = VerifStringSql(lector, "AMATERNO");
                    entidad.APATERNO = VerifStringSql(lector, "APATERNO");
                    entidad.CEDULA = VerifStringSql(lector, "CEDULA");
                    entidad.CONSULTORIO = VerifStringSql(lector, "CONSULTORIO");
                    entidad.CUBREMED= VerifStringSql(lector, "CUBREMED");
                    entidad.ESPECIALIDAD = VerifStringSql(lector, "ESPECIALIDAD");
                    entidad.FCANCELFIN = VerifStringSql(lector, "FCANCELFIN");
                    entidad.FCANCELINI = VerifStringSql(lector, "FCANCELINI");
                    entidad.FCUBREFIN = VerifStringSql(lector, "FCUBREFIN");
                    entidad.FCUBREINI = VerifStringSql(lector, "FCUBREINI");
                    entidad.NOMBRE = VerifStringSql(lector, "NOMBRE");
                    entidad.OBSCANCEL= VerifStringSql(lector, "OBSCANCEL");
                    entidad.RECEPCION = VerifStringSql(lector, "RECEPCION");
                    entidad.SOCIO1 = VerifStringSql(lector, "SOCIO1");

                    entidad.SOCIO2= VerifStringSql(lector, "SOCIO2");
                    entidad.TITULO = VerifStringSql(lector, "TITULO");

                    entidad.Dias_Disponibles = VerifStringSql(lector, "Dias_Disponibles");
                    entidad.Quinquenio = VerifStringSql(lector, "Quinquenio");


                    entidad.DIAS_RESTANTES_2 = VerifStringSql(lector, "DIAS_RESTANTES_2");
                    entidad.Tipo_Empleado = VerifStringSql(lector, "Tipo_Empleado");
                    entidad.CURP = VerifStringSql(lector, "CURP");
                }

                lector.Close();

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


        public Medicos BuscarBsqMedico(string v_num)
        {
            SqlConnection cnn = null;



            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = "SELECT pac_lun,HRLUNES,OBLUNES,pac_mar,HRMARTES,OBMARTES,pac_mie,HRMIERCOLES,OBMIERCOLES,pac_jue,HRJUEVES,OBJUEVES,pac_vie,HRVIERNES,OBVIERNES,HRSABADO,OBSABADO,HRDOMINGO,OBDOMINGO,SOCIO1,SOCIO2 FROM MEDICO  ";               
                cmdTxt += "  WHERE NUMERO = '" + v_num.Trim() + "'";

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Medicos entidad = new Medicos();

                while (lector.Read())
                {

                    entidad = new Medicos();
                    entidad.NUMERO = VerifStringSql(lector, "NUMERO");
                    entidad.AMATERNO = VerifStringSql(lector, "AMATERNO");
                    entidad.APATERNO = VerifStringSql(lector, "APATERNO");
                    entidad.CEDULA = VerifStringSql(lector, "CEDULA");
                    entidad.CONSULTORIO = VerifStringSql(lector, "CONSULTORIO");
                    entidad.CUBREMED = VerifStringSql(lector, "CUBREMED");
                    entidad.ESPECIALIDAD = VerifStringSql(lector, "ESPECIALIDAD");
                    entidad.FCANCELFIN = VerifStringSql(lector, "FCANCELFIN");
                    entidad.FCANCELINI = VerifStringSql(lector, "FCANCELINI");
                    entidad.FCUBREFIN = VerifStringSql(lector, "FCUBREFIN");
                    entidad.FCUBREINI = VerifStringSql(lector, "FCUBREINI");
                    entidad.NOMBRE = VerifStringSql(lector, "NOMBRE");
                    entidad.OBSCANCEL = VerifStringSql(lector, "OBSCANCEL");
                    entidad.RECEPCION = VerifStringSql(lector, "RECEPCION");
                    entidad.SOCIO1 = VerifStringSql(lector, "SOCIO1");

                    entidad.SOCIO2 = VerifStringSql(lector, "SOCIO2");
                    entidad.TITULO = VerifStringSql(lector, "TITULO");


                }

                lector.Close();

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

        public List<Combos> BuscarMed_horario()
        {
            SqlConnection cnn = null;
            List<Combos> lista = new List<Combos>();
            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = " SELECT clave,descripcion  FROM med_horario ORDER BY clave";

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Combos entidad = new Combos();

                while (lector.Read())
                {

                    entidad = new Combos();
                    entidad.Id = VerifStringSql(lector, "clave");
                    entidad.Valor = VerifStringSql(lector, "descripcion");


                    lista.Add(entidad);
                }

                lector.Close();

                return lista;
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
        public List<Combos> Buscardir_medico()
        {
            SqlConnection cnn = null;
            List<Combos> lista = new List<Combos>();
            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = " SELECT clave,descripcion  FROM dir_medico ORDER BY clave";

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Combos entidad = new Combos();

                while (lector.Read())
                {

                    entidad = new Combos();
                    entidad.Id = VerifStringSql(lector, "clave");
                    entidad.Valor = VerifStringSql(lector, "descripcion");


                    lista.Add(entidad);
                }

                lector.Close();

                return lista;
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
        public List<Combos> BuscarUniversidades()
        {
            SqlConnection cnn = null;
            List<Combos> lista = new List<Combos>();
            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = " SELECT clave,descripcion  FROM universidades ORDER BY CLAVE";

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Combos entidad = new Combos();

                while (lector.Read())
                {

                    entidad = new Combos();
                    entidad.Id = VerifStringSql(lector, "clave");
                    entidad.Valor = VerifStringSql(lector, "descripcion");


                    lista.Add(entidad);
                }

                lector.Close();

                return lista;
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
        public List<Combos> BuscarEspecialidades()
        {
            SqlConnection cnn = null;
            List<Combos> lista = new List<Combos>();
            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = " SELECT clave,descripcion  FROM ESPECIALIDADES ORDER BY DESCRIPCION ";
          
                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Combos entidad = new Combos();
              
                while (lector.Read())
                {

                    entidad = new Combos();
                    entidad.Id = VerifStringSql(lector, "clave");
                    entidad.Valor = VerifStringSql(lector, "descripcion");


                    lista.Add(entidad);
                }

                lector.Close();

                return lista;
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
        public string VerifStringSql(SqlDataReader lector, string s)
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


        public static string VerifStringSql2(SqlDataReader lector, string s)
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
        public List<Combos> cmbEspMed()
        {
            SqlConnection cnn = null;
            List<Combos> lista = new List<Combos>();
            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = " SELECT MEDICOS.NUMERO,concat (ESPECIALIDADES.DESCRIPCION ,' - ',MEDICOS.APATERNO,',',MEDICOS.AMATERNO,',',MEDICOS.NOMBRE)  descripcion ";
                cmdTxt += "  FROM   MEDICOS, ESPECIALIDADES WHERE 		MEDICOS.CLAVE = ESPECIALIDADES.CLAVE AND  ";
                cmdTxt += "  MEDICOS.APATERNO <> '' and MEDICOS.estatus <> '1'   ORDER BY MEDICOS.CLAVE, MEDICOS.APATERNO, MEDICOS.AMATERNO, MEDICOS.NOMBRE  ";
                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Combos entidad = new Combos();

                while (lector.Read())
                {

                    entidad = new Combos();
                    entidad.Id = VerifStringSql(lector, "NUMERO");
                    entidad.Valor = VerifStringSql(lector, "descripcion");


                    lista.Add(entidad);
                }

                lector.Close();

                return lista;
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
    }


    public class Medicos
    {
        
              public string NUMERO { get; set; }
        public  string APATERNO { get; set; }
        public string AMATERNO { get; set; }
        public string NOMBRE { get; set; }
        public string TITULO { get; set; }
        public string ESPECIALIDAD { get; set; }
        public string CONSULTORIO { get; set; }
        public string FCANCELINI { get; set; }
        public string FCANCELFIN { get; set; }

        public string OBSCANCEL { get; set; }
        public string RECEPCION { get; set; }
        public string CEDULA { get; set; }
        public string SOCIO1 { get; set; }

        public string SOCIO2 { get; set; }
        public string FCUBREINI { get; set; }
        public string FCUBREFIN { get; set; }


        public string CUBREMED { get; set; }


        public string CURP { get; set; }
        public string Tipo_Empleado { get; set; }
        public string DIAS_RESTANTES_2 { get; set; }
        public string Dias_Disponibles { get; set; }

        public string Quinquenio { get; set; }
    }

    public class Medicos_especialidad
    {

        public string NUMERO { get; set; }
        public string NOMBRES { get; set; }
        public string LUNES { get; set; }
        public string MARTES { get; set; }
        public string MIERCOLES { get; set; }
        public string JUEVES  { get; set; }
        public string VIERNES { get; set; }
       


    }

    public class Medicos_agenda
    {

        public string NUMERO { get; set; }
        public string NOMBRES { get; set; }
        public string fcancelini { get; set; }
        public string fcancelfin { get; set; }
        public string f2cancelini { get; set; }
        public string f2cancelfin { get; set; }

        public string tipo { get; set; }


    }

    public class Med_Expediente
    {
        public string NOMBRESMEDICO { get; set; }
        public string TITULO { get; set; }
        public string NUMEMP { get; set; }
        public string ANIO { get; set; }
        public string MES { get; set; }
        public string PESO { get; set; }
        public string TALLA { get; set; }
        public string PRES_ART1 { get; set; }
        public string PRES_ART2 { get; set; }
        public string DHAPATERNO { get; set; }
        public string DHAMATERNO { get; set; }
        public string DHNOMBRES { get; set; }
        public string FNAC { get; set; }
        public string SUBJETIVO { get; set; }
        public string OBJETIVO { get; set; }
        public string ANALISISI { get; set; }
        public string PLAN { get; set; }
        public string DIAGNOSTICO3 { get; set; }
        public string DIAGNOSTICO { get; set; }
        public string DIAGNOSTICO2 { get; set; }
        public string RECETA { get; set; }
    }
    public class Exp_Receta
    {
        public string EXPEDIENTE { get; set; }
        public string FOLIO { get; set; }
        public string CANTIDAD { get; set; }
        public string DOSIS { get; set; }
    }


    public static class Edad
    {
        public static int CalcularEdad(DateTime fechaNacimiento)
        {
            // Obtiene la fecha actual:
            DateTime fechaActual = DateTime.Today;

            // Comprueba que la se haya introducido una fecha válida; si 
            // la fecha de nacimiento es mayor a la fecha actual se muestra mensaje 
            // de advertencia:
            if (fechaNacimiento > fechaActual)
            {
                Console.WriteLine("La fecha de nacimiento es mayor que la actual.");
                return -1;
            }
            else
            {
                int edad = fechaActual.Year - fechaNacimiento.Year;

                // Comprueba que el mes de la fecha de nacimiento es mayor 
                // que el mes de la fecha actual:
                if (fechaNacimiento.Month > fechaActual.Month)
                {
                    --edad;
                }

                return edad;
            }
        }
    }

}