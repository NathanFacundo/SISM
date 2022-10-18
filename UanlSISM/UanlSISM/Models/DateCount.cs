using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Postal;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using UanlSISM.Models;

namespace UanlSISM.Models
{
    public class DateCount
    {
        public DateTime FechaHora { get; set; }
        public int Cantidad { get; set; }

        public DateCount(DateTime FechaHora1, int Cantidad1)
        {
            FechaHora = FechaHora1;
            Cantidad = Cantidad1;
        }

        public DateCount()
        {

        }
    }


    public class DalExpedientes_hu
    {
        List<DateCount> Lista = new List<DateCount>();
        public List<DateCount> ListaIng()
        {
            SqlConnection cnn = null;
            try
            {



                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = " select ing_fecha from  expediente_hu where ing_fecha is not null order by ing_fecha ";
                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                DateCount entidad = new DateCount();
                Lista.Clear();
                while (lector.Read())
                {

                    entidad = new DateCount();
                    entidad.FechaHora = Convert.ToDateTime(lector["ing_fecha"]);
                    entidad.Cantidad = 0;

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

        public List<DateCount> ListaEgr()
        {
            SqlConnection cnn = null;
            try
            {



                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                string cmdTxt = "  select egr_fecha from  expediente_hu where egr_fecha is not null and  ing_fecha  is not null order by egr_fecha ";
                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                DateCount entidad = new DateCount();
                Lista.Clear();
                while (lector.Read())
                {

                    entidad = new DateCount();
                    entidad.FechaHora = Convert.ToDateTime(lector["egr_fecha"]);
                    entidad.Cantidad = 0;

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


        public List<DateCount> ListaInternados()
        {
            SqlConnection cnn = null;
            try
            {



                var context = new SMDEVEntities16();
                var connection = context.Database.Connection;
                string conStr2 = connection.ConnectionString;

                cnn = new SqlConnection(conStr2);

                cnn.Open();

                //   string cmdTxt = " select egr_fecha from  expediente_hu where egr_fecha is not null order by egr_fecha ";

                string cmdTxt = "   with cte as   (  select getdate() as n   union all   select  dateadd(DAY, -1, n) from cte where dateadd(dd, -1, n) > DATEADD(month, -3, getdate())   )  select     convert(datetime,convert(char,n, 103), 103) n from expediente_hu, cte  where ing_fecha<= n and(egr_fecha > n or egr_fecha is null)    order by ing_fecha  ";
                
                SqlCommand cmm = new SqlCommand(cmdTxt, cnn);

                SqlDataReader lector = cmm.ExecuteReader();
                DateCount entidad = new DateCount();
                Lista.Clear();
                while (lector.Read())
                {

                    entidad = new DateCount();
                    entidad.FechaHora = Convert.ToDateTime(lector["n"]);
                    entidad.Cantidad = 0;

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
    }
}