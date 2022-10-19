using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using UanlSISM.Models;
using UanlSISM.ViewModels;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class ServiciosMedicosController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        // GET: ServiciosMedicos


        public ActionResult Index()
        {

            //System.Diagnostics.Debug.WriteLine(createWebService());

            if (User.IsInRole("SinAgenda"))
            {

                return View();

            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    //System.Diagnostics.Debug.WriteLine(User.Identity.GetUserName());
                    return RedirectToAction("Citas", "ServiciosMedicos");
                }
                else
                {
                    if (User.IsInRole("Subrogados"))
                    {
                        return RedirectToAction("Subrogados", "ServiciosMedicos");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

            }
        }


        public JsonResult CambiarModalidad(string modalidad)
        {
            //System.Diagnostics.Debug.WriteLine(modalidad);

            var userStore = new UserStore<IdentityUser>();
            var manager = new UserManager<IdentityUser>(userStore);

            IdentityUser user = manager.FindById(User.Identity.GetUserId());
            //Get the authentication manager
            var authenticationManager = HttpContext.GetOwinContext().Authentication;

            if (modalidad == "Con agenda")
            {
                manager.RemoveFromRole(user.Id, "SinAgenda");
                manager.AddToRole(user.Id, "ServiciosMedicos");
            }
            else
            {
                manager.RemoveFromRole(user.Id, "ServiciosMedicos");
                manager.AddToRole(user.Id, "SinAgenda");
            }


            //Log the user out
            authenticationManager.SignOut();

            //Log the user back in
            var identity = manager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new Microsoft.Owin.Security.AuthenticationProperties() { IsPersistent = true }, identity);


            var result = "";
            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Subrogados()
        {
            return View();
        }

        public ActionResult Subrogados2()
        {
            return View();
        }

        public ActionResult Calendario()
        {
            if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
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

        public ActionResult Index2()
        {
            if (User.IsInRole("SinAgenda"))
            {
                return View();
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    //System.Diagnostics.Debug.WriteLine(User.Identity.GetUserName());
                    return RedirectToAction("Citas", "ServiciosMedicos");
                }
                else
                {
                    if (User.IsInRole("Subrogados"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

            }
        }

        public JsonResult BuscarDhabientes(string numemp, string numexp, string nombre, string apaterno, string amaterno)
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

                    if (User.IsInRole("Subrogados"))
                    {
                        var fecha = DateTime.Now.AddYears(-1).ToString("yyyy-MM-ddT00:00:00.000");
                        string querySub = "select numemp as numemp FROM servicio_prestaciones WHERE numemp = '" + dhab.numexp + "' and fecha_sol >= '" + fecha + "' ";
                        var resultSub = db.Database.SqlQuery<ServicioPrestaciones>(querySub);
                        var resSub = resultSub.FirstOrDefault();

                        var vigencia = "";

                        if (fechaHoyDT > dhab.fvigencia)
                        {
                            vigencia = "Vigencia vencida";
                        }
                        else
                        {
                            vigencia = "Vigente";
                        }

                        if (resSub != null)
                        {
                            var resultado = new BuscarDhabList
                            {
                                //numemp = dh.NUMEMP + "*" + dhab.paciente + "*" + edad + "*" + dhab.sexo,
                                paciente = dhab.paciente,
                                numexp = dhab.numexp,
                                numemp = dhab.numemp,
                                edad = Years + " años con " + Months + " meses" + " y " + Days + " días",
                                sexo = dhab.sexo,
                                info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + " y " + Days + " días" + "*" + dhab.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", dhab.fvigencia),
                            };

                            results1.Add(resultado);
                        }

                    }
                    else
                    {
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
                            edad = Years + " años con " + Months + " meses" + " y " + Days + " días",
                            sexo = dhab.sexo,
                            info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + " y " + Days + " días" + "*" + dhab.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", dhab.fvigencia),
                        };

                        results1.Add(resultado);
                    }

                }
            }

            //System.Diagnostics.Debug.WriteLine(results1);

            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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
                    var prueCovid = (from d in db2.CovidSolicitud
                                     where d.NumEmp == dh.NUMEMP
                                     //where d.Resultado == null
                                     //where r.fecha_crea >= fecha_correcta
                                     //orderby r.fecha_crea descending
                                     select d)
                                     .OrderByDescending(g => g.SolicitudId)
                                     .FirstOrDefault();


                    //System.Diagnostics.Debug.WriteLine(prueCovid.Tipo);


                    if (prueCovid != null)
                    {
                        var vigencia = "";

                        if (fechaHoyDT > dhab.fvigencia)
                        {
                            vigencia = "Vigencia vencida";
                        }
                        else
                        {
                            vigencia = "Vigente";
                        }


                        var tipo = "";


                        if (prueCovid.Tipo == "Antigenos")
                        {
                            tipo = "PASE PARA PRUEBA DE ANTIGENOS COVID 19 - Antígenos";
                        }
                        else
                        {
                            if (prueCovid.Tipo == "AEMA")
                            {
                                tipo = "PASE A AEMA";
                            }
                            else
                            {
                                if (prueCovid.Tipo == "UER")
                                {
                                    tipo = "PASE A LA UNIDAD DE ENFERMEDADES RESPIRATORIAS DE S.M";
                                }
                                else
                                {
                                    if (prueCovid.Tipo == "2")
                                    {
                                        tipo = "PASE PARA PRUEBA DE ANTIGENOS COVID 19 - Antígenos y UER";
                                    }
                                }
                            }
                        }


                        //System.Diagnostics.Debug.WriteLine(tipo);


                        var resultado = new BuscarDhabList
                        {
                            //numemp = dh.NUMEMP + "*" + dhab.paciente + "*" + edad + "*" + dhab.sexo,
                            paciente = dhab.paciente,
                            numexp = dhab.numexp,
                            numemp = dhab.numemp,
                            edad = Years + " años con " + Months + " meses",
                            sexo = dhab.sexo,
                            //info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + "*" + dhab.sexo + "*" + prueCovid.SolicitudId + "*" + string.Format("{0:dddd, dd MMMM yyyy}", prueCovid.FechaSol),
                            info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + "*" + dhab.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", dhab.fvigencia) + "*" + tipo + "*" + prueCovid.SolicitudId,
                            //info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + "*" + dhab.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", dhab.fvigencia),
                        };

                        results1.Add(resultado);
                    }


                }
            }

            //System.Diagnostics.Debug.WriteLine(results1);

            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult BuscarDhabPrestaciones(string numemp, string numexp, string nombre, string apaterno, string amaterno)
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
                    var solSer = (from q in db2.SolicitudPrestaciones
                                  where q.num_exp == dh.NUMEMP
                                  where q.estado == 1
                                  join medicos in db2.MEDICOS on q.medico equals medicos.Numero into medicosX
                                  from medicosIn in medicosX.DefaultIfEmpty()
                                  join serv in db2.prestaciones_servicios on q.id_servicio equals serv.id into servX
                                  from servIn in servX.DefaultIfEmpty()
                                  join subserv in db2.prestaciones_subservicios on q.id_subservicio equals subserv.id into subservX
                                  from subservIn in subservX.DefaultIfEmpty()
                                  select new
                                  {
                                      usuario = q.medico,
                                      medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                      servicio = servIn.servicio,
                                      subservicio = subservIn.subservicio,
                                      fecha = q.fecha,
                                      id = q.id,
                                      folio = q.folio,
                                  })
                                  .OrderByDescending(g => g.fecha)
                                  .FirstOrDefault();


                    //System.Diagnostics.Debug.WriteLine(prueCovid.Tipo);


                    if (solSer != null)
                    {
                        var vigencia = "";

                        if (fechaHoyDT > dhab.fvigencia)
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
                            paciente = dhab.paciente,
                            numexp = dhab.numexp,
                            numemp = dhab.numemp,
                            edad = Years + " años con " + Months + " meses",
                            sexo = dhab.sexo,
                            //info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + "*" + dhab.sexo + "*" + prueCovid.SolicitudId + "*" + string.Format("{0:dddd, dd MMMM yyyy}", prueCovid.FechaSol),
                            info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + "*" + dhab.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", dhab.fvigencia) + "*" + solSer.id + "*" + solSer.folio + "*" + solSer.servicio + "*" + solSer.subservicio,
                            //info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + "*" + dhab.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", dhab.fvigencia),
                        };

                        results1.Add(resultado);
                    }


                }
            }

            //System.Diagnostics.Debug.WriteLine(results1);

            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        public JsonResult ConsultasHoy()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();


            var expHoy = (from a in db.expediente
                          join dhabientes in db.DHABIENTES on a.numemp equals dhabientes.NUMEMP into dhabientesX
                          from dhabientesIn in dhabientesX.DefaultIfEmpty()
                          where a.medico == username
                          where a.fecha == fechaDT
                          select new
                          {
                              paciente = dhabientesIn.NOMBRES + " " + dhabientesIn.APATERNO + " " + dhabientesIn.AMATERNO,
                              //fnac = d.FNAC,
                              //sexo = d.SEXO,
                              numexp = a.numemp,
                              //numemp = d.NUMAFIL,

                          }).ToList();

            /*var results = new List<ResurtimientoList>();

            foreach (var item in expHoy)
            {

                var result = new ResurtimientoList
                {
                    recetaNorCount = item.recetaNorCount,
                    folio_rc = item.folio_rc,
                    codigo = item.codigo,
                    cantidad = item.cantidad,
                    dosis = item.dosis,
                    indicaciones = item.indicaciones,
                    meses_surtir = item.meses_surtir,
                    fec_ini_sintomas = string.Format("{0:yyyy-MM-dd}", item.fec_ini_sintomas),
                    fecha_crea = string.Format("{0:yyyy-MM-dd}", item.fecha_crea),
                    terminada = item.terminada
                };

                results.Add(result);

            }*/


            return new JsonResult { Data = expHoy, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Consultahistorial_incapacidades(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DalHojaFrontal hf = new DalHojaFrontal();
            HojaFrontal hoja = hf.BuscarTitular(id);

            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabientes = (from a in db.DHABIENTES
                              where a.NUMEMP == id
                              select a).FirstOrDefault();

            ViewData["hoja"] = hoja;
            return View(dhabientes);
        }


        public ActionResult BsqCalendario()
        {
            var today = DateTime.Today.Year;

            DalHojaFrontal dalObj = new DalHojaFrontal();

            return new JsonResult { Data = dalObj.BuscarCalendario(User.Identity.Name), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult llamar(string reg)
        {
            //var ip_realiza = GetLocalIPAddress();
            var ip_realiza = Request.UserHostAddress;

            //System.Diagnostics.Debug.WriteLine(ip_realiza);
            DalHojaFrontal dalObj = new DalHojaFrontal();

            return new JsonResult { Data = dalObj.Llamar(reg, User.Identity.Name, ip_realiza), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult agenda1()
        {
            return View();
        }

        public ActionResult citas()
        {
            //Session["user"] = User.Identity.Name;

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            db.Database.ExecuteSqlCommand("UPDATE CITAS SET hora_recepcion = null WHERE Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha + "' and hora_recepcion = ''");
            //System.Diagnostics.Debug.WriteLine(fecha);
            //Actualizar si no tiene hora_recepcion y ya paso 1 hora

            string query = "select CITAS.Id as id, CITAS.Tipo as tipo, CITAS.hora_recepcion as hora_recepcion, CITAS.Medico as Medico, CITAS.NumEmp as NumEmp, CITAS.turno as turno, CITAS.turno as turno FROM CITAS WHERE CITAS.Medico = '" + User.Identity.GetUserName() + "' and CITAS.Fecha = '" + fecha + "' and CITAS.hora_recepcion is null and CITAS.hr_consultorio is null and (Tipo != '13' and Tipo != '25' and Tipo != '12' and Tipo != '01' and Tipo != '02' and Tipo != '08' and Tipo != '09')";
            var result = db.Database.SqlQuery<Citas>(query);
            var res = result.ToList();

            //System.Diagnostics.Debug.WriteLine(res);

            foreach (var item in res)
            {
                var turno1 = item.turno.Substring(0, 2);
                var turno2 = item.turno.Substring(2, 2);

                var turno = DateTime.Now.ToString("yyyy-MM-ddT" + turno1 + ":" + turno2 + ":ss");
                var turnoDT = DateTime.Parse(turno);

                var fechaUnaHoraMenos = DateTime.Now.AddHours(-1).ToString("yyyy-MM-ddTHH:mm:ss");
                var fechaUnaHoraMenosDT = DateTime.Parse(fechaUnaHoraMenos);

                //System.Diagnostics.Debug.WriteLine(item.tipo);

                if (item.NumEmp != null || item.NumEmp != "")
                {

                    if (item.tipo != "13" || item.tipo != "25" || item.tipo != "12" || item.tipo != "01" || item.tipo != "02" || item.tipo != "08" || item.tipo != "09")
                    {
                        //Si ya se paso, se cancela la cita
                        if (fechaUnaHoraMenosDT > turnoDT)
                        {
                            db.Database.ExecuteSqlCommand("UPDATE CITAS SET Tipo = '00' WHERE NumEmp = '" + item.NumEmp + "' and id = " + item.id + " and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha + "' and (Tipo != '13' and Tipo != '25' and Tipo != '12' and Tipo != '01' and Tipo != '02' and Tipo != '08' and Tipo != '09') and hr_consultorio is null");
                        }
                    }

                }



            }


            return View();
        }

        /*public ActionResult BsqCalendario()
        {
            var today = DateTime.Today.Year; 

            DalHojaFrontal dalObj = new DalHojaFrontal(); 

            return new JsonResult { Data = dalObj.BuscarCalendario(User.Identity.Name ), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult agenda1()
        {
            return View();
        }
        
        public ActionResult citas()
        {
            Session["user"] = User.Identity.Name;



            return View();
        }*/

        [HttpGet]
        //SOAP en servicios médicos
        public ActionResult SOAP(string id)
        {
            //LINEAS DE MILTON
            SMDEVTriage dbMilton = new SMDEVTriage();
            var fechaMilton = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
            var fechaHoyMilton = DateTime.Parse(fechaMilton);

            //var soapTriage = (from a in dbMilton.TblTriage
            //            where a.Id > 0 
            //            select a).FirstOrDefault();
            //ViewData["Triage"] = soapTriage;

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Revisar si ya termino el SOAP 
                /*var fechaSOAP = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                var fechaSOAPDT = DateTime.Parse(fechaSOAP);
                var username = User.Identity.GetUserName();
                Models.SMDEVEntities33 db6 = new Models.SMDEVEntities33();
                var expRevisar = (from a in db6.expediente
                                  where a.numemp == id
                                  where a.medico == username
                                  where a.fecha == fechaSOAPDT
                                  where a.hora_termino != null
                                  select a).FirstOrDefault();

                if (expRevisar == null)
                {*/

                if (User.Identity.GetUserName() == "02290" || User.Identity.GetUserName() == "02305" || User.Identity.GetUserName() == "02294")
                {
                    return RedirectToAction("SOAP2/" + id, "ServiciosMedicos");
                }
                else
                {
                    if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                    {
                        var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                        Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                        string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "' and Tipo != '00'";
                        var result = db4.Database.SqlQuery<Citas>(query);
                        var res = result.FirstOrDefault();

                        //System.Diagnostics.Debug.WriteLine(res);

                        //LINEAS DE MILTON
                        //dbMilton.Database.ExecuteSqlCommand("UPDATE TblTriage SET PasarASoap = CONVERT(VARCHAR(5),getdate(),108) WHERE Expediente = '" + id + "' and Fecha = '" + fechaMilton + "'");
                        //var soap = (from a in dbMilton.TblTriage
                        //            where a.Expediente == id &&
                        //            a.Fecha >= fechaHoyMilton
                        //            select a).FirstOrDefault();


                        //if (soap != null) {
                        //    if (soap.PasarASoap == null)
                        //    {
                        //        soap.PasarASoap = "1";
                        //        dbMilton.SaveChanges();
                        //    }
                        //}


                        if (res != null)
                        {
                            //Si es presencial
                            if (res.tipo == "11")
                            {
                                //System.Diagnostics.Debug.WriteLine(res.hr_llamado);
                                //Si no ha llegado el paciente a recepcion o no lo haz llamado
                                if (res.hora_recepcion == null || res.hr_llamado == null)
                                {
                                    //System.Diagnostics.Debug.WriteLine(res.hora_recepcion);

                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " es presencial, haz clic en el boton llamar antes de iniciar la nota médica";
                                    return RedirectToAction("Citas", "ServiciosMedicos");
                                    //return RedirectToAction("Index", "Home");
                                }
                                //Si ya llego
                                else
                                {
                                    //Se va a actualizar citas
                                    var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                    //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                                    if (res.hr_consultorio == null)
                                    {
                                        db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "' and Tipo != '00' and fecha_kiosco is not null");
                                    }

                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                                    var diagnosticos = (from q in db2.DIAGNOSTICOS
                                                        where q.DescCompleta != null
                                                        where q.Clave != null
                                                        || q.Clave != ""
                                                        select new
                                                        {
                                                            CheckBox = "",
                                                            Clave = q.Clave,
                                                            DescCompleta = q.DescCompleta,
                                                            DesCorta = q.DesCorta
                                                        })
                                                        //.Skip(1)
                                                        .ToList();

                                    //System.Diagnostics.Debug.WriteLine(diagnosticos);

                                    string json = JsonConvert.SerializeObject(diagnosticos);
                                    string path = Server.MapPath("~/json/");
                                    System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                                    Models.SMDEVEntities18 db5 = new Models.SMDEVEntities18();

                                    var especialidades = (from q in db5.ESPECIALIDADES
                                                          select new
                                                          {
                                                              clave = q.CLAVE,
                                                              descripcion = q.DESCRIPCION,
                                                          })
                                                         .OrderBy(g => g.descripcion)
                                                        .ToList();

                                    string jsonEsp = JsonConvert.SerializeObject(especialidades);
                                    string pathEsp = Server.MapPath("~/json/");
                                    System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                                    return View(dhabientes);
                                }


                            }
                            //Si no es presencial entonces es telefonica
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                                if (res.hr_consultorio == null)
                                {
                                    db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                                }

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                                var diagnosticos = (from q in db2.DIAGNOSTICOS
                                                    where q.DescCompleta != null
                                                    where q.Clave != null
                                                    || q.Clave != ""
                                                    select new
                                                    {
                                                        CheckBox = "",
                                                        Clave = q.Clave,
                                                        DescCompleta = q.DescCompleta,
                                                        DesCorta = q.DesCorta
                                                    })
                                        .ToList();

                                string json = JsonConvert.SerializeObject(diagnosticos);
                                string path = Server.MapPath("~/json/");
                                System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                                Models.SMDEVEntities18 db5 = new Models.SMDEVEntities18();

                                var especialidades = (from q in db5.ESPECIALIDADES
                                                      select new
                                                      {
                                                          clave = q.CLAVE,
                                                          descripcion = q.DESCRIPCION,
                                                      })
                                                      .OrderBy(g => g.descripcion)
                                                    .ToList();

                                string jsonEsp = JsonConvert.SerializeObject(especialidades);
                                string pathEsp = Server.MapPath("~/json/");
                                System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                                return View(dhabientes);


                            }

                        }
                        else
                        {
                            string query2 = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                            var result2 = db4.Database.SqlQuery<Citas>(query2);
                            var res2 = result2.FirstOrDefault();

                            if (res2 != null)
                            {


                                if (res2.tipo == "00")
                                {
                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " fue cancelada";
                                    return RedirectToAction("Citas", "ServiciosMedicos");
                                }
                                else
                                {
                                    //No tiene o llegaron tarde
                                    return RedirectToAction("Index", "Home");
                                }

                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }

                        }



                    }
                    else
                    {
                        if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                        {
                            //LINEAS DE MILTON
                            //dbMilton.Database.ExecuteSqlCommand("UPDATE TblTriage SET PasarASoap = CONVERT(VARCHAR(5),getdate(),108) WHERE Expediente = '" + id + "' and Fecha = '" + fechaMilton + "'");
                            //var soap = (from a in dbMilton.TblTriage
                            //            where a.Expediente == id &&
                            //            a.Fecha >= fechaHoyMilton
                            //            select a).FirstOrDefault();


                            //if (soap != null)
                            //{
                            //    if (soap.PasarASoap == null)
                            //    {
                            //        soap.PasarASoap = "1";
                            //        dbMilton.SaveChanges();
                            //    }
                            //}

                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                            var diagnosticos = (from q in db2.DIAGNOSTICOS
                                                where q.DescCompleta != null
                                                where q.Clave != null
                                                || q.Clave != ""
                                                select new
                                                {
                                                    CheckBox = "",
                                                    Clave = q.Clave,
                                                    DescCompleta = q.DescCompleta,
                                                    DesCorta = q.DesCorta
                                                })
                                    .ToList();

                            string json = JsonConvert.SerializeObject(diagnosticos);
                            string path = Server.MapPath("~/json/");
                            System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                            Models.SMDEVEntities18 db5 = new Models.SMDEVEntities18();

                            var especialidades = (from q in db5.ESPECIALIDADES
                                                  select new
                                                  {
                                                      clave = q.CLAVE,
                                                      descripcion = q.DESCRIPCION,
                                                  })
                                                  .OrderBy(g => g.descripcion)
                                                .ToList();

                            string jsonEsp = JsonConvert.SerializeObject(especialidades);
                            string pathEsp = Server.MapPath("~/json/");
                            System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                            return View(dhabientes);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                }


                /*}
                else
                {
                    TempData["message_warning"] = "";
                    return RedirectToAction("Citas", "ServiciosMedicos");
                }*/

            }
        }


        [HttpGet]
        //SOAP en servicios médicos
        public ActionResult SOAP2(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                /*var especialidad = User.Identity.GetUserName().Substring(0, 2);

                if (especialidad == "03" || User.Identity.GetUserName() == "02101" || User.Identity.GetUserName() == "22011" || User.Identity.GetUserName() == "26009" || User.Identity.GetUserName() == "50022" || User.Identity.GetUserName() == "03129" || User.Identity.GetUserName() == "26001" || User.Identity.GetUserName() == "26007" || User.Identity.GetUserName() == "21010" || User.Identity.GetUserName() == "21007" || User.Identity.GetUserName() == "21002" || User.Identity.GetUserName() == "22017")
                {
                    return RedirectToAction("SOAP2/" + id, "ServiciosMedicos");
                }
                else
                {*/
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "' and Tipo != '00'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    //System.Diagnostics.Debug.WriteLine(res);

                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //System.Diagnostics.Debug.WriteLine(res.hr_llamado);
                            //Si no ha llegado el paciente a recepcion o no lo haz llamado
                            if (res.hora_recepcion == null || res.hr_llamado == null)
                            {
                                //System.Diagnostics.Debug.WriteLine(res.hora_recepcion);

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " es presencial, haz clic en el boton llamar antes de iniciar la nota médica";
                                return RedirectToAction("Citas", "ServiciosMedicos");
                                //return RedirectToAction("Index", "Home");
                            }
                            //Si ya llego
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                                if (res.hr_consultorio == null)
                                {
                                    db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "' and Tipo != '00' and fecha_kiosco is not null");
                                }

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                                var diagnosticos = (from q in db2.DIAGNOSTICOS
                                                    where q.DescCompleta != null
                                                    where q.Clave != null
                                                    || q.Clave != ""
                                                    select new
                                                    {
                                                        CheckBox = "",
                                                        Clave = q.Clave,
                                                        DescCompleta = q.DescCompleta,
                                                        DesCorta = q.DesCorta
                                                    })
                                                    //.Skip(1)
                                                    .ToList();

                                //System.Diagnostics.Debug.WriteLine(diagnosticos);

                                string json = JsonConvert.SerializeObject(diagnosticos);
                                string path = Server.MapPath("~/json/");
                                System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                                Models.SMDEVEntities18 db5 = new Models.SMDEVEntities18();

                                var especialidades = (from q in db5.ESPECIALIDADES
                                                      select new
                                                      {
                                                          clave = q.CLAVE,
                                                          descripcion = q.DESCRIPCION,
                                                      })
                                                     .OrderBy(g => g.descripcion)
                                                    .ToList();

                                string jsonEsp = JsonConvert.SerializeObject(especialidades);
                                string pathEsp = Server.MapPath("~/json/");
                                System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                                return View(dhabientes);
                            }


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");
                            //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                            if (res.hr_consultorio == null)
                            {
                                db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                            }

                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                            var diagnosticos = (from q in db2.DIAGNOSTICOS
                                                where q.DescCompleta != null
                                                where q.Clave != null
                                                || q.Clave != ""
                                                select new
                                                {
                                                    CheckBox = "",
                                                    Clave = q.Clave,
                                                    DescCompleta = q.DescCompleta,
                                                    DesCorta = q.DesCorta
                                                })
                                    .ToList();

                            string json = JsonConvert.SerializeObject(diagnosticos);
                            string path = Server.MapPath("~/json/");
                            System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                            Models.SMDEVEntities18 db5 = new Models.SMDEVEntities18();

                            var especialidades = (from q in db5.ESPECIALIDADES
                                                  select new
                                                  {
                                                      clave = q.CLAVE,
                                                      descripcion = q.DESCRIPCION,
                                                  })
                                                  .OrderBy(g => g.descripcion)
                                                .ToList();

                            string jsonEsp = JsonConvert.SerializeObject(especialidades);
                            string pathEsp = Server.MapPath("~/json/");
                            System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                            return View(dhabientes);


                        }

                    }
                    else
                    {
                        string query2 = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                        var result2 = db4.Database.SqlQuery<Citas>(query2);
                        var res2 = result2.FirstOrDefault();

                        if (res2 != null)
                        {


                            if (res2.tipo == "00")
                            {
                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " fue cancelada";
                                return RedirectToAction("Citas", "ServiciosMedicos");
                            }
                            else
                            {
                                //No tiene o llegaron tarde
                                return RedirectToAction("Index", "Home");
                            }

                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }



                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {
                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();

                        Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                        var diagnosticos = (from q in db2.DIAGNOSTICOS
                                            where q.DescCompleta != null
                                            where q.Clave != null
                                            || q.Clave != ""
                                            select new
                                            {
                                                CheckBox = "",
                                                Clave = q.Clave,
                                                DescCompleta = q.DescCompleta,
                                                DesCorta = q.DesCorta
                                            })
                                .ToList();

                        string json = JsonConvert.SerializeObject(diagnosticos);
                        string path = Server.MapPath("~/json/");
                        System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                        Models.SMDEVEntities18 db5 = new Models.SMDEVEntities18();

                        var especialidades = (from q in db5.ESPECIALIDADES
                                              select new
                                              {
                                                  clave = q.CLAVE,
                                                  descripcion = q.DESCRIPCION,
                                              })
                                              .OrderBy(g => g.descripcion)
                                            .ToList();

                        string jsonEsp = JsonConvert.SerializeObject(especialidades);
                        string pathEsp = Server.MapPath("~/json/");
                        System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                        return View(dhabientes);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                /*}*/
            }
        }


        public ActionResult SOAP3(string id)
        {
            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabientes = (from a in db.DHABIENTES
                              where a.NUMEMP == id
                              select a).FirstOrDefault();

            return View(dhabientes);
        }

        [HttpGet]
        //SOAP en servicios médicos
        public ActionResult SOAPInternamiento(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //Si no ha llegado el paciente a recepcion
                            if (res.hora_recepcion == null)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            //Si ya llego
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                                if (res.hr_consultorio == null)
                                {
                                    db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                                }

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                                var diagnosticos = (from q in db2.DIAGNOSTICOS
                                                    where q.DescCompleta != null
                                                    where q.Clave != null
                                                    || q.Clave != ""
                                                    select new
                                                    {
                                                        CheckBox = "",
                                                        Clave = q.Clave,
                                                        DescCompleta = q.DescCompleta,
                                                        DesCorta = q.DesCorta
                                                    })
                                        .ToList();

                                string json = JsonConvert.SerializeObject(diagnosticos);
                                string path = Server.MapPath("~/json/");
                                System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                                Models.SMDEVEntities18 db5 = new Models.SMDEVEntities18();

                                var especialidades = (from q in db5.ESPECIALIDADES
                                                      select new
                                                      {
                                                          clave = q.CLAVE,
                                                          descripcion = q.DESCRIPCION,
                                                      })
                                                      .OrderBy(g => g.descripcion)
                                                    .ToList();

                                string jsonEsp = JsonConvert.SerializeObject(especialidades);
                                string pathEsp = Server.MapPath("~/json/");
                                System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                                return View(dhabientes);
                            }


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");
                            //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                            if (res.hr_consultorio == null)
                            {
                                db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                            }

                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                            var diagnosticos = (from q in db2.DIAGNOSTICOS
                                                where q.DescCompleta != null
                                                where q.Clave != null
                                                || q.Clave != ""
                                                select new
                                                {
                                                    CheckBox = "",
                                                    Clave = q.Clave,
                                                    DescCompleta = q.DescCompleta,
                                                    DesCorta = q.DesCorta
                                                })
                                    .ToList();

                            string json = JsonConvert.SerializeObject(diagnosticos);
                            string path = Server.MapPath("~/json/");
                            System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                            Models.SMDEVEntities18 db5 = new Models.SMDEVEntities18();

                            var especialidades = (from q in db5.ESPECIALIDADES
                                                  select new
                                                  {
                                                      clave = q.CLAVE,
                                                      descripcion = q.DESCRIPCION,
                                                  })
                                                  .OrderBy(g => g.descripcion)
                                                .ToList();

                            string jsonEsp = JsonConvert.SerializeObject(especialidades);
                            string pathEsp = Server.MapPath("~/json/");
                            System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                            return View(dhabientes);


                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }



                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {
                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();

                        Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                        var diagnosticos = (from q in db2.DIAGNOSTICOS
                                            where q.DescCompleta != null
                                            where q.Clave != null
                                            || q.Clave != ""
                                            select new
                                            {
                                                CheckBox = "",
                                                Clave = q.Clave,
                                                DescCompleta = q.DescCompleta,
                                                DesCorta = q.DesCorta
                                            })
                                .ToList();

                        string json = JsonConvert.SerializeObject(diagnosticos);
                        string path = Server.MapPath("~/json/");
                        System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                        Models.SMDEVEntities18 db5 = new Models.SMDEVEntities18();

                        var especialidades = (from q in db5.ESPECIALIDADES
                                              select new
                                              {
                                                  clave = q.CLAVE,
                                                  descripcion = q.DESCRIPCION,
                                              })
                                              .OrderBy(g => g.descripcion)
                                            .ToList();

                        string jsonEsp = JsonConvert.SerializeObject(especialidades);
                        string pathEsp = Server.MapPath("~/json/");
                        System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                        return View(dhabientes);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }

            }
        }


        [HttpPost]
        public ActionResult GuardarSOAP(expediente model, bool interCheck, bool urgenciaCheck)
        {

            //System.Diagnostics.Debug.WriteLine(interCheck);

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            //Tomar ip
            var ip_realiza = Request.UserHostAddress;


            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var expCount = (from a in db.expediente
                            where a.numemp == model.numemp
                            where a.medico == model.medico
                            where a.fecha == fecha_correcta
                            //where a.hora_termino == null
                            select a).Count();


            //Tomar año y edad del paciente para guardarlo en el expediente
            var dhabiente = (from a in db.DHABIENTES
                             where a.NUMEMP == model.numemp
                             //where a.hora_termino == null
                             select a).FirstOrDefault();


            var edad = 0;
            var meses = 0;

            if (dhabiente.FNAC != null)
            {
                var today = DateTime.Today;

                DateTime fnac = (DateTime)dhabiente.FNAC;

                //var fnac = DateTime.Now.Date.Subtract((DateTime)dhabiente.FNAC);

                //edad = today.Year - fnac.Year;
                //meses = (today.Month - fnac.Month) + 12;

                //edad = today.Year - fnac.Year;
                var meses1 = today.Month - fnac.Month;
                var edad1 = today.Year - fnac.Year;
                //meses = 0;
                if (meses1 >= 0)
                {
                    meses = (today.Month - fnac.Month);
                }
                else
                {
                    meses = (today.Month - fnac.Month) + 12;
                }

                if (edad1 >= 99)
                {
                    edad = 99;
                }
                else
                {
                    edad = today.Year - fnac.Year;
                }

            }
            else
            {
                edad = 0;
                meses = 0;
            }


            var fecha_termina = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

            var consultadistancia = "0";

            if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
            {
                //Si no tiene agenda entonces es presencial por lo pronto, se va a quitar esto
                consultadistancia = "0";
            }
            else
            {
                //Revisar si la consulta es presencial o es telefonica
                string query = "select tipo as tipo from CITAS WHERE NumEmp = '" + model.numemp + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha + "'";
                var result = db.Database.SqlQuery<Citas>(query);
                var res = result.FirstOrDefault();

                if (res != null)
                {
                    if (res.tipo == "11" || res.tipo == "25")
                    {
                        //presencial
                        consultadistancia = "0";
                    }
                    else
                    {
                        //telefonica
                        consultadistancia = "1";
                    }

                }
                else
                {
                    consultadistancia = null;
                }
            }





            if (expCount > 0)
            {
                //Si se hace un referido a un especialista
                if (interCheck == true)
                {
                    //System.Diagnostics.Debug.WriteLine(urgenciaCheck);
                    //SI ES URGENTE
                    if (urgenciaCheck == true)
                    {
                        db.Database.ExecuteSqlCommand("UPDATE expediente SET numemp = '" + model.numemp + "', medico = '" + model.medico + "', diagnostico = '" + model.diagnostico + "', diagnostico2 = '" + model.diagnostico2 + "', diagnostico3 = '" + model.diagnostico3 + "', tipconsulta = '" + model.tipconsulta + "', tipconsulta2 = '" + model.tipconsulta2 + "', tipconsulta3 = '" + model.tipconsulta3 + "', s_exp = '" + model.s_exp + "', o_exp = '" + model.o_exp + "', p_exp = '" + model.p_exp + "', fecha = '" + fecha + "', peso_r = '" + model.peso_r + "', talla_r = '" + model.talla_r + "', temp_r = '" + model.temp_r + "', fresp = '" + model.fresp + "', fcard = '" + model.fcard + "', ta1 = '" + model.ta1 + "', ta2 = '" + model.ta2 + "', dstx = '" + model.dstx + "', ip_realiza = '" + ip_realiza + "', referido = '" + model.referido + "', ref_exp = '" + model.ref_exp + "', referido_urg = '1', edd_anio = '" + edad + "', edd_mes = '" + meses + "', consultadistancia = '" + consultadistancia + "'   WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and fecha = '" + fecha + "'");
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("UPDATE expediente SET numemp = '" + model.numemp + "', medico = '" + model.medico + "', diagnostico = '" + model.diagnostico + "', diagnostico2 = '" + model.diagnostico2 + "', diagnostico3 = '" + model.diagnostico3 + "', tipconsulta = '" + model.tipconsulta + "', tipconsulta2 = '" + model.tipconsulta2 + "', tipconsulta3 = '" + model.tipconsulta3 + "', s_exp = '" + model.s_exp + "', o_exp = '" + model.o_exp + "', p_exp = '" + model.p_exp + "', fecha = '" + fecha + "', peso_r = '" + model.peso_r + "', talla_r = '" + model.talla_r + "', temp_r = '" + model.temp_r + "', fresp = '" + model.fresp + "', fcard = '" + model.fcard + "', ta1 = '" + model.ta1 + "', ta2 = '" + model.ta2 + "', dstx = '" + model.dstx + "', ip_realiza = '" + ip_realiza + "', referido = '" + model.referido + "', ref_exp = '" + model.ref_exp + "', referido_urg = NULL, edd_anio = '" + edad + "', edd_mes = '" + meses + "', consultadistancia = '" + consultadistancia + "' WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and fecha = '" + fecha + "'");
                    }
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET numemp = '" + model.numemp + "', medico = '" + model.medico + "', diagnostico = '" + model.diagnostico + "', diagnostico2 = '" + model.diagnostico2 + "', diagnostico3 = '" + model.diagnostico3 + "', tipconsulta = '" + model.tipconsulta + "', tipconsulta2 = '" + model.tipconsulta2 + "', tipconsulta3 = '" + model.tipconsulta3 + "', s_exp = '" + model.s_exp + "', o_exp = '" + model.o_exp + "', p_exp = '" + model.p_exp + "', fecha = '" + fecha + "', peso_r = '" + model.peso_r + "', talla_r = '" + model.talla_r + "', temp_r = '" + model.temp_r + "', fresp = '" + model.fresp + "', fcard = '" + model.fcard + "', ta1 = '" + model.ta1 + "', ta2 = '" + model.ta2 + "', dstx = '" + model.dstx + "', ip_realiza = '" + ip_realiza + "', referido = null, ref_exp = null, edd_anio = '" + edad + "', edd_mes = '" + meses + "', consultadistancia = '" + consultadistancia + "' WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and fecha = '" + fecha + "'");
                }
            }
            else
            {
                //Si se hace un referido a un especialista
                if (interCheck == true)
                {
                    if (urgenciaCheck == true)
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, diagnostico, diagnostico2, diagnostico3, tipconsulta, tipconsulta2, tipconsulta3,  s_exp, o_exp, p_exp, fecha, peso_r, talla_r, temp_r, fresp, fcard, ta1, ta2, dstx, ip_realiza, referido, ref_exp, referido_urg, edd_anio, edd_mes, consultadistancia) VALUES('" + model.numemp + "', '" + model.medico + "', '" + model.diagnostico + "', '" + model.diagnostico2 + "', '" + model.diagnostico3 + "', '" + model.tipconsulta + "', '" + model.tipconsulta2 + "', '" + model.tipconsulta3 + "', '" + model.s_exp + "', '" + model.o_exp + "', '" + model.p_exp + "', '" + fecha + "', '" + model.peso_r + "', '" + model.talla_r + "', '" + model.temp_r + "', '" + model.fresp + "', '" + model.fcard + "', '" + model.ta1 + "', '" + model.ta2 + "', '" + model.dstx + "', '" + ip_realiza + "', '" + model.referido + "', '" + model.ref_exp + "','1','" + edad + "','" + meses + "','" + consultadistancia + "')");
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, diagnostico, diagnostico2, diagnostico3, tipconsulta, tipconsulta2, tipconsulta3,  s_exp, o_exp, p_exp, fecha, peso_r, talla_r, temp_r, fresp, fcard, ta1, ta2, dstx, ip_realiza, referido, ref_exp, edd_anio, edd_mes, consultadistancia) VALUES('" + model.numemp + "', '" + model.medico + "', '" + model.diagnostico + "', '" + model.diagnostico2 + "', '" + model.diagnostico3 + "', '" + model.tipconsulta + "', '" + model.tipconsulta2 + "', '" + model.tipconsulta3 + "', '" + model.s_exp + "', '" + model.o_exp + "', '" + model.p_exp + "', '" + fecha + "', '" + model.peso_r + "', '" + model.talla_r + "', '" + model.temp_r + "', '" + model.fresp + "', '" + model.fcard + "', '" + model.ta1 + "', '" + model.ta2 + "', '" + model.dstx + "', '" + ip_realiza + "', '" + model.referido + "', '" + model.ref_exp + "','" + edad + "','" + meses + "','" + consultadistancia + "')");
                    }
                }
                else
                {
                    //System.Diagnostics.Debug.WriteLine(model.medico);
                    db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, diagnostico, diagnostico2, diagnostico3, tipconsulta, tipconsulta2, tipconsulta3,  s_exp, o_exp, p_exp, fecha, peso_r, talla_r, temp_r, fresp, fcard, ta1, ta2, dstx, ip_realiza, edd_anio, edd_mes, consultadistancia) VALUES('" + model.numemp + "', '" + model.medico + "', '" + model.diagnostico + "', '" + model.diagnostico2 + "', '" + model.diagnostico3 + "', '" + model.tipconsulta + "', '" + model.tipconsulta2 + "', '" + model.tipconsulta3 + "', '" + model.s_exp + "', '" + model.o_exp + "', '" + model.p_exp + "', '" + fecha + "', '" + model.peso_r + "', '" + model.talla_r + "', '" + model.temp_r + "', '" + model.fresp + "', '" + model.fcard + "', '" + model.ta1 + "', '" + model.ta2 + "', '" + model.dstx + "', '" + ip_realiza + "', '" + edad + "','" + meses + "','" + consultadistancia + "')");
                }
            }

            if (model.p_exp == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {

                db.Database.ExecuteSqlCommand("UPDATE expediente SET hora_termino = '" + fecha_termina + "' WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and fecha = '" + fecha + "'");

                TempData["message_success"] = "Nota médica terminada con éxito";
                return Redirect(Request.UrlReferrer.ToString());

                //return RedirectToAction("Recetas/" + model.numemp, "ServiciosMedicos");
            }

            //return Redirect(Request.UrlReferrer.ToString());
        }



        [HttpPost]
        public ActionResult GuardarSOAP2(bool interCheck, string numemp, float peso, float talla, string masa_muscular, string masa_grasa, string porcentaje_grasa, string destrostrix, float temp, string fresp, string fcard, string ta1, string ta2, string dstx, string per_cefalico, string s_exp, string o_exp, string p_exp, string dx1, string tipconsulta, string dx2, string tipconsulta2, string dx3, string tipconsulta3, string referido, string ref_exp, bool urgenciaCheck, string referido2, string ref_exp2, bool urgenciaCheck2, string referido3, string ref_exp3, bool urgenciaCheck3)
        {


            //System.Diagnostics.Debug.WriteLine(ref_exp2);


            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var horaTrm = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha_correcta = DateTime.Parse(fecha);
            var horaTrmDT = DateTime.Parse(horaTrm);
            //Tomar ip
            var ip_realiza = Request.UserHostAddress;
            var medico = User.Identity.GetUserName();


            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var expCount = (from a in db.expediente
                            where a.numemp == numemp
                            where a.medico == medico
                            where a.fecha == fecha_correcta
                            //where a.hora_termino == null
                            select a).Count();


            //Tomar año y edad del paciente para guardarlo en el expediente
            var dhabiente = (from a in db.DHABIENTES
                             where a.NUMEMP == numemp
                             //where a.hora_termino == null
                             select a).FirstOrDefault();


            int edad = 0;
            int meses = 0;

            if (dhabiente.FNAC != null)
            {
                var today = DateTime.Today;
                DateTime fnac = (DateTime)dhabiente.FNAC;
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

                edad = Years;
                meses = Months;

            }
            else
            {
                edad = 0;
                meses = 0;
            }


            var fecha_termina = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

            var consultadistancia = "0";

            if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
            {
                //Si no tiene agenda entonces es presencial por lo pronto, se va a quitar esto
                consultadistancia = "0";
            }
            else
            {
                //Revisar si la consulta es presencial o es telefonica
                string query = "select tipo as tipo from CITAS WHERE NumEmp = '" + numemp + "' and Medico = '" + medico + "' and Fecha = '" + fecha + "'";
                var result = db.Database.SqlQuery<Citas>(query);
                var res = result.FirstOrDefault();

                if (res != null)
                {
                    if (res.tipo == "11" || res.tipo == "25")
                    {
                        //presencial
                        consultadistancia = "0";
                    }
                    else
                    {
                        //telefonica
                        consultadistancia = "1";
                    }

                }
                else
                {
                    consultadistancia = null;
                }
            }


            if (expCount > 0)
            {
                //Actualizar  s_exp, o_exp, p_ex

                if (dx1 != null && dx1 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico = '" + dx1 + "', tipconsulta = '" + tipconsulta + "',  hora_termino = '" + horaTrm + "',  ip_realiza = '" + ip_realiza + "', consultadistancia = '" + consultadistancia + "', edd_anio = '" + edad.ToString() + "', edd_mes = '" + meses.ToString() + "'  WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (s_exp != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET s_exp = '" + s_exp + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (o_exp != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET o_exp = '" + o_exp + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (p_exp != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET p_exp = '" + p_exp + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (peso != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET peso_r = '" + peso + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (talla != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET talla_r = '" + talla + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }



                if (temp != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET temp_r = '" + temp + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (fresp != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET fresp = '" + fresp + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (fcard != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET fcard = '" + fcard + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (ta1 != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ta1 = '" + ta1 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (ta2 != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ta2 = '" + ta2 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (dstx != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET dstx = '" + dstx + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (masa_muscular != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET masa_muscular = '" + masa_muscular + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (masa_grasa != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET masa_grasa = '" + masa_grasa + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (porcentaje_grasa != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET porcentaje_grasa = '" + porcentaje_grasa + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (per_cefalico != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET per_cefalico = '" + per_cefalico + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (dx2 != null && dx2 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico2 = '" + dx2 + "', tipconsulta2 = '" + tipconsulta2 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (dx3 != null && dx3 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico3 = '" + dx3 + "', tipconsulta3 = '" + tipconsulta3 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (referido != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido = '" + referido + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (urgenciaCheck)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg = '1' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (ref_exp != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp = '" + ref_exp + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (referido2 != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido2 = '" + referido2 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (urgenciaCheck2)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg2 = '1' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (ref_exp2 != null && ref_exp2 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp2 = '" + ref_exp2 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (referido3 != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido3 = '" + referido3 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (urgenciaCheck3)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg3 = '1' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (ref_exp3 != null && ref_exp3 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp3 = '" + ref_exp3 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (destrostrix != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET dstx = '" + destrostrix + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                TempData["message_success"] = "Nota médica actualizada con éxito";
                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                //Agregar  

                //Insertar las que son obligatorias
                db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, diagnostico, tipconsulta, s_exp, o_exp, p_exp, fecha, edd_anio, edd_mes, peso_r, talla_r, ip_realiza, consultadistancia, hora_termino) VALUES('" + numemp + "', '" + medico + "', '" + dx1 + "', '" + tipconsulta + "', '" + s_exp + "', '" + o_exp + "', '" + p_exp + "', '" + fecha + "', '" + edad.ToString() + "', '" + meses.ToString() + "', '" + peso + "', '" + talla + "', '" + ip_realiza + "','" + consultadistancia + "', '" + horaTrm + "')");


                //Buscar el ultimo soap para actualizar
                var expFirst = (from a in db.expediente
                                where a.numemp == numemp
                                where a.medico == medico
                                where a.fecha == fecha_correcta
                                //where a.hora_termino == null
                                select a).FirstOrDefault();

                if (temp != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET temp_r = '" + temp + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (fresp != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET fresp = '" + fresp + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (fcard != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET fcard = '" + fcard + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (ta1 != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ta1 = '" + ta1 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (ta2 != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ta2 = '" + ta2 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (dstx != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET dstx = '" + dstx + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (masa_muscular != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET masa_muscular = '" + masa_muscular + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (masa_grasa != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET masa_grasa = '" + masa_grasa + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (porcentaje_grasa != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET porcentaje_grasa = '" + porcentaje_grasa + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (per_cefalico != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET per_cefalico = '" + per_cefalico + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (dx2 != null && dx2 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico2 = '" + dx2 + "', tipconsulta2 = '" + tipconsulta2 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (dx3 != null && dx3 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico3 = '" + dx3 + "', tipconsulta3 = '" + tipconsulta3 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (referido != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido = '" + referido + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (urgenciaCheck)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg = '1' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (ref_exp != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp = '" + ref_exp + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (referido2 != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido2 = '" + referido2 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (urgenciaCheck2)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg2 = '1' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (ref_exp2 != null && ref_exp2 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp2 = '" + ref_exp2 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (referido3 != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido3 = '" + referido3 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (urgenciaCheck3)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg3 = '1' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (ref_exp3 != null && ref_exp3 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp3 = '" + ref_exp3 + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                if (destrostrix != null)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET dstx = '" + destrostrix + "' WHERE medico = '" + medico + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                TempData["message_success"] = "Nota médica terminada con éxito";
                return Redirect(Request.UrlReferrer.ToString());

            }

        }


        public JsonResult GuardaProcesoSOAP(string s_exp, string o_exp, string p_exp, string numemp)
        {
            //System.Diagnostics.Debug.WriteLine(interCheck);

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            //Tomar ip
            var ip_realiza = Request.UserHostAddress;

            var username = User.Identity.GetUserName();


            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var expCount = (from a in db.expediente
                            where a.numemp == numemp
                            where a.medico == username
                            where a.fecha == fecha_correcta
                            //where a.hora_termino == null
                            select a).Count();

            //System.Diagnostics.Debug.WriteLine(expCount);


            if (expCount > 0)
            {

                if (s_exp != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET s_exp = '" + s_exp + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET s_exp = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (o_exp != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET o_exp = '" + o_exp + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET o_exp = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (p_exp != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET p_exp = '" + p_exp + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET p_exp = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }



            }
            else
            {
                //System.Diagnostics.Debug.WriteLine(diagnostico);
                db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, fecha, s_exp) VALUES('" + numemp + "', '" + username + "', '" + fecha + "', '" + s_exp + "')");

            }

            var soap = "";

            return new JsonResult { Data = soap, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GuardaDiagSOAP(string numemp, string diagnostico1, string diagnostico2, string diagnostico3, string tipoconsulta1, string tipoconsulta2, string tipoconsulta3)
        {

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            //Tomar ip
            var ip_realiza = Request.UserHostAddress;
            var username = User.Identity.GetUserName();

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var expCount = (from a in db.expediente
                            where a.numemp == numemp
                            where a.medico == username
                            where a.fecha == fecha_correcta
                            //where a.hora_termino == null
                            select a).Count();

            if (expCount > 0)
            {
                if (diagnostico1 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico = '" + diagnostico1 + "', tipconsulta = '" + tipoconsulta1 + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico = null, tipconsulta = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (diagnostico2 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico2 = '" + diagnostico2 + "', tipconsulta2 = '" + tipoconsulta2 + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico2 = null, tipconsulta2 = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (diagnostico3 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico3 = '" + diagnostico3 + "', tipconsulta3 = '" + tipoconsulta3 + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET diagnostico3 = null, tipconsulta3 = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

            }
            else
            {
                //System.Diagnostics.Debug.WriteLine(diagnostico);
                db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, fecha, diagnostico, tipconsulta) VALUES('" + numemp + "', '" + username + "', '" + fecha + "', '" + diagnostico1 + "', '" + tipoconsulta1 + "')");
            }

            var soap = "";

            return new JsonResult { Data = soap, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GuardaSignosSOAP(string numemp, string peso_r, string talla_r, string temp_r, string fresp, string fcard, string ta1, string ta2, string dstx, string per_cefalico, string masa_muscular, string masa_grasa, string porcentaje_grasa)
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            //Tomar ip
            var ip_realiza = Request.UserHostAddress;
            var username = User.Identity.GetUserName();

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var expCount = (from a in db.expediente
                            where a.numemp == numemp
                            where a.medico == username
                            where a.fecha == fecha_correcta
                            //where a.hora_termino == null
                            select a).Count();

            //System.Diagnostics.Debug.WriteLine(temp_r);

            if (expCount > 0)
            {
                if (temp_r == "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET peso_r = '" + peso_r + "', talla_r = '" + talla_r + "', fresp = '" + fresp + "', fcard = '" + fcard + "', ta1 = '" + ta1 + "', ta2 = '" + ta2 + "', dstx = '" + dstx + "', per_cefalico = '" + per_cefalico + "', masa_muscular = '" + masa_muscular + "', masa_grasa = '" + masa_grasa + "', porcentaje_grasa = '" + porcentaje_grasa + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET peso_r = '" + peso_r + "', talla_r = '" + talla_r + "', temp_r = '" + temp_r + "', fresp = '" + fresp + "', fcard = '" + fcard + "', ta1 = '" + ta1 + "', ta2 = '" + ta2 + "', dstx = '" + dstx + "', per_cefalico = '" + per_cefalico + "', masa_muscular = '" + masa_muscular + "', masa_grasa = '" + masa_grasa + "', porcentaje_grasa = '" + porcentaje_grasa + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
            }
            else
            {
                if (temp_r == "")
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, fecha, peso_r, talla_r, fresp, fcard, ta1, ta2, dstx, per_cefalico, masa_muscular, masa_grasa, porcentaje_grasa) VALUES('" + numemp + "', '" + username + "', '" + fecha + "', '" + peso_r + "', '" + talla_r + "', '" + fresp + "', '" + fcard + "', '" + ta1 + "', '" + ta2 + "', '" + dstx + "', '" + per_cefalico + "', '" + masa_muscular + "', '" + masa_grasa + "', '" + porcentaje_grasa + "')");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, fecha, peso_r, talla_r, temp_r, fresp, fcard, ta1, ta2, dstx, per_cefalico, masa_muscular, masa_grasa, porcentaje_grasa) VALUES('" + numemp + "', '" + username + "', '" + fecha + "', '" + peso_r + "', '" + talla_r + "', '" + temp_r + "', '" + fresp + "', '" + fcard + "', '" + ta1 + "', '" + ta2 + "', '" + dstx + "', '" + per_cefalico + "', '" + masa_muscular + "', '" + masa_grasa + "', '" + porcentaje_grasa + "')");
                }
            }

            var soap = "";
            //System.Diagnostics.Debug.WriteLine(soap);

            return new JsonResult { Data = soap, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public class unidad_medica
        {
            public int id_unidad { get; set; }
            public string descripcion_unidad { get; set; }

        }

        public JsonResult TomarUltimoSOAP(string numemp)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();

            var ultimo_soap = (from r in db.expediente
                               where r.numemp == numemp
                               where r.medico == username
                               join diagnosticoNombre in db.DIAGNOSTICOS on r.diagnostico equals diagnosticoNombre.Clave into diaX1
                               from diaIn1 in diaX1.DefaultIfEmpty()
                               join diagnostico2Nombre in db.DIAGNOSTICOS on r.diagnostico2 equals diagnostico2Nombre.Clave into diaX2
                               from diaIn2 in diaX2.DefaultIfEmpty()
                               join diagnostico3Nombre in db.DIAGNOSTICOS on r.diagnostico3 equals diagnostico3Nombre.Clave into diaX3
                               from diaIn3 in diaX3.DefaultIfEmpty()
                               where r.fecha == fecha_correcta
                               //where r.hora_termino == null
                               orderby fecha descending
                               select new
                               {
                                   peso_r = r.peso_r,
                                   talla_r = r.talla_r,
                                   temp_r = r.temp_r,
                                   fresp = r.fresp,
                                   fcard = r.fcard,
                                   ta1 = r.ta1,
                                   ta2 = r.ta2,
                                   dstx = r.dstx,
                                   per_cefalico = r.per_cefalico,
                                   masa_muscular = r.masa_muscular,
                                   masa_grasa = r.masa_grasa,
                                   porcentaje_grasa = r.porcentaje_grasa,
                                   s_exp = r.s_exp,
                                   o_exp = r.o_exp,
                                   diagnostico = r.diagnostico,
                                   diagnosticoNombre = diaIn1.DesCorta,
                                   diagnostico2 = r.diagnostico2,
                                   diagnostico2Nombre = diaIn2.DesCorta,
                                   diagnostico3 = r.diagnostico3,
                                   diagnostico3Nombre = diaIn3.DesCorta,
                                   tipconsulta = r.tipconsulta,
                                   tipconsulta2 = r.tipconsulta2,
                                   tipconsulta3 = r.tipconsulta3,
                                   p_exp = r.p_exp,
                                   hora_termino = r.hora_termino,
                                   referido = r.referido,
                                   ref_exp = r.ref_exp,
                                   referido_urg = r.referido_urg,
                                   referido2 = r.referido2,
                                   ref_exp2 = r.ref_exp2,
                                   referido_urg2 = r.referido_urg2,
                                   referido3 = r.referido3,
                                   ref_exp3 = r.ref_exp3,
                                   referido_urg3 = r.referido_urg3,
                                   consultadistancia = r.consultadistancia,
                                   subreferido = r.subreferido,
                                   medicoreferido = r.medicoreferido,
                                   subreferido2 = r.subreferido2,
                                   medicoreferido2 = r.medicoreferido2,
                                   subreferido3 = r.subreferido3,
                                   medicoreferido3 = r.medicoreferido3,
                               })
                              .FirstOrDefault();


            if (ultimo_soap != null)
            {
                //System.Diagnostics.Debug.WriteLine(ultimo_soap.talla_r);


                string fresp_dos = null;
                if (ultimo_soap.fresp != null)
                {
                    fresp_dos = ultimo_soap.fresp.Replace(" ", "");
                }
                else
                {
                    fresp_dos = null;
                }


                string fcard_dos = null;
                if (ultimo_soap.fcard != null)
                {
                    fcard_dos = ultimo_soap.fcard.Replace(" ", "");
                }
                else
                {
                    fcard_dos = null;
                }


                string ta1_dos = null;
                if (ultimo_soap.ta1 != null)
                {
                    ta1_dos = ultimo_soap.ta1.Replace(" ", "");
                }
                else
                {
                    ta1_dos = null;
                }


                string ta2_dos = null;
                if (ultimo_soap.ta2 != null)
                {
                    ta2_dos = ultimo_soap.ta2.Replace(" ", "");
                }
                else
                {
                    ta2_dos = null;
                }


                string dstx_dos = null;
                if (ultimo_soap.dstx != null)
                {
                    dstx_dos = ultimo_soap.dstx.Replace(" ", "");
                }
                else
                {
                    dstx_dos = null;
                }


                string percef = null;
                if (ultimo_soap.per_cefalico != null)
                {
                    percef = ultimo_soap.per_cefalico.Replace(" ", "");
                }
                else
                {
                    percef = null;
                }

                string masaMuscular = null;
                if (ultimo_soap.masa_muscular != null)
                {
                    masaMuscular = ultimo_soap.masa_muscular.Replace(" ", "");
                }
                else
                {
                    masaMuscular = null;
                }


                string masaGrasa = null;
                if (ultimo_soap.masa_grasa != null)
                {
                    masaGrasa = ultimo_soap.masa_grasa.Replace(" ", "");
                }
                else
                {
                    masaGrasa = null;
                }


                string porcenGrasa = null;
                if (ultimo_soap.porcentaje_grasa != null)
                {
                    porcenGrasa = ultimo_soap.porcentaje_grasa.Replace(" ", "");
                }
                else
                {
                    porcenGrasa = null;
                }

                //System.Diagnostics.Debug.WriteLine(ultimo_soap.per_cefalico);
                //Tomar la unidad del médico referido


                var resultado = new
                {
                    peso_r = ultimo_soap.peso_r,
                    talla_r = ultimo_soap.talla_r,
                    temp_r = ultimo_soap.temp_r,
                    fresp = fresp_dos,
                    fcard = fcard_dos,
                    ta1 = ta1_dos,
                    ta2 = ta2_dos,
                    dstx = dstx_dos,
                    per_cefalico = percef,
                    masa_muscular = masaMuscular,
                    masa_grasa = masaGrasa,
                    porcentaje_grasa = porcenGrasa,
                    s_exp = ultimo_soap.s_exp,
                    o_exp = ultimo_soap.o_exp,
                    diagnostico = ultimo_soap.diagnostico,
                    diagnosticoNombre = ultimo_soap.diagnosticoNombre,
                    diagnostico2 = ultimo_soap.diagnostico2,
                    diagnostico2Nombre = ultimo_soap.diagnostico2Nombre,
                    diagnostico3 = ultimo_soap.diagnostico3,
                    diagnostico3Nombre = ultimo_soap.diagnostico3Nombre,
                    tipconsulta = ultimo_soap.tipconsulta,
                    tipconsulta2 = ultimo_soap.tipconsulta2,
                    tipconsulta3 = ultimo_soap.tipconsulta3,
                    p_exp = ultimo_soap.p_exp,
                    hora_termino = ultimo_soap.hora_termino,
                    referido = ultimo_soap.referido,
                    ref_exp = ultimo_soap.ref_exp,
                    referido_urg = ultimo_soap.referido_urg,
                    referido2 = ultimo_soap.referido2,
                    ref_exp2 = ultimo_soap.ref_exp2,
                    referido_urg2 = ultimo_soap.referido_urg2,
                    referido3 = ultimo_soap.referido3,
                    ref_exp3 = ultimo_soap.ref_exp3,
                    referido_urg3 = ultimo_soap.referido_urg3,
                    consultadistancia = ultimo_soap.consultadistancia,
                    subreferido = ultimo_soap.subreferido,
                    medicoreferido = ultimo_soap.medicoreferido,
                    subreferido2 = ultimo_soap.subreferido2,
                    medicoreferido2 = ultimo_soap.medicoreferido2,
                    subreferido3 = ultimo_soap.subreferido3,
                    medicoreferido3 = ultimo_soap.medicoreferido3,
                    //unidad = unidad,
                };

                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var resultado = "";

                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }


        public JsonResult FinalizarSOAP(string numemp, bool interCheck, bool urgenciaCheck, string referido, string ref_exp, bool urgenciaCheck2, string referido2, string ref_exp2, bool urgenciaCheck3, string referido3, string ref_exp3, string subreferido, string medicoreferido, string subreferido2, string medicoreferido2, string subreferido3, string medicoreferido3)
        {
            

            //System.Diagnostics.Debug.WriteLine(ref_exp);
            //System.Diagnostics.Debug.WriteLine(ref_exp2);
            //System.Diagnostics.Debug.WriteLine(ref_exp3);

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var hora_termino = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

            //Tomar ip
            var ip_realiza = Request.UserHostAddress;

            var username = User.Identity.GetUserName();

            //LINEAS DEL TRIAGE -MILTON------------------------------------------------------------------------
            SMDEVTriage TRIAGE = new SMDEVTriage();

            var UsuTriage = (from a in TRIAGE.TblTriage
                             where a.Expediente == numemp
                             where a.MedicoLlama == username
                             where a.Fecha >= fecha_correcta
                             select a).FirstOrDefault();

            if(UsuTriage != null)
            {
                if(UsuTriage.PasarASoap == null)
                {
                    UsuTriage.PasarASoap = "1";
                    TRIAGE.SaveChanges();
                }
            }

            //--------------------------------------------------------------------------------------------------

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var expFirst = (from a in db.expediente
                            where a.numemp == numemp
                            where a.medico == username
                            where a.fecha == fecha_correcta
                            //where a.hora_termino == null
                            select a).FirstOrDefault();

            /*var resultado = "";
            
            if(expFirst.hora_termino == null)
            {
                resultado = "Nota médica terminada con éxito";
            }
            else
            {
                resultado = "Nota médica editada con éxito";
            }*/

            //Tomar año y edad del paciente para guardarlo en el expediente
            var dhabiente = (from a in db.DHABIENTES
                             where a.NUMEMP == numemp
                             //where a.hora_termino == null
                             select a).FirstOrDefault();


            /*var edad = 0;
            var meses = 0;

            if (dhabiente.FNAC != null)
            {
                var today = DateTime.Today;

                DateTime fnac = (DateTime)dhabiente.FNAC;

                var meses1 = today.Month - fnac.Month;
                var edad1 = today.Year - fnac.Year;
                //meses = 0;
                if (meses1 >= 0)
                {
                    meses = (today.Month - fnac.Month);
                }
                else
                {
                    meses = (today.Month - fnac.Month) + 12;
                }

                if (edad1 >= 99)
                {
                    edad = 99;
                }
                else
                {
                    edad = today.Year - fnac.Year;
                }

            }
            else
            {
                edad = 0;
                meses = 0;
            }*/

            int edad = 0;
            int meses = 0;

            var today = DateTime.Today;
            DateTime fnac = (DateTime)dhabiente.FNAC;
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

            if (Years >= 99)
            {
                edad = 99;
            }
            else
            {
                edad = Years;
            }

            meses = Months;


            var consultadistancia = "0";

            if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
            {
                //Si no tiene agenda entonces es presencial por lo pronto, se va a quitar esto
                consultadistancia = "0";
            }
            else
            {
                //Revisar si la consulta es presencial o es telefonica
                string query = "select tipo as tipo from CITAS WHERE NumEmp = '" + numemp + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha + "'";
                var result = db.Database.SqlQuery<Citas>(query);
                var res = result.FirstOrDefault();

                if (res != null)
                {
                    if (res.tipo == "11" || res.tipo == "25")
                    {
                        //presencial
                        consultadistancia = "0";
                    }
                    else
                    {
                        //telefonica
                        consultadistancia = "1";
                    }

                }
                else
                {
                    consultadistancia = null;
                }
            }


            //Actualizar SOAP

            //Si si va a poner una interconsulta
            if (interCheck == true)
            {
                //if (referido != "")
                //{
                //Actualiza
                if (referido != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido = '" + referido + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }



                if (ref_exp != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp = '" + ref_exp + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (urgenciaCheck == true)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg = 1 WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }

                //}


                //if (referido2 != "")
                //{
                //Actualiza
                if (referido2 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido2 = '" + referido2 + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido2 = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }



                if (ref_exp2 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp2 = '" + ref_exp2 + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp2 = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (urgenciaCheck2 == true)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg2 = 1 WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg2 = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                //}

                //if (referido3 != "")
                //{
                //Actualiza
                if (referido3 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido3 = '" + referido3 + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido3 = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }



                if (ref_exp3 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp3 = '" + ref_exp3 + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp3 = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                if (urgenciaCheck3 == true)
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg3 = 1 WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET referido_urg3 = null WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }


                //tomar las observaciones
                var observaciones = (from a in db.expediente
                                     where a.numemp == numemp
                                     where a.medico == username
                                     where a.fecha == fecha_correcta
                                     select a).FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(subreferido);

                if (subreferido != null && subreferido != "")
                {
                    var subint1 = Int16.Parse(subreferido);
                    var inter1 = (from a in db.InterconsultasSub
                                  where a.id == subint1
                                  select a).FirstOrDefault();

                    var med1 = (from a in db.MEDICOS
                                where a.Numero == medicoreferido
                                select new
                                {
                                    nombre = a.Nombre + " " + a.Apaterno + " " + a.Amaterno
                                }).FirstOrDefault();

                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp = '" + inter1.descripcion + " " + med1.nombre + " " + observaciones.ref_exp + "', subreferido = '" + subint1 + "' , medicoreferido = '" + medicoreferido + "'  WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");

                }
                else
                {
                    if (User.IsInRole("SolicitudDental"))
                    {
                        var med1 = (from a in db.MEDICOS
                                    where a.Numero == medicoreferido
                                    select new
                                    {
                                        nombre = a.Nombre + " " + a.Apaterno + " " + a.Amaterno
                                    }).FirstOrDefault();

                        db.Database.ExecuteSqlCommand("UPDATE expediente SET medicoreferido = '" + medicoreferido + "'  WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");

                    }
                }



                if (subreferido2 != null && subreferido2 != "")
                {
                    var subint2 = Int16.Parse(subreferido2);

                    var inter2 = (from a in db.InterconsultasSub
                                  where a.id == subint2
                                  select a).FirstOrDefault();

                    var med2 = (from a in db.MEDICOS
                                where a.Numero == medicoreferido2
                                select new
                                {
                                    nombre = a.Nombre + " " + a.Apaterno + " " + a.Amaterno
                                }).FirstOrDefault();

                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp2 = '" + inter2.descripcion + " " + med2.nombre + " " + observaciones.ref_exp2 + "', subreferido2 = '" + subint2 + "' , medicoreferido2 = '" + medicoreferido2 + "'  WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");

                }
                else
                {
                    if (User.IsInRole("SolicitudDental"))
                    {
                        var med1 = (from a in db.MEDICOS
                                    where a.Numero == medicoreferido
                                    select new
                                    {
                                        nombre = a.Nombre + " " + a.Apaterno + " " + a.Amaterno
                                    }).FirstOrDefault();

                        db.Database.ExecuteSqlCommand("UPDATE expediente SET medicoreferido2 = '" + medicoreferido2 + "'  WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");

                    }
                }



                if (subreferido3 != null && subreferido3 != "")
                {
                    var subint3 = Int16.Parse(subreferido3);

                    var inter3 = (from a in db.InterconsultasSub
                                  where a.id == subint3
                                  select a).FirstOrDefault();

                    var med3 = (from a in db.MEDICOS
                                where a.Numero == medicoreferido3
                                select new
                                {
                                    nombre = a.Nombre + " " + a.Apaterno + " " + a.Amaterno
                                }).FirstOrDefault();

                    db.Database.ExecuteSqlCommand("UPDATE expediente SET ref_exp3 = '" + inter3.descripcion + " " + med3.nombre + " " + observaciones.ref_exp3 + "', subreferido3 = '" + subint3 + "' , medicoreferido3 = '" + medicoreferido3 + "'  WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
                }
                else
                {
                    if (User.IsInRole("SolicitudDental"))
                    {
                        var med1 = (from a in db.MEDICOS
                                    where a.Numero == medicoreferido
                                    select new
                                    {
                                        nombre = a.Nombre + " " + a.Apaterno + " " + a.Amaterno
                                    }).FirstOrDefault();

                        db.Database.ExecuteSqlCommand("UPDATE expediente SET medicoreferido3 = '" + medicoreferido3 + "'  WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");

                    }
                }


                //}

                db.Database.ExecuteSqlCommand("UPDATE expediente SET hora_termino = '" + hora_termino + "', edd_anio = '" + edad + "' , edd_mes = '" + meses + "', consultadistancia = '" + consultadistancia + "', ip_realiza = '" + ip_realiza + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");

            }
            else
            {
                db.Database.ExecuteSqlCommand("UPDATE expediente SET referido = null, ref_exp = null, referido_urg = null, referido2 = null, ref_exp2 = null, referido_urg2 = null, referido3 = null, ref_exp3 = null, referido_urg3 = null,  hora_termino = '" + hora_termino + "', edd_anio = '" + edad + "' , edd_mes = '" + meses + "', consultadistancia = '" + consultadistancia + "', ip_realiza = '" + ip_realiza + "' WHERE medico = '" + username + "' and numemp = '" + numemp + "' and fecha = '" + fecha + "'");
            }


            //Identificar si es de nutri/psico para mostrar mensaje de proxima cita

            var info = username.Substring(0, 2);


            return new JsonResult { Data = info, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        //Diagnosticos frecuentes del medico
        public JsonResult DiagnosticosMedico()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();

            var diagnosticos = (from r in db.expediente
                                join diagnostico1 in db.DIAGNOSTICOS on r.diagnostico equals diagnostico1.Clave into diaX1
                                from diaIn1 in diaX1.DefaultIfEmpty()
                                    //join diagnostico2Nombre in db.DIAGNOSTICOS on r.diagnostico2 equals diagnostico2Nombre.Clave
                                    //join diagnostico3Nombre in db.DIAGNOSTICOS on r.diagnostico3 equals diagnostico3Nombre.Clave
                                    //where r.DesCorta.Contains(diagnostico)
                                    //|| r.DescCompleta.Contains(diagnostico)
                                    //|| r.Clave.Contains(diagnostico)
                                where r.medico == username
                                where r.fecha >= fecha_correcta
                                where r.diagnostico != null
                                where r.hora_termino != null
                                select new
                                {
                                    //diagnostico = r.diagnostico,
                                    DesCorta = diaIn1.DesCorta,
                                    Clave = diaIn1.Clave,
                                })
                                .GroupBy(p => new
                                {
                                    p.DesCorta,
                                    p.Clave,
                                })
                                .Select(g => new
                                {
                                    DesCorta = g.Key.DesCorta,
                                    Clave = g.Key.Clave,
                                    Count = g.Count(),
                                })
                                .OrderByDescending(g => g.Count)
                                .ToList()
                                .Take(6);

            return new JsonResult { Data = diagnosticos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        //Diagnosticos frecuentes del paciente
        public JsonResult DiagnosticosPaciente(string numemp)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();

            var diagnosticos = (from r in db.expediente
                                join diagnostico1 in db.DIAGNOSTICOS on r.diagnostico equals diagnostico1.Clave into diaX1
                                from diaIn1 in diaX1.DefaultIfEmpty()
                                where r.numemp == numemp
                                where r.fecha >= fecha_correcta
                                where r.diagnostico != null
                                where r.hora_termino != null
                                select new
                                {
                                    //diagnostico = r.diagnostico,
                                    DesCorta = diaIn1.DesCorta,
                                    Clave = diaIn1.Clave,
                                })
                                .GroupBy(p => new
                                {
                                    p.DesCorta,
                                    p.Clave,
                                })
                                .Select(g => new
                                {
                                    DesCorta = g.Key.DesCorta,
                                    Clave = g.Key.Clave,
                                    Count = g.Count(),
                                })
                                .OrderByDescending(g => g.Count)
                                .ToList()
                                .Take(6);

            return new JsonResult { Data = diagnosticos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        //Diagnosticos frecuentes de la especialidad
        public JsonResult DiagnosticosEspecialidad(string numemp)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();
            var especialidad = username.Substring(0, 2);

            var diagnosticos = (from r in db.expediente
                                join diagnostico1 in db.DIAGNOSTICOS on r.diagnostico equals diagnostico1.Clave into diaX1
                                from diaIn1 in diaX1.DefaultIfEmpty()
                                where r.medico.Substring(0, 2) == especialidad
                                where r.fecha >= fecha_correcta
                                where r.diagnostico != null
                                where r.hora_termino != null
                                select new
                                {
                                    //diagnostico = r.diagnostico,
                                    DesCorta = diaIn1.DesCorta,
                                    Clave = diaIn1.Clave,
                                })
                                .GroupBy(p => new
                                {
                                    p.DesCorta,
                                    p.Clave,
                                })
                                .Select(g => new
                                {
                                    DesCorta = g.Key.DesCorta,
                                    Clave = g.Key.Clave,
                                    Count = g.Count(),
                                })
                                .OrderByDescending(g => g.Count)
                                .ToList()
                                .Take(6);

            return new JsonResult { Data = diagnosticos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult DiagnosticosDental(string numemp)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();
            var especialidad = username.Substring(0, 2);

            /*
            var diagnosticos = (from r in db.expediente
                                join diagnostico1 in db.DIAGNOSTICOS on r.diagnostico equals diagnostico1.Clave into diaX1
                                from diaIn1 in diaX1.DefaultIfEmpty()
                                //where r.medico.Substring(0, 2) == especialidad
                                where r.medico.Substring(0, 2) == "04" || r.medico.Substring(0, 2) == "45"
                                || r.medico.Substring(0, 2) == "46" || r.medico.Substring(0, 2) == "41"
                                || r.medico.Substring(0, 2) == "44"
                                where r.fecha >= fecha_correcta
                                where r.diagnostico != null
                                where r.hora_termino != null
                                select new
                                {
                                    //diagnostico = r.diagnostico,
                                    DesCorta = diaIn1.DesCorta,
                                    Clave = diaIn1.Clave,
                                })
                                .GroupBy(p => new
                                {
                                    p.DesCorta,
                                    p.Clave,
                                })
                                .Select(g => new
                                {
                                    DesCorta = g.Key.DesCorta,
                                    Clave = g.Key.Clave,
                                    Count = g.Count(),
                                })
                                .OrderByDescending(g => g.Count)
                                .ToList();

            */

            string queryDet = "select DIAGNOSTICOS.DesCorta as DesCorta, DIAGNOSTICOS.Clave as clave from expediente_dental INNER JOIN DIAGNOSTICOS ON expediente_dental.diagnostico = DIAGNOSTICOS.Clave where expediente_dental.fecha >= '" + fecha + "' and expediente_dental.diagnostico is not null group by DIAGNOSTICOS.DesCorta, DIAGNOSTICOS.Clave;";
            var resultDenDet = db.Database.SqlQuery<Dental>(queryDet);
            var diagnosticos = resultDenDet.ToList();

            return new JsonResult { Data = diagnosticos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        public JsonResult BuscarDiagnosticos(string diagnostico)
        {
            Models.SMDEVEntities29 db = new Models.SMDEVEntities29();

            var diagnosticoChido = diagnostico;
            var diag = diagnosticoChido.Replace(" ", "%");
            /*var diagnosticos = (from r in db.DIAGNOSTICOS
                               where r.DesCorta.Contains(diagnostico)
                               || r.DescCompleta.Contains(diagnostico)
                               || r.Clave.Contains(diagnostico)
                                // r.medico == username
                                select new
                                {
                                    //diagnostico = r.diagnostico,
                                    DesCorta = r.DesCorta,
                                }).ToList();*/

            string query = "SELECT * FROM DIAGNOSTICOS WHERE DesCorta like '%" + diag + "%' COLLATE Latin1_General_CI_AI OR Clave like '%" + diag + "%'";
            var result = db.Database.SqlQuery<DIAGNOSTICOS>(query);
            var diagnosticos = result.ToList();

            return new JsonResult { Data = diagnosticos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult SignosVitales(string numexp)
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();

            SMDEVEnfermeriaSignosVitales db2 = new SMDEVEnfermeriaSignosVitales();

            var signosvitales = (from r in db2.EnfermeriaSignosVitales
                                 where r.numemp == numexp
                                 where r.FechaCaptura >= fechaDT
                                 select new
                                 {
                                     peso = r.peso_r,
                                     talla = r.talla_r,
                                     temp = r.temp_r,
                                     fresp = r.fresp,
                                     fcard = r.fcard,
                                     ta1 = r.ta1,
                                     ta2 = r.ta2,
                                     dstx = r.dstx,
                                 })
                                .FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(signosvitales);

            if (signosvitales != null)
            {
                var expCount = (from a in db.expediente
                                where a.numemp == numexp
                                where a.medico == username
                                where a.fecha == fechaDT
                                select a).Count();

                //System.Diagnostics.Debug.WriteLine(expCount);

                if (expCount == 0)
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, fecha, peso_r, temp_r, talla_r, fresp, fcard, ta1, ta2, dstx) VALUES('" + numexp + "', '" + username + "', '" + fecha + "', '" + signosvitales.peso + "', '" + signosvitales.temp + "', '" + signosvitales.talla + "', '" + signosvitales.fresp + "', '" + signosvitales.fcard + "', '" + signosvitales.ta1 + "', '" + signosvitales.ta2 + "', '" + signosvitales.dstx + "')");
                }

                return new JsonResult { Data = signosvitales, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                var result = "";
                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }


        public static string GetLocalIPAddress()
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();

            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();
        }


        [HttpGet]
        public ActionResult Recetas(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();


                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //Si no ha llegado el paciente a recepcion
                            if (res.hora_recepcion == null || res.hr_llamado == null)
                            {
                                //System.Diagnostics.Debug.WriteLine(res.hora_recepcion);

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " es presencial, haz clic en el boton llamar antes de crear una receta";
                                return RedirectToAction("Citas", "ServiciosMedicos");
                                //return RedirectToAction("Index", "Home");
                            }
                            //Si ya llego
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                                var nombreusuario = User.Identity.GetUserName();
                                var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                                var fecha = DateTime.Parse(fecha_string);

                                var exp = (from a in db3.expediente
                                           where a.numemp == id
                                           where a.medico == nombreusuario
                                           where a.hora_termino != null
                                           where a.fecha == fecha
                                           select a).FirstOrDefault();

                                if (exp != null)
                                {
                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    var especialidad = nombreusuario.Substring(0, 2);

                                    Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                                    if (dhabientes.NUMAFIL != "P72112")
                                    {
                                        return View(dhabientes);
                                    }
                                    else
                                    {
                                        TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                        return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                    }

                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Debes terminar una nota médica antes de crear una receta";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }

                            }


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");

                            Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                            var nombreusuario = User.Identity.GetUserName();
                            var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                            var fecha = DateTime.Parse(fecha_string);

                            var exp = (from a in db3.expediente
                                       where a.numemp == id
                                       where a.medico == nombreusuario
                                       where a.hora_termino != null
                                       where a.fecha == fecha
                                       select a).FirstOrDefault();

                            if (exp != null)
                            {
                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                var especialidad = nombreusuario.Substring(0, 2);

                                Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                                if (dhabientes.NUMAFIL != "P72112")
                                {
                                    return View(dhabientes);
                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }

                            }
                            else
                            {
                                //TempData["numemp"] = exp.numemp;
                                TempData["message_recetas_warning"] = "Debes terminar una nota médica antes de crear una receta";
                                return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                            }

                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {

                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();


                        Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();


                        if (dhabientes.NUMAFIL != "P72112")
                        {
                            return View(dhabientes);
                        }
                        else
                        {
                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                            return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

            }
        }



        public ActionResult Recetas2(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();


                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //Si no ha llegado el paciente a recepcion
                            if (res.hora_recepcion == null || res.hr_llamado == null)
                            {
                                //System.Diagnostics.Debug.WriteLine(res.hora_recepcion);

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " es presencial, haz clic en el boton llamar antes de crear una receta";
                                return RedirectToAction("Citas", "ServiciosMedicos");
                                //return RedirectToAction("Index", "Home");
                            }
                            //Si ya llego
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                                var nombreusuario = User.Identity.GetUserName();
                                var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                                var fecha = DateTime.Parse(fecha_string);

                                var exp = (from a in db3.expediente
                                           where a.numemp == id
                                           where a.medico == nombreusuario
                                           where a.hora_termino != null
                                           where a.fecha == fecha
                                           select a).FirstOrDefault();

                                if (exp != null)
                                {
                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    var especialidad = nombreusuario.Substring(0, 2);

                                    Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                                    if (dhabientes.NUMAFIL != "P72112")
                                    {
                                        return View(dhabientes);
                                    }
                                    else
                                    {
                                        TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                        return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                    }

                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Debes terminar una nota médica antes de crear una receta";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }

                            }


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");

                            Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                            var nombreusuario = User.Identity.GetUserName();
                            var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                            var fecha = DateTime.Parse(fecha_string);

                            var exp = (from a in db3.expediente
                                       where a.numemp == id
                                       where a.medico == nombreusuario
                                       where a.hora_termino != null
                                       where a.fecha == fecha
                                       select a).FirstOrDefault();

                            if (exp != null)
                            {
                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                var especialidad = nombreusuario.Substring(0, 2);

                                Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                                if (dhabientes.NUMAFIL != "P72112")
                                {
                                    return View(dhabientes);
                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }

                            }
                            else
                            {
                                //TempData["numemp"] = exp.numemp;
                                TempData["message_recetas_warning"] = "Debes terminar una nota médica antes de crear una receta";
                                return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                            }

                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {

                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();


                        Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();


                        if (dhabientes.NUMAFIL != "P72112")
                        {
                            return View(dhabientes);
                        }
                        else
                        {
                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                            return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

            }
        }




        public ActionResult PruebaAntigenos()
        {
            if (User.IsInRole("PruebaAntigenos"))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult GuardarPruebaAnt(int id, string numexp, string resultado, string nota)
        {

            //System.Diagnostics.Debug.WriteLine(id);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();

            var pruebaAntigenos = (from a in db.PruebaAntigenos
                                   where a.numexp == numexp
                                   select a).FirstOrDefault();

            /*if (id == 0)
            {
                //Agregar
                db.Database.ExecuteSqlCommand("INSERT INTO PruebaAntigenos (resultado, nota, fecha, usuario, numexp) VALUES('" + resultado + "', '" + nota + "','" + fecha + "','" + username + "','" + numexp + "')");

                TempData["message_success"] = "Resultado registrado con éxito";

            }
            else
            {
                //Editar
                db.Database.ExecuteSqlCommand("UPDATE PruebaAntigenos SET resultado = '" + resultado + "', nota = '" + nota + "', fecha = '" + fecha + "' , usuario = '" + username + "', numexp = '" + numexp + "' WHERE id = '" + id + "'");
                
                TempData["message_success"] = "Resultado editado con éxito";
            }*/


            //Editar
            db.Database.ExecuteSqlCommand("UPDATE CovidSolicitud SET Resultado = '" + resultado + "', NotaResultado = '" + nota + "', FechaResultado = '" + fecha + "' , UsuarioResultado = '" + username + "' WHERE SolicitudId = '" + id + "'");

            TempData["message_success"] = "Resultado registrado con éxito";


            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult ResultadoAntigenos(string id)
        {
            //System.Diagnostics.Debug.WriteLine(id);

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias") || User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                {
                    Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();

                    Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es");


                    return View(dhabientes);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }


            }

        }

        public JsonResult RegistrosPruebaAnti()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.AddDays(-7).ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            var fecha2 = DateTime.Now.AddDays(-22).ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT2 = DateTime.Parse(fecha2);
            var username = User.Identity.GetUserName();


            /*var pruebaAn = (from a in db.PruebaAntigenos
                          join dhabientes in db.DHABIENTES on a.numexp equals dhabientes.NUMEMP into dhabientesX
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
                              resultado = a.resultado,
                              nota = a.nota,
                              numexp = a.numexp,
                              fecha = a.fecha,
                              numemp = dhabientesIn.NUMAFIL,
                              fnac = dhabientesIn.FNAC,
                              sexo = dhabientesIn.SEXO,
                              //numemp = d.NUMAFIL,

                          })
                          .OrderByDescending(g => g.id)
                          .ToList();*/

            if (User.IsInRole("PruebaAntigenosImpresion"))
            {
                var pruebaAn = (from a in db.CovidSolicitud
                                join dhabientes in db.DHABIENTES on a.NumEmp equals dhabientes.NUMEMP into dhabientesX
                                from dhabientesIn in dhabientesX.DefaultIfEmpty()
                                    //where a.NumEmp == "15965100"
                                where a.FechaResultado >= fechaDT2
                                select new
                                {
                                    paciente = dhabientesIn.NOMBRES + " " + dhabientesIn.APATERNO + " " + dhabientesIn.AMATERNO,
                                    fvigencia = dhabientesIn.FVIGENCIA,
                                    //fnac = d.FNAC,
                                    //sexo = d.SEXO,
                                    id = a.SolicitudId,
                                    resultado = a.Resultado,
                                    nota = a.NotaResultado,
                                    numexp = a.NumEmp,
                                    fecha = a.FechaResultado,
                                    tipo = a.Tipo,
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


                    var tipo = "";

                    if (item.tipo == "Antigenos")
                    {
                        tipo = "PASE PARA PRUEBA DE ANTIGENOS COVID 19 - Antígenos";
                    }
                    else
                    {
                        if (item.tipo == "AEMA")
                        {
                            tipo = "PASE A AEMA";
                        }
                        else
                        {
                            if (item.tipo == "UER")
                            {
                                tipo = "PASE A LA UNIDAD DE ENFERMEDADES RESPIRATORIAS DE S.M";
                            }
                            else
                            {
                                if (item.tipo == "2")
                                {
                                    tipo = "PASE PARA PRUEBA DE ANTIGENOS COVID 19 - Antígenos y UER";
                                }
                            }
                        }
                    }

                    var info = "";

                    if (User.IsInRole("PruebaAntigenosImpresion"))
                    {
                        info = "<a data-info='" + item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia) + "*" + item.resultado + "*" + item.nota + "*" + tipo + "*" + item.id + "' type='button' class='btn btn-imprimir'>Imprimir</a>";
                    }
                    else
                    {
                        info = "<a data-info='" + item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia) + "*" + item.resultado + "*" + item.nota + "*" + tipo + "*" + item.id + "' type='button' class='btn btn-asignar'>Editar</a> <a data-info='" + item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia) + "*" + item.resultado + "*" + item.nota + "*" + tipo + "*" + item.id + "' type='button' class='btn btn-imprimir'>Imprimir</a>";
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
                        //info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + "*" + dhab.sexo + "*" + prueCovid.SolicitudId + "*" + string.Format("{0:dddd, dd MMMM yyyy}", prueCovid.FechaSol),
                        //info = item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia) + "*" + item.resultado + "*" + item.nota + "*" + tipo + "*" + item.id,
                        info = info
                        //info = item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + item.id + "*" + item.resultado + "*" + item.nota + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia),
                    };

                    results1.Add(resultado);
                }

                return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


            }
            else
            {
                var pruebaAn = (from a in db.CovidSolicitud
                                join dhabientes in db.DHABIENTES on a.NumEmp equals dhabientes.NUMEMP into dhabientesX
                                from dhabientesIn in dhabientesX.DefaultIfEmpty()
                                where a.UsuarioResultado == username
                                where a.FechaResultado >= fechaDT
                                select new
                                {
                                    paciente = dhabientesIn.NOMBRES + " " + dhabientesIn.APATERNO + " " + dhabientesIn.AMATERNO,
                                    fvigencia = dhabientesIn.FVIGENCIA,
                                    //fnac = d.FNAC,
                                    //sexo = d.SEXO,
                                    id = a.SolicitudId,
                                    resultado = a.Resultado,
                                    nota = a.NotaResultado,
                                    numexp = a.NumEmp,
                                    fecha = a.FechaResultado,
                                    tipo = a.Tipo,
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


                    var tipo = "";

                    if (item.tipo == "Antigenos")
                    {
                        tipo = "PASE PARA PRUEBA DE ANTIGENOS COVID 19 - Antígenos";
                    }
                    else
                    {
                        if (item.tipo == "AEMA")
                        {
                            tipo = "PASE A AEMA";
                        }
                        else
                        {
                            if (item.tipo == "UER")
                            {
                                tipo = "PASE A LA UNIDAD DE ENFERMEDADES RESPIRATORIAS DE S.M";
                            }
                            else
                            {
                                if (item.tipo == "2")
                                {
                                    tipo = "PASE PARA PRUEBA DE ANTIGENOS COVID 19 - Antígenos y UER";
                                }
                            }
                        }
                    }

                    var info = "";

                    if (User.IsInRole("PruebaAntigenosImpresion"))
                    {
                        info = "<a data-info='" + item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia) + "*" + item.resultado + "*" + item.nota + "*" + tipo + "*" + item.id + "' type='button' class='btn btn-imprimir'>Imprimir</a>";
                    }
                    else
                    {
                        info = "<a data-info='" + item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia) + "*" + item.resultado + "*" + item.nota + "*" + tipo + "*" + item.id + "' type='button' class='btn btn-asignar'>Editar</a> <a data-info='" + item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia) + "*" + item.resultado + "*" + item.nota + "*" + tipo + "*" + item.id + "' type='button' class='btn btn-imprimir'>Imprimir</a>";
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
                        //info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + "*" + dhab.sexo + "*" + prueCovid.SolicitudId + "*" + string.Format("{0:dddd, dd MMMM yyyy}", prueCovid.FechaSol),
                        //info = item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia) + "*" + item.resultado + "*" + item.nota + "*" + tipo + "*" + item.id,
                        info = info
                        //info = item.numexp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + item.id + "*" + item.resultado + "*" + item.nota + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia),
                    };

                    results1.Add(resultado);
                }

                return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


            }

        }

        public JsonResult DetallesAntigenos(int id)
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.AddDays(-7).ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();

            var pruebaAn = (from a in db.CovidSolicitud
                            join dhabientes in db.DHABIENTES on a.NumEmp equals dhabientes.NUMEMP into dhabientesX
                            from dhabientesIn in dhabientesX.DefaultIfEmpty()
                            where a.SolicitudId == id
                            select new
                            {
                                paciente = dhabientesIn.NOMBRES + " " + dhabientesIn.APATERNO + " " + dhabientesIn.AMATERNO,
                                fvigencia = dhabientesIn.FVIGENCIA,
                                //fnac = a.FNAC,
                                //sexo = a.SEXO,
                                id = a.SolicitudId,
                                resultado = a.Resultado,
                                nota = a.NotaResultado,
                                numexp = a.NumEmp,
                                fecha = a.FechaResultado,
                                tipo = a.Tipo,
                                numemp = dhabientesIn.NUMAFIL,
                                fnac = dhabientesIn.FNAC,
                                sexo = dhabientesIn.SEXO,
                                //numemp = d.NUMAFIL,

                            })
                          .OrderByDescending(g => g.id)
                          .FirstOrDefault();


            var today = DateTime.Today;
            DateTime fnac = (DateTime)pruebaAn.fnac;
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


            var resultado = "";
            if (pruebaAn.resultado == 1)
            {
                resultado = "Positivo";
            }
            else
            {
                resultado = "Negativo";
            }

            var result = new Object();

            result = new
            {
                paciente = pruebaAn.paciente,
                id = pruebaAn.id,
                edad = Years,
                fecha = string.Format("{0:dd/MM/yyyy}", pruebaAn.fecha),
                fnac = string.Format("{0:dd/MM/yyyy}", pruebaAn.fnac),
                sexo = pruebaAn.sexo,
                resultado = resultado,

            };



            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        public class ResultadoAnt
        {
            public int id { get; set; }
            public string usuario { get; set; }
            public string resultado { get; set; }
            public string nota { get; set; }
            public string fecha { get; set; }
        }


        public JsonResult GetPruebaAntigenos(string numexp)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var pruebaantigenos = (from a in db.CovidSolicitud
                                   join medicos in db.MEDICOS on a.UsuarioResultado equals medicos.Numero into medicosX
                                   from medicosIn in medicosX.DefaultIfEmpty()
                                   where a.NumEmp == numexp
                                   where a.Resultado != null
                                   select new
                                   {
                                       id = a.SolicitudId,
                                       resultado = a.Resultado,
                                       usuario = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                       nota = a.NotaResultado,
                                       fecha = a.FechaResultado,
                                   })
                                  .OrderByDescending(g => g.fecha)
                                  .ToList();

            var resuAnt = new List<ResultadoAnt>();

            foreach (var item in pruebaantigenos)
            {

                var resultado = "";
                if (item.resultado == 1)
                {
                    resultado = "Positivo";
                }
                else
                {
                    resultado = "Negativo";
                }

                var listrest = new ResultadoAnt
                {
                    id = item.id,
                    resultado = resultado,
                    usuario = item.usuario,
                    nota = item.nota,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                };

                resuAnt.Add(listrest);
            }


            return new JsonResult { Data = resuAnt, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult RecetasUnidadER(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("UnidadER"))
                {

                    Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();


                    return View(dhabientes);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }

        }


        public ActionResult RecetasMederos(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("Mederos"))
                {

                    Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();


                    return View(dhabientes);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }

        }



        public JsonResult RecetasMedUER(string id)
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //RECETAS
            if (User.IsInRole("UnidadER"))
            {

                string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_uder where id_sustancia != 425 and cactual is not null";
                var result = db.Database.SqlQuery<RecetasMederos>(query);
                var res = result.ToList();

                var medicamentos = new List<Result>();

                foreach (var medicamento in res)
                {

                    /*
                    string query2 = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + medicamento.id_sustancia + "  and id_sustancia = 2650 ";
                    var result2 = db2.Database.SqlQuery<LstInv>(query2);
                    var res2 = result2.FirstOrDefault();
                    */

                    var res2 = (from a in db.Sustancia
                                join grupo in db.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                                from grupoIn in grupoX.DefaultIfEmpty()
                                where a.Id == medicamento.id_sustancia
                                where a.descripcion_21 != ""
                                where a.descripcion_21 != null
                                select new
                                {
                                    descripcion_21 = a.descripcion_21,
                                    clave = a.Clave,
                                    nivel_21 = a.Nivel_21,
                                    sobranteInv2022 = a.SobranteInv2022,
                                    descripcion_grupo = grupoIn.descripcion,
                                }).FirstOrDefault();


                    //System.Diagnostics.Debug.WriteLine(res2);

                    if (res2 != null)
                    {

                        var medCon2 = (from a in db.MedicamentosControlados
                                       where a.clave == res2.clave
                                       select a).FirstOrDefault();

                        //
                        var medMed = (from a in db2.MedicoMedicamento
                                      where a.clave == res2.clave
                                      select a).FirstOrDefault();

                        if (medMed == null)
                        {
                            //El nivel 5 es solo para las enfermeras
                            if (res2.nivel_21 != 5 || medCon2 != null)
                            {
                                var nivel = 0;
                                //Coordinadores nivel 4
                                if (User.IsInRole("Coordinador"))
                                {
                                    nivel = 4;
                                }
                                else
                                {
                                    if (User.IsInRole("Subespecialista"))
                                    {
                                        nivel = 3;
                                    }
                                    else
                                    {
                                        if (User.IsInRole("Especialistas"))
                                        {
                                            nivel = 2;
                                        }
                                        else
                                        {
                                            nivel = 1;
                                        }
                                    }
                                }

                                //System.Diagnostics.Debug.WriteLine(item.clave);

                                if (res2.nivel_21 <= nivel || res2.nivel_21 == null)
                                {
                                    //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                                    //System.Diagnostics.Debug.WriteLine(item.clave);


                                    var disponibilidad = "";
                                    if (medicamento.cactual > 0)
                                    {
                                        disponibilidad = "Disponible";
                                    }
                                    else
                                    {
                                        disponibilidad = "No disponible";
                                    }

                                    var medCon = (from a in db.MedicamentosControlados
                                                  where a.clave == res2.clave
                                                  select a).FirstOrDefault();

                                    if (medCon != null)
                                    {
                                        if (medCon.status == 1)
                                        {
                                            disponibilidad = "Controlado";
                                        }
                                        else
                                        {
                                            disponibilidad = "Disponible";
                                        }

                                    }


                                    if (res2.sobranteInv2022 == "2 ")
                                    {

                                        if (medicamento.cactual > 0)
                                        {

                                            var listamedicamentos = new Result
                                            {
                                                Clave = res2.clave,
                                                Descripcion_Sal = res2.descripcion_21,
                                                Descripcion_Grupo = res2.descripcion_grupo,
                                                Historial = res2.descripcion_grupo,
                                                Presentacion = res2.descripcion_21,
                                                catual = medicamento.cactual,
                                                Disponibilidad = disponibilidad,
                                            };

                                            medicamentos.Add(listamedicamentos);
                                        }
                                    }
                                    else
                                    {
                                        var listamedicamentos = new Result
                                        {
                                            Clave = res2.clave,
                                            Descripcion_Sal = res2.descripcion_21,
                                            Descripcion_Grupo = res2.descripcion_grupo,
                                            Historial = res2.descripcion_grupo,
                                            Presentacion = res2.descripcion_21,
                                            catual = medicamento.cactual,
                                            Disponibilidad = disponibilidad,
                                        };

                                        medicamentos.Add(listamedicamentos);
                                    }
                                }


                            }

                        }
                        else
                        {
                            if (User.Identity.GetUserName() == medMed.medico)
                            {
                                //El nivel 5 es solo para las enfermeras
                                if (res2.nivel_21 != 5 || medCon2 != null)
                                {
                                    var nivel = 0;
                                    //Coordinadores nivel 4
                                    if (User.IsInRole("Coordinador"))
                                    {
                                        nivel = 4;
                                    }
                                    else
                                    {
                                        if (User.IsInRole("Subespecialista"))
                                        {
                                            nivel = 3;
                                        }
                                        else
                                        {
                                            if (User.IsInRole("Especialistas"))
                                            {
                                                nivel = 2;
                                            }
                                            else
                                            {
                                                nivel = 1;
                                            }
                                        }
                                    }

                                    //System.Diagnostics.Debug.WriteLine(item.clave);

                                    if (res2.nivel_21 <= nivel || res2.nivel_21 == null)
                                    {
                                        //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                                        //System.Diagnostics.Debug.WriteLine(item.clave);


                                        var disponibilidad = "";
                                        if (medicamento.cactual > 0)
                                        {
                                            disponibilidad = "Disponible";
                                        }
                                        else
                                        {
                                            disponibilidad = "No disponible";
                                        }

                                        var medCon = (from a in db.MedicamentosControlados
                                                      where a.clave == res2.clave
                                                      select a).FirstOrDefault();

                                        if (medCon != null)
                                        {
                                            if (medCon.status == 1)
                                            {
                                                disponibilidad = "Controlado";
                                            }
                                            else
                                            {
                                                disponibilidad = "Disponible";
                                            }

                                        }


                                        if (res2.sobranteInv2022 == "2 ")
                                        {

                                            if (medicamento.cactual > 0)
                                            {

                                                var listamedicamentos = new Result
                                                {
                                                    Clave = res2.clave,
                                                    Descripcion_Sal = res2.descripcion_21,
                                                    Descripcion_Grupo = res2.descripcion_grupo,
                                                    Historial = res2.descripcion_grupo,
                                                    Presentacion = res2.descripcion_21,
                                                    catual = medicamento.cactual,
                                                    Disponibilidad = disponibilidad,
                                                };

                                                medicamentos.Add(listamedicamentos);
                                            }
                                        }
                                        else
                                        {
                                            var listamedicamentos = new Result
                                            {
                                                Clave = res2.clave,
                                                Descripcion_Sal = res2.descripcion_21,
                                                Descripcion_Grupo = res2.descripcion_grupo,
                                                Historial = res2.descripcion_grupo,
                                                Presentacion = res2.descripcion_21,
                                                catual = medicamento.cactual,
                                                Disponibilidad = disponibilidad,
                                            };

                                            medicamentos.Add(listamedicamentos);
                                        }
                                    }


                                }

                            }
                        }


                    }

                }

                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }


        public JsonResult RecetasMedMederos(string id)
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //RECETAS
            if (User.IsInRole("Mederos"))
            {

                string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_mederos where id_sustancia != 425 and cactual is not null";
                var result = db.Database.SqlQuery<RecetasMederos>(query);
                var res = result.ToList();

                var medicamentos = new List<Result>();

                foreach (var medicamento in res)
                {

                    /*
                    string query2 = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + medicamento.id_sustancia + "  and id_sustancia = 2650 ";
                    var result2 = db2.Database.SqlQuery<LstInv>(query2);
                    var res2 = result2.FirstOrDefault();
                    */

                    var res2 = (from a in db.Sustancia
                                join grupo in db.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                                from grupoIn in grupoX.DefaultIfEmpty()
                                where a.Id == medicamento.id_sustancia
                                where a.descripcion_21 != ""
                                where a.descripcion_21 != null
                                select new
                                {
                                    descripcion_21 = a.descripcion_21,
                                    clave = a.Clave,
                                    nivel_21 = a.Nivel_21,
                                    sobranteInv2022 = a.SobranteInv2022,
                                    descripcion_grupo = grupoIn.descripcion,
                                }).FirstOrDefault();


                    //System.Diagnostics.Debug.WriteLine(res2);

                    if (res2 != null)
                    {

                        var medCon2 = (from a in db.MedicamentosControlados
                                       where a.clave == res2.clave
                                       select a).FirstOrDefault();

                        var medMed = (from a in db2.MedicoMedicamento
                                      where a.clave == res2.clave
                                      select a).FirstOrDefault();

                        if (medMed == null)
                        {
                            //El nivel 5 es solo para las enfermeras
                            if (res2.nivel_21 != 5 || medCon2 != null)
                            {
                                var nivel = 0;
                                //Coordinadores nivel 4
                                if (User.IsInRole("Coordinador"))
                                {
                                    nivel = 4;
                                }
                                else
                                {
                                    if (User.IsInRole("Subespecialista"))
                                    {
                                        nivel = 3;
                                    }
                                    else
                                    {
                                        if (User.IsInRole("Especialistas"))
                                        {
                                            nivel = 2;
                                        }
                                        else
                                        {
                                            nivel = 1;
                                        }
                                    }
                                }

                                //System.Diagnostics.Debug.WriteLine(item.clave);

                                if (res2.nivel_21 <= nivel || res2.nivel_21 == null)
                                {
                                    //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                                    //System.Diagnostics.Debug.WriteLine(item.clave);


                                    var disponibilidad = "";
                                    if (medicamento.cactual > 0)
                                    {
                                        disponibilidad = "Disponible";
                                    }
                                    else
                                    {
                                        disponibilidad = "No disponible";
                                    }

                                    var medCon = (from a in db.MedicamentosControlados
                                                  where a.clave == res2.clave
                                                  select a).FirstOrDefault();

                                    if (medCon != null)
                                    {
                                        if (medCon.status == 1)
                                        {
                                            disponibilidad = "Controlado";
                                        }
                                        else
                                        {
                                            disponibilidad = "Disponible";
                                        }

                                    }


                                    if (res2.sobranteInv2022 == "2 ")
                                    {

                                        if (medicamento.cactual > 0)
                                        {

                                            var listamedicamentos = new Result
                                            {
                                                Clave = res2.clave,
                                                Descripcion_Sal = res2.descripcion_21,
                                                Descripcion_Grupo = res2.descripcion_grupo,
                                                Historial = res2.descripcion_grupo,
                                                Presentacion = res2.descripcion_21,
                                                catual = medicamento.cactual,
                                                Disponibilidad = disponibilidad,
                                            };

                                            medicamentos.Add(listamedicamentos);
                                        }
                                    }
                                    else
                                    {
                                        var listamedicamentos = new Result
                                        {
                                            Clave = res2.clave,
                                            Descripcion_Sal = res2.descripcion_21,
                                            Descripcion_Grupo = res2.descripcion_grupo,
                                            Historial = res2.descripcion_grupo,
                                            Presentacion = res2.descripcion_21,
                                            catual = medicamento.cactual,
                                            Disponibilidad = disponibilidad,
                                        };

                                        medicamentos.Add(listamedicamentos);
                                    }
                                }


                            }

                        }
                        else
                        {
                            if (User.Identity.GetUserName() == medMed.medico)
                            {
                                //El nivel 5 es solo para las enfermeras
                                if (res2.nivel_21 != 5 || medCon2 != null)
                                {
                                    var nivel = 0;
                                    //Coordinadores nivel 4
                                    if (User.IsInRole("Coordinador"))
                                    {
                                        nivel = 4;
                                    }
                                    else
                                    {
                                        if (User.IsInRole("Subespecialista"))
                                        {
                                            nivel = 3;
                                        }
                                        else
                                        {
                                            if (User.IsInRole("Especialistas"))
                                            {
                                                nivel = 2;
                                            }
                                            else
                                            {
                                                nivel = 1;
                                            }
                                        }
                                    }

                                    //System.Diagnostics.Debug.WriteLine(item.clave);

                                    if (res2.nivel_21 <= nivel || res2.nivel_21 == null)
                                    {
                                        //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                                        //System.Diagnostics.Debug.WriteLine(item.clave);


                                        var disponibilidad = "";
                                        if (medicamento.cactual > 0)
                                        {
                                            disponibilidad = "Disponible";
                                        }
                                        else
                                        {
                                            disponibilidad = "No disponible";
                                        }

                                        var medCon = (from a in db.MedicamentosControlados
                                                      where a.clave == res2.clave
                                                      select a).FirstOrDefault();

                                        if (medCon != null)
                                        {
                                            if (medCon.status == 1)
                                            {
                                                disponibilidad = "Controlado";
                                            }
                                            else
                                            {
                                                disponibilidad = "Disponible";
                                            }

                                        }


                                        if (res2.sobranteInv2022 == "2 ")
                                        {

                                            if (medicamento.cactual > 0)
                                            {

                                                var listamedicamentos = new Result
                                                {
                                                    Clave = res2.clave,
                                                    Descripcion_Sal = res2.descripcion_21,
                                                    Descripcion_Grupo = res2.descripcion_grupo,
                                                    Historial = res2.descripcion_grupo,
                                                    Presentacion = res2.descripcion_21,
                                                    catual = medicamento.cactual,
                                                    Disponibilidad = disponibilidad,
                                                };

                                                medicamentos.Add(listamedicamentos);
                                            }
                                        }
                                        else
                                        {
                                            var listamedicamentos = new Result
                                            {
                                                Clave = res2.clave,
                                                Descripcion_Sal = res2.descripcion_21,
                                                Descripcion_Grupo = res2.descripcion_grupo,
                                                Historial = res2.descripcion_grupo,
                                                Presentacion = res2.descripcion_21,
                                                catual = medicamento.cactual,
                                                Disponibilidad = disponibilidad,
                                            };

                                            medicamentos.Add(listamedicamentos);
                                        }
                                    }


                                }

                            }
                        }


                    }

                }

                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }



        public ActionResult RecetasCU(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("Ciudad Universitaria"))
                {

                    Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();


                    return View(dhabientes);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }

        }


        public ActionResult RecetasUnidadERSEMAC(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("UnidadERSEMAC"))
                {

                    Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();


                    return View(dhabientes);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }

        }



        public ActionResult RecetasUnidadERMederos(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("UnidadERMederos"))
                {

                    Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();


                    return View(dhabientes);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }

        }


        public ActionResult RecetasFarmaco(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("RecetasFarmaco"))
                {

                    Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();


                    return View(dhabientes);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }

        }



        public JsonResult RecetasMedCU(string id)
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //RECETAS
            if (User.IsInRole("Ciudad Universitaria"))
            {

                string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_cu where id_sustancia != 425 and cactual is not null";
                var result = db.Database.SqlQuery<RecetasMederos>(query);
                var res = result.ToList();

                var medicamentos = new List<Result>();

                foreach (var medicamento in res)
                {

                    /*
                    string query2 = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + medicamento.id_sustancia + "  and id_sustancia = 2650 ";
                    var result2 = db2.Database.SqlQuery<LstInv>(query2);
                    var res2 = result2.FirstOrDefault();
                    */

                    var res2 = (from a in db.Sustancia
                                join grupo in db.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                                from grupoIn in grupoX.DefaultIfEmpty()
                                where a.Id == medicamento.id_sustancia
                                where a.descripcion_21 != ""
                                where a.descripcion_21 != null
                                select new
                                {
                                    descripcion_21 = a.descripcion_21,
                                    clave = a.Clave,
                                    nivel_21 = a.Nivel_21,
                                    sobranteInv2022 = a.SobranteInv2022,
                                    descripcion_grupo = grupoIn.descripcion,
                                }).FirstOrDefault();


                    //System.Diagnostics.Debug.WriteLine(res2);

                    if (res2 != null)
                    {

                        var medCon2 = (from a in db.MedicamentosControlados
                                       where a.clave == res2.clave
                                       select a).FirstOrDefault();

                        var medMed = (from a in db2.MedicoMedicamento
                                      where a.clave == res2.clave
                                      select a).FirstOrDefault();

                        //
                        if (medMed == null)
                        {
                            //El nivel 5 es solo para las enfermeras
                            if (res2.nivel_21 != 5 || medCon2 != null)
                            {
                                var nivel = 0;
                                //Coordinadores nivel 4
                                if (User.IsInRole("Coordinador"))
                                {
                                    nivel = 4;
                                }
                                else
                                {
                                    if (User.IsInRole("Subespecialista"))
                                    {
                                        nivel = 3;
                                    }
                                    else
                                    {
                                        if (User.IsInRole("Especialistas"))
                                        {
                                            nivel = 2;
                                        }
                                        else
                                        {
                                            nivel = 1;
                                        }
                                    }
                                }

                                //System.Diagnostics.Debug.WriteLine(item.clave);

                                if (res2.nivel_21 <= nivel || res2.nivel_21 == null)
                                {
                                    //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                                    //System.Diagnostics.Debug.WriteLine(item.clave);


                                    var disponibilidad = "";
                                    if (medicamento.cactual > 0)
                                    {
                                        disponibilidad = "Disponible";
                                    }
                                    else
                                    {
                                        disponibilidad = "No disponible";
                                    }

                                    var medCon = (from a in db.MedicamentosControlados
                                                  where a.clave == res2.clave
                                                  select a).FirstOrDefault();

                                    if (medCon != null)
                                    {
                                        if (medCon.status == 1)
                                        {
                                            disponibilidad = "Controlado";
                                        }
                                        else
                                        {
                                            disponibilidad = "Disponible";
                                        }

                                    }


                                    if (res2.sobranteInv2022 == "2 ")
                                    {

                                        if (medicamento.cactual > 0)
                                        {

                                            var listamedicamentos = new Result
                                            {
                                                Clave = res2.clave,
                                                Descripcion_Sal = res2.descripcion_21,
                                                Descripcion_Grupo = res2.descripcion_grupo,
                                                Historial = res2.descripcion_grupo,
                                                Presentacion = res2.descripcion_21,
                                                catual = medicamento.cactual,
                                                Disponibilidad = disponibilidad,
                                            };

                                            medicamentos.Add(listamedicamentos);
                                        }
                                    }
                                    else
                                    {
                                        var listamedicamentos = new Result
                                        {
                                            Clave = res2.clave,
                                            Descripcion_Sal = res2.descripcion_21,
                                            Descripcion_Grupo = res2.descripcion_grupo,
                                            Historial = res2.descripcion_grupo,
                                            Presentacion = res2.descripcion_21,
                                            catual = medicamento.cactual,
                                            Disponibilidad = disponibilidad,
                                        };

                                        medicamentos.Add(listamedicamentos);
                                    }
                                }


                            }

                        }
                        else
                        {
                            if (User.Identity.GetUserName() == medMed.medico)
                            {
                                //El nivel 5 es solo para las enfermeras
                                if (res2.nivel_21 != 5 || medCon2 != null)
                                {
                                    var nivel = 0;
                                    //Coordinadores nivel 4
                                    if (User.IsInRole("Coordinador"))
                                    {
                                        nivel = 4;
                                    }
                                    else
                                    {
                                        if (User.IsInRole("Subespecialista"))
                                        {
                                            nivel = 3;
                                        }
                                        else
                                        {
                                            if (User.IsInRole("Especialistas"))
                                            {
                                                nivel = 2;
                                            }
                                            else
                                            {
                                                nivel = 1;
                                            }
                                        }
                                    }

                                    //System.Diagnostics.Debug.WriteLine(item.clave);

                                    if (res2.nivel_21 <= nivel || res2.nivel_21 == null)
                                    {
                                        //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                                        //System.Diagnostics.Debug.WriteLine(item.clave);


                                        var disponibilidad = "";
                                        if (medicamento.cactual > 0)
                                        {
                                            disponibilidad = "Disponible";
                                        }
                                        else
                                        {
                                            disponibilidad = "No disponible";
                                        }

                                        var medCon = (from a in db.MedicamentosControlados
                                                      where a.clave == res2.clave
                                                      select a).FirstOrDefault();

                                        if (medCon != null)
                                        {
                                            if (medCon.status == 1)
                                            {
                                                disponibilidad = "Controlado";
                                            }
                                            else
                                            {
                                                disponibilidad = "Disponible";
                                            }

                                        }


                                        if (res2.sobranteInv2022 == "2 ")
                                        {

                                            if (medicamento.cactual > 0)
                                            {

                                                var listamedicamentos = new Result
                                                {
                                                    Clave = res2.clave,
                                                    Descripcion_Sal = res2.descripcion_21,
                                                    Descripcion_Grupo = res2.descripcion_grupo,
                                                    Historial = res2.descripcion_grupo,
                                                    Presentacion = res2.descripcion_21,
                                                    catual = medicamento.cactual,
                                                    Disponibilidad = disponibilidad,
                                                };

                                                medicamentos.Add(listamedicamentos);
                                            }
                                        }
                                        else
                                        {
                                            var listamedicamentos = new Result
                                            {
                                                Clave = res2.clave,
                                                Descripcion_Sal = res2.descripcion_21,
                                                Descripcion_Grupo = res2.descripcion_grupo,
                                                Historial = res2.descripcion_grupo,
                                                Presentacion = res2.descripcion_21,
                                                catual = medicamento.cactual,
                                                Disponibilidad = disponibilidad,
                                            };

                                            medicamentos.Add(listamedicamentos);
                                        }
                                    }


                                }

                            }
                        }


                    }


                }

                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public JsonResult RecetasMedFarmaco(string id)
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //RECETAS
            if (User.IsInRole("RecetasFarmaco"))
            {

                string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_farmaco where id_sustancia != 425 and cactual is not null";
                var result = db.Database.SqlQuery<RecetasMederos>(query);
                var res = result.ToList();

                var medicamentos = new List<Result>();

                foreach (var medicamento in res)
                {

                    /*
                    string query2 = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + medicamento.id_sustancia + "  and id_sustancia = 2650 ";
                    var result2 = db2.Database.SqlQuery<LstInv>(query2);
                    var res2 = result2.FirstOrDefault();
                    */

                    var res2 = (from a in db.Sustancia
                                join grupo in db.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                                from grupoIn in grupoX.DefaultIfEmpty()
                                where a.Id == medicamento.id_sustancia
                                where a.descripcion_21 != ""
                                where a.descripcion_21 != null
                                select new
                                {
                                    descripcion_21 = a.descripcion_21,
                                    clave = a.Clave,
                                    nivel_21 = a.Nivel_21,
                                    sobranteInv2022 = a.SobranteInv2022,
                                    descripcion_grupo = grupoIn.descripcion,
                                }).FirstOrDefault();


                    //System.Diagnostics.Debug.WriteLine(res2);

                    if (res2 != null)
                    {

                        var medCon2 = (from a in db.MedicamentosControlados
                                       where a.clave == res2.clave
                                       select a).FirstOrDefault();

                        //El nivel 5 es solo para las enfermeras
                        if (res2.nivel_21 != 5 || medCon2 != null)
                        {
                            var nivel = 0;
                            //Coordinadores nivel 4
                            if (User.IsInRole("Coordinador"))
                            {
                                nivel = 4;
                            }
                            else
                            {
                                if (User.IsInRole("Subespecialista"))
                                {
                                    nivel = 3;
                                }
                                else
                                {
                                    if (User.IsInRole("Especialistas"))
                                    {
                                        nivel = 2;
                                    }
                                    else
                                    {
                                        nivel = 1;
                                    }
                                }
                            }

                            //System.Diagnostics.Debug.WriteLine(item.clave);

                            if (res2.nivel_21 <= nivel || res2.nivel_21 == null)
                            {
                                //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                                //System.Diagnostics.Debug.WriteLine(item.clave);


                                var disponibilidad = "";
                                if (medicamento.cactual > 0)
                                {
                                    disponibilidad = "Disponible";
                                }
                                else
                                {
                                    disponibilidad = "No disponible";
                                }

                                var medCon = (from a in db.MedicamentosControlados
                                              where a.clave == res2.clave
                                              select a).FirstOrDefault();

                                if (medCon != null)
                                {
                                    if (medCon.status == 1)
                                    {
                                        disponibilidad = "Controlado";
                                    }
                                    else
                                    {
                                        disponibilidad = "Disponible";
                                    }

                                }


                                if (res2.sobranteInv2022 == "2 ")
                                {

                                    if (medicamento.cactual > 0)
                                    {

                                        var listamedicamentos = new Result
                                        {
                                            Clave = res2.clave,
                                            Descripcion_Sal = res2.descripcion_21,
                                            Descripcion_Grupo = res2.descripcion_grupo,
                                            Historial = res2.descripcion_grupo,
                                            Presentacion = res2.descripcion_21,
                                            catual = medicamento.cactual,
                                            Disponibilidad = disponibilidad,
                                        };

                                        medicamentos.Add(listamedicamentos);
                                    }
                                }
                                else
                                {
                                    var listamedicamentos = new Result
                                    {
                                        Clave = res2.clave,
                                        Descripcion_Sal = res2.descripcion_21,
                                        Descripcion_Grupo = res2.descripcion_grupo,
                                        Historial = res2.descripcion_grupo,
                                        Presentacion = res2.descripcion_21,
                                        catual = medicamento.cactual,
                                        Disponibilidad = disponibilidad,
                                    };

                                    medicamentos.Add(listamedicamentos);
                                }
                            }


                        }

                    }


                }

                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }


        public JsonResult RecetasMedUERSEMAC(string id)
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //RECETAS
            if (User.IsInRole("UnidadERSEMAC"))
            {

                string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_udersemac";
                var result = db.Database.SqlQuery<RecetasCU>(query);
                var res = result.ToList();

                var medicamentos = new List<Result>();

                foreach (var medicamento in res)
                {
                    string query2 = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + medicamento.id_sustancia + "";
                    var result2 = db2.Database.SqlQuery<LstInv>(query2);
                    var res2 = result2.FirstOrDefault();

                    //System.Diagnostics.Debug.WriteLine(inventariofarmacia.Clave);

                    var disponibilidad = "";
                    if (medicamento.cactual > 0)
                    {
                        disponibilidad = "Disponible";
                    }
                    else
                    {
                        disponibilidad = "No disponible";
                    }

                    var medCon = (from a in db.MedicamentosControlados
                                  where a.clave == res2.clave
                                  select a).FirstOrDefault();

                    if (medCon != null)
                    {
                        if (medCon.status == 1)
                        {
                            disponibilidad = "Controlado";
                        }
                        else
                        {
                            disponibilidad = "Disponible";
                        }

                    }

                    if (res2 != null)
                    {
                        if (res2.sobranteinv2022 == "2 ")
                        {

                            if (medicamento.cactual > 0)
                            {

                                var listamedicamentos = new Result
                                {
                                    Clave = res2.clave,
                                    Descripcion_Sal = res2.descripcion_sal,
                                    Descripcion_Grupo = res2.descripcion_grupo,
                                    Historial = res2.descripcion_grupo,
                                    Presentacion = res2.presentacion,
                                    catual = medicamento.cactual,
                                    Disponibilidad = disponibilidad,
                                };

                                medicamentos.Add(listamedicamentos);
                            }
                        }
                        else
                        {
                            var listamedicamentos = new Result
                            {
                                Clave = res2.clave,
                                Descripcion_Sal = res2.descripcion_sal,
                                Descripcion_Grupo = res2.descripcion_grupo,
                                Historial = res2.descripcion_grupo,
                                Presentacion = res2.presentacion,
                                catual = medicamento.cactual,
                                Disponibilidad = disponibilidad,
                            };

                            medicamentos.Add(listamedicamentos);
                        }
                    }


                }

                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }


        public JsonResult RecetasMedUERMederos(string id)
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //RECETAS
            if (User.IsInRole("UnidadERMederos"))
            {

                string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_udermederos";
                var result = db.Database.SqlQuery<RecetasCU>(query);
                var res = result.ToList();

                var medicamentos = new List<Result>();

                foreach (var medicamento in res)
                {
                    string query2 = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + medicamento.id_sustancia + "";
                    var result2 = db2.Database.SqlQuery<LstInv>(query2);
                    var res2 = result2.FirstOrDefault();

                    //System.Diagnostics.Debug.WriteLine(inventariofarmacia.Clave);

                    var disponibilidad = "";
                    if (medicamento.cactual > 0)
                    {
                        disponibilidad = "Disponible";
                    }
                    else
                    {
                        disponibilidad = "No disponible";
                    }

                    var medCon = (from a in db.MedicamentosControlados
                                  where a.clave == res2.clave
                                  select a).FirstOrDefault();

                    if (medCon != null)
                    {
                        if (medCon.status == 1)
                        {
                            disponibilidad = "Controlado";
                        }
                        else
                        {
                            disponibilidad = "Disponible";
                        }

                    }

                    if (res2 != null)
                    {
                        if (res2.sobranteinv2022 == "2 ")
                        {

                            if (medicamento.cactual > 0)
                            {

                                var listamedicamentos = new Result
                                {
                                    Clave = res2.clave,
                                    Descripcion_Sal = res2.descripcion_sal,
                                    Descripcion_Grupo = res2.descripcion_grupo,
                                    Historial = res2.descripcion_grupo,
                                    Presentacion = res2.presentacion,
                                    catual = medicamento.cactual,
                                    Disponibilidad = disponibilidad,
                                };

                                medicamentos.Add(listamedicamentos);
                            }
                        }
                        else
                        {
                            var listamedicamentos = new Result
                            {
                                Clave = res2.clave,
                                Descripcion_Sal = res2.descripcion_sal,
                                Descripcion_Grupo = res2.descripcion_grupo,
                                Historial = res2.descripcion_grupo,
                                Presentacion = res2.presentacion,
                                catual = medicamento.cactual,
                                Disponibilidad = disponibilidad,
                            };

                            medicamentos.Add(listamedicamentos);
                        }
                    }



                }

                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }


        public JsonResult RecetasMed(string id)
        {
            Models.SERVMEDEntities4 db4 = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            //RECETAS
            if (User.IsInRole("Especialistas"))
            {
                var inventariofarmacia = (from q in db2.InventarioFarmacia
                                          join sustan in db2.Sustancia on q.Clave equals sustan.Clave into susX
                                          from susIn in susX.DefaultIfEmpty()
                                          where susIn.Consultorio != "0"
                                          //where q.Inv_Act != 0
                                          select new
                                          {
                                              Clave = q.Clave,
                                              //IdSustancia = susIn.Id,
                                              Descripcion_Sal = q.Descripcion_Sal,
                                              Descripcion_Grupo = q.Descripcion_Grupo,
                                              Presentacion = q.Presentacion,
                                              Inv_Act = q.Inv_Act,
                                              Clave_Nivel = q.Clave_Nivel,
                                          })
                                         .OrderByDescending(g => g.Descripcion_Grupo)
                                         .ToList();

                var medicamentos = new List<Result>();

                foreach (var medicamento in inventariofarmacia)
                {

                    //ALERGIAS
                    //var pacAlergico = false;
                    var disponibilidad = "";
                    /*var alergias = (from d in db2.Alergias_Exp
                                    where d.num_exp == id
                                    where d.estado == 1
                                    select new
                                    {
                                        medicamento = d.medicamento,
                                    })
                               .ToList();*/

                    //foreach(var alergia in alergias)
                    //{
                    //string query = "SELECT * FROM InventarioFarmacia WHERE Descripcion_Sal like '%" + alergia.medicamento + "%' COLLATE Latin1_General_CI_AI and Clave = '" + medicamento.Clave + "'";
                    //var result = db2.Database.SqlQuery<InventarioFarmacia>(query);
                    //var res = result.FirstOrDefault();

                    if (medicamento.Inv_Act > 0)
                    {
                        //if (res != null)
                        //{

                        //disponibilidad = "No disponible por alergia";
                        //}
                        //else
                        //{
                        disponibilidad = "Disponible";
                        //}
                        //}
                        //else
                        //{
                        //if (res != null)
                        //{

                        //disponibilidad = "No disponible por alergia";
                    }
                    else
                    {
                        disponibilidad = "No disponible";
                    }
                    //}
                    //}
                    var presentacion = "";

                    var desMed = (from q in db4.Sustancia
                                  where q.Clave == medicamento.Clave
                                  where q.descripcion_21 != null && q.descripcion_21 != ""
                                  select new
                                  {
                                      Presentacion = q.descripcion_21,
                                  })
                                  .FirstOrDefault();

                    if (desMed != null)
                    {
                        presentacion = desMed.Presentacion;
                    }
                    else
                    {
                        presentacion = medicamento.Presentacion;
                    }

                    //System.Diagnostics.Debug.WriteLine(presentacion);


                    //ALERGIAS


                    //COORDINADORES NIVEL 7
                    if (User.IsInRole("Coordinador"))
                    {

                        //Revisar que no esten deshabilitados
                        var inhabmed = (from q in db4.InhabilitarMedicamentos
                                        where q.clave == medicamento.Clave
                                        select q).FirstOrDefault();

                        //if(medicamento.Clave != "293901" && medicamento.Clave != "442401" && medicamento.Clave != "442701" && medicamento.Clave != "350604" && medicamento.Clave != "470201" && medicamento.Clave != "081202" && medicamento.Clave != "213501" && medicamento.Clave != "255101" && medicamento.Clave != "264501" && medicamento.Clave != "300101" && medicamento.Clave != "310801" && medicamento.Clave != "358201" && medicamento.Clave != "356601" && medicamento.Clave != "356602" && medicamento.Clave != "401201" && medicamento.Clave != "420601" && medicamento.Clave != "430901" && medicamento.Clave != "501101" && medicamento.Clave != "021701" && medicamento.Clave != "070701" && medicamento.Clave != "300102" && medicamento.Clave != "440601" && medicamento.Clave != "211102" && medicamento.Clave != "301802" && medicamento.Clave != "356201" && medicamento.Clave != "430602" && medicamento.Clave != "070402" && medicamento.Clave != "90505" && medicamento.Clave != "254701" && medicamento.Clave != "213401" && medicamento.Clave != "474701" && medicamento.Clave != "502301" && medicamento.Clave != "232002" && medicamento.Clave != "180202" && medicamento.Clave != "502301" && medicamento.Clave != "361201" && medicamento.Clave != "390501" && medicamento.Clave != "150301" && medicamento.Clave != "452403" && medicamento.Clave != "232003" && medicamento.Clave != "313401" && medicamento.Clave != "081201" && medicamento.Clave != "293501" && medicamento.Clave != "090508" && medicamento.Clave != "262602" && medicamento.Clave != "392002" && medicamento.Clave != "392021" && medicamento.Clave != "430101" && medicamento.Clave != "061001" && medicamento.Clave != "392003" && medicamento.Clave != "061005" && medicamento.Clave != "251402" && medicamento.Clave != "262601" && medicamento.Clave != "257101" && medicamento.Clave != "232001" && medicamento.Clave != "314103")
                        //if (medicamento.Clave != "293901" && medicamento.Clave != "442401" && medicamento.Clave != "442701" && medicamento.Clave != "350604" && medicamento.Clave != "470201" && medicamento.Clave != "081202" && medicamento.Clave != "213501" && medicamento.Clave != "255101" && medicamento.Clave != "264501" && medicamento.Clave != "300101" && medicamento.Clave != "310801" && medicamento.Clave != "358201" && medicamento.Clave != "356601" && medicamento.Clave != "356602" && medicamento.Clave != "401201" && medicamento.Clave != "420601" && medicamento.Clave != "430901" && medicamento.Clave != "501101" && medicamento.Clave != "021701" && medicamento.Clave != "070701" && medicamento.Clave != "300102" && medicamento.Clave != "440601" && medicamento.Clave != "211102" && medicamento.Clave != "301802" && medicamento.Clave != "356201" && medicamento.Clave != "430602" && medicamento.Clave != "070402" && medicamento.Clave != "90505" && medicamento.Clave != "254701" && medicamento.Clave != "213401" && medicamento.Clave != "474701" && medicamento.Clave != "502301" && medicamento.Clave != "232002" && medicamento.Clave != "180202" && medicamento.Clave != "502301" && medicamento.Clave != "361201" && medicamento.Clave != "390501" && medicamento.Clave != "150301" && medicamento.Clave != "452403" && medicamento.Clave != "232003" && medicamento.Clave != "262602" && medicamento.Clave != "392002" && medicamento.Clave != "430101" && medicamento.Clave != "392003" && medicamento.Clave != "262601" && medicamento.Clave != "257101" && medicamento.Clave != "232001" && medicamento.Clave != "314103" && medicamento.Clave != "392021")
                        //if(inhabmed == null)
                        //{
                        var listamedicamentos = new Result
                        {
                            Clave = medicamento.Clave,
                            Descripcion_Sal = medicamento.Descripcion_Sal,
                            Descripcion_Grupo = medicamento.Descripcion_Grupo,
                            Historial = medicamento.Descripcion_Grupo,
                            //Presentacion = presentacion,
                            Presentacion = medicamento.Presentacion,
                            //Inv_Act = medicamento.Inv_Act,
                            Disponibilidad = disponibilidad,
                            //pacAlergico = pacAlergico,
                        };

                        medicamentos.Add(listamedicamentos);
                        //}


                    }
                    else
                    {
                        if (medicamento.Clave_Nivel != "7")
                        {
                            //Revisar que no esten deshabilitados
                            var inhabmed = (from q in db4.InhabilitarMedicamentos
                                            where q.clave == medicamento.Clave
                                            select q).FirstOrDefault();

                            //if(medicamento.Clave != "293901" && medicamento.Clave != "442401" && medicamento.Clave != "442701" && medicamento.Clave != "350604" && medicamento.Clave != "470201" && medicamento.Clave != "081202" && medicamento.Clave != "213501" && medicamento.Clave != "255101" && medicamento.Clave != "264501" && medicamento.Clave != "300101" && medicamento.Clave != "310801" && medicamento.Clave != "358201" && medicamento.Clave != "356601" && medicamento.Clave != "356602" && medicamento.Clave != "401201" && medicamento.Clave != "420601" && medicamento.Clave != "430901" && medicamento.Clave != "501101" && medicamento.Clave != "021701" && medicamento.Clave != "070701" && medicamento.Clave != "300102" && medicamento.Clave != "440601" && medicamento.Clave != "211102" && medicamento.Clave != "301802" && medicamento.Clave != "356201" && medicamento.Clave != "430602" && medicamento.Clave != "070402" && medicamento.Clave != "90505" && medicamento.Clave != "254701" && medicamento.Clave != "213401" && medicamento.Clave != "474701" && medicamento.Clave != "502301" && medicamento.Clave != "232002" && medicamento.Clave != "180202" && medicamento.Clave != "502301" && medicamento.Clave != "361201" && medicamento.Clave != "390501" && medicamento.Clave != "150301" && medicamento.Clave != "452403" && medicamento.Clave != "232003" && medicamento.Clave != "313401" && medicamento.Clave != "081201" && medicamento.Clave != "293501" && medicamento.Clave != "090508" && medicamento.Clave != "262602" && medicamento.Clave != "392002" && medicamento.Clave != "392021" && medicamento.Clave != "430101" && medicamento.Clave != "061001" && medicamento.Clave != "392003" && medicamento.Clave != "061005" && medicamento.Clave != "251402" && medicamento.Clave != "262601" && medicamento.Clave != "257101" && medicamento.Clave != "232001" && medicamento.Clave != "314103")
                            //if (medicamento.Clave != "293901" && medicamento.Clave != "442401" && medicamento.Clave != "442701" && medicamento.Clave != "350604" && medicamento.Clave != "470201" && medicamento.Clave != "081202" && medicamento.Clave != "213501" && medicamento.Clave != "255101" && medicamento.Clave != "264501" && medicamento.Clave != "300101" && medicamento.Clave != "310801" && medicamento.Clave != "358201" && medicamento.Clave != "356601" && medicamento.Clave != "356602" && medicamento.Clave != "401201" && medicamento.Clave != "420601" && medicamento.Clave != "430901" && medicamento.Clave != "501101" && medicamento.Clave != "021701" && medicamento.Clave != "070701" && medicamento.Clave != "300102" && medicamento.Clave != "440601" && medicamento.Clave != "211102" && medicamento.Clave != "301802" && medicamento.Clave != "356201" && medicamento.Clave != "430602" && medicamento.Clave != "070402" && medicamento.Clave != "90505" && medicamento.Clave != "254701" && medicamento.Clave != "213401" && medicamento.Clave != "474701" && medicamento.Clave != "502301" && medicamento.Clave != "232002" && medicamento.Clave != "180202" && medicamento.Clave != "502301" && medicamento.Clave != "361201" && medicamento.Clave != "390501" && medicamento.Clave != "150301" && medicamento.Clave != "452403" && medicamento.Clave != "232003" && medicamento.Clave != "262602" && medicamento.Clave != "392002" && medicamento.Clave != "430101" && medicamento.Clave != "392003" && medicamento.Clave != "262601" && medicamento.Clave != "257101" && medicamento.Clave != "232001" && medicamento.Clave != "314103" && medicamento.Clave != "392021")
                            //if (inhabmed == null)
                            //{
                            var listamedicamentos = new Result
                            {
                                Clave = medicamento.Clave,
                                Descripcion_Sal = medicamento.Descripcion_Sal,
                                Descripcion_Grupo = medicamento.Descripcion_Grupo,
                                Historial = medicamento.Descripcion_Grupo,
                                //Presentacion = presentacion,
                                Presentacion = medicamento.Presentacion,
                                //Inv_Act = medicamento.Inv_Act,
                                Disponibilidad = disponibilidad,
                                //pacAlergico = pacAlergico,
                            };

                            medicamentos.Add(listamedicamentos);
                            //}
                        }
                    }
                }

                //Buscar si ya se le ha dado medicamento anteriormente
                var fecha_unanio = DateTime.Now.AddYears(-1).ToString("yyyy-MM-ddT00:00:00.000");
                var fecha_unanio_correcta = DateTime.Parse(fecha_unanio);

                Models.SERVMEDEntities4 db3 = new Models.SERVMEDEntities4();
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var receta_resur_detalles = (from d in db3.Receta_Detalle
                                             join recetaExp in db3.Receta_Exp on d.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                             from receExp in reExp.DefaultIfEmpty()
                                                 //Expediente va a ser igual al que se envia
                                             where receExp.Expediente == id
                                             //De hace un año
                                             where receExp.Fecha >= fecha_unanio_correcta
                                             select new
                                             {
                                                 Folio_Rcta = d.Folio_Rcta,
                                                 Codigo = d.Codigo,
                                                 //meses_surtir = receExp.meses_surtir,
                                             })
                                             .GroupBy(p => p.Codigo)
                                             .Select(g => new
                                             {
                                                 Codigo = g.Key,
                                             })
                                             .ToList();

                //System.Diagnostics.Debug.WriteLine(receta_resur_detalles);

                foreach (var item in receta_resur_detalles)
                {
                    Models.SERVMEDEntities4 db6 = new Models.SERVMEDEntities4();

                    var inventariofarmacia2 = (from q in db.InventarioFarmacia
                                                   //join sustan in db.Sustancia on q.Clave equals sustan.Clave into susX
                                                   //from susIn in susX.DefaultIfEmpty()
                                                   //where susIn.Consultorio != "0"
                                               where q.Clave == item.Codigo
                                               where q.Clave_Nivel != "2"
                                               where q.Clave_Nivel != "1"
                                               // where q.Inv_Act != 0
                                               select new
                                               {
                                                   Clave = q.Clave,
                                                   Descripcion_Sal = q.Descripcion_Sal,
                                                   Descripcion_Grupo = q.Descripcion_Grupo,
                                                   Presentacion = q.Presentacion,
                                                   Inv_Act = q.Inv_Act,
                                               })
                                          .OrderByDescending(g => g.Descripcion_Grupo)
                                          .FirstOrDefault();


                    var presentacion = "";

                    var desMed = (from q in db4.Sustancia
                                  where q.Clave == item.Codigo
                                  where q.descripcion_21 != null && q.descripcion_21 != ""
                                  select new
                                  {
                                      Presentacion = q.descripcion_21,
                                  })
                                  .FirstOrDefault();

                    if (desMed != null)
                    {
                        presentacion = desMed.Presentacion;
                    }
                    else
                    {
                        if (inventariofarmacia2 != null)
                        {
                            presentacion = inventariofarmacia2.Presentacion;
                        }

                    }


                    var disponibilidad = "";


                    if (inventariofarmacia2 != null)
                    {

                        if (inventariofarmacia2.Inv_Act > 0)
                        {
                            disponibilidad = "Disponible";
                        }
                        else
                        {
                            disponibilidad = "No disponible";
                        }

                        //Revisar que no esten deshabilitados
                        var inhabmed = (from q in db4.InhabilitarMedicamentos
                                        where q.clave == inventariofarmacia2.Clave
                                        select q).FirstOrDefault();

                        //if(medicamento.Clave != "293901" && medicamento.Clave != "442401" && medicamento.Clave != "442701" && medicamento.Clave != "350604" && medicamento.Clave != "470201" && medicamento.Clave != "081202" && medicamento.Clave != "213501" && medicamento.Clave != "255101" && medicamento.Clave != "264501" && medicamento.Clave != "300101" && medicamento.Clave != "310801" && medicamento.Clave != "358201" && medicamento.Clave != "356601" && medicamento.Clave != "356602" && medicamento.Clave != "401201" && medicamento.Clave != "420601" && medicamento.Clave != "430901" && medicamento.Clave != "501101" && medicamento.Clave != "021701" && medicamento.Clave != "070701" && medicamento.Clave != "300102" && medicamento.Clave != "440601" && medicamento.Clave != "211102" && medicamento.Clave != "301802" && medicamento.Clave != "356201" && medicamento.Clave != "430602" && medicamento.Clave != "070402" && medicamento.Clave != "90505" && medicamento.Clave != "254701" && medicamento.Clave != "213401" && medicamento.Clave != "474701" && medicamento.Clave != "502301" && medicamento.Clave != "232002" && medicamento.Clave != "180202" && medicamento.Clave != "502301" && medicamento.Clave != "361201" && medicamento.Clave != "390501" && medicamento.Clave != "150301" && medicamento.Clave != "452403" && medicamento.Clave != "232003" && medicamento.Clave != "313401" && medicamento.Clave != "081201" && medicamento.Clave != "293501" && medicamento.Clave != "090508" && medicamento.Clave != "262602" && medicamento.Clave != "392002" && medicamento.Clave != "392021" && medicamento.Clave != "430101" && medicamento.Clave != "061001" && medicamento.Clave != "392003" && medicamento.Clave != "061005" && medicamento.Clave != "251402" && medicamento.Clave != "262601" && medicamento.Clave != "257101" && medicamento.Clave != "232001" && medicamento.Clave != "314103")
                        //if (medicamento.Clave != "293901" && medicamento.Clave != "442401" && medicamento.Clave != "442701" && medicamento.Clave != "350604" && medicamento.Clave != "470201" && medicamento.Clave != "081202" && medicamento.Clave != "213501" && medicamento.Clave != "255101" && medicamento.Clave != "264501" && medicamento.Clave != "300101" && medicamento.Clave != "310801" && medicamento.Clave != "358201" && medicamento.Clave != "356601" && medicamento.Clave != "356602" && medicamento.Clave != "401201" && medicamento.Clave != "420601" && medicamento.Clave != "430901" && medicamento.Clave != "501101" && medicamento.Clave != "021701" && medicamento.Clave != "070701" && medicamento.Clave != "300102" && medicamento.Clave != "440601" && medicamento.Clave != "211102" && medicamento.Clave != "301802" && medicamento.Clave != "356201" && medicamento.Clave != "430602" && medicamento.Clave != "070402" && medicamento.Clave != "90505" && medicamento.Clave != "254701" && medicamento.Clave != "213401" && medicamento.Clave != "474701" && medicamento.Clave != "502301" && medicamento.Clave != "232002" && medicamento.Clave != "180202" && medicamento.Clave != "502301" && medicamento.Clave != "361201" && medicamento.Clave != "390501" && medicamento.Clave != "150301" && medicamento.Clave != "452403" && medicamento.Clave != "232003" && medicamento.Clave != "262602" && medicamento.Clave != "392002" && medicamento.Clave != "430101" && medicamento.Clave != "392003" && medicamento.Clave != "262601" && medicamento.Clave != "257101" && medicamento.Clave != "232001" && medicamento.Clave != "314103" && medicamento.Clave != "392021")
                        //if (inhabmed == null)
                        //{

                        var result = new Result
                        {
                            Clave = inventariofarmacia2.Clave,
                            Descripcion_Sal = inventariofarmacia2.Descripcion_Sal,
                            Descripcion_Grupo = inventariofarmacia2.Descripcion_Grupo,
                            Historial = "1.- Historial de medicamentos especializados del paciente",
                            //Presentacion = presentacion,
                            Presentacion = inventariofarmacia2.Presentacion,
                            //Inv_Act = inventariofarmacia2.Inv_Act,
                            Disponibilidad = disponibilidad,
                        };

                        medicamentos.Add(result);

                        //}
                    }

                }

                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                //Si no es especialista, que solo me de los medicamentos de nivel 1 y 2
                var inventariofarmacia = (from q in db2.InventarioFarmacia
                                          join sustan in db2.Sustancia on q.Clave equals sustan.Clave into susX
                                          from susIn in susX.DefaultIfEmpty()
                                          where susIn.Consultorio != "0"
                                          where q.Clave_Nivel == "1" || q.Clave_Nivel == "2"
                                          //where q.Inv_Act != 0
                                          select new
                                          {
                                              Clave = q.Clave,
                                              Descripcion_Sal = q.Descripcion_Sal,
                                              Descripcion_Grupo = q.Descripcion_Grupo,
                                              Presentacion = q.Presentacion,
                                              Inv_Act = q.Inv_Act,
                                          })
                                          .OrderByDescending(g => g.Descripcion_Grupo)
                                          .ToList();

                var medicamentos = new List<Result>();

                foreach (var medicamento in inventariofarmacia)
                {

                    //Buscar presentacion en descripcion_21

                    var presentacion = "";

                    var desMed = (from q in db4.Sustancia
                                  where q.Clave == medicamento.Clave
                                  where q.descripcion_21 != null && q.descripcion_21 != ""
                                  select new
                                  {
                                      Presentacion = q.descripcion_21,
                                  })
                                  .FirstOrDefault();

                    if (desMed != null)
                    {
                        presentacion = desMed.Presentacion;
                    }
                    else
                    {
                        presentacion = medicamento.Presentacion;
                    }

                    var disponibilidad = "";
                    if (medicamento.Inv_Act > 0)
                    {
                        disponibilidad = "Disponible";
                    }
                    else
                    {
                        disponibilidad = "No disponible";
                    }

                    //Revisar que no esten deshabilitados
                    var inhabmed = (from q in db4.InhabilitarMedicamentos
                                    where q.clave == medicamento.Clave
                                    select q).FirstOrDefault();

                    //if(medicamento.Clave != "293901" && medicamento.Clave != "442401" && medicamento.Clave != "442701" && medicamento.Clave != "350604" && medicamento.Clave != "470201" && medicamento.Clave != "081202" && medicamento.Clave != "213501" && medicamento.Clave != "255101" && medicamento.Clave != "264501" && medicamento.Clave != "300101" && medicamento.Clave != "310801" && medicamento.Clave != "358201" && medicamento.Clave != "356601" && medicamento.Clave != "356602" && medicamento.Clave != "401201" && medicamento.Clave != "420601" && medicamento.Clave != "430901" && medicamento.Clave != "501101" && medicamento.Clave != "021701" && medicamento.Clave != "070701" && medicamento.Clave != "300102" && medicamento.Clave != "440601" && medicamento.Clave != "211102" && medicamento.Clave != "301802" && medicamento.Clave != "356201" && medicamento.Clave != "430602" && medicamento.Clave != "070402" && medicamento.Clave != "90505" && medicamento.Clave != "254701" && medicamento.Clave != "213401" && medicamento.Clave != "474701" && medicamento.Clave != "502301" && medicamento.Clave != "232002" && medicamento.Clave != "180202" && medicamento.Clave != "502301" && medicamento.Clave != "361201" && medicamento.Clave != "390501" && medicamento.Clave != "150301" && medicamento.Clave != "452403" && medicamento.Clave != "232003" && medicamento.Clave != "313401" && medicamento.Clave != "081201" && medicamento.Clave != "293501" && medicamento.Clave != "090508" && medicamento.Clave != "262602" && medicamento.Clave != "392002" && medicamento.Clave != "392021" && medicamento.Clave != "430101" && medicamento.Clave != "061001" && medicamento.Clave != "392003" && medicamento.Clave != "061005" && medicamento.Clave != "251402" && medicamento.Clave != "262601" && medicamento.Clave != "257101" && medicamento.Clave != "232001" && medicamento.Clave != "314103")
                    //if (medicamento.Clave != "293901" && medicamento.Clave != "442401" && medicamento.Clave != "442701" && medicamento.Clave != "350604" && medicamento.Clave != "470201" && medicamento.Clave != "081202" && medicamento.Clave != "213501" && medicamento.Clave != "255101" && medicamento.Clave != "264501" && medicamento.Clave != "300101" && medicamento.Clave != "310801" && medicamento.Clave != "358201" && medicamento.Clave != "356601" && medicamento.Clave != "356602" && medicamento.Clave != "401201" && medicamento.Clave != "420601" && medicamento.Clave != "430901" && medicamento.Clave != "501101" && medicamento.Clave != "021701" && medicamento.Clave != "070701" && medicamento.Clave != "300102" && medicamento.Clave != "440601" && medicamento.Clave != "211102" && medicamento.Clave != "301802" && medicamento.Clave != "356201" && medicamento.Clave != "430602" && medicamento.Clave != "070402" && medicamento.Clave != "90505" && medicamento.Clave != "254701" && medicamento.Clave != "213401" && medicamento.Clave != "474701" && medicamento.Clave != "502301" && medicamento.Clave != "232002" && medicamento.Clave != "180202" && medicamento.Clave != "502301" && medicamento.Clave != "361201" && medicamento.Clave != "390501" && medicamento.Clave != "150301" && medicamento.Clave != "452403" && medicamento.Clave != "232003" && medicamento.Clave != "262602" && medicamento.Clave != "392002" && medicamento.Clave != "430101" && medicamento.Clave != "392003" && medicamento.Clave != "262601" && medicamento.Clave != "257101" && medicamento.Clave != "232001" && medicamento.Clave != "314103" && medicamento.Clave != "392021")
                    //if (inhabmed == null)
                    //{
                    var listamedicamentos = new Result
                    {
                        Clave = medicamento.Clave,
                        Descripcion_Sal = medicamento.Descripcion_Sal,
                        Descripcion_Grupo = medicamento.Descripcion_Grupo,
                        Historial = medicamento.Descripcion_Grupo,
                        //Presentacion = presentacion,
                        Presentacion = medicamento.Presentacion,
                        //Inv_Act = inventariofarmacia2.Inv_Act,
                        Disponibilidad = disponibilidad,
                    };

                    medicamentos.Add(listamedicamentos);
                    //}
                }

                //Buscar si ya se le ha dado medicamento anteriormente
                var fecha_unanio = DateTime.Now.AddYears(-1).ToString("yyyy-MM-ddT00:00:00.000");
                var fecha_unanio_correcta = DateTime.Parse(fecha_unanio);

                Models.SERVMEDEntities4 db3 = new Models.SERVMEDEntities4();
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var receta_resur_detalles = (from d in db3.Receta_Detalle
                                             join recetaExp in db3.Receta_Exp on d.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                             from receExp in reExp.DefaultIfEmpty()
                                                 //Expediente va a ser igual al que se envia
                                             where receExp.Expediente == id
                                             //De hace un año
                                             where receExp.Fecha >= fecha_unanio_correcta
                                             select new
                                             {
                                                 Folio_Rcta = d.Folio_Rcta,
                                                 Codigo = d.Codigo,
                                                 //meses_surtir = receExp.meses_surtir,
                                             })
                                             .GroupBy(p => p.Codigo)
                                             .Select(g => new
                                             {
                                                 Codigo = g.Key,
                                             })
                                             .ToList();



                foreach (var item in receta_resur_detalles)
                {
                    Models.SERVMEDEntities4 db6 = new Models.SERVMEDEntities4();

                    var inventariofarmacia2 = (from q in db.InventarioFarmacia
                                                   //join sustan in db.Sustancia on q.Clave equals sustan.Clave into susX
                                                   //from susIn in susX.DefaultIfEmpty()
                                                   //where susIn.Consultorio != "0"
                                               where q.Clave == item.Codigo
                                               where q.Clave_Nivel != "2"
                                               where q.Clave_Nivel != "1"
                                               //where q.Inv_Act != 0
                                               select new
                                               {
                                                   Clave = q.Clave,
                                                   Descripcion_Sal = q.Descripcion_Sal,
                                                   Descripcion_Grupo = q.Descripcion_Grupo,
                                                   Presentacion = q.Presentacion,
                                                   Inv_Act = q.Inv_Act,
                                               })
                                          .OrderByDescending(g => g.Descripcion_Grupo)
                                          .FirstOrDefault();


                    var presentacion = "";

                    var desMed = (from q in db4.Sustancia
                                  where q.Clave == item.Codigo
                                  where q.descripcion_21 != null && q.descripcion_21 != ""
                                  select new
                                  {
                                      Presentacion = q.descripcion_21,
                                  })
                                  .FirstOrDefault();

                    if (desMed != null)
                    {
                        presentacion = desMed.Presentacion;
                    }
                    else
                    {
                        if (inventariofarmacia2 != null)
                        {
                            presentacion = inventariofarmacia2.Presentacion;
                        }
                    }


                    if (inventariofarmacia2 != null)
                    {
                        //Revisar que no esten deshabilitados
                        var inhabmed = (from q in db4.InhabilitarMedicamentos
                                        where q.clave == inventariofarmacia2.Clave
                                        select q).FirstOrDefault();

                        //if(medicamento.Clave != "293901" && medicamento.Clave != "442401" && medicamento.Clave != "442701" && medicamento.Clave != "350604" && medicamento.Clave != "470201" && medicamento.Clave != "081202" && medicamento.Clave != "213501" && medicamento.Clave != "255101" && medicamento.Clave != "264501" && medicamento.Clave != "300101" && medicamento.Clave != "310801" && medicamento.Clave != "358201" && medicamento.Clave != "356601" && medicamento.Clave != "356602" && medicamento.Clave != "401201" && medicamento.Clave != "420601" && medicamento.Clave != "430901" && medicamento.Clave != "501101" && medicamento.Clave != "021701" && medicamento.Clave != "070701" && medicamento.Clave != "300102" && medicamento.Clave != "440601" && medicamento.Clave != "211102" && medicamento.Clave != "301802" && medicamento.Clave != "356201" && medicamento.Clave != "430602" && medicamento.Clave != "070402" && medicamento.Clave != "90505" && medicamento.Clave != "254701" && medicamento.Clave != "213401" && medicamento.Clave != "474701" && medicamento.Clave != "502301" && medicamento.Clave != "232002" && medicamento.Clave != "180202" && medicamento.Clave != "502301" && medicamento.Clave != "361201" && medicamento.Clave != "390501" && medicamento.Clave != "150301" && medicamento.Clave != "452403" && medicamento.Clave != "232003" && medicamento.Clave != "313401" && medicamento.Clave != "081201" && medicamento.Clave != "293501" && medicamento.Clave != "090508" && medicamento.Clave != "262602" && medicamento.Clave != "392002" && medicamento.Clave != "392021" && medicamento.Clave != "430101" && medicamento.Clave != "061001" && medicamento.Clave != "392003" && medicamento.Clave != "061005" && medicamento.Clave != "251402" && medicamento.Clave != "262601" && medicamento.Clave != "257101" && medicamento.Clave != "232001" && medicamento.Clave != "314103")
                        //if (medicamento.Clave != "293901" && medicamento.Clave != "442401" && medicamento.Clave != "442701" && medicamento.Clave != "350604" && medicamento.Clave != "470201" && medicamento.Clave != "081202" && medicamento.Clave != "213501" && medicamento.Clave != "255101" && medicamento.Clave != "264501" && medicamento.Clave != "300101" && medicamento.Clave != "310801" && medicamento.Clave != "358201" && medicamento.Clave != "356601" && medicamento.Clave != "356602" && medicamento.Clave != "401201" && medicamento.Clave != "420601" && medicamento.Clave != "430901" && medicamento.Clave != "501101" && medicamento.Clave != "021701" && medicamento.Clave != "070701" && medicamento.Clave != "300102" && medicamento.Clave != "440601" && medicamento.Clave != "211102" && medicamento.Clave != "301802" && medicamento.Clave != "356201" && medicamento.Clave != "430602" && medicamento.Clave != "070402" && medicamento.Clave != "90505" && medicamento.Clave != "254701" && medicamento.Clave != "213401" && medicamento.Clave != "474701" && medicamento.Clave != "502301" && medicamento.Clave != "232002" && medicamento.Clave != "180202" && medicamento.Clave != "502301" && medicamento.Clave != "361201" && medicamento.Clave != "390501" && medicamento.Clave != "150301" && medicamento.Clave != "452403" && medicamento.Clave != "232003" && medicamento.Clave != "262602" && medicamento.Clave != "392002" && medicamento.Clave != "430101" && medicamento.Clave != "392003" && medicamento.Clave != "262601" && medicamento.Clave != "257101" && medicamento.Clave != "232001" && medicamento.Clave != "314103" && medicamento.Clave != "392021")
                        //if (inhabmed == null)
                        //{
                        var disponibilidad = "";
                        if (inventariofarmacia2.Inv_Act > 0)
                        {
                            disponibilidad = "Disponible";
                        }
                        else
                        {
                            disponibilidad = "No disponible";
                        }


                        var result = new Result
                        {
                            Clave = inventariofarmacia2.Clave,
                            Descripcion_Sal = inventariofarmacia2.Descripcion_Sal,
                            Descripcion_Grupo = inventariofarmacia2.Descripcion_Grupo,
                            Historial = "1.- Historial de medicamentos especializados del paciente",
                            //Presentacion = presentacion,
                            Presentacion = inventariofarmacia2.Presentacion,
                            //Inv_Act = inventariofarmacia2.Inv_Act,
                            Disponibilidad = disponibilidad,
                        };

                        medicamentos.Add(result);
                        //}
                    }

                }

                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }

        }


        public class LstInv
        {
            public int id_sustancia { get; set; }
            public int id { get; set; }
            public int sal { get; set; }
            public int inv_act { get; set; }
            public int manejodisponible { get; set; }
            public string clave { get; set; }
            public string descripcion_grupo { get; set; }
            public string descripcion_sal { get; set; }
            public string presentacion { get; set; }
            public string clave_nivel { get; set; }
            public int nivel_21 { get; set; }
            public string usuario_registra { get; set; }
            public string sobranteinv2022 { get; set; }
        }


        public class LstInv2
        {
            public int id_sustancia { get; set; }
            public string clave { get; set; }
            public string descripcion_sal { get; set; }
            public string presentacion { get; set; }
            public string descripcion_grupo { get; set; }
            public int inv_act { get; set; }
            public int manejodisponible { get; set; }
            public string usuario_registra { get; set; }
            public string boton { get; set; }
            public string historial { get; set; }
            public string disponibilidad { get; set; }
            public string sobranteinv2022 { get; set; }
            public string medicamento { get; set; }

        }

        public JsonResult RecetasMed2(string id)
        {
            Models.SERVMEDEntities4 db4 = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);
            //RECETAS
            //string query = "SELECT Sustancia_1.Nivel_21 AS nivel_21, Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and Sustancia_1.Clave != '251001'";
            //string query = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.id AS nivel_21 FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id LEFT JOIN SERVMED.dbo.nivel_21 AS Nivel_1 ON Sustancia_1.Nivel_21 = Nivel_1.id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and Sustancia_1.Clave != '251001'";
            //var result = db4.Database.SqlQuery<LstInv>(query);
            //var res = result.ToList();

            var res = (from q in db4.InvFarm
                       join sust in db4.Sustancia on q.Id_Sustancia equals sust.Id into sustX
                       from sustIn in sustX.DefaultIfEmpty()
                       join grupo in db4.grupo_21 on sustIn.id_grupo_21 equals grupo.id into grupoX
                       from grupoIn in grupoX.DefaultIfEmpty()
                       where sustIn.descripcion_21 != null && sustIn.descripcion_21 != ""
                       //where sustIn.Clave != "251001"
                       //where sustIn.Clave != "291001"
                       //where sustIn.Clave != "300601"
                       //where sustIn.Clave != "990071"
                       where q.InvFarmId == 81
                       //where sustIn.SobranteInv2022 != "2"
                       select new
                       {
                           //inv_act = invfarmIn.Inv_Act,
                           nivel_21 = sustIn.Nivel_21,
                           clave = sustIn.Clave,
                           presentacion = sustIn.descripcion_21,
                           descripcion_grupo = grupoIn.descripcion,
                           inv_act = q.Inv_Act,
                           SobranteInv2022 = sustIn.SobranteInv2022,
                       })
                       .ToList();

            //System.Diagnostics.Debug.WriteLine(res);

            var medicamentos = new List<LstInv2>();


            foreach (var item in res)
            {
                //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                //Medicos generales

                //System.Diagnostics.Debug.WriteLine(item.clave_nivel);
                /*var inmed = (from a in db4.InhabilitarMedicamentos
                             where a.clave == item.clave
                             select a).FirstOrDefault();*/

                var medCon2 = (from a in db4.MedicamentosControlados
                               where a.clave == item.clave
                               select a).FirstOrDefault();

                var nombreusu = User.Identity.GetUserName();

                var medMed = (from a in db2.MedicoMedicamento
                              where a.clave == item.clave
                              where a.medico == nombreusu
                              select a).FirstOrDefault();

                
                /*
                if (medMed != null)
                {
                    System.Diagnostics.Debug.WriteLine(medMed);
                }*/
                

                //solo los puede recetar unos especificos medicos
                if (medMed == null)
                {

                    //System.Diagnostics.Debug.WriteLine(medMed);

                    //El nivel 5 es solo para las enfermeras
                    if (item.nivel_21 != 5 || medCon2 != null)
                    {
                        /*
                        if (medCon2 != null)
                        {
                            System.Diagnostics.Debug.WriteLine(medCon2);
                        }*/

                        var nivel = 0;
                        //Coordinadores nivel 4
                        if (User.IsInRole("Coordinador"))
                        {
                            nivel = 4;
                        }
                        else
                        {
                            if (User.IsInRole("Subespecialista"))
                            {
                                nivel = 3;
                            }
                            else
                            {
                                if (User.IsInRole("Especialistas"))
                                {
                                    nivel = 2;
                                }
                                else
                                {
                                    nivel = 1;
                                }
                            }
                        }

                        //System.Diagnostics.Debug.WriteLine(item.clave);

                        if (item.nivel_21 <= nivel || item.nivel_21 == null)
                        {
                            //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                            //System.Diagnostics.Debug.WriteLine(item.clave);


                            var disponibilidad = "";
                            if (item.inv_act > 0)
                            {
                                disponibilidad = "Disponible";
                            }
                            else
                            {
                                disponibilidad = "No disponible";
                            }

                            var medCon = (from a in db4.MedicamentosControlados
                                          where a.clave == item.clave
                                          select a).FirstOrDefault();

                            if (medCon != null)
                            {
                                if(medCon.status == 1)
                                {
                                    disponibilidad = "Controlado";
                                }
                                else
                                {
                                    disponibilidad = "Disponible";
                                }
                                
                            }

                            //foraneos estaran todos diponibles
                            var username = User.Identity.GetUserName();

                            //foraneos
                            if (username == "02319" || username == "02316" || username == "02318" || username == "02317" || username == "38126"
                                || username == "02321" || username == "02324" || username == "38128" || username == "38129" || username == "38127"
                                || username == "02347" || username == "03139" || username == "05041" || username == "08058" || username == "08059"
                                || username == "19018" || username == "03140" || username == "02334" || username == "13032" || username == "06026"
                                || username == "02340" || username == "02333" || username == "02321" || username == "07017" || username == "08060"
                                || username == "05042" || username == "14038" || username == "05043" || username == "18015" || username == "06027"
                                || username == "02338" || username == "02336" || username == "02337" || username == "13035" || username == "02341" 
                                || username == "52034" || username == "52023" || username == "52024" || username == "52025" || username == "52028"
                                || username == "08053" || username == "38119" || username == "52033")
                            {
                                disponibilidad = "Disponible";
                            }

                            if (medCon2 != null)
                            {
                                var listamedicamentos = new LstInv2
                                {
                                    clave = item.clave,
                                    descripcion_sal = item.presentacion,
                                    descripcion_grupo = item.descripcion_grupo,
                                    historial = item.descripcion_grupo,
                                    presentacion = item.presentacion,
                                    disponibilidad = disponibilidad,
                                };

                                medicamentos.Add(listamedicamentos);
                            }
                            else
                            {
                                if (item.SobranteInv2022 == "2 ")
                                {

                                    if (item.inv_act > 0)
                                    {

                                        var listamedicamentos = new LstInv2
                                        {
                                            clave = item.clave,
                                            descripcion_sal = item.presentacion,
                                            descripcion_grupo = item.descripcion_grupo,
                                            historial = item.descripcion_grupo,
                                            presentacion = item.presentacion,
                                            disponibilidad = disponibilidad,
                                        };

                                        medicamentos.Add(listamedicamentos);
                                    }
                                }
                                else
                                {
                                    //if (item.inv_act > 0)
                                    //{

                                    var listamedicamentos = new LstInv2
                                    {
                                        clave = item.clave,
                                        descripcion_sal = item.presentacion,
                                        descripcion_grupo = item.descripcion_grupo,
                                        historial = item.descripcion_grupo,
                                        presentacion = item.presentacion,
                                        disponibilidad = disponibilidad,
                                    };

                                    medicamentos.Add(listamedicamentos);
                                    //}


                                }
                            }


                        }


                    }


                }
                else
                {

                    //System.Diagnostics.Debug.WriteLine(medMed);

                    if (User.Identity.GetUserName() == medMed.medico)
                    {

                        //System.Diagnostics.Debug.WriteLine(medMed);


                        if (item.nivel_21 != 5 || medCon2 != null)
                        {
                            /*
                            if (medCon2 != null)
                            {
                                System.Diagnostics.Debug.WriteLine(medCon2);
                            }*/

                            var nivel = 0;
                            //Coordinadores nivel 4
                            if (User.IsInRole("Coordinador"))
                            {
                                nivel = 4;
                            }
                            else
                            {
                                if (User.IsInRole("Subespecialista"))
                                {
                                    nivel = 3;
                                }
                                else
                                {
                                    if (User.IsInRole("Especialistas"))
                                    {
                                        nivel = 2;
                                    }
                                    else
                                    {
                                        nivel = 1;
                                    }
                                }
                            }

                            //System.Diagnostics.Debug.WriteLine(item.clave);

                            if (item.nivel_21 <= nivel || item.nivel_21 == null)
                            {
                                //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                                //System.Diagnostics.Debug.WriteLine(item.clave);


                                var disponibilidad = "";
                                if (item.inv_act > 0)
                                {
                                    disponibilidad = "Disponible";
                                }
                                else
                                {
                                    disponibilidad = "No disponible";
                                }

                                var medCon = (from a in db4.MedicamentosControlados
                                              where a.clave == item.clave
                                              select a).FirstOrDefault();

                                if (medCon != null)
                                {
                                    if (medCon.status == 1)
                                    {
                                        disponibilidad = "Controlado";
                                    }
                                    else
                                    {
                                        disponibilidad = "Disponible";
                                    }

                                }

                                //foraneos estaran todos diponibles
                                var username = User.Identity.GetUserName();

                                //foraneos
                                if (username == "02319" || username == "02316" || username == "02318" || username == "02317" || username == "38126"
                                    || username == "02321" || username == "02324" || username == "38128" || username == "38129" || username == "38127"
                                    || username == "02347" || username == "03139" || username == "05041" || username == "08058" || username == "08059"
                                    || username == "19018" || username == "03140" || username == "02334" || username == "13032" || username == "06026"
                                    || username == "02340" || username == "02333" || username == "02321" || username == "07017" || username == "08060"
                                    || username == "05042" || username == "14038" || username == "05043" || username == "18015" || username == "06027"
                                    || username == "02338" || username == "02336" || username == "02337" || username == "13035" || username == "02341"
                                    || username == "52034" || username == "52023" || username == "52024" || username == "52025" || username == "52028"
                                    || username == "08053" || username == "38119" || username == "52033")
                                {
                                    disponibilidad = "Disponible";
                                }

                                if (medCon2 != null)
                                {
                                    var listamedicamentos = new LstInv2
                                    {
                                        clave = item.clave,
                                        descripcion_sal = item.presentacion,
                                        descripcion_grupo = item.descripcion_grupo,
                                        historial = item.descripcion_grupo,
                                        presentacion = item.presentacion,
                                        disponibilidad = disponibilidad,
                                    };

                                    medicamentos.Add(listamedicamentos);
                                }
                                else
                                {
                                    if (item.SobranteInv2022 == "2 ")
                                    {

                                        if (item.inv_act > 0)
                                        {

                                            var listamedicamentos = new LstInv2
                                            {
                                                clave = item.clave,
                                                descripcion_sal = item.presentacion,
                                                descripcion_grupo = item.descripcion_grupo,
                                                historial = item.descripcion_grupo,
                                                presentacion = item.presentacion,
                                                disponibilidad = disponibilidad,
                                            };

                                            medicamentos.Add(listamedicamentos);
                                        }
                                    }
                                    else
                                    {
                                        //if (item.inv_act > 0)
                                        //{

                                        var listamedicamentos = new LstInv2
                                        {
                                            clave = item.clave,
                                            descripcion_sal = item.presentacion,
                                            descripcion_grupo = item.descripcion_grupo,
                                            historial = item.descripcion_grupo,
                                            presentacion = item.presentacion,
                                            disponibilidad = disponibilidad,
                                        };

                                        medicamentos.Add(listamedicamentos);
                                        //}


                                    }
                                }


                            }


                        }

                    }
                }

            }


            //if (User.IsInRole("Especialistas"))
            //{
            var recetaResDts = (from d in db2.receta_detalle_crn
                                join recetaExp in db2.receta_exp_crn on d.folio_rc equals recetaExp.folio_rc into reExp
                                from receExp in reExp.DefaultIfEmpty()
                                where receExp.expediente == id
                                where receExp.fecha_crea >= fechaDT
                                select new
                                {
                                    clave = d.codigo,
                                })
                               .GroupBy(p => new
                               {
                                   p.clave,
                               })
                               .Select(g => new
                               {
                                   clave = g.Key.clave,
                               })
                               .ToList();


            var recetaResDts2 = (from d in db4.Receta_Detalle
                                 join recetaExp in db4.Receta_Exp on d.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                 from receExp in reExp.DefaultIfEmpty()
                                 join sust in db4.Sustancia on d.Codigo equals sust.Clave into sustX
                                 from sustIn in sustX.DefaultIfEmpty()
                                 where receExp.Expediente == id
                                 where receExp.Fecha >= fechaDT
                                 where sustIn.Nivel_21 >= 1
                                 select new
                                 {
                                     clave = d.Codigo,
                                 })
                               .GroupBy(p => new
                               {
                                   p.clave,
                               })
                               .Select(g => new
                               {
                                   clave = g.Key.clave,
                               })
                               .ToList();

            //System.Diagnostics.Debug.WriteLine(recetaResDts);

            foreach (var item in recetaResDts)
            {
                var res3 = (from q in db4.InvFarm
                            join sust in db4.Sustancia on q.Id_Sustancia equals sust.Id into sustX
                            from sustIn in sustX.DefaultIfEmpty()
                            join grupo in db4.grupo_21 on sustIn.id_grupo_21 equals grupo.id into grupoX
                            from grupoIn in grupoX.DefaultIfEmpty()
                            where sustIn.descripcion_21 != null && sustIn.descripcion_21 != ""
                            where sustIn.Clave == item.clave
                            where q.InvFarmId == 81
                            select new
                            {
                                presentacion = sustIn.descripcion_21,
                                descripcion_grupo = grupoIn.descripcion,
                                inv_act = q.Inv_Act,
                                nivel_21 = sustIn.Nivel_21,
                                SobranteInv2022 = sustIn.SobranteInv2022,
                            })
                           .FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(res3);


                if (res3 != null)
                {

                    var medCon2 = (from a in db4.MedicamentosControlados
                                   where a.clave == item.clave
                                   select a).FirstOrDefault();

                    if (res3.nivel_21 != 5 || medCon2 != null) {

                        if (res3.nivel_21 > 1)
                        {

                            //System.Diagnostics.Debug.WriteLine(res3);

                            var disponibilidad = "";
                            if (res3.inv_act > 0)
                            {
                                disponibilidad = "Disponible";
                            }
                            else
                            {
                                disponibilidad = "No disponible";
                            }

                            var medCon = (from a in db4.MedicamentosControlados
                                          where a.clave == item.clave
                                          select a).FirstOrDefault();

                            if (medCon != null)
                            {
                                if (medCon.status == 1)
                                {
                                    disponibilidad = "Controlado";
                                }
                                else
                                {
                                    disponibilidad = "Disponible";
                                }

                            }


                            //foraneos estaran todos diponibles
                            var username = User.Identity.GetUserName();

                            //foraneos
                            if (username == "02319" || username == "02316" || username == "02318" || username == "02317" || username == "38126"
                                || username == "02321" || username == "02324" || username == "38128" || username == "38129" || username == "38127"
                                || username == "02347" || username == "03139" || username == "05041" || username == "08058" || username == "08059"
                                || username == "19018" || username == "03140" || username == "02334" || username == "13032" || username == "06026"
                                || username == "02340" || username == "02333" || username == "02321" || username == "07017" || username == "08060"
                                || username == "05042" || username == "14038" || username == "05043" || username == "18015" || username == "06027"
                                || username == "02338" || username == "02336" || username == "02337" || username == "13035" || username == "02341"
                                || username == "52034" || username == "52023" || username == "52024" || username == "52025" || username == "52028"
                                || username == "08053" || username == "38119" || username == "52033")
                            {
                                disponibilidad = "Disponible";
                            }


                            if (res3.SobranteInv2022 == "2 ")
                            {

                                if (res3.inv_act > 0)
                                {

                                    var listamedicamentos = new LstInv2
                                    {
                                        clave = item.clave,
                                        descripcion_sal = res3.presentacion,
                                        descripcion_grupo = res3.descripcion_grupo,
                                        historial = "1.- Historial de medicamentos especializados del paciente",
                                        presentacion = res3.presentacion,
                                        disponibilidad = disponibilidad,
                                    };

                                    medicamentos.Add(listamedicamentos);
                                }
                            }
                            else
                            {
                                //if (res3.inv_act > 0)
                                //{

                                var listamedicamentos = new LstInv2
                                {
                                    clave = item.clave,
                                    descripcion_sal = res3.presentacion,
                                    descripcion_grupo = res3.descripcion_grupo,
                                    historial = "1.- Historial de medicamentos especializados del paciente",
                                    presentacion = res3.presentacion,
                                    disponibilidad = disponibilidad,
                                };

                                medicamentos.Add(listamedicamentos);
                                //}
                            }


                        }

                    }
                }
            }
            //}



            foreach (var item in recetaResDts2)
            {
                var res3 = (from q in db4.InvFarm
                            join sust in db4.Sustancia on q.Id_Sustancia equals sust.Id into sustX
                            from sustIn in sustX.DefaultIfEmpty()
                            join grupo in db4.grupo_21 on sustIn.id_grupo_21 equals grupo.id into grupoX
                            from grupoIn in grupoX.DefaultIfEmpty()
                            where sustIn.descripcion_21 != null && sustIn.descripcion_21 != ""
                            where sustIn.Clave == item.clave
                            where q.InvFarmId == 81
                            select new
                            {
                                presentacion = sustIn.descripcion_21,
                                descripcion_grupo = grupoIn.descripcion,
                                inv_act = q.Inv_Act,
                                nivel_21 = sustIn.Nivel_21,
                                SobranteInv2022 = sustIn.SobranteInv2022,
                            })
                           .FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(res3);


                if (res3 != null)
                {

                    var medCon2 = (from a in db4.MedicamentosControlados
                                   where a.clave == item.clave
                                   select a).FirstOrDefault();

                    if (res3.nivel_21 != 5 || medCon2 != null)
                    {

                        if (res3.nivel_21 > 1)
                        {

                            //System.Diagnostics.Debug.WriteLine(res3);

                            var disponibilidad = "";
                            if (res3.inv_act > 0)
                            {
                                disponibilidad = "Disponible";
                            }
                            else
                            {
                                disponibilidad = "No disponible";
                            }

                            var medCon = (from a in db4.MedicamentosControlados
                                          where a.clave == item.clave
                                          select a).FirstOrDefault();

                            if (medCon != null)
                            {
                                if (medCon.status == 1)
                                {
                                    disponibilidad = "Controlado";
                                }
                                else
                                {
                                    disponibilidad = "Disponible";
                                }

                            }


                            //foraneos estaran todos diponibles
                            var username = User.Identity.GetUserName();

                            //foraneos
                            if (username == "02319" || username == "02316" || username == "02318" || username == "02317" || username == "38126"
                                || username == "02321" || username == "02324" || username == "38128" || username == "38129" || username == "38127"
                                || username == "02347" || username == "03139" || username == "05041" || username == "08058" || username == "08059"
                                || username == "19018" || username == "03140" || username == "02334" || username == "13032" || username == "06026"
                                || username == "02340" || username == "02333" || username == "02321" || username == "07017" || username == "08060"
                                || username == "05042" || username == "14038" || username == "05043" || username == "18015" || username == "06027"
                                || username == "02338" || username == "02336" || username == "02337" || username == "13035" || username == "02341"
                                || username == "52034" || username == "52023" || username == "52024" || username == "52025" || username == "52028"
                                || username == "08053" || username == "38119" || username == "52033")
                            {
                                disponibilidad = "Disponible";
                            }




                            if (res3.SobranteInv2022 == "2 ")
                            {

                                if (res3.inv_act > 0)
                                {

                                    var listamedicamentos = new LstInv2
                                    {
                                        clave = item.clave,
                                        descripcion_sal = res3.presentacion,
                                        descripcion_grupo = res3.descripcion_grupo,
                                        historial = "1.- Historial de medicamentos especializados del paciente",
                                        presentacion = res3.presentacion,
                                        disponibilidad = disponibilidad,
                                    };

                                    medicamentos.Add(listamedicamentos);
                                }
                            }
                            else
                            {
                                //if (res3.inv_act > 0)
                                //{

                                var listamedicamentos = new LstInv2
                                {
                                    clave = item.clave,
                                    descripcion_sal = res3.presentacion,
                                    descripcion_grupo = res3.descripcion_grupo,
                                    historial = "1.- Historial de medicamentos especializados del paciente",
                                    presentacion = res3.presentacion,
                                    disponibilidad = disponibilidad,
                                };

                                medicamentos.Add(listamedicamentos);
                                //}
                            }


                        }

                    }
                }
            }





            return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }


        public JsonResult MedicamentoHisEsp(string clave, string id)
        {
            Models.SERVMEDEntities4 db4 = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);

            var recetaResDts = (from d in db2.receta_detalle_crn
                                join recetaExp in db2.receta_exp_crn on d.folio_rc equals recetaExp.folio_rc into reExp
                                from receExp in reExp.DefaultIfEmpty()
                                    //join sust in db2.Sustancia on d.codigo equals sust.Clave into sustX
                                    //from sustIn in sustX.DefaultIfEmpty()
                                where d.codigo == clave
                                where receExp.expediente == id
                                where receExp.fecha_crea >= fechaDT
                                //where sustIn.Nivel_21 >= 2
                                select new
                                {
                                    fecha = receExp.fecha_crea,
                                    codigo = d.codigo,
                                })
                                .FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(recetaResDts);

            var fechaMed = "";

            if (recetaResDts != null)
            {

                var nivelMed = (from d in db4.Sustancia
                                where d.Clave == recetaResDts.codigo
                                where d.Nivel_21 >= 2
                                select d)
                                .FirstOrDefault();

                if (nivelMed != null)
                {
                    var fechaDTNueva = DateTime.Parse(recetaResDts.fecha.ToString());
                    var myDate = new DateTime(fechaDTNueva.Year, fechaDTNueva.Month, fechaDTNueva.Day).AddMonths(3);
                    //System.Diagnostics.Debug.WriteLine(myDate);
                    //var fechaDTNueva = DateTime.Parse(fechaNueva);

                    fechaMed = string.Format("{0:dddd, dd MMMM yyyy}", myDate, new CultureInfo("es-ES"));
                }
                else
                {
                    fechaMed = "";
                }

            }
            else
            {
                fechaMed = "";
            }

            return new JsonResult { Data = fechaMed, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult RecetasMedCro2(string id)
        {
            Models.SERVMEDEntities4 db4 = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            //RECETAS
            //string query = "SELECT Sustancia_1.Nivel_21 AS nivel_21, Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and Sustancia_1.Clave != '251001'";
            //string query = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.id AS nivel_21 FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id LEFT JOIN SERVMED.dbo.nivel_21 AS Nivel_1 ON Sustancia_1.Nivel_21 = Nivel_1.id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and Sustancia_1.Clave != '251001'";
            //var result = db4.Database.SqlQuery<LstInv>(query);
            //var res = result.ToList();

            var res = (from q in db4.InvFarm
                       join sust in db4.Sustancia on q.Id_Sustancia equals sust.Id into sustX
                       from sustIn in sustX.DefaultIfEmpty()
                       join grupo in db4.grupo_21 on sustIn.id_grupo_21 equals grupo.id into grupoX
                       from grupoIn in grupoX.DefaultIfEmpty()
                       where sustIn.descripcion_21 != null && sustIn.descripcion_21 != ""
                       //where sustIn.Clave != "251001"
                       //where sustIn.Clave != "291001"
                       //where sustIn.Clave != "300601"
                       //where sustIn.Clave != "990071"
                       where q.InvFarmId == 81
                       //where sustIn.SobranteInv2022 != "2"
                       select new
                       {
                           //inv_act = invfarmIn.Inv_Act,
                           nivel_21 = sustIn.Nivel_21,
                           clave = sustIn.Clave,
                           presentacion = sustIn.descripcion_21,
                           descripcion_grupo = grupoIn.descripcion,
                           inv_act = q.Inv_Act,
                           SobranteInv2022 = sustIn.SobranteInv2022,
                       })
                       .ToList();

            //System.Diagnostics.Debug.WriteLine(res);

            var medicamentos = new List<LstInv2>();

            foreach (var item in res)
            {
                //Medicos generales

                //System.Diagnostics.Debug.WriteLine(item.clave_nivel);

                var medCon2 = (from a in db4.MedicamentosControlados
                               where a.clave == item.clave
                               select a).FirstOrDefault();

                //El nivel 5 es solo para las enfermeras
                if (item.nivel_21 != 5 || medCon2 != null)
                {
                    var nivel = 0;
                    //Coordinadores nivel 4
                    if (User.IsInRole("Coordinador"))
                    {
                        nivel = 4;
                    }
                    else
                    {
                        if (User.IsInRole("Subespecialista"))
                        {
                            nivel = 3;
                        }
                        else
                        {
                            if (User.IsInRole("Especialistas"))
                            {
                                nivel = 2;
                            }
                            else
                            {
                                nivel = 1;
                            }
                        }
                    }

                    //System.Diagnostics.Debug.WriteLine(item.clave);

                    if (item.nivel_21 <= nivel || item.nivel_21 == null)
                    {
                        //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                        //System.Diagnostics.Debug.WriteLine(item.clave);


                        var disponibilidad = "";
                        if (item.inv_act > 0)
                        {
                            disponibilidad = "Disponible";
                        }
                        else
                        {
                            disponibilidad = "No disponible";
                        }

                        var medCon = (from a in db4.MedicamentosControlados
                                      where a.clave == item.clave
                                      select a).FirstOrDefault();

                        if (medCon != null)
                        {
                            if (medCon.status == 1)
                            {
                                disponibilidad = "Controlado";
                            }
                            else
                            {
                                disponibilidad = "Disponible";
                            }

                        }

                        //foraneos estaran todos diponibles
                        var username = User.Identity.GetUserName();

                        //foraneos
                        if (username == "02319" || username == "02316" || username == "02318" || username == "02317" || username == "38126"
                            || username == "02321" || username == "02324" || username == "38128" || username == "38129" || username == "38127"
                            || username == "02347" || username == "03139" || username == "05041" || username == "08058" || username == "08059"
                            || username == "19018" || username == "03140" || username == "02334" || username == "13032" || username == "06026"
                            || username == "02340" || username == "02333" || username == "02321" || username == "07017" || username == "08060"
                            || username == "05042" || username == "14038" || username == "05043" || username == "18015" || username == "06027"
                            || username == "02338" || username == "02336" || username == "02337" || username == "13035" || username == "02341"
                            || username == "52034" || username == "52023" || username == "52024" || username == "52025" || username == "52028"
                            || username == "08053" || username == "38119" || username == "52033")
                        {
                            disponibilidad = "Disponible";
                        }


                        if (item.SobranteInv2022 == "2 ")
                        {

                            if (item.inv_act > 0)
                            {

                                var listamedicamentos = new LstInv2
                                {
                                    clave = item.clave,
                                    descripcion_sal = item.presentacion,
                                    descripcion_grupo = item.descripcion_grupo,
                                    historial = item.descripcion_grupo,
                                    presentacion = item.presentacion,
                                    disponibilidad = disponibilidad,
                                };

                                medicamentos.Add(listamedicamentos);
                            }
                        }
                        else
                        {

                            //if (item.inv_act > 0) { 

                            var listamedicamentos = new LstInv2
                            {
                                clave = item.clave,
                                descripcion_sal = item.presentacion,
                                descripcion_grupo = item.descripcion_grupo,
                                historial = item.descripcion_grupo,
                                presentacion = item.presentacion,
                                disponibilidad = disponibilidad,
                            };

                            medicamentos.Add(listamedicamentos);

                            //}
                        }
                    }


                }


            }

            return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }


        public JsonResult RecetasMedCro2_Prueba(string id)
        {
            Models.SERVMEDEntities4 db4 = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var res = (from q in db4.InvFarm
                       join sust in db4.Sustancia on q.Id_Sustancia equals sust.Id into sustX
                       from sustIn in sustX.DefaultIfEmpty()
                       join grupo in db4.grupo_21 on sustIn.id_grupo_21 equals grupo.id into grupoX
                       from grupoIn in grupoX.DefaultIfEmpty()
                       where sustIn.descripcion_21 != null && sustIn.descripcion_21 != ""
                       where q.InvFarmId == 81
                       select new
                       {
                           nivel_21 = sustIn.Nivel_21,
                           clave = sustIn.Clave,
                           presentacion = sustIn.descripcion_21,
                           descripcion_grupo = grupoIn.descripcion,
                           inv_act = q.Inv_Act,
                           SobranteInv2022 = sustIn.SobranteInv2022,
                       })
                       .ToList();

            //System.Diagnostics.Debug.WriteLine(res);

            var medicamentos = new List<LstInv2>();

            foreach (var item in res)
            {
                //Medicos generales

                //System.Diagnostics.Debug.WriteLine(item.clave_nivel);

                var medCon2 = (from a in db4.MedicamentosControlados
                               where a.clave == item.clave
                               select a).FirstOrDefault();

                //
                var medMed = (from a in db2.MedicoMedicamento
                              where a.clave == item.clave
                              select a).FirstOrDefault();

                if (medMed == null)
                {
                    //El nivel 5 es solo para las enfermeras
                    if (item.nivel_21 != 5 || medCon2 != null)
                    {
                        var nivel = 0;
                        //Coordinadores nivel 4
                        if (User.IsInRole("Coordinador"))
                        {
                            nivel = 4;
                        }
                        else
                        {
                            if (User.IsInRole("Subespecialista"))
                            {
                                nivel = 3;
                            }
                            else
                            {
                                if (User.IsInRole("Especialistas"))
                                {
                                    nivel = 2;
                                }
                                else
                                {
                                    nivel = 1;
                                }
                            }
                        }

                        //System.Diagnostics.Debug.WriteLine(item.clave);

                        if (item.nivel_21 <= nivel || item.nivel_21 == null)
                        {
                            //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                            //System.Diagnostics.Debug.WriteLine(item.clave);


                            var disponibilidad = "";
                            if (item.inv_act > 0)
                            {
                                disponibilidad = "Disponible";
                            }
                            else
                            {
                                disponibilidad = "No disponible";
                            }

                            var medCon = (from a in db4.MedicamentosControlados
                                          where a.clave == item.clave
                                          select a).FirstOrDefault();

                            if (medCon != null)
                            {
                                if (medCon.status == 1)
                                {
                                    disponibilidad = "Controlado";
                                }
                                else
                                {
                                    disponibilidad = "Disponible";
                                }

                            }


                            if (medCon != null)
                            {
                                var listamedicamentos = new LstInv2
                                {
                                    clave = item.clave,
                                    descripcion_sal = item.presentacion,
                                    descripcion_grupo = item.descripcion_grupo,
                                    historial = item.descripcion_grupo,
                                    presentacion = item.presentacion,
                                    disponibilidad = disponibilidad,
                                };

                                medicamentos.Add(listamedicamentos);
                            }
                            else
                            {

                                if (item.SobranteInv2022 == "2 ")
                                {

                                    if (item.inv_act > 0)
                                    {

                                        var listamedicamentos = new LstInv2
                                        {
                                            clave = item.clave,
                                            descripcion_sal = item.presentacion,
                                            descripcion_grupo = item.descripcion_grupo,
                                            historial = item.descripcion_grupo,
                                            presentacion = item.presentacion,
                                            disponibilidad = disponibilidad,
                                        };

                                        medicamentos.Add(listamedicamentos);
                                    }
                                }
                                else
                                {

                                    //if (item.inv_act > 0) { 

                                    var listamedicamentos = new LstInv2
                                    {
                                        clave = item.clave,
                                        descripcion_sal = item.presentacion,
                                        descripcion_grupo = item.descripcion_grupo,
                                        historial = item.descripcion_grupo,
                                        presentacion = item.presentacion,
                                        disponibilidad = disponibilidad,
                                    };

                                    medicamentos.Add(listamedicamentos);

                                    //}
                                }

                            }
                        }


                    }

                }
                else {
                    if (User.Identity.GetUserName() == medMed.medico)
                    {
                        //El nivel 5 es solo para las enfermeras
                        if (item.nivel_21 != 5 || medCon2 != null)
                        {
                            var nivel = 0;
                            //Coordinadores nivel 4
                            if (User.IsInRole("Coordinador"))
                            {
                                nivel = 4;
                            }
                            else
                            {
                                if (User.IsInRole("Subespecialista"))
                                {
                                    nivel = 3;
                                }
                                else
                                {
                                    if (User.IsInRole("Especialistas"))
                                    {
                                        nivel = 2;
                                    }
                                    else
                                    {
                                        nivel = 1;
                                    }
                                }
                            }

                            //System.Diagnostics.Debug.WriteLine(item.clave);

                            if (item.nivel_21 <= nivel || item.nivel_21 == null)
                            {
                                //System.Diagnostics.Debug.WriteLine(item.SobranteInv2022);
                                //System.Diagnostics.Debug.WriteLine(item.clave);


                                var disponibilidad = "";
                                if (item.inv_act > 0)
                                {
                                    disponibilidad = "Disponible";
                                }
                                else
                                {
                                    disponibilidad = "No disponible";
                                }

                                var medCon = (from a in db4.MedicamentosControlados
                                              where a.clave == item.clave
                                              select a).FirstOrDefault();

                                if (medCon != null)
                                {
                                    if (medCon.status == 1)
                                    {
                                        disponibilidad = "Controlado";
                                    }
                                    else
                                    {
                                        disponibilidad = "Disponible";
                                    }

                                }


                                if (medCon != null)
                                {
                                    var listamedicamentos = new LstInv2
                                    {
                                        clave = item.clave,
                                        descripcion_sal = item.presentacion,
                                        descripcion_grupo = item.descripcion_grupo,
                                        historial = item.descripcion_grupo,
                                        presentacion = item.presentacion,
                                        disponibilidad = disponibilidad,
                                    };

                                    medicamentos.Add(listamedicamentos);
                                }
                                else
                                {

                                    if (item.SobranteInv2022 == "2 ")
                                    {

                                        if (item.inv_act > 0)
                                        {

                                            var listamedicamentos = new LstInv2
                                            {
                                                clave = item.clave,
                                                descripcion_sal = item.presentacion,
                                                descripcion_grupo = item.descripcion_grupo,
                                                historial = item.descripcion_grupo,
                                                presentacion = item.presentacion,
                                                disponibilidad = disponibilidad,
                                            };

                                            medicamentos.Add(listamedicamentos);
                                        }
                                    }
                                    else
                                    {

                                        //if (item.inv_act > 0) { 

                                        var listamedicamentos = new LstInv2
                                        {
                                            clave = item.clave,
                                            descripcion_sal = item.presentacion,
                                            descripcion_grupo = item.descripcion_grupo,
                                            historial = item.descripcion_grupo,
                                            presentacion = item.presentacion,
                                            disponibilidad = disponibilidad,
                                        };

                                        medicamentos.Add(listamedicamentos);

                                        //}
                                    }

                                }
                            }


                        }

                    }
                }


            }

            return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }


        public class tipodeenvase_21
        {
            public int id { get; set; }
            public string descripcion { get; set; }
        }

        public JsonResult MedicamentoDetalle(string clave)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();


            //string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_mederos";
            //var result = db.Database.SqlQuery<RecetasMederos>(query);
            //var res = result.ToList();

            //var medicamentos = new List<Result>();

            var res2 = (from a in db2.Sustancia
                        join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                        from grupoIn in grupoX.DefaultIfEmpty()
                        where a.Clave == clave
                        select new
                        {

                            Id = a.Id,
                            Clave = a.Clave,
                            descripcion_21 = a.descripcion_21,
                            id_unidadeslicitacion_21 = a.id_unidadeslicitacion_21,
                            Descripcion_Grupo = grupoIn.descripcion,

                        }).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(inventariofarmacia.Clave);

            if (res2 != null)
            {

                //string query = "select Id_Sustancia as id_sustancia, Inv_Act as cactual from InvFarm where Id_Sustancia = " + res2.Id + "";
                //string query2 = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + res2.Id + "";
                //var result = db2.Database.SqlQuery<RecetasMederos>(query2);
                //var res = result.FirstOrDefault();

                string query = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + res2.Id + "";
                var result = db2.Database.SqlQuery<LstInv>(query);
                var res = result.FirstOrDefault();

                var resultNew = new Object();

                /*
                var habilitado = 1;

                if (res.inv_act <= 0)
                {
                    habilitado = 0;
                }
                else
                {
                    habilitado = 1;
                }*/


                decimal? unidadlicitacion = 0;

                if (res2.id_unidadeslicitacion_21 != null)
                {
                    //unidadlicitacion = desMed.unidadlicitacion;
                    var unidad = (from q in db2.unidadeslicitacion_21
                                  where q.id == res2.id_unidadeslicitacion_21
                                  //where q.descripcion_21 != null && q.descripcion_21 != ""
                                  select new
                                  {
                                      unidadlicitacion = q.descripcion,

                                  })
                                 .FirstOrDefault();

                    unidadlicitacion = unidad.unidadlicitacion;

                }


                var tipo = "";


                //var inhabmed = 0;
                var mensajeRcta = 0;

                var inmed = (from a in db2.InhabilitarMedicamentos
                             where a.clave == res2.Clave
                             select a).FirstOrDefault();

                //medicamentos controlados
                var medCon2 = (from a in db2.MedicamentosControlados
                               where a.clave == res2.Clave
                               select a).FirstOrDefault();

                //foraneos estaran todos diponibles
                var username = User.Identity.GetUserName();

                //foraneos
                if (username == "02319" || username == "02316" || username == "02318" || username == "02317" || username == "38126"
                    || username == "02321" || username == "02324" || username == "38128" || username == "38129" || username == "38127"
                    || username == "02347" || username == "03139" || username == "05041" || username == "08058" || username == "08059"
                    || username == "19018" || username == "03140" || username == "02334" || username == "13032" || username == "06026"
                    || username == "02340" || username == "02333" || username == "02321" || username == "07017" || username == "08060"
                    || username == "05042" || username == "14038" || username == "05043" || username == "18015" || username == "06027"
                    || username == "02338" || username == "02336" || username == "02337" || username == "13035" || username == "02341"
                    || username == "52034" || username == "52023" || username == "52024" || username == "52025" || username == "52028"
                    || username == "08053" || username == "38119" || username == "52033")
                {
                    mensajeRcta = 0;
                }
                else
                {

                    //si no son coordinadores
                    if (!User.IsInRole("Coordinador"))
                    {
                        //despues si no esta inhabilitado por farmacia
                        if (inmed == null)
                        {


                            //buscar pimero si es un medicamento controlado
                            if (medCon2 != null)
                            {
                                mensajeRcta = 0;
                            }
                            else
                            {
                                if (res.inv_act <= 0)
                                {
                                    mensajeRcta = 0;
                                }
                                else
                                {
                                    mensajeRcta = 0;
                                }
                            }

                        }
                        else
                        {
                            mensajeRcta = 1;
                            //System.Diagnostics.Debug.WriteLine(mensajeRcta);
                        }

                    }
                    else
                    {
                        mensajeRcta = 0;
                    }

                }





                resultNew = new
                {
                    Clave = res2.Clave,
                    Descripcion_Sal = res2.descripcion_21,
                    //Habilitado = habilitado,
                    Presentacion = res2.descripcion_21,
                    Descripcion_Grupo = res2.Descripcion_Grupo,
                    Inv_Act = res.inv_act,
                    Tipo = tipo,
                    Unidadlicitacion = unidadlicitacion,
                    //inhabmed = inhabmed,
                    mensajeRcta = mensajeRcta,
                };

                return new JsonResult { Data = resultNew, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var medicamentos = "";
            return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



            //return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult MedicamentoDetalleCroPrueba(string clave, string numexp)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            var res2 = (from a in db2.Sustancia
                        join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                        from grupoIn in grupoX.DefaultIfEmpty()
                        where a.Clave == clave
                        select new
                        {

                            Id = a.Id,
                            Clave = a.Clave,
                            descripcion_21 = a.descripcion_21,
                            id_unidadeslicitacion_21 = a.id_unidadeslicitacion_21,
                            Descripcion_Grupo = grupoIn.descripcion,

                        }).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(res2);

            if (res2 != null)
            {

                string query = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + res2.Id + "";
                var result = db2.Database.SqlQuery<LstInv>(query);
                var res = result.FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(res);

                var resultNew = new Object();

                var habilitado = 1;

                if (res.inv_act <= 0)
                {
                    habilitado = 0;
                }
                else
                {
                    habilitado = 1;
                }


                decimal? unidadlicitacion = 0;

                if (res2.id_unidadeslicitacion_21 != null)
                {
                    //unidadlicitacion = desMed.unidadlicitacion;
                    var unidad = (from q in db2.unidadeslicitacion_21
                                  where q.id == res2.id_unidadeslicitacion_21
                                  //where q.descripcion_21 != null && q.descripcion_21 != ""
                                  select new
                                  {
                                      unidadlicitacion = q.descripcion,

                                  })
                                 .FirstOrDefault();

                    unidadlicitacion = unidad.unidadlicitacion;

                }

                var tipo = "";

                var inmed = (from a in db2.InhabilitarMedicamentos
                             where a.clave == res2.Clave
                             select a).FirstOrDefault();


                var fecha3meses = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
                var fechaDT3meses = DateTime.Parse(fecha3meses);

                //Buscar si se ha recetado en los ultimos 3 meses
                var receta3meses = (from a in db2.Receta_Detalle
                                    join recetaExp in db2.Receta_Exp on a.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                    from receExp in reExp.DefaultIfEmpty()
                                    where a.Codigo == res2.Clave
                                    where receExp.Fecha >= fechaDT3meses
                                    select a).Count();

                //Buscar si se ha recetado en los ultimos 3 meses
                var recetacrn3meses = (from a in db.receta_detalle_crn
                                       join recetaExp in db.receta_exp_crn on a.folio_rc equals recetaExp.folio_rc into reExp
                                       from receExp in reExp.DefaultIfEmpty()
                                       where a.codigo == res2.Clave
                                       where receExp.fecha_crea >= fechaDT3meses
                                       select a).Count();


                //buscar si se ha surtido en otro resurtimiento
                var recetaCrnExp = (from a in db.receta_detalle_crn
                                    join recetaExp in db.receta_exp_crn on a.folio_rc equals recetaExp.folio_rc into reExp
                                    from receExp in reExp.DefaultIfEmpty()
                                    where a.codigo == res2.Clave
                                    where receExp.expediente == numexp
                                    //where receExp.meses_surtir >= fechaDT3meses
                                    select new
                                    {
                                        meses_surtir = receExp.meses_surtir,
                                        folio_rc = receExp.folio_rc,
                                        fecha_crea = receExp.fecha_crea,

                                    }).OrderByDescending(g => g.folio_rc)
                                    .FirstOrDefault();

                //Buscar el folio en las recetas de normales
                //eventos
                var eventos = 0;
                if (recetaCrnExp != null)
                {
                    var recetaExpFol = (from a in db2.Receta_Exp
                                        where a.folio_rc == recetaCrnExp.folio_rc
                                        select a)
                                        .Count();

                    eventos = recetaExpFol;

                    //System.Diagnostics.Debug.WriteLine(recetaExpFol);
                }


                //se podra recetar medicamento 
                var mensajeRcta = 0;


                //Coordinadores
                if (!User.IsInRole("Coordinador"))
                {

                    if (receta3meses > 0 || recetacrn3meses > 0)
                    {
                        //si lo puede recetar
                        mensajeRcta = 0;
                    }
                    else
                    {
                        //no se puede recetar porque no se ha surtido en los ultimos 3 meses
                        mensajeRcta = 1;
                    }

                    //System.Diagnostics.Debug.WriteLine(mensajeRcta);


                    if (mensajeRcta == 0)
                    {
                        //System.Diagnostics.Debug.WriteLine(recetaCrnExp);
                        if (recetaCrnExp != null)
                        {
                            var fechaRCE = DateTime.Now.AddMonths(-recetaCrnExp.meses_surtir).ToString("yyyy-MM-dd");
                            var fechaRCEDT = DateTime.Parse(fechaRCE);
                            //System.Diagnostics.Debug.WriteLine(recetaCrnExp.fecha_crea);
                            //System.Diagnostics.Debug.WriteLine(fechaRCEDT);

                            if (eventos >= recetaCrnExp.meses_surtir)
                            {
                                //si lo puede recetar
                                mensajeRcta = 0;
                            }
                            else
                            {
                                //no se puede recetar porque ya se surtio en otro que esta activo
                                //mensajeRcta = 2;
                                mensajeRcta = 0;
                            }

                            /*
                            if (fechaRCEDT >= recetaCrnExp.fecha_crea)
                            {
                                //si lo puede recetar
                                mensajeRcta = 0;
                            }
                            else
                            {
                                //no se puede recetar porque ya se surtio en otro que esta activo
                                mensajeRcta = 2;
                            }
                            */
                        }
                    }



                    if (res.inv_act <= 0)
                    {
                        //mensajeRcta = 3;
                        mensajeRcta = 0;
                    }

                    //
                    var medCon2 = (from a in db2.MedicamentosControlados
                                   where a.clave == res.clave
                                   select a).FirstOrDefault();

                    if (medCon2 != null)
                    {
                        mensajeRcta = 0;
                    }
                    //System.Diagnostics.Debug.WriteLine(mensajeRcta);
                    //System.Diagnostics.Debug.WriteLine(mensajeRcta);

                }
                else
                {
                    mensajeRcta = 0;
                }

                if (inmed != null)
                {
                    //Medicamento inhabilitado por farmacia
                    mensajeRcta = 3;
                }


                resultNew = new
                {
                    Clave = res2.Clave,
                    Descripcion_Sal = res2.descripcion_21,
                    Habilitado = habilitado,
                    Presentacion = res2.descripcion_21,
                    Descripcion_Grupo = res2.Descripcion_Grupo,
                    Inv_Act = res.inv_act,
                    Tipo = tipo,
                    Unidadlicitacion = unidadlicitacion,
                    //inhabmed = inhabmed,
                    mensajeRcta = mensajeRcta,
                };


                return new JsonResult { Data = resultNew, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var medicamentos = "";
            return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



            //return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult MedicamentoDetalleMed(string clave)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            //RECETAS
            if (User.IsInRole("Mederos"))
            {

                //string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_mederos";
                //var result = db.Database.SqlQuery<RecetasMederos>(query);
                //var res = result.ToList();

                //var medicamentos = new List<Result>();

                var res2 = (from a in db2.Sustancia
                            join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                            from grupoIn in grupoX.DefaultIfEmpty()
                            where a.Clave == clave
                            select new
                            {

                                Id = a.Id,
                                Clave = a.Clave,
                                descripcion_21 = a.descripcion_21,
                                id_unidadeslicitacion_21 = a.id_unidadeslicitacion_21,
                                Descripcion_Grupo = grupoIn.descripcion,

                            }).FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(inventariofarmacia.Clave);

                if (res2 != null)
                {

                    string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_mederos where id_sustancia = " + res2.Id + "";
                    var result = db2.Database.SqlQuery<RecetasMederos>(query);
                    var res = result.FirstOrDefault();

                    var resultNew = new Object();

                    var habilitado = 1;

                    if (res.cactual <= 0)
                    {
                        //habilitado = 0;
                        habilitado = 1;
                    }
                    else
                    {
                        habilitado = 1;
                    }


                    decimal? unidadlicitacion = 0;

                    if (res2.id_unidadeslicitacion_21 != null)
                    {
                        //unidadlicitacion = desMed.unidadlicitacion;
                        var unidad = (from q in db2.unidadeslicitacion_21
                                      where q.id == res2.id_unidadeslicitacion_21
                                      //where q.descripcion_21 != null && q.descripcion_21 != ""
                                      select new
                                      {
                                          unidadlicitacion = q.descripcion,

                                      })
                                     .FirstOrDefault();

                        unidadlicitacion = unidad.unidadlicitacion;

                    }

                    var tipo = "";


                    resultNew = new
                    {
                        Clave = res2.Clave,
                        Descripcion_Sal = res2.descripcion_21,
                        Habilitado = habilitado,
                        Presentacion = res2.descripcion_21,
                        Descripcion_Grupo = res2.Descripcion_Grupo,
                        Inv_Act = res.cactual,
                        Tipo = tipo,
                        Unidadlicitacion = unidadlicitacion,
                    };

                    return new JsonResult { Data = resultNew, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }



        public JsonResult MedicamentoDetalleUER(string clave)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            //RECETAS
            if (User.IsInRole("UnidadER"))
            {

                //string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_mederos";
                //var result = db.Database.SqlQuery<RecetasMederos>(query);
                //var res = result.ToList();

                //var medicamentos = new List<Result>();

                var res2 = (from a in db2.Sustancia
                            join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                            from grupoIn in grupoX.DefaultIfEmpty()
                            where a.Clave == clave
                            select new
                            {

                                Id = a.Id,
                                Clave = a.Clave,
                                descripcion_21 = a.descripcion_21,
                                id_unidadeslicitacion_21 = a.id_unidadeslicitacion_21,
                                Descripcion_Grupo = grupoIn.descripcion,

                            }).FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(inventariofarmacia.Clave);

                if (res2 != null)
                {

                    string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_uder where id_sustancia = " + res2.Id + "";
                    var result = db2.Database.SqlQuery<RecetasMederos>(query);
                    var res = result.FirstOrDefault();

                    var resultNew = new Object();

                    var habilitado = 1;

                    if (res.cactual <= 0)
                    {
                        //habilitado = 0;
                        habilitado = 1;
                    }
                    else
                    {
                        habilitado = 1;
                    }


                    decimal? unidadlicitacion = 0;

                    if (res2.id_unidadeslicitacion_21 != null)
                    {
                        //unidadlicitacion = desMed.unidadlicitacion;
                        var unidad = (from q in db2.unidadeslicitacion_21
                                      where q.id == res2.id_unidadeslicitacion_21
                                      //where q.descripcion_21 != null && q.descripcion_21 != ""
                                      select new
                                      {
                                          unidadlicitacion = q.descripcion,

                                      })
                                     .FirstOrDefault();

                        unidadlicitacion = unidad.unidadlicitacion;

                    }

                    var tipo = "";


                    resultNew = new
                    {
                        Clave = res2.Clave,
                        Descripcion_Sal = res2.descripcion_21,
                        Habilitado = habilitado,
                        Presentacion = res2.descripcion_21,
                        Descripcion_Grupo = res2.Descripcion_Grupo,
                        Inv_Act = res.cactual,
                        Tipo = tipo,
                        Unidadlicitacion = unidadlicitacion,
                    };

                    return new JsonResult { Data = resultNew, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }


        public JsonResult MedicamentoDetalleCU(string clave)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            //RECETAS
            if (User.IsInRole("Ciudad Universitaria"))
            {

                //string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_mederos";
                //var result = db.Database.SqlQuery<RecetasMederos>(query);
                //var res = result.ToList();

                //var medicamentos = new List<Result>();

                var res2 = (from a in db2.Sustancia
                            join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                            from grupoIn in grupoX.DefaultIfEmpty()
                            where a.Clave == clave
                            select new
                            {

                                Id = a.Id,
                                Clave = a.Clave,
                                descripcion_21 = a.descripcion_21,
                                id_unidadeslicitacion_21 = a.id_unidadeslicitacion_21,
                                Descripcion_Grupo = grupoIn.descripcion,

                            }).FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(inventariofarmacia.Clave);

                if (res2 != null)
                {

                    string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_cu where id_sustancia = " + res2.Id + "";
                    var result = db2.Database.SqlQuery<RecetasMederos>(query);
                    var res = result.FirstOrDefault();

                    var resultNew = new Object();

                    var habilitado = 1;

                    if (res.cactual <= 0)
                    {
                        //habilitado = 0;
                        habilitado = 1;
                    }
                    else
                    {
                        habilitado = 1;
                    }


                    decimal? unidadlicitacion = 0;

                    if (res2.id_unidadeslicitacion_21 != null)
                    {
                        //unidadlicitacion = desMed.unidadlicitacion;
                        var unidad = (from q in db2.unidadeslicitacion_21
                                      where q.id == res2.id_unidadeslicitacion_21
                                      //where q.descripcion_21 != null && q.descripcion_21 != ""
                                      select new
                                      {
                                          unidadlicitacion = q.descripcion,

                                      })
                                     .FirstOrDefault();

                        unidadlicitacion = unidad.unidadlicitacion;

                    }

                    var tipo = "";


                    resultNew = new
                    {
                        Clave = res2.Clave,
                        Descripcion_Sal = res2.descripcion_21,
                        Habilitado = habilitado,
                        Presentacion = res2.descripcion_21,
                        Descripcion_Grupo = res2.Descripcion_Grupo,
                        Inv_Act = res.cactual,
                        Tipo = tipo,
                        Unidadlicitacion = unidadlicitacion,
                    };

                    return new JsonResult { Data = resultNew, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }


        public JsonResult MedicamentoDetalleFarmaco(string clave)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            //RECETAS
            if (User.IsInRole("RecetasFarmaco"))
            {

                //string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_mederos";
                //var result = db.Database.SqlQuery<RecetasMederos>(query);
                //var res = result.ToList();

                //var medicamentos = new List<Result>();

                var res2 = (from a in db2.Sustancia
                            join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                            from grupoIn in grupoX.DefaultIfEmpty()
                            where a.Clave == clave
                            select new
                            {

                                Id = a.Id,
                                Clave = a.Clave,
                                descripcion_21 = a.descripcion_21,
                                id_unidadeslicitacion_21 = a.id_unidadeslicitacion_21,
                                Descripcion_Grupo = grupoIn.descripcion,

                            }).FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(inventariofarmacia.Clave);

                if (res2 != null)
                {

                    string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_farmaco where id_sustancia = " + res2.Id + "";
                    var result = db2.Database.SqlQuery<RecetasMederos>(query);
                    var res = result.FirstOrDefault();

                    var resultNew = new Object();

                    var habilitado = 1;

                    if (res.cactual <= 0)
                    {
                        //habilitado = 0;
                        habilitado = 1;
                    }
                    else
                    {
                        habilitado = 1;
                    }


                    decimal? unidadlicitacion = 0;

                    if (res2.id_unidadeslicitacion_21 != null)
                    {
                        //unidadlicitacion = desMed.unidadlicitacion;
                        var unidad = (from q in db2.unidadeslicitacion_21
                                      where q.id == res2.id_unidadeslicitacion_21
                                      //where q.descripcion_21 != null && q.descripcion_21 != ""
                                      select new
                                      {
                                          unidadlicitacion = q.descripcion,

                                      })
                                     .FirstOrDefault();

                        unidadlicitacion = unidad.unidadlicitacion;

                    }

                    var tipo = "";


                    resultNew = new
                    {
                        Clave = res2.Clave,
                        Descripcion_Sal = res2.descripcion_21,
                        Habilitado = habilitado,
                        Presentacion = res2.descripcion_21,
                        Descripcion_Grupo = res2.Descripcion_Grupo,
                        Inv_Act = res.cactual,
                        Tipo = tipo,
                        Unidadlicitacion = unidadlicitacion,
                    };

                    return new JsonResult { Data = resultNew, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var medicamentos = "";
                return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }


        public JsonResult MedicamentoAlergia(string id, string clave)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var medicamento = (from q in db.InventarioFarmacia
                               where q.Clave == clave
                               select new
                               {
                                   Clave = q.Clave,
                                   Descripcion_Sal = q.Descripcion_Sal,
                                   Descripcion_Grupo = q.Descripcion_Grupo,
                                   Presentacion = q.Presentacion,
                                   Inv_Act = q.Inv_Act,
                               })
                              .FirstOrDefault();


            var alergias = (from d in db.Alergias_Exp
                            where d.num_exp == id
                            where d.estado == 1
                            select new
                            {
                                medicamento = d.medicamento,
                            })
                               .ToList();

            var info = "";

            foreach (var alergia in alergias)
            {
                string query = "SELECT * FROM InventarioFarmacia WHERE Descripcion_Sal like '%" + alergia.medicamento + "%' COLLATE Latin1_General_CI_AI and Clave = '" + medicamento.Clave + "'";
                var result = db.Database.SqlQuery<InventarioFarmacia>(query);
                var res = result.FirstOrDefault();

                if (res != null)
                {
                    //Es alergico al medicamento
                    info = "El paciente es alérgico al medicamento";
                }
            }


            return new JsonResult { Data = info, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult RecetasCronicas(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    //System.Diagnostics.Debug.WriteLine(res);

                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //Si no ha llegado el paciente a recepcion
                            if (res.hora_recepcion == null || res.hr_llamado == null)
                            {
                                //System.Diagnostics.Debug.WriteLine(res.hora_recepcion);

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " es presencial, haz clic en el boton llamar antes de crear una receta";
                                return RedirectToAction("Citas", "ServiciosMedicos");
                                //return RedirectToAction("Index", "Home");
                            }
                            //Si ya llego
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                                /*if (res.hr_consultorio == null)
                                {
                                    db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                                }*/

                                Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                                var nombreusuario = User.Identity.GetUserName();
                                var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                                var fecha = DateTime.Parse(fecha_string);

                                var exp = (from a in db3.expediente
                                           where a.numemp != id
                                           where a.medico == nombreusuario
                                           where a.hora_termino == null
                                           where a.fecha == fecha
                                           select a).FirstOrDefault();

                                //System.Diagnostics.Debug.WriteLine(exp);

                                if (exp == null)
                                {
                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    var especialidad = nombreusuario.Substring(0, 2);

                                    Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                                    if (dhabientes.NUMAFIL != "P72112")
                                    {
                                        return View(dhabientes);
                                    }
                                    else
                                    {
                                        TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                        return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                    }
                                }
                                else
                                {
                                    TempData["numemp"] = exp.numemp;
                                    return RedirectToAction("SOAP/" + exp.numemp, "ServiciosMedicos");
                                }

                            }


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");
                            //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                            /*if (res.hr_consultorio == null)
                            {
                                db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                            }*/

                            Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                            var nombreusuario = User.Identity.GetUserName();
                            var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                            var fecha = DateTime.Parse(fecha_string);

                            var exp = (from a in db3.expediente
                                       where a.numemp != id
                                       where a.medico == nombreusuario
                                       where a.hora_termino == null
                                       where a.fecha == fecha
                                       select a).FirstOrDefault();

                            if (exp == null)
                            {
                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                var especialidad = nombreusuario.Substring(0, 2);

                                Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                                if (dhabientes.NUMAFIL != "P72112")
                                {
                                    return View(dhabientes);
                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }
                            }
                            else
                            {
                                TempData["numemp"] = exp.numemp;
                                return RedirectToAction("SOAP/" + exp.numemp, "ServiciosMedicos");
                            }

                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {

                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();


                        Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                        if (dhabientes.NUMAFIL != "P72112")
                        {
                            return View(dhabientes);
                        }
                        else
                        {
                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                            return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }

            }
        }


        public ActionResult RecetasCronicas2(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    //System.Diagnostics.Debug.WriteLine(res);

                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //Si no ha llegado el paciente a recepcion
                            if (res.hora_recepcion == null || res.hr_llamado == null)
                            {
                                //System.Diagnostics.Debug.WriteLine(res.hora_recepcion);

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " es presencial, haz clic en el boton llamar antes de crear una receta";
                                return RedirectToAction("Citas", "ServiciosMedicos");
                                //return RedirectToAction("Index", "Home");
                            }
                            //Si ya llego
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                                /*if (res.hr_consultorio == null)
                                {
                                    db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                                }*/

                                Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                                var nombreusuario = User.Identity.GetUserName();
                                var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                                var fecha = DateTime.Parse(fecha_string);

                                var exp = (from a in db3.expediente
                                           where a.numemp != id
                                           where a.medico == nombreusuario
                                           where a.hora_termino == null
                                           where a.fecha == fecha
                                           select a).FirstOrDefault();

                                //System.Diagnostics.Debug.WriteLine(exp);

                                if (exp == null)
                                {
                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    var especialidad = nombreusuario.Substring(0, 2);

                                    Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                                    if (dhabientes.NUMAFIL != "P72112")
                                    {
                                        return View(dhabientes);
                                    }
                                    else
                                    {
                                        TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                        return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                    }
                                }
                                else
                                {
                                    TempData["numemp"] = exp.numemp;
                                    return RedirectToAction("SOAP/" + exp.numemp, "ServiciosMedicos");
                                }

                            }


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");
                            //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                            /*if (res.hr_consultorio == null)
                            {
                                db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                            }*/

                            Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                            var nombreusuario = User.Identity.GetUserName();
                            var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                            var fecha = DateTime.Parse(fecha_string);

                            var exp = (from a in db3.expediente
                                       where a.numemp != id
                                       where a.medico == nombreusuario
                                       where a.hora_termino == null
                                       where a.fecha == fecha
                                       select a).FirstOrDefault();

                            if (exp == null)
                            {
                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                var especialidad = nombreusuario.Substring(0, 2);

                                Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                                if (dhabientes.NUMAFIL != "P72112")
                                {
                                    return View(dhabientes);
                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }
                            }
                            else
                            {
                                TempData["numemp"] = exp.numemp;
                                return RedirectToAction("SOAP/" + exp.numemp, "ServiciosMedicos");
                            }

                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {

                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();


                        Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                        if (dhabientes.NUMAFIL != "P72112")
                        {
                            return View(dhabientes);
                        }
                        else
                        {
                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                            return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }

            }
        }


        public JsonResult RecetasCronicasMed(string id)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            //RECETAS
            //Buscar resurtimientos
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_actual_correcta = DateTime.Parse(fecha_actual);

            var recetascro = (from rc in db.receta_detalle_crn
                              join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                              from receIn in receExp.DefaultIfEmpty()
                              join inventario in db.InventarioFarmacia on rc.codigo equals inventario.Clave into inventarioExp
                              from inventarioIn in inventarioExp.DefaultIfEmpty()
                              join medicos in db.MEDICOS on receIn.medico_crea equals medicos.Numero into medicosExp
                              from medicosIn in medicosExp.DefaultIfEmpty()
                              join sustan in db.Sustancia on rc.codigo equals sustan.Clave into susX
                              from susIn in susX.DefaultIfEmpty()
                              where susIn.Consultorio != "0"
                              //where inventarioIn.Inv_Act != 0
                              //Expediente va a ser igual al que se envia
                              where receIn.expediente == id
                              //Mientras no la hayan recetado hoy
                              //where receIn.fecha_crea == fecha_actual_correcta
                              //where receIn.terminada != 1
                              select new
                              {
                                  Clave = inventarioIn.Clave,
                                  Clave_Nivel = inventarioIn.Clave_Nivel,
                                  Descripcion_Sal = inventarioIn.Descripcion_Sal,
                                  Descripcion_Grupo = inventarioIn.Descripcion_Grupo,
                                  Presentacion = inventarioIn.Presentacion,
                                  Dosis = rc.dosis,
                                  //Inv_Act = q.Inv_Act,
                                  Folio_rc = rc.folio_rc,
                                  Meses_surtir = receIn.meses_surtir,
                                  Fecha = receIn.fecha_crea,
                                  Medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno
                                  //codigo = rc.codigo,
                                  //meses_surtir = receExp.meses_surtir,
                              })
                              .OrderByDescending(g => g.Folio_rc)
                              .ToList();

            //Ttomar los ultimos resurtimientos del ultimo año
            /*var ultimo_res = (from ur in db.receta_exp_crn
                              where ur.expediente == id
                              where ur.fecha_crea >= fecha_actual_correcta
                              select new{
                                  fecha_crea = ur.fecha_crea,
                                  folio_rc = ur.folio_rc,
                              })
                              .OrderByDescending(g => g.fecha_crea)
                              .FirstOrDefault();*/

            //Si activo uno y no ha dado en terminar
            var ultimo_res_count = (from ur in db.receta_exp_crn
                                    where ur.expediente == id
                                    where ur.fecha_crea == fecha_actual_correcta
                                    where ur.terminada != 1
                                    select ur)
                                  .Count();

            //Ver si esta activo o no 

            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();



            var medicamentos = new List<Result>();

            foreach (var medicamento in recetascro)
            {

                var recetas_exp = (from q in db2.Receta_Exp
                                   where q.Expediente == id
                                   //Si no se le dio, se quedo con estado de kiosko
                                   where q.Estado != "7"
                                   where q.folio_rc == medicamento.Folio_rc
                                   select q).Count();

                var recetas2Count = recetas_exp + 1;

                //Varable para saber si esta activa o no la receta de resurtimiento
                var estado = "";
                //Si los eventos no se han terminado
                if (recetas2Count < medicamento.Meses_surtir)
                {
                    estado = "Activa";
                }
                else
                {
                    estado = "Inactiva";
                }


                //Boton reactivar
                var boton = "";
                //Fecha del ultimo resurtimiento menos una semana
                var fecha_unasemanaatras = DateTime.Now.AddMonths(-medicamento.Meses_surtir).AddDays(-7).ToString("yyyy-MM-dd");
                var fecha_unasemanaatras2 = DateTime.Parse(fecha_unasemanaatras);
                //Fecha del ultimo resurtimiento mas una semana
                var fecha_unasemandespues = DateTime.Now.AddMonths(-medicamento.Meses_surtir).AddDays(7).ToString("yyyy-MM-dd");
                var fecha_unasemandespues2 = DateTime.Parse(fecha_unasemandespues);

                //Fecha menos una semana
                var fecha_hoy_unasemanaatras = DateTime.Now.AddDays(-9).ToString("yyyy-MM-dd");
                var fecha_hoy_unasemanaatras2 = DateTime.Parse(fecha_hoy_unasemanaatras);
                //Fecha mas una semana
                var fecha_hoy_unasemandespues = DateTime.Now.AddDays(9).ToString("yyyy-MM-dd");
                var fecha_hoy_unasemandespues2 = DateTime.Parse(fecha_hoy_unasemandespues);


                //Fecha del resurtimiento
                //var fechares = string.Format("{0:yyyy-MM-dd}", medicamento.Fecha);
                var fecha_unyear = DateTime.Now.AddMonths(-10).ToString("yyyy-MM-dd");
                var fecha_unyear2 = DateTime.Parse(fecha_unyear);


                //Mienstras que la fecha no 
                if (medicamento.Fecha >= fecha_unyear2 && ultimo_res_count == 0 && medicamento.Fecha != fecha_actual_correcta)
                {
                    //Mientras no se le haya surtido en los ultimos días
                    /*if(ultimo_res.fecha_crea >= fecha_hoy_unasemanaatras2 && ultimo_res.fecha_crea <= fecha_hoy_unasemandespues2) {
                        boton = "No reactivar";
                    }
                    else
                    {*/
                    var fechaNCB2 = DateTime.Now.ToString("2022-04-11T00:00:00.000");
                    var fechaNCBDT2 = DateTime.Parse(fechaNCB2);

                    if (medicamento.Fecha >= fechaNCBDT2)
                    {
                        boton = "Reactivar";
                    }
                    //boton = "Reactivar";
                    /*}*/

                }
                else
                {
                    boton = "No reactivar";
                }

                //COORDINADORES NIVEL 7
                if (User.IsInRole("Coordinador"))
                {
                    //buscar sustancia


                    var listamedicamentos = new Result
                    {
                        Clave = medicamento.Clave + "*" + medicamento.Folio_rc.ToString() + "*" + id,
                        Medicamento = "<span><b>" + medicamento.Descripcion_Sal + "</b></span><br><span>" + medicamento.Descripcion_Grupo + "</span><br><span>" + medicamento.Presentacion + "</span>",
                        Dosis = medicamento.Dosis,
                        Medico = medicamento.Medico,
                        //Descripcion_Grupo = medicamento.Descripcion_Grupo,
                        //Presentacion = medicamento.Presentacion,
                        Fecha = string.Format("{0:yyyy-MM-dd}", medicamento.Fecha),
                        Detalles = medicamento.Folio_rc.ToString() + "*" + medicamento.Meses_surtir + "*" + string.Format("{0:yyyy-MM-dd}", medicamento.Fecha) + "*" + estado + "*" + boton
                    };

                    medicamentos.Add(listamedicamentos);
                }
                else
                {
                    if (medicamento.Clave_Nivel != "7")
                    {
                        var listamedicamentos = new Result
                        {
                            Clave = medicamento.Clave + "*" + medicamento.Folio_rc.ToString() + "*" + id,
                            Medicamento = "<span><b>" + medicamento.Descripcion_Sal + "</b></span><br><span>" + medicamento.Descripcion_Grupo + "</span><br><span>" + medicamento.Presentacion + "</span>",
                            Dosis = medicamento.Dosis,
                            Medico = medicamento.Medico,
                            //Descripcion_Grupo = medicamento.Descripcion_Grupo,
                            //Presentacion = medicamento.Presentacion,
                            Fecha = string.Format("{0:yyyy-MM-dd}", medicamento.Fecha),
                            Detalles = medicamento.Folio_rc.ToString() + "*" + medicamento.Meses_surtir + "*" + string.Format("{0:yyyy-MM-dd}", medicamento.Fecha) + "*" + estado + "*" + boton
                        };

                        medicamentos.Add(listamedicamentos);
                    }
                }



            }

            return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        /*
        private static WebService createWebService()
        {
            WebService service = new WebService();

            string url = ConfigurationManager.AppSettings["webserviceURL"];

            return service;
        }
        */

        public JsonResult RecetasCronicasMed2(string id)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            //RECETAS
            //Buscar resurtimientos
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_actual_correcta = DateTime.Parse(fecha_actual);
            var fecha2 = DateTime.Now.ToString("2022-01-06T00:00:00.000");
            var fechaDT2 = DateTime.Parse(fecha2);

            var recetascro = (from rc in db.receta_detalle_crn
                              join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                              from receIn in receExp.DefaultIfEmpty()
                              join inventario in db.InventarioFarmacia on rc.codigo equals inventario.Clave into inventarioExp
                              from inventarioIn in inventarioExp.DefaultIfEmpty()
                              join medicos in db.MEDICOS on receIn.medico_crea equals medicos.Numero into medicosExp
                              from medicosIn in medicosExp.DefaultIfEmpty()
                              join sustan in db.Sustancia on rc.codigo equals sustan.Clave into susX
                              from susIn in susX.DefaultIfEmpty()
                              //where inventarioIn.Clave != null
                              //where inventarioIn.Inv_Act != 0
                              //Expediente va a ser igual al que se envia
                              where receIn.expediente == id
                              //Mientras no la hayan recetado hoy
                              //where receIn.fecha_crea == fecha_actual_correcta
                              //where receIn.terminada != 1
                              select new
                              {
                                  Clave = inventarioIn.Clave,
                                  Clave_Nivel = inventarioIn.Clave_Nivel,
                                  Descripcion_Sal = inventarioIn.Descripcion_Sal,
                                  Descripcion_Grupo = inventarioIn.Descripcion_Grupo,
                                  Presentacion = inventarioIn.Presentacion,
                                  Dosis = rc.dosis,
                                  //Inv_Act = q.Inv_Act,
                                  Folio_rc = rc.folio_rc,
                                  Meses_surtir = receIn.meses_surtir,
                                  Fecha = receIn.fecha_crea,
                                  Medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  //SobranteInv2022 = susIn.SobranteInv2022
                                  //codigo = rc.codigo,
                                  //meses_surtir = receExp.meses_surtir,
                              })
                              .OrderByDescending(g => g.Folio_rc)
                              .ToList();

            //System.Diagnostics.Debug.WriteLine(recetascro);

            //Ttomar los ultimos resurtimientos del ultimo año
            /*var ultimo_res = (from ur in db.receta_exp_crn
                              where ur.expediente == id
                              where ur.fecha_crea >= fecha_actual_correcta
                              select new{
                                  fecha_crea = ur.fecha_crea,
                                  folio_rc = ur.folio_rc,
                              })
                              .OrderByDescending(g => g.fecha_crea)
                              .FirstOrDefault();*/

            //Si activo uno y no ha dado en terminar
            var ultimo_res_count = (from ur in db.receta_exp_crn
                                    where ur.expediente == id
                                    where ur.fecha_crea == fecha_actual_correcta
                                    where ur.terminada != 1
                                    select ur)
                                  .Count();

            //Ver si esta activo o no 

            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();



            var medicamentos = new List<Result>();

            foreach (var medicamento in recetascro)
            {

                var recetas_exp = (from q in db2.Receta_Exp
                                   //where q.Expediente == id
                                   //Si no se le dio, se quedo con estado de kiosko
                                   where q.Estado != "7"
                                   where q.folio_rc == medicamento.Folio_rc
                                   select q).Count();

                var recetas2Count = recetas_exp + 1;

                //Varable para saber si esta activa o no la receta de resurtimiento
                var estado = "";
                //Si los eventos no se han terminado
                if (recetas2Count < medicamento.Meses_surtir)
                {
                    estado = "Activa";
                }
                else
                {
                    estado = "Inactiva";
                }


                //Boton reactivar
                var boton = "";
                //Fecha del ultimo resurtimiento menos una semana
                var fecha_unasemanaatras = DateTime.Now.AddMonths(-medicamento.Meses_surtir).AddDays(-7).ToString("yyyy-MM-dd");
                var fecha_unasemanaatras2 = DateTime.Parse(fecha_unasemanaatras);
                //Fecha del ultimo resurtimiento mas una semana
                var fecha_unasemandespues = DateTime.Now.AddMonths(-medicamento.Meses_surtir).AddDays(7).ToString("yyyy-MM-dd");
                var fecha_unasemandespues2 = DateTime.Parse(fecha_unasemandespues);

                //Fecha menos una semana
                var fecha_hoy_unasemanaatras = DateTime.Now.AddDays(-9).ToString("yyyy-MM-dd");
                var fecha_hoy_unasemanaatras2 = DateTime.Parse(fecha_hoy_unasemanaatras);
                //Fecha mas una semana
                var fecha_hoy_unasemandespues = DateTime.Now.AddDays(9).ToString("yyyy-MM-dd");
                var fecha_hoy_unasemandespues2 = DateTime.Parse(fecha_hoy_unasemandespues);


                //Fecha del resurtimiento
                //var fechares = string.Format("{0:yyyy-MM-dd}", medicamento.Fecha);
                var fecha_unyear = DateTime.Now.AddMonths(-10).ToString("yyyy-MM-dd");
                var fecha_unyear2 = DateTime.Parse(fecha_unyear);

                /*
                if (estado == "Activa")
                {
                    var fechaNCB2 = DateTime.Now.ToString("2022-04-11T00:00:00.000");
                    var fechaNCBDT2 = DateTime.Parse(fechaNCB2);

                    if (medicamento.Fecha >= fechaNCBDT2)
                    {
                        //buscar si se esta editando o no una receta

                        var recetaEdt = (from r in db.receta_exp_crn
                                       where r.folio_rc == medicamento.Folio_rc
                                       where r.editada == 1
                                       select new
                                       {
                                           folio_rc = r.folio_rc,
                                       }).FirstOrDefault();

                        if(recetaEdt != null)
                        {
                            boton = "Editando";
                        }
                        else
                        {
                            boton = "Editar";
                        }

                    }
                    else
                    {
                        boton = "No reactivar";
                    }
                }
                else
                {*/
                    //Mieditarresnstras que la fecha no 
                    if (medicamento.Fecha >= fecha_unyear2 && ultimo_res_count == 0 && medicamento.Fecha != fecha_actual_correcta)
                    {
                        //Mientras no se le haya surtido en los ultimos días
                        /*if(ultimo_res.fecha_crea >= fecha_hoy_unasemanaatras2 && ultimo_res.fecha_crea <= fecha_hoy_unasemandespues2) {
                            boton = "No reactivar";
                        }
                        else
                        {*/
                        var clavemedico = User.Identity.GetUserName().Substring(0, 2);
                        var username = User.Identity.GetUserName();

                        if (User.IsInRole("Coordinador") || clavemedico == "13" || username == "60003" || username == "02101")
                        {
                            var fechaNCB2 = DateTime.Now.ToString("2022-04-11T00:00:00.000");
                            var fechaNCBDT2 = DateTime.Parse(fechaNCB2);

                            if (medicamento.Fecha >= fechaNCBDT2)
                            {
                                boton = "Reactivar";
                            }
                            //boton = "Reactivar";
                        }
                        else
                        {
                            boton = "No reactivar";
                        }
                        /*}*/

                    }
                    else
                    {
                        /*
                        var fecha = DateTime.Now.ToString("yyyy-MM-dd");
                        var fechaDT = DateTime.Parse(fecha);

                        if (medicamento.Fecha == fechaDT)
                        {
                            boton = "Editar";
                        }
                        else
                        {*/
                        boton = "No reactivar";
                        // }

                    }

                //}

                //COORDINADORES NIVEL 7
                if (User.IsInRole("Coordinador"))
                {
                    //buscar sustancia

                    //Estatus Vic

                    var susSobra = (from a in db2.Sustancia
                                    join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                                    from grupoIn in grupoX.DefaultIfEmpty()
                                    where a.Clave == medicamento.Clave
                                    select new
                                     {

                                         Id = a.Id,
                                         Clave = a.Clave,
                                         descripcion_21 = a.descripcion_21,
                                         Descripcion_Grupo = grupoIn.descripcion,

                                     }).FirstOrDefault();

                    if (susSobra != null)
                    {

                    //
                    var fechaNCB2 = DateTime.Now.ToString("2022-04-11T00:00:00.000");
                    var fechaNCBDT2 = DateTime.Parse(fechaNCB2);

                    var medicamentoPres = "";

                    if (medicamento.Fecha < fechaNCBDT2)
                    {
                        medicamentoPres = "<span><b>" + medicamento.Descripcion_Sal + "</b></span><br><span>" + medicamento.Descripcion_Grupo + "</span><br><span>" + medicamento.Presentacion + "</span>";
                        
                    }
                    else
                    {
                        //descripcion 21
                        medicamentoPres = "<span><b>" + susSobra.descripcion_21 + "</b></span><br><span>" + susSobra.Descripcion_Grupo + "</span><br>";
                    }

                    //if (susSobra.SobranteInv2022 == "2")
                    //{
                    var listamedicamentos = new Result
                        {
                            Clave = medicamento.Clave + "*" + medicamento.Folio_rc.ToString() + "*" + id,
                        //Medicamento = "<span><b>" + medicamento.Descripcion_Sal + "</b></span><br><span>" + medicamento.Descripcion_Grupo + "</span><br><span>" + medicamento.Presentacion + "</span>",
                            Medicamento = medicamentoPres,
                            Dosis = medicamento.Dosis,
                            Medico = medicamento.Medico,
                            //Descripcion_Grupo = medicamento.Descripcion_Grupo,
                            //Presentacion = medicamento.Presentacion,
                            Fecha = string.Format("{0:yyyy-MM-dd}", medicamento.Fecha),
                            Detalles = medicamento.Folio_rc.ToString() + "*" + medicamento.Meses_surtir + "*" + string.Format("{0:yyyy-MM-dd}", medicamento.Fecha) + "*" + estado + "*" + boton
                        };

                        medicamentos.Add(listamedicamentos);
                        //}

                    }
                }
                else
                {
                    //if (medicamento.Clave_Nivel != "7")
                    //{

                        var susSobra = (from a in db2.Sustancia
                                        join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                                        from grupoIn in grupoX.DefaultIfEmpty()
                                        where a.Clave == medicamento.Clave
                                        select new
                                        {

                                            Id = a.Id,
                                            Clave = a.Clave,
                                            descripcion_21 = a.descripcion_21,
                                            Descripcion_Grupo = grupoIn.descripcion,

                                        }).FirstOrDefault();

                        //
                        //System.Diagnostics.Debug.WriteLine(susSobra);
                        if(susSobra != null)
                        {

                        
                        //
                        var fechaNCB2 = DateTime.Now.ToString("2022-04-11T00:00:00.000");
                        var fechaNCBDT2 = DateTime.Parse(fechaNCB2);

                        var medicamentoPres = "";

                        if (medicamento.Fecha < fechaNCBDT2)
                        {
                            medicamentoPres = "<span><b>" + medicamento.Descripcion_Sal + "</b></span><br><span>" + medicamento.Descripcion_Grupo + "</span><br><span>" + medicamento.Presentacion + "</span>";

                        }
                        else
                        {
                            //descripcion 21
                            medicamentoPres = "<span><b>" + susSobra.descripcion_21 + "</b></span><br><span>" + susSobra.Descripcion_Grupo + "</span><br>";
                        }
                        //Estatus Vic
                        //if (susSobra.SobranteInv2022 == "2")
                        //{
                        var listamedicamentos = new Result
                        {
                            Clave = medicamento.Clave + "*" + medicamento.Folio_rc.ToString() + "*" + id,
                            //Medicamento = "<span><b>" + medicamento.Descripcion_Sal + "</b></span><br><span>" + medicamento.Descripcion_Grupo + "</span><br><span>" + medicamento.Presentacion + "</span>",
                            Medicamento = medicamentoPres,
                            Dosis = medicamento.Dosis,
                            Medico = medicamento.Medico,
                            //Descripcion_Grupo = medicamento.Descripcion_Grupo,
                            //Presentacion = medicamento.Presentacion,
                            Fecha = string.Format("{0:yyyy-MM-dd}", medicamento.Fecha),
                            Detalles = medicamento.Folio_rc.ToString() + "*" + medicamento.Meses_surtir + "*" + string.Format("{0:yyyy-MM-dd}", medicamento.Fecha) + "*" + estado + "*" + boton
                        };

                        medicamentos.Add(listamedicamentos);
                        //}

                        }
                    //}

                }



            }



            //System.Diagnostics.Debug.WriteLine(medicamentos);

            return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult RecetasPruebas()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            //RECETAS
            //Buscar resurtimientos
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_actual_correcta = DateTime.Parse(fecha_actual);
            var fecha2 = DateTime.Now.ToString("2022-06-01");
            var fechaDT2 = DateTime.Parse(fecha2);

            var recetascro = (from rc in db.receta_detalle_crn
                              join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                              from receIn in receExp.DefaultIfEmpty()
                              where receIn.fecha_crea >= fechaDT2
                              where receIn.terminada == 1
                              select new
                              {
                                  folio_rc = rc.folio_rc,
                                  clave = rc.codigo,
                                  meses_surtir = receIn.meses_surtir,
                              })
                              .OrderByDescending(g => g.folio_rc)
                              .ToList();

            //System.Diagnostics.Debug.WriteLine(recetascro);

            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();



            var medicamentos = new List<Result>();

            foreach (var medicamento in recetascro)
            {

                var recetas_exp = (from q in db2.Receta_Exp
                                   where q.Estado != "7"
                                   where q.folio_rc == medicamento.folio_rc
                                   select q).Count();

                
                var susSobra = (from a in db2.Sustancia
                                where a.Clave == medicamento.clave
                                select new
                                {
                                    Clave = a.Clave,
                                    descripcion_21 = a.descripcion_21,
                                }).FirstOrDefault();

                
                var recetas2Count = recetas_exp + 1;

                //Varable para saber si esta activa o no la receta de resurtimiento
                var estado = "";
                //Si los eventos no se han terminado
                if (recetas2Count < medicamento.meses_surtir)
                {
                    if(susSobra != null)
                    {
                        var listamedicamentos = new Result
                        {
                            Clave = medicamento.clave,
                            Medicamento = susSobra.descripcion_21,
                        };

                        medicamentos.Add(listamedicamentos);
                    }
   
                }

            }


            //sSystem.Diagnostics.Debug.WriteLine(medicamentos);

            return View(medicamentos);

        }




        public JsonResult ResurtirMedicamento(int folio_rc, string codigo, string numemp)
        {

            //System.Diagnostics.Debug.WriteLine(folio_rc);
            //System.Diagnostics.Debug.WriteLine(codigo);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var nombreusuario = User.Identity.GetUserName();

            var fecha_actual = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_actual_correcta = DateTime.Parse(fecha_actual);

            var receta_hoy = (from rc in db.receta_exp_crn
                              where rc.fecha_crea == fecha_actual_correcta
                              where rc.terminada == 0
                              where rc.expediente == numemp
                              //where rc.medico_crea == nombreusuario
                              select new
                              {
                                  Meses_surtir = rc.meses_surtir,
                                  fec_ini_sintomas = rc.fec_ini_sintomas,
                              }).FirstOrDefault();

            var medicamento = (from rc in db.receta_detalle_crn
                               join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                               from receIn in receExp.DefaultIfEmpty()
                               join inventario in db.InventarioFarmacia on rc.codigo equals inventario.Clave into inventarioExp
                               from inventarioIn in inventarioExp.DefaultIfEmpty()
                               join medicos in db.MEDICOS on receIn.medico_crea equals medicos.Numero into medicosExp
                               from medicosIn in medicosExp.DefaultIfEmpty()
                               where rc.folio_rc == folio_rc
                               where rc.codigo == codigo
                               select new
                               {
                                   numemp = receIn.expediente,
                                   medico_numero = nombreusuario,
                                   Clave = inventarioIn.Clave,
                                   Descripcion_Sal = inventarioIn.Descripcion_Sal,
                                   Descripcion_Grupo = inventarioIn.Descripcion_Grupo,
                                   Presentacion = inventarioIn.Presentacion,
                                   Dosis = rc.dosis,
                                   Cantidad = rc.cantidad,
                                   Indicaciones = rc.indicaciones,
                                   //Inv_Act = q.Inv_Act,
                                   Folio_rc = rc.folio_rc,
                                   Meses_surtir = receIn.meses_surtir,
                                   Fecha = receIn.fecha_crea,
                                   Medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                   fec_ini_sintomas = receIn.fec_ini_sintomas,
                                   //meses_surtir = receExp.meses_surtir,
                               })
                                  .OrderByDescending(g => g.Folio_rc)
                                  .FirstOrDefault();

            int meses_surtir;
            DateTime fec_ini_sintomas;

            if (receta_hoy != null)
            {
                meses_surtir = receta_hoy.Meses_surtir;
                fec_ini_sintomas = receta_hoy.fec_ini_sintomas;
            }
            else
            {
                meses_surtir = medicamento.Meses_surtir;
                fec_ini_sintomas = medicamento.fec_ini_sintomas;
            }

            var resultado = new
            {
                numemp = medicamento.numemp,
                medico_numero = medicamento.medico_numero,
                Clave = medicamento.Clave,
                Descripcion_Sal = medicamento.Descripcion_Sal,
                Descripcion_Grupo = medicamento.Descripcion_Grupo,
                Presentacion = medicamento.Presentacion,
                Dosis = medicamento.Dosis,
                Cantidad = medicamento.Cantidad,
                Folio_rc = medicamento.Folio_rc,
                Meses_surtir = meses_surtir,
                Fecha = medicamento.Fecha,
                Medico = medicamento.Medico,
                Indicaciones = medicamento.Indicaciones,
                fec_ini_sintomas = string.Format("{0:yyyy-MM-dd}", fec_ini_sintomas),
            };

            //System.Diagnostics.Debug.WriteLine(medicamento.Folio_rc);

            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult ResurtirNuevoMedicamento(string codigo, int folio_rc)
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();


            //string query = "select id_sustancia as id_sustancia, cactual as cactual from inv_mederos";
            //var result = db.Database.SqlQuery<RecetasMederos>(query);
            //var res = result.ToList();

            //var medicamentos = new List<Result>();

            var res2 = (from a in db2.Sustancia
                        join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                        from grupoIn in grupoX.DefaultIfEmpty()
                        where a.Clave == codigo
                        select new
                        {

                            Id = a.Id,
                            Clave = a.Clave,
                            descripcion_21 = a.descripcion_21,
                            id_unidadeslicitacion_21 = a.id_unidadeslicitacion_21,
                            Descripcion_Grupo = grupoIn.descripcion,

                        }).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(inventariofarmacia.Clave);

            if (res2 != null)
            {

                //string query = "select Id_Sustancia as id_sustancia, Inv_Act as cactual from InvFarm where Id_Sustancia = " + res2.Id + "";
                //string query2 = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + res2.Id + "";
                //var result = db2.Database.SqlQuery<RecetasMederos>(query2);
                //var res = result.FirstOrDefault();

                string query = "SELECT Sustancia_1.SobranteInv2022 as sobranteinv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' and id_sustancia = " + res2.Id + "";
                var result = db2.Database.SqlQuery<LstInv>(query);
                var res = result.FirstOrDefault();

                var resultNew = new Object();

                var habilitado = 1;

                if (res.inv_act <= 0)
                {
                    habilitado = 0;
                }
                else
                {
                    habilitado = 1;
                }


                decimal? unidadlicitacion = 0;

                if (res2.id_unidadeslicitacion_21 != null)
                {
                    //unidadlicitacion = desMed.unidadlicitacion;
                    var unidad = (from q in db2.unidadeslicitacion_21
                                  where q.id == res2.id_unidadeslicitacion_21
                                  //where q.descripcion_21 != null && q.descripcion_21 != ""
                                  select new
                                  {
                                      unidadlicitacion = q.descripcion,

                                  })
                                 .FirstOrDefault();

                    unidadlicitacion = unidad.unidadlicitacion;

                }

                var tipo = "";


                int meses_surtir;
                DateTime fec_ini_sintomas;

               meses_surtir = 1;
               fec_ini_sintomas = DateTime.Now;
                


                resultNew = new
                {
                    Clave = res2.Clave,
                    Descripcion_Sal = res2.descripcion_21,
                    Habilitado = habilitado,
                    Presentacion = res2.descripcion_21,
                    Descripcion_Grupo = res2.Descripcion_Grupo,
                    Inv_Act = res.inv_act,
                    Tipo = tipo,
                    Unidadlicitacion = unidadlicitacion,
                    Meses_surtir = meses_surtir,
                    fec_ini_sintomas = string.Format("{0:yyyy-MM-dd}", fec_ini_sintomas),
                };

                return new JsonResult { Data = resultNew, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            var medicamentos = "";
            return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            
        }


        public JsonResult ReactivarResurtimiento(int folio_rc)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var nombreusuario = User.Identity.GetUserName();

            var fecha_crea = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_crea_correcta = DateTime.Parse(fecha_crea);

            var reactivar = (from rc in db.receta_exp_crn
                             where rc.folio_rc == folio_rc
                             select rc).FirstOrDefault();

            var fechaSintomas = string.Format("{0:yyyy-MM-ddT00:00:00.000}", reactivar.fec_ini_sintomas);

            db.Database.ExecuteSqlCommand("INSERT INTO receta_exp_crn (medico_crea, medico_ref, fecha_crea, estado, platica_edu, lab_relev, trat_actual, fec_ini_sintomas, evento_adverso, meses_surtir, expediente, policl, terminada) VALUES('" + nombreusuario + "', '00','" + fecha_crea + "',0,0,'','','" + fechaSintomas + "','','" + reactivar.meses_surtir + "', '" + reactivar.expediente + "',0,0)");

            var ultimo_folio_rc = (from rc in db.receta_exp_crn
                                       //where rc.folio_rc == folio_rc
                                   where rc.expediente == reactivar.expediente
                                   where rc.fecha_crea == fecha_crea_correcta
                                   where rc.medico_crea == nombreusuario
                                   select rc).FirstOrDefault();


            //Buscar detalles de los medicmentos a reactivar
            var reactivar_detalles = (from rc in db.receta_detalle_crn
                                      join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                                      from receIn in receExp.DefaultIfEmpty()
                                      join inventario in db.InventarioFarmacia on rc.codigo equals inventario.Clave into inventarioExp
                                      from inventarioIn in inventarioExp.DefaultIfEmpty()
                                      join medicos in db.MEDICOS on receIn.medico_crea equals medicos.Numero into medicosExp
                                      from medicosIn in medicosExp.DefaultIfEmpty()
                                      where rc.folio_rc == folio_rc
                                      select rc).ToList();

            //System.Diagnostics.Debug.WriteLine(reactivar_detalles);


            foreach (var item in reactivar_detalles)
            {
                //Insertar costo de recetas
                var costoMedicamento = (from a in db.Sustancia
                                        where a.Clave == item.codigo
                                        select a).FirstOrDefault();

                //No agregar si tiene el sobranteinv21

                //System.Diagnostics.Debug.WriteLine(costoMedicamento);


                float? costo = 0;

                if (costoMedicamento != null)
                {
                    costo = item.cantidad * costoMedicamento.cto_promedio;
                }

                //System.Diagnostics.Debug.WriteLine(costo);


                db.Database.ExecuteSqlCommand("INSERT INTO receta_detalle_crn (folio_rc, codigo, cantidad, dosis, indicaciones, costo) VALUES('" + ultimo_folio_rc.folio_rc + "', '" + item.codigo + "','" + item.cantidad + "','" + item.dosis + "','" + item.indicaciones + "','" + costo + "')");

            }

            TempData["message_success"] = "Receta reactivada con éxito";
            return new JsonResult { Data = reactivar, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult ReactivarResurtimiento2(int folio_rc)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            var nombreusuario = User.Identity.GetUserName();

            var fecha_crea = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_crea_correcta = DateTime.Parse(fecha_crea);

            var reactivar = (from rc in db.receta_exp_crn
                             where rc.folio_rc == folio_rc
                             select rc).FirstOrDefault();

            var fechaSintomas = string.Format("{0:yyyy-MM-ddT00:00:00.000}", reactivar.fec_ini_sintomas);

            db.Database.ExecuteSqlCommand("INSERT INTO receta_exp_crn (medico_crea, medico_ref, fecha_crea, estado, platica_edu, lab_relev, trat_actual, fec_ini_sintomas, evento_adverso, meses_surtir, expediente, policl, terminada) VALUES('" + nombreusuario + "', '00','" + fecha_crea + "',0,0,'','','" + fechaSintomas + "','','" + reactivar.meses_surtir + "', '" + reactivar.expediente + "',0,0)");

            var ultimo_folio_rc = (from rc in db.receta_exp_crn
                                       //where rc.folio_rc == folio_rc
                                   where rc.expediente == reactivar.expediente
                                   where rc.fecha_crea == fecha_crea_correcta
                                   where rc.medico_crea == nombreusuario
                                   select rc).FirstOrDefault();


            //Buscar detalles de los medicmentos a reactivar
            var reactivar_detalles = (from rc in db.receta_detalle_crn
                                      join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                                      from receIn in receExp.DefaultIfEmpty()
                                      join inventario in db.InventarioFarmacia on rc.codigo equals inventario.Clave into inventarioExp
                                      from inventarioIn in inventarioExp.DefaultIfEmpty()
                                      join medicos in db.MEDICOS on receIn.medico_crea equals medicos.Numero into medicosExp
                                      from medicosIn in medicosExp.DefaultIfEmpty()
                                      where rc.folio_rc == folio_rc
                                      select rc).ToList();

            //System.Diagnostics.Debug.WriteLine(reactivar_detalles);


            foreach (var item in reactivar_detalles)
            {
                //Insertar costo de recetas
                var costoMedicamento = (from a in db2.Sustancia
                                        where a.Clave == item.codigo
                                        select a).FirstOrDefault();


                var inventario = (from a in db2.InvFarm
                                        where a.Id_Sustancia == costoMedicamento.Id
                                        select a).FirstOrDefault();

                //No agregar si tiene el sobranteinv21 yes igual a 0 el inv disponible
                if (costoMedicamento.SobranteInv2022 != "2" && inventario.Inv_Act != 0)
                {
                    //System.Diagnostics.Debug.WriteLine(costoMedicamento);


                    float? costo = 0;

                    if (costoMedicamento != null)
                    {
                        costo = item.cantidad * costoMedicamento.cto_promedio;
                    }

                    //System.Diagnostics.Debug.WriteLine(costo);


                    db.Database.ExecuteSqlCommand("INSERT INTO receta_detalle_crn (folio_rc, codigo, cantidad, dosis, indicaciones, costo) VALUES('" + ultimo_folio_rc.folio_rc + "', '" + item.codigo + "','" + item.cantidad + "','" + item.dosis + "','" + item.indicaciones + "','" + costo + "')");

                }

            }

            //Contar si se agregaron recetas que no tienen sobranteinv21
            var ultimo_folio_rc2 = (from rc in db.receta_detalle_crn
                                    where rc.folio_rc == ultimo_folio_rc.folio_rc
                                    select rc).Count();

            if(ultimo_folio_rc2 == 0)
            {
                db.Database.ExecuteSqlCommand("DELETE FROM receta_exp_crn WHERE folio_rc = " + ultimo_folio_rc.folio_rc + "");
                TempData["message_error"] = "No hay medicamentos disponibles en el resurtimiento activado";
            }
            else
            {
                TempData["message_success"] = "Receta reactivada con éxito";
            }

            
            return new JsonResult { Data = reactivar, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult EditarResurtimiento(int folio_rc)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            var reactivar_detalles = (from rc in db.receta_detalle_crn
                                      join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                                      from receIn in receExp.DefaultIfEmpty()
                                      //join inventario in db.InventarioFarmacia on rc.codigo equals inventario.Clave into inventarioExp
                                      //from inventarioIn in inventarioExp.DefaultIfEmpty()
                                      join medicos in db.MEDICOS on receIn.medico_crea equals medicos.Numero into medicosExp
                                      from medicosIn in medicosExp.DefaultIfEmpty()
                                      where rc.folio_rc == folio_rc
                                      select new
                                      {
                                          folio_rc = rc.folio_rc,
                                          codigo = rc.codigo,
                                          cantidad = rc.cantidad,
                                          indicaciones = rc.indicaciones,
                                          dosis = rc.dosis,
                                          costo = rc.costo,
                                      }).ToList();

            //System.Diagnostics.Debug.WriteLine(reactivar_detalles);
            var results = new List<ResurtimientoList>();

            foreach (var item in reactivar_detalles)
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fecha_correcta = DateTime.Parse(fecha);
                //guardar en una temporal la info de la fecha de esta receta
                
                receta_detalle_crn_edit recetaEdit = new receta_detalle_crn_edit();
                recetaEdit.folio_rc = item.folio_rc;
                recetaEdit.codigo = item.codigo;
                recetaEdit.cantidad = item.cantidad;
                recetaEdit.dosis = item.dosis;
                recetaEdit.indicaciones = item.indicaciones;
                recetaEdit.costo = item.costo;
                recetaEdit.usuario_realiza = User.Identity.GetUserName();
                recetaEdit.fecha = fecha_correcta;
                db.receta_detalle_crn_edit.Add(recetaEdit);
                db.SaveChanges();


                //var fecha_crea = DateTime.Now.ToString("yyyy-MM-dd");
                //var fecha_crea_correcta = DateTime.Parse(fecha_crea);

                //System.Diagnostics.Debug.WriteLine(reactivar_detalles.folio_rc);
                //cambiar fecha de receta de resurtimiento para que aparezca toda

                var costoMedicamento = (from a in db2.Sustancia
                                        join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                                        from grupoIn in grupoX.DefaultIfEmpty()
                                        where a.Clave == item.codigo
                                        select new
                                        {
                                            medicamento = a.descripcion_21,
                                            grupo = grupoIn.descripcion,
                                        }).FirstOrDefault();

               if(costoMedicamento != null)
               {
                    var result = new ResurtimientoList
                    {
                        medicamento = costoMedicamento.medicamento,
                        grupo = costoMedicamento.grupo,
                        folio_rc = item.folio_rc,
                        codigo = item.codigo,
                        cantidad = item.cantidad,
                        dosis = item.dosis,
                        indicaciones = item.indicaciones,
                    };

                    results.Add(result);
               }
               else
               {
                    //medicamento no disponible
               }

                    


            }

            //Agregar un estatus de que esta siendo actualizada
            //1 = esta editandose
            db.Database.ExecuteSqlCommand("UPDATE receta_exp_crn SET editada = 1 WHERE folio_rc = '" + folio_rc + "'");

            //TempData["message_success"] = "Receta reactivada con éxito";
            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult BuscarEditarRes()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            var nombreusuario = User.Identity.GetUserName();

            var reactivar_detalles = (from rc in db.receta_detalle_crn_edit
                                      join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                                      from receIn in receExp.DefaultIfEmpty()
                                      //join inventario in db.InventarioFarmacia on rc.codigo equals inventario.Clave into inventarioExp
                                      //from inventarioIn in inventarioExp.DefaultIfEmpty()
                                      //join medicos in db.MEDICOS on receIn.medico_crea equals medicos.Numero into medicosExp
                                      //from medicosIn in medicosExp.DefaultIfEmpty()
                                      where rc.usuario_realiza == nombreusuario
                                      //where rc.fecha == fechaDT
                                      where receIn.editada == 1
                                      select new
                                      {
                                          folio_rc = rc.folio_rc,
                                          codigo = rc.codigo,
                                          cantidad = rc.cantidad,
                                          indicaciones = rc.indicaciones,
                                          dosis = rc.dosis,
                                          costo = rc.costo,
                                      }).ToList();

            var results = new List<ResurtimientoList>();

            foreach (var item in reactivar_detalles)
            {
                var costoMedicamento = (from a in db2.Sustancia
                                        join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                                        from grupoIn in grupoX.DefaultIfEmpty()
                                        where a.Clave == item.codigo
                                        select new
                                        {
                                            medicamento = a.descripcion_21,
                                            grupo = grupoIn.descripcion,
                                        }).FirstOrDefault();

                if (costoMedicamento != null)
                {
                    var result = new ResurtimientoList
                    {
                        medicamento = costoMedicamento.medicamento,
                        grupo = costoMedicamento.grupo,
                        folio_rc = item.folio_rc,
                        codigo = item.codigo,
                        cantidad = item.cantidad,
                        dosis = item.dosis,
                        indicaciones = item.indicaciones,
                    };

                    results.Add(result);
                }
                else
                {
                    //medicamento no disponible
                }
            }

            var recetaExpDts = (from rc in db.receta_detalle_crn_edit
                                  join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                                  from receIn in receExp.DefaultIfEmpty()
                                      //join inventario in db.InventarioFarmacia on rc.codigo equals inventario.Clave into inventarioExp
                                      //from inventarioIn in inventarioExp.DefaultIfEmpty()
                                  join medicos in db.MEDICOS on receIn.medico_crea equals medicos.Numero into medicosExp
                                  from medicosIn in medicosExp.DefaultIfEmpty()
                                  where rc.usuario_realiza == nombreusuario
                                  //where rc.fecha == fechaDT
                                  where receIn.editada == 1
                                  select new
                                  {
                                      folio_rc = rc.folio_rc,
                                      codigo = rc.codigo,
                                      cantidad = rc.cantidad,
                                      indicaciones = rc.indicaciones,
                                      dosis = rc.dosis,
                                      costo = rc.costo,
                                      fecha = receIn.fecha_crea,
                                      medico = medicosIn.Titulo + " " + medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  }).FirstOrDefault();

            var rst = new Object();

            if (recetaExpDts != null)
            {
                rst = new
                {
                    folio_rc = recetaExpDts.folio_rc,
                    medico = recetaExpDts.medico,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", recetaExpDts.fecha),
                };
            }


            var resultdata = new { Result1 = results, Result12 = rst };

            //System.Diagnostics.Debug.WriteLine(reactivar_detalles);

            //TempData["message_success"] = "Receta reactivada con éxito";
            return new JsonResult { Data = resultdata, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult BuscarEditarResFolio(int folio_rc)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            var nombreusuario = User.Identity.GetUserName();

            var reactivar_detalles = (from rc in db.receta_detalle_crn_edit
                                      join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                                      from receIn in receExp.DefaultIfEmpty()
                                      where rc.usuario_realiza == nombreusuario
                                      where rc.folio_rc == folio_rc
                                      where receIn.editada == 1
                                      select new
                                      {
                                          folio_rc = rc.folio_rc,
                                          codigo = rc.codigo,
                                          cantidad = rc.cantidad,
                                          indicaciones = rc.indicaciones,
                                          dosis = rc.dosis,
                                          costo = rc.costo,
                                      }).ToList();

            var results = new List<ResurtimientoList>();

            foreach (var item in reactivar_detalles)
            {
                var costoMedicamento = (from a in db2.Sustancia
                                        join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                                        from grupoIn in grupoX.DefaultIfEmpty()
                                        where a.Clave == item.codigo
                                        select new
                                        {
                                            medicamento = a.descripcion_21,
                                            grupo = grupoIn.descripcion,
                                        }).FirstOrDefault();

                if (costoMedicamento != null)
                {
                    var result = new ResurtimientoList
                    {
                        medicamento = costoMedicamento.medicamento,
                        grupo = costoMedicamento.grupo,
                        folio_rc = item.folio_rc,
                        codigo = item.codigo,
                        cantidad = item.cantidad,
                        dosis = item.dosis,
                        indicaciones = item.indicaciones,
                    };

                    results.Add(result);
                }
                else
                {
                    //medicamento no disponible
                }
            }

            var recetaExpDts = (from rc in db.receta_detalle_crn_edit
                                join recetaExp in db.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into receExp
                                from receIn in receExp.DefaultIfEmpty()
                                join medicos in db.MEDICOS on receIn.medico_crea equals medicos.Numero into medicosExp
                                from medicosIn in medicosExp.DefaultIfEmpty()
                                where rc.usuario_realiza == nombreusuario
                                where rc.folio_rc == folio_rc
                                where receIn.editada == 1
                                select new
                                {
                                    folio_rc = rc.folio_rc,
                                    codigo = rc.codigo,
                                    cantidad = rc.cantidad,
                                    indicaciones = rc.indicaciones,
                                    dosis = rc.dosis,
                                    costo = rc.costo,
                                    fecha = receIn.fecha_crea,
                                    medico = medicosIn.Titulo + " " + medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                }).FirstOrDefault();

            var rst = new Object();

            if (recetaExpDts != null)
            {
                rst = new
                {
                    folio_rc = recetaExpDts.folio_rc,
                    medico = recetaExpDts.medico,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", recetaExpDts.fecha),
                };
            }


            var resultdata = new { Result1 = results, Result12 = rst };

            //System.Diagnostics.Debug.WriteLine(reactivar_detalles);

            //TempData["message_success"] = "Receta reactivada con éxito";
            return new JsonResult { Data = resultdata, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public ActionResult GuardarReceta(Receta_Exp model, Receta_Detalle model2, receta_exp_crn model3, receta_detalle_crn model4, string unidad_medica)
        {
            //System.Diagnostics.Debug.WriteLine(unidad_medica);

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var fecha_crea = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_crea_correcta = DateTime.Parse(fecha_crea);

            //var dir_ip = GetLocalIPAddress();
            var dir_ip = Request.UserHostAddress;

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //Para las recetas crónicas
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var receta_hoy = (from a in db.Receta_Exp
                              where a.Expediente == model.Expediente
                              where a.Medico == model.Medico
                              where a.Fecha == fecha_correcta
                              where a.unidad_medica != 3
                              where a.Hora_Rcta == null
                              select a).FirstOrDefault();

            var datedate = string.Format("{0:yyyy-MM-ddT00:00:00.000}", model3.fec_ini_sintomas);

            //System.Diagnostics.Debug.WriteLine(datedate);

            //Buscar en recetas de resurtimiento que no sea una receta terminada
            var folio_rc = (from a in db2.receta_exp_crn
                            where a.expediente == model.Expediente
                            where a.medico_crea == model.Medico
                            //where a.folio_rc == model3.folio_rc
                            where a.terminada == 0
                            where a.fecha_crea == fecha_crea_correcta
                            select a).FirstOrDefault();

            if (model2.Cantidad != 0)
            {



                if (model3.meses_surtir != 0)
                {
                    //Si no hay ninguna hoy y no ha terminado la receta de resurtimiento
                    if (folio_rc == null)
                    {
                        db2.Database.ExecuteSqlCommand("INSERT INTO receta_exp_crn (medico_crea, medico_ref, fecha_crea, estado, platica_edu, lab_relev, trat_actual, fec_ini_sintomas, evento_adverso, meses_surtir, expediente, policl, terminada) VALUES('" + model.Medico + "', '00','" + fecha_crea + "',0,0,'','','','','" + model3.meses_surtir + "', '" + model.Expediente + "',0,0)");
                        //db2.Database.ExecuteSqlCommand("INSERT INTO receta_exp_crn (medico_crea, medico_ref, fecha_crea, estado, platica_edu, lab_relev, trat_actual, evento_adverso, meses_surtir, expediente, policl, terminada) VALUES('" + model.Medico + "', '00','" + fecha_crea + "',0,0,'','','','" + model3.meses_surtir + "', '" + model.Expediente + "',0,0)");


                    }


                    //Traer el folio que se genero
                    var folio_nuevo = (from a in db2.receta_exp_crn
                                       where a.expediente == model.Expediente
                                       //where a.folio_rc == model3.folio_rc
                                       where a.terminada == 0
                                       where a.fecha_crea == fecha_crea_correcta
                                       select a).FirstOrDefault();


                    //Buscar la receta de resurtimiento que se acaba de hacer si  no se ha agregado ese medicamento
                    var medicamento = (from rc in db2.receta_detalle_crn
                                       join recetaExp in db2.receta_exp_crn on rc.folio_rc equals recetaExp.folio_rc into reExp
                                       from receIn in reExp.DefaultIfEmpty()
                                       where rc.codigo == model4.codigo
                                       where receIn.folio_rc == folio_nuevo.folio_rc
                                       where receIn.expediente == model.Expediente
                                       where receIn.fecha_crea == fecha_crea_correcta
                                       select rc).FirstOrDefault();


                    //System.Diagnostics.Debug.WriteLine(receta_rc_detalles_hoy);
                    //Tomar el costo del medicamento
                    var costoMedicamento = (from a in db2.Sustancia
                                            where a.Clave == model2.Codigo
                                            select a).FirstOrDefault();
                    float? costo = 0;

                    if (costoMedicamento != null)
                    {
                        costo = model2.Cantidad * costoMedicamento.cto_promedio;
                    }

                    //Si no se ha agregado ese medicamento agregar
                    if (medicamento == null)
                    {
                        //System.Diagnostics.Debug.WriteLine(model2.Codigo);
                        /*if (model3.meses_surtir != 0)
                        {*/

                        db2.Database.ExecuteSqlCommand("INSERT INTO receta_detalle_crn (folio_rc, codigo, cantidad, dosis, indicaciones, costo) VALUES('" + folio_nuevo.folio_rc + "', '" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "','" + costo + "')");
                        //db2.Database.ExecuteSqlCommand("INSERT INTO receta_detalle_crn (folio_rc, codigo, cantidad, dosis, indicaciones) VALUES('" + folio_nuevo.folio_rc + "', '" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "')");
                        TempData["message_success"] = "Medicamento agregado con éxito";
                        /*}*/
                    }
                    else
                    {
                        //SI ya esta agregado, actualizar ese medicamento
                        db2.Database.ExecuteSqlCommand("UPDATE receta_exp_crn SET meses_surtir = '" + model3.meses_surtir + "' WHERE folio_rc = '" + folio_rc.folio_rc + "'");

                        //db2.Database.ExecuteSqlCommand("UPDATE receta_detalle_crn SET codigo = '" + model2.Codigo + "', cantidad = '" + model2.Cantidad + "', dosis = '" + model2.Dosis + "', indicaciones = '" + model2.Indicaciones + "' , costo = '" + costo + "' WHERE folio_rc = '" + folio_rc.folio_rc + "' AND codigo = '" + medicamento.codigo + "'");
                        db2.Database.ExecuteSqlCommand("UPDATE receta_detalle_crn SET codigo = '" + model2.Codigo + "', cantidad = '" + model2.Cantidad + "', dosis = '" + model2.Dosis + "', indicaciones = '" + model2.Indicaciones + "' WHERE folio_rc = '" + folio_rc.folio_rc + "' AND codigo = '" + medicamento.codigo + "'");

                        TempData["message_success"] = "Medicamento editado con éxito";

                    }

                }
                else
                {
                    //Receta normales
                    if (receta_hoy == null)
                    {

                        db.Database.ExecuteSqlCommand("INSERT INTO Receta_Exp (Expediente, Medico, Estado, Dir_Ip, Fecha, unidad_medica) VALUES('" + model.Expediente + "','" + model.Medico + "','" + 9 + "','" + dir_ip + "','" + fecha + "', '" + model.unidad_medica + "')");

                        //db2.Database.ExecuteSqlCommand("INSERT INTO expediente (Expediente, Medico, Estado, Dir_Ip, Fecha) VALUES('" + model.Expediente + "','" + model.Medico + "','" + 9 + "','" + dir_ip + "','" + fecha + "')");

                    }



                    //Tomar el Folio_Rcta de la ultima receta con el mismo expediente y medico
                    var ultima_receta_folio = db.Receta_Exp.Where(v => v.Medico == model.Medico).Where(v => v.Expediente == model.Expediente).Where(v => v.Estado == "9").Where(v => v.Hora_Rcta == null).Where(v => v.Fecha == fecha_correcta).OrderByDescending(v => v.Fecha)
                    .Select(ultima_folio => new
                    {
                        Folio_Rcta = ultima_folio.Folio_Rcta,
                    }).FirstOrDefault();

                    if (ultima_receta_folio != null)
                    {
                        //Revisar si ese medicamento ya existe en la receta
                        var receta_detalles_hoy = (from a in db.Receta_Detalle
                                                   where a.Codigo == model2.Codigo
                                                   where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                                   select a).FirstOrDefault();

                        var receta_detalles_hoy_count = (from a in db.Receta_Detalle
                                                             //where a.Codigo == model2.Codigo
                                                         where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                                         select a).Count();

                        //Tomar el costo del medicamento
                        var costoMedicamento = (from a in db2.Sustancia
                                                where a.Clave == model2.Codigo
                                                select a).FirstOrDefault();

                        float? costo = 0;

                        if (costoMedicamento != null)
                        {
                            costo = model2.Cantidad * costoMedicamento.cto_promedio;
                        }


                        if (receta_detalles_hoy == null)
                        {
                            if (receta_detalles_hoy_count < 3)
                            {
                                /*if (model3.meses_surtir == 0)
                                {*/


                                db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones, costo) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "', '" + costo + "')");
                                //db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "')");


                                //Agregar lo de la receta en el expediente del paciente, con ese doctor, en esa fecha

                                //Buscar primero el medicamento
                                /*
                                var medicamento_receta = (from a in db2.InventarioFarmacia
                                                          where a.Clave == model2.Codigo
                                                          select a).FirstOrDefault();
                                */

                                var medicamento_receta = (from a in db2.Sustancia
                                                          where a.Clave == model2.Codigo
                                                          select a).FirstOrDefault();

                                //Buscar cantidad y dosis del medicamento
                                var rece_det_exp = (from a in db.Receta_Detalle
                                                    join recetaExp in db.Receta_Exp on a.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                                    from receExp in reExp.DefaultIfEmpty()
                                                    where a.Codigo == model2.Codigo
                                                    where receExp.Medico == model.Medico
                                                    where receExp.Expediente == model.Expediente
                                                    where receExp.Fecha == fecha_correcta
                                                    select a).FirstOrDefault();


                                //Tomar el valor de a_exp de lo que se lleno del soap
                                var a_exp = (from a in db2.expediente
                                             where a.numemp == model.Expediente
                                             where a.medico == model.Medico
                                             where a.fecha == fecha_correcta
                                             select a).FirstOrDefault();

                                //Si es el primero medicamento que se le va agregar al expediente
                                if (a_exp != null)
                                {

                                    if (a_exp.a_exp == null)
                                    {
                                        db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '" + rece_det_exp.Cantidad + ", " + medicamento_receta.descripcion_21 + ", " + rece_det_exp.Dosis + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");


                                    }
                                    else
                                    {
                                        //Ponme lo que ya se puso en el a_exp
                                        db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '" + a_exp.a_exp + " - " + rece_det_exp.Cantidad + ", " + medicamento_receta.descripcion_21 + ", " + rece_det_exp.Dosis + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");

                                    }

                                }

                                //db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '"+ a_exp.a_exp + ", " + rece_det_exp.Cantidad + ", " + medicamento_receta.Descripcion_Sal + ", " + medicamento_receta.Presentacion + ", " + rece_det_exp.Dosis  + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");


                                TempData["message_success"] = "Medicamento agregado con éxito";
                                /*}
                                else
                                {
                                    //System.Diagnostics.Debug.WriteLine(model3.folio_rc);
                                    if (model3.folio_rc != 0)
                                    {
                                        db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "')");
                                        TempData["message_success"] = "Medicamento agregado con éxito";
                                    }
                                }*/
                            }
                            else
                            {
                                TempData["message_error"] = "Ya agregaste 3 medicamentos en esta receta";
                            }
                        }
                        else
                        {
                            //System.Diagnostics.Debug.WriteLine(model2.Cantidad);
                            db.Database.ExecuteSqlCommand("UPDATE Receta_Detalle SET Codigo = '" + model2.Codigo + "', Cantidad = '" + model2.Cantidad + "', Dosis = '" + model2.Dosis + "', Indicaciones = '" + model2.Indicaciones + "' , costo = '" + costo + "' WHERE Folio_Rcta = '" + receta_detalles_hoy.Folio_Rcta + "' AND Codigo = '" + receta_detalles_hoy.Codigo + "'");
                            //db.Database.ExecuteSqlCommand("UPDATE Receta_Detalle SET Codigo = '" + model2.Codigo + "', Cantidad = '" + model2.Cantidad + "', Dosis = '" + model2.Dosis + "', Indicaciones = '" + model2.Indicaciones + "' WHERE Folio_Rcta = '" + receta_detalles_hoy.Folio_Rcta + "' AND Codigo = '" + receta_detalles_hoy.Codigo + "'");
                            TempData["message_success"] = "Medicamento editado con éxito";
                        }
                    }


                }


                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                TempData["message_error"] = "La cantidad ha surtir debe ser mayor a 0";
                return Redirect(Request.UrlReferrer.ToString());
            }
            //System.Diagnostics.Debug.WriteLine(receta_detalles_hoy);

            //return RedirectToAction("Recetas/" + model.numemp, "Manage");

        }


        [HttpPost]
        public ActionResult GuardarRecetaUER(Receta_Exp model, Receta_Detalle model2, receta_exp_crn model3, receta_detalle_crn model4)
        {
            //System.Diagnostics.Debug.WriteLine(model3.meses_surtir);

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var fecha_crea = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_crea_correcta = DateTime.Parse(fecha_crea);

            //var dir_ip = GetLocalIPAddress();
            var dir_ip = Request.UserHostAddress;

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //Para las recetas crónicas
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var receta_hoy = (from a in db.Receta_Exp
                              where a.Expediente == model.Expediente
                              where a.Medico == model.Medico
                              where a.Fecha == fecha_correcta
                              where a.Hora_Rcta == null
                              select a).FirstOrDefault();

            var datedate = string.Format("{0:yyyy-MM-ddT00:00:00.000}", model3.fec_ini_sintomas);

            //System.Diagnostics.Debug.WriteLine(datedate);

            //Buscar en recetas de resurtimiento que no sea una receta terminada
            var folio_rc = (from a in db2.receta_exp_crn
                            where a.expediente == model.Expediente
                            where a.medico_crea == model.Medico
                            //where a.folio_rc == model3.folio_rc
                            where a.terminada == 0
                            where a.fecha_crea == fecha_crea_correcta
                            select a).FirstOrDefault();

            if (model2.Cantidad != 0)
            {

                //Receta normales
                if (receta_hoy == null)
                {

                    db.Database.ExecuteSqlCommand("INSERT INTO Receta_Exp (Expediente, Medico, Estado, Dir_Ip, Fecha) VALUES('" + model.Expediente + "','" + model.Medico + "','" + 6 + "','" + dir_ip + "','" + fecha + "')");

                    //db2.Database.ExecuteSqlCommand("INSERT INTO expediente (Expediente, Medico, Estado, Dir_Ip, Fecha) VALUES('" + model.Expediente + "','" + model.Medico + "','" + 9 + "','" + dir_ip + "','" + fecha + "')");

                }



                //Tomar el Folio_Rcta de la ultima receta con el mismo expediente y medico
                var ultima_receta_folio = db.Receta_Exp.Where(v => v.Medico == model.Medico).Where(v => v.Expediente == model.Expediente).Where(v => v.Estado == "6").Where(v => v.Hora_Rcta == null).Where(v => v.Fecha == fecha_correcta).OrderByDescending(v => v.Fecha)
                .Select(ultima_folio => new
                {
                    Folio_Rcta = ultima_folio.Folio_Rcta,
                }).FirstOrDefault();

                if (ultima_receta_folio != null)
                {
                    //Revisar si ese medicamento ya existe en la receta
                    var receta_detalles_hoy = (from a in db.Receta_Detalle
                                               where a.Codigo == model2.Codigo
                                               where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                               select a).FirstOrDefault();

                    var receta_detalles_hoy_count = (from a in db.Receta_Detalle
                                                         //where a.Codigo == model2.Codigo
                                                     where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                                     select a).Count();

                    //Tomar el costo del medicamento
                    var costoMedicamento = (from a in db2.Sustancia
                                            where a.Clave == model2.Codigo
                                            select a).FirstOrDefault();

                    float? costo = 0;

                    if (costoMedicamento != null)
                    {
                        costo = model2.Cantidad * costoMedicamento.cto_promedio;
                    }


                    if (receta_detalles_hoy == null)
                    {
                        if (receta_detalles_hoy_count < 3)
                        {
                            /*if (model3.meses_surtir == 0)
                            {*/


                            db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones, costo) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "', '" + costo + "')");
                            //db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "')");


                            //Agregar lo de la receta en el expediente del paciente, con ese doctor, en esa fecha

                            //Buscar primero el medicamento
                            /*var medicamento_receta = (from a in db2.InventarioFarmacia
                                                      where a.Clave == model2.Codigo
                                                      select a).FirstOrDefault();*/

                            var medicamento_receta = (from a in db2.Sustancia
                                                      where a.Clave == model2.Codigo
                                                      select a).FirstOrDefault();

                            //Buscar cantidad y dosis del medicamento
                            var rece_det_exp = (from a in db.Receta_Detalle
                                                join recetaExp in db.Receta_Exp on a.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                                from receExp in reExp.DefaultIfEmpty()
                                                where a.Codigo == model2.Codigo
                                                where receExp.Medico == model.Medico
                                                where receExp.Expediente == model.Expediente
                                                where receExp.Fecha == fecha_correcta
                                                select a).FirstOrDefault();


                            //Tomar el valor de a_exp de lo que se lleno del soap
                            var a_exp = (from a in db2.expediente
                                         where a.numemp == model.Expediente
                                         where a.medico == model.Medico
                                         where a.fecha == fecha_correcta
                                         select a).FirstOrDefault();

                            //Si es el primero medicamento que se le va agregar al expediente
                            if (a_exp != null)
                            {

                                if (a_exp.a_exp == null)
                                {
                                    db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '" + rece_det_exp.Cantidad + ", " + medicamento_receta.descripcion_21 + ", " + rece_det_exp.Dosis + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");


                                }
                                else
                                {
                                    //Ponme lo que ya se puso en el a_exp
                                    db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '" + a_exp.a_exp + " - " + rece_det_exp.Cantidad + ", " + medicamento_receta.descripcion_21 + ", " + rece_det_exp.Dosis + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");

                                }

                            }

                            //db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '"+ a_exp.a_exp + ", " + rece_det_exp.Cantidad + ", " + medicamento_receta.Descripcion_Sal + ", " + medicamento_receta.Presentacion + ", " + rece_det_exp.Dosis  + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");


                            TempData["message_success"] = "Medicamento agregado con éxito";
                            /*}
                            else
                            {
                                //System.Diagnostics.Debug.WriteLine(model3.folio_rc);
                                if (model3.folio_rc != 0)
                                {
                                    db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "')");
                                    TempData["message_success"] = "Medicamento agregado con éxito";
                                }
                            }*/
                        }
                        else
                        {
                            TempData["message_error"] = "Ya agregaste 3 medicamentos en esta receta";
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(model2.Cantidad);
                        db.Database.ExecuteSqlCommand("UPDATE Receta_Detalle SET Codigo = '" + model2.Codigo + "', Cantidad = '" + model2.Cantidad + "', Dosis = '" + model2.Dosis + "', Indicaciones = '" + model2.Indicaciones + "' , costo = '" + costo + "' WHERE Folio_Rcta = '" + receta_detalles_hoy.Folio_Rcta + "' AND Codigo = '" + receta_detalles_hoy.Codigo + "'");
                        //db.Database.ExecuteSqlCommand("UPDATE Receta_Detalle SET Codigo = '" + model2.Codigo + "', Cantidad = '" + model2.Cantidad + "', Dosis = '" + model2.Dosis + "', Indicaciones = '" + model2.Indicaciones + "' WHERE Folio_Rcta = '" + receta_detalles_hoy.Folio_Rcta + "' AND Codigo = '" + receta_detalles_hoy.Codigo + "'");
                        TempData["message_success"] = "Medicamento editado con éxito";
                    }
                }


                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                TempData["message_error"] = "La cantidad ha surtir debe ser mayor a 0";
                return Redirect(Request.UrlReferrer.ToString());
            }
            //System.Diagnostics.Debug.WriteLine(receta_detalles_hoy);

            //return RedirectToAction("Recetas/" + model.numemp, "Manage");

        }


        [HttpPost]
        public ActionResult GuardarRecetaCU(Receta_Exp model, Receta_Detalle model2, receta_exp_crn model3, receta_detalle_crn model4, string unidad_medica)
        {
            //System.Diagnostics.Debug.WriteLine(unidad_medica);

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var fecha_crea = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_crea_correcta = DateTime.Parse(fecha_crea);

            //var dir_ip = GetLocalIPAddress();
            var dir_ip = Request.UserHostAddress;

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //Para las recetas crónicas
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var receta_hoy = (from a in db.Receta_Exp
                              where a.Expediente == model.Expediente
                              where a.Medico == model.Medico
                              where a.Fecha == fecha_correcta
                              where a.unidad_medica == 3
                              where a.Hora_Rcta == null
                              select a).FirstOrDefault();

            var datedate = string.Format("{0:yyyy-MM-ddT00:00:00.000}", model3.fec_ini_sintomas);

            //System.Diagnostics.Debug.WriteLine(datedate);


            if (model2.Cantidad != 0)
            {

                //Receta normales
                if (receta_hoy == null)
                {

                    db.Database.ExecuteSqlCommand("INSERT INTO Receta_Exp (Expediente, Medico, Estado, Dir_Ip, Fecha, unidad_medica) VALUES('" + model.Expediente + "','" + model.Medico + "','" + 6 + "','" + dir_ip + "','" + fecha + "', '" + model.unidad_medica + "')");

                }



                //Tomar el Folio_Rcta de la ultima receta con el mismo expediente y medico
                var ultima_receta_folio = db.Receta_Exp.Where(v => v.Medico == model.Medico).Where(v => v.Expediente == model.Expediente).Where(v => v.Estado == "6").Where(v => v.Hora_Rcta == null).Where(v => v.Fecha == fecha_correcta).OrderByDescending(v => v.Fecha)
                .Select(ultima_folio => new
                {
                    Folio_Rcta = ultima_folio.Folio_Rcta,
                }).FirstOrDefault();

                if (ultima_receta_folio != null)
                {
                    //Revisar si ese medicamento ya existe en la receta
                    var receta_detalles_hoy = (from a in db.Receta_Detalle
                                               where a.Codigo == model2.Codigo
                                               where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                               select a).FirstOrDefault();

                    var receta_detalles_hoy_count = (from a in db.Receta_Detalle
                                                         //where a.Codigo == model2.Codigo
                                                     where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                                     select a).Count();

                    //Tomar el costo del medicamento
                    var costoMedicamento = (from a in db2.Sustancia
                                            where a.Clave == model2.Codigo
                                            select a).FirstOrDefault();

                    float? costo = 0;

                    if (costoMedicamento != null)
                    {
                        costo = model2.Cantidad * costoMedicamento.cto_promedio;
                    }


                    if (receta_detalles_hoy == null)
                    {
                        if (receta_detalles_hoy_count < 3)
                        {

                            db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones, costo) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "', '" + costo + "')");
                            //db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "')");


                            //Agregar lo de la receta en el expediente del paciente, con ese doctor, en esa fecha

                            //Buscar primero el medicamento
                            /*var medicamento_receta = (from a in db2.InventarioFarmacia
                                                      where a.Clave == model2.Codigo
                                                      select a).FirstOrDefault();
                            */

                            var medicamento_receta = (from a in db2.Sustancia
                                                      where a.Clave == model2.Codigo
                                                      select a).FirstOrDefault();

                            //Buscar cantidad y dosis del medicamento
                            var rece_det_exp = (from a in db.Receta_Detalle
                                                join recetaExp in db.Receta_Exp on a.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                                from receExp in reExp.DefaultIfEmpty()
                                                where a.Codigo == model2.Codigo
                                                where receExp.Medico == model.Medico
                                                where receExp.Expediente == model.Expediente
                                                where receExp.Fecha == fecha_correcta
                                                select a).FirstOrDefault();


                            //Tomar el valor de a_exp de lo que se lleno del soap
                            var a_exp = (from a in db2.expediente
                                         where a.numemp == model.Expediente
                                         where a.medico == model.Medico
                                         where a.fecha == fecha_correcta
                                         select a).FirstOrDefault();

                            //Si es el primero medicamento que se le va agregar al expediente
                            if (a_exp != null)
                            {

                                if (a_exp.a_exp == null)
                                {
                                    db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '" + rece_det_exp.Cantidad + ", " + medicamento_receta.descripcion_21 + ", " + rece_det_exp.Dosis + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");


                                }
                                else
                                {
                                    //Ponme lo que ya se puso en el a_exp
                                    db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '" + a_exp.a_exp + " - " + rece_det_exp.Cantidad + ", " + medicamento_receta.descripcion_21 + ", " + rece_det_exp.Dosis + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");

                                }

                            }


                            TempData["message_success"] = "Medicamento agregado con éxito";
                        }
                        else
                        {
                            TempData["message_error"] = "Ya agregaste 3 medicamentos en esta receta";
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(model2.Cantidad);
                        db.Database.ExecuteSqlCommand("UPDATE Receta_Detalle SET Codigo = '" + model2.Codigo + "', Cantidad = '" + model2.Cantidad + "', Dosis = '" + model2.Dosis + "', Indicaciones = '" + model2.Indicaciones + "' , costo = '" + costo + "' WHERE Folio_Rcta = '" + receta_detalles_hoy.Folio_Rcta + "' AND Codigo = '" + receta_detalles_hoy.Codigo + "'");
                        TempData["message_success"] = "Medicamento editado con éxito";
                    }
                }


                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                TempData["message_error"] = "La cantidad ha surtir debe ser mayor a 0";
                return Redirect(Request.UrlReferrer.ToString());
            }


        }


        [HttpPost]
        public ActionResult GuardarRecetaUERSEMAC(Receta_Exp model, Receta_Detalle model2, receta_exp_crn model3, receta_detalle_crn model4, string unidad_medica)
        {
            //System.Diagnostics.Debug.WriteLine(unidad_medica);

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var fecha_crea = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_crea_correcta = DateTime.Parse(fecha_crea);

            //var dir_ip = GetLocalIPAddress();
            var dir_ip = Request.UserHostAddress;

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //Para las recetas crónicas
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var receta_hoy = (from a in db.Receta_Exp
                              where a.Expediente == model.Expediente
                              where a.Medico == model.Medico
                              where a.Fecha == fecha_correcta
                              where a.unidad_medica == 3
                              where a.Hora_Rcta == null
                              select a).FirstOrDefault();

            var datedate = string.Format("{0:yyyy-MM-ddT00:00:00.000}", model3.fec_ini_sintomas);

            //System.Diagnostics.Debug.WriteLine(datedate);


            if (model2.Cantidad != 0)
            {

                //Receta normales
                if (receta_hoy == null)
                {

                    db.Database.ExecuteSqlCommand("INSERT INTO Receta_Exp (Expediente, Medico, Estado, Dir_Ip, Fecha, unidad_medica) VALUES('" + model.Expediente + "','" + model.Medico + "','" + 6 + "','" + dir_ip + "','" + fecha + "', '" + model.unidad_medica + "')");

                }



                //Tomar el Folio_Rcta de la ultima receta con el mismo expediente y medico
                var ultima_receta_folio = db.Receta_Exp.Where(v => v.Medico == model.Medico).Where(v => v.Expediente == model.Expediente).Where(v => v.Estado == "6").Where(v => v.Hora_Rcta == null).Where(v => v.Fecha == fecha_correcta).OrderByDescending(v => v.Fecha)
                .Select(ultima_folio => new
                {
                    Folio_Rcta = ultima_folio.Folio_Rcta,
                }).FirstOrDefault();

                if (ultima_receta_folio != null)
                {
                    //Revisar si ese medicamento ya existe en la receta
                    var receta_detalles_hoy = (from a in db.Receta_Detalle
                                               where a.Codigo == model2.Codigo
                                               where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                               select a).FirstOrDefault();

                    var receta_detalles_hoy_count = (from a in db.Receta_Detalle
                                                         //where a.Codigo == model2.Codigo
                                                     where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                                     select a).Count();

                    //Tomar el costo del medicamento
                    var costoMedicamento = (from a in db2.Sustancia
                                            where a.Clave == model2.Codigo
                                            select a).FirstOrDefault();

                    float? costo = 0;

                    if (costoMedicamento != null)
                    {
                        costo = model2.Cantidad * costoMedicamento.cto_promedio;
                    }


                    if (receta_detalles_hoy == null)
                    {
                        if (receta_detalles_hoy_count < 3)
                        {

                            db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones, costo) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "', '" + costo + "')");
                            //db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "')");


                            //Agregar lo de la receta en el expediente del paciente, con ese doctor, en esa fecha

                            //Buscar primero el medicamento
                            /*var medicamento_receta = (from a in db2.InventarioFarmacia
                                                      where a.Clave == model2.Codigo
                                                      select a).FirstOrDefault();
                            */

                            var medicamento_receta = (from a in db2.Sustancia
                                                      where a.Clave == model2.Codigo
                                                      select a).FirstOrDefault();

                            //Buscar cantidad y dosis del medicamento
                            var rece_det_exp = (from a in db.Receta_Detalle
                                                join recetaExp in db.Receta_Exp on a.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                                from receExp in reExp.DefaultIfEmpty()
                                                where a.Codigo == model2.Codigo
                                                where receExp.Medico == model.Medico
                                                where receExp.Expediente == model.Expediente
                                                where receExp.Fecha == fecha_correcta
                                                select a).FirstOrDefault();


                            //Tomar el valor de a_exp de lo que se lleno del soap
                            var a_exp = (from a in db2.expediente
                                         where a.numemp == model.Expediente
                                         where a.medico == model.Medico
                                         where a.fecha == fecha_correcta
                                         select a).FirstOrDefault();

                            //Si es el primero medicamento que se le va agregar al expediente
                            if (a_exp != null)
                            {

                                if (a_exp.a_exp == null)
                                {
                                    db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '" + rece_det_exp.Cantidad + ", " + medicamento_receta.descripcion_21 + ", " + rece_det_exp.Dosis + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");


                                }
                                else
                                {
                                    //Ponme lo que ya se puso en el a_exp
                                    db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '" + a_exp.a_exp + " - " + rece_det_exp.Cantidad + ", " + medicamento_receta.descripcion_21 + ", " + rece_det_exp.Dosis + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");

                                }

                            }


                            TempData["message_success"] = "Medicamento agregado con éxito";
                        }
                        else
                        {
                            TempData["message_error"] = "Ya agregaste 3 medicamentos en esta receta";
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(model2.Cantidad);
                        db.Database.ExecuteSqlCommand("UPDATE Receta_Detalle SET Codigo = '" + model2.Codigo + "', Cantidad = '" + model2.Cantidad + "', Dosis = '" + model2.Dosis + "', Indicaciones = '" + model2.Indicaciones + "' , costo = '" + costo + "' WHERE Folio_Rcta = '" + receta_detalles_hoy.Folio_Rcta + "' AND Codigo = '" + receta_detalles_hoy.Codigo + "'");
                        TempData["message_success"] = "Medicamento editado con éxito";
                    }
                }


                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                TempData["message_error"] = "La cantidad ha surtir debe ser mayor a 0";
                return Redirect(Request.UrlReferrer.ToString());
            }


        }


        [HttpPost]
        public ActionResult GuardarRecetaUERMederos(Receta_Exp model, Receta_Detalle model2, receta_exp_crn model3, receta_detalle_crn model4, string unidad_medica)
        {
            //System.Diagnostics.Debug.WriteLine(unidad_medica);

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var fecha_crea = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_crea_correcta = DateTime.Parse(fecha_crea);

            //var dir_ip = GetLocalIPAddress();
            var dir_ip = Request.UserHostAddress;

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //Para las recetas crónicas
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var receta_hoy = (from a in db.Receta_Exp
                              where a.Expediente == model.Expediente
                              where a.Medico == model.Medico
                              where a.Fecha == fecha_correcta
                              where a.unidad_medica == 3
                              where a.Hora_Rcta == null
                              select a).FirstOrDefault();

            var datedate = string.Format("{0:yyyy-MM-ddT00:00:00.000}", model3.fec_ini_sintomas);

            //System.Diagnostics.Debug.WriteLine(datedate);


            if (model2.Cantidad != 0)
            {

                //Receta normales
                if (receta_hoy == null)
                {

                    db.Database.ExecuteSqlCommand("INSERT INTO Receta_Exp (Expediente, Medico, Estado, Dir_Ip, Fecha, unidad_medica) VALUES('" + model.Expediente + "','" + model.Medico + "','" + 6 + "','" + dir_ip + "','" + fecha + "', '" + model.unidad_medica + "')");

                }



                //Tomar el Folio_Rcta de la ultima receta con el mismo expediente y medico
                var ultima_receta_folio = db.Receta_Exp.Where(v => v.Medico == model.Medico).Where(v => v.Expediente == model.Expediente).Where(v => v.Estado == "6").Where(v => v.Hora_Rcta == null).Where(v => v.Fecha == fecha_correcta).OrderByDescending(v => v.Fecha)
                .Select(ultima_folio => new
                {
                    Folio_Rcta = ultima_folio.Folio_Rcta,
                }).FirstOrDefault();

                if (ultima_receta_folio != null)
                {
                    //Revisar si ese medicamento ya existe en la receta
                    var receta_detalles_hoy = (from a in db.Receta_Detalle
                                               where a.Codigo == model2.Codigo
                                               where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                               select a).FirstOrDefault();

                    var receta_detalles_hoy_count = (from a in db.Receta_Detalle
                                                         //where a.Codigo == model2.Codigo
                                                     where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                                     select a).Count();

                    //Tomar el costo del medicamento
                    var costoMedicamento = (from a in db2.Sustancia
                                            where a.Clave == model2.Codigo
                                            select a).FirstOrDefault();

                    float? costo = 0;

                    if (costoMedicamento != null)
                    {
                        costo = model2.Cantidad * costoMedicamento.cto_promedio;
                    }


                    if (receta_detalles_hoy == null)
                    {
                        if (receta_detalles_hoy_count < 3)
                        {

                            db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones, costo) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "', '" + costo + "')");
                            //db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "')");


                            //Agregar lo de la receta en el expediente del paciente, con ese doctor, en esa fecha

                            //Buscar primero el medicamento
                            /*var medicamento_receta = (from a in db2.InventarioFarmacia
                                                      where a.Clave == model2.Codigo
                                                      select a).FirstOrDefault();*/

                            var medicamento_receta = (from a in db2.Sustancia
                                                      where a.Clave == model2.Codigo
                                                      select a).FirstOrDefault();

                            //Buscar cantidad y dosis del medicamento
                            var rece_det_exp = (from a in db.Receta_Detalle
                                                join recetaExp in db.Receta_Exp on a.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                                from receExp in reExp.DefaultIfEmpty()
                                                where a.Codigo == model2.Codigo
                                                where receExp.Medico == model.Medico
                                                where receExp.Expediente == model.Expediente
                                                where receExp.Fecha == fecha_correcta
                                                select a).FirstOrDefault();


                            //Tomar el valor de a_exp de lo que se lleno del soap
                            var a_exp = (from a in db2.expediente
                                         where a.numemp == model.Expediente
                                         where a.medico == model.Medico
                                         where a.fecha == fecha_correcta
                                         select a).FirstOrDefault();

                            //Si es el primero medicamento que se le va agregar al expediente
                            if (a_exp != null)
                            {

                                if (a_exp.a_exp == null)
                                {
                                    db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '" + rece_det_exp.Cantidad + ", " + medicamento_receta.descripcion_21 + ", " + rece_det_exp.Dosis + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");


                                }
                                else
                                {
                                    //Ponme lo que ya se puso en el a_exp
                                    db2.Database.ExecuteSqlCommand("UPDATE expediente SET a_exp = '" + a_exp.a_exp + " - " + rece_det_exp.Cantidad + ", " + medicamento_receta.descripcion_21 + ", " + rece_det_exp.Dosis + "' WHERE numemp = '" + model.Expediente + "' AND medico = '" + model.Medico + "' AND fecha = '" + fecha + "'");

                                }

                            }


                            TempData["message_success"] = "Medicamento agregado con éxito";
                        }
                        else
                        {
                            TempData["message_error"] = "Ya agregaste 3 medicamentos en esta receta";
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(model2.Cantidad);
                        db.Database.ExecuteSqlCommand("UPDATE Receta_Detalle SET Codigo = '" + model2.Codigo + "', Cantidad = '" + model2.Cantidad + "', Dosis = '" + model2.Dosis + "', Indicaciones = '" + model2.Indicaciones + "' , costo = '" + costo + "' WHERE Folio_Rcta = '" + receta_detalles_hoy.Folio_Rcta + "' AND Codigo = '" + receta_detalles_hoy.Codigo + "'");
                        TempData["message_success"] = "Medicamento editado con éxito";
                    }
                }


                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                TempData["message_error"] = "La cantidad ha surtir debe ser mayor a 0";
                return Redirect(Request.UrlReferrer.ToString());
            }


        }


        public JsonResult GetLastRecetaCU(string search)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();

            var receta = (from r in db.Receta_Exp
                          where r.Medico == nombreusuario
                          where r.Expediente == search
                          where r.Fecha == fecha_correcta
                          where r.unidad_medica == 3
                          where r.TurnoFar == null
                          where r.Hora_Rcta == null
                          select new
                          {
                              Folio_Rcta = r.Folio_Rcta,
                              Estado = r.Estado,
                              Hora_Rcta = r.Hora_Rcta,
                              unidad_medica = r.unidad_medica,
                          })
                          .FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(receta);

            if (receta != null)
            {
                var receta_detalle = (from r in db.Receta_Detalle
                                      where r.Folio_Rcta == receta.Folio_Rcta
                                      select new
                                      {
                                          Folio_Rcta = r.Folio_Rcta,
                                          Codigo = r.Codigo,
                                          Cantidad = r.Cantidad,
                                          Dosis = r.Dosis,
                                          Indicaciones = r.Indicaciones,
                                          Estado = receta.Estado,
                                          Hora_Rcta = receta.Hora_Rcta,
                                          unidad_medica = receta.unidad_medica,
                                      })
                                     .ToList();

                return new JsonResult { Data = receta_detalle, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var receta_detalle = "";
                return new JsonResult { Data = receta_detalle, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



        }


        public JsonResult GetLastRecetaCUSEMAC(string search)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();

            var receta = (from r in db.Receta_Exp
                          where r.Medico == nombreusuario
                          where r.Expediente == search
                          where r.Fecha == fecha_correcta
                          where r.unidad_medica == 3
                          where r.TurnoFar == null
                          where r.Hora_Rcta == null
                          select new
                          {
                              Folio_Rcta = r.Folio_Rcta,
                              Estado = r.Estado,
                              Hora_Rcta = r.Hora_Rcta,
                              unidad_medica = r.unidad_medica,
                          })
                          .FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(receta);

            if (receta != null)
            {
                var receta_detalle = (from r in db.Receta_Detalle
                                      where r.Folio_Rcta == receta.Folio_Rcta
                                      select new
                                      {
                                          Folio_Rcta = r.Folio_Rcta,
                                          Codigo = r.Codigo,
                                          Cantidad = r.Cantidad,
                                          Dosis = r.Dosis,
                                          Indicaciones = r.Indicaciones,
                                          Estado = receta.Estado,
                                          Hora_Rcta = receta.Hora_Rcta,
                                          unidad_medica = receta.unidad_medica,
                                      })
                                     .ToList();

                return new JsonResult { Data = receta_detalle, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var receta_detalle = "";
                return new JsonResult { Data = receta_detalle, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



        }


        public JsonResult GetLastRecetaCUMederos(string search)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();

            var receta = (from r in db.Receta_Exp
                          where r.Medico == nombreusuario
                          where r.Expediente == search
                          where r.Fecha == fecha_correcta
                          where r.unidad_medica == 2
                          where r.TurnoFar == null
                          where r.Hora_Rcta == null
                          select new
                          {
                              Folio_Rcta = r.Folio_Rcta,
                              Estado = r.Estado,
                              Hora_Rcta = r.Hora_Rcta,
                              unidad_medica = r.unidad_medica,
                          })
                          .FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(receta);

            if (receta != null)
            {
                var receta_detalle = (from r in db.Receta_Detalle
                                      where r.Folio_Rcta == receta.Folio_Rcta
                                      select new
                                      {
                                          Folio_Rcta = r.Folio_Rcta,
                                          Codigo = r.Codigo,
                                          Cantidad = r.Cantidad,
                                          Dosis = r.Dosis,
                                          Indicaciones = r.Indicaciones,
                                          Estado = receta.Estado,
                                          Hora_Rcta = receta.Hora_Rcta,
                                          unidad_medica = receta.unidad_medica,
                                      })
                                     .ToList();

                return new JsonResult { Data = receta_detalle, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var receta_detalle = "";
                return new JsonResult { Data = receta_detalle, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



        }


        public class ResurtimientoList
        {
            public int recetaNorCount { get; set; }
            public int? folio_rc { get; set; }
            public string codigo { get; set; }
            public int? cantidad { get; set; }
            public string dosis { get; set; }
            public string indicaciones { get; set; }
            public int meses_surtir { get; set; }
            public string fec_ini_sintomas { get; set; }
            public string fecha_crea { get; set; }
            public int? terminada { get; set; }
            public string medicamento { get; set; }
            public string grupo { get; set; }
        }

        public JsonResult GetLastRecetaResur(string search)
        {
            //Para las recetas crónicas
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();

            var receta_normales_count = (from r in db2.Receta_Exp
                                         where r.Medico == nombreusuario
                                         where r.Expediente == search
                                         where r.Fecha == fecha_correcta
                                         //where r.Estado == "9"
                                         where r.TurnoFar == null
                                         where r.Hora_Rcta == null
                                         select r).Count();

            var receta = (from r in db.receta_exp_crn
                          where r.medico_crea == nombreusuario
                          where r.expediente == search
                          where r.fecha_crea == fecha_correcta
                          where r.terminada == 0 || r.terminada == null
                          select new
                          {
                              folio_rc = r.folio_rc,
                          })
                          .FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(receta);

            if (receta != null)
            {
                var receta_detalle = (from r in db.receta_detalle_crn
                                      where r.folio_rc == receta.folio_rc
                                      join recetaExp in db.receta_exp_crn on r.folio_rc equals recetaExp.folio_rc into reExp
                                      from receExp in reExp.DefaultIfEmpty()
                                      select new
                                      {
                                          recetaNorCount = receta_normales_count,
                                          folio_rc = r.folio_rc,
                                          codigo = r.codigo,
                                          cantidad = r.cantidad,
                                          dosis = r.dosis,
                                          indicaciones = r.indicaciones,
                                          meses_surtir = receExp.meses_surtir,
                                          fec_ini_sintomas = receExp.fec_ini_sintomas,
                                          fecha_crea = receExp.fecha_crea,
                                          terminada = receExp.terminada,
                                      })
                                     .ToList();

                var results = new List<ResurtimientoList>();

                foreach (var item in receta_detalle)
                {

                    var result = new ResurtimientoList
                    {
                        recetaNorCount = item.recetaNorCount,
                        folio_rc = item.folio_rc,
                        codigo = item.codigo,
                        cantidad = item.cantidad,
                        dosis = item.dosis,
                        indicaciones = item.indicaciones,
                        meses_surtir = item.meses_surtir,
                        fec_ini_sintomas = string.Format("{0:yyyy-MM-dd}", item.fec_ini_sintomas),
                        fecha_crea = string.Format("{0:yyyy-MM-dd}", item.fecha_crea),
                        terminada = item.terminada
                    };

                    results.Add(result);

                }

                //System.Diagnostics.Debug.WriteLine(receta_detalle);

                return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {

                var receta2 = (from r in db.receta_exp_crn
                              where r.medico_crea == nombreusuario
                              where r.expediente == search
                              where r.editada == 1
                               //where r.fecha_crea == fecha_correcta
                               //where r.terminada == 0 || r.terminada == null
                               select new
                              {
                                  folio_rc = r.folio_rc,
                              })
                          .FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(receta2);
                /*
                if(receta2 != null)
                {
                    var receta_detalle2 = (from r in db.receta_detalle_crn
                                           where r.folio_rc == receta2.folio_rc
                                           join recetaExp in db.receta_exp_crn on r.folio_rc equals recetaExp.folio_rc into reExp
                                           from receExp in reExp.DefaultIfEmpty()
                                           select new
                                           {
                                               recetaNorCount = receta_normales_count,
                                               folio_rc = r.folio_rc,
                                               codigo = r.codigo,
                                               cantidad = r.cantidad,
                                               dosis = r.dosis,
                                               indicaciones = r.indicaciones,
                                               meses_surtir = receExp.meses_surtir,
                                               fec_ini_sintomas = receExp.fec_ini_sintomas,
                                               fecha_crea = receExp.fecha_crea,
                                               terminada = receExp.terminada,
                                           })
                                     .ToList();

                    var results = new List<ResurtimientoList>();

                    foreach (var item in receta_detalle2)
                    {

                        var result = new ResurtimientoList
                        {
                            recetaNorCount = item.recetaNorCount,
                            folio_rc = item.folio_rc,
                            codigo = item.codigo,
                            cantidad = item.cantidad,
                            dosis = item.dosis,
                            indicaciones = item.indicaciones,
                            meses_surtir = item.meses_surtir,
                            fec_ini_sintomas = string.Format("{0:yyyy-MM-dd}", item.fec_ini_sintomas),
                            fecha_crea = string.Format("{0:yyyy-MM-dd}", item.fecha_crea),
                            terminada = item.terminada
                        };

                        results.Add(result);

                    }

                    return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                else
                {*/
                    var receta_detalle = "";
                    return new JsonResult { Data = receta_detalle, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                //}

                
            }



        }


        public JsonResult GetLastRecetaFolioRc(string search)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();

            var receta = (from r in db.Receta_Exp
                          where r.Medico == nombreusuario
                          where r.Expediente == search
                          where r.folio_rc != null
                          where r.Fecha == fecha_correcta
                          //where r.Estado == "9"
                          where r.TurnoFar == null
                          where r.Hora_Rcta == null
                          select new
                          {
                              Folio_Rcta = r.Folio_Rcta,
                              Estado = r.Estado,
                              Hora_Rcta = r.Hora_Rcta,
                              folio_rc = r.folio_rc,
                          })
                          .FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(receta.Folio_Rcta);

            if (receta != null)
            {
                Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
                var receta_detalle = (from r in db2.receta_detalle_crn
                                      where r.folio_rc == receta.folio_rc
                                      select new
                                      {
                                          Folio_Rcta = receta.Folio_Rcta,
                                          Codigo = r.codigo,
                                          Cantidad = r.cantidad,
                                          Dosis = r.dosis,
                                          Indicaciones = r.indicaciones,
                                          Estado = receta.Estado,
                                          Hora_Rcta = receta.Hora_Rcta,
                                      })
                                     .ToList();

                return new JsonResult { Data = receta_detalle, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var receta_detalle = "";
                return new JsonResult { Data = receta_detalle, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



        }


        public JsonResult DeleteMedicamentoResur(int folio, string codigo)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var recetas_count = (from r in db.receta_detalle_crn
                                 where r.folio_rc == folio
                                 //where r.codigo == codigo
                                 select r).Count();

            //System.Diagnostics.Debug.WriteLine(recetas_count);

            if (recetas_count > 1)
            {
                db.Database.ExecuteSqlCommand("DELETE FROM receta_detalle_crn WHERE folio_rc = " + folio + " AND codigo = '" + codigo + "' ");
                var claveDeleted = codigo;
                return new JsonResult { Data = claveDeleted, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                db.Database.ExecuteSqlCommand("DELETE FROM receta_detalle_crn WHERE folio_rc = " + folio + " AND codigo = '" + codigo + "' ");
                db.Database.ExecuteSqlCommand("DELETE FROM receta_exp_crn WHERE folio_rc = " + folio + "");
                var claveDeleted = codigo;
                return new JsonResult { Data = claveDeleted, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            //return Redirect(Request.UrlReferrer.ToString());
        }


        public class Result
        {
            public string Clave { get; set; }
            public string Descripcion_Sal { get; set; }
            public string Descripcion_Grupo { get; set; }
            public string Presentacion { get; set; }
            public int Inv_Act { get; set; }
            public string Detalles { get; set; }
            public string Fecha { get; set; }
            public string Medicamento { get; set; }
            public string Medico { get; set; }
            public string Dosis { get; set; }
            public string Historial { get; set; }
            public decimal catual { get; set; }
            public string Disponibilidad { get; set; }
        }


        public JsonResult GetMedicamentosResur(string numemp)
        {
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha__actual_correcta = DateTime.Parse(fecha_actual);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var receta_resur_detalles = (from d in db.receta_detalle_crn
                                         join recetaExp in db.receta_exp_crn on d.folio_rc equals recetaExp.folio_rc into reExp
                                         from receExp in reExp.DefaultIfEmpty()
                                         where receExp.expediente == numemp
                                         where receExp.fecha_crea != fecha__actual_correcta
                                         select new
                                         {
                                             folio_rc = d.folio_rc,
                                             codigo = d.codigo,
                                             meses_surtir = receExp.meses_surtir,
                                         })
                                         .ToList();



            var results = new List<Result>();

            foreach (var item in receta_resur_detalles)
            {
                Models.SERVMEDEntities4 db6 = new Models.SERVMEDEntities4();

                //revisa en la tabla de receta_exp cuantas se le han dado
                var recetas_exp = (from q in db6.Receta_Exp
                                   where q.Expediente == numemp
                                   where q.folio_rc == item.folio_rc
                                   select q).Count();

                var recetas2Count = recetas_exp + 1;

                if (recetas2Count < item.meses_surtir)
                {

                    var inventariofarmacia = (from q in db.InventarioFarmacia
                                              join sustan in db.Sustancia on q.Clave equals sustan.Clave into susX
                                              from susIn in susX.DefaultIfEmpty()
                                              where susIn.Consultorio != "0"
                                              where q.Clave == item.codigo
                                              where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                              select new
                                              {
                                                  Clave = q.Clave,
                                                  Descripcion_Sal = q.Descripcion_Sal,
                                                  Descripcion_Grupo = q.Descripcion_Grupo,
                                                  Presentacion = q.Presentacion,
                                                  Inv_Act = q.Inv_Act,
                                              })
                                              .OrderByDescending(g => g.Descripcion_Grupo)
                                              .FirstOrDefault();

                    var result = new Result
                    {
                        Clave = inventariofarmacia.Clave,
                        Descripcion_Sal = inventariofarmacia.Descripcion_Sal,
                        Descripcion_Grupo = inventariofarmacia.Descripcion_Grupo,
                        Presentacion = inventariofarmacia.Presentacion,
                        Inv_Act = inventariofarmacia.Inv_Act,
                    };

                    results.Add(result);
                    //db.InventarioFarmacia.Add(result);
                }
            }

            //System.Diagnostics.Debug.WriteLine(results);

            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetMedicamentosResur2(string numemp)
        {
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha__actual_correcta = DateTime.Parse(fecha_actual);

            var fecha = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-ddT00:00:00");
            var fecha_correcta = DateTime.Parse(fecha);

            //System.Diagnostics.Debug.WriteLine(fecha_correcta);

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var receta_resur_detalles = (from d in db.Receta_Detalle
                                         join recetaExp in db.Receta_Exp on d.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                         from receExp in reExp.DefaultIfEmpty()
                                         where receExp.Expediente == numemp
                                         where receExp.Fecha != fecha__actual_correcta && receExp.Fecha >= fecha_correcta
                                         select new
                                         {
                                             folio_rc = d.Folio_Rcta,
                                             codigo = d.Codigo,
                                             //meses_surtir = receExp.meses_surtir,
                                         })
                                         .ToList();

            //System.Diagnostics.Debug.WriteLine(receta_resur_detalles);


            var results = new List<Result>();

            foreach (var item in receta_resur_detalles)
            {
                //
                var inventariofarmacia = (from q in db2.InventarioFarmacia
                                          join sustan in db2.Sustancia on q.Clave equals sustan.Clave into susX
                                          from susIn in susX.DefaultIfEmpty()
                                          where susIn.Consultorio != "0"
                                          where q.Clave == item.codigo
                                          where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                          select new
                                          {
                                              Clave = q.Clave,
                                              Descripcion_Sal = q.Descripcion_Sal,
                                              Descripcion_Grupo = q.Descripcion_Grupo,
                                              Presentacion = q.Presentacion,
                                              Inv_Act = q.Inv_Act,
                                          })
                                           .OrderByDescending(g => g.Descripcion_Grupo)
                                           .FirstOrDefault();

                var result = new Result
                {
                    Clave = inventariofarmacia.Clave,
                    Descripcion_Sal = inventariofarmacia.Descripcion_Sal,
                    Descripcion_Grupo = inventariofarmacia.Descripcion_Grupo,
                    Presentacion = inventariofarmacia.Presentacion,
                    Inv_Act = inventariofarmacia.Inv_Act,
                };

                results.Add(result); //db.InventarioFarmacia.Add(result);
            }

            //System.Diagnostics.Debug.WriteLine(results);

            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public ActionResult TerminarReceta(Receta_Exp model, receta_exp_crn model2)
        {
            //System.Diagnostics.Debug.WriteLine(model.Folio_Rcta);
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha__actual_correcta = DateTime.Parse(fecha_actual);

            if (model2.folio_rc != 0)
            {
                //Tomar el costo por medicamento recetado
                var costoMedicamento = (from a in db2.receta_detalle_crn
                                        where a.folio_rc == model2.folio_rc
                                        select a).ToList();

                float? costoTotal = 0;

                foreach (var cstMed in costoMedicamento)
                {
                    costoTotal = costoTotal + cstMed.costo;
                }


                db2.Database.ExecuteSqlCommand("UPDATE receta_exp_crn SET terminada = 1, costo = '" + costoTotal + "' WHERE folio_rc = '" + model2.folio_rc + "' ");
                //db2.Database.ExecuteSqlCommand("UPDATE receta_exp_crn SET terminada = 1 WHERE folio_rc = '" + model2.folio_rc + "' ");

                //TempData["message_success"] = "¡Receta de resurtimiento activada con éxito! Favor de indicarle al paciente que pasé a recepción para agendar su próxima cita.";
                TempData["message_success"] = "¡Receta de resurtimiento activada con éxito!";

            }

            if (model.Folio_Rcta != 0)
            {
                //Tomar el costo por medicamento recetado
                var costoMedicamento = (from a in db.Receta_Detalle
                                        where a.Folio_Rcta == model.Folio_Rcta
                                        select a).ToList();

                float? costoTotal = 0;

                foreach (var cstMed in costoMedicamento)
                {
                    costoTotal = costoTotal + cstMed.costo;
                }

                //System.Diagnostics.Debug.WriteLine(costoTotal);


                //Unidad Medica
                if(model.unidad_medica == 1)
                {
                    db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '7', costo = '" + costoTotal + "', unidad_medica = '" + model.unidad_medica + "' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");

                }
                else
                {
                    if (model.unidad_medica == 2)
                    {
                        db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '8', costo = '" + costoTotal + "', unidad_medica = '" + model.unidad_medica + "' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");

                    }
                    else
                    {
                        if (model.unidad_medica == 3)
                        {
                            db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '8', costo = '" + costoTotal + "', unidad_medica = '" + model.unidad_medica + "' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");

                        }
                    }
                }
                //db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '7' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");

                //TempData["message_success"] = "¡Receta terminada con éxito!";
            }

            return RedirectToAction("ListaRecetas/" + model.Expediente, "ServiciosMedicos");

        }


        [HttpPost]
        public ActionResult TerminarRecetaUER(Receta_Exp model, receta_exp_crn model2)
        {
            //System.Diagnostics.Debug.WriteLine(model.Folio_Rcta);
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha__actual_correcta = DateTime.Parse(fecha_actual);


            if (model.Folio_Rcta != 0)
            {
                //Tomar el costo por medicamento recetado
                var costoMedicamento = (from a in db.Receta_Detalle
                                        where a.Folio_Rcta == model.Folio_Rcta
                                        select a).ToList();

                float? costoTotal = 0;

                foreach (var cstMed in costoMedicamento)
                {
                    costoTotal = costoTotal + cstMed.costo;
                }

                //System.Diagnostics.Debug.WriteLine(costoTotal);

                db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '8', costo = '" + costoTotal + "' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");
                //db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '7' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");

                //TempData["message_success"] = "¡Receta terminada con éxito!";
            }

            return RedirectToAction("ListaRecetas/" + model.Expediente, "ServiciosMedicos");

        }


        [HttpGet]
        public ActionResult ListaRecetas(string id)
        {
            //System.Diagnostics.Debug.WriteLine(id);

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    //System.Diagnostics.Debug.WriteLine(res);


                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //System.Diagnostics.Debug.WriteLine(res.tipo);
                            //Si no ha llegado el paciente a recepcion
                            if (res.hora_recepcion == null || res.hr_llamado == null)
                            {
                                //System.Diagnostics.Debug.WriteLine(res.hora_recepcion);

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " es presencial, haz clic en el boton llamar antes de iniciar la nota médica";
                                return RedirectToAction("Citas", "ServiciosMedicos");
                                //return RedirectToAction("Index", "Home");
                            }
                            //Si ya llego
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                                /*if (res.hr_consultorio == null)
                                {
                                    db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                                }*/

                                Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                                var nombreusuario = User.Identity.GetUserName();
                                var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                                var fecha = DateTime.Parse(fecha_string);

                                var exp = (from a in db3.expediente
                                               //where a.numemp == expediente
                                           where a.numemp != id
                                           where a.medico == nombreusuario
                                           where a.hora_termino == null
                                           where a.fecha == fecha
                                           select a).FirstOrDefault();

                                //System.Diagnostics.Debug.WriteLine(exp);

                                if (exp == null)
                                {
                                    //System.Diagnostics.Debug.WriteLine(exp);
                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    if (dhabientes.NUMAFIL != "P72112")
                                    {
                                        return View(dhabientes);
                                    }
                                    else
                                    {
                                        TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                        return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                    }
                                }
                                else
                                {
                                    TempData["numemp"] = exp.numemp;
                                    return RedirectToAction("SOAP/" + exp.numemp, "ServiciosMedicos");
                                }
                            }


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");
                            //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                            /*if (res.hr_consultorio == null)
                            {
                                db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                            }*/

                            Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                            var nombreusuario = User.Identity.GetUserName();
                            var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                            var fecha = DateTime.Parse(fecha_string);

                            var exp = (from a in db3.expediente
                                           //where a.numemp == expediente
                                       where a.medico == nombreusuario
                                       where a.hora_termino == null
                                       where a.fecha == fecha
                                       select a).FirstOrDefault();

                            if (exp == null)
                            {
                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();


                                if (dhabientes.NUMAFIL != "P72112")
                                {
                                    return View(dhabientes);
                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }
                            }
                            else
                            {
                                TempData["numemp"] = exp.numemp;
                                return RedirectToAction("SOAP/" + exp.numemp, "ServiciosMedicos");
                            }
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }


                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {
                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();


                        Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                        if (dhabientes.NUMAFIL != "P72112")
                        {
                            return View(dhabientes);
                        }
                        else
                        {
                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                            return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }

            }
        }


        [HttpGet]
        public ActionResult ListaRecetas2(string id)
        {
            //System.Diagnostics.Debug.WriteLine(id);

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();


                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //Si no ha llegado el paciente a recepcion
                            if (res.hora_recepcion == null || res.hr_llamado == null)
                            {
                                //System.Diagnostics.Debug.WriteLine(res.hora_recepcion);

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " es presencial, haz clic en el boton llamar antes de iniciar la nota médica";
                                return RedirectToAction("Citas", "ServiciosMedicos");
                                //return RedirectToAction("Index", "Home");
                            }
                            //Si ya llego
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");

                                Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                                var nombreusuario = User.Identity.GetUserName();
                                var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                                var fecha = DateTime.Parse(fecha_string);

                                var exp = (from a in db3.expediente
                                               //where a.numemp == expediente
                                           where a.medico == nombreusuario
                                           where a.hora_termino == null
                                           where a.fecha == fecha
                                           select a).FirstOrDefault();

                                if (exp == null)
                                {
                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    if (dhabientes.NUMAFIL != "P72112")
                                    {
                                        return View(dhabientes);
                                    }
                                    else
                                    {
                                        TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                        return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                    }
                                }
                                else
                                {
                                    TempData["numemp"] = exp.numemp;
                                    return RedirectToAction("SOAP/" + exp.numemp, "ServiciosMedicos");
                                }
                            }


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");

                            Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                            var nombreusuario = User.Identity.GetUserName();
                            var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                            var fecha = DateTime.Parse(fecha_string);

                            var exp = (from a in db3.expediente
                                           //where a.numemp == expediente
                                       where a.medico == nombreusuario
                                       where a.hora_termino == null
                                       where a.fecha == fecha
                                       select a).FirstOrDefault();

                            if (exp == null)
                            {
                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                if (dhabientes.NUMAFIL != "P72112")
                                {
                                    return View(dhabientes);
                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }
                            }
                            else
                            {
                                TempData["numemp"] = exp.numemp;
                                return RedirectToAction("SOAP/" + exp.numemp, "ServiciosMedicos");
                            }
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }


                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {
                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();


                        Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                        if (dhabientes.NUMAFIL != "P72112")
                        {
                            return View(dhabientes);
                        }
                        else
                        {
                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                            return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }

            }
        }


        public class DiferimientoList
        {
            public string fecha { get; set; }
        }

        public JsonResult DiferimientoMedico()
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaUnMes = DateTime.Now.AddMonths(6).ToString("yyyy-MM-ddT00:00:00.000");
            //var fecha_correcta = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            string query = "SELECT TOP (5) Fecha as start FROM CITAS WHERE (NumEmp is null OR NumEmp = '') and Medico = '" + User.Identity.GetUserName() + "' AND (Fecha > '" + fecha + "' and Fecha < '" + fechaUnMes + "') and tipo = '01' group by Fecha order by Fecha asc;";
            var result = db.Database.SqlQuery<Citas>(query);
            var res = result.ToList();

            var diferimientoLista = new List<DiferimientoList>();

            foreach (var cita in res)
            {
                var citaList = new DiferimientoList
                {
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", cita.start, new CultureInfo("es-ES")),
                };

                diferimientoLista.Add(citaList);
            }

            //System.Diagnostics.Debug.WriteLine(res);

            return new JsonResult { Data = diferimientoLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        public class TodasRecetasLista
        {
            public string expediente { get; set; }
            public string medico { get; set; }
            public string estado { get; set; }
            public int? terminada { get; set; }
            public string medicamento { get; set; }
            public int folio_rcta { get; set; }
            public int folio_rc { get; set; }
            public string proxima_cita { get; set; }
            public string fecha { get; set; }
            public int especialista { get; set; }
            public decimal? unidad_medica { get; set; }
        }

        public JsonResult TodasRecetas(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();

            //Recetas nomarles
            var recetas = (from r in db2.Receta_Exp
                           where r.Medico == nombreusuario
                           where r.Expediente == expediente
                           where r.Fecha == fecha_correcta
                           where r.Estado == "9" || r.Estado == "7" || r.Estado == "6" || r.Estado == "8"
                           select new
                           {
                               expediente = r.Expediente,
                               medico = r.Medico,
                               folio_rcta = r.Folio_Rcta,
                               estado = r.Estado,
                               unidad_medica = r.unidad_medica,
                               //proxima_cita = r.proxima_cita,
                               fecha = r.Fecha,
                           })
                          .ToList();


            //Recetas Resurti
            var recetasRes = (from r in db.receta_exp_crn
                              where r.medico_crea == nombreusuario
                              where r.expediente == expediente
                              where r.fecha_crea == fecha_correcta
                              where r.terminada == 1
                              select new
                              {
                                  expediente = r.expediente,
                                  medico = r.medico_crea,
                                  folio_rc = r.folio_rc,
                                  terminada = r.terminada,
                                  //proxima_cita = r.proxima_cita,
                                  fecha = r.fecha_crea,
                              })
                          .ToList();

            var proxCita = (from r in db.expediente
                            where r.medico == nombreusuario
                            where r.numemp == expediente
                            where r.fecha == fecha_correcta
                            select new
                            {
                                proxima_cita = r.proxima_cita,
                                //fecha_prox_cita = r.fecha_prox_cita
                            })
                          .FirstOrDefault();

            var textoProxCita = "";

            //Para los medicos que son sin agenda
            if (proxCita == null)
            {
                //Buscar en la receta si no tiene el campo de proxima_cita
                var recetaFirst = (from r in db2.Receta_Exp
                                   where r.Medico == nombreusuario
                                   where r.Expediente == expediente
                                   where r.Fecha == fecha_correcta
                                   where r.Estado == "9" || r.Estado == "7"
                                   select new
                                   {
                                       proxima_cita = r.proxima_cita,
                                   })
                                   .FirstOrDefault();

                if (recetaFirst != null)
                {
                    if (recetaFirst.proxima_cita != null)
                    {
                        textoProxCita = recetaFirst.proxima_cita;
                    }
                    else
                    {
                        textoProxCita = null;
                    }

                }
                else
                {
                    //Buscar en la receta si no tiene el campo de proxima_cita
                    var recetaResFirst = (from r in db.receta_exp_crn
                                          where r.medico_crea == nombreusuario
                                          where r.expediente == expediente
                                          where r.fecha_crea == fecha_correcta
                                          where r.terminada == 1
                                          select new
                                          {
                                              proxima_cita = r.proxima_cita,
                                          })
                                       .FirstOrDefault();

                    //System.Diagnostics.Debug.WriteLine(recetaResFirst);

                    if (recetaResFirst != null)
                    {
                        if (recetaResFirst.proxima_cita != null)
                        {
                            textoProxCita = recetaResFirst.proxima_cita;
                            //System.Diagnostics.Debug.WriteLine(textoProxCita);
                        }
                        else
                        {
                            textoProxCita = null;
                            System.Diagnostics.Debug.WriteLine(textoProxCita);
                        }
                    }
                    else
                    {
                        textoProxCita = null;
                    }


                }


            }
            else
            {
                textoProxCita = proxCita.proxima_cita;
            }

            //Recetas de resurtimiento
            /*var recetas_crn = (from r in db.receta_exp_crn
                               where r.medico_crea == nombreusuario
                               where r.expediente == expediente
                               where r.fecha_crea == fecha_correcta
                               //where r.estado == "9" || r.estado == "7"
                               select new
                               {
                                   expediente = r.expediente,
                                   medico = r.medico_crea,
                                   folio_rc = r.folio_rc,
                                   terminada = r.terminada,
                                   proxima_cita = r.proxima_cita,
                                   fecha = r.fecha_crea,
                               })
                              .ToList();*/

            //System.Diagnostics.Debug.WriteLine(textoProxCita);


            var recetaLista = new List<TodasRecetasLista>();


            var especialista = 0;

            if (User.IsInRole("Especialistas"))
            {
                especialista = 1;
            }
            else
            {
                especialista = 2;
            }

            foreach (var receta in recetas)
            {
                var recet = new TodasRecetasLista
                {
                    //expediente = receta.expediente,
                    //medico = receta.medico,
                    estado = receta.estado,
                    folio_rcta = receta.folio_rcta,
                    unidad_medica = receta.unidad_medica,
                    //proxima_cita = receta.proxima_cita,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", receta.fecha, new CultureInfo("es-ES")),
                    proxima_cita = textoProxCita,
                    especialista = especialista
                };

                recetaLista.Add(recet);
            }

            foreach (var receta2 in recetasRes)
            {
                var recetRs = new TodasRecetasLista
                {
                    //expediente = receta.expediente,
                    //medico = receta.medico,
                    terminada = receta2.terminada,
                    folio_rc = receta2.folio_rc,
                    //folio_rcta = null,
                    //proxima_cita = receta.proxima_cita,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", receta2.fecha, new CultureInfo("es-ES")),
                    proxima_cita = textoProxCita,
                    especialista = especialista
                };

                recetaLista.Add(recetRs);
            }


            /*foreach (var receta_crn in recetas_crn)
            {
                var recet_crm = new TodasRecetasLista
                {
                    //expediente = receta.expediente,
                    //medico = receta.medico,
                    terminada = receta_crn.terminada,
                    folio_rc = receta_crn.folio_rc,
                    proxima_cita = receta_crn.proxima_cita,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", receta_crn.fecha, new CultureInfo("es-ES")),
                };

                recetaLista.Add(recet_crm);
            }*/


            //System.Diagnostics.Debug.WriteLine(recetaLista);


            return new JsonResult { Data = recetaLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }




        public JsonResult GetRecetaDetalles(int folio_receta)
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            var recetas = (from r in db.Receta_Detalle_2
                           where r.Folio_Rcta == folio_receta
                           join inven in db.InventarioFarmacia on r.Codigo equals inven.Clave into invenX
                           from invenIn in invenX.DefaultIfEmpty()
                           select new
                           {
                               Descripcion_Sal = invenIn.Descripcion_Sal,
                               Presentacion = invenIn.Presentacion,
                               Codigo = r.Codigo,
                               Dosis = r.Dosis,
                               Indicaciones = r.Indicaciones,
                               // medicamento = "<b style='padding-top:5px;'>" + invenIn.Descripcion_Sal + " " + invenIn.Presentacion + "</b><br><span>" + r.Dosis + "</span><br><span>" + r.Indicaciones + "</span><br>",
                           }).ToList();

            var medicamentos = new List<LstInv2>();

            foreach (var item in recetas)
            {

                var sustancia = (from a in db2.Sustancia
                                 //join grupo in db2.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                                 //from grupoIn in grupoX.DefaultIfEmpty()
                                 where a.Clave == item.Codigo
                                 select a).FirstOrDefault();

                var listamedicamentos = new LstInv2
                {
                    //medicamento = "<b style='padding-top:5px;'>" + item.Descripcion_Sal + " " + sustancia.descripcion_21 + "</b><br><span>" + item.Dosis + "</span><br><span>" + item.Indicaciones + "</span><br>",
                    medicamento = "<b style='padding-top:5px;'>" + sustancia.descripcion_21 + "</b><br><span>" + item.Dosis + "</span><br><span>" + item.Indicaciones + "</span><br>",

                };

                medicamentos.Add(listamedicamentos);
            }

            //System.Diagnostics.Debug.WriteLine(recetas);

            return new JsonResult { Data = medicamentos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetRecetaDetallesCRN(int folio_receta)
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var recetas = (from r in db.receta_detalle_crn
                           where r.folio_rc == folio_receta
                           join inven in db.InventarioFarmacia on r.codigo equals inven.Clave into invenX
                           from invenIn in invenX.DefaultIfEmpty()
                           select new
                           {
                               medicamento = "<b style='padding-top:5px;'>" + invenIn.Descripcion_Sal + ", " + invenIn.Presentacion + "</b><br><span>" + r.dosis + "</span><br><span>" + r.indicaciones + "</span><br>",
                           }).ToList();

            //System.Diagnostics.Debug.WriteLine(recetas);

            return new JsonResult { Data = recetas, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult BuscarRecetaProxCita(string numexp)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            var nombreusuario = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            //Contar expediente
            if (User.IsInRole("Especialistas"))
            {

            
            var recCount = (from a in db2.Receta_Exp
                            where a.Expediente == numexp
                            where a.Medico == nombreusuario
                            where a.Fecha == fechaDT
                            where a.proxima_cita != null
                            select a).Count();
                return new JsonResult { Data = recCount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var recCount = 1;
                return new JsonResult { Data = recCount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


            

        }

        [HttpPost]
        public ActionResult RecetaProximaCita(string cantidad, string tiempo, string exp)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            var nombreusuario = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_DT = DateTime.Parse(fecha);

            //Enlistar las recetas normales
            /*var recetas_nomales = (from a in db2.Receta_Exp
                                  where a.Expediente == exp
                                  where a.Medico == nombreusuario
                                  where a.Fecha == fecha_DT
                                   select a).ToList();*/

            //Contar expediente
            var expCount = (from a in db.expediente
                            where a.numemp == exp
                            where a.medico == nombreusuario
                            where a.fecha == fecha_DT
                            select a).Count();


            if (expCount != 0)
            {
                var expFirst = (from a in db.expediente
                                where a.numemp == exp
                                where a.medico == nombreusuario
                                where a.fecha == fecha_DT
                                select a).FirstOrDefault();

                var fechaProxCita = DateTime.Now.AddMonths(1).ToString("yyyy-MM-ddT00:00:00.000");

                if (expFirst.proxima_cita == null)
                {
                    int cantidadInt = Int32.Parse(cantidad);

                    if (tiempo == "día(s)")
                    {
                        fechaProxCita = DateTime.Now.AddDays(cantidadInt).ToString("yyyy-MM-ddT00:00:00.000");
                    }
                    else
                    {
                        if (tiempo == "semana(s)")
                        {
                            var semana = cantidadInt * 7;
                            fechaProxCita = DateTime.Now.AddDays(semana).ToString("yyyy-MM-ddT00:00:00.000");
                        }
                        else
                        {
                            if (tiempo == "mes(es)")
                            {
                                fechaProxCita = DateTime.Now.AddMonths(cantidadInt).ToString("yyyy-MM-ddT00:00:00.000");
                            }
                            else
                            {
                                fechaProxCita = DateTime.Now.AddMonths(1).ToString("yyyy-MM-ddT00:00:00.000");
                            }
                        }
                    }
                    //Actualizar SOAP
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET proxima_cita = '" + cantidad + ' ' + tiempo + "', fecha_prox_cita = '" + fechaProxCita + "' WHERE fecha = '" + fecha + "' and medico = '" + nombreusuario + "' and numemp = '" + exp + "'");
                    TempData["message_success"] = "Nota de citas agregada con éxito";
                }


            }
            else
            {
                //Si es sin agenda y no hizo el SOAP, se guardara la proxima cita en la receta 
                if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                {
                    var recFirst = (from a in db2.Receta_Exp
                                    where a.Expediente == exp
                                    where a.Medico == nombreusuario
                                    where a.Fecha == fecha_DT
                                    select a).FirstOrDefault();

                    var fechaProxCita = DateTime.Now.AddMonths(1).ToString("yyyy-MM-ddT00:00:00.000");

                    System.Diagnostics.Debug.WriteLine(recFirst);

                    if (recFirst != null)
                    {
                        int cantidadInt = Int32.Parse(cantidad);

                        if (tiempo == "día(s)")
                        {
                            fechaProxCita = DateTime.Now.AddDays(cantidadInt).ToString("yyyy-MM-ddT00:00:00.000");
                        }
                        else
                        {
                            if (tiempo == "semana(s)")
                            {
                                var semana = cantidadInt * 7;
                                fechaProxCita = DateTime.Now.AddDays(semana).ToString("yyyy-MM-ddT00:00:00.000");
                            }
                            else
                            {
                                if (tiempo == "mes(es)")
                                {
                                    fechaProxCita = DateTime.Now.AddMonths(cantidadInt).ToString("yyyy-MM-ddT00:00:00.000");
                                }
                                else
                                {
                                    fechaProxCita = DateTime.Now.AddMonths(1).ToString("yyyy-MM-ddT00:00:00.000");
                                }
                            }
                        }
                        //Actualizar SOAP
                        db2.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET proxima_cita = '" + cantidad + ' ' + tiempo + "', fecha_prox_cita = '" + fechaProxCita + "' WHERE Fecha = '" + fecha + "' and Medico = '" + nombreusuario + "' and Expediente = '" + exp + "'");
                        TempData["message_success"] = "Nota de citas agregada con éxito";
                    }
                    else
                    {
                        //Buscar si solo hizo de resurtimiento
                        var recFirstRes = (from a in db.receta_exp_crn
                                           where a.expediente == exp
                                           where a.medico_crea == nombreusuario
                                           where a.fecha_crea == fecha_DT
                                           select a).FirstOrDefault();

                        System.Diagnostics.Debug.WriteLine(recFirstRes);

                        if (recFirstRes != null)
                        {
                            int cantidadInt = Int32.Parse(cantidad);

                            if (tiempo == "día(s)")
                            {
                                fechaProxCita = DateTime.Now.AddDays(cantidadInt).ToString("yyyy-MM-ddT00:00:00.000");
                            }
                            else
                            {
                                if (tiempo == "semana(s)")
                                {
                                    var semana = cantidadInt * 7;
                                    fechaProxCita = DateTime.Now.AddDays(semana).ToString("yyyy-MM-ddT00:00:00.000");
                                }
                                else
                                {
                                    if (tiempo == "mes(es)")
                                    {
                                        fechaProxCita = DateTime.Now.AddMonths(cantidadInt).ToString("yyyy-MM-ddT00:00:00.000");
                                    }
                                    else
                                    {
                                        fechaProxCita = DateTime.Now.AddMonths(1).ToString("yyyy-MM-ddT00:00:00.000");
                                    }
                                }
                            }
                            //Actualizar receta
                            db.Database.ExecuteSqlCommand("UPDATE receta_exp_crn SET proxima_cita = '" + cantidad + ' ' + tiempo + "', fecha_prox_cita = '" + fechaProxCita + "' WHERE fecha_crea = '" + fecha + "' and medico_crea = '" + nombreusuario + "' and expediente = '" + exp + "'");
                            TempData["message_success"] = "Nota de citas agregada con éxito";
                        }
                    }
                }
                else
                {
                    var dhabiente = (from a in db.DHABIENTES
                                     where a.NUMEMP == exp
                                     select a).FirstOrDefault();

                    TempData["message_warning"] = "Niguna nota médica ha sido generada a " + dhabiente.NOMBRES;
                }


            }

            return Redirect(Request.UrlReferrer.ToString());
        }


        public JsonResult CheckLastRecetaResur(string expediente, string codigo)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var receta_resur_detalles = (from d in db2.receta_detalle_crn
                                         join recetaExp in db2.receta_exp_crn on d.folio_rc equals recetaExp.folio_rc into reExp
                                         from receExp in reExp.DefaultIfEmpty()
                                         where receExp.expediente == expediente
                                         where d.codigo == codigo
                                         select new
                                         {
                                             folio_rc = d.folio_rc,
                                             codigo = d.codigo,
                                             meses_surtir = receExp.meses_surtir,
                                             cantidad = d.cantidad,
                                             dosis = d.dosis,
                                             indicaciones = d.indicaciones,
                                         })
                                         .FirstOrDefault();

            return new JsonResult { Data = receta_resur_detalles, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public class NotaFarmacoLista
        {
            public string notafarmaco { get; set; }
            public string medico { get; set; }
            public string fecha { get; set; }
            public string expediente { get; set; }

        }

        public JsonResult NotaFarmaco(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var username = User.Identity.GetUserName();
            //De tres meses en adelante
            var fecha = DateTime.Now.AddYears(-1).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_DT = DateTime.Parse(fecha);

            var notafarmaco = (from d in db.TblNotaFarmaco
                               where d.NUMEMP == expediente
                               where d.FechaNotaFarmaco >= fecha_DT
                               join medico in db.MEDICOS on d.MedicoRealizo equals medico.Numero into medicoX
                               from medicoIn in medicoX.DefaultIfEmpty()
                               select new
                               {
                                   id = d.IdNota,
                                   notafarmaco = d.NotaFarmaco,
                                   //medico = d.MedicoRealizo,
                                   medico = medicoIn.Titulo + " " + medicoIn.Nombre + " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                                   fecha = d.FechaNotaFarmaco,
                                   expediente = d.NUMEMP,
                               })
                               .ToList();

            var notafarLista = new List<NotaFarmacoLista>();

            foreach (var item in notafarmaco)
            {
                //Buscar si ya vio esa nota 
                var notaUsu = (from nf in db.NotaFar_Usuario
                                   //join notaFar in db.TblNotaFarmaco on nf.id_nota equals notaFar.id into notaFarX
                                   //from notaFarIn in notaFarX.DefaultIfEmpty()
                               where nf.usuario == username
                               where nf.id_nota == item.id
                               select nf)
                               .FirstOrDefault();

                if (notaUsu == null)
                {
                    var notfar = new NotaFarmacoLista
                    {
                        notafarmaco = item.notafarmaco,
                        medico = item.medico,
                        expediente = item.expediente,
                        fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                    };

                    notafarLista.Add(notfar);
                }



            }


            return new JsonResult { Data = notafarLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public class NotaTrabajoSocialLista
        {
            public string nota { get; set; }
            public string fecha { get; set; }
            public string num_exp { get; set; }

        }

        public JsonResult NotaTrabajoSocial(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var username = User.Identity.GetUserName();
            //De tres meses en adelante
            var fecha = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_DT = DateTime.Parse(fecha);

            var notatrabajo = (from d in db.TrabajoSocialNota
                               where d.num_exp == expediente
                               //where d.FechaNotaFarmaco >= fecha_DT
                               //join medico in db.acces on d.MedicoRealizo equals medico.Numero into medicoX
                               //from medicoIn in medicoX.DefaultIfEmpty()
                               select new
                               {
                                   id = d.id,
                                   nota = d.nota,
                                   //medico = d.MedicoRealizo,
                                   //medico = medicoIn.Titulo + " " + medicoIn.Nombre + " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                                   fecha = d.fecha,
                                   num_exp = d.num_exp,
                               })
                               .ToList();

            var notatsLista = new List<NotaTrabajoSocialLista>();

            foreach (var item in notatrabajo)
            {
                //Buscar si ya vio esa nota 

                var notaUsu = (from nts in db.NotaTS_Usuario
                               where nts.usuario == username
                               where nts.id_nota == item.id
                               select nts)
                               .FirstOrDefault();

                if (notaUsu == null)
                {
                    var notts = new NotaTrabajoSocialLista
                    {
                        nota = item.nota,
                        num_exp = item.num_exp,
                        fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                    };

                    notatsLista.Add(notts);
                }

            }


            return new JsonResult { Data = notatsLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult ConfirmarNotaTS(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var username = User.Identity.GetUserName();
            //De tres meses en adelante
            var fecha = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_DT = DateTime.Parse(fecha);

            var notatrabajo = (from d in db.TrabajoSocialNota
                               where d.num_exp == expediente
                               select new
                               {
                                   id = d.id,
                                   nota = d.nota,
                                   fecha = d.fecha,
                                   num_exp = d.num_exp,
                               })
                               .ToList();

            var notatsLista = new List<NotaTrabajoSocialLista>();

            foreach (var item in notatrabajo)
            {
                //Buscar si ya vio esa nota 
                var notaUsu = (from nf in db.NotaTS_Usuario
                               where nf.usuario == username
                               where nf.id_nota == item.id
                               select nf)
                               .FirstOrDefault();

                if (notaUsu == null)
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO NotaTS_Usuario (usuario, id_nota, fecha, estado) VALUES('" + username + "', " + item.id + ", '" + fecha + "', 1)");
                }

            }

            return new JsonResult { Data = fecha, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult TodasNotaTS(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var username = User.Identity.GetUserName();
            //De tres meses en adelante
            var fecha = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_DT = DateTime.Parse(fecha);

            var notatrabajo = (from d in db.TrabajoSocialNota
                               where d.num_exp == expediente
                               //where d.FechaNotaFarmaco >= fecha_DT
                               //join medico in db.MEDICOS on d.MedicoRealizo equals medico.Numero into medicoX
                               //from medicoIn in medicoX.DefaultIfEmpty()
                               select new
                               {
                                   id = d.id,
                                   nota = d.nota,
                                   //medico = d.MedicoRealizo,
                                   //medico = medicoIn.Titulo + " " + medicoIn.Nombre + " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                                   fecha = d.fecha,
                                   num_exp = d.num_exp,
                               })
                               .ToList();

            var notatsLista = new List<NotaTrabajoSocialLista>();

            foreach (var item in notatrabajo)
            {
                var notts = new NotaTrabajoSocialLista
                {
                    nota = item.nota,
                    num_exp = item.num_exp,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                };

                notatsLista.Add(notts);
            }


            return new JsonResult { Data = notatsLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        [HttpPost]
        public ActionResult GuardarAlergia(string medicamento, string num_exp)
        {
            //System.Diagnostics.Debug.WriteLine(id);
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);
            //System.Diagnostics.Debug.WriteLine(ultima_orden.id_orden);

            Alergias_Exp alergias = new Alergias_Exp();
            alergias.medicamento = medicamento;
            alergias.num_exp = num_exp;
            alergias.medico = User.Identity.GetUserName();
            alergias.fecha = fechaDT;
            alergias.estado = 1;
            db.Alergias_Exp.Add(alergias);
            db.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }

        public JsonResult SinAlergia(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);

            Alergias_Exp alergias = new Alergias_Exp();
            alergias.medicamento = null;
            alergias.num_exp = expediente;
            alergias.medico = User.Identity.GetUserName();
            alergias.fecha = fechaDT;
            alergias.estado = 0;
            db.Alergias_Exp.Add(alergias);
            db.SaveChanges();


            return new JsonResult { Data = alergias, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult TodasAlergias(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var alergias = (from d in db.Alergias_Exp
                            where d.num_exp == expediente
                            where d.estado == 1
                            select new
                            {
                                medicamento = d.medicamento,
                            })
                               .ToList();


            return new JsonResult { Data = alergias, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult ContarAlergias(string expediente)
        {
            //Solo los medicos generales
            var clavemedico = User.Identity.GetUserName().Substring(0, 2);

            var alergias = 1;

            if(clavemedico == "02")
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                alergias = (from d in db.Alergias_Exp
                                where d.num_exp == expediente
                                //where d.estado == 1
                                select d)
                                .Count();
            }
            


            return new JsonResult { Data = alergias, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ConfirmarNotaFar(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var username = User.Identity.GetUserName();
            //De tres meses en adelante
            var fecha = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_DT = DateTime.Parse(fecha);

            var notafarmaco = (from d in db.TblNotaFarmaco
                               where d.NUMEMP == expediente
                               where d.FechaNotaFarmaco >= fecha_DT
                               join medico in db.MEDICOS on d.MedicoRealizo equals medico.Numero into medicoX
                               from medicoIn in medicoX.DefaultIfEmpty()
                               select new
                               {
                                   id = d.IdNota,
                                   notafarmaco = d.NotaFarmaco,
                                   //medico = d.MedicoRealizo,
                                   medico = medicoIn.Titulo + " " + medicoIn.Nombre + " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                                   fecha = d.FechaNotaFarmaco,
                                   expediente = d.NUMEMP,
                               })
                               .ToList();

            var notafarLista = new List<NotaFarmacoLista>();

            foreach (var item in notafarmaco)
            {
                //Buscar si ya vio esa nota 
                var notaUsu = (from nf in db.NotaFar_Usuario
                               where nf.usuario == username
                               where nf.id_nota == item.id
                               select nf)
                               .FirstOrDefault();

                if (notaUsu == null)
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO NotaFar_Usuario (usuario, id_nota, fecha, estado) VALUES('" + username + "', " + item.id + ", '" + fecha + "', 1)");
                }

            }

            return new JsonResult { Data = fecha, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult TodasNotaFar(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var username = User.Identity.GetUserName();
            //De tres meses en adelante
            var fecha = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_DT = DateTime.Parse(fecha);

            var notafarmaco = (from d in db.TblNotaFarmaco
                               where d.NUMEMP == expediente
                               where d.FechaNotaFarmaco >= fecha_DT
                               join medico in db.MEDICOS on d.MedicoRealizo equals medico.Numero into medicoX
                               from medicoIn in medicoX.DefaultIfEmpty()
                               select new
                               {
                                   id = d.IdNota,
                                   notafarmaco = d.NotaFarmaco,
                                   //medico = d.MedicoRealizo,
                                   medico = medicoIn.Titulo + " " + medicoIn.Nombre + " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                                   fecha = d.FechaNotaFarmaco,
                                   expediente = d.NUMEMP,
                               })
                               .ToList();

            var notafarLista = new List<NotaFarmacoLista>();

            foreach (var item in notafarmaco)
            {
                var notfar = new NotaFarmacoLista
                {
                    notafarmaco = item.notafarmaco,
                    medico = item.medico,
                    expediente = item.expediente,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                };

                notafarLista.Add(notfar);
            }


            return new JsonResult { Data = notafarLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult HojaFrontal(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //if (!User.IsInRole("RecepcionQuirofano"){
                    if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                    {

                        var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                        Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                        string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                        var result = db4.Database.SqlQuery<Citas>(query);
                        var res = result.FirstOrDefault();

                        if (res != null)
                        {
                            //Si es presencial
                            if (res.tipo == "11")
                            {

                                DalHojaFrontal hf = new DalHojaFrontal();
                                HojaFrontal hoja = hf.BuscarTitular(id);

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                ViewData["hoja"] = hoja;

                                //Prepa 7
                                if (dhabientes.NUMAFIL != "P72112")
                                {
                                    return View(dhabientes);
                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }




                            }
                            //Si no es presencial entonces es telefonica
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                DalHojaFrontal hf = new DalHojaFrontal();
                                HojaFrontal hoja = hf.BuscarTitular(id);

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                ViewData["hoja"] = hoja;

                                if (dhabientes.NUMAFIL != "P72112")
                                {
                                    return View(dhabientes);
                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }
                            }

                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    else
                    {
                        if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                        {

                            DalHojaFrontal hf = new DalHojaFrontal();
                            HojaFrontal hoja = hf.BuscarTitular(id);

                            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            ViewData["hoja"] = hoja;


                            if (dhabientes.NUMAFIL != "P72112")
                            {
                                return View(dhabientes);
                            }
                            else
                            {
                                TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                            }

                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");

                        }
                    }
                /*}
                else
                {
                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                }*/
            }

        }



        public ActionResult HojaFrontal2(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {

                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    //System.Diagnostics.Debug.WriteLine(res);

                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //Si no ha llegado el paciente a recepcion
                            /*if (res.hora_recepcion == null)
                            {
                                return RedirectToAction("Index", "Home");
                            }*/
                            //Si ya llego
                            /*else
                            {*/
                            //Se va a actualizar citas
                            //var hr_consultorio = DateTime.Now.ToString("HH:mm");
                            //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                            /*if(res.hr_consultorio == null && res.hora_recepcion != null)
                            {
                                db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                            }*/

                            DalHojaFrontal hf = new DalHojaFrontal();
                            HojaFrontal hoja = hf.BuscarTitular(id);

                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            ViewData["hoja"] = hoja;
                            return View(dhabientes);
                            /*}*/


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");
                            //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                            /*if (res.hr_consultorio == null)
                            {
                                db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                            }*/
                            DalHojaFrontal hf = new DalHojaFrontal();
                            HojaFrontal hoja = hf.BuscarTitular(id);

                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();


                            //string cmdTxt = " SELECT NUMEMP, PA.DESCR   PARIENTE  ,NOMBRES ,APATERNO ,AMATERNO , convert(char, FNAC, 103)  FNAC ,SEXO,convert(char, FNAC, 103) FNAC,floor((cast(datediff(day, FNAC, getdate()) as float)/365-(floor(cast(datediff(day, FNAC, getdate()) as float)/365)))*12) AS 'meses', floor(cast(datediff(day, FNAC, getdate()) as float)/365) AS 'anios' ";
                            //cmdTxt += " FROM DHABIENTES   DH,  PARENTESCO PA WHERE  DH.FVIGENCIA != '1900-01-01T00:00:00' and dh.PARIENTE=PA.PARIENTE and  DH.NUMEMP='" + v_NUMEMP + "' ";
                            //// cmdTxt += " FROM DHABIENTES   DH,  PARENTESCO PA WHERE DH.FVIGENCIA != '1900-01-01T00:00:00' AND  dh.PARIENTE=PA.PARIENTE and DH.NUMAFIL = '" + entidad.NUMEMP + "' ";

                            //cmm = null;
                            //cmm = new SqlCommand(cmdTxt, cnn);

                            //lector = null;
                            //lector = cmm.ExecuteReader();

                            ////System.Diagnostics.Debug.WriteLine(VerifStringSql(lector, "FNAC"));



                            ////System.Diagnostics.Debug.WriteLine(nacimiento);



                            //while (lector.Read())
                            //{
                            //    DHABIENTES entidad2 = new DHABIENTES();


                            //    entidad2 = VerifStringSql(lector, "AMATERNO");

                            //}

                            //lector.Close();


                            ViewData["hoja"] = hoja;
                            return View(dhabientes);
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {

                        DalHojaFrontal hf = new DalHojaFrontal();
                        HojaFrontal hoja = hf.BuscarTitular(id);

                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();

                        ViewData["hoja"] = hoja;
                        return View(dhabientes);

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

        }


        public class HistorialPadecimientosLista
        {
            public string id { get; set; }
            public string numemp { get; set; }
            public string medico { get; set; }
            public string especialidad { get; set; }
            public string s_exp { get; set; }
            public string o_exp { get; set; }
            public string a_exp { get; set; }
            public string p_exp { get; set; }
            public string diagnostico { get; set; }
            public string edd_anio { get; set; }
            public string edd_mes { get; set; }
            public float? peso_r { get; set; }
            public float? talla_r { get; set; }
            public string ta { get; set; }
            public float? temp_r { get; set; }
            public string fresp { get; set; }
            public string fecha { get; set; }
            
            //para que me ordene correctamente el historial por fecha
            public string fecha2 { get; set; }

        }


        [HttpGet]
        public ActionResult HistorialPadecimientos(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {

                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");

                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            Models.SMDEVEntities28 db3 = new Models.SMDEVEntities28();

                            var expediente = (from q in db3.expediente
                                              where q.numemp == id
                                              join medicos in db3.MEDICOS on q.medico equals medicos.Numero
                                              join diagnosticos1 in db3.DIAGNOSTICOS on q.diagnostico equals diagnosticos1.Clave into dia1gX
                                              from diag1In in dia1gX.DefaultIfEmpty()
                                              join diagnosticos2 in db3.DIAGNOSTICOS on q.diagnostico2 equals diagnosticos2.Clave into dia2gX
                                              from diag2In in dia2gX.DefaultIfEmpty()
                                              join diagnosticos3 in db3.DIAGNOSTICOS on q.diagnostico3 equals diagnosticos3.Clave into dia3gX
                                              from diag3In in dia3gX.DefaultIfEmpty()
                                              join espe in db3.ESPECIALIDADES on q.medico.Substring(0, 2) equals espe.CLAVE
                                              //where q.Medico == "30001"
                                              select new
                                              {
                                                  numemp = q.numemp,
                                                  medico = medicos.Nombre + " " + medicos.Apaterno + " " + medicos.Amaterno,
                                                  especialidad = espe.DESCRIPCION,
                                                  s_exp = q.s_exp,
                                                  a_exp = q.a_exp,
                                                  o_exp = q.o_exp,
                                                  p_exp = q.p_exp,
                                                  diagnostico = diag1In.DesCorta + " + " + diag2In.DesCorta + " + " + diag3In.DesCorta,
                                                  edd_anio = q.edd_anio,
                                                  edd_mes = q.edd_mes,
                                                  peso_r = q.peso_r,
                                                  talla_r = q.talla_r,
                                                  ta = q.ta1 + "-" + q.ta2,
                                                  temp_r = q.temp_r,
                                                  fresp = q.fresp,
                                                  fecha = q.hora_termino,
                                              })
                                                .OrderByDescending(g => g.fecha)
                                                .ToList();


                            var historialLista = new List<HistorialPadecimientosLista>();

                            foreach (var item in expediente)
                            {

                                var historial = new HistorialPadecimientosLista
                                {
                                    numemp = item.numemp,
                                    medico = item.medico,
                                    especialidad = item.especialidad,
                                    s_exp = item.s_exp,
                                    a_exp = item.a_exp,
                                    o_exp = item.o_exp,
                                    p_exp = item.p_exp,
                                    diagnostico = item.diagnostico,
                                    edd_anio = item.edd_anio,
                                    edd_mes = item.edd_mes,
                                    peso_r = item.peso_r,
                                    talla_r = item.talla_r,
                                    ta = item.ta,
                                    temp_r = item.temp_r,
                                    fresp = item.fresp,
                                    fecha = string.Format("{0:dddd, dd MMMM yyyy HH:mm}", item.fecha, new CultureInfo("es-ES")),
                                };

                                historialLista.Add(historial);
                            }

                            string json = JsonConvert.SerializeObject(historialLista);
                            string path = Server.MapPath("~/json/historial_padecimientos/");
                            System.IO.File.WriteAllText(path + "historial_expediente_" + id + ".json", json);


                            if (dhabientes.NUMAFIL != "P72112")
                            {
                                return View(dhabientes);
                            }
                            else
                            {
                                TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                            }


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");
                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            Models.SMDEVEntities28 db3 = new Models.SMDEVEntities28();

                            var expediente = (from q in db3.expediente
                                              where q.numemp == id
                                              join medicos in db3.MEDICOS on q.medico equals medicos.Numero
                                              join diagnosticos1 in db3.DIAGNOSTICOS on q.diagnostico equals diagnosticos1.Clave into dia1gX
                                              from diag1In in dia1gX.DefaultIfEmpty()
                                              join diagnosticos2 in db3.DIAGNOSTICOS on q.diagnostico2 equals diagnosticos2.Clave into dia2gX
                                              from diag2In in dia2gX.DefaultIfEmpty()
                                              join diagnosticos3 in db3.DIAGNOSTICOS on q.diagnostico3 equals diagnosticos3.Clave into dia3gX
                                              from diag3In in dia3gX.DefaultIfEmpty()
                                              join espe in db3.ESPECIALIDADES on q.medico.Substring(0, 2) equals espe.CLAVE
                                              //where q.Medico == "30001"
                                              select new
                                              {
                                                  numemp = q.numemp,
                                                  medico = medicos.Nombre + " " + medicos.Apaterno + " " + medicos.Amaterno,
                                                  especialidad = espe.DESCRIPCION,
                                                  s_exp = q.s_exp,
                                                  a_exp = q.a_exp,
                                                  o_exp = q.o_exp,
                                                  p_exp = q.p_exp,
                                                  diagnostico = diag1In.DesCorta + " + " + diag2In.DesCorta + " + " + diag3In.DesCorta,
                                                  edd_anio = q.edd_anio,
                                                  edd_mes = q.edd_mes,
                                                  peso_r = q.peso_r,
                                                  talla_r = q.talla_r,
                                                  ta = q.ta1 + "-" + q.ta2,
                                                  temp_r = q.temp_r,
                                                  fresp = q.fresp,
                                                  fecha = q.hora_termino,
                                              })
                                                .OrderByDescending(g => g.fecha)
                                                .ToList();


                            var historialLista = new List<HistorialPadecimientosLista>();

                            foreach (var item in expediente)
                            {

                                var historial = new HistorialPadecimientosLista
                                {
                                    numemp = item.numemp,
                                    medico = item.medico,
                                    especialidad = item.especialidad,
                                    s_exp = item.s_exp,
                                    a_exp = item.a_exp,
                                    o_exp = item.o_exp,
                                    p_exp = item.p_exp,
                                    diagnostico = item.diagnostico,
                                    edd_anio = item.edd_anio,
                                    edd_mes = item.edd_mes,
                                    peso_r = item.peso_r,
                                    talla_r = item.talla_r,
                                    ta = item.ta,
                                    temp_r = item.temp_r,
                                    fresp = item.fresp,
                                    fecha = string.Format("{0:dddd, dd MMMM yyyy HH:mm}", item.fecha, new CultureInfo("es-ES")),
                                };

                                historialLista.Add(historial);
                            }


                            //ViewData["expediente"] = expediente;
                            string json = JsonConvert.SerializeObject(historialLista);
                            string path = Server.MapPath("~/json/historial_padecimientos/");
                            System.IO.File.WriteAllText(path + "historial_expediente_" + id + ".json", json);

                            if (dhabientes.NUMAFIL != "P72112")
                            {
                                return View(dhabientes);
                            }
                            else
                            {
                                TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                            }
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }


                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {
                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();

                        //Models.SMDEVEntities24 db2 = new Models.SMDEVEntities24();

                        Models.SMDEVEntities28 db3 = new Models.SMDEVEntities28();

                        var expediente = (from q in db3.expediente
                                          where q.numemp == id
                                          join medicos in db3.MEDICOS on q.medico equals medicos.Numero
                                          join diagnosticos1 in db3.DIAGNOSTICOS on q.diagnostico equals diagnosticos1.Clave into dia1gX
                                          from diag1In in dia1gX.DefaultIfEmpty()
                                          join diagnosticos2 in db3.DIAGNOSTICOS on q.diagnostico2 equals diagnosticos2.Clave into dia2gX
                                          from diag2In in dia2gX.DefaultIfEmpty()
                                          join diagnosticos3 in db3.DIAGNOSTICOS on q.diagnostico3 equals diagnosticos3.Clave into dia3gX
                                          from diag3In in dia3gX.DefaultIfEmpty()
                                          join espe in db3.ESPECIALIDADES on q.medico.Substring(0, 2) equals espe.CLAVE
                                          //where q.Medico == "30001"
                                          select new
                                          {
                                              numemp = q.numemp,
                                              medico = medicos.Nombre + " " + medicos.Apaterno + " " + medicos.Amaterno,
                                              especialidad = espe.DESCRIPCION,
                                              s_exp = q.s_exp,
                                              a_exp = q.a_exp,
                                              o_exp = q.o_exp,
                                              p_exp = q.p_exp,
                                              diagnostico = diag1In.DesCorta + " + " + diag2In.DesCorta + " + " + diag3In.DesCorta,
                                              edd_anio = q.edd_anio,
                                              edd_mes = q.edd_mes,
                                              peso_r = q.peso_r,
                                              talla_r = q.talla_r,
                                              ta = q.ta1 + "-" + q.ta2,
                                              temp_r = q.temp_r,
                                              fresp = q.fresp,
                                              fecha = q.hora_termino,
                                          })
                                            .OrderByDescending(g => g.fecha)
                                            .ToList();


                        var historialLista = new List<HistorialPadecimientosLista>();

                        foreach (var item in expediente)
                        {

                            var historial = new HistorialPadecimientosLista
                            {
                                numemp = item.numemp,
                                medico = item.medico,
                                especialidad = item.especialidad,
                                s_exp = item.s_exp,
                                a_exp = item.a_exp,
                                o_exp = item.o_exp,
                                p_exp = item.p_exp,
                                diagnostico = item.diagnostico,
                                edd_anio = item.edd_anio,
                                edd_mes = item.edd_mes,
                                peso_r = item.peso_r,
                                talla_r = item.talla_r,
                                ta = item.ta,
                                temp_r = item.temp_r,
                                fresp = item.fresp,
                                fecha = string.Format("{0:dddd, dd MMMM yyyy HH:mm}", item.fecha, new CultureInfo("es-ES")),
                            };

                            historialLista.Add(historial);
                        }


                        //ViewData["expediente"] = expediente;
                        string json = JsonConvert.SerializeObject(historialLista);
                        string path = Server.MapPath("~/json/historial_padecimientos/");
                        System.IO.File.WriteAllText(path + "historial_expediente_" + id + ".json", json);

                        if (dhabientes.NUMAFIL != "P72112")
                        {
                            return View(dhabientes);
                        }
                        else
                        {
                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                            return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }

            }

        }


        public JsonResult ListaHistorialPad(string id)
        {
            Models.SMDEVEntities33 db3 = new Models.SMDEVEntities33();

            //System.Diagnostics.Debug.WriteLine(id);

            var expediente = (from q in db3.expediente
                              where q.numemp == id
                              join medicos in db3.MEDICOS on q.medico equals medicos.Numero
                              join diagnosticos1 in db3.DIAGNOSTICOS on q.diagnostico equals diagnosticos1.Clave into dia1gX
                              from diag1In in dia1gX.DefaultIfEmpty()
                              join diagnosticos2 in db3.DIAGNOSTICOS on q.diagnostico2 equals diagnosticos2.Clave into dia2gX
                              from diag2In in dia2gX.DefaultIfEmpty()
                              join diagnosticos3 in db3.DIAGNOSTICOS on q.diagnostico3 equals diagnosticos3.Clave into dia3gX
                              from diag3In in dia3gX.DefaultIfEmpty()
                              join espe in db3.ESPECIALIDADES on q.medico.Substring(0, 2) equals espe.CLAVE
                              //where q.medico != "02101"
                              where q.hora_termino != null
                              select new
                              {
                                  medico = medicos.Nombre + " " + medicos.Apaterno + " " + medicos.Amaterno,
                                  especialidad = espe.DESCRIPCION,
                                  fecha = q.fecha,
                                  id = q.numemp + "*" + q.medico + "*" + q.fecha,
                                  medico_numero = q.medico
                              })
                                .OrderByDescending(g => g.fecha)
                                .ToList();

            string queryDet = "SELECT Dertbl.especialidad as especialidad, Dertbl.medico as medico, Dertbl.nombre as nombre, Dertbl.fecha as fecha FROM(SELECT expediente_dental.medico as medico, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as nombre, ESPECIALIDADES.DESCRIPCION as especialidad, expediente_dental.fecha, COUNT(*) as count FROM expediente_dental, ESPECIALIDADES, MEDICOS WHERE ESPECIALIDADES.CLAVE = Left(expediente_dental.medico, 2) and expediente_dental.expediente = '"+id+"' AND MEDICOS.Numero = expediente_dental.medico GROUP BY fecha, ESPECIALIDADES.DESCRIPCION, medico, MEDICOS.Nombre, MEDICOS.Apaterno, MEDICOS.Amaterno, expediente_dental.expediente) AS Dertbl GROUP BY Dertbl.medico, Dertbl.fecha, Dertbl.nombre, Dertbl.especialidad; ";
            var resultDenDet = db3.Database.SqlQuery<Dental>(queryDet);
            var expedi_den_det = resultDenDet.ToList();


            var cirugias = (from q in db3.NotaQuirurgica
                              //where q.ex == id
                            join orden in db3.Orden_Int on q.id_orden equals orden.id_orden into ordenX
                            from ordenIn in ordenX.DefaultIfEmpty()
                            join medico in db3.MEDICOS on ordenIn.medico equals medico.Numero into medicoX
                            from medicoIn in medicoX.DefaultIfEmpty()
                            join espe in db3.ESPECIALIDADES on ordenIn.medico.Substring(0, 2) equals espe.CLAVE into espeX
                            from espeIn in espeX.DefaultIfEmpty()
                            where ordenIn.medico != "02101"
                            where ordenIn.numemp == id
                            select new
                            {
                                medico = medicoIn.Nombre + " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                                especialidad = espeIn.DESCRIPCION,
                                fecha = q.fecha_registro,
                                id = ordenIn.numemp + "*" + ordenIn.medico + "*" + q.fecha_registro,
                                medico_numero = ordenIn.medico,
                                id_cirugia = q.id
                            })
                            .OrderByDescending(g => g.fecha)
                            .ToList();


            //System.Diagnostics.Debug.WriteLine(id);

            var historialLista = new List<HistorialPadecimientosLista>();

            foreach (var item in expediente)
            {

                var especialidad = "";
                var medicoNombre = "";

                if(item.medico_numero == "38112" || item.medico_numero == "38113" || item.medico_numero == "14037" || item.medico_numero == "38111" || item.medico_numero == "45001" || item.medico_numero == "03132" || item.medico_numero == "38110" || item.medico_numero == "38114" || item.medico_numero == "19017")
                {
                    especialidad = "Subrogado";
                    medicoNombre = item.medico;
                }
                else
                {
                    if(item.medico_numero == "41015")
                    {
                        especialidad = "Rayos X";
                        medicoNombre = "Rayos X";
                    }
                    else
                    {
                        especialidad = item.especialidad;
                        medicoNombre = item.medico;
                    }
                }

                var historial = new HistorialPadecimientosLista
                {
                    medico = medicoNombre,
                    especialidad = especialidad,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                    //id = id + "*" + item.medico_numero + "*"+ string.Format("{0:yyyy-MM-dd}", item.fecha, new CultureInfo("es-ES")),
                    id = "<input type='button' data-info="+ id + "*" + item.medico_numero + "*" + string.Format("{0:yyyy-MM-dd}", item.fecha, new CultureInfo("es-ES")) + " class='btn btn-vermas detalles' value='+ Consulta'>",
                    fecha2 = string.Format("{0:yyyy-MM-dd}", item.fecha, new CultureInfo("es-ES")),
                };

                historialLista.Add(historial);
            }
            
            foreach (var item in cirugias)
            {

                var especialidad = "";
                var medicoNombre = "";

                if (item.medico_numero == "38112" || item.medico_numero == "38113" || item.medico_numero == "14037" || item.medico_numero == "38111" || item.medico_numero == "45001" || item.medico_numero == "03132" || item.medico_numero == "38110" || item.medico_numero == "38114" || item.medico_numero == "19017")
                {
                    especialidad = "Subrogado";
                    medicoNombre = item.medico;
                }
                else
                {
                    if (item.medico_numero == "41015")
                    {
                        especialidad = "Rayos X";
                        medicoNombre = "Rayos X";
                    }
                    else
                    {
                        especialidad = item.especialidad;
                        medicoNombre = item.medico;
                    }
                }

                var historial = new HistorialPadecimientosLista
                {
                    medico = medicoNombre,
                    especialidad = especialidad,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                    //id = id + "*" + item.medico_numero + "*"+ string.Format("{0:yyyy-MM-dd}", item.fecha, new CultureInfo("es-ES")),
                    id = "<input type='button' data-info=" + item.id_cirugia + " class='btn btn-vermas detallesCir' value='+ Cirugía'>",
                    fecha2 = string.Format("{0:yyyy-MM-dd}", item.fecha, new CultureInfo("es-ES")),
                };

                historialLista.Add(historial);
            }
            

            foreach (var item in expedi_den_det)
            {

                var historial = new HistorialPadecimientosLista
                {
                    medico = item.nombre,
                    especialidad = "DENTAL",
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                    //id = id + "*" + item.medico_numero + "*"+ string.Format("{0:yyyy-MM-dd}", item.fecha, new CultureInfo("es-ES")),
                    id = "<input type='button' data-info=" + id + "*" + item.medico + "*" + string.Format("{0:yyyy-MM-dd}", item.fecha, new CultureInfo("es-ES")) + " class='btn btn-vermas detalles' value='+ Dental'>",
                    fecha2 = string.Format("{0:yyyy-MM-dd}", item.fecha, new CultureInfo("es-ES")),
                };

                historialLista.Add(historial);
            }

            
            return new JsonResult { Data = historialLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }

        public JsonResult DetallesConsultas(string numexp, string medico, string fecha)
        {
            //var result = "";

            //System.Diagnostics.Debug.WriteLine(fecha);

            Models.SMDEVEntities33 db3 = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            //var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            //System.Diagnostics.Debug.WriteLine(id);

            var expediente = (from q in db3.expediente
                              where q.numemp == numexp
                              where q.medico == medico
                              where q.fecha == fechaDT
                              join medicos in db3.MEDICOS on q.medico equals medicos.Numero
                              join diagnosticos1 in db3.DIAGNOSTICOS on q.diagnostico equals diagnosticos1.Clave into dia1gX
                              from diag1In in dia1gX.DefaultIfEmpty()
                              join diagnosticos2 in db3.DIAGNOSTICOS on q.diagnostico2 equals diagnosticos2.Clave into dia2gX
                              from diag2In in dia2gX.DefaultIfEmpty()
                              join diagnosticos3 in db3.DIAGNOSTICOS on q.diagnostico3 equals diagnosticos3.Clave into dia3gX
                              from diag3In in dia3gX.DefaultIfEmpty()
                              join espe in db3.ESPECIALIDADES on q.medico.Substring(0, 2) equals espe.CLAVE
                              where q.hora_termino != null
                              select new
                              {
                                  medico = medicos.Nombre + " " + medicos.Apaterno + " " + medicos.Amaterno,
                                  especialidad = espe.DESCRIPCION,
                                  fecha = q.fecha,
                                  hora_termino = q.hora_termino,
                                  s_exp = q.s_exp,
                                  a_exp = q.a_exp,
                                  o_exp = q.o_exp,
                                  p_exp = q.p_exp,
                                  diagnostico1 = diag1In.DesCorta,
                                  diagnostico2 = diag2In.DesCorta,
                                  diagnostico3 = diag3In.DesCorta,
                                  edad = q.edd_anio,
                                  mes = q.edd_mes,
                                  peso = q.peso_r,
                                  talla = q.talla_r,
                                  ta = q.ta1 + "-" + q.ta2,
                                  temp = q.temp_r,
                                  fresp = q.fresp,
                                  fcard = q.fcard,
                                  dstx = q.dstx,
                                  masa_muscular = q.masa_muscular,
                                  masa_grasa = q.masa_grasa,
                                  porcentaje_grasa = q.porcentaje_grasa,
                              })
                              .FirstOrDefault();


            var dhabiente = (from q in db3.DHABIENTES
                             where q.NUMEMP == numexp
                             select q).
                             FirstOrDefault();


            int edad = 0;
            int meses = 0;

            var today = fechaDT;
            DateTime fnac = (DateTime)dhabiente.FNAC;
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

            if (Years >= 99)
            {
                edad = 99;
            }
            else
            {
                edad = Years;
            }

            meses = Months;




            var result = new Object();

            if (expediente != null)
            {

                var analisis = "";

                if (expediente.diagnostico1 != null)
                {
                    analisis = expediente.diagnostico1;
                }

                if (expediente.diagnostico2 != null)
                {
                    analisis = expediente.diagnostico1 + " + " + expediente.diagnostico2;
                }

                if (expediente.diagnostico3 != null)
                {
                    analisis = expediente.diagnostico1 + " + " + expediente.diagnostico2 + " + " + expediente.diagnostico1;
                }

                //Signos vitales
                var clavemedico = medico.Substring(0, 2);
                var signos = "";

                //System.Diagnostics.Debug.WriteLine(clavemedico);

                //Nutricion
                if (clavemedico == "26")
                {
                    signos =
                        "<div class='row m-0 pt-2'>" +
                            "<div class='col-12 p-2'>" +
                                "<div class='linea2 mt-3 mb-3'></div>" +
                                "<h5 class='mt-2'><b>Signos Vitales:</b></h5>" +
                            "</div>" +
                            "<div class='col-6 p-2'>" +
                                "<h6 class='m-0'><b>Peso:</b> " + expediente.peso + "</h5>" +
                                "<h6 class='m-0'><b>Talla:</b> " + expediente.talla + "</h5>" +
                                "<h6 class='m-0'><b>Masa muscular:</b> " + expediente.masa_muscular + "</h5>" +
                            "</div>" +
                            "<div class='col-6 p-2'>" +
                                "<h6 class='m-0'><b>Masa grasa:</b> " + expediente.masa_grasa + "</h5>" +
                                "<h6 class='m-0'><b>Porcentaje grasa:</b> " + expediente.porcentaje_grasa + "</h5>" +
                                "<h6 class='m-0'><b>Destrostrix:</b> " + expediente.dstx + "</h5>" +
                            "</div>" +
                        "</div>";
                }
                else
                {
                    signos =
                        "<div class='row m-0 pt-2'>" +
                            "<div class='col-12 p-2'>" +
                                "<div class='linea2 mt-3 mb-3'></div>" +
                                "<h5 class='mt-2'><b>Signos Vitales</b></h5>" +
                            "</div>" +
                            "<div class='col-6 p-2'>" +
                                "<h6 class='m-0'><b>Peso:</b> " + expediente.peso + "</h5>" +
                                "<h6 class='m-0'><b>Talla:</b> " + expediente.talla + "</h5>" +
                                "<h6 class='m-0'><b>Temperatura:</b> " + expediente.temp + "</h5>" +
                                "<h6 class='m-0'><b>F.Resp:</b> " + expediente.fresp + "</h5>" +
                            "</div>" +
                            "<div class='col-6 p-2'>" +
                                "<h6 class='m-0'><b>F.Card:</b> " + expediente.fcard + "</h5>" +
                                "<h6 class='m-0'><b>T.A.:</b> " + expediente.ta + "</h5>" +
                                "<h6 class='m-0'><b>Destrostrix:</b> " + expediente.dstx + "</h5>" +
                            "</div>" +
                        "</div>";
                }


                //Revisar si tiene una receta

                var fechaNCB = DateTime.Now.ToString("2022-04-04T00:00:00.000");
                var fechaNCBDT = DateTime.Parse(fechaNCB);

                var receta = "";

                //System.Diagnostics.Debug.WriteLine(fechaDT);

                if (fechaDT >= fechaNCBDT) {

                    var recetas_exp = (from q in db.Receta_Exp
                                       where q.Expediente == numexp
                                       where q.Medico == medico
                                       where q.Fecha == fechaDT
                                       select q).ToList();

                    var recetasCount = (from q in db.Receta_Exp
                                       where q.Expediente == numexp
                                       where q.Medico == medico
                                       where q.Fecha == fechaDT
                                       select q).Count();

                    //System.Diagnostics.Debug.WriteLine(recetasCount);

                    if (recetasCount > 0)
                    {

                    
                        receta ="<div class='row m-0 pt-2'>" +
                                    "<div class='col-12 p-2'>" +
                                        "<div class='linea2 mt-3 mb-3'></div>" +
                                        "<h5 class='mt-2'><b>Receta</b></h5>" +
                                    "</div>";

                        foreach (var item in recetas_exp)
                        {

                            var recetas_dtl = (from q in db.Receta_Detalle
                                               join sustan in db.Sustancia on q.Codigo equals sustan.Clave into susX
                                               from susIn in susX.DefaultIfEmpty()
                                               where q.Folio_Rcta == item.Folio_Rcta
                                               //where q.CantSurtida != null
                                               select new
                                               {
                                                   Folio_Rcta = q.Folio_Rcta,
                                                   Medicamento = susIn.descripcion_21,
                                                   Dosis = q.Dosis,
                                                   CantSurtida = q.CantSurtida,
                                                   Indicaciones = q.Indicaciones,

                                               }).ToList();

                            var recetasDtlCount = (from q in db.Receta_Detalle
                                               join sustan in db.Sustancia on q.Codigo equals sustan.Clave into susX
                                               from susIn in susX.DefaultIfEmpty()
                                               where q.Folio_Rcta == item.Folio_Rcta
                                               //where q.CantSurtida != null
                                               select q).Count();

                            foreach (var item2 in recetas_dtl)
                            {

                                receta = receta +
                                    "<div class='col-12 p-2'>" +
                                        "<h6 class='m-0'> " + item2.CantSurtida + ", " + item2.Medicamento + "</h5>" +
                                        "<h6 class='m-0'> " + item2.Dosis + "</h5>" +
                                        "<h6 class='m-0'> " + item2.Indicaciones + "</h5>" +
                                        //"<h6 class='m-0'><b>Cantidad surtida:</b>" + item2.CantSurtida + "</h5>" +
                                    "</div>";
                            }


                            if (recetasCount > 1)
                            {
                                receta = receta +  
                                    "<div class='col-12 p-2'>" +
                                        "<div class='linea3 mt-3 mb-3'></div>" +
                                    "</div>";
                            }

                            if(recetasDtlCount == 0)
                            {
                                receta = "<div>";
                            }


                        }


                        receta = receta +
                                "</div>";

                    }


                }
                else
                {

                //System.Diagnostics.Debug.WriteLine(receta);
                
                    if(expediente.a_exp != null)
                    {
                        receta =
                            "<div class='row m-0 pt-2'>" +
                                "<div class='col-12 p-2'>" +
                                    "<div class='linea2 mt-3 mb-3'></div>" +
                                    "<h5 class='mt-2'><b>Receta</b></h5>" +
                                "</div>" +
                                "<div class='col-12 p-2'>" +
                                    "<h6 class='m-0'><b>Receta:</b> " + expediente.a_exp + "</h5>" +
                                "</div>" +
                            "</div>";
                    }
                }

                var medicoHead = "";

                if (User.IsInRole("RecepcionQuirofano"))
                {
                    medicoHead = "<div class='row m-0 pt-2'>" +
                                    "<div class='col-3 p-2 text-center logo1'>" +
                                        "<img src='/Imagenes/logo-uanl-impresion.png'/>" +
                                    "</div>" +
                                    "<div class='col p-2 text-center'>" +
                                        "<h5 class='m-0'><b>" + expediente.medico + "</b></h5>" +
                                    "</div>" +
                                    "<div class='col-3 p-2 text-center logo2'>" +
                                        "<img src='/Imagenes/Servicios Medicos_Mesa de trabajo 1.png' />" +
                                    "</div>" +
                                "</div>";
                }
                else
                {
                    medicoHead = "<div class='row m-0 pt-2'>" +
                                    "<div class='col-12 p-2 text-center'>" +
                                        "<h5 class='m-0'><b>" + expediente.medico + "</b></h5>" +
                                    "</div>" +
                                "</div>";
                }


                result = new
                {
                    paciente =
                    "<div class='row m-0 pt-2'>" +
                        "<div class='col-12 p-2'>" +
                            "<div class='linea2 mt-3 mb-3'></div>" +
                            "<h5 class='mt-2 mb-0'><b>Paciente</b></h5>" +
                        "</div>" +
                        "<div class='col-6 p-2'>" +
                            "<h6 class='m-0'><b>Nombre: </b>" + dhabiente.NOMBRES + " " + dhabiente.APATERNO + " " + dhabiente.AMATERNO + "</h6>" +
                            "<h6 class='m-0'><b>Edad: </b>" + edad + "</h6>" +
                            "<h6 class='m-0'><b>Meses: </b>" + meses + "</h6>" +
                        "</div>" +
                     "</div>",

                 
                    medico = medicoHead,

                    info =
                    signos + 
                    "<div class='row m-0 pt-2'>" +
                        "<div class='col-12 p-2'>" +
                            "<div class='linea2 mt-3 mb-3'></div>" +
                            "<h5 class='mt-2 mb-0'><b>Nota Médica</b></h5>" +
                        "</div>" +
                        "<div class='col-12 p-2'>" +
                            "<h6><b>Subjetivo: </b>" + expediente.s_exp + "</h6>" +
                            "<h6><b>Objetivo: </b>" + expediente.o_exp + "</h6>" +
                            "<h6><b>Analisis: </b>" + analisis + "</h6>" +
                            "<h6><b>Plan: </b>" + expediente.p_exp + "</h6>" +
                        "</div>" +
                     "</div>" +
                     receta,

                    fecha =
                        "<div class='row m-0'>" +
                            "<div class='col-12 p-2 text-center'>" +
                                //"<h6>Fecha: <em>" + string.Format("{0:yyyy-MM-ddTHH:mm}", expediente.hora_termino) + "</em></h6>" +
                                "<h6>Fecha: <em>" + string.Format("{0:dddd, dd MMMM yyyy hh:mm tt}", expediente.hora_termino) + "</em></h6>" +      
                            "</div>" +
                        "</div>",

                };


                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



            }
            else
            {

                //Ver si es de dental
                //string queryDet = "SELECT Dertbl.especialidad as especialidad, Dertbl.medico as medico, Dertbl.nombre as nombre, Dertbl.fecha as fecha FROM(SELECT expediente_dental.medico as medico, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as nombre, ESPECIALIDADES.DESCRIPCION as especialidad, expediente_dental.fecha, COUNT(*) as count FROM expediente_dental, ESPECIALIDADES, MEDICOS WHERE ESPECIALIDADES.CLAVE = Left(expediente_dental.medico, 2) and expediente_dental.expediente = '" + numexp + "' and expediente_dental.medico = '" + medico + "' and expediente_dental.fecha = '" + fecha + "T00:00:00.000' AND MEDICOS.Numero = expediente_dental.medico GROUP BY fecha, ESPECIALIDADES.DESCRIPCION, medico, MEDICOS.Nombre, MEDICOS.Apaterno, MEDICOS.Amaterno, expediente_dental.expediente) AS Dertbl GROUP BY Dertbl.medico, Dertbl.fecha, Dertbl.nombre, Dertbl.especialidad; ";
                string queryDet = "SELECT expediente, edad, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as medico, pieza, nota, fecha, DIAGNOSTICOS.DescCompleta as diagnostico, Dental_N1.descripcion as servicio, DENTAL_N2.DESCRIPCION as sub_servicio FROM expediente_dental INNER JOIN DIAGNOSTICOS ON expediente_dental.diagnostico = DIAGNOSTICOS.Clave INNER JOIN MEDICOS ON expediente_dental.medico = MEDICOS.Numero INNER JOIN Dental_N1 ON expediente_dental.servicio = Dental_N1.codigo INNER JOIN DENTAL_N2 ON expediente_dental.sub_servicio = DENTAL_N2.CODIGO  WHERE expediente = '" + numexp + "' and medico = '" + medico + "' and fecha = '" + fecha + "T00:00:00.000'; ";
                var resultDenDet = db3.Database.SqlQuery<Dental>(queryDet);
                var expedi_den_det = resultDenDet.ToList();

                //System.Diagnostics.Debug.WriteLine(expedi_den_det);

                var pieza = "";
                var medico_dental = "";
                var fecha_dental = "";

                foreach (var item in expedi_den_det)
                {
                    pieza = pieza +
                            "<div class='row m-0 pt-2'>" +
                                "<div class='col-12 p-2'>" +
                                    "<div class='linea2 mt-3 mb-3'></div>" +
                                    "<h6 class='mt-2'><b>Pieza:</b> " + item.pieza + " </h6>" +
                                    "<h6><b>Servicio:</b> " + item.servicio + " | " + item.sub_servicio + "</h6>" +
                                    "<h6><b>Diagnóstico:</b> " + item.diagnostico + "</h6>" +
                                    "<h6><b>Nota:</b> " + item.nota + "</h6>" +
                                "</div>" +
                            "</div>";

                    medico_dental = 
                        "<div class='row m-0 pt-2'>" +
                            "<div class='col-12 p-2 text-center'>" +
                                "<h5 class='m-0'><b>" + item.medico + "</b></h5>" +
                            "</div>" +
                        "</div>";

                    fecha_dental =
                        "<div class='row m-0'>" +
                            "<div class='col-12 p-2 text-center'>" +
                                //"<h6>Fecha: <em>" + string.Format("{0:yyyy-MM-dd}", item.fecha) + "</em></h6>" +
                                "<h6>Fecha: <em>" + string.Format("{0:dddd, dd MMMM yyyy}", item.fecha) + "</em></h6>" +
                            "</div>" +
                        "</div>";
                }

                //System.Diagnostics.Debug.WriteLine(pieza);

                result = new
                {
                    paciente =
                    "<div class='row m-0 pt-2'>" +
                        "<div class='col-12 p-2'>" +
                            "<div class='linea2 mt-3 mb-3'></div>" +
                            "<h5 class='mt-2 mb-0'><b>Paciente</b></h5>" +
                        "</div>" +
                        "<div class='col-6 p-2'>" +
                            "<h6 class='m-0'><b>Nombre: </b>" + dhabiente.NOMBRES + " " + dhabiente.APATERNO + " " + dhabiente.AMATERNO + "</h6>" +
                            "<h6 class='m-0'><b>Edad: </b>" + edad + "</h6>" +
                            "<h6 class='m-0'><b>Meses: </b>" + meses + "</h6>" +
                        "</div>" +
                     "</div>",

                    medico = medico_dental,
                    info = pieza,
                    fecha = fecha_dental,
                };

                return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
        }

        public JsonResult DetallesConsultasCir(int id)
        {

            Models.SMDEVEntities33 db3 = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            var cirugias = (from q in db3.NotaQuirurgica
                            where q.id == id
                            join orden in db3.Orden_Int on q.id_orden equals orden.id_orden into ordenX
                            from ordenIn in ordenX.DefaultIfEmpty()
                            join medico in db3.MEDICOS on ordenIn.medico equals medico.Numero into medicoX
                            from medicoIn in medicoX.DefaultIfEmpty()
                            join espe in db3.ESPECIALIDADES on ordenIn.medico.Substring(0, 2) equals espe.CLAVE into espeX
                            from espeIn in espeX.DefaultIfEmpty()
                            join dhab in db3.DHABIENTES on ordenIn.numemp equals dhab.NUMEMP into dhabX
                            from dhabIn in dhabX.DefaultIfEmpty()
                            join diag1 in db3.DIAGNOSTICOS on q.diagnostico1 equals diag1.Clave into diag1X
                            from diag1In in diag1X.DefaultIfEmpty()
                            join diag2 in db3.DIAGNOSTICOS on q.diagnostico2 equals diag2.Clave into diag2X
                            from diag2In in diag2X.DefaultIfEmpty()
                            join diag3 in db3.DIAGNOSTICOS on q.diagnostico3 equals diag3.Clave into diag3X
                            from diag3In in diag3X.DefaultIfEmpty()
                            where ordenIn.medico != "02101"
                            select new
                            {
                                medico = medicoIn.Nombre + " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                                especialidad = espeIn.DESCRIPCION,
                                fecha = q.fecha_registro,
                                id = ordenIn.numemp + "*" + ordenIn.medico + "*" + q.fecha_registro,
                                medico_numero = ordenIn.medico,
                                id_cirugia = q.id,
                                dhabiente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                fnac = dhabIn.FNAC,
                                cirugia_planeada = q.cirugia_planeada,
                                cirugia_realizada = q.cirugia_realizada,
                                tipo_anestesia = q.tipo_anestesia,
                                descripcion = q.descripcion,
                                hallazgo = q.hallazgo,
                                incidentes = q.incidentes,
                                sangrado = q.sangrado,
                                estado_actual = q.estado_actual,
                                plan_manejo = q.plan_manejo,
                                pronostico = q.pronostico,
                                tiempo = q.tiempo,
                                dia1 = diag1In.DescCompleta,
                                dia2 = diag2In.DescCompleta,
                                dia3 = diag3In.DescCompleta,
                                motivo_alta = ordenIn.motivo_alta,
                                fecha_alta = ordenIn.fecha_alta
                            })
                            .OrderByDescending(g => g.fecha)
                            .FirstOrDefault();


            int edad = 0;
            int meses = 0;

            var today = fechaDT;
            DateTime fnac = (DateTime)cirugias.fnac;
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

            if (Years >= 99)
            {
                edad = 99;
            }
            else
            {
                edad = Years;
            }

            meses = Months;

            var result = new Object();


            var alta = "";
            if(cirugias.motivo_alta != null)
            {
                alta =
                    "<div class='row m-0 pt-2'>" +
                        "<div class='col-12 p-2'>" +
                            "<div class='linea2 mt-3 mb-3'></div>" +
                            "<h5 class='mt-2 mb-0'><b>Alta</b></h5>" +
                        "</div>" +
                        "<div class='col-12 p-2'>" +
                        "<h6 class='m-0'><b>Motivo: </b>" + cirugias.motivo_alta + "</h6>" +
                        "<h6 class='m-0'><b>Fecha de alta: </b>" + cirugias.fecha_alta + "</h6>" +
                        "</div>" +
                     "</div>";
            }
            else
            {
                alta =
                    "<div class='row m-0 pt-2'>" +
                        "<div class='col-12 p-2'>" +
                            "<div class='linea2 mt-3 mb-3'></div>" +
                            "<h5 class='mt-2 mb-0'><b>Alta</b></h5>" +
                        "</div>" +
                        "<div class='col-12 p-2'>" +
                        "<h6 class='m-0'><b>Sin registrar</h6>" +
                        "</div>" +
                     "</div>";
            }

            result = new
            {
                paciente =
                    "<div class='row m-0 pt-2'>" +
                        "<div class='col-12 p-2'>" +
                            "<div class='linea2 mt-3 mb-3'></div>" +
                            "<h5 class='mt-2 mb-0'><b>Paciente</b></h5>" +
                        "</div>" +
                        "<div class='col-6 p-2'>" +
                            "<h6 class='m-0'><b>Nombre: </b>" + cirugias.dhabiente + "</h6>" +
                            "<h6 class='m-0'><b>Edad: </b>" + edad + "</h6>" +
                            "<h6 class='m-0'><b>Meses: </b>" + meses + "</h6>" +
                        "</div>" +
                     "</div>",

                medico =
                        "<div class='row m-0 pt-2'>" +
                            "<div class='col-12 p-2 text-center'>" +
                                "<h5 class='m-0'><b>" + cirugias.medico + "</b></h5>" +
                            "</div>" +
                        "</div>",

                info =
                    "<div class='row m-0 pt-2'>" +
                        "<div class='col-12 p-2'>" +
                            "<div class='linea2 mt-3 mb-3'></div>" +
                            "<h5 class='mt-2 mb-0'><b>Nota Pre Quirúrgica</b></h5>" +
                        "</div>" +
                        "<div class='col-12 p-2'>" +
                        "<h6 class='m-0'><b>Cirugía Planeada: </b>" + cirugias.cirugia_planeada + "</h6>" +
                        "<h6 class='m-0'><b>Cirugía Realizada: </b>" + cirugias.cirugia_realizada + "</h6>" +
                        "<h6 class='m-0'><b>Tipo de Anestesia: </b>" + cirugias.tipo_anestesia + "</h6>" +
                        "<h6 class='m-0'><b>Técnica Quirúrgica: </b>" + cirugias.descripcion + "</h6>" +
                        "<h6 class='m-0'><b>Hallazgo: </b>" + cirugias.hallazgo + "</h6>" +
                        "<h6 class='m-0'><b>Incidentes: </b>" + cirugias.incidentes + "</h6>" +
                        "<h6 class='m-0'><b>Sangrado: </b>" + cirugias.sangrado + "</h6>" +
                        "<h6 class='m-0'><b>Estado Actual Problemas Clínicos: </b>" + cirugias.estado_actual + "</h6>" +
                        "<h6 class='m-0'><b>Plan de manejo: </b>" + cirugias.plan_manejo + "</h6>" +
                        "<h6 class='m-0'><b>Pronóstico: </b>" + cirugias.pronostico + "</h6>" +
                        "<h6 class='m-0'><b>Tiempo de cirugía: </b>" + cirugias.tiempo + "</h6>" +
                        "<h6 class='m-0'><b>Diagnostico 1: </b>" + cirugias.dia1 + "</h6>" +
                        "<h6 class='m-0'><b>Diagnostico 2: </b>" + cirugias.dia2 + "</h6>" +
                        "<h6 class='m-0'><b>Diagnostico 3: </b>" + cirugias.dia3 + "</h6>" +
                        "</div>" +
                     "</div>" + alta,

                fecha =
                        "<div class='row m-0'>" +
                            "<div class='col-12 p-2 text-center'>" +
                                //"<h6>Fecha: <em>" + string.Format("{0:yyyy-MM-dd}", cirugias.fecha) + "</em></h6>" +
                                "<h6>Fecha: <em>" + string.Format("{0:dddd, dd MMMM yyyy hh:mm tt}", cirugias.fecha) + "</em></h6>" +
                            "</div>" +
                        "</div>",

            };

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Kardex(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {

                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {

                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();



                            if (dhabientes.NUMAFIL != "P72112")
                            {
                                return View(dhabientes);
                            }
                            else
                            {
                                TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                            }

                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {

                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();



                            if (dhabientes.NUMAFIL != "P72112")
                            {
                                return View(dhabientes);
                            }
                            else
                            {
                                TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                            }
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }


                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {
                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();

                        if (dhabientes.NUMAFIL != "P72112")
                        {
                            return View(dhabientes);
                        }
                        else
                        {
                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                            return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }

            }

        }

        public class KardexLista
        {
            public string medico { get; set; }
            public string especialidad { get; set; }
            public string incidencia { get; set; }
            public string fecha { get; set; }
            public string fecha2 { get; set; }
        }

        public JsonResult ListaKardex(string id)
        {
            var fechaCita = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();

            //Citas tardes en el kiosco
            string query = "select fecha as start,  MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as Medico, ESPECIALIDADES.DESCRIPCION as especialidad from CITAS INNER JOIN MEDICOS ON CITAS.medico = MEDICOS.Numero INNER JOIN ESPECIALIDADES ON LEFT(MEDICOS.Numero, 2) = ESPECIALIDADES.CLAVE WHERE NumEmp = '" + id + "' and fecha_tarde is not null and fecha <= '"+ fechaCita + "';";
            var result = db4.Database.SqlQuery<Citas>(query);
            var res = result.ToList();

            //Citas canceladas
            string query2 = "select fecha as start,  MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as Medico, ESPECIALIDADES.DESCRIPCION as especialidad from CITAS INNER JOIN MEDICOS ON CITAS.medico = MEDICOS.Numero INNER JOIN ESPECIALIDADES ON LEFT(MEDICOS.Numero, 2) = ESPECIALIDADES.CLAVE WHERE CITAS.NumEmp = '" + id + "' and CITAS.Tipo = '00' and CITAS.fecha_tarde is null and CITAS.fecha <= '" + fechaCita + "';";
            var result2 = db4.Database.SqlQuery<Citas>(query2);
            var res2 = result2.ToList();

            //No se presento
            string query3 = "select fecha as start,  MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as Medico, ESPECIALIDADES.DESCRIPCION as especialidad from CITAS INNER JOIN MEDICOS ON CITAS.medico = MEDICOS.Numero INNER JOIN ESPECIALIDADES ON LEFT(MEDICOS.Numero, 2) = ESPECIALIDADES.CLAVE WHERE CITAS.NumEmp = '" + id + "' and CITAS.Tipo = '11' and CITAS.hora_recepcion is null and CITAS.fecha <= '" + fechaCita + "'; ";
            var result3 = db4.Database.SqlQuery<Citas>(query3);
            var res3 = result3.ToList();

            //No contesto
            string query4 = "select fecha as start,  MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as Medico, ESPECIALIDADES.DESCRIPCION as especialidad from CITAS INNER JOIN MEDICOS ON CITAS.medico = MEDICOS.Numero INNER JOIN ESPECIALIDADES ON LEFT(MEDICOS.Numero, 2) = ESPECIALIDADES.CLAVE WHERE CITAS.NumEmp = '" + id + "' and CITAS.NoContesto = 1 and CITAS.fecha <= '" + fechaCita + "'; ";
            var result4 = db4.Database.SqlQuery<Citas>(query4);
            var res4 = result4.ToList();

            var kardexLista = new List<KardexLista>();

            foreach (var item in res)
            {

                var kardelst = new KardexLista
                {
                    medico = item.Medico,
                    especialidad = item.especialidad,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.start, new CultureInfo("es-ES")),
                    fecha2 = string.Format("{0:yyyy-MM-dd}", item.start, new CultureInfo("es-ES")),
                    incidencia = "Retardo",
                };

                kardexLista.Add(kardelst);
            }


            foreach (var item in res2)
            {

                var kardelst = new KardexLista
                {
                    medico = item.Medico,
                    especialidad = item.especialidad,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.start, new CultureInfo("es-ES")),
                    fecha2 = string.Format("{0:yyyy-MM-dd}", item.start, new CultureInfo("es-ES")),
                    incidencia = "Cancelada",
                };

                kardexLista.Add(kardelst);
            }


            foreach (var item in res3)
            {

                var kardelst = new KardexLista
                {
                    medico = item.Medico,
                    especialidad = item.especialidad,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.start, new CultureInfo("es-ES")),
                    fecha2 = string.Format("{0:yyyy-MM-dd}", item.start, new CultureInfo("es-ES")),
                    incidencia = "No se presentó",
                };

                kardexLista.Add(kardelst);
            }


            foreach (var item in res4)
            {

                var kardelst = new KardexLista
                {
                    medico = item.Medico,
                    especialidad = item.especialidad,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.start, new CultureInfo("es-ES")),
                    fecha2 = string.Format("{0:yyyy-MM-dd}", item.start, new CultureInfo("es-ES")),
                    incidencia = "No constesto llamada",
                };

                kardexLista.Add(kardelst);
            }


            //System.Diagnostics.Debug.WriteLine(kardexLista);

            return new JsonResult { Data = kardexLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public class HistorialRecetasLista
        {
            public string medico { get; set; }
            public int? folio_rcta { get; set; }
            public string medicamento { get; set; }
            public string dosis { get; set; }
            public string indicaciones { get; set; }
            public DateTime fecha { get; set; }
            public string fecha2 { get; set; }
            public int? cantidadsurtida { get; set; }
        }


        [HttpGet]
        public ActionResult HistorialRecetas(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var fecha = DateTime.Now.AddYears(-1).ToString("yyyy-MM-ddT00:00:00.000");
                var fecha_correcta = DateTime.Parse(fecha);
                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == id
                                  select a).FirstOrDefault();

                Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
                Models.SERVMEDEntities4 db3 = new Models.SERVMEDEntities4();
                Models.SMDEVEntities28 db4 = new Models.SMDEVEntities28();

                var receta_resur_detalles = (from d in db3.Receta_Detalle
                                             join recetaExp in db3.Receta_Exp on d.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                             from receExp in reExp.DefaultIfEmpty()
                                             where receExp.Expediente == id
                                             //where receExp.folio_rc == null
                                             where receExp.Fecha >= fecha_correcta
                                             select new
                                             {
                                                 folio_rcta = d.Folio_Rcta,
                                                 codigo = d.Codigo,
                                                 medico = receExp.Medico,
                                                 dosis = d.Dosis,
                                                 indicaciones = d.Indicaciones,
                                                 fecha = receExp.Fecha,
                                                 cantidadsurtida = d.CantSurtida,
                                                 //medico_hu = receExp.medico_hu,
                                                 //meses_surtir = receExp.meses_surtir,
                                             })
                                            .OrderByDescending(g => g.fecha)
                                            .ToList();

                //System.Diagnostics.Debug.WriteLine(receta_resur_detalles);


                var results = new List<HistorialRecetasLista>();

                foreach (var item in receta_resur_detalles)
                {
                    var inventariofarmacia = (from q in db2.InventarioFarmacia
                                                  //join sustan in db2.Sustancia on q.Clave equals sustan.Clave into susX
                                                  //from susIn in susX.DefaultIfEmpty()
                                                  //where susIn.Consultorio != "0"
                                              where q.Clave == item.codigo
                                              //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                              select new
                                              {
                                                  Clave = q.Clave,
                                                  Descripcion_Sal = q.Descripcion_Sal,
                                                  Descripcion_Grupo = q.Descripcion_Grupo,
                                                  Presentacion = q.Presentacion,
                                                  Inv_Act = q.Inv_Act,
                                              })
                                               .OrderByDescending(g => g.Descripcion_Grupo)
                                               .FirstOrDefault();

                    var medicoNombre = "";

                    if(item.medico == "51000")
                    {
                        var rcthu = (from hu in db3.Receta_Exp
                                      where hu.Folio_Rcta == item.folio_rcta
                                      //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                      select new
                                      {
                                          medico_hu = hu.medico_hu,
                                          medico = hu.Medico
                                      })
                                   .FirstOrDefault();

                        medicoNombre = "<h6><b>" + rcthu.medico_hu + "</b></h6><span>" + rcthu.medico + "</span>";
                    }
                    else
                    {
                        var medico = (from m in db4.MEDICOS
                                      where m.Numero == item.medico
                                      //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                      select new
                                      {
                                          nombre = m.Nombre + " " + m.Apaterno + " " + m.Amaterno,
                                          numero = m.Numero,
                                      })
                                   .FirstOrDefault();

                        medicoNombre = "<h6><b>" + medico.nombre + "</b></h6><span>" + medico.numero + "</span>";
                    }

                    
                    if(inventariofarmacia != null)
                    {
                        var result = new HistorialRecetasLista
                        {
                            medico = medicoNombre,
                            //folio_rcta = item.folio_rcta,
                            medicamento = "<h6><b>" + inventariofarmacia.Descripcion_Sal + "</b></h6><span>" + inventariofarmacia.Descripcion_Grupo + "</span><br><span>" + inventariofarmacia.Presentacion + "</span>",
                            dosis = item.dosis,
                            indicaciones = item.indicaciones,
                            fecha = item.fecha,
                            cantidadsurtida = item.cantidadsurtida
                        };

                        results.Add(result);

                    }

                }





                var receta_resur_detalles2 = (from d in db2.receta_detalle_crn
                                              join recetaExp in db2.receta_exp_crn on d.folio_rc equals recetaExp.folio_rc into reExp
                                              from receExp in reExp.DefaultIfEmpty()
                                              where receExp.expediente == id
                                              where receExp.fecha_crea >= fecha_correcta
                                              select new
                                              {
                                                  folio_rc = d.folio_rc,
                                                  codigo = d.codigo,
                                                  medico = receExp.medico_crea,
                                                  dosis = d.dosis,
                                                  indicaciones = d.indicaciones,
                                                  fecha = receExp.fecha_crea,
                                                  cantidadsurtida = d.cantidad,
                                                  meses_surtir = receExp.meses_surtir,
                                              })
                                         .ToList();


                foreach (var item in receta_resur_detalles2)
                {
                    Models.SERVMEDEntities4 db6 = new Models.SERVMEDEntities4();


                    var inventariofarmacia = (from q in db6.InvFarm
                                                  join sustan in db6.Sustancia on q.Id_Sustancia equals sustan.Id into susX
                                                  from susIn in susX.DefaultIfEmpty()
                                              join sal in db6.Sal on susIn.Id_Sal equals sal.Id into salX
                                              from salIn in salX.DefaultIfEmpty()
                                              join grupo in db6.Grupo on susIn.Id_Grupo equals grupo.Id into grupoX
                                              from grupoIn in grupoX.DefaultIfEmpty()
                                                  //where susIn.Consultorio != "0"
                                              where susIn.Clave == item.codigo
                                              where q.InvFarmId == 81
                                              //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                              select new
                                              {
                                                  Clave = susIn.Clave,
                                                  Descripcion_Sal = salIn.Descripcion_Sal,
                                                  Descripcion_Grupo = grupoIn.Descripcion_Grupo,
                                                  Presentacion = susIn.Presentacion,
                                                  Inv_Act = q.Inv_Act,
                                              })
                                           .OrderByDescending(g => g.Descripcion_Grupo)
                                           .FirstOrDefault();

                    var medico2 = (from m in db4.MEDICOS
                                   where m.Numero == item.medico
                                   //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                   select new
                                   {
                                       nombre = m.Nombre + " " + m.Apaterno + " " + m.Amaterno,
                                       numero = m.Numero,
                                   })
                                   .FirstOrDefault();

                    if (inventariofarmacia != null)
                    {
                        var result2 = new HistorialRecetasLista
                        {
                            medico = "<h6><b>" + medico2.nombre + "</b></h6><span>" + medico2.numero + "</span>",
                            //folio_rcta = item.folio_rcta,
                            medicamento = "<h6><b>" + inventariofarmacia.Descripcion_Sal + "</b></h6><span>" + inventariofarmacia.Descripcion_Grupo + "</span><br><span>" + inventariofarmacia.Presentacion + "</span><br><span> <b> Meses a surtir:" + item.meses_surtir + "</b></span>",
                            dosis = item.dosis,
                            indicaciones = item.indicaciones,
                            fecha = item.fecha,
                            cantidadsurtida = item.cantidadsurtida
                        };

                        results.Add(result2);

                    }
                    //db.InventarioFarmacia.Add(result);
                }


                //System.Diagnostics.Debug.WriteLine(results);

                string json = JsonConvert.SerializeObject(results);
                string path = Server.MapPath("~/json/recetas/");
                System.IO.File.WriteAllText(path + "historial_recetas_" + id + ".json", json);


                if (dhabientes.NUMAFIL != "P72112")
                {
                    return View(dhabientes);
                }
                else
                {
                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                }

            }

        }


        public JsonResult HistorialRecetasList(string id)
        {

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var fecha = DateTime.Now.AddYears(-1).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var dhabientes = (from a in db2.DHABIENTES
                              where a.NUMEMP == id
                              select a).FirstOrDefault();

            var receta_resur_detalles = (from d in db.Receta_Detalle
                                         join recetaExp in db.Receta_Exp on d.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                         from receExp in reExp.DefaultIfEmpty()
                                         where receExp.Expediente == id
                                         //where receExp.folio_rc == null
                                         where receExp.Fecha >= fecha_correcta
                                         select new
                                         {
                                             folio_rcta = d.Folio_Rcta,
                                             codigo = d.Codigo,
                                             medico = receExp.Medico,
                                             dosis = d.Dosis,
                                             indicaciones = d.Indicaciones,
                                             fecha = receExp.Fecha,
                                             cantidadsurtida = d.CantSurtida,
                                             //medico_hu = receExp.medico_hu,
                                             //meses_surtir = receExp.meses_surtir,
                                         })
                                        .OrderByDescending(g => g.fecha)
                                        .ToList();

            //System.Diagnostics.Debug.WriteLine(receta_resur_detalles);


            var results = new List<HistorialRecetasLista>();

            foreach (var item in receta_resur_detalles)
            {
                //Antes del nuevo cuadro básico
                var fechaNCB = DateTime.Now.ToString("2022-04-04T00:00:00.000");
                var fechaNCBDT = DateTime.Parse(fechaNCB);

                //System.Diagnostics.Debug.WriteLine(fechaNCBDT);

                var inventariofarmacia = (from q in db2.InventarioFarmacia
                                          where q.Clave == item.codigo
                                          select new
                                          {
                                              Clave = q.Clave,
                                              Descripcion_Sal = q.Descripcion_Sal,
                                              Descripcion_Grupo = q.Descripcion_Grupo,
                                              Presentacion = q.Presentacion,
                                              Inv_Act = q.Inv_Act,
                                          })
                                           .OrderByDescending(g => g.Descripcion_Grupo)
                                           .FirstOrDefault();


                var res2 = (from a in db.Sustancia
                            join grupo in db.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                            from grupoIn in grupoX.DefaultIfEmpty()
                            where a.Clave == item.codigo
                            select new
                            {

                                Id = a.Id,
                                Clave = a.Clave,
                                descripcion_21 = a.descripcion_21,
                                Descripcion_Grupo = grupoIn.descripcion,

                            }).FirstOrDefault();


                var medicoNombre = "";

                if (item.medico == "51000")
                {
                    var rcthu = (from hu in db.Receta_Exp
                                 where hu.Folio_Rcta == item.folio_rcta
                                 //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                 select new
                                 {
                                     medico_hu = hu.medico_hu,
                                     medico = hu.Medico
                                 })
                               .FirstOrDefault();

                    medicoNombre = "<h6><b>" + rcthu.medico_hu + "</b></h6><span>" + rcthu.medico + "</span>";
                }
                else
                {
                    var medico = (from m in db2.MEDICOS
                                  where m.Numero == item.medico
                                  //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                  select new
                                  {
                                      nombre = m.Nombre + " " + m.Apaterno + " " + m.Amaterno,
                                      numero = m.Numero,
                                  })
                               .FirstOrDefault();

                    medicoNombre = "<h6><b>" + medico.nombre + "</b></h6><span>" + medico.numero + "</span>";
                }



                var medicamento = "";

                if (inventariofarmacia != null)
                {

                    if (item.fecha < fechaNCBDT)
                    {
                        medicamento = "<h6><b>" + inventariofarmacia.Descripcion_Sal + "</b></h6><span>" + inventariofarmacia.Descripcion_Grupo + "</span><br><span>" + inventariofarmacia.Presentacion + "</span>";

                    }
                    else
                    {
                        medicamento = "<h6><b>" + res2.descripcion_21 + "</b></h6><span>" + res2.Descripcion_Grupo + "</span>";

                    }

                }

                
                if(inventariofarmacia != null) { 

                    var result = new HistorialRecetasLista
                    {
                        medico = medicoNombre,
                        //folio_rcta = item.folio_rcta,
                        medicamento = medicamento,
                        dosis = item.dosis,
                        indicaciones = item.indicaciones,
                        fecha = item.fecha,
                        fecha2 = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                        cantidadsurtida = item.cantidadsurtida
                    };

                    results.Add(result);

                }


            }



            var receta_resur_detalles2 = (from d in db2.receta_detalle_crn
                                          join recetaExp in db2.receta_exp_crn on d.folio_rc equals recetaExp.folio_rc into reExp
                                          from receExp in reExp.DefaultIfEmpty()
                                          where receExp.expediente == id
                                          where receExp.fecha_crea >= fecha_correcta
                                          select new
                                          {
                                              folio_rc = d.folio_rc,
                                              codigo = d.codigo,
                                              medico = receExp.medico_crea,
                                              dosis = d.dosis,
                                              indicaciones = d.indicaciones,
                                              fecha = receExp.fecha_crea,
                                              cantidadsurtida = d.cantidad,
                                              meses_surtir = receExp.meses_surtir,
                                          })
                                     .ToList();


            foreach (var item in receta_resur_detalles2)
            {
                Models.SERVMEDEntities4 db6 = new Models.SERVMEDEntities4();


                var inventariofarmacia = (from q in db2.InventarioFarmacia
                                          where q.Clave == item.codigo
                                          select new
                                          {
                                              Clave = q.Clave,
                                              Descripcion_Sal = q.Descripcion_Sal,
                                              Descripcion_Grupo = q.Descripcion_Grupo,
                                              Presentacion = q.Presentacion,
                                              Inv_Act = q.Inv_Act,
                                          })
                                       .OrderByDescending(g => g.Descripcion_Grupo)
                                       .FirstOrDefault();

                var res2 = (from a in db.Sustancia
                            join grupo in db.grupo_21 on a.id_grupo_21 equals grupo.id into grupoX
                            from grupoIn in grupoX.DefaultIfEmpty()
                            where a.Clave == item.codigo
                            select new
                            {

                                Id = a.Id,
                                Clave = a.Clave,
                                descripcion_21 = a.descripcion_21,
                                Descripcion_Grupo = grupoIn.descripcion,

                            }).FirstOrDefault();

                var medico2 = (from m in db2.MEDICOS
                               where m.Numero == item.medico
                               //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                               select new
                               {
                                   nombre = m.Nombre + " " + m.Apaterno + " " + m.Amaterno,
                                   numero = m.Numero,
                               })
                               .FirstOrDefault();

                var fechaNCB2 = DateTime.Now.ToString("2022-04-11T00:00:00.000");
                var fechaNCBDT2 = DateTime.Parse(fechaNCB2);

                var medicamento = "";

                if (inventariofarmacia != null)
                {

                    if (item.fecha < fechaNCBDT2)
                    {
                        medicamento = "<h6><b>" + inventariofarmacia.Descripcion_Sal + "</b></h6><span>" + inventariofarmacia.Descripcion_Grupo + "</span><br><span>" + inventariofarmacia.Presentacion + "</span>";

                    }
                    else
                    {
                        medicamento = "<h6><b>" + res2.descripcion_21 + "</b></h6><span>" + res2.Descripcion_Grupo + "</span>";

                    }

                }

                if (inventariofarmacia != null)
                {

                    var result2 = new HistorialRecetasLista
                    {
                        medico = "<h6><b>" + medico2.nombre + "</b></h6><span>" + medico2.numero + "</span>",
                        //folio_rcta = item.folio_rcta,
                        //medicamento = "<h6><b>" + inventariofarmacia.Descripcion_Sal + "</b></h6><span>" + inventariofarmacia.Descripcion_Grupo + "</span><br><span>" + inventariofarmacia.Presentacion + "</span><br><span> <b> Meses a surtir:" + item.meses_surtir + "</b></span>",
                        medicamento = medicamento,
                        dosis = item.dosis,
                        indicaciones = item.indicaciones,
                        fecha = item.fecha,
                        fecha2 = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                        cantidadsurtida = item.cantidadsurtida
                    };

                    results.Add(result2);

                }
                //db.InventarioFarmacia.Add(result);
            }


            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult SanJeronimo()
        {
            if (User.IsInRole("Especialistas"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public class IntSanJeronimoLista
        {
            public int operatoriaid { get; set; }
            public string folio { get; set; }
            public string paciente { get; set; }
            public string estudios { get; set; }
            public string fecha_internamiento { get; set; }

        }

        public JsonResult InternadosSanJeronimo()
        {
            var fecha = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-ddT00:00:00.000");
            //var fecha_correcta = DateTime.Parse(fecha);
            //Models.SERVMEDEntities2 db = new Models.SERVMEDEntities2();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var nombreusuario = User.Identity.GetUserName();

            var orden_int = (from d in db.NotaOperatoria

                             join ordenInt in db.Orden_Int on d.OrdenFolio equals ordenInt.id_orden into ordenIntX
                             from ordenIntIn in ordenIntX.DefaultIfEmpty()

                             join dhabiente in db.DHABIENTES on ordenIntIn.numemp equals dhabiente.NUMEMP into dhabienteX
                             from dhabienteIn in dhabienteX.DefaultIfEmpty()

                             where ordenIntIn.medico == nombreusuario
                             where ordenIntIn.proveedor == "193"

                             select new
                             {
                                 operatoriaid = d.OperatoriaId,
                                 folio = ordenIntIn.folio,
                                 paciente = dhabienteIn.NOMBRES + " " + dhabienteIn.APATERNO + " " + dhabienteIn.AMATERNO,
                                 estudios = ordenIntIn.estudios,
                                 fecha_internamiento = ordenIntIn.fecha_internamiento
                                 //descripcion = q.DESCRIPCION,
                             }).ToList();

            var results1 = new List<IntSanJeronimoLista>();

            foreach (var orden in orden_int)
            {

                var resultado = new IntSanJeronimoLista
                {
                    operatoriaid = orden.operatoriaid,
                    folio = orden.folio,
                    estudios = orden.estudios,
                    paciente = orden.paciente,
                    fecha_internamiento = string.Format("{0:dddd, dd MMMM yyyy HH:mm}", orden.fecha_internamiento, new CultureInfo("es-ES"))
                    //Recetas = cita.Numero_Medico,
                    //Recetas = "",
                };

                results1.Add(resultado);

            }

            //System.Diagnostics.Debug.WriteLine(tipoConsulta);

            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        #region Prestaciones

        public ActionResult Prestaciones(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                
                Models.SMDEVEntities33 dbSol = new Models.SMDEVEntities33();
                var fechaSol = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-ddT00:00:00.000");
                var fechaSolDT = DateTime.Parse(fechaSol);
                var fechaHoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fechaHoyDT = DateTime.Parse(fechaHoy);

                /*
                var solSer = (from a in dbSol.SolicitudPrestaciones
                              where a.num_exp == id
                              where a.fecha >= fechaSolDT
                              where a.fecha != fechaHoyDT
                              //where a.id_servicio != 2
                              select a).FirstOrDefault();
                */

                //System.Diagnostics.Debug.WriteLine(solSer);

                //if (solSer == null || User.IsInRole("SolicitudDental"))
                //{
                

                    if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                    {


                        var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                        var fechaDT = DateTime.Parse(fecha2);
                        var usuario = User.Identity.GetUserName();
                        Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                        string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                        var result = db4.Database.SqlQuery<Citas>(query);
                        var res = result.FirstOrDefault();

                        //System.Diagnostics.Debug.WriteLine(res);

                        if (res != null)
                        {
                            //Si es presencial
                            if (res.tipo == "11")
                            {
                                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                //Revisar si ya hizo el SOAP
                                var exp = (from a in db.expediente
                                           where a.numemp == id
                                           where a.hora_termino != null
                                           where a.fecha == fechaDT
                                           where a.medico == usuario
                                           select a).Count();

                                if (User.IsInRole("SolicitudDental")) {

                                    return View(dhabientes);
                                }
                                else
                                {
                                    if(exp != 0)
                                    {
                                        if (User.IsInRole("PrestacionSolicitud") || User.Identity.Name.Substring(0, 2) == "05" || User.Identity.Name.Substring(0, 2) == "29" || User.Identity.Name.Substring(0, 2) == "14" || User.Identity.Name.Substring(0, 2) == "60" || User.Identity.Name.Substring(0, 2) == "23" || User.Identity.Name.Substring(0, 2) == "09" || User.Identity.Name.Substring(0, 2) == "63")
                                        {

                                            if (dhabientes.NUMAFIL != "P72112")
                                            {
                                                return View(dhabientes);
                                            }
                                            else
                                            {
                                                TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                                return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                            }

                                        }
                                        else
                                        {
                                            return RedirectToAction("Index", "Home");
                                        }
                                
                                    }
                                    else
                                    {
                                        TempData["message_recetas_warning"] = "Debes terminar una nota médica antes de crear una solicitud de servicio";
                                        return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                    }

                                }



                                }
                            //Si no es presencial entonces es telefonica
                            else
                            {

                                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                //Revisar si ya hizo el SOAP
                                var exp = (from a in db.expediente
                                           where a.numemp == id
                                           where a.hora_termino != null
                                           where a.fecha == fechaDT
                                           where a.medico == usuario
                                           select a).Count();

                                if (exp != 0)
                                {
                                    if (User.IsInRole("PrestacionSolicitud") || User.Identity.Name.Substring(0, 2) == "05" || User.Identity.Name.Substring(0, 2) == "29" || User.Identity.Name.Substring(0, 2) == "14" || User.Identity.Name.Substring(0, 2) == "60" || User.Identity.Name.Substring(0, 2) == "23" || User.Identity.Name.Substring(0, 2) == "09" || User.Identity.Name.Substring(0, 2) == "63")
                                    {
                                        if (dhabientes.NUMAFIL != "P72112")
                                        {
                                            return View(dhabientes);
                                        }
                                        else
                                        {
                                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                            return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                        }
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }

                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Debes terminar una nota médica antes de crear una solicitud de servicio";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }

                            }

                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    else
                    {
                        if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                        {
                            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                            var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                            var fechaDT = DateTime.Parse(fecha2);
                            var usuario = User.Identity.GetUserName();

                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            //Revisar si ya hizo el SOAP
                            var exp = (from a in db.expediente
                                       where a.numemp == id
                                       where a.hora_termino != null
                                       where a.fecha == fechaDT
                                       where a.medico == usuario
                                       select a).Count();

                                if (User.IsInRole("SolicitudDental"))
                                {

                                    return View(dhabientes);
                                }
                                else
                                {



                                    if (exp != 0)
                                    {
                                        if (User.IsInRole("PrestacionSolicitud") || User.Identity.Name.Substring(0, 2) == "05" || User.Identity.Name.Substring(0, 2) == "29" || User.Identity.Name.Substring(0, 2) == "14" || User.Identity.Name.Substring(0, 2) == "60" || User.Identity.Name.Substring(0, 2) == "23" || User.Identity.Name.Substring(0, 2) == "09" || User.Identity.Name.Substring(0, 2) == "63")
                                        {
                                            if (dhabientes.NUMAFIL != "P72112")
                                            {
                                                return View(dhabientes);
                                            }
                                            else
                                            {
                                                TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                                return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                            }
                                        }
                                        else
                                        {
                                            return RedirectToAction("Index", "Home");
                                        }
                                    }
                                    else
                                    {
                                        TempData["message_recetas_warning"] = "Debes terminar una nota médica antes de crear una solicitud de servicio";
                                        return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                    }
                                }

                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }

                
                /*}
                
                else
                {
                    TempData["message_warning"] = "Ya existe una solicitud de menos de un mes generada a este paciente";
                    return RedirectToAction("VerPrestaciones/" + id, "ServiciosMedicos");
                }*/
                
            }
        }


        public ActionResult GuardarPrestacion(int servicio, int subservicio, int subservicio2, int subservicio3, int proveedor, string num_exp, string lado, string opcion, string dedo, string lado2, string opcion2, string dedo2, string lado3, string opcion3, string dedo3, bool dx1, bool dx2, bool dx3, int sesiones, string nota, string justificacion, string diagnosticodental)
        //public ActionResult GuardarPrestacion(int servicio, int subservicio, int subservicio2, int subservicio3, string num_exp, string lado, string opcion, string dedo, string lado2, string opcion2, string dedo2, string lado3, string opcion3, string dedo3, bool dx1, bool dx2, bool dx3, int sesiones, string nota, string justificacion)
        {

            if (!User.IsInRole("SolicitudDental"))
            {
                if (dx1 == false && dx2 == false && dx3 == false)
                {
                    TempData["message_error"] = "Debes de seleccionar al menos un diagnóstico";
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
        //System.Diagnostics.Debug.WriteLine(subservicio2);
        //Buscar si hay hecho un SOAP
        Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var usuario = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;

            var exp2 = (from a in db.expediente
                       where a.numemp == num_exp
                       where a.hora_termino != null
                       where a.fecha == fechaDT
                       where a.medico == usuario
                       select a).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(lado);

            

            SolicitudPrestaciones sollpres = new SolicitudPrestaciones();
            sollpres.id_servicio = servicio;
            sollpres.id_subservicio = subservicio;
            
            if(subservicio2 != 0)
            {
                sollpres.id_subservicio2 = subservicio2;
            }
            if (subservicio3 != 0)
            {
                sollpres.id_subservicio3 = subservicio3;
            }

            sollpres.clave_proveedor = proveedor;

            if (opcion != null)
            {
                sollpres.descripcion = opcion + " " + lado + " " + dedo;
            }
            else
            {
                if (lado != null)
                {
                    sollpres.descripcion = lado;
                }
            }

            if (opcion2 != null)
            {
                sollpres.descripcion2 = opcion2 + " " + lado2 + " " + dedo2;
            }
            else
            {
                if (lado2 != null)
                {
                    sollpres.descripcion2 = lado2;
                }
            }

            if (opcion3 != null)
            {
                sollpres.descripcion3 = opcion3 + " " + lado3 + " " + dedo3;
            }
            else
            {
                if (lado3 != null)
                {
                    sollpres.descripcion3 = lado3;
                }
            }

            sollpres.num_exp = num_exp;
            sollpres.medico = User.Identity.GetUserName();
            sollpres.fecha = fechaDT;
            sollpres.sesiones = sesiones;
            sollpres.nota = nota;
            sollpres.ip_realiza = ip_realiza;
            sollpres.estado = 1;


            if(sesiones == 0) {

                //dental diagnosticos
                sollpres.diagnostico1 = diagnosticodental;

            }
            else
            {
                if(dx1 == true)
                {
                    sollpres.diagnostico1 = exp2.diagnostico;
                }

                if (dx2 == true)
                {
                    sollpres.diagnostico2 = exp2.diagnostico2;
                }

                if (dx3 == true)
                {
                    sollpres.diagnostico3 = exp2.diagnostico3;
                }
            }

            /*if (adomicilio == true)
            {
                sollpres.adomicilio = 1;
                sollpres.justificacion = justificacion;
            }*/

            db.SolicitudPrestaciones.Add(sollpres);
            db.SaveChanges();
            

            var lastSolPre = (from s in db.SolicitudPrestaciones
                              join prov in db.prov_subrrogados on s.clave_proveedor.ToString() equals prov.clave into provX
                              from provIn in provX.DefaultIfEmpty()
                              join serv in db.prestaciones_servicios on s.id_servicio equals serv.id into servX
                              from servIn in servX.DefaultIfEmpty()
                              join subserv in db.prestaciones_subservicios on s.id_subservicio equals subserv.id into subservX
                              from subservIn in subservX.DefaultIfEmpty()
                              join subserv2 in db.prestaciones_subservicios on s.id_subservicio2 equals subserv2.id into subservX2
                              from subservIn2 in subservX2.DefaultIfEmpty()
                              join subserv3 in db.prestaciones_subservicios on s.id_subservicio3 equals subserv3.id into subservX3
                              from subservIn3 in subservX3.DefaultIfEmpty()
                              join diagnosticoNombre in db.DIAGNOSTICOS on s.diagnostico1 equals diagnosticoNombre.Clave into diaX1
                              from diaIn1 in diaX1.DefaultIfEmpty()
                              join diagnostico2Nombre in db.DIAGNOSTICOS on s.diagnostico2 equals diagnostico2Nombre.Clave into diaX2
                              from diaIn2 in diaX2.DefaultIfEmpty()
                              join diagnostico3Nombre in db.DIAGNOSTICOS on s.diagnostico3 equals diagnostico3Nombre.Clave into diaX3
                              from diaIn3 in diaX3.DefaultIfEmpty()
                              join medico in db.MEDICOS on s.medico equals medico.Numero into medicoX
                              from medicoIn in medicoX.DefaultIfEmpty()
                              where s.num_exp == num_exp
                              where s.medico == usuario
                              where s.fecha == fechaDT
                              orderby s.id descending
                              select new
                              {
                                  id = s.id,
                                  clave_proveedor = s.clave_proveedor,
                                  fecha = s.fecha,
                                  proveedor_nombre = provIn.nombre,
                                  proveedor_direccion = provIn.direccion + ", Colonia " + provIn.colonia,
                                  proveedor_telefono = provIn.telefono1,
                                  servicio = servIn.servicio,
                                  subservicio = subservIn.subservicio,
                                  subservicio2 = subservIn2.subservicio,
                                  subservicio3 = subservIn3.subservicio,
                                  descripcion = s.descripcion,
                                  descripcion2 = s.descripcion2,
                                  descripcion3 = s.descripcion3,
                                  sesiones = s.sesiones,
                                  nota = s.nota,
                                  diagnostico1 = diaIn1.DescCompleta,
                                  diagnostico2 = diaIn2.DescCompleta,
                                  diagnostico3 = diaIn3.DescCompleta,
                                  adomicilio = s.adomicilio,
                                  justificacion = s.justificacion,
                                  medico = medicoIn.Titulo + " " + medicoIn.Nombre + " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                                  cedula = medicoIn.Cedula,
                                  maps = provIn.maps,
                                  croquis = provIn.croquis,

                              })
                              .OrderByDescending(g => g.id)
                              .FirstOrDefault();

            if(lastSolPre != null)
            {
                //Poner el folio
                var servicioTxt = lastSolPre.servicio.Substring(0, 3); 

                db.Database.ExecuteSqlCommand("UPDATE SolicitudPrestaciones SET folio = 'SS-"+ servicioTxt + lastSolPre.id  + "' WHERE id='" + lastSolPre.id + "';");

                //System.Diagnostics.Debug.WriteLine(lastSolPre.clave_proveedor);

                if (lastSolPre.clave_proveedor == 29)
                {
                    var proveString = (from p in db.prov_subrrogados
                                       where p.clave == "029"
                                       select new
                                       {
                                           proveedor_nombre = p.nombre,
                                           proveedor_direccion = p.direccion + ", Colonia " + p.colonia,
                                           proveedor_telefono = p.telefono1,
                                           maps = p.maps,
                                           croquis = p.croquis,
                                       }).FirstOrDefault();

                    TempData["proveedor_nombre"] = proveString.proveedor_nombre;
                    TempData["proveedor_direccion"] = proveString.proveedor_direccion;
                    TempData["proveedor_telefono"] = proveString.proveedor_telefono;
                    TempData["proveedor_telefono"] = proveString.proveedor_telefono;
                    TempData["maps"] = proveString.maps;
                    TempData["croquis"] = proveString.croquis;

                }
                else
                {
                    TempData["proveedor_nombre"] = lastSolPre.proveedor_nombre;
                    TempData["proveedor_direccion"] = lastSolPre.proveedor_direccion;
                    TempData["proveedor_telefono"] = lastSolPre.proveedor_telefono;
                    TempData["maps"] = lastSolPre.maps;
                    TempData["croquis"] = lastSolPre.croquis;
                }

                TempData["servicio"] = lastSolPre.servicio;



                //Primer subservicio
                if (lastSolPre.descripcion != null) {
                    TempData["subservicio"] = lastSolPre.subservicio + " " + lastSolPre.descripcion;
                }
                else
                {
                    TempData["subservicio"] = lastSolPre.subservicio;
                }


                //Segundo subservicio
                if (lastSolPre.descripcion2 != null)
                {
                    TempData["subservicio2"] = lastSolPre.subservicio2 + " " + lastSolPre.descripcion2;
                }
                else
                {
                    TempData["subservicio2"] = lastSolPre.subservicio2;
                }


                //Tercer subservicio
                if (lastSolPre.descripcion3 != null)
                {
                    TempData["subservicio3"] = lastSolPre.subservicio3 + " " + lastSolPre.descripcion3;
                }
                else
                {
                    TempData["subservicio3"] = lastSolPre.subservicio3;
                }



                if (lastSolPre.diagnostico1 != null)
                {
                    TempData["diagnostico1"] = lastSolPre.diagnostico1;
                }

                if (lastSolPre.diagnostico2 != null)
                {
                    TempData["diagnostico2"] = lastSolPre.diagnostico2;
                }

                if (lastSolPre.diagnostico3 != null)
                {
                    TempData["diagnostico3"] = lastSolPre.diagnostico3;
                }

                if (lastSolPre.adomicilio != null)
                {
                    TempData["adomicilio"] = "Si";
                    TempData["justificacion"] = lastSolPre.justificacion;
                }

                if(lastSolPre.sesiones != 0)
                {
                    TempData["sesiones"] = lastSolPre.sesiones;
                }
                TempData["nota"] = lastSolPre.nota;
                TempData["medico"] = lastSolPre.medico;
                TempData["cedula"] = lastSolPre.cedula;
                TempData["folio"] = "SS-"+ servicioTxt + lastSolPre.id;
                TempData["message_success"] = "Solicitud de servicio generada con éxito";
                TempData["fecha"] =  string.Format("{0:dddd, dd MMMM yyyy}", lastSolPre.fecha, new CultureInfo("es-ES"));
                //System.Diagnostics.Debug.WriteLine(TempData["message_success"]);
                //return RedirectToAction("Prestaciones/" + num_exp, "ServiciosMedicos");
                return Redirect(Request.UrlReferrer.ToString());

            }
            else
            {
                TempData["message_error"] = "Algo salio mal, comunicate al departamento de sistemas";
                return Redirect(Request.UrlReferrer.ToString());
            }
        }


        public JsonResult PrestacionesServicios()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            if (User.IsInRole("SolicitudDental"))
            {
                var preSer = (from p in db.prestaciones_servicios
                              //where p.estado == 1
                              select new
                              {
                                  id = p.id,
                                  servicio = p.servicio
                              }).ToList();

                return new JsonResult { Data = preSer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var preSer = (from p in db.prestaciones_servicios
                              where p.estado == 1
                              select new
                              {
                                  id = p.id,
                                  servicio = p.servicio
                              }).ToList();

                return new JsonResult { Data = preSer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

                

            
        }


        public JsonResult PrestacionesSubservicios(int servicio)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            if (User.IsInRole("SolicitudDental"))
            {

                var preSubSer = (from p in db.prestaciones_subservicios
                                 //where p.estado == 1
                                 where p.id_servicio == servicio
                                 select new
                                 {
                                     id = p.id,
                                     subservicio = p.subservicio
                                 }).ToList();

                return new JsonResult { Data = preSubSer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                var preSubSer = (from p in db.prestaciones_subservicios
                                 where p.estado == 1
                                 where p.id_servicio == servicio
                                 select new
                                 {
                                     id = p.id,
                                     subservicio = p.subservicio
                                 }).ToList();

                return new JsonResult { Data = preSubSer, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            
        }


        public JsonResult PrestacionesProveedores(int subservicio, int adomicilio)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            //System.Diagnostics.Debug.WriteLine(adomicilio);

            if(adomicilio == 1)
            {
                var proveedores = (from p in db.proveedor_subservicio
                                   join prov in db.prov_subrrogados on p.clave_proveedor equals prov.clave into provX
                                   from provIn in provX.DefaultIfEmpty()
                                   where p.id_subservicio == subservicio
                                   where provIn.adomicilio == 1
                                   select new
                                   {
                                       clave = provIn.clave,
                                       proveedor = provIn.nombre,
                                       direccion = provIn.direccion
                                   }).ToList();

                return new JsonResult { Data = proveedores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                var proveedores = (from p in db.proveedor_subservicio
                                   join prov in db.prov_subrrogados on p.clave_proveedor equals prov.clave into provX
                                   from provIn in provX.DefaultIfEmpty()
                                   where p.id_subservicio == subservicio
                                   select new
                                   {
                                       clave = provIn.clave,
                                       proveedor = provIn.nombre,
                                       direccion = provIn.direccion
                                   }).ToList();

                return new JsonResult { Data = proveedores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



            }


        }

        public JsonResult DetallesFiltroPrestaciones(string folio)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var presDts = (from q in db.SolicitudPrestaciones
                           where q.folio == folio
                           join dhab in db.DHABIENTES on q.num_exp equals dhab.NUMEMP into dhabX
                           from dhabIn in dhabX.DefaultIfEmpty()
                           join medicos in db.MEDICOS on q.medico equals medicos.Numero into medicosX
                           from medicosIn in medicosX.DefaultIfEmpty()
                           join serv in db.prestaciones_servicios on q.id_servicio equals serv.id into servX
                           from servIn in servX.DefaultIfEmpty()
                           join subserv in db.prestaciones_subservicios on q.id_subservicio equals subserv.id into subservX
                           from subservIn in subservX.DefaultIfEmpty()
                           join subserv2 in db.prestaciones_subservicios on q.id_subservicio2 equals subserv2.id into subservX2
                           from subservIn2 in subservX2.DefaultIfEmpty()
                           join subserv3 in db.prestaciones_subservicios on q.id_subservicio3 equals subserv3.id into subservX3
                           from subservIn3 in subservX3.DefaultIfEmpty()
                           join provee in db.prov_subrrogados on q.clave_proveedor.ToString() equals provee.clave into proveeX
                           from proveeIn in proveeX.DefaultIfEmpty()
                           join diagnosticoNombre in db.DIAGNOSTICOS on q.diagnostico1 equals diagnosticoNombre.Clave into diaX1
                           from diaIn1 in diaX1.DefaultIfEmpty()
                           join diagnostico2Nombre in db.DIAGNOSTICOS on q.diagnostico2 equals diagnostico2Nombre.Clave into diaX2
                           from diaIn2 in diaX2.DefaultIfEmpty()
                           join diagnostico3Nombre in db.DIAGNOSTICOS on q.diagnostico3 equals diagnostico3Nombre.Clave into diaX3
                           from diaIn3 in diaX3.DefaultIfEmpty()
                           select new
                           {
                               folio = q.folio,
                               numexp = q.num_exp,
                               paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                               fnac = dhabIn.FNAC,
                               sexo = dhabIn.SEXO,
                               fvigencia = dhabIn.FVIGENCIA,
                               clave_proveedor = q.clave_proveedor,
                               proveedor_nombre = proveeIn.nombre,
                               proveedor_telefono = proveeIn.telefono1,
                               proveedor_direccion = proveeIn.direccion,
                               medico = medicosIn.Titulo + " " +  medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                               cedula = medicosIn.Cedula,
                               servicio = servIn.servicio,
                               subservicio = subservIn.subservicio,
                               subservicio2 = subservIn2.subservicio,
                               subservicio3 = subservIn3.subservicio,
                               descripcion = q.descripcion,
                               descripcion2 = q.descripcion2,
                               descripcion3 = q.descripcion3,
                               sesiones = q.sesiones,
                               nota = q.nota,
                               diagnostico1 = diaIn1.DesCorta,
                               diagnostico2 = diaIn2.DesCorta,
                               diagnostico3 = diaIn3.DesCorta,
                               fecha = q.fecha,
                               id = q.id,
                               adomicilio = q.adomicilio,
                               justificacion = q.justificacion,
                               nota_filtro = q.nota_filtro,
                               estado = q.estado,
                           })
                              .OrderByDescending(g => g.fecha)
                              .FirstOrDefault();

            var today = DateTime.Today;
            DateTime fnac = (DateTime)presDts.fnac;
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


            var subservicio = "";
            if (presDts.descripcion != null)
            {
                subservicio = presDts.subservicio + " " + presDts.descripcion;
            }
            else
            {
                subservicio = presDts.subservicio;
            }


            var subservicio2 = "";
            if (presDts.descripcion2 != null)
            {
                subservicio2 = presDts.subservicio2 + " " + presDts.descripcion2;
            }
            else
            {
                subservicio2 = presDts.subservicio2;
            }


            var subservicio3 = "";
            if (presDts.descripcion3 != null)
            {
                subservicio3 = presDts.subservicio3 + " " + presDts.descripcion3;
            }
            else
            {
                subservicio3 = presDts.subservicio3;
            }

            var proveedor = "";
            var proveedor_nombre = "";
            var proveedor_direccion = "";
            var proveedor_telefono = "";

            if (presDts.clave_proveedor != null)
            {

                if (presDts.clave_proveedor == 29)
                {
                    var proveString = (from p in db.prov_subrrogados
                                       where p.clave == "029"
                                       select new
                                       {
                                           proveedor_nombre = p.nombre,
                                           proveedor_direccion = p.direccion,
                                           proveedor_telefono = p.telefono1
                                       }).FirstOrDefault();

                    proveedor_nombre = proveString.proveedor_nombre;
                    proveedor_direccion = proveString.proveedor_direccion;
                    proveedor_telefono = proveString.proveedor_telefono;

                }
                else
                {
                    proveedor_nombre = presDts.proveedor_nombre;
                    proveedor_direccion = presDts.proveedor_direccion;
                    proveedor_telefono = presDts.proveedor_telefono;
                }


                proveedor = "<div class='col-12 p-2'>" +
                                    "<div class='linea2 mt-3 mb-3'></div>" +
                                    "<h5 class='mt-2'><b>Proveedor</b></h5>" +
                                "</div>" +
                                "<div class='col-12 p-2'>" +
                                     "<h6 class='m-0'><b>Proveedor:</b>" + proveedor_nombre + "</h6>" +
                                     "<h6 class='m-0'><b>Dirección:</b>" + proveedor_direccion + "</h6>" +
                                     "<h6 class='m-0'><b>Teléfono:</b>" + proveedor_telefono + "</h6>" +
                                     presDts.adomicilio +
                                "</div>";


            }
            else
            {
                proveedor = "<div class='col-12 p-2'>" +
                                    "<div class='linea2 mt-3 mb-3'></div>" +
                                    "<h5 class='mt-2'><b>Proveedor</b></h5>" +
                                "</div>" +
                                "<div class='col-12 p-2'>" +
                                     "<h6 class='m-0'><b>No se ha registrado el proveedor</b></h6>" +
                                "</div>";
            }


            var subservicios = "";
            //System.Diagnostics.Debug.WriteLine(subservicio3);

            if ( subservicio3 != null)
            {
                //System.Diagnostics.Debug.WriteLine(subservicio3);
                subservicios = subservicio + ", " + subservicio2 + ", " + subservicio3;
            }
            else
            {
                if (subservicio2 != null)
                {
                    subservicios = subservicio + ", " + subservicio2;
                }
                else
                {
                    subservicios = subservicio;
               }
            }

            var claveProv = "";

            if(presDts.clave_proveedor == 29)
            {
                claveProv = "029";
            }
            else
            {
                claveProv = presDts.clave_proveedor.ToString();
            }

            result = new
            {
                folio = presDts.folio,
                numexp = presDts.numexp,
                paciente = presDts.paciente,
                fnac = string.Format("{0:yyyy/MM/dd}", presDts.fnac),
                sexo = presDts.sexo,
                proveedor = proveedor,
                clave_proveedor = claveProv,
                proveedor_nombre = proveedor_nombre,
                proveedor_telefono = proveedor_telefono,
                proveedor_direccion = proveedor_direccion,
                medico = presDts.medico,
                cedula = presDts.cedula,
                servicio = presDts.servicio,
                subservicios = subservicios,
                subservicio = subservicio,
                subservicio2 = subservicio2,
                subservicio3 = subservicio3,
                sesiones = presDts.sesiones,
                nota = presDts.nota,
                diagnostico1 = presDts.diagnostico1,
                diagnostico2 = presDts.diagnostico2,
                diagnostico3 = presDts.diagnostico3,
                fecha = presDts.fecha,
                fecha2 = string.Format("{0:dddd, dd MMMM yyyy}", presDts.fecha),
                fecha_vigencia = string.Format("{0:MMMM yyyy}", presDts.fvigencia),
                id = presDts.id,
                edad = Years,
                adomicilio = presDts.adomicilio,
                justificacion = presDts.justificacion,
                nota_filtro = presDts.nota_filtro,
                estado = presDts.estado,
            };


            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult PresProveedores()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            //System.Diagnostics.Debug.WriteLine(adomicilio);
            var proveedores = (from p in db.proveedor_subservicio
                               join prov in db.prov_subrrogados on p.clave_proveedor equals prov.clave into provX
                               from provIn in provX.DefaultIfEmpty()
                                   //where p.id_subservicio == subservicio
                                   //where provIn.adomicilio == 1
                               select new
                               {
                                   clave = provIn.clave,
                                   proveedor = provIn.nombre,
                                   direccion = provIn.direccion
                               })
                               .GroupBy(p => new
                               {
                                   p.clave,
                                   p.proveedor,
                                   p.direccion,
                               })
                               .Select(g => new
                               {
                                   clave = g.Key.clave,
                                   proveedor = g.Key.proveedor,
                                   direccion = g.Key.direccion,
                               })
                               .ToList();

            return new JsonResult { Data = proveedores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }



        public ActionResult VerPrestaciones(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {

                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            //SolicitudLaboratorioExp labExpediente = new SolicitudLaboratorioExp();
                            //labExpediente.DerechoHabiente = dhabiente;
                            //labExpediente.LabExpediente = resultLabExp.labexp;

                            if (User.IsInRole("PrestacionSolicitud") || User.Identity.Name.Substring(0, 2) == "05" || User.Identity.Name.Substring(0, 2) == "29" || User.Identity.Name.Substring(0, 2) == "14" || User.Identity.Name.Substring(0, 2) == "60" || User.Identity.Name.Substring(0, 2) == "23" || User.Identity.Name.Substring(0, 2) == "09" || User.Identity.Name.Substring(0, 2) == "63")
                            {
                                return View(dhabientes);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }

                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {

                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();


                            if (User.IsInRole("PrestacionSolicitud") || User.Identity.Name.Substring(0, 2) == "05" || User.Identity.Name.Substring(0, 2) == "29" || User.Identity.Name.Substring(0, 2) == "14" || User.Identity.Name.Substring(0, 2) == "60" || User.Identity.Name.Substring(0, 2) == "23" || User.Identity.Name.Substring(0, 2) == "09" || User.Identity.Name.Substring(0, 2) == "63")
                            {
                                return View(dhabientes);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {
                        Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();

                        if (User.IsInRole("PrestacionSolicitud") || User.Identity.Name.Substring(0, 2) == "05" || User.Identity.Name.Substring(0, 2) == "29" || User.Identity.Name.Substring(0, 2) == "14" || User.Identity.Name.Substring(0, 2) == "60" || User.Identity.Name.Substring(0, 2) == "23" || User.Identity.Name.Substring(0, 2) == "09" || User.Identity.Name.Substring(0, 2) == "63")
                        {
                            return View(dhabientes);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

        }

        public ActionResult EditarPrestaciones(string numexp, int id)
        {
            /*Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var dhabiente = (from a in db.DHABIENTES
                             where a.NUMEMP == numexp
                             select a).FirstOrDefault();

            var solpres = (from a in db.SolicitudPrestaciones
                           where a.id == id
                           select a).FirstOrDefault();


            SolicitudPrestaciones prestaciones = new SolicitudPrestaciones();
            prestaciones.DerechoHabiente = dhabiente;
            prestaciones.SolPrestaciones = solpres;

            if (User.Identity.Name == "02101" || User.Identity.Name == "22017" || User.Identity.Name.Substring(0, 2) == "05" || User.Identity.Name.Substring(0, 2) == "29" || User.Identity.Name.Substring(0, 2) == "14" || User.Identity.Name.Substring(0, 2) == "60" || User.Identity.Name.Substring(0, 2) == "23" || User.Identity.Name.Substring(0, 2) == "09")
            {
                return View(prestaciones);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }*/
            return View();
        }

        public class LstPrestaciones
        {
            public string id { get; set; }
            public string medico { get; set; }
            public string servicio { get; set; }
            public string subservicio { get; set; }
            public string fecha { get; set; }
            public string fecha2 { get; set; }
            public string paciente { get; set; }
            public string info { get; set; }
            public string folio { get; set; }
            //public int id { get; set; }

        }

        public JsonResult ListaPrestaciones(string id)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            string usuario = User.Identity.GetUserName();

            //System.Diagnostics.Debug.WriteLine(id);

            var expediente = (from q in db.SolicitudPrestaciones
                              where q.num_exp == id
                              join medicos in db.MEDICOS on q.medico equals medicos.Numero into medicosX
                              from medicosIn in medicosX.DefaultIfEmpty()
                              join serv in db.prestaciones_servicios on q.id_servicio equals serv.id into servX
                              from servIn in servX.DefaultIfEmpty()
                              join subserv in db.prestaciones_subservicios on q.id_subservicio equals subserv.id into subservX
                              from subservIn in subservX.DefaultIfEmpty()
                              select new
                              {
                                  usuario = q.medico,
                                  medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  servicio = servIn.servicio,
                                  subservicio = subservIn.subservicio,
                                  fecha = q.fecha,
                                  id = q.id,
                              })
                              .OrderByDescending(g => g.fecha)
                              .ToList();


            var presLista = new List<LstPrestaciones>();

            foreach (var item in expediente)
            {
                //Revisar si la fecha es de hoy para que puedan editarla y eliminarla
                var habilitado = 0;
                if (item.fecha >= fechaDT && item.usuario == usuario)
                {
                    habilitado = 1;
                }

                var prestacion = new LstPrestaciones
                {
                    medico = item.medico,
                    servicio = item.servicio,
                    subservicio = item.subservicio,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                    id = item.id + "*" + habilitado,
                    fecha2 = string.Format("{0:yyyy-MM-dd}", item.fecha, new CultureInfo("es-ES")),
                };

                presLista.Add(prestacion);
            }

            return new JsonResult { Data = presLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        public JsonResult DetallesPrestaciones(int id)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var presDts = (from q in db.SolicitudPrestaciones
                              where q.id == id
                              join dhab in db.DHABIENTES on q.num_exp equals dhab.NUMEMP into dhabX
                              from dhabIn in dhabX.DefaultIfEmpty()
                              join medicos in db.MEDICOS on q.medico equals medicos.Numero into medicosX
                              from medicosIn in medicosX.DefaultIfEmpty()
                              join serv in db.prestaciones_servicios on q.id_servicio equals serv.id into servX
                              from servIn in servX.DefaultIfEmpty()
                              join subserv in db.prestaciones_subservicios on q.id_subservicio equals subserv.id into subservX
                              from subservIn in subservX.DefaultIfEmpty()
                              join subserv2 in db.prestaciones_subservicios on q.id_subservicio2 equals subserv2.id into subservX2
                              from subservIn2 in subservX2.DefaultIfEmpty()
                              join subserv3 in db.prestaciones_subservicios on q.id_subservicio3 equals subserv3.id into subservX3
                              from subservIn3 in subservX3.DefaultIfEmpty()
                              join provee in db.prov_subrrogados on q.clave_proveedor.ToString() equals provee.clave into proveeX
                              from proveeIn in proveeX.DefaultIfEmpty()
                              join diagnosticoNombre in db.DIAGNOSTICOS on q.diagnostico1 equals diagnosticoNombre.Clave into diaX1
                              from diaIn1 in diaX1.DefaultIfEmpty()
                              join diagnostico2Nombre in db.DIAGNOSTICOS on q.diagnostico2 equals diagnostico2Nombre.Clave into diaX2
                              from diaIn2 in diaX2.DefaultIfEmpty()
                              join diagnostico3Nombre in db.DIAGNOSTICOS on q.diagnostico3 equals diagnostico3Nombre.Clave into diaX3
                              from diaIn3 in diaX3.DefaultIfEmpty()
                              select new
                              {
                                  folio = q.folio,
                                  numexp = q.num_exp,
                                  paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                  fnac = dhabIn.FNAC,
                                  sexo = dhabIn.SEXO,
                                  clave_proveedor = q.clave_proveedor,
                                  proveedor_nombre = proveeIn.nombre,
                                  proveedor_telefono = proveeIn.telefono1,
                                  proveedor_direccion = proveeIn.direccion,
                                  medico = medicosIn.Titulo + " " + medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  cedula = medicosIn.Cedula,
                                  servicio = servIn.servicio,
                                  subservicio = subservIn.subservicio,
                                  subservicio2 = subservIn2.subservicio,
                                  subservicio3 = subservIn3.subservicio,
                                  descripcion = q.descripcion,
                                  descripcion2 = q.descripcion2,
                                  descripcion3 = q.descripcion3,
                                  sesiones = q.sesiones,
                                  nota = q.nota,
                                  diagnostico1 = diaIn1.DesCorta,
                                  diagnostico2 = diaIn2.DesCorta,
                                  diagnostico3 = diaIn3.DesCorta,
                                  fecha = q.fecha,
                                  id = q.id,
                                  adomicilio = q.adomicilio,
                                  justificacion = q.justificacion,
                              })
                              .OrderByDescending(g => g.fecha)
                              .FirstOrDefault();

            var today = DateTime.Today;
            DateTime fnac = (DateTime)presDts.fnac;
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


            var subservicio = "";
            if(presDts.descripcion != null)
            {
                subservicio = presDts.subservicio + " " + presDts.descripcion;
            }
            else
            {
                subservicio = presDts.subservicio;
            }


            var subservicio2 = "";
            if (presDts.descripcion2 != null)
            {
                subservicio2 = presDts.subservicio2 + " " + presDts.descripcion2;
            }
            else
            {
                subservicio2 = presDts.subservicio2;
            }


            var subservicio3 = "";
            if (presDts.descripcion3 != null)
            {
                subservicio3 = presDts.subservicio3 + " " + presDts.descripcion3;
            }
            else
            {
                subservicio3 = presDts.subservicio3;
            }

            var proveedor = "";
            var proveedor_nombre = "";
            var proveedor_direccion = "";
            var proveedor_telefono = "";

            if(presDts.clave_proveedor != null)
            {

                if (presDts.clave_proveedor == 29)
                {
                    var proveString = (from p in db.prov_subrrogados
                                       where p.clave == "029"
                                       select new
                                       {
                                           proveedor_nombre = p.nombre,
                                           proveedor_direccion = p.direccion,
                                           proveedor_telefono = p.telefono1
                                       }).FirstOrDefault();

                    proveedor_nombre = proveString.proveedor_nombre;
                    proveedor_direccion = proveString.proveedor_direccion;
                    proveedor_telefono = proveString.proveedor_telefono;

                }
                else
                {
                    proveedor_nombre = presDts.proveedor_nombre;
                    proveedor_direccion = presDts.proveedor_direccion;
                    proveedor_telefono = presDts.proveedor_telefono;
                }

                
                proveedor = "<div class='col-12 p-2'>" +
                                    "<div class='linea2 mt-3 mb-3'></div>" +
                                    "<h5 class='mt-2'><b>Proveedor</b></h5>" +
                                "</div>" +
                                "<div class='col-12 p-2'>" +
                                     "<h6 class='m-0'><b>Proveedor:</b>" + proveedor_nombre + "</h6>" +
                                     "<h6 class='m-0'><b>Dirección:</b>" + proveedor_direccion + "</h6>" +
                                     "<h6 class='m-0'><b>Teléfono:</b>" + proveedor_telefono + "</h6>" +
                                     presDts.adomicilio +
                                "</div>";
                

            }
            else
            {
                proveedor = "<div class='col-12 p-2'>" +
                                    "<div class='linea2 mt-3 mb-3'></div>" +
                                    "<h5 class='mt-2'><b>Proveedor</b></h5>" +
                                "</div>" +
                                "<div class='col-12 p-2'>" +
                                     "<h6 class='m-0'><b>No se ha registrado el proveedor</b></h6>" +
                                "</div>";
            }
            



            result = new
            {
                folio = presDts.folio,
                numexp = presDts.numexp,
                paciente = presDts.paciente,
                fnac = string.Format("{0:yyyy/MM/dd}", presDts.fnac),
                sexo = presDts.sexo,
                proveedor = proveedor,
                proveedor_nombre = proveedor_nombre,
                proveedor_telefono = proveedor_telefono,
                proveedor_direccion = proveedor_direccion,
                medico = presDts.medico,
                cedula = presDts.cedula,
                servicio = presDts.servicio,
                subservicio = subservicio,
                subservicio2 = subservicio2,
                subservicio3 = subservicio3,
                sesiones = presDts.sesiones,
                nota = presDts.nota,
                diagnostico1 = presDts.diagnostico1,
                diagnostico2 = presDts.diagnostico2,
                diagnostico3 = presDts.diagnostico3,
                fecha = presDts.fecha,
                fecha2 = string.Format("{0:dddd, dd MMMM yyyy}", presDts.fecha),
                id = presDts.id,
                edad = Years,
                adomicilio = presDts.adomicilio,
                justificacion = presDts.justificacion,
            };


            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult FiltroPrestaciones()
        {
            if (User.IsInRole("FiltroPrestaciones"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult GuardarFiltroPrestaciones(int id, int filtro, string nota_filtro, string proveedor)
        {
            //System.Diagnostics.Debug.WriteLine(proveedor);
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var usuario_filtro = User.Identity.GetUserName();
            var fecha_filtro = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");

            //db.Database.ExecuteSqlCommand("UPDATE SolicitudPrestaciones SET clave_proveedor = '" + proveedor + "', estado = " + filtro + ", nota_filtro = '" + nota_filtro + "', usuario_filtro = '" + usuario_filtro + "', fecha_filtro = '" + fecha_filtro + "' WHERE id='" + id + "' ");
            db.Database.ExecuteSqlCommand("UPDATE SolicitudPrestaciones SET estado = " + filtro + ", nota_filtro = '" + nota_filtro + "', usuario_filtro = '" + usuario_filtro + "', fecha_filtro = '" + fecha_filtro + "' WHERE id='" + id + "' ");
 

            var filtroTxt = "";
            if(filtro == 2)
            {
                filtroTxt = "Aceptada";
            }

            if (filtro == 3)
            {
                filtroTxt = "Rechazada";
            }

            if (filtro == 4)
            {
                filtroTxt = "Pendiente";
            }

            TempData["message_success"] = "Solicitud " + filtroTxt + " con éxito";
            return Redirect(Request.UrlReferrer.ToString());
        }

        public JsonResult RegistrosPrestaciones()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            string usuario = User.Identity.GetUserName();

            //System.Diagnostics.Debug.WriteLine(id);

            var expediente = (from q in db.SolicitudPrestaciones
                              //where q.num_exp == id
                              //where q.usuario_filtro == usuario
                              join dhab in db.DHABIENTES on q.num_exp equals dhab.NUMEMP into dhabX
                              from dhabIn in dhabX.DefaultIfEmpty()
                              join medico in db.MEDICOS on q.medico equals medico.Numero into medicoX
                              from medicoIn in medicoX.DefaultIfEmpty()
                              join provee in db.prov_subrrogados on q.clave_proveedor.ToString() equals provee.clave into proveeX
                              from proveeIn in proveeX.DefaultIfEmpty()
                              join serv in db.prestaciones_servicios on q.id_servicio equals serv.id into servX
                              from servIn in servX.DefaultIfEmpty()
                              join subserv in db.prestaciones_subservicios on q.id_subservicio equals subserv.id into subservX
                              from subservIn in subservX.DefaultIfEmpty()
                              join subserv2 in db.prestaciones_subservicios on q.id_subservicio2 equals subserv2.id into subservX2
                              from subservIn2 in subservX2.DefaultIfEmpty()
                              join subserv3 in db.prestaciones_subservicios on q.id_subservicio3 equals subserv3.id into subservX3
                              from subservIn3 in subservX3.DefaultIfEmpty()
                              select new
                              {
                                  paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                  medico = medicoIn.Titulo + " " +  medicoIn.Nombre + " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                                  proveedor_nombre = proveeIn.nombre,
                                  proveedor_direccion = proveeIn.direccion,
                                  proveedor_telefono = proveeIn.telefono1,
                                  fnac = dhabIn.FNAC,
                                  sexo = dhabIn.SEXO,
                                  fvigencia = dhabIn.FVIGENCIA,
                                  id = q.id,
                                  estado = q.estado,
                                  nota_filtro = q.nota_filtro,
                                  num_exp = q.num_exp,
                                  fecha_filtro = q.fecha_filtro,
                                  fecha = q.fecha,
                                  folio = q.folio,
                                  servicio = servIn.servicio,
                                  id_servicio = q.id_servicio,
                                  subservicio = subservIn.subservicio,
                                  id_subservicio = q.id_subservicio,
                                  subservicio2 = subservIn2.subservicio,
                                  id_subservicio2 = q.id_subservicio2,
                                  subservicio3 = subservIn3.subservicio,
                                  id_subservicio3 = q.id_subservicio3,
                                  descripcion = q.descripcion,
                                  descripcion2 = q.descripcion2,
                                  descripcion3 = q.descripcion3,
                                  adomicilio = q.adomicilio,
                                  justificacion = q.justificacion,
                                  nota = q.nota,
                              })
                              .ToList();


            var presLista = new List<LstPrestaciones>();

            foreach (var item in expediente)
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

                var proveedor = "";
                if (item.adomicilio != null)
                {
                    proveedor = "<span>" + item.proveedor_nombre + ", " + item.proveedor_direccion + ", " + item.proveedor_telefono + "</span><br>" + "<span>A domicilio: Si</span><br>" + "<span>Justificación: "+ item.justificacion + "</span>";
                }
                else
                {
                    proveedor = "<span>" +  item.proveedor_nombre + ", " + item.proveedor_direccion + ", " + item.proveedor_telefono + "</span>";
                }



                var prestacion = new LstPrestaciones
                {
                    folio = item.folio,
                    paciente = item.paciente,
                    medico = item.medico,
                    fecha = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha, new CultureInfo("es-ES")),
                    //id = item.id,
                    //info = item.num_exp + "*" + item.paciente + "*" + Years + " años con " + Months + " meses" + "*" + item.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", item.fvigencia) + "*" + item.estado + "*" + item.nota_filtro + "*" + item.id + "*" + item.folio + "*" + item.servicio + "*" + item.subservicio + " " + item.descripcion + "*" + item.subservicio2 + " " + item.descripcion2 + "*" + item.subservicio3 + " " + item.descripcion3 + "*" + proveedor + "*" + item.nota + "*" + item.id_servicio + "*" + item.id_subservicio + "*" + item.adomicilio,
                };

                presLista.Add(prestacion);
            }

            return new JsonResult { Data = presLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /*public JsonResult Proveedores(string search)
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();


            var proveedor = (from p in db.prov_subrrogados
                             where (p.nombre.Contains(search))
                             where p.clave == "195" || p.clave == "197" || p.clave == "218"
                             select new
                             {
                                 label = p.nombre,
                                 value = p.clave
                             }).ToList();


            return new JsonResult { Data = proveedor, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }*/

        #endregion


        public class EspecialidadesList
        {
            public string CLAVE { get; set; }
            public string DESCRIPCION { get; set; }

        }

        public JsonResult Especialidades()
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var especialidades = (from esp in db.ESPECIALIDADES
                                orderby esp.DESCRIPCION ascending
                                select esp).ToList();

            return new JsonResult { Data = especialidades, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult EspecialidadesSoap()
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var especialidades = (from esp in db.ESPECIALIDADES
                                  orderby esp.DESCRIPCION ascending
                                  select esp).ToList();

            var results1 = new List<EspecialidadesList>();

            foreach (var item in especialidades) {
                if (item.CLAVE != "91" && item.CLAVE != "48") { 
                    if (item.CLAVE == "40")
                    {
                        var clavemedico = User.Identity.GetUserName().Substring(0, 2);

                        if (User.IsInRole("Coordinador") || clavemedico == "13")
                        {
                            var resultado = new EspecialidadesList
                            {
                                CLAVE = item.CLAVE,
                                DESCRIPCION = item.DESCRIPCION,
                            };

                            results1.Add(resultado);
                        }
                    }
                    else
                    {
                        var resultado = new EspecialidadesList
                        {
                            CLAVE = item.CLAVE,
                            DESCRIPCION = item.DESCRIPCION,
                        };

                        results1.Add(resultado);
                    }
                }

                
            }


            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult InterconsultasSub(string especialidad)
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var intersub = (from inter in db.InterconsultasSub
                            where inter.clave_especialidad == especialidad
                            where inter.estado == 1
                            //orderby esp.DESCRIPCION ascending
                            select new
                            {
                                descripcion = inter.descripcion,
                                id = inter.id,

                            }).ToList();

            return new JsonResult { Data = intersub, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult InterconsultaMedico(int subcategoria)
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var intermed = (from intmed in db.InterconsultaMedico
                            join med in db.MEDICOS on intmed.medico equals med.Numero into medX
                            from medIn in medX.DefaultIfEmpty()
                            join user in db.AspNetUsers on intmed.medico equals user.UserName into userX
                            from userIn in userX.DefaultIfEmpty()
                            join unid in db.UnidadMedica on userIn.Unidad equals unid.id into unidX
                            from unidIn in unidX.DefaultIfEmpty()
                            join espe in db.ESPECIALIDADES on intmed.medico.Substring(0, 2) equals espe.CLAVE into espeX
                            from espeIn in espeX.DefaultIfEmpty()
                            where intmed.id_subcategoria == subcategoria
                            where intmed.estado == 1
                            select new
                            {
                                medico = medIn.Numero,
                                nombre = medIn.Nombre + " " + medIn.Apaterno + " " + medIn.Amaterno,
                                imprimir = intmed.imprimir,
                                unidad = unidIn.unidad,
                                direccion = unidIn.direccion,
                                telefono = unidIn.telefono,
                                ubicacion = unidIn.ubicacion,
                                especialidad = espeIn.DESCRIPCION,

                            }).ToList();

            


            return new JsonResult { Data = intermed, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Estudios()
        {
            /*obj.Images = Directory.EnumerateFiles("http://148.234.143.201/imgcli/gastro/62759100/")
                .Select(fn => "http://148.234.143.201/imgcli/gastro/62759100/" + Path.GetFileName(fn));
            var objfile = new DirectoryInfo("http://148.234.143.201/imgcli/gastro/62759100/");
            var file = objfile.GetFiles("*.*");*/

            //var url = "http://148.234.143.201";


            /*using (WebClient client = new WebClient())
            {
                client.DownloadFile(new Uri(url), @"G:\IMAGEN\gastro\62759100\62759100_16072010.jpg"); 
                // OR 
                client.DownloadFileAsync(new Uri(url), @"G:\IMAGEN\gastro\62759100\62759100_16072010.jpg");
            }*/

            /*using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData("http://148.234.143.201/imgcli/gastro/62759100/62759100_16072010.jpg");

                using (MemoryStream mem = new MemoryStream(data))
                {
                    using (var yourImage = Image.FromStream(mem))
                    {
                        // If you want it as Png
                        //yourImage.Save("path_to_your_file.png", ImageFormat.Png);

                        // If you want it as Jpeg
                        yourImage.Save("~/Imagenes/Estudios/62759100/path_to_your_file.jpg", ImageFormat.Jpeg);
                    }
                }

            }*/

            return View();
        }



        #region SolicitudLaboratorio

        public ActionResult SolicitudLaboratorioExp(int folio)
        {
            Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();
            var resultLabExp = (from labexp in db.Lab_exp
                                where (labexp.Folio_lab == folio)
                                select new { labexp }).FirstOrDefault();

            var resultlabDet = (from labDet in db.Lab_detalle
                                join labCat in db.lab_catalogo on labDet.id_servicio equals labCat.lab_id
                                where (labDet.Folio_lab == resultLabExp.labexp.Folio_lab)
                                orderby labDet.Folio_lab descending
                                select labCat).ToList();

            SolicitudLaboratorioExp labExpediente = new SolicitudLaboratorioExp();
            labExpediente.SysDatetimeNow = DateTime.Now;
            labExpediente.LabExpediente = resultLabExp.labexp;
            labExpediente.LabDetalle = resultlabDet;

            return View(labExpediente);

        }

        public ActionResult SolicitudLaboratorio(string expediente, string medico)
        {


            if (expediente == null || medico == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabientes = (from a in db.DHABIENTES
                              where a.NUMEMP == expediente
                              select a).FirstOrDefault();


            Models.SMDEVEntitiesSoLab db2 = new Models.SMDEVEntitiesSoLab();

            var listLabExpd = new Lab_Expediente();

            var resultLabExp = (from labexp in db2.Lab_exp
                                where (labexp.expediente == expediente)
                                && (labexp.medico_crea == medico)
                                orderby labexp.fecha_crea descending
                                select labexp).ToList();

            listLabExpd.DHabiente = dhabientes;
            listLabExpd.expediente = expediente;
            listLabExpd.LabExpediente = resultLabExp;
            return View(listLabExpd);
        }


        public ActionResult ConsultaSolicitudesLaboratorio(string expediente, string medico)
        {
            Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();

            var listLabExpd = new Lab_Expediente();

            var resultLabExp = (from labexp in db.Lab_exp
                                where (labexp.expediente == expediente)
                                && (labexp.medico_crea == medico)
                                orderby labexp.fecha_crea descending
                                select labexp).ToList();

            listLabExpd.expediente = expediente;
            listLabExpd.LabExpediente = resultLabExp;
            return View(listLabExpd);
        }

        public JsonResult GetHistorialLab(string expediente)
        {

            Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();

            var lastmonth = DateTime.Today.AddMonths(-1);

            var resultlabDet = (from labDet in db.Lab_detalle
                                join labCat in db.lab_catalogo on labDet.id_servicio equals labCat.lab_id
                                join labexp in db.Lab_exp on labDet.Folio_lab equals labexp.Folio_lab
                                join med in db.MEDICOS on labexp.medico_crea equals med.Numero
                                where (labexp.expediente == expediente)
                                orderby labexp.fecha_crea descending
                                select new { labDet, labCat, labexp, med }).ToList();

            return new JsonResult { Data = resultlabDet, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GuardarSolicitudLaboratorioExp(string folioLab, string labDetalle)
        {

            List<Lab_Detalle> labDet = JsonConvert.DeserializeObject<List<Lab_Detalle>>(labDetalle);

            Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();

            try
            {

                foreach (var item in labDet)


                    if (item.lab_id == "0381")
                    {
                        db.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + folioLab + "','" + item.lab_id + "','56A'," + item.lab_instrucciones + ")");
                        db.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + folioLab + "','" + item.lab_id + "','56M'," + item.lab_instrucciones + ")");

                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + folioLab + "','" + item.lab_id + "','" + item.lab_lab + "'," + item.lab_instrucciones + ")");
                    }

            }
            catch (Exception ex)
            {

                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CrearSolicitudLaboratorioExp(string newSolLab)
        {
            Lab_exp NewLabExp = JsonConvert.DeserializeObject<Lab_exp>(newSolLab);
            var FechaActual = DateTime.Now.ToShortDateString();

            Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();
            try
            {
                db.Database.ExecuteSqlCommand("insert into lab_exp (expediente,medico_crea,fecha_crea,estado,urgente,observaciones) values ('" + NewLabExp.expediente + "','" + NewLabExp.medico_crea + "', '" + FechaActual + "',0,'" + NewLabExp.urgente + "','" + NewLabExp.observaciones + "')");
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult DeleteSolicitudLab(string folioLab)
        {
            Models.SMDEVEntitiesRayosX db = new Models.SMDEVEntitiesRayosX();

            try
            {
                db.Database.ExecuteSqlCommand("delete from Lab_exp where Folio_lab=" + folioLab + ";");
                db.Database.ExecuteSqlCommand("delete from Lab_detalle where Folio_lab=" + folioLab + ";");
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult GetLaboratorioCatalogo(string search)
        {

            Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();


            var resultLabExp = (from labexp in db.lab_catalogo
                                where (labexp.lab_des.Contains(search))
                                select new { idserv = labexp.lab_lab, idlab = labexp.lab_id, instructions = labexp.lab_instrucciones, value = labexp.lab_grupo, label = labexp.lab_des, }).ToList();


            return new JsonResult { Data = resultLabExp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        #endregion

        #region RayosX

        public ActionResult RayosX(string id, string medico)
        {

            if (id == null || medico == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                {
                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    if (res != null)
                    {

                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //Si no ha llegado el paciente a recepcion
                            if (res.hora_recepcion == null)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            //Si ya llego
                            else
                            {
                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                Models.SMDEVEntitiesRayosX db3 = new Models.SMDEVEntitiesRayosX();

                                var prestaciones = db3.Database.SqlQuery<PrestacionesRayosX>("SELECT sp.codigo,sp.cantidad ,sp.numemp,sp.medico,ISNULL(sp.fecha_sol, GETDATE() - GETDATE()) As 'fecha_sol',sp.urgente_sol,ISNULL(sp.fecha_realiza, GETDATE() - GETDATE()) As 'fecha_realiza',sp.sexo,sp.edad ,sp.turno,sp.dx_sol,sp.proveedor,sp.prim_sub,sp.nota,sp.interp,sp.rea_interp,ISNULL(sp.fec_interp, GETDATE() - GETDATE()) As 'fec_interp',sp.realiza,srx2.codigo as 'rxcodigo', srx2.tipo, srx2.descripcion FROM servicio_rayosx sp INNER JOIN serv_rx_n2 srx2 ON srx2.codigo = sp.codigo WHERE sp.numemp='" + id + "' AND sp.medico=" + medico + "ORDER BY sp.fecha_realiza DESC").ToList();
                                var histprestaciones = db3.Database.SqlQuery<PrestacionesRayosX>("SELECT sp.codigo,sp.cantidad ,sp.numemp,sp.medico,ISNULL(sp.fecha_sol, GETDATE() - GETDATE()) As 'fecha_sol',sp.urgente_sol,ISNULL(sp.fecha_realiza, GETDATE() - GETDATE()) As 'fecha_realiza',sp.sexo,sp.edad ,sp.turno,sp.dx_sol,sp.proveedor,sp.prim_sub,sp.nota,sp.interp,sp.rea_interp,ISNULL(sp.fec_interp, GETDATE() - GETDATE()) As 'fec_interp',sp.realiza,srx2.codigo as 'rxcodigo', srx2.tipo, srx2.descripcion FROM servicio_rayosx sp INNER JOIN serv_rx_n2 srx2 ON srx2.codigo = sp.codigo WHERE sp.numemp='" + id + "' ORDER BY sp.fecha_realiza DESC").ToList();

                                ServicioPrestaciones dHabientesPrestaciones = new ServicioPrestaciones();
                                dHabientesPrestaciones.NUMEMP = id;
                                dHabientesPrestaciones.DHabiente = dhabientes;
                                dHabientesPrestaciones.Prestaciones = prestaciones;
                                dHabientesPrestaciones.HistorialPrestaciones = histprestaciones;

                                if (dhabientes.NUMAFIL != "P72112")
                                {
                                    return View(dHabientesPrestaciones);
                                }
                                else
                                {
                                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                                }


                            }
                        }
                        //Entonces es telefonica
                        else
                        {
                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            Models.SMDEVEntitiesRayosX db3 = new Models.SMDEVEntitiesRayosX();

                            var prestaciones = db3.Database.SqlQuery<PrestacionesRayosX>("SELECT sp.codigo,sp.cantidad ,sp.numemp,sp.medico,ISNULL(sp.fecha_sol, GETDATE() - GETDATE()) As 'fecha_sol',sp.urgente_sol,ISNULL(sp.fecha_realiza, GETDATE() - GETDATE()) As 'fecha_realiza',sp.sexo,sp.edad ,sp.turno,sp.dx_sol,sp.proveedor,sp.prim_sub,sp.nota,sp.interp,sp.rea_interp,ISNULL(sp.fec_interp, GETDATE() - GETDATE()) As 'fec_interp',sp.realiza,srx2.codigo as 'rxcodigo', srx2.tipo, srx2.descripcion FROM servicio_rayosx sp INNER JOIN serv_rx_n2 srx2 ON srx2.codigo = sp.codigo WHERE sp.numemp='" + id + "' AND sp.medico=" + medico + "ORDER BY sp.fecha_realiza DESC").ToList();
                            var histprestaciones = db3.Database.SqlQuery<PrestacionesRayosX>("SELECT sp.codigo,sp.cantidad ,sp.numemp,sp.medico,ISNULL(sp.fecha_sol, GETDATE() - GETDATE()) As 'fecha_sol',sp.urgente_sol,ISNULL(sp.fecha_realiza, GETDATE() - GETDATE()) As 'fecha_realiza',sp.sexo,sp.edad ,sp.turno,sp.dx_sol,sp.proveedor,sp.prim_sub,sp.nota,sp.interp,sp.rea_interp,ISNULL(sp.fec_interp, GETDATE() - GETDATE()) As 'fec_interp',sp.realiza,srx2.codigo as 'rxcodigo', srx2.tipo, srx2.descripcion FROM servicio_rayosx sp INNER JOIN serv_rx_n2 srx2 ON srx2.codigo = sp.codigo WHERE sp.numemp='" + id + "' ORDER BY sp.fecha_realiza DESC").ToList();

                            ServicioPrestaciones dHabientesPrestaciones = new ServicioPrestaciones();
                            dHabientesPrestaciones.NUMEMP = id;
                            dHabientesPrestaciones.DHabiente = dhabientes;
                            dHabientesPrestaciones.Prestaciones = prestaciones;
                            dHabientesPrestaciones.HistorialPrestaciones = histprestaciones;


                            if (dhabientes.NUMAFIL != "P72112")
                            {
                                return View(dHabientesPrestaciones);
                            }
                            else
                            {
                                TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                                return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                            }


                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                    {
                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();

                        Models.SMDEVEntitiesRayosX db3 = new Models.SMDEVEntitiesRayosX();

                        var prestaciones = db3.Database.SqlQuery<PrestacionesRayosX>("SELECT sp.codigo,sp.cantidad ,sp.numemp,sp.medico,ISNULL(sp.fecha_sol, GETDATE() - GETDATE()) As 'fecha_sol',sp.urgente_sol,ISNULL(sp.fecha_realiza, GETDATE() - GETDATE()) As 'fecha_realiza',sp.sexo,sp.edad ,sp.turno,sp.dx_sol,sp.proveedor,sp.prim_sub,sp.nota,sp.interp,sp.rea_interp,ISNULL(sp.fec_interp, GETDATE() - GETDATE()) As 'fec_interp',sp.realiza,srx2.codigo as 'rxcodigo', srx2.tipo, srx2.descripcion FROM servicio_rayosx sp INNER JOIN serv_rx_n2 srx2 ON srx2.codigo = sp.codigo WHERE sp.numemp='" + id + "' AND sp.medico=" + medico + "ORDER BY sp.fecha_realiza DESC").ToList();
                        var histprestaciones = db3.Database.SqlQuery<PrestacionesRayosX>("SELECT sp.codigo,sp.cantidad ,sp.numemp,sp.medico,ISNULL(sp.fecha_sol, GETDATE() - GETDATE()) As 'fecha_sol',sp.urgente_sol,ISNULL(sp.fecha_realiza, GETDATE() - GETDATE()) As 'fecha_realiza',sp.sexo,sp.edad ,sp.turno,sp.dx_sol,sp.proveedor,sp.prim_sub,sp.nota,sp.interp,sp.rea_interp,ISNULL(sp.fec_interp, GETDATE() - GETDATE()) As 'fec_interp',sp.realiza,srx2.codigo as 'rxcodigo', srx2.tipo, srx2.descripcion FROM servicio_rayosx sp INNER JOIN serv_rx_n2 srx2 ON srx2.codigo = sp.codigo WHERE sp.numemp='" + id + "' ORDER BY sp.fecha_realiza DESC").ToList();

                        ServicioPrestaciones dHabientesPrestaciones = new ServicioPrestaciones();
                        dHabientesPrestaciones.NUMEMP = id;
                        dHabientesPrestaciones.DHabiente = dhabientes;
                        dHabientesPrestaciones.Prestaciones = prestaciones;
                        dHabientesPrestaciones.HistorialPrestaciones = histprestaciones;

                        if (dhabientes.NUMAFIL != "P72112")
                        {
                            return View(dHabientesPrestaciones);
                        }
                        else
                        {
                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                            return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                        }


                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

            }

        }


        public JsonResult GetCategoriasRX()
        {
            var clavemedico = User.Identity.GetUserName().Substring(0, 2);


            if (clavemedico == "02")
            {
                Models.SMDEVEntitiesRayosX db = new Models.SMDEVEntitiesRayosX();
                var CatergoriasRX = db.Database.SqlQuery<serv_rx_n1>("SELECT * FROM serv_rx_n1 clave CLAVE NOT IN('005','002')").ToList();
                return new JsonResult { Data = CatergoriasRX, JsonRequestBehavior = JsonRequestBehavior.AllowGet, RecursionLimit = 100 };
            }

            else
            {
                Models.SMDEVEntitiesRayosX db = new Models.SMDEVEntitiesRayosX();
                var CatergoriasRX = db.Database.SqlQuery<serv_rx_n1>("SELECT * FROM serv_rx_n1").ToList();
                return new JsonResult { Data = CatergoriasRX, JsonRequestBehavior = JsonRequestBehavior.AllowGet, RecursionLimit = 100 };
            }


        }


        public JsonResult GetServiciosRX(string tipo)
        {
            var clavemedico = User.Identity.GetUserName().Substring(0, 2);
            Models.SMDEVEntitiesRayosX db = new Models.SMDEVEntitiesRayosX();

            string query = "";

            if (clavemedico == "02")
            {
                if (tipo == "003")
                {
                    query = "SELECT * FROM serv_rx_n2 WHERE tipo =" + tipo + " AND codigo IN('003011','003019','003014','003016');";
                }
                else
                {
                    query = "SELECT * FROM serv_rx_n2 WHERE tipo =" + tipo;
                }
            }

            else
            {
                query = "SELECT * FROM serv_rx_n2 WHERE tipo =" + tipo;
            }

            var ServiciosRX = db.Database.SqlQuery<serv_rx_n2>(query).ToList();

            return new JsonResult { Data = ServiciosRX, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult CrearSolicitudRX(string newSolRX, string urgencia)
        {
            PrestacionesRayosX NewLabRX = JsonConvert.DeserializeObject<PrestacionesRayosX>(newSolRX);
            Models.SMDEVEntitiesRayosX db = new Models.SMDEVEntitiesRayosX();

            try
            {
                db.Database.ExecuteSqlCommand("INSERT INTO servicio_rayosx (codigo,cantidad,numemp,medico,fecha_sol,sexo,edad,turno,nota,realiza,urgente_sol) values ('" + NewLabRX.codigo + "','" + NewLabRX.cantidad + "','" + NewLabRX.numemp + "','" + NewLabRX.medico + "',GETDATE(),'" + NewLabRX.sexo + "','" + NewLabRX.edad + "','" + NewLabRX.turno + "','" + NewLabRX.nota + "','" + NewLabRX.realiza + "','" + urgencia + "');");
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult DeleteSolicitudRX(string SolRX)
        {
            PrestacionesRayosX NewLabRX = JsonConvert.DeserializeObject<PrestacionesRayosX>(SolRX);
            Models.SMDEVEntitiesRayosX db = new Models.SMDEVEntitiesRayosX();

            try
            {

                string fecha = NewLabRX.fecha_realiza.ToString("yyyy/MM/dd HH:mm:ss");
                db.Database.ExecuteSqlCommand("DELETE FROM servicio_rayosx where numemp='" + NewLabRX.numemp + "' AND medico='" + NewLabRX.medico + "' AND fecha_sol = CONVERT(SMALLDATETIME,'" + fecha + "',101) AND codigo ='" + NewLabRX.codigo + "';");

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        #endregion

        #region  "ProcAmbulatorios"

        [HttpGet]
        public ActionResult ProcAmbOrdenes(string id)
        {
            if (User.IsInRole("Especialistas"))
            {
                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == id
                                  select a).FirstOrDefault();

                if (dhabientes.NUMAFIL != "P72112")
                {
                    return View(dhabientes);
                }
                else
                {
                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                    return RedirectToAction("SOAP/" + id, "ServiciosMedicos");
                }


            }
            else
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        public ActionResult ProcAmbulOrdenes(string id)
        {
            if (User.IsInRole("Especialistas"))
            {
                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == id
                                  select a).FirstOrDefault();
                return View(dhabientes);
            }
            else
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
        }

        public ActionResult VerProcAmbOrdenes(string id)
        {

            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabientes = (from a in db.DHABIENTES
                              where a.NUMEMP == id
                              select a).FirstOrDefault();
            return View(dhabientes);

        }

        public JsonResult GetMedicoNombresById(string Id)
        {
            var nombreusuario = Id;
            Models.SMDEVEntities19 db = new Models.SMDEVEntities19();
            var medico_info = (from a in db.MEDICOS
                               where a.Numero == nombreusuario
                               //orderby a.medico descending
                               select a).FirstOrDefault();

            if (medico_info != null)
            {
                Models.SMDEVEntities22 db2 = new Models.SMDEVEntities22();
                var especialidades = (from b in db2.ESPECIALIDADES
                                      where b.CLAVE == medico_info.Numero.Substring(0, 2)
                                      //orderby a.medico descending
                                      select b).FirstOrDefault();
                var resultado = new
                {
                    numero = medico_info.Numero,
                    nombres = medico_info.Nombre,
                    apellido_paterno = medico_info.Apaterno,
                    apellido_materno = medico_info.Amaterno,
                    cedula = medico_info.Cedula,
                    cedula_esp = medico_info.cedula_esp,
                    especialidad = especialidades.DESCRIPCION
                };

                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var resultado = "";

                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }


        }




        public JsonResult GetProcAmb(string med, string expd)
        {
            Models.SMDEVEntities32 dbOrdenes = new Models.SMDEVEntities32();


            List<Object> lst = new List<Object>();


            var orden_int = (from q in dbOrdenes.Orden_Int
                             where q.medico == med
                             where q.numemp == expd
                             where q.proveedor == "193"
                             join medicos in dbOrdenes.MEDICOS on q.medico equals medicos.Numero
                             join dhabi in dbOrdenes.DHABIENTES on q.numemp equals dhabi.NUMEMP
                             join prov in dbOrdenes.prov_subrrogados on q.proveedor equals prov.clave into provee
                             from proveedor in provee.DefaultIfEmpty()
                             select new
                             {
                                 id_orden = q.id_orden,
                                 folio = q.folio,
                                 numemp = q.numemp,
                                 nombre = dhabi.NOMBRES + " " + dhabi.APATERNO + " " + dhabi.AMATERNO,
                                 medico = medicos.Nombre + " " + medicos.Apaterno + " " + medicos.Amaterno,
                                 fecha_internamiento = q.fecha_internamiento,
                                 fecha_alta = q.fecha_alta,
                                 proveerdor = proveedor.nombre,
                                 proveerdor_direccion = proveedor.direccion,
                                 proveerdor_telefono = proveedor.telefono1,
                                 estudios = q.estudios,
                                 motivo = q.motivo,
                                 indicaciones = q.indicaciones,
                                 fecha_registro = q.fecha_registro,
                                 tipo = q.tipo,
                                 medicoid = medicos.Numero
                             })
                      .OrderByDescending(g => g.fecha_registro)
                      .ToList();

            foreach (var item in orden_int)
            {

                SMDEVNotaIngreso db = new SMDEVNotaIngreso();
                var notaIng = (from n in db.NotaIngreso
                               where
                               n.Medico == med &&
                               n.NumEmp == expd &&
                               n.OrdenFolio == item.id_orden
                               select n).ToList();

                SMDEVNotaPreoperatoria db2 = new SMDEVNotaPreoperatoria();
                var notaPreop = (from n in db2.NotaPreoperatoria
                                 where
                                 n.Medico == med &&
                                 n.NumEmp == expd &&
                                 n.OrdenFolio == item.id_orden
                                 select n).ToList();

                SMDEVNotaOperatoria db3 = new SMDEVNotaOperatoria();
                var notaOp = (from n in db3.NotaOperatoria
                              where
                              n.Medico == med &&
                              n.NumEmp == expd &&
                              n.OrdenFolio == item.id_orden
                              select n).ToList();


                SMDEVNotaEgreso db4 = new SMDEVNotaEgreso();
                var notaEgreso = (from n in db4.NotaEgreso
                                  where
                                  n.Medico == med &&
                                  n.NumEmp == expd &&
                                  n.OrdenFolio == item.id_orden
                                  select n).ToList();

                var lstitem = new
                {
                    id_orden = item.id_orden,
                    folio = item.folio,
                    numemp = item.numemp,
                    nombre = item.nombre,
                    medico = item.medico,
                    fecha_internamiento = item.fecha_internamiento,
                    fecha_alta = item.fecha_alta,
                    proveerdor = item.proveerdor,
                    proveerdor_direccion = item.proveerdor_direccion,
                    proveerdor_telefono = item.proveerdor_telefono,
                    estudios = item.estudios,
                    motivo = item.motivo,
                    indicaciones = item.indicaciones,
                    fecha_registro = item.fecha_registro,
                    tipo = item.tipo,
                    NotaIngreso = notaIng,
                    NotaPreoperatoria = notaPreop,
                    NotaOperatoria = notaOp,
                    NotaEgreso = notaEgreso,
                    medicoid = item.medicoid
                };

                lst.Add(lstitem);

            }

            return new JsonResult { Data = lst, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetProcAmbVer(string expd)
        {
            Models.SMDEVEntities32 dbOrdenes = new Models.SMDEVEntities32();


            List<Object> lst = new List<Object>();


            var orden_int = (from q in dbOrdenes.Orden_Int
                             where q.numemp == expd
                             where q.proveedor == "193"
                             join medicos in dbOrdenes.MEDICOS on q.medico equals medicos.Numero
                             join dhabi in dbOrdenes.DHABIENTES on q.numemp equals dhabi.NUMEMP
                             join prov in dbOrdenes.prov_subrrogados on q.proveedor equals prov.clave into provee
                             from proveedor in provee.DefaultIfEmpty()
                             select new
                             {
                                 id_orden = q.id_orden,
                                 folio = q.folio,
                                 numemp = q.numemp,
                                 nombre = dhabi.NOMBRES + " " + dhabi.APATERNO + " " + dhabi.AMATERNO,
                                 medico = medicos.Nombre + " " + medicos.Apaterno + " " + medicos.Amaterno,
                                 fecha_internamiento = q.fecha_internamiento,
                                 fecha_alta = q.fecha_alta,
                                 proveerdor = proveedor.nombre,
                                 proveerdor_direccion = proveedor.direccion,
                                 proveerdor_telefono = proveedor.telefono1,
                                 estudios = q.estudios,
                                 motivo = q.motivo,
                                 indicaciones = q.indicaciones,
                                 fecha_registro = q.fecha_registro,
                                 tipo = q.tipo,
                                 medicoid = medicos.Numero
                             })
                      .OrderByDescending(g => g.fecha_registro)
                      .ToList();

            foreach (var item in orden_int)
            {

                SMDEVNotaIngreso db = new SMDEVNotaIngreso();
                var notaIng = (from n in db.NotaIngreso
                               where
                               n.NumEmp == expd &&
                               n.OrdenFolio == item.id_orden
                               select n).ToList();

                SMDEVNotaPreoperatoria db2 = new SMDEVNotaPreoperatoria();
                var notaPreop = (from n in db2.NotaPreoperatoria
                                 where
                                 n.NumEmp == expd &&
                                 n.OrdenFolio == item.id_orden
                                 select n).ToList();

                SMDEVNotaOperatoria db3 = new SMDEVNotaOperatoria();
                var notaOp = (from n in db3.NotaOperatoria
                              where
                              n.NumEmp == expd &&
                              n.OrdenFolio == item.id_orden
                              select n).ToList();


                SMDEVNotaEgreso db4 = new SMDEVNotaEgreso();
                var notaEgreso = (from n in db4.NotaEgreso
                                  where
                                  n.NumEmp == expd &&
                                  n.OrdenFolio == item.id_orden
                                  select n).ToList();

                var lstitem = new
                {
                    id_orden = item.id_orden,
                    folio = item.folio,
                    numemp = item.numemp,
                    nombre = item.nombre,
                    medico = item.medico,
                    fecha_internamiento = item.fecha_internamiento,
                    fecha_alta = item.fecha_alta,
                    proveerdor = item.proveerdor,
                    proveerdor_direccion = item.proveerdor_direccion,
                    proveerdor_telefono = item.proveerdor_telefono,
                    estudios = item.estudios,
                    motivo = item.motivo,
                    indicaciones = item.indicaciones,
                    fecha_registro = item.fecha_registro,
                    tipo = item.tipo,
                    NotaIngreso = notaIng,
                    NotaPreoperatoria = notaPreop,
                    NotaOperatoria = notaOp,
                    NotaEgreso = notaEgreso,
                    medicoid = item.medicoid
                };

                lst.Add(lstitem);

            }

            return new JsonResult { Data = lst, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion

        #region "PaqEstudiosPreoperatorios"

        public ActionResult GuardarPaquetePreoperatorio(string DHabiente)
        {

            string Medico = User.Identity.GetUserName();

            Models.SMDEVEntitiesRayosX dbRX = new Models.SMDEVEntitiesRayosX();
            Models.SMDEVEntitiesSoLab dbSolLab = new Models.SMDEVEntitiesSoLab();

            Models.SMDEVEntities20 dbhab = new Models.SMDEVEntities20();
            var DHabientes = (from a in dbhab.DHABIENTES
                              where a.NUMEMP == DHabiente
                              select a).FirstOrDefault();

            Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
            var exp = (from a in db3.expediente
                       where a.numemp == DHabiente
                       where a.medico == Medico
                       where a.hora_termino != null
                       && a.fecha.Day == DateTime.Now.Day
                       && a.fecha.Month == DateTime.Now.Month
                       && a.fecha.Year == DateTime.Now.Year
                       select a).FirstOrDefault();

            var estudios = db3.Database.SqlQuery<string>("select codigo from servicio_rayosx where numemp= '" + DHabiente + "' and medico='" + Medico + "' and nota like 'Paquete Preoperatorio' and CAST(fecha_sol AS DATE) = CAST(GETDATE() AS DATE)").FirstOrDefault();


            int Edad = 0;

            if (DHabientes.FNAC != null)
            {
                Edad = (DateTime.Now.Year - DHabientes.FNAC.Value.Year);
            }

            int FolioLab;

            try
            {

                if (exp != null)
                {

                    if (exp.referido != "62")
                    {
                        db3.Database.ExecuteSqlCommand("UPDATE expediente SET referido= '62' WHERE numemp='" + DHabiente + "' AND medico='" + Medico + "' AND fecha = CONVERT(SMALLDATETIME,'" + exp.fecha.ToString("yyyy/MM/dd HH:mm:ss") + "',101);");

                    }
                }
                else
                {
                    return new JsonResult { Data = "Debes terminar una nota médica antes de crear un paquete preoperatorio", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }



                if (estudios != null)
                {
                    return new JsonResult { Data = "El paciente ya cuenta con un paquete preoperatorio", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }



                dbRX.Database.ExecuteSqlCommand("INSERT INTO servicio_rayosx (codigo,cantidad,numemp,medico,fecha_sol,sexo,edad,turno,nota,realiza,urgente_sol) values ('001095','1','" + DHabiente + "','" + Medico + "',GETDATE(),'" + DHabientes.SEXO + "','" + Edad.ToString() + "',null,'Paquete Preoperatorio','" + Medico + "','1');");

                using (SqlCommand cmd = new SqlCommand("insert into lab_exp (expediente,medico_crea,fecha_crea,estado,urgente,observaciones) values ('" + DHabiente + "','" + Medico + "',GETDATE(),0,'1','Paquete Preoperatorio')"))
                {
                    cmd.Connection = new SqlConnection(dbSolLab.Database.Connection.ConnectionString);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT SCOPE_IDENTITY()";
                    FolioLab = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Connection.Close();
                };

                dbSolLab.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + FolioLab + "','0021','1','1')");
                dbSolLab.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + FolioLab + "','0386','286','1')");
                dbSolLab.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + FolioLab + "','0016','7','1')");
                //Agregar estudios y    EKG 
                dbSolLab.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + FolioLab + "','0017','5','1')");
                dbSolLab.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + FolioLab + "','0019','6','1')");
                //dbSolLab.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + FolioLab + "','0059','83','1')");
                //dbSolLab.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + FolioLab + "','0043','42','1')");


                var ekgres = GuardarSolicitudEnfermeria(DHabiente);
                if (ekgres == false)
                {
                    return new JsonResult { Data = "Error al generar EKG", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                dbRX.Database.ExecuteSqlCommand("INSERT INTO servicio_rayosx (codigo,cantidad,numemp,medico,fecha_sol,sexo,edad,turno,nota,realiza,urgente_sol) values ('001095','1','" + DHabiente + "','" + Medico + "',GETDATE(),'" + DHabientes.SEXO + "','" + Edad.ToString() + "',null,'Paquete Preoperatorio','" + Medico + "','1');");
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public bool GuardarSolicitudEnfermeria(string numemp)
        {
            EnfermeriaSolicitud NewEnf = new EnfermeriaSolicitud();
            NewEnf.NumEmp = numemp;
            NewEnf.Medico = User.Identity.Name;


            EnfermeriaSolicitudDetalle NewEnfDetalle = new EnfermeriaSolicitudDetalle();
            NewEnfDetalle.IdServicio = 1;
            NewEnfDetalle.Descripcion = "Electrocardiograma";

            Models.SMDEVEnfermeriaSolicitud db = new Models.SMDEVEnfermeriaSolicitud();
            int folio;

            try
            {

                using (SqlCommand cmd = new SqlCommand("insert into EnfermeriaSolicitud(NumEmp, Medico, FechaSol,FechaAgenda,UsuarioAgenda, FechaRealiza, UsuarioRealiza,NotaRealiza,Nota,Urgente,Finalizado,Cancelado,NoPresento) values('" + NewEnf.NumEmp + "', '" + NewEnf.Medico + "', GETDATE(),GETDATE()- GETDATE(),'',GETDATE()- GETDATE(),'','','"+ "Paquete Preoperatorio"  +"', '" + 1 + "','0','0','0')"))
                {

                    cmd.Connection = new SqlConnection(db.Database.Connection.ConnectionString);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT SCOPE_IDENTITY()";
                    folio = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Connection.Close();
                }

 
                   db.Database.ExecuteSqlCommand("insert into EnfermeriaSolicitudDetalle (SolicitudId, IdServicio,Descripcion, FechaSol, FechaRealiza,UsuarioRealiza) values ('" + folio + "','" + NewEnfDetalle.IdServicio + "','" + NewEnfDetalle.Descripcion + "',GETDATE(),GETDATE()- GETDATE(),'')");
                    
            }
            catch (Exception ex)
            {


                return false;
            }

            return true;
        }

        public JsonResult GetInternamientoInfo(string DHabiente)
        {
            string Medico = User.Identity.GetUserName();
            var result = new Object();

            Models.SMDEVEntities19 db = new Models.SMDEVEntities19();
            var medico_info = (from a in db.MEDICOS
                               where a.Numero == Medico
                               //orderby a.medico descending
                               select a).FirstOrDefault();

            Models.SMDEVEntities20 dbhab = new Models.SMDEVEntities20();
            var DHabientes = (from a in dbhab.DHABIENTES
                              where a.NUMEMP == DHabiente
                              select a).FirstOrDefault();

            Models.SMDEVEntities33 db3 = new Models.SMDEVEntities33();

            var internamiento = (from a in db3.Orden_Int
                                 join prov in db3.prov_subrrogados on a.proveedor equals prov.clave
                                 where a.numemp == DHabiente
                                 where a.medico == Medico
                                 where a.fecha_registro.Value.Day == DateTime.Now.Day
                                 where a.fecha_registro.Value.Month == DateTime.Now.Month
                                 where a.fecha_registro.Value.Year == DateTime.Now.Year
                                 select new { a, prov }).FirstOrDefault();

            Models.SMDEVEntitiesRayosX dbRX = new Models.SMDEVEntitiesRayosX();
            var estudios = db3.Database.SqlQuery<string>("select codigo from servicio_rayosx where numemp= '" + DHabiente + "' and medico='" + Medico + "' and nota like 'Paquete Preoperatorio' and CAST(fecha_sol AS DATE) = CAST(GETDATE() AS DATE)").FirstOrDefault();

            if (estudios == null)
            {
                estudios = "no cuenta con estudios";
            }


            if (internamiento == null)
            {
                if(medico_info != null) {
                    result = new
                    {
                        PacienteNombre = DHabientes.NOMBRES + " " + DHabientes.APATERNO + " " + DHabientes.AMATERNO,
                        PacienteExpediente = DHabientes.NUMEMP,
                        MedicoNombre = medico_info.Nombre + " " + medico_info.Apaterno + " " + medico_info.Amaterno,
                        InternamientoFecha = "NA",
                        InternamientoProveedor = "NA",
                        estudios = estudios
                    };
                }
            }
            else
            {
                if (medico_info != null)
                {
                    result = new
                    {
                        PacienteNombre = DHabientes.NOMBRES + " " + DHabientes.APATERNO + " " + DHabientes.AMATERNO,
                        PacienteExpediente = DHabientes.NUMEMP,
                        MedicoNombre = medico_info.Nombre + " " + medico_info.Apaterno + " " + medico_info.Amaterno,
                        InternamientoFecha = internamiento.a.fecha_internamiento.GetValueOrDefault(),
                        InternamientoProveedor = internamiento.prov.nombre,
                        estudios = estudios
                    };
                }
            }


            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion


        public ActionResult Insumos(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == id
                                  select a).FirstOrDefault();

                if (dhabientes != null)
                {
                    return View(dhabientes);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }


        [HttpPost]
        public ActionResult GuardarInsumos(string numexp, string descripcion)
        {

            //System.Diagnostics.Debug.WriteLine(numexp);

            //Models.SMDEVEntities16 db = new Models.SMDEVEntities16();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaHoy2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaHoyDT = DateTime.Parse(fechaHoy);
            var fechaHoyDT2 = DateTime.Parse(fechaHoy2);
            var ip_realiza = Request.UserHostAddress;
            var username = User.Identity.GetUserName();

            Insumos insu = new Insumos();
            insu.descripcion = descripcion;
            insu.fecha = fechaHoyDT;
            insu.ip_realiza= ip_realiza;
            insu.estatus = 1;
            insu.numexp = numexp;
            insu.usuario = username;
            db.Insumos.Add(insu);
            db.SaveChanges();

            var insum = (from d in db.Insumos
                        //where d.fecha == fechaHoyDT
                        where d.numexp == numexp
                        where d.usuario == username
                        where d.estatus == 1
                        select d)
                        .OrderByDescending(g => g.fecha)
                        .FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(insum);

            //actualizar folio
            if (insum != null)
            {
                db.Database.ExecuteSqlCommand("UPDATE Insumos SET folio = 'SI-" + insum.id + "' WHERE id = '" + insum.id + "' ");

                var medico_info = (from a in db.MEDICOS
                                   where a.Numero == username
                                   select a).FirstOrDefault();

                var dhab_info = (from a in db.DHABIENTES
                                   where a.NUMEMP == numexp
                                 select a).FirstOrDefault();


                TempData["folio"] = "SI-" + insum.id;
                TempData["descripcion"] = descripcion;
                TempData["medico"] = medico_info.Nombre+" "+ medico_info.Apaterno+" "+ medico_info.Amaterno;
                TempData["paciente"] = dhab_info.NOMBRES+" "+ dhab_info.APATERNO+" "+ dhab_info.AMATERNO;
                TempData["fecha"] = string.Format("{0:dddd, dd MMMM yyyy}", fechaHoyDT, new CultureInfo("es-ES"));
                TempData["message_success_insumo"] = "Insumo generado con éxito";

                return RedirectToAction("Insumos/" + numexp, "ServiciosMedicos");

            }
            else
            {
                TempData["message_error"] = "Error";
                return RedirectToAction("Insumos/" + numexp, "ServiciosMedicos");
            }
        }

    }
}