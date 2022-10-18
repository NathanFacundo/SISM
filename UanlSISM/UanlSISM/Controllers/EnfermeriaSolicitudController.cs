          using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
                                      
    [Authorize]
    public class EnfermeriaSolicitudController : Controller
    {
        // GET: EnfermeriaSolicitud
        public ActionResult Index(string expediente)
        {

            Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();



            var enfermeriaSolicitud = db.EnfermeriaSolicitud.Where(x => x.NumEmp == expediente && x.Medico == User.Identity.Name).ToList();
            var enfermeriaHist = (from historial in db.EnfermeriaSolicitudDetalle
                                  join solicitud in db.EnfermeriaSolicitud on historial.SolicitudId equals solicitud.SolicitudId
                                  join med in db.MEDICOS on solicitud.Medico equals med.Numero
                                  where solicitud.NumEmp == expediente
                                  select new
                                  {
                                      Descripcion = historial.Descripcion,
                                      FechaSol = solicitud.FechaSol,
                                      UsuarioRealiza = med.Nombre + " " + med.Apaterno + " " + med.Amaterno,
                                      Urgente = solicitud.Urgente,
                                      Nota = solicitud.Nota
                                  }).ToList();



            Models.SMDEVEntities20 db2 = new Models.SMDEVEntities20();
            var dhabiente = (from a in db2.DHABIENTES
                             where a.NUMEMP == expediente
                             select a).FirstOrDefault();

            List<ExpandoObject> model = new List<ExpandoObject>();

            foreach (var item in enfermeriaHist)
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

            ViewBag.EnfermeriaSolicitudList = enfermeriaSolicitud;
            ViewBag.HistorialEnfermeria = model;


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

        public ActionResult IndexAgenda(string expediente)
        {

            Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();



            var enfermeriaSolicitud = (from enfsol in db.EnfermeriaSolicitud
                                       join med in db.MEDICOS on enfsol.Medico equals med.Numero
                                       where enfsol.NumEmp == expediente
                                       select new
                                       {
                                           SolicitudId = enfsol.SolicitudId,
                                           FechaSol = enfsol.FechaSol,
                                           FechaAgenda = enfsol.FechaAgenda,
                                           FechaRealiza = enfsol.FechaRealiza,
                                           Medico = med.Nombre + " " + med.Apaterno + " " + med.Amaterno,
                                           Urgente = enfsol.Urgente,
                                           Nota = enfsol.Nota
                                       }).ToList();

            var enfermeriaHist = (from historial in db.EnfermeriaSolicitudDetalle
                                  join solicitud in db.EnfermeriaSolicitud on historial.SolicitudId equals solicitud.SolicitudId
                                  join med in db.MEDICOS on solicitud.Medico equals med.Numero
                                  where solicitud.NumEmp == expediente
                                  select new
                                  {
                                      Descripcion = historial.Descripcion,
                                      FechaSol = solicitud.FechaSol,
                                      UsuarioRealiza = med.Nombre + " " + med.Apaterno + " " + med.Amaterno,
                                      Urgente = solicitud.Urgente,
                                      Nota = solicitud.Nota
                                  }).ToList();



            Models.SMDEVEntities20 db2 = new Models.SMDEVEntities20();
            var dhabiente = (from a in db2.DHABIENTES
                             where a.NUMEMP == expediente
                             select a).FirstOrDefault();

            List<ExpandoObject> model = new List<ExpandoObject>();

            foreach (var item in enfermeriaHist)
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

            List<ExpandoObject> model2 = new List<ExpandoObject>();

            foreach (var item in enfermeriaSolicitud)
            {
                IDictionary<string, object> itemExpando = new ExpandoObject();
                foreach (PropertyDescriptor property
                         in
                         TypeDescriptor.GetProperties(item.GetType()))
                {
                    itemExpando.Add(property.Name, property.GetValue(item));
                }
                model2.Add(itemExpando as ExpandoObject);
            }

            ViewBag.HistorialEnfermeria = model;
            ViewBag.EnfermeriaSolicitudList = model2;

            return View(dhabiente);
        }

        // GET: EnfermeriaSolicitud/Create

        public ActionResult MenuEnfermeria()
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


        // POST: EnfermeriaSolicitud/Create

        public JsonResult GuardarSolicitudEnfermeria(string newSolEnf, string Detalle)
        {

            List<EnfermeriaSolicitudDetalle> NewEnfDetalle = JsonConvert.DeserializeObject<List<EnfermeriaSolicitudDetalle>>(Detalle);
            EnfermeriaSolicitud NewEnf = JsonConvert.DeserializeObject<EnfermeriaSolicitud>(newSolEnf);

            Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();
            int folio;

            try
            {

                using (SqlCommand cmd = new SqlCommand("insert into EnfermeriaSolicitud(NumEmp, Medico, FechaSol,FechaAgenda,UsuarioAgenda, FechaRealiza, UsuarioRealiza,NotaRealiza,Nota,Urgente,Finalizado,Cancelado,NoPresento) values('" + NewEnf.NumEmp + "', '" + NewEnf.Medico + "', GETDATE(),GETDATE()- GETDATE(),'',GETDATE()- GETDATE(),'','','" + NewEnf.Nota + "', '" + NewEnf.Urgente + "','0','0','0')"))
                {

                    cmd.Connection = new SqlConnection(db.Database.Connection.ConnectionString);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT SCOPE_IDENTITY()";
                    folio = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Connection.Close();
                }

                foreach (var item in NewEnfDetalle)
                {

                    {
                        db.Database.ExecuteSqlCommand("insert into EnfermeriaSolicitudDetalle (SolicitudId, IdServicio,Descripcion, FechaSol, FechaRealiza,UsuarioRealiza) values ('" + folio + "','" + item.IdServicio + "','" + item.Descripcion + "',GETDATE(),GETDATE()- GETDATE(),'')");
                    }
                }

            }
            catch (Exception ex)
            {


                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult AgendarSolicitudEKG(string Folio, string FechaAgenda, string Nota, string Realizado, string Cancelado, string NoPresento)
        {

            try
            {

                string FechadeAgenda = DateTime.Parse(FechaAgenda).ToString("yyyy/MM/dd HH:mm:ss");
                if (Realizado != "0")
                {
                    Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();
                    db.Database.ExecuteSqlCommand("UPDATE EnfermeriaSolicitud SET FechaAgenda = CONVERT(SMALLDATETIME,'" + FechadeAgenda + "',101),NotaRealiza ='" + Nota + "',Finalizado='" + Realizado + "', UsuarioRealiza='" + User.Identity.Name + "',FechaRealiza=CONVERT(SMALLDATETIME,GETDATE(),101) WHERE SolicitudId=" + Folio);
                }

                if (Cancelado != "0")
                {
                    Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();
                    db.Database.ExecuteSqlCommand("UPDATE EnfermeriaSolicitud SET FechaAgenda = CONVERT(SMALLDATETIME,'" + FechadeAgenda + "',101),NotaRealiza ='" + Nota + "',Cancelado='" + Cancelado + "' WHERE SolicitudId=" + Folio);
                }

                if (NoPresento != "0")
                {
                    Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();
                    db.Database.ExecuteSqlCommand("UPDATE EnfermeriaSolicitud SET FechaAgenda = CONVERT(SMALLDATETIME,'" + FechadeAgenda + "',101),NotaRealiza ='" + Nota + "',NoPresento='" + NoPresento + "' WHERE SolicitudId=" + Folio);
                }

                if(Realizado == "0" & Cancelado == "0" & NoPresento == "0")
                {
                    Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();
                    db.Database.ExecuteSqlCommand("UPDATE EnfermeriaSolicitud SET FechaAgenda = CONVERT(SMALLDATETIME,'" + FechadeAgenda + "',101),UsuarioAgenda= '" + User.Identity.Name + "',NotaRealiza ='" + Nota + "',Finalizado='" + Realizado + "' WHERE SolicitudId=" + Folio);
                }

            }
            catch (Exception ex)
            {

                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Edit(int id)
        {

            Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();
            Models.SMDEVEnfermeriaSolicitudDetalle db2 = new Models.SMDEVEnfermeriaSolicitudDetalle();

            var enfermeriaSolicitud = db.EnfermeriaSolicitud.Where(x => x.SolicitudId == id).FirstOrDefault();

            var solDetalle = db2.EnfermeriaSolicitudDetalle.Where(x => x.SolicitudId == id).ToList();

            Models.SMDEVEntities20 db3 = new Models.SMDEVEntities20();
            var dhabiente = (from a in db3.DHABIENTES
                             where a.NUMEMP == enfermeriaSolicitud.NumEmp
                             select a).FirstOrDefault();

            ViewBag.SolicitudEnfermria = enfermeriaSolicitud;
            ViewBag.SolicitudDetalle = solDetalle;

            return View(dhabiente);
        }

        public ActionResult VerSolicitudEnf(int id)
        {

            Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();
            Models.SMDEVEnfermeriaSolicitudDetalle db2 = new Models.SMDEVEnfermeriaSolicitudDetalle();

            var enfermeriaSolicitud = db.EnfermeriaSolicitud.Where(x => x.SolicitudId == id).FirstOrDefault();

            var solDetalle = db2.EnfermeriaSolicitudDetalle.Where(x => x.SolicitudId == id).ToList();

            Models.SMDEVEntities20 db3 = new Models.SMDEVEntities20();
            var dhabiente = (from a in db3.DHABIENTES
                             where a.NUMEMP == enfermeriaSolicitud.NumEmp
                             select a).FirstOrDefault();

            EnfermeriaAgendar res = new EnfermeriaAgendar();
            res.EnfermeriaSolicitud = enfermeriaSolicitud;
            res.EnfermeriaSolicitudDetalle = solDetalle;
            res.DHABIENTES = dhabiente;

            return View(res);

        }

        public ActionResult Search()
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

            var actividadesSM = (from actividad in db.ActividadesDepartamento
                                 where actividad.Departamento == "Enfermeria" &&
                                 actividad.Ubicacion == "SM"
                                 select actividad).ToList();

            var actividadesLinares = (from actividad in db.ActividadesDepartamento
                                      where actividad.Departamento == "Enfermeria" &&
                                      actividad.Ubicacion == "Linares"
                                      select actividad).ToList();

            var actividadesMederos = (from actividad in db.ActividadesDepartamento
                                      where actividad.Departamento == "Enfermeria" &&
                                      actividad.Ubicacion == "Mederos"
                                      select actividad).ToList();

            ViewBag.ActividadesSM = actividadesSM;
            ViewBag.ActividadesLinares = actividadesLinares;
            ViewBag.ActividadesMederos = actividadesMederos;

            return View(weeks);
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
                        where actividades.Departamento == "Enfermeria"
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


        public ActionResult CapturaSVitales(string expediente)
        {
            Models.SMDEVEntities20 db2 = new Models.SMDEVEntities20();
            var dhabiente = (from a in db2.DHABIENTES
                             where a.NUMEMP == expediente
                             select a).FirstOrDefault();

            return View(dhabiente);
        }

        public ActionResult SearchSignos()
        {
            return View();
        }


        public JsonResult GuardarSV(string numemp, string peso_r, string talla_r, string temp_r, string fresp, string fcard, string ta1, string ta2, string dstx ,string sp2)
        {
            try
            {
                var Signos = new EnfermeriaSignosVitales{

                    numemp = numemp,
                    peso_r = Convert.ToInt32(peso_r),
                    talla_r = Convert.ToInt32(talla_r),
                    temp_r = Convert.ToInt32(temp_r),
                    fresp = fresp,
                    fcard = fcard,
                    ta1 = ta1,
                    ta2 = ta2,
                    dstx = dstx,
                    sp2 = sp2,
                    UsuarioCaptura = User.Identity.Name,
                    FechaCaptura = DateTime.Now
                };

                SMDEVEnfermeriaSignosVitales db = new SMDEVEnfermeriaSignosVitales();
                db.EnfermeriaSignosVitales.Add(Signos);
                db.SaveChanges();
            }

            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetReporte(int FechaInicio, int FechaFin)
        {
            Models.SMDEVReporteSemanalActividades db = new Models.SMDEVReporteSemanalActividades();

            var list = (from reporte in db.ReporteActividadesSemanal
                        join actividades in db.ActividadesDepartamento
                        on reporte.ActividadId equals actividades.ActividadId
                        where reporte.Semana >= FechaInicio &&
                        reporte.Semana <= FechaFin
                        where actividades.Departamento == "Enfermeria"
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
                        where actividades.Departamento == "Enfermeria"
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

        public JsonResult ActualizarSolicitudEnfermeria(int Folio, string Detalle)
        {
            List<EnfermeriaSolicitudDetalle> NewEnfDetalle = JsonConvert.DeserializeObject<List<EnfermeriaSolicitudDetalle>>(Detalle);

            Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();
            foreach (var item in NewEnfDetalle)
            {

                {
                    db.Database.ExecuteSqlCommand("insert into EnfermeriaSolicitudDetalle (SolicitudId, IdServicio,Descripcion, FechaSol, FechaRealiza,UsuarioRealiza) values ('" + Folio + "','" + item.IdServicio + "','" + item.Descripcion + "',GETDATE(),GETDATE(),'" + User.Identity.Name + "')");
                }
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetEnfermeriaCatalogo(string search)
        {

            Models.SMDEVEnfermeriaSolicitudDetalle db = new Models.SMDEVEnfermeriaSolicitudDetalle();

            var catalogo = (from enfermeria in db.ServicioEnfermeria
                            where (enfermeria.Nombre.Contains(search))
                            select new { value = enfermeria.Clave, label = enfermeria.Nombre, }).ToList();


            return new JsonResult { Data = catalogo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult DeleteSolicitud(string id)
        {
            Models.SMDEVEntitiesRayosX db = new Models.SMDEVEntitiesRayosX();

            try
            {
                db.Database.ExecuteSqlCommand("delete from EnfermeriaSolicitud where SolicitudId=" + id + ";");
                db.Database.ExecuteSqlCommand("delete from EnfermeriaSolicitudDetalle where SolicitudId=" + id + ";");

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

    }

    public class Semana
    {
        DateTime fi;
        DateTime ff;

        public DateTime FechaInicial
        {
            get { return fi; }
            private set { fi = value; }
        }

        public DateTime FechaFinal
        {
            get { return ff; }
            private set { ff = value; }
        }


        public Semana(int num)
        {
            if (num < 1 || num > 53)
            {
                throw new ArgumentException("En número de la semana debe estar comprendido en el rango [1..53]");
            }

            var jan1 = new DateTime(DateTime.Today.Year, 1, 1);
            //beware different cultures, see other answers
            var startOfFirstWeek = jan1.AddDays(1 - (int)(jan1.DayOfWeek));

            FechaInicial = startOfFirstWeek.AddDays(num * 7);
            FechaFinal = FechaInicial.AddDays(6);
        }
    }

    public class SemanasList
    {
        public string Semana { get; set; }
        public int SemanaId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

    }
}
