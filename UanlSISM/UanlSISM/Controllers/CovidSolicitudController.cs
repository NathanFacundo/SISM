using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;
using static UanlSISM.Controllers.ServiciosMedicosController;

namespace UanlSISM.Controllers
{
    public class CovidSolicitudController : Controller
    {
        public ActionResult Index(string expediente)
        {

            Models.SMDEVCovidSolicitud db = new Models.SMDEVCovidSolicitud();



            var CovidSolicitud = db.CovidSolicitud.Where(x => x.NumEmp == expediente && x.Medico == User.Identity.Name.Substring(0, 5)).ToList();

            Models.SMDEVEntities20 db2 = new Models.SMDEVEntities20();
            var dhabiente = (from a in db2.DHABIENTES
                             where a.NUMEMP == expediente
                             select a).FirstOrDefault();

            List<ExpandoObject> model = new List<ExpandoObject>();

            ViewBag.CovidSolicitudList = CovidSolicitud;

            if (dhabiente.NUMAFIL != "P72112")
            {
                return View(dhabiente);
            }
            else
            {
                TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                return RedirectToAction("SOAP/" + expediente, "ServiciosMedicos");
            }

        }

        public ActionResult Busqueda()
        {
            return View();
        }

        public ActionResult Create(string expediente)
        {

            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabiente = (from a in db.DHABIENTES
                             where a.NUMEMP == expediente
                             select a).FirstOrDefault();
            return View(dhabiente);

        }
        public ActionResult Details(int id)
        {
            Models.SMDEVCovidSolicitud db = new Models.SMDEVCovidSolicitud();

            var covidSolicitud = db.CovidSolicitud.Where(x => x.SolicitudId == id).FirstOrDefault();

            Models.SMDEVEntities20 db3 = new Models.SMDEVEntities20();
            var dhabiente = (from a in db3.DHABIENTES
                             where a.NUMEMP == covidSolicitud.NumEmp
                             select a).FirstOrDefault();

            ViewBag.SolicitudCovid = covidSolicitud;

            return View(dhabiente);

        }

