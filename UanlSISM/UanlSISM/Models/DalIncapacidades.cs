using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace UanlSISM.Models
{
    public class DalIncapacidades
    {
        public List<excel1> BuscarExel1(string v_dig, List<string>  v_dep, string v_ini, string v_fin)
        {
            SqlConnection cnn = null;

            List<excel1> Lista = new List<excel1>();

            try
            {
                var departamentos = String.Join(", ", v_dep.ToArray());
                var Diagnostico = v_dig;

                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = "  select DEPENDENCIAS.DESCR as 'Dependencia',	incapacidades.expediente as 'Expediente',	DHABIENTES.NOMBRES + ' ' + DHABIENTES.APATERNO + ' ' + DHABIENTES.AMATERNO as 'Nombre_Completo', ";

                cmdTxt += "  convert(char,incapacidades.fecha_inicio, 103) as 'Fecha_de_Inicio',	DIAGNOSTICOS.DescCompleta as 'Diagnostico',	incapacidades.dias as 'Dias' ,  convert(char,DATEADD(day,incapacidades.dias,incapacidades.fecha_inicio), 103) as 'Fecha_de_Fin' ";
                cmdTxt += " from incapacidades ";
                cmdTxt += " inner join DHABIENTES on incapacidades.expediente = DHABIENTES.NUMEMP ";
                cmdTxt += " inner join DEPENDENCIAS on incapacidades.dependencia = DEPENDENCIAS.CVEDEP ";
                cmdTxt += " inner join DIAGNOSTICOS on incapacidades.dx_1 = DIAGNOSTICOS.Clave ";
                cmdTxt += " where UPPER(dx_1) in  ('"+ Diagnostico + "')  and    DEPENDENCIAS.CVEDEP in ( " + departamentos + ")   and incapacidades.fecha_inicio >= CONVERT(smalldatetime,'" + v_ini + "',101)   and incapacidades.fecha_inicio <= CONVERT(smalldatetime,'" + v_fin + "',101)  ";
                cmdTxt += " order by  DEPENDENCIAS.DESCR ,  incapacidades.expediente ";
                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                excel1 entidad = new excel1();

                while (lector.Read())
                {

                    entidad = new excel1();
                    entidad.Dependencia = VerifStringSql(lector, "Dependencia");
                    entidad.Diagnostico = VerifStringSql(lector, "Diagnostico");
                    entidad.Dias = VerifStringSql(lector, "Dias");
                    entidad.Expediente = VerifStringSql(lector, "Expediente");
                    entidad.Fecha_de_Inicio = VerifStringSql(lector, "Fecha_de_Inicio");
                    entidad.Fecha_de_Fin = VerifStringSql(lector, "Fecha_de_Fin");
                    entidad.Nombre_Completo = VerifStringSql(lector, "Nombre_Completo");

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


        public List<Combos> DiagnoticoCMB()
        {
            SqlConnection cnn = null;

            List<Combos> Lista = new List<Combos>();

            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = "select distinct  clave Id,   concat (clave ,' ',DesCorta) Valor  from  DIAGNOSTICOS where clave<> '' order by clave";
               
                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
               Combos entidad = new Combos();

                while (lector.Read())
                {
                     
                    entidad = new Combos();
                    
                    entidad.Id = VerifStringSql(lector, "Id");
                    entidad.Valor = VerifStringSql(lector, "Valor");

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

        public List<Combos> DependenciasCMB()
        {
            SqlConnection cnn = null;

            List<Combos> Lista = new List<Combos>();

            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = "select CVEDEP Id,DESCR Valor  from  DEPENDENCIAS where CVEDEP <> '' order by DESCR ";

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Combos entidad = new Combos();

                while (lector.Read())
                {

                    entidad = new Combos();

                    entidad.Id = VerifStringSql(lector, "Id");
                    entidad.Valor = VerifStringSql(lector, "Valor");

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

        public List<excel1> Grilla()
        {
            SqlConnection cnn = null;

            List<excel1> Lista = new List<excel1>();

            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = "select top 50 	DEPENDENCIAS.DESCR as 'Dependencia',	incapacidades.expediente as 'Expediente',	DHABIENTES.NOMBRES + ' ' + DHABIENTES.APATERNO + ' ' + DHABIENTES.AMATERNO as 'Nombre Completo',";
                cmdTxt += " incapacidades.fecha_inicio as 'Fecha de Inicio',	DIAGNOSTICOS.DescCompleta as 'Diagnostico',	incapacidades.dias as 'Dias'	";
                cmdTxt += " from 	incapacidades inner join 	DHABIENTES on incapacidades.expediente = DHABIENTES.NUMEMP";
                cmdTxt += " inner join 	DEPENDENCIAS on incapacidades.dependencia = DEPENDENCIAS.CVEDEP inner join 	DIAGNOSTICOS on incapacidades.dx_1 = DIAGNOSTICOS.Clave ";
                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
               
                excel1 entidad = new excel1();

                while (lector.Read())
                {

                    entidad = new excel1();
                    entidad.Dependencia = VerifStringSql(lector, "Dependencia");
                    entidad.Diagnostico = VerifStringSql(lector, "Diagnostico");
                    entidad.Dias = VerifStringSql(lector, "Dias");
                    entidad.Expediente = VerifStringSql(lector, "Expediente");
                    entidad.Fecha_de_Inicio = VerifStringSql(lector, "Fecha_de_Inicio");
                    entidad.Nombre_Completo = VerifStringSql(lector, "Nombre_Completo");

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

        public Incapacidad frmIncapacidad(string v_exp)
        {
            SqlConnection cnn = null;

            Incapacidad Lista = new Incapacidad();

            try
            {
                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = "select expediente, convert(char,DATEADD(day,dias, fecha_inicio), 103)  fecha_fin,convert(char, fecha_inicio, 103)  fecha_inicio  from  incapacidades where  expediente= '"+ v_exp + "' ";

                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                Incapacidad entidad = new Incapacidad();

                while (lector.Read())
                {

                    entidad = new Incapacidad();

                    entidad.expediente = VerifStringSql(lector, "expediente");
                    entidad.fecha_fin = VerifStringSql(lector, "fecha_fin");
                    entidad.fecha_inicio = VerifStringSql(lector, "fecha_inicio");

                     


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

    }
    public class Combos {
        public string Id { get; set; }
        public string Valor { get; set; }

    }

    public class Incapacidad
    {
        public string expediente { get; set; }
        public string fecha_fin { get; set; }
        public string fecha_inicio { get; set; }

    }



}