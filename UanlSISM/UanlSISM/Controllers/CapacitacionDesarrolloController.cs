using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class CapacitacionDesarrolloController : Controller
    {
        private SMDEVReporteSemanalActividades db = new SMDEVReporteSemanalActividades();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }


        public ActionResult ReporteSemanal()
        {
            Models.SMDEVActividadesDepartamento db = new Models.SMDEVActividadesDepartamento();

            var jan1 = DateTime.Now.AddDays(-7);
            var startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));
            CultureInfo myCI = new CultureInfo("es-MX");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            var weeks =
            Enumerable
                .Range(0, 54)
                .Select(i => new {
                    Empieza = startOfFirstWeek.AddDays(i * 7)
                })
                .TakeWhile(x => x.Empieza.Year <= jan1.Year)
                .Select(x => new {
                    x.Empieza,
                    Termina = x.Empieza.AddDays(6)
                })
                .SkipWhile(x => x.Termina < jan1.AddDays(1))
                .Select((x, i) => new SemanasList
                {
                    Semana = "Semana " + Convert.ToString(Convert.ToInt32(myCal.GetWeekOfYear(x.Empieza, myCWR, myFirstDOW) - 1)) + " desde " + x.Empieza.ToString("dd/MM/yyyy") + " hasta " + x.Termina.ToString("dd/MM/yyyy"),
                    SemanaId = myCal.GetWeekOfYear(x.Empieza, myCWR, myFirstDOW) - 1,
                    FechaInicio = x.Empieza,
                    FechaFin = x.Termina
                }); ;

            var actividades = (from actividad in db.ActividadesDepartamento
                               where actividad.Departamento == "CapacitacionDesarrollo"
                               select actividad).ToList();



            ViewBag.Actividades = actividades;

            return View(weeks);
        }

        public JsonResult GuardarReporteSemanal(string Detalle)
        {
            List<ReporteActividadesSemanal> Det = JsonConvert.DeserializeObject<List<ReporteActividadesSemanal>>(Detalle);
            SMDEVReporteSemanalActividades reportesa = new SMDEVReporteSemanalActividades();

            int ActividadId = Det[0].ActividadId;
            int Semana = Det[0].Semana;

            var reporteverificar = (from actividad in reportesa.ReporteActividadesSemanal
                                    where actividad.ActividadId == ActividadId &&
                                    actividad.Semana == Semana
                                    select actividad).ToList();

            if (reporteverificar.Count > 0)
            {
                return new JsonResult { Data = "El reporte de esta semana ya fue capturado", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            try
            {
                foreach (var item in Det)
                {
                    Semana s = new Semana(item.Semana);
                    item.FechaInicio = s.FechaInicial;
                    item.FechaFin = s.FechaFinal;
                    item.FechaRealiza = DateTime.Now;
                    item.UsuarioRealiza = User.Identity.Name;
                    reportesa.ReporteActividadesSemanal.Add(item);
                    reportesa.SaveChanges();
                }

            }

            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult ReporteSemanalIndex()
        {
            Models.SMDEVReporteSemanalActividades db = new Models.SMDEVReporteSemanalActividades();

            CultureInfo myCI = new CultureInfo("es-MX");
            int weekNum = myCI.Calendar.GetWeekOfYear(DateTime.Now, myCI.DateTimeFormat.CalendarWeekRule, DayOfWeek.Monday) - 1;

            var list = (from reporte in db.ReporteActividadesSemanal
                        join actividades in db.ActividadesDepartamento
                        on reporte.ActividadId equals actividades.ActividadId
                        where reporte.Semana >= weekNum - 2 &&
                        reporte.Semana <= weekNum - 1
                        where actividades.Departamento == "CapacitacionDesarrollo"
                        select new { reporte, actividades }).ToList();

            List<ReportedeActividades> res = new List<ReportedeActividades>();
            foreach (var item in list)
            {
                var newitem = new ReportedeActividades();
                newitem.ActividadesDepartamento = item.actividades;
                newitem.ReporteActividadesSemanal = item.reporte;

                var cp = (from reporte in db.ReporteActividadesSemanal
                          where reporte.ActividadId == item.reporte.ActividadId
                          where reporte.Semana == item.reporte.Semana - 1
                          select reporte).FirstOrDefault();

                if (cp == null)
                {
                    newitem.ReporteActividadesSemanal.CantidadSemanaPasada = 0;
                }
                else
                {
                    newitem.ReporteActividadesSemanal.CantidadSemanaPasada = cp.Cantidad;
                }

                res.Add(newitem);
            }

            var jan1 = new DateTime(DateTime.Today.Year, 1, 1);
            var startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            var weeks =
            Enumerable
                .Range(1, 54)
                .Select(i => new {
                    Empieza = startOfFirstWeek.AddDays(i * 7)
                })
                .TakeWhile(x => x.Empieza.Year <= jan1.Year)
                .Select(x => new {
                    x.Empieza,
                    Termina = x.Empieza.AddDays(6)
                })
                .SkipWhile(x => x.Termina < jan1.AddDays(1))
                .Select((x, i) => new SemanasList
                {
                    Semana = "Semana " + Convert.ToString(Convert.ToInt32(myCal.GetWeekOfYear(x.Empieza, myCWR, myFirstDOW) - 1)) + " desde " + x.Empieza.ToString("dd/MM/yyyy") + " hasta " + x.Termina.ToString("dd/MM/yyyy"),
                    SemanaId = myCal.GetWeekOfYear(x.Empieza, myCWR, myFirstDOW) - 1,
                    FechaInicio = x.Empieza,
                    FechaFin = x.Termina
                });

            ViewBag.Semanas = weeks;
            ViewBag.WeekNumber = weekNum;

            return View(res);
        }
        public JsonResult GetReporte(int FechaInicio, int FechaFin)
        {
            Models.SMDEVReporteSemanalActividades db = new Models.SMDEVReporteSemanalActividades();

            var list = (from reporte in db.ReporteActividadesSemanal
                        join actividades in db.ActividadesDepartamento
                        on reporte.ActividadId equals actividades.ActividadId
                        where reporte.Semana >= FechaInicio &&
                        reporte.Semana <= FechaFin
                        where actividades.Departamento == "CapacitacionDesarrollo"
                        select new { reporte, actividades }).ToList();

            List<ReportedeActividades> res = new List<ReportedeActividades>();
            foreach (var item in list)
            {
                var newitem = new ReportedeActividades();
                newitem.ActividadesDepartamento = item.actividades;
                newitem.ReporteActividadesSemanal = item.reporte;

                var cp = (from reporte in db.ReporteActividadesSemanal
                          where reporte.ActividadId == item.reporte.ActividadId
                          where reporte.Semana == item.reporte.Semana - 1
                          select reporte).FirstOrDefault();

                if (cp == null)
                {
                    newitem.ReporteActividadesSemanal.CantidadSemanaPasada = 0;
                }
                else
                {
                    newitem.ReporteActividadesSemanal.CantidadSemanaPasada = cp.Cantidad;
                }


                res.Add(newitem);
            }

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetReporteGrafica(int FechaInicio, int FechaFin)
        {
            Models.SMDEVReporteSemanalActividades db = new Models.SMDEVReporteSemanalActividades();

            //var inicio = DateTime.Parse(FechaInicio).ToString("yyyy/MM/dd HH:mm:ss");
            //var fin = DateTime.Parse(FechaFin).ToString("yyyy/MM/dd HH:mm:ss");
            //DateTime FechadeInicio = DateTime.ParseExact(inicio, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            //DateTime FechadeFin = DateTime.ParseExact(fin, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var list = (from reporte in db.ReporteActividadesSemanal
                        join actividades in db.ActividadesDepartamento
                        on reporte.ActividadId equals actividades.ActividadId
                        where reporte.Semana >= FechaInicio &&
                        reporte.Semana <= FechaFin
                        where actividades.Departamento == "CapacitacionDesarrollo"
                        group actividades by actividades.Descripcion into newGroup
                        orderby newGroup.Key

                        select new { newGroup }).ToList();

            var reportes = (from reporte in db.ReporteActividadesSemanal
                            join actividades in db.ActividadesDepartamento
                            on reporte.ActividadId equals actividades.ActividadId
                            where reporte.Semana >= FechaInicio &&
                            reporte.Semana <= FechaFin
                            select new { reporte }).ToList();

            var semanas = (from reporte in db.ReporteActividadesSemanal
                           join actividades in db.ActividadesDepartamento
                           on reporte.ActividadId equals actividades.ActividadId
                           where reporte.Semana >= FechaInicio &&
                           reporte.Semana <= FechaFin
                           group reporte by reporte.Semana into newGroup
                           orderby newGroup.Key
                           select new { newGroup }).ToList();




            var res = new List<object>();
            foreach (var item in list)
            {

                var report = new
                {
                    name = item.newGroup.Key,
                    data = reportes.ToList().Where(a => a.reporte.ActividadId == item.newGroup.FirstOrDefault().ActividadId).Select(x => x.reporte.Cantidad).ToList(),
                    categories = semanas.Select(x => x.newGroup.FirstOrDefault().Semana).ToList()
                };

                res.Add(report);

            }

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult ReporteSemanalGrafica()
        {

            CultureInfo myCI = new CultureInfo("es-MX");
            int weekNum = myCI.Calendar.GetWeekOfYear(DateTime.Now, myCI.DateTimeFormat.CalendarWeekRule, DayOfWeek.Monday) - 1;

            var jan1 = new DateTime(DateTime.Today.Year, 1, 1);
            var startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            var weeks =
            Enumerable
                .Range(1, 54)
                .Select(i => new {
                    Empieza = startOfFirstWeek.AddDays(i * 7)
                })
                .TakeWhile(x => x.Empieza.Year <= jan1.Year)
                .Select(x => new {
                    x.Empieza,
                    Termina = x.Empieza.AddDays(6)
                })
                .SkipWhile(x => x.Termina < jan1.AddDays(1))
                .Select((x, i) => new SemanasList
                {
                    Semana = "Semana " + Convert.ToString(Convert.ToInt32(myCal.GetWeekOfYear(x.Empieza, myCWR, myFirstDOW) - 1)) + " desde " + x.Empieza.ToString("dd/MM/yyyy") + " hasta " + x.Termina.ToString("dd/MM/yyyy"),
                    SemanaId = myCal.GetWeekOfYear(x.Empieza, myCWR, myFirstDOW) - 1,
                    FechaInicio = x.Empieza,
                    FechaFin = x.Termina
                });

            ViewBag.WeekNumber = weekNum;
            return View(weeks);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
