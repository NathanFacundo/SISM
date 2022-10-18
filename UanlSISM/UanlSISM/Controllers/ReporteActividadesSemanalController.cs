using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class ReporteActividadesSemanalController : Controller
    {
        private SMDEVReporteSemanalActividades db = new SMDEVReporteSemanalActividades();

        // GET: ReporteActividadesSemanal
        public ActionResult Index()
        {
            Models.SMDEVReporteSemanalActividades db = new Models.SMDEVReporteSemanalActividades();

            CultureInfo myCI = new CultureInfo("es-MX");
            int weekNum = myCI.Calendar.GetWeekOfYear(DateTime.Now, myCI.DateTimeFormat.CalendarWeekRule, DayOfWeek.Monday) - 1;

            var list = (from reporte in db.ReporteActividadesSemanal
                        join actividades in db.ActividadesDepartamento
                        on reporte.ActividadId equals actividades.ActividadId
                        where reporte.Semana >= weekNum - 2 &&
                        reporte.Semana <= weekNum - 1
                        select new { reporte, actividades }).ToList();

            List<ReportedeActividades> res = new List<ReportedeActividades>();
            foreach (var item in list)
            {
                var newitem = new ReportedeActividades();
                newitem.ActividadesDepartamento = item.actividades;
                newitem.ReporteActividadesSemanal = item.reporte;

                var cp = (from reporte in db.ReporteActividadesSemanal
                          where reporte.ActividadId == item.reporte.ActividadId
                          where reporte.Semana == item.reporte.Semana -1
                          select  reporte ).FirstOrDefault();

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

            var departamentos = (from departamento in db.ActividadesDepartamento
                                 group departamento by departamento.Departamento into newGroup
                                 orderby newGroup.Key
                                 select new { newGroup.Key }).ToList(); 

            List<ExpandoObject> model = new List<ExpandoObject>();

            foreach (var item in departamentos)
            {
                IDictionary<string, object> itemExpando = new ExpandoObject();
                foreach (PropertyDescriptor property
                         in
                         TypeDescriptor.GetProperties(item.GetType()))
                {
                    itemExpando.Add(property.Name, property.GetValue(item));
                }
                model.Add(itemExpando as ExpandoObject);
            }

            ViewBag.Semanas = weeks;
            ViewBag.WeekNumber = weekNum;
            ViewBag.Departamentos = model;

            return View(res);

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

        // GET: ReporteActividadesSemanal/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ReporteActividadesSemanal reporteActividadesSemanal = db.ReporteActividadesSemanal.Find(id);
        //    if (reporteActividadesSemanal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(reporteActividadesSemanal);
        //}

        // GET: ReporteActividadesSemanal/Create
        public ActionResult Create()
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
                });

            var actividades = (from actividad in db.ActividadesDepartamento
                                 select actividad).ToList();


            ViewBag.Actividades = actividades;
            //ViewBag.ActividadesLinares = actividadesLinares;
            //ViewBag.ActividadesMederos = actividadesMederos;

            return View(weeks);
        }

        //// POST: ReporteActividadesSemanal/Create
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ReporteSemanalActividadId,ActividadId,Cantidad,Semana,FechaInicio,FechaFin,UsuarioRealiza,FechaRealiza")] ReporteActividadesSemanal reporteActividadesSemanal)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ReporteActividadesSemanal.Add(reporteActividadesSemanal);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(reporteActividadesSemanal);
        //}

        //// GET: ReporteActividadesSemanal/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ReporteActividadesSemanal reporteActividadesSemanal = db.ReporteActividadesSemanal.Find(id);
        //    if (reporteActividadesSemanal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(reporteActividadesSemanal);
        //}

        //// POST: ReporteActividadesSemanal/Edit/5
        //// Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        //// más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ReporteSemanalActividadId,ActividadId,Cantidad,Semana,FechaInicio,FechaFin,UsuarioRealiza,FechaRealiza")] ReporteActividadesSemanal reporteActividadesSemanal)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(reporteActividadesSemanal).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(reporteActividadesSemanal);
        //}

        //// GET: ReporteActividadesSemanal/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ReporteActividadesSemanal reporteActividadesSemanal = db.ReporteActividadesSemanal.Find(id);
        //    if (reporteActividadesSemanal == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(reporteActividadesSemanal);
        //}

        //// POST: ReporteActividadesSemanal/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ReporteActividadesSemanal reporteActividadesSemanal = db.ReporteActividadesSemanal.Find(id);
        //    db.ReporteActividadesSemanal.Remove(reporteActividadesSemanal);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        public JsonResult GetReporte(int FechaInicio,int FechaFin, string Unidad, string Departamento)
        {
            Models.SMDEVReporteSemanalActividades db = new Models.SMDEVReporteSemanalActividades();
            List<ReportedeActividades> res = new List<ReportedeActividades>();

            if (Unidad == "")
            {
                var list = (from reporte in db.ReporteActividadesSemanal
                            join actividades in db.ActividadesDepartamento
                            on reporte.ActividadId equals actividades.ActividadId
                            where reporte.Semana >= FechaInicio &&
                            reporte.Semana <= FechaFin
                            where actividades.Departamento == Departamento
                            select new { reporte, actividades }).ToList();


                foreach (var item in list)
                {
                    var newitem = new ReportedeActividades();
                    newitem.ActividadesDepartamento = item.actividades;
                    newitem.ReporteActividadesSemanal = item.reporte;
                    res.Add(newitem);
                }
            }
            else
            {
                var list = (from reporte in db.ReporteActividadesSemanal
                            join actividades in db.ActividadesDepartamento
                            on reporte.ActividadId equals actividades.ActividadId
                            where reporte.Semana >= FechaInicio &&
                            reporte.Semana <= FechaFin
                            where actividades.Departamento == Departamento
                            where actividades.Ubicacion == Unidad
                            select new { reporte, actividades }).ToList();

              
                foreach (var item in list)
                {
                    var newitem = new ReportedeActividades();
                    newitem.ActividadesDepartamento = item.actividades;
                    newitem.ReporteActividadesSemanal = item.reporte;

                    var cp = (from reporte in db.ReporteActividadesSemanal
                              where reporte.ActividadId == item.reporte.ActividadId
                              where reporte.Semana == item.reporte.Semana - 1 select reporte).FirstOrDefault();

                    if(cp == null)
                    {
                        newitem.ReporteActividadesSemanal.CantidadSemanaPasada = 0;
                    }
                    else
                    {
                        newitem.ReporteActividadesSemanal.CantidadSemanaPasada = cp.Cantidad;
                    }

                    res.Add(newitem);
                }
            }

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetReporte2(int FechaInicio, int FechaFin, string Departamento, string Unidad)
        {
            Models.SMDEVReporteSemanalActividades db = new Models.SMDEVReporteSemanalActividades();

            //var inicio = DateTime.Parse(FechaInicio).ToString("yyyy/MM/dd HH:mm:ss");
            //var fin = DateTime.Parse(FechaFin).ToString("yyyy/MM/dd HH:mm:ss");
            //DateTime FechadeInicio = DateTime.ParseExact(inicio, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            //DateTime FechadeFin = DateTime.ParseExact(fin, "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var res = new List<object>();
            if (Unidad == "")
            {
                var list = (from reporte in db.ReporteActividadesSemanal
                            join actividades in db.ActividadesDepartamento
                            on reporte.ActividadId equals actividades.ActividadId
                            where reporte.Semana >= FechaInicio &&
                            reporte.Semana <= FechaFin
                            where actividades.Departamento == Departamento
                            group actividades by actividades.Descripcion into newGroup
                            orderby newGroup.Key
                            select new { newGroup }).ToList();

                var reportes = (from reporte in db.ReporteActividadesSemanal
                                join actividades in db.ActividadesDepartamento
                                on reporte.ActividadId equals actividades.ActividadId
                                where reporte.Semana >= FechaInicio &&
                                reporte.Semana <= FechaFin
                                where actividades.Departamento == Departamento
                                select new { reporte }).ToList();

                var semanas = (from reporte in db.ReporteActividadesSemanal
                               join actividades in db.ActividadesDepartamento
                               on reporte.ActividadId equals actividades.ActividadId
                               where reporte.Semana >= FechaInicio &&
                               reporte.Semana <= FechaFin
                               where actividades.Departamento == Departamento
                               group reporte by reporte.Semana into newGroup
                               orderby newGroup.Key
                               select new { newGroup }).ToList();

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

            }

            else
            {
                var list = (from reporte in db.ReporteActividadesSemanal
                            join actividades in db.ActividadesDepartamento
                            on reporte.ActividadId equals actividades.ActividadId
                            where reporte.Semana >= FechaInicio &&
                            reporte.Semana <= FechaFin
                            where actividades.Ubicacion == Unidad
                            where actividades.Departamento == Departamento
                            group actividades by actividades.Descripcion into newGroup
                            orderby newGroup.Key
                            select new { newGroup }).ToList();

                var reportes = (from reporte in db.ReporteActividadesSemanal
                                join actividades in db.ActividadesDepartamento
                                on reporte.ActividadId equals actividades.ActividadId
                                where reporte.Semana >= FechaInicio &&
                                reporte.Semana <= FechaFin
                                where actividades.Ubicacion == Unidad
                                where actividades.Departamento == Departamento
                                select new { reporte }).ToList();

                var semanas = (from reporte in db.ReporteActividadesSemanal
                               join actividades in db.ActividadesDepartamento
                               on reporte.ActividadId equals actividades.ActividadId
                               where reporte.Semana >= FechaInicio &&
                               reporte.Semana <= FechaFin
                               where actividades.Ubicacion == Unidad
                               where actividades.Departamento == Departamento
                               group reporte by reporte.Semana into newGroup
                               orderby newGroup.Key
                               select new { newGroup }).ToList();

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

            }
           
            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