        public JsonResult BuscarDhabPruebaCovid(string numemp, string numexp, string nombre, string apaterno, string amaterno)
        {
            Models.SMDEVEntities16 db = new Models.SMDEVEntities16();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaHoyDT = DateTime.Parse(fechaHoy);

            //string query = "SELECT * FROM DHABIENTES WHERE NUMEMP LIKE '%"+ expTxt + "%' AND NOMBRES like '%" + nombreTxt + "%' COLLATE Latin1_General_CI_AI AND APATERNO like '%" + apaternoTxt + "%' COLLATE Latin1_General_CI_AI AND AMATERNO like '%" + amaternoTxt + "%' COLLATE Latin1_General_CI_AI";
            string query = " SELECT top 300  NUMEMP FROM DHABIENTES  WHERE FVIGENCIA != '1900-01-01T00:00:00' AND NUMAFIL IS NOT NULL AND IsNull(BAJA,'0') <> '01' AND IsNull(BAJA,'0') <> '03' AND (NUMAFIL like '" + numemp + "%' or  ''='" + numemp + "'  ) AND (NUMEMP like '" + numexp + "%' or  ''='" + numexp + "'  ) and ( NOMBRES like '%" + nombre.ToUpper() + "%' or ''='" + nombre.ToUpper() + "'  ) and ( APATERNO like '%" + apaterno.ToUpper() + "%'  or ''='" + apaterno.ToUpper() + "' ) and  (AMATERNO like '%" + amaterno.ToUpper() + "%' or ''='" + amaterno.ToUpper() + "'  )  ";
            var result = db.Database.SqlQuery<DHABIENTES>(query);
            var dhabientes = result.ToList();

            //System.Diagnostics.Debug.WriteLine(dhabientes);

            var results1 = new List<BuscarDhabList>();


            foreach (var dh in dhabientes)
            {
                //Buscar en tabla Lab_exp si tiene un registro
                var dhab = (from d in db2.DHABIENTES
                            where d.NUMEMP == dh.NUMEMP
                            //where r.fecha_crea >= fecha_correcta
                            //orderby r.fecha_crea descending
                            select new
                            {
                                paciente = d.NOMBRES + " " + d.APATERNO + " " + d.AMATERNO,
                                fnac = d.FNAC,
                                sexo = d.SEXO,
                                numexp = d.NUMEMP,
                                numemp = d.NUMAFIL,
                                fvigencia = d.FVIGENCIA,
                            }).FirstOrDefault();


                var edad = 0;
                var meses = 0;

                if (dhab != null)
                {
                    var today = DateTime.Today;
                    DateTime fnac = (DateTime)dhab.fnac;
                    int Years = 0;
                    int Months = 0;
                    int Days = 0;

                    if ((today.Year - fnac.Year) > 0 ||
                    (((today.Year - fnac.Year) == 0) && ((fnac.Month < today.Month) ||
                    ((fnac.Month == today.Month) && (fnac.Day <= today.Day)))))
                    {
                        int DaysInBdayMonth = DateTime.DaysInMonth(fnac.Year, fnac.Month);
                        int DaysRemain = today.Day + (DaysInBdayMonth - fnac.Day);

                        if (today.Month > fnac.Month)
                        {
                            Years = today.Year - fnac.Year;
                            Months = today.Month - (fnac.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
                            Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                        }
                        else if (today.Month == fnac.Month)
                        {
                            if (today.Day >= fnac.Day)
                            {
                                Years = today.Year - fnac.Year;
                                Months = 0;
                                Days = today.Day - fnac.Day;
                            }
                            else
                            {
                                Years = (today.Year - 1) - fnac.Year;
                                Months = 11;
                                Days = DateTime.DaysInMonth(fnac.Year, fnac.Month) - (fnac.Day - today.Day);
                            }
                        }
                        else
                        {
                            Years = (today.Year - 1) - fnac.Year;
                            Months = today.Month + (11 - fnac.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
                            Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                        }
                    }


                    var fecha = DateTime.Now.AddYears(-1).ToString("yyyy-MM-ddT00:00:00.000");

                    //Buscar si tienen un folio
                    /*var prueCovid = (from d in db2.CovidSolicitud
                                     where d.NumEmp == dh.NUMEMP
                                     where d.Resultado == null
                                     //where r.fecha_crea >= fecha_correcta
                                     //orderby r.fecha_crea descending
                                     select new
                                     {
                                         SolicitudId = d.SolicitudId,
                                         FechaSol = d.FechaSol,
                                     })
                                     .OrderByDescending(g => g.SolicitudId)
                                     .FirstOrDefault();*/


                    //if (prueCovid != null)
                    //{
                    var vigencia = "";

                    if (fechaHoyDT > dhab.fvigencia)
                    {
                        vigencia = "Vigencia vencida";
                    }
                    else
                    {
                        vigencia = "Vigente";
                    }


                    var resultado = new BuscarDhabList
                    {
                        //numemp = dh.NUMEMP + "*" + dhab.paciente + "*" + edad + "*" + dhab.sexo,
                        paciente = dhab.paciente,
                        numexp = dhab.numexp,
                        numemp = dhab.numemp,
                        edad = Years + " años con " + Months + " meses",
                        sexo = dhab.sexo,
                        //info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + "*" + dhab.sexo + "*" + prueCovid.SolicitudId + "*" + string.Format("{0:dddd, dd MMMM yyyy}", prueCovid.FechaSol),
                        info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + "*" + dhab.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", dhab.fvigencia),
                    };

                    results1.Add(resultado);
                    //}


                }
            }

            //System.Diagnostics.Debug.WriteLine(results1);

            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GuardarSolicitudCovid(string newSolCovid)
        {
            CovidSolicitud NewSol = JsonConvert.DeserializeObject<CovidSolicitud>(newSolCovid);             
            Models.SMDEVCovidSolicitud db = new Models.SMDEVCovidSolicitud();
            int id = 0;
            try
            {                
                //NewSol.Medico = User.Identity.Name;
                NewSol.Medico = User.Identity.Name.Substring(0, 5);
                NewSol.FechaSol = DateTime.Now;
                NewSol.Finalizado = false;
                NewSol .Cancelado = false;
                NewSol .NoPresento = false;
                NewSol .Urgente = "1";

                db.CovidSolicitud.Add(NewSol );
                db.SaveChanges();
                id = NewSol.SolicitudId;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            //catch (Exception ex)
            //{
            //    return new JsonResult { Data = "0", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //}
            return new JsonResult { Data = id.ToString(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }        public JsonResult DeleteSolicitud(string id)
        {
            Models.SMDEVCovidSolicitud db = new Models.SMDEVCovidSolicitud();

            try
            {
                var cs = db.CovidSolicitud.Find(Convert.ToInt32(id));
                db.CovidSolicitud.Remove(cs);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        #region "AEMA"

        public ActionResult IndexAEMA(string expediente)
        {

            Models.SMDEVCovidSolicitud db = new Models.SMDEVCovidSolicitud();

            var CovidSolicitud = db.CovidSolicitud.Where(x => x.NumEmp == expediente && x.Medico == User.Identity.Name).ToList();

            Models.SMDEVEntities20 db2 = new Models.SMDEVEntities20();
            var dhabiente = (from a in db2.DHABIENTES
                             where a.NUMEMP == expediente
                             select a).FirstOrDefault();

            List<ExpandoObject> model = new List<ExpandoObject>();

            ViewBag.CovidSolicitudList = CovidSolicitud;

            return View(dhabiente);

        }


        public ActionResult CreateAEMA(string expediente)
        {

            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabiente = (from a in db.DHABIENTES
                             where a.NUMEMP == expediente
                             select a).FirstOrDefault();
            return View(dhabiente);

        }


        public ActionResult DetailsAEMA(int id)
        {
            Models.SMDEVCovidSolicitud db = new Models.SMDEVCovidSolicitud();

            var covidSolicitud = db.CovidSolicitud.Where(x => x.SolicitudId == id).FirstOrDefault();

            Models.SMDEVEntities20 db3 = new Models.SMDEVEntities20();
            var dhabiente = (from a in db3.DHABIENTES
                             where a.NUMEMP == covidSolicitud.NumEmp
                             select a).FirstOrDefault();

            ViewBag.SolicitudCovid = covidSolicitud;

            return View(dhabiente);

        }


        public ActionResult BusquedaAEMA()
        {
            return View();
        }
        #endregion

    }
}