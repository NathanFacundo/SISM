using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class TrabajoSocialController : Controller
    {
        // GET: TrabajoSocial
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
                        where actividades.Departamento == "TrabajoSocialDF"
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
                        where actividades.Departamento == "TrabajoSocialDF"
                        select new { reporte, actividades }).ToList();

            List<ReportedeActividades> res = new List<ReportedeActividades>();
            foreach (var item in list)
            {
                var newitem = new ReportedeActividades();
                newitem.ActividadesDepartamento = item.actividades;
                newitem.ReporteActividadesSemanal = item.reporte;
                res.Add(newitem);
            }

            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                               where actividad.Departamento == "TrabajoSocialDF"
                               select actividad).ToList();

            ViewBag.Actividades = actividades;

            return View(weeks);
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
                        where actividades.Departamento == "TrabajoSocialDF"
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
                                    && actividad.FechaInicio.Year == DateTime.Now.Year
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


        public ActionResult CapturarNota()
        {
            if (User.IsInRole("TrabajoSocial")) {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult GuardarNota(string numexp, string nota, int id)
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();

            //System.Diagnostics.Debug.WriteLine(id);

            if (id == 0)
            {
                db.Database.ExecuteSqlCommand("INSERT INTO TrabajoSocialNota (nota, fecha, usuario, num_exp, estado) VALUES('" + nota + "','" + fecha + "','" + username + "','" + numexp + "', 1)");
                TempData["message_success"] = "Nota registrada con éxito";
            }
            else
            {
                db.Database.ExecuteSqlCommand("UPDATE TrabajoSocialNota SET nota = '" + nota + "' WHERE id = '" + id + "'");
                TempData["message_success"] = "Nota editada con éxito";
            }

            return Redirect(Request.UrlReferrer.ToString());
        }


        public class BuscarDhabList
        {
            public string numemp { get; set; }
            public string numexp { get; set; }
            public string paciente { get; set; }
            public string edad { get; set; }
            public string sexo { get; set; }
            public string info { get; set; }
            public string fecha { get; set; }

        }


        public JsonResult RegistrosNotas()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.AddDays(-7).ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();

            var pruebaAn = (from a in db.TrabajoSocialNota
                            join dhabientes in db.DHABIENTES on a.num_exp equals dhabientes.NUMEMP into dhabientesX
                            from dhabientesIn in dhabientesX.DefaultIfEmpty()
                            where a.usuario == username
                            where a.fecha >= fechaDT
                            select new
                            {
                                paciente = dhabientesIn.NOMBRES + " " + dhabientesIn.APATERNO + " " + dhabientesIn.AMATERNO,
                                fvigencia = dhabientesIn.FVIGENCIA,
                                //fnac = d.FNAC,
                                //sexo = d.SEXO,
                                id = a.id,
                                nota = a.nota,
                                numexp = a.num_exp,
                                fecha = a.fecha,
                                numemp = dhabientesIn.NUMAFIL,
                                fnac = dhabientesIn.FNAC,
                                sexo = dhabientesIn.SEXO,
                                //numemp = d.NUMAFIL,

                            })
                          .OrderByDescending(g => g.id)
                          .ToList();




            var results1 = new List<BuscarDhabList>();

            foreach (var item in pruebaAn)
            {

                var today = DateTime.Today;
                DateTime fnac = (DateTime)item.fnac;
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


                var fechaHoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fechaHoyDT = DateTime.Parse(fechaHoy);

                var vigencia = "";

                if (fechaHoyDT > item.fvigencia)
                {
                    vigencia = "Vigencia vencida";
                }
                else
                {
                    vigencia = "Vigente";
                }


                //System.Diagnostics.Debug.WriteLine(tipo);


                var resultado = new BuscarDhabList
                {
                    //numemp = dh.NUMEMP + "*" + dhab.paciente + "*" + edad + "*" + dhab.sexo,
                    paciente = item.paciente,
                    numexp = item.numexp,
                    numemp = item.numemp,
                    edad = Years + " años con " + Months + " meses",
                    sexo = item.sexo,
                    fecha = string.Format("{0:yyyy-MM-dd}", item.fecha),
                    info = item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia) + "*" + item.nota + "*" + item.id,
                };

                results1.Add(resultado);
            }


            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult HUPacientes()
        {
            if (User.IsInRole("AltaUrgencias"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
                
        }

        public class CitaLista
        {
            public string paciente { get; set; }
            public string medico { get; set; }
            public string fecha_ingreso { get; set; }
            public string boton { get; set; }

        }

        public JsonResult Citas()
        {
            var nombreusuario = User.Identity.GetUserName();

            //Models.SERVMEDEntities2 db = new Models.SERVMEDEntities2();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var citasUrg = (from d in db.AltaUrgencias
                            join dhab in db.DHABIENTES on d.numexp equals dhab.NUMEMP into dhabX
                            from dhabIn in dhabX.DefaultIfEmpty()
                            join medicos in db.MEDICOS on d.medico equals medicos.Numero into medicosX
                            from medicosIn in medicosX.DefaultIfEmpty()
                            where d.usuario_registra == nombreusuario
                            select new
                            {
                                medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                fecha_ingreso = d.fecha_ingreso,
                                id = d.id,
                            })
                            .ToList();

            var results1 = new List<CitaLista>();

            foreach (var cturg in citasUrg)
            {

                var resultado = new CitaLista
                {
                    medico = cturg.medico,
                    paciente = cturg.paciente,
                    fecha_ingreso = string.Format("{0:yyyy-MM-dd}", cturg.fecha_ingreso),
                    boton = "<button class='btn btn-asignar' id='" + cturg.id + "'>Editar</button>",

                };

                results1.Add(resultado);
            }


            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult GuardarCitas(string motivo, string numexp, string medico, string fecha_ingreso, int modificar)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);
            //System.Diagnostics.Debug.WriteLine(fecha_ingreso);
            var fecha2 = DateTime.Now.ToString(fecha_ingreso + "THH:mm:ss.fff");
            var fechaDT2 = DateTime.Parse(fecha2);
            var username = User.Identity.GetUserName();
            var ip_realiza = Request.UserHostAddress;

            if (modificar == 0)
            {
                AltaUrgencias altaurg = new AltaUrgencias();
                altaurg.numexp = numexp;
                altaurg.medico = medico;
                altaurg.fecha_ingreso = fechaDT2;
                altaurg.fecha_registro = fechaDT;
                altaurg.motivo = motivo;
                altaurg.usuario_registra = username;
                altaurg.ip_realiza = ip_realiza;
                db.AltaUrgencias.Add(altaurg);
                db.SaveChanges();

                TempData["message_success"] = "Registro agregado con éxito";
            }
            else
            {
                ///System.Diagnostics.Debug.WriteLine(modificar);
                db.Database.ExecuteSqlCommand("UPDATE AltaUrgencias SET numexp = '" + numexp + "', motivo = '" + motivo + "', medico = '" + medico + "', fecha_ingreso = '" + fecha2 + "', fecha_registro = '" + fecha + "', ip_realiza = '" + ip_realiza + "' WHERE id = '" + modificar + "'");
                TempData["message_success"] = "Registro editado con éxito";
            }

            return Redirect(Request.UrlReferrer.ToString());
        }


        public JsonResult MisMedicos()
        {
            var nombreusuario = User.Identity.GetUserName();

            //Models.SERVMEDEntities2 db = new Models.SERVMEDEntities2();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var mismedicos = (from d in db.Citas_Medicos_Externos
                              join medicos in db.MEDICOS on d.medico equals medicos.Numero into medicosX
                              from medicosIn in medicosX.DefaultIfEmpty()
                              where d.usuario == nombreusuario
                              select new
                              {
                                  Medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  Numero = medicosIn.Numero,
                              })
                              .ToList();

            //System.Diagnostics.Debug.WriteLine(tipoConsulta);

            return new JsonResult { Data = mismedicos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult UrgenciasIngresoDts(int id)
        {
            var nombreusuario = User.Identity.GetUserName();

            //Models.SERVMEDEntities2 db = new Models.SERVMEDEntities2();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var citasUrg = (from d in db.AltaUrgencias
                            join dhab in db.DHABIENTES on d.numexp equals dhab.NUMEMP into dhabX
                            from dhabIn in dhabX.DefaultIfEmpty()
                            join medicos in db.MEDICOS on d.medico equals medicos.Numero into medicosX
                            from medicosIn in medicosX.DefaultIfEmpty()
                            where d.id == id
                            select new
                            {
                                medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                numexp = dhabIn.NUMEMP,
                                fnac = dhabIn.FNAC,
                                sexo = dhabIn.SEXO,
                                fecha_ingreso = d.fecha_ingreso,
                                motivo = d.motivo,
                                id = d.id,
                                numero_medico = d.medico,

                            })
                            .FirstOrDefault();




            if(citasUrg != null) {


                var today = DateTime.Today;
                DateTime fnac = (DateTime)citasUrg.fnac;
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

                var result = new Object();

                result = new
                {
                    numero_medico = citasUrg.numero_medico,
                    medico = citasUrg.medico,
                    paciente = citasUrg.paciente,
                    numexp = citasUrg.numexp,
                    motivo = citasUrg.motivo,
                    id = citasUrg.id,
                    edad = Years,
                    sexo = citasUrg.sexo,
                    fecha_ingreso = string.Format("{0:yyyy-MM-dd}", citasUrg.fecha_ingreso),

                };

                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                var result = "";
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


            
        }

    }
}
