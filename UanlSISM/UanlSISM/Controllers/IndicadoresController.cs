using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class IndicadoresController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        // GET: ServiciosMedicos


        public ActionResult Index()
        {
            if (User.IsInRole("Coordinador"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ProductividadFarmacia()
        {
            if (User.IsInRole("Coordinador"))
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:00:00.000");
                var fecha_correcta = DateTime.Parse(fecha);

                Models.SMDEVEntities18 db = new Models.SMDEVEntities18();

                Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

                var productividad_far = (from q in db2.Receta_Exp
                                         where q.Hora_Rcta >= fecha_correcta
                                         where q.Hora_Rcta != null
                                         where q.UserFar != null || q.UsuarioId != null
                                         join usuar in db2.Usuario on q.UserFar equals usuar.UsuarioId into usuX
                                         from usuIn in usuX.DefaultIfEmpty()
                                         join usuar2 in db2.Usuario on q.UsuarioId equals usuar2.UsuarioId into usuX2
                                         from usuIn2 in usuX2.DefaultIfEmpty()
                                         select new
                                         {
                                             //username = usuIn.Usu_Nombre,
                                             //username = usuIn.Usu_Nombre == null ? usuIn2.Usu_Nombre : 
                                             //         usuIn.Usu_Nombre != null ? usuIn.Usu_Nombre : "",
                                             username =
                                               (
                                                   q.UserFar != null ? usuIn.Usu_Nombre :
                                                   q.UsuarioId != null ? usuIn2.Usu_Nombre :
                                                   q.UserFar == null && q.UsuarioId == null ? "Desconocido" : "Desconocido"
                                               ),
                                             fecha = q.Hora_Rcta,
                                             folio_rcta = q.Folio_Rcta,
                                             manual = q.Manual,
                                             folio_rc = q.folio_rc,
                                             userfar = q.UserFar,
                                             usuarioid = q.UsuarioId,
                                         })
                                  .ToList().Select(x => new
                                  {
                                      username = x.username,
                                      fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                      folio_rcta = x.folio_rcta,
                                      manual = x.manual,
                                      folio_rc = x.folio_rc,
                                      userfar = x.userfar,
                                      usuarioid = x.usuarioid,
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.username,
                                      p.fecha,
                                  })
                                  .Select(g => new
                                  {
                                      username = g.Key.username,
                                      fecha = g.Key.fecha,
                                      count = g.Count(),
                                      //recetas = "Recetas Normales: " + g.Where(p => p.manual == null).Where(p => p.folio_rc == null).Where(p => p.fecha != null).Where(p => p.userfar != null).Count() + " Recetas Manuales: " + g.Where(p => p.manual != null).Where(p => p.fecha != null).Where(p => p.fecha != null).Where(p => p.usuarioid != null).Count() + " Recetas Tarjeton:  " + g.Where(p => p.manual == null).Where(p => p.folio_rc != null).Where(p => p.fecha != null).Where(p => p.usuarioid != null).Count(),
                                      receta_normal = g.Where(p => p.manual == null).Where(p => p.folio_rc == null).Where(p => p.fecha != null).Where(p => p.userfar != null).Count(),
                                      receta_manuales = g.Where(p => p.manual != null).Where(p => p.fecha != null).Where(p => p.fecha != null).Where(p => p.usuarioid != null).Count(),
                                      receta_tarjeton = g.Where(p => p.manual == null).Where(p => p.folio_rc != null).Where(p => p.fecha != null).Where(p => p.usuarioid != null).Count(),
                                  })
                                  .OrderBy(g => g.fecha)
                                  .OrderBy(g => g.username)
                                  .ToList();

                string json = JsonConvert.SerializeObject(productividad_far);
                string path = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(path + "indicadores/farmacia/productividad.json", json);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult ProductividadCU()
        {
            if (User.IsInRole("CoordinadorCU"))
            {

                #region "Ciudad Universitaria"

                var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                var fecha_correcta = DateTime.Parse(fecha);

                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var fechaCU = DateTime.Now.ToString("2021-10-13T00:00:00.000");
                var fechaCUDT = DateTime.Parse(fechaCU);


                //Ciudad Universitario detalles
                var expCu = (from ex in db.expediente
                             where ex.ip_realiza.StartsWith("148.234.218")
                             where ex.medico != "02101"
                             //where ex.medico == "22023" || ex.medico == "02235" || ex.medico == "62003" || ex.medico == "08055" || ex.medico == "05038" || ex.medico == "02294"
                             where ex.fecha >= fecha_correcta
                             where ex.hora_termino != null
                             join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                             from medicosIn in mediX.DefaultIfEmpty()
                             join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                             from especiIn in especiX.DefaultIfEmpty()
                             select new
                             {
                                 medico = ex.medico,
                                 fecha = ex.fecha,
                                 especialidad = especiIn.DESCRIPCION,
                                 nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                 consultadistancia = ex.consultadistancia
                             })
                                  .ToList().Select(x => new
                                  {
                                  //clave = x.clave,
                                  medico = x.medico,
                                      nombre = x.nombre,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .ToList().Select(z => new
                                  {
                                      medico = z.medico,
                                      nombre = z.nombre,
                                      especialidad = z.especialidad,
                                      fecha = z.fecha,
                                      consultadistancia = z.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.nombre,
                                      p.fecha,
                                      p.medico,
                                      p.especialidad
                                  })
                                 .Select(g => new
                                 {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                     nombre = g.Key.nombre,
                                     especialidad = g.Key.especialidad,
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 //.OrderBy(g => g.fecha)
                                 .OrderBy(g => g.medico)
                                 .ToList();


                var resultsDetCU = new List<IndEspListDet>();

                foreach (var item in expCu)
                {
                    var espe = item.especialidad;

                    if (item.medico == "02235" || item.medico == "02294")
                    {
                        espe = "MEDICINA GENERAL";
                    }

                    var resultCU = new IndEspListDet
                    {
                        medico = item.medico,
                        nombre = item.nombre,
                        especialidad = espe,
                        fecha = item.fecha,
                        count = item.count,
                        telefonica = item.telefonica,
                        presencial = item.presencial,
                    };

                    resultsDetCU.Add(resultCU);
                }

                //System.Diagnostics.Debug.WriteLine(expCu);

                string jsonCU = JsonConvert.SerializeObject(resultsDetCU);
                string pathCU = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(pathCU + "indicadores/especialidades/detalles_cu.json", jsonCU);


                #endregion

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public class IndEspList
        {
            public string clave { get; set; }
            public string especialidad { get; set; }
            public string fecha { get; set; }
            public int count { get; set; }
            public int telefonica { get; set; }
            public int presencial { get; set; }
        }

        public class IndEspListDet
        {
            public string medico { get; set; }
            public string nombre { get; set; }
            public string especialidad { get; set; }
            public string fecha { get; set; }
            public int count { get; set; }
            public string lugar { get; set; }
            public int telefonica { get; set; }
            public int presencial { get; set; }
            public int ordenint { get; set; }

        }

        public class IndGrafica
        {
            public double Date { get; set; }
            public int Count { get; set; }
        }

        public class ListAntigenos
        {
            public int Count { get; set; }
            public string Fecha { get; set; }

        }

        public class ListAntigenosDet
        {
            public string medico { get; set; }
            public string nombre { get; set; }
            public string paciente { get; set; }
            public string dependencia { get; set; }
            public string fecha { get; set; }
            public string resultado { get; set; }
        }


        public ActionResult ProductividadConsulta()
        {
            if (User.IsInRole("Productividad1") || User.IsInRole("Productividad2") || User.IsInRole("Productividad3") || User.IsInRole("Productividad4") || User.IsInRole("Productividad5") || User.IsInRole("Productividad6") || User.IsInRole("Productividad7") || User.IsInRole("Productividad8") || User.IsInRole("Productividad9") || User.IsInRole("Productividad10"))
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                var fecha_correcta = DateTime.Parse(fecha);

                Models.SMDEVEntities28 db = new Models.SMDEVEntities28();

                if (User.IsInRole("Productividad1"))
                {
                    var expediente = (from q in db.expediente
                                          //where q.medico.StartsWith(item.CLAVE)
                                      where !q.ip_realiza.StartsWith("148.234.64")
                                      where q.ip_realiza != "148.234.143.203"
                                      where q.ip_realiza != "148.234.140.9"
                                      where q.fecha >= fecha_correcta
                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                      where especiIn.CLAVE == "05"
                                      select new
                                      {
                                          clave = especiIn.CLAVE,
                                          fecha = q.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          consultadistancia = q.consultadistancia
                                      })
                                  .ToList().Select(x => new
                                  {
                                      clave = x.clave,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.especialidad,
                                      p.fecha,
                                      p.clave,
                                  })
                                 .Select(g => new
                                 {
                                     clave = g.Key.clave,
                                     especialidad = g.Key.especialidad,
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 .OrderBy(g => g.especialidad)
                                 .ToList();

                    var results1 = new List<IndEspList>();

                    foreach (var item in expediente)
                    {
                        var result = new IndEspList
                        {
                            clave = item.clave,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        results1.Add(result);
                    }

                    //System.Diagnostics.Debug.WriteLine(results1);

                    string json = JsonConvert.SerializeObject(results1);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad.json", json);


                    //DETALLES
                    var expediedet = (from ex in db.expediente
                                      where !ex.ip_realiza.StartsWith("148.234.64")
                                      where ex.ip_realiza != "148.234.143.203"
                                      where ex.ip_realiza != "148.234.140.9"
                                      where ex.fecha >= fecha_correcta
                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                      from medicosIn in mediX.DefaultIfEmpty()
                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                      where especiIn.CLAVE == "05"
                                      select new
                                      {
                                          //clave = especiIn.CLAVE,
                                          medico = ex.medico,
                                          fecha = ex.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                          consultadistancia = ex.consultadistancia
                                      })
                              .ToList().Select(x => new
                              {
                                  //clave = x.clave,
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  //clave = z.clave,
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  fecha = z.fecha,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  //p.clave,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = g.Key.fecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

                    var resultsDet1 = new List<IndEspListDet>();

                    foreach (var item in expediedet)
                    {
                        var resultDet = new IndEspListDet
                        {
                            medico = item.medico,
                            nombre = item.nombre,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        resultsDet1.Add(resultDet);
                    }

                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                    string path2 = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);

                }


                if (User.IsInRole("Productividad2"))
                {
                    var expediente = (from q in db.expediente
                                          //where q.medico.StartsWith(item.CLAVE)
                                      where !q.ip_realiza.StartsWith("148.234.64")
                                      where q.ip_realiza != "148.234.143.203"
                                      where q.ip_realiza != "148.234.140.9"
                                      where q.fecha >= fecha_correcta
                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Derma
                                      where especiIn.CLAVE == "12"
                                      //Reuma
                                      || especiIn.CLAVE == "23"
                                      //Neumo
                                      || especiIn.CLAVE == "16"
                                      //Nefro
                                      || especiIn.CLAVE == "17"
                                      //Neuro
                                      || especiIn.CLAVE == "14"
                                      //Gastro
                                      || especiIn.CLAVE == "37"
                                      //Hemato
                                      || especiIn.CLAVE == "36"
                                      //Infec
                                      || especiIn.CLAVE == "33"
                                      //Medicina Interna
                                      || especiIn.CLAVE == "13"
                                      //Alergias
                                      || especiIn.CLAVE == "11"
                                      //Cardio
                                      || especiIn.CLAVE == "34"
                                      //Geriatria
                                      || especiIn.CLAVE == "60"
                                      //Psiquiatras
                                      || especiIn.CLAVE == "15"
                                      //Algologia Medicina del dolor
                                      || especiIn.CLAVE == "63"
                                      select new
                                      {
                                          clave = especiIn.CLAVE,
                                          fecha = q.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          consultadistancia = q.consultadistancia
                                      })
                                  .ToList().Select(x => new
                                  {
                                      clave = x.clave,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.especialidad,
                                      p.fecha,
                                      p.clave,
                                  })
                                 .Select(g => new
                                 {
                                     clave = g.Key.clave,
                                     especialidad = g.Key.especialidad,
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 .OrderBy(g => g.especialidad)
                                 .ToList();

                    var results1 = new List<IndEspList>();

                    foreach (var item in expediente)
                    {
                        var result = new IndEspList
                        {
                            clave = item.clave,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        results1.Add(result);
                    }

                    //System.Diagnostics.Debug.WriteLine(results1);

                    string json = JsonConvert.SerializeObject(results1);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad.json", json);


                    //DETALLES
                    var expediedet = (from ex in db.expediente
                                      where !ex.ip_realiza.StartsWith("148.234.64")
                                      where ex.ip_realiza != "148.234.143.203"
                                      where ex.ip_realiza != "148.234.140.9"
                                      where ex.fecha >= fecha_correcta
                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                      from medicosIn in mediX.DefaultIfEmpty()
                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Derma
                                      where especiIn.CLAVE == "12"
                                      //Reuma
                                      || especiIn.CLAVE == "23"
                                      //Neumo
                                      || especiIn.CLAVE == "16"
                                      //Nefro
                                      || especiIn.CLAVE == "17"
                                      //Neuro
                                      || especiIn.CLAVE == "14"
                                      //Gastro
                                      || especiIn.CLAVE == "37"
                                      //Hemato
                                      || especiIn.CLAVE == "36"
                                      //Infec
                                      || especiIn.CLAVE == "33"
                                      //Medicina Interna
                                      || especiIn.CLAVE == "13"
                                      //Alergias
                                      || especiIn.CLAVE == "11"
                                      //Cardio
                                      || especiIn.CLAVE == "34"
                                      //Geriatria
                                      || especiIn.CLAVE == "60"
                                      select new
                                      {
                                          //clave = especiIn.CLAVE,
                                          medico = ex.medico,
                                          fecha = ex.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                          consultadistancia = ex.consultadistancia
                                      })
                              .ToList().Select(x => new
                              {
                                  //clave = x.clave,
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  //clave = z.clave,
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  fecha = z.fecha,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  //p.clave,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = g.Key.fecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

                    var resultsDet1 = new List<IndEspListDet>();

                    foreach (var item in expediedet)
                    {
                        var resultDet = new IndEspListDet
                        {
                            medico = item.medico,
                            nombre = item.nombre,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        resultsDet1.Add(resultDet);
                    }

                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                    string path2 = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);

                }


                if (User.IsInRole("Productividad3"))
                {
                    var expediente = (from q in db.expediente
                                          //where q.medico.StartsWith(item.CLAVE)
                                      where !q.ip_realiza.StartsWith("148.234.64")
                                      where q.ip_realiza != "148.234.143.203"
                                      where q.ip_realiza != "148.234.140.9"
                                      where q.fecha >= fecha_correcta
                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Gine
                                      where especiIn.CLAVE == "08"
                                      select new
                                      {
                                          clave = especiIn.CLAVE,
                                          fecha = q.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          consultadistancia = q.consultadistancia
                                      })
                                  .ToList().Select(x => new
                                  {
                                      clave = x.clave,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.especialidad,
                                      p.fecha,
                                      p.clave,
                                  })
                                 .Select(g => new
                                 {
                                     clave = g.Key.clave,
                                     especialidad = g.Key.especialidad,
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 .OrderBy(g => g.especialidad)
                                 .ToList();

                    var results1 = new List<IndEspList>();

                    foreach (var item in expediente)
                    {
                        var result = new IndEspList
                        {
                            clave = item.clave,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        results1.Add(result);
                    }

                    //System.Diagnostics.Debug.WriteLine(results1);

                    string json = JsonConvert.SerializeObject(results1);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad.json", json);


                    //DETALLES
                    var expediedet = (from ex in db.expediente
                                      where !ex.ip_realiza.StartsWith("148.234.64")
                                      where ex.ip_realiza != "148.234.143.203"
                                      where ex.ip_realiza != "148.234.140.9"
                                      where ex.fecha >= fecha_correcta
                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                      from medicosIn in mediX.DefaultIfEmpty()
                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Gine
                                      where especiIn.CLAVE == "08"
                                      select new
                                      {
                                          //clave = especiIn.CLAVE,
                                          medico = ex.medico,
                                          fecha = ex.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                          consultadistancia = ex.consultadistancia
                                      })
                              .ToList().Select(x => new
                              {
                                  //clave = x.clave,
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  //clave = z.clave,
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  fecha = z.fecha,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  //p.clave,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = g.Key.fecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

                    var resultsDet1 = new List<IndEspListDet>();

                    foreach (var item in expediedet)
                    {
                        var resultDet = new IndEspListDet
                        {
                            medico = item.medico,
                            nombre = item.nombre,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        resultsDet1.Add(resultDet);
                    }

                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                    string path2 = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);

                }


                if (User.IsInRole("Productividad4"))
                {
                    var expediente = (from q in db.expediente
                                          //where q.medico.StartsWith(item.CLAVE)
                                      where !q.ip_realiza.StartsWith("148.234.64")
                                      where q.ip_realiza != "148.234.143.203"
                                      where q.ip_realiza != "148.234.140.9"
                                      where q.fecha >= fecha_correcta
                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Urolo
                                      where especiIn.CLAVE == "18"
                                      //Otorrino
                                      || especiIn.CLAVE == "07"
                                      select new
                                      {
                                          clave = especiIn.CLAVE,
                                          fecha = q.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          consultadistancia = q.consultadistancia
                                      })
                                  .ToList().Select(x => new
                                  {
                                      clave = x.clave,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.especialidad,
                                      p.fecha,
                                      p.clave,
                                  })
                                 .Select(g => new
                                 {
                                     clave = g.Key.clave,
                                     especialidad = g.Key.especialidad,
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 .OrderBy(g => g.especialidad)
                                 .ToList();

                    var results1 = new List<IndEspList>();

                    foreach (var item in expediente)
                    {
                        var result = new IndEspList
                        {
                            clave = item.clave,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        results1.Add(result);
                    }

                    //System.Diagnostics.Debug.WriteLine(results1);

                    string json = JsonConvert.SerializeObject(results1);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad.json", json);


                    //DETALLES
                    var expediedet = (from ex in db.expediente
                                      where !ex.ip_realiza.StartsWith("148.234.64")
                                      where ex.ip_realiza != "148.234.143.203"
                                      where ex.ip_realiza != "148.234.140.9"
                                      where ex.fecha >= fecha_correcta
                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                      from medicosIn in mediX.DefaultIfEmpty()
                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Urolo
                                      where especiIn.CLAVE == "18"
                                      //Otorrino
                                      || especiIn.CLAVE == "07"
                                      select new
                                      {
                                          //clave = especiIn.CLAVE,
                                          medico = ex.medico,
                                          fecha = ex.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                          consultadistancia = ex.consultadistancia
                                      })
                              .ToList().Select(x => new
                              {
                                  //clave = x.clave,
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  //clave = z.clave,
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  fecha = z.fecha,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  //p.clave,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = g.Key.fecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

                    var resultsDet1 = new List<IndEspListDet>();

                    foreach (var item in expediedet)
                    {
                        var resultDet = new IndEspListDet
                        {
                            medico = item.medico,
                            nombre = item.nombre,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        resultsDet1.Add(resultDet);
                    }

                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                    string path2 = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);

                }


                if (User.IsInRole("Productividad5"))
                {
                    var expediente = (from q in db.expediente
                                          //where q.medico.StartsWith(item.CLAVE)
                                      where !q.ip_realiza.StartsWith("148.234.64")
                                      where q.ip_realiza != "148.234.143.203"
                                      where q.ip_realiza != "148.234.140.9"
                                      where q.fecha >= fecha_correcta
                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Nutricion
                                      where especiIn.CLAVE == "26"
                                      //Psico
                                      || especiIn.CLAVE == "21"
                                      select new
                                      {
                                          clave = especiIn.CLAVE,
                                          fecha = q.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          consultadistancia = q.consultadistancia
                                      })
                                  .ToList().Select(x => new
                                  {
                                      clave = x.clave,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.especialidad,
                                      p.fecha,
                                      p.clave,
                                  })
                                 .Select(g => new
                                 {
                                     clave = g.Key.clave,
                                     especialidad = g.Key.especialidad,
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 .OrderBy(g => g.especialidad)
                                 .ToList();

                    var results1 = new List<IndEspList>();

                    foreach (var item in expediente)
                    {
                        var result = new IndEspList
                        {
                            clave = item.clave,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        results1.Add(result);
                    }

                    //System.Diagnostics.Debug.WriteLine(results1);

                    string json = JsonConvert.SerializeObject(results1);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad.json", json);


                    //DETALLES
                    var expediedet = (from ex in db.expediente
                                      where !ex.ip_realiza.StartsWith("148.234.64")
                                      where ex.ip_realiza != "148.234.143.203"
                                      where ex.ip_realiza != "148.234.140.9"
                                      where ex.fecha >= fecha_correcta
                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                      from medicosIn in mediX.DefaultIfEmpty()
                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Nutricion
                                      where especiIn.CLAVE == "26"
                                      //Psico
                                      || especiIn.CLAVE == "21"
                                      select new
                                      {
                                          //clave = especiIn.CLAVE,
                                          medico = ex.medico,
                                          fecha = ex.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                          consultadistancia = ex.consultadistancia
                                      })
                              .ToList().Select(x => new
                              {
                                  //clave = x.clave,
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  //clave = z.clave,
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  fecha = z.fecha,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  //p.clave,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = g.Key.fecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

                    var resultsDet1 = new List<IndEspListDet>();

                    foreach (var item in expediedet)
                    {
                        var resultDet = new IndEspListDet
                        {
                            medico = item.medico,
                            nombre = item.nombre,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        resultsDet1.Add(resultDet);
                    }

                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                    string path2 = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);

                }

                //MEDEROS
                if (User.IsInRole("Productividad6"))
                {
                    var expediente = (from q in db.expediente
                                      where q.ip_realiza.StartsWith("148.234.64")
                                      where q.fecha >= fecha_correcta
                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                      select new
                                      {
                                          clave = especiIn.CLAVE,
                                          fecha = q.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          consultadistancia = q.consultadistancia
                                      })
                                  .ToList().Select(x => new
                                  {
                                      clave = x.clave,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.especialidad,
                                      p.fecha,
                                      p.clave,
                                  })
                                 .Select(g => new
                                 {
                                     clave = g.Key.clave,
                                     especialidad = g.Key.especialidad,
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 .OrderBy(g => g.especialidad)
                                 .ToList();

                    var results1 = new List<IndEspList>();

                    foreach (var item in expediente)
                    {
                        var result = new IndEspList
                        {
                            clave = item.clave,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        results1.Add(result);
                    }

                    //System.Diagnostics.Debug.WriteLine(results1);

                    string json = JsonConvert.SerializeObject(results1);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad.json", json);


                    //DETALLES
                    var expediedet = (from ex in db.expediente
                                      where ex.ip_realiza.StartsWith("148.234.64")
                                      where ex.fecha >= fecha_correcta
                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                      from medicosIn in mediX.DefaultIfEmpty()
                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                      select new
                                      {
                                          //clave = especiIn.CLAVE,
                                          medico = ex.medico,
                                          fecha = ex.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                          consultadistancia = ex.consultadistancia
                                      })
                              .ToList().Select(x => new
                              {
                                  //clave = x.clave,
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  //clave = z.clave,
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  fecha = z.fecha,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  //p.clave,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = g.Key.fecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

                    var resultsDet1 = new List<IndEspListDet>();

                    foreach (var item in expediedet)
                    {
                        var resultDet = new IndEspListDet
                        {
                            medico = item.medico,
                            nombre = item.nombre,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        resultsDet1.Add(resultDet);
                    }

                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                    string path2 = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);

                }

                //DENTAL
                if (User.IsInRole("Productividad7"))
                {
                    string query = "SELECT Dertbl.fecha as fecha, Count(*) as count, Count(*) as telefonica, Count(*) as presencial FROM( SELECT expediente_dental.expediente as expediente, Left(expediente_dental.medico, 2) as clave, ESPECIALIDADES.DESCRIPCION as especialidad, expediente_dental.fecha, COUNT(*) as count FROM expediente_dental, ESPECIALIDADES WHERE ESPECIALIDADES.CLAVE = Left(expediente_dental.medico, 2) and expediente_dental.fecha = '" + fecha + "' GROUP BY fecha, Left(medico, 2), ESPECIALIDADES.DESCRIPCION, expediente_dental.expediente ) AS Dertbl GROUP BY Dertbl.fecha";

                    var resultDen = db.Database.SqlQuery<Dental>(query);
                    var expedi_den = resultDen.ToList();

                    var results1 = new List<IndEspList>();

                    foreach (var item in expedi_den)
                    {
                        var result2 = new IndEspList
                        {
                            clave = "04",
                            especialidad = "DENTAL",
                            fecha = fechaHoy,
                            count = item.count,
                            telefonica = 0,
                            presencial = item.count,
                        };

                        results1.Add(result2);

                    }

                    string json = JsonConvert.SerializeObject(results1);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad.json", json);



                    string queryDet = "SELECT Dertbl.especialidad as especialidad, Dertbl.medico as medico, Dertbl.nombre as nombre, Dertbl.fecha as fecha, Count(*) as count, Count(*) as telefonica, Count(*) as presencial  FROM ( SELECT expediente_dental.medico as medico, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as nombre, ESPECIALIDADES.DESCRIPCION as especialidad, expediente_dental.fecha, COUNT(*) as count FROM expediente_dental, ESPECIALIDADES, MEDICOS WHERE ESPECIALIDADES.CLAVE = Left(expediente_dental.medico, 2) and expediente_dental.fecha = '" + fecha + "' AND MEDICOS.Numero = expediente_dental.medico GROUP BY fecha, ESPECIALIDADES.DESCRIPCION, medico, MEDICOS.Nombre, MEDICOS.Apaterno, MEDICOS.Amaterno, expediente_dental.expediente) AS Dertbl GROUP BY Dertbl.medico, Dertbl.fecha, Dertbl.nombre, Dertbl.especialidad;";

                    var resultDenDet = db.Database.SqlQuery<Dental>(queryDet);
                    var expedi_den_det = resultDenDet.ToList();


                    var resultsDet1 = new List<IndEspListDet>();


                    foreach (var item in expedi_den_det)
                    {
                        var resultDet2 = new IndEspListDet
                        {
                            medico = item.medico,
                            nombre = item.nombre,
                            especialidad = "DENTAL",
                            fecha = fechaHoy,
                            count = item.count,
                            telefonica = 0,
                            presencial = item.presencial,
                        };

                        resultsDet1.Add(resultDet2);

                    }


                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                    string path2 = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);

                }


                if (User.IsInRole("Productividad8"))
                {
                    var expediente = (from q in db.expediente
                                          //where q.medico.StartsWith(item.CLAVE)
                                      where !q.ip_realiza.StartsWith("148.234.64")
                                      where q.ip_realiza != "148.234.143.203"
                                      where q.ip_realiza != "148.234.140.9"
                                      where q.fecha >= fecha_correcta
                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Cirugia General
                                      where especiIn.CLAVE == "06"
                                      select new
                                      {
                                          clave = especiIn.CLAVE,
                                          fecha = q.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          consultadistancia = q.consultadistancia
                                      })
                                  .ToList().Select(x => new
                                  {
                                      clave = x.clave,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.especialidad,
                                      p.fecha,
                                      p.clave,
                                  })
                                 .Select(g => new
                                 {
                                     clave = g.Key.clave,
                                     especialidad = g.Key.especialidad,
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 .OrderBy(g => g.especialidad)
                                 .ToList();

                    var results1 = new List<IndEspList>();

                    foreach (var item in expediente)
                    {
                        var result = new IndEspList
                        {
                            clave = item.clave,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        results1.Add(result);
                    }

                    //System.Diagnostics.Debug.WriteLine(results1);

                    string json = JsonConvert.SerializeObject(results1);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad.json", json);


                    //DETALLES
                    var expediedet = (from ex in db.expediente
                                      where !ex.ip_realiza.StartsWith("148.234.64")
                                      where ex.ip_realiza != "148.234.143.203"
                                      where ex.ip_realiza != "148.234.140.9"
                                      where ex.fecha >= fecha_correcta
                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                      from medicosIn in mediX.DefaultIfEmpty()
                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Cirugia General
                                      where especiIn.CLAVE == "06"
                                      select new
                                      {
                                          //clave = especiIn.CLAVE,
                                          medico = ex.medico,
                                          fecha = ex.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                          consultadistancia = ex.consultadistancia
                                      })
                              .ToList().Select(x => new
                              {
                                  //clave = x.clave,
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  //clave = z.clave,
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  fecha = z.fecha,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  //p.clave,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = g.Key.fecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

                    var resultsDet1 = new List<IndEspListDet>();

                    foreach (var item in expediedet)
                    {
                        var resultDet = new IndEspListDet
                        {
                            medico = item.medico,
                            nombre = item.nombre,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        resultsDet1.Add(resultDet);
                    }

                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                    string path2 = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);

                }

                //Pediatria
                if (User.IsInRole("Productividad9"))
                {
                    var expediente = (from q in db.expediente
                                          //where q.medico.StartsWith(item.CLAVE)
                                      where !q.ip_realiza.StartsWith("148.234.64")
                                      where q.ip_realiza != "148.234.143.203"
                                      where q.ip_realiza != "148.234.140.9"
                                      where q.fecha >= fecha_correcta
                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Mora Puga
                                      where q.medico == "14034"
                                      //Pediatria
                                      || especiIn.CLAVE == "03"
                                      //Alergia Pediatria
                                      || especiIn.CLAVE == "54"

                                      select new
                                      {
                                          clave = especiIn.CLAVE,
                                          fecha = q.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          consultadistancia = q.consultadistancia
                                      })
                                  .ToList().Select(x => new
                                  {
                                      clave = x.clave,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.especialidad,
                                      p.fecha,
                                      p.clave,
                                  })
                                 .Select(g => new
                                 {
                                     clave = g.Key.clave,
                                     especialidad = g.Key.especialidad,
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 .OrderBy(g => g.especialidad)
                                 .ToList();

                    var results1 = new List<IndEspList>();

                    foreach (var item in expediente)
                    {
                        var result = new IndEspList
                        {
                            clave = item.clave,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        results1.Add(result);
                    }

                    //System.Diagnostics.Debug.WriteLine(results1);

                    string json = JsonConvert.SerializeObject(results1);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad.json", json);


                    //DETALLES
                    var expediedet = (from ex in db.expediente
                                      where !ex.ip_realiza.StartsWith("148.234.64")
                                      where ex.ip_realiza != "148.234.143.203"
                                      where ex.ip_realiza != "148.234.140.9"
                                      where ex.fecha >= fecha_correcta
                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                      from medicosIn in mediX.DefaultIfEmpty()
                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Mora Puga
                                      where ex.medico == "14034"
                                      //Pediatria
                                      || especiIn.CLAVE == "03"
                                      //Alergia Pediatria
                                      || especiIn.CLAVE == "54"
                                      select new
                                      {
                                          //clave = especiIn.CLAVE,
                                          medico = ex.medico,
                                          fecha = ex.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                          consultadistancia = ex.consultadistancia
                                      })
                              .ToList().Select(x => new
                              {
                                  //clave = x.clave,
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  //clave = z.clave,
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  fecha = z.fecha,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  //p.clave,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = g.Key.fecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

                    var resultsDet1 = new List<IndEspListDet>();

                    foreach (var item in expediedet)
                    {
                        var resultDet = new IndEspListDet
                        {
                            medico = item.medico,
                            nombre = item.nombre,
                            especialidad = item.especialidad,
                            fecha = item.fecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        resultsDet1.Add(resultDet);
                    }

                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                    string path2 = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);

                }

                //LINARES
                if (User.IsInRole("Productividad10"))
                {
                    return RedirectToAction("Productividad10", "Indicadores");

                }



                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }




        }


        public JsonResult ChangeIndicadoresEspes(string minDate, string maxDate)
        {

            //System.Diagnostics.Debug.WriteLine(minDate);

            var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

            //System.Diagnostics.Debug.WriteLine(fechaHoy);


            var fecha_minDate = DateTime.Parse(minDate);
            var fecha_maxDate = DateTime.Parse(maxDate);

            //System.Diagnostics.Debug.WriteLine(fecha_minDate);

            var textoFecha = "";

            if (minDate == maxDate)
            {
                textoFecha = minDate;
            }
            else
            {
                textoFecha = "De: " + minDate + " Hasta: " + maxDate;
            }

            //System.Diagnostics.Debug.WriteLine(fecha_minDate);


            Models.SMDEVEntities28 db = new Models.SMDEVEntities28();

            if (User.IsInRole("Productividad1"))
            {
                var expediente = (from q in db.expediente
                                  where !q.ip_realiza.StartsWith("148.234.64")
                                  where q.ip_realiza != "148.234.143.203"
                                  where q.ip_realiza != "148.234.140.9"
                                  where q.fecha >= fecha_minDate
                                  where q.fecha <= fecha_maxDate
                                  join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                  from especiIn in especiX.DefaultIfEmpty()
                                  where especiIn.CLAVE == "05"
                                  select new
                                  {
                                      clave = especiIn.CLAVE,
                                      //fecha = q.fecha,
                                      especialidad = especiIn.DESCRIPCION,
                                      consultadistancia = q.consultadistancia
                                  })
                              .ToList().Select(x => new
                              {
                                  clave = x.clave,
                                  especialidad = x.especialidad,
                                  //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  clave = z.clave,
                                  especialidad = z.especialidad,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.especialidad,
                                  //p.fecha,
                                  p.clave,
                              })
                             .Select(g => new
                             {
                                 clave = g.Key.clave,
                                 especialidad = g.Key.especialidad,
                                 fecha = textoFecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.especialidad)
                             .ToList();

                //System.Diagnostics.Debug.WriteLine(expedi_den);

                var results1 = new List<IndEspList>();

                foreach (var item in expediente)
                {
                    var result = new IndEspList
                    {
                        clave = item.especialidad,
                        especialidad = item.especialidad,
                        fecha = textoFecha,
                        count = item.count,
                        telefonica = item.telefonica,
                        presencial = item.presencial,
                    };

                    results1.Add(result);
                }


                string json = JsonConvert.SerializeObject(results1);
                string path = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad_busqueda.json", json);



                //DETALLES
                var expediedet = (from ex in db.expediente
                                  where !ex.ip_realiza.StartsWith("148.234.64")
                                  where ex.ip_realiza != "148.234.143.203"
                                  where ex.ip_realiza != "148.234.140.9"
                                  where ex.fecha >= fecha_minDate
                                  where ex.fecha <= fecha_maxDate
                                  //where ex.medico.Substring(0, 2) == espec.CLAVE
                                  join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                  from medicosIn in mediX.DefaultIfEmpty()
                                  join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                  from especiIn in especiX.DefaultIfEmpty()
                                  where especiIn.CLAVE == "05"
                                  select new
                                  {
                                      //clave = especiIn.CLAVE,
                                      medico = ex.medico,
                                      //fecha = q.fecha,
                                      especialidad = especiIn.DESCRIPCION,
                                      nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                      consultadistancia = ex.consultadistancia
                                  })
                              .ToList().Select(x => new
                              {
                                  //clave = x.clave,
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  //clave = z.clave,
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  //p.fecha,
                                  //p.clave,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = textoFecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

                var resultsDet1 = new List<IndEspListDet>();

                foreach (var item in expediedet)
                {
                    var resultDet = new IndEspListDet
                    {
                        medico = item.medico,
                        nombre = item.nombre,
                        especialidad = item.especialidad,
                        fecha = textoFecha,
                        count = item.count,
                        telefonica = item.telefonica,
                        presencial = item.presencial,
                    };

                    resultsDet1.Add(resultDet);
                }


                string json2 = JsonConvert.SerializeObject(resultsDet1);
                string path2 = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);


                return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                if (User.IsInRole("Productividad2"))
                {
                    var expediente = (from q in db.expediente
                                      where !q.ip_realiza.StartsWith("148.234.64")
                                      where q.ip_realiza != "148.234.143.203"
                                      where q.ip_realiza != "148.234.140.9"
                                      where q.fecha >= fecha_minDate
                                      where q.fecha <= fecha_maxDate
                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Derma
                                      where especiIn.CLAVE == "12"
                                      //Reuma
                                      || especiIn.CLAVE == "23"
                                      //Neumo
                                      || especiIn.CLAVE == "16"
                                      //Nefro
                                      || especiIn.CLAVE == "17"
                                      //Neuro
                                      || especiIn.CLAVE == "14"
                                      //Gastro
                                      || especiIn.CLAVE == "37"
                                      //Hemato
                                      || especiIn.CLAVE == "36"
                                      //Infec
                                      || especiIn.CLAVE == "33"
                                      //Medicina Interna
                                      || especiIn.CLAVE == "13"
                                      //Alergias
                                      || especiIn.CLAVE == "11"
                                      //Cardio
                                      || especiIn.CLAVE == "34"
                                      //Geriatria
                                      || especiIn.CLAVE == "60"
                                      //Psiquiatras
                                      || especiIn.CLAVE == "15"
                                      //Algologia Medicina del dolor
                                      || especiIn.CLAVE == "63"
                                      select new
                                      {
                                          clave = especiIn.CLAVE,
                                          //fecha = q.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          consultadistancia = q.consultadistancia
                                      })
                                  .ToList().Select(x => new
                                  {
                                      clave = x.clave,
                                      especialidad = x.especialidad,
                                      //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .ToList().Select(z => new
                                  {
                                      clave = z.clave,
                                      especialidad = z.especialidad,
                                      //fecha = DateTime.Parse(z.fecha),
                                      consultadistancia = z.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.especialidad,
                                      //p.fecha,
                                      p.clave,
                                  })
                                 .Select(g => new
                                 {
                                     clave = g.Key.clave,
                                     especialidad = g.Key.especialidad,
                                     fecha = textoFecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 //.OrderBy(g => g.fecha)
                                 .OrderBy(g => g.especialidad)
                                 .ToList();

                    //System.Diagnostics.Debug.WriteLine(expedi_den);

                    var results1 = new List<IndEspList>();

                    foreach (var item in expediente)
                    {
                        var result = new IndEspList
                        {
                            clave = item.especialidad,
                            especialidad = item.especialidad,
                            fecha = textoFecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        results1.Add(result);
                    }


                    string json = JsonConvert.SerializeObject(results1);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad_busqueda.json", json);



                    //DETALLES
                    var expediedet = (from ex in db.expediente
                                      where !ex.ip_realiza.StartsWith("148.234.64")
                                      where ex.ip_realiza != "148.234.143.203"
                                      where ex.ip_realiza != "148.234.140.9"
                                      where ex.fecha >= fecha_minDate
                                      where ex.fecha <= fecha_maxDate
                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                      from medicosIn in mediX.DefaultIfEmpty()
                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                      from especiIn in especiX.DefaultIfEmpty()
                                          //Derma
                                      where especiIn.CLAVE == "12"
                                      //Reuma
                                      || especiIn.CLAVE == "23"
                                      //Neumo
                                      || especiIn.CLAVE == "16"
                                      //Nefro
                                      || especiIn.CLAVE == "17"
                                      //Neuro
                                      || especiIn.CLAVE == "14"
                                      //Gastro
                                      || especiIn.CLAVE == "37"
                                      //Hemato
                                      || especiIn.CLAVE == "36"
                                      //Infec
                                      || especiIn.CLAVE == "33"
                                      //Medicina Interna
                                      || especiIn.CLAVE == "13"
                                      //Alergias
                                      || especiIn.CLAVE == "11"
                                      //Cardio
                                      || especiIn.CLAVE == "34"
                                      //Geriatria
                                      || especiIn.CLAVE == "60"
                                      select new
                                      {
                                          //clave = especiIn.CLAVE,
                                          medico = ex.medico,
                                          //fecha = q.fecha,
                                          especialidad = especiIn.DESCRIPCION,
                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                          consultadistancia = ex.consultadistancia
                                      })
                                  .ToList().Select(x => new
                                  {
                                      //clave = x.clave,
                                      medico = x.medico,
                                      nombre = x.nombre,
                                      especialidad = x.especialidad,
                                      //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .ToList().Select(z => new
                                  {
                                      //clave = z.clave,
                                      medico = z.medico,
                                      nombre = z.nombre,
                                      especialidad = z.especialidad,
                                      //fecha = DateTime.Parse(z.fecha),
                                      consultadistancia = z.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.nombre,
                                      //p.fecha,
                                      //p.clave,
                                      p.medico,
                                      p.especialidad
                                  })
                                 .Select(g => new
                                 {
                                     //clave = g.Key.clave,
                                     medico = g.Key.medico,
                                     nombre = g.Key.nombre,
                                     especialidad = g.Key.especialidad,
                                     fecha = textoFecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 //.OrderBy(g => g.fecha)
                                 .OrderBy(g => g.medico)
                                 .ToList();

                    var resultsDet1 = new List<IndEspListDet>();

                    foreach (var item in expediedet)
                    {
                        var resultDet = new IndEspListDet
                        {
                            medico = item.medico,
                            nombre = item.nombre,
                            especialidad = item.especialidad,
                            fecha = textoFecha,
                            count = item.count,
                            telefonica = item.telefonica,
                            presencial = item.presencial,
                        };

                        resultsDet1.Add(resultDet);
                    }


                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                    string path2 = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);


                    return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    if (User.IsInRole("Productividad3"))
                    {
                        var expediente = (from q in db.expediente
                                          where !q.ip_realiza.StartsWith("148.234.64")
                                          where q.ip_realiza != "148.234.143.203"
                                          where q.ip_realiza != "148.234.140.9"
                                          where q.fecha >= fecha_minDate
                                          where q.fecha <= fecha_maxDate
                                          join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                          from especiIn in especiX.DefaultIfEmpty()
                                              //Gine
                                          where especiIn.CLAVE == "08"
                                          select new
                                          {
                                              clave = especiIn.CLAVE,
                                              //fecha = q.fecha,
                                              especialidad = especiIn.DESCRIPCION,
                                              consultadistancia = q.consultadistancia
                                          })
                                      .ToList().Select(x => new
                                      {
                                          clave = x.clave,
                                          especialidad = x.especialidad,
                                          //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                          consultadistancia = x.consultadistancia
                                      })
                                      .ToList().Select(z => new
                                      {
                                          clave = z.clave,
                                          especialidad = z.especialidad,
                                          //fecha = DateTime.Parse(z.fecha),
                                          consultadistancia = z.consultadistancia
                                      })
                                      .GroupBy(p => new
                                      {
                                          p.especialidad,
                                          //p.fecha,
                                          p.clave,
                                      })
                                     .Select(g => new
                                     {
                                         clave = g.Key.clave,
                                         especialidad = g.Key.especialidad,
                                         fecha = textoFecha,
                                         count = g.Count(),
                                         telefonica = g.Count(p => p.consultadistancia == "1"),
                                         presencial = g.Count(p => p.consultadistancia != "1"),
                                     })
                                     //.OrderBy(g => g.fecha)
                                     .OrderBy(g => g.especialidad)
                                     .ToList();

                        //System.Diagnostics.Debug.WriteLine(expedi_den);

                        var results1 = new List<IndEspList>();

                        foreach (var item in expediente)
                        {
                            var result = new IndEspList
                            {
                                clave = item.especialidad,
                                especialidad = item.especialidad,
                                fecha = textoFecha,
                                count = item.count,
                                telefonica = item.telefonica,
                                presencial = item.presencial,
                            };

                            results1.Add(result);
                        }


                        string json = JsonConvert.SerializeObject(results1);
                        string path = Server.MapPath("~/json/");
                        System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad_busqueda.json", json);



                        //DETALLES
                        var expediedet = (from ex in db.expediente
                                          where !ex.ip_realiza.StartsWith("148.234.64")
                                          where ex.ip_realiza != "148.234.143.203"
                                          where ex.ip_realiza != "148.234.140.9"
                                          where ex.fecha >= fecha_minDate
                                          where ex.fecha <= fecha_maxDate
                                          //where ex.medico.Substring(0, 2) == espec.CLAVE
                                          join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                          from medicosIn in mediX.DefaultIfEmpty()
                                          join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                          from especiIn in especiX.DefaultIfEmpty()
                                              //Gine
                                          where especiIn.CLAVE == "08"
                                          select new
                                          {
                                              //clave = especiIn.CLAVE,
                                              medico = ex.medico,
                                              //fecha = q.fecha,
                                              especialidad = especiIn.DESCRIPCION,
                                              nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                              consultadistancia = ex.consultadistancia
                                          })
                                      .ToList().Select(x => new
                                      {
                                          //clave = x.clave,
                                          medico = x.medico,
                                          nombre = x.nombre,
                                          especialidad = x.especialidad,
                                          //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                          consultadistancia = x.consultadistancia
                                      })
                                      .ToList().Select(z => new
                                      {
                                          //clave = z.clave,
                                          medico = z.medico,
                                          nombre = z.nombre,
                                          especialidad = z.especialidad,
                                          //fecha = DateTime.Parse(z.fecha),
                                          consultadistancia = z.consultadistancia
                                      })
                                      .GroupBy(p => new
                                      {
                                          p.nombre,
                                          //p.fecha,
                                          //p.clave,
                                          p.medico,
                                          p.especialidad
                                      })
                                     .Select(g => new
                                     {
                                         //clave = g.Key.clave,
                                         medico = g.Key.medico,
                                         nombre = g.Key.nombre,
                                         especialidad = g.Key.especialidad,
                                         fecha = textoFecha,
                                         count = g.Count(),
                                         telefonica = g.Count(p => p.consultadistancia == "1"),
                                         presencial = g.Count(p => p.consultadistancia != "1"),
                                     })
                                     //.OrderBy(g => g.fecha)
                                     .OrderBy(g => g.medico)
                                     .ToList();

                        var resultsDet1 = new List<IndEspListDet>();

                        foreach (var item in expediedet)
                        {
                            var resultDet = new IndEspListDet
                            {
                                medico = item.medico,
                                nombre = item.nombre,
                                especialidad = item.especialidad,
                                fecha = textoFecha,
                                count = item.count,
                                telefonica = item.telefonica,
                                presencial = item.presencial,
                            };

                            resultsDet1.Add(resultDet);
                        }


                        string json2 = JsonConvert.SerializeObject(resultsDet1);
                        string path2 = Server.MapPath("~/json/");
                        System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);


                        return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        if (User.IsInRole("Productividad4"))
                        {
                            var expediente = (from q in db.expediente
                                              where !q.ip_realiza.StartsWith("148.234.64")
                                              where q.ip_realiza != "148.234.143.203"
                                              where q.ip_realiza != "148.234.140.9"
                                              where q.fecha >= fecha_minDate
                                              where q.fecha <= fecha_maxDate
                                              join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                              from especiIn in especiX.DefaultIfEmpty()
                                                  //Urolo
                                              where especiIn.CLAVE == "18"
                                              //Otorrino
                                              || especiIn.CLAVE == "07"
                                              select new
                                              {
                                                  clave = especiIn.CLAVE,
                                                  //fecha = q.fecha,
                                                  especialidad = especiIn.DESCRIPCION,
                                                  consultadistancia = q.consultadistancia
                                              })
                                          .ToList().Select(x => new
                                          {
                                              clave = x.clave,
                                              especialidad = x.especialidad,
                                              //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                              consultadistancia = x.consultadistancia
                                          })
                                          .ToList().Select(z => new
                                          {
                                              clave = z.clave,
                                              especialidad = z.especialidad,
                                              //fecha = DateTime.Parse(z.fecha),
                                              consultadistancia = z.consultadistancia
                                          })
                                          .GroupBy(p => new
                                          {
                                              p.especialidad,
                                              //p.fecha,
                                              p.clave,
                                          })
                                         .Select(g => new
                                         {
                                             clave = g.Key.clave,
                                             especialidad = g.Key.especialidad,
                                             fecha = textoFecha,
                                             count = g.Count(),
                                             telefonica = g.Count(p => p.consultadistancia == "1"),
                                             presencial = g.Count(p => p.consultadistancia != "1"),
                                         })
                                         //.OrderBy(g => g.fecha)
                                         .OrderBy(g => g.especialidad)
                                         .ToList();

                            //System.Diagnostics.Debug.WriteLine(expedi_den);

                            var results1 = new List<IndEspList>();

                            foreach (var item in expediente)
                            {
                                var result = new IndEspList
                                {
                                    clave = item.especialidad,
                                    especialidad = item.especialidad,
                                    fecha = textoFecha,
                                    count = item.count,
                                    telefonica = item.telefonica,
                                    presencial = item.presencial,
                                };

                                results1.Add(result);
                            }


                            string json = JsonConvert.SerializeObject(results1);
                            string path = Server.MapPath("~/json/");
                            System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad_busqueda.json", json);



                            //DETALLES
                            var expediedet = (from ex in db.expediente
                                              where !ex.ip_realiza.StartsWith("148.234.64")
                                              where ex.ip_realiza != "148.234.143.203"
                                              where ex.ip_realiza != "148.234.140.9"
                                              where ex.fecha >= fecha_minDate
                                              where ex.fecha <= fecha_maxDate
                                              //where ex.medico.Substring(0, 2) == espec.CLAVE
                                              join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                              from medicosIn in mediX.DefaultIfEmpty()
                                              join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                              from especiIn in especiX.DefaultIfEmpty()
                                                  //Urolo
                                              where especiIn.CLAVE == "18"
                                              //Otorrino
                                              || especiIn.CLAVE == "07"
                                              select new
                                              {
                                                  //clave = especiIn.CLAVE,
                                                  medico = ex.medico,
                                                  //fecha = q.fecha,
                                                  especialidad = especiIn.DESCRIPCION,
                                                  nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                                  consultadistancia = ex.consultadistancia
                                              })
                                          .ToList().Select(x => new
                                          {
                                              //clave = x.clave,
                                              medico = x.medico,
                                              nombre = x.nombre,
                                              especialidad = x.especialidad,
                                              //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                              consultadistancia = x.consultadistancia
                                          })
                                          .ToList().Select(z => new
                                          {
                                              //clave = z.clave,
                                              medico = z.medico,
                                              nombre = z.nombre,
                                              especialidad = z.especialidad,
                                              //fecha = DateTime.Parse(z.fecha),
                                              consultadistancia = z.consultadistancia
                                          })
                                          .GroupBy(p => new
                                          {
                                              p.nombre,
                                              //p.fecha,
                                              //p.clave,
                                              p.medico,
                                              p.especialidad
                                          })
                                         .Select(g => new
                                         {
                                             //clave = g.Key.clave,
                                             medico = g.Key.medico,
                                             nombre = g.Key.nombre,
                                             especialidad = g.Key.especialidad,
                                             fecha = textoFecha,
                                             count = g.Count(),
                                             telefonica = g.Count(p => p.consultadistancia == "1"),
                                             presencial = g.Count(p => p.consultadistancia != "1"),
                                         })
                                         //.OrderBy(g => g.fecha)
                                         .OrderBy(g => g.medico)
                                         .ToList();

                            var resultsDet1 = new List<IndEspListDet>();

                            foreach (var item in expediedet)
                            {
                                var resultDet = new IndEspListDet
                                {
                                    medico = item.medico,
                                    nombre = item.nombre,
                                    especialidad = item.especialidad,
                                    fecha = textoFecha,
                                    count = item.count,
                                    telefonica = item.telefonica,
                                    presencial = item.presencial,
                                };

                                resultsDet1.Add(resultDet);
                            }


                            string json2 = JsonConvert.SerializeObject(resultsDet1);
                            string path2 = Server.MapPath("~/json/");
                            System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);


                            return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                        else
                        {
                            if (User.IsInRole("Productividad5"))
                            {
                                var expediente = (from q in db.expediente
                                                  where !q.ip_realiza.StartsWith("148.234.64")
                                                  where q.ip_realiza != "148.234.143.203"
                                                  where q.ip_realiza != "148.234.140.9"
                                                  where q.fecha >= fecha_minDate
                                                  where q.fecha <= fecha_maxDate
                                                  join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                                  from especiIn in especiX.DefaultIfEmpty()
                                                      //Nutricion
                                                  where especiIn.CLAVE == "26"
                                                  //Psico
                                                  || especiIn.CLAVE == "21"
                                                  select new
                                                  {
                                                      clave = especiIn.CLAVE,
                                                      //fecha = q.fecha,
                                                      especialidad = especiIn.DESCRIPCION,
                                                      consultadistancia = q.consultadistancia
                                                  })
                                              .ToList().Select(x => new
                                              {
                                                  clave = x.clave,
                                                  especialidad = x.especialidad,
                                                  //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                                  consultadistancia = x.consultadistancia
                                              })
                                              .ToList().Select(z => new
                                              {
                                                  clave = z.clave,
                                                  especialidad = z.especialidad,
                                                  //fecha = DateTime.Parse(z.fecha),
                                                  consultadistancia = z.consultadistancia
                                              })
                                              .GroupBy(p => new
                                              {
                                                  p.especialidad,
                                                  //p.fecha,
                                                  p.clave,
                                              })
                                             .Select(g => new
                                             {
                                                 clave = g.Key.clave,
                                                 especialidad = g.Key.especialidad,
                                                 fecha = textoFecha,
                                                 count = g.Count(),
                                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                                 presencial = g.Count(p => p.consultadistancia != "1"),
                                             })
                                             //.OrderBy(g => g.fecha)
                                             .OrderBy(g => g.especialidad)
                                             .ToList();

                                //System.Diagnostics.Debug.WriteLine(expedi_den);

                                var results1 = new List<IndEspList>();

                                foreach (var item in expediente)
                                {
                                    var result = new IndEspList
                                    {
                                        clave = item.especialidad,
                                        especialidad = item.especialidad,
                                        fecha = textoFecha,
                                        count = item.count,
                                        telefonica = item.telefonica,
                                        presencial = item.presencial,
                                    };

                                    results1.Add(result);
                                }


                                string json = JsonConvert.SerializeObject(results1);
                                string path = Server.MapPath("~/json/");
                                System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad_busqueda.json", json);



                                //DETALLES
                                var expediedet = (from ex in db.expediente
                                                  where !ex.ip_realiza.StartsWith("148.234.64")
                                                  where ex.ip_realiza != "148.234.143.203"
                                                  where ex.ip_realiza != "148.234.140.9"
                                                  where ex.fecha >= fecha_minDate
                                                  where ex.fecha <= fecha_maxDate
                                                  //where ex.medico.Substring(0, 2) == espec.CLAVE
                                                  join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                                  from medicosIn in mediX.DefaultIfEmpty()
                                                  join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                                  from especiIn in especiX.DefaultIfEmpty()
                                                      //Nutricion
                                                  where especiIn.CLAVE == "26"
                                                  //Psico
                                                  || especiIn.CLAVE == "21"
                                                  select new
                                                  {
                                                      //clave = especiIn.CLAVE,
                                                      medico = ex.medico,
                                                      //fecha = q.fecha,
                                                      especialidad = especiIn.DESCRIPCION,
                                                      nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                                      consultadistancia = ex.consultadistancia
                                                  })
                                              .ToList().Select(x => new
                                              {
                                                  //clave = x.clave,
                                                  medico = x.medico,
                                                  nombre = x.nombre,
                                                  especialidad = x.especialidad,
                                                  //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                                  consultadistancia = x.consultadistancia
                                              })
                                              .ToList().Select(z => new
                                              {
                                                  //clave = z.clave,
                                                  medico = z.medico,
                                                  nombre = z.nombre,
                                                  especialidad = z.especialidad,
                                                  //fecha = DateTime.Parse(z.fecha),
                                                  consultadistancia = z.consultadistancia
                                              })
                                              .GroupBy(p => new
                                              {
                                                  p.nombre,
                                                  //p.fecha,
                                                  //p.clave,
                                                  p.medico,
                                                  p.especialidad
                                              })
                                             .Select(g => new
                                             {
                                                 //clave = g.Key.clave,
                                                 medico = g.Key.medico,
                                                 nombre = g.Key.nombre,
                                                 especialidad = g.Key.especialidad,
                                                 fecha = textoFecha,
                                                 count = g.Count(),
                                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                                 presencial = g.Count(p => p.consultadistancia != "1"),
                                             })
                                             //.OrderBy(g => g.fecha)
                                             .OrderBy(g => g.medico)
                                             .ToList();

                                var resultsDet1 = new List<IndEspListDet>();

                                foreach (var item in expediedet)
                                {
                                    var resultDet = new IndEspListDet
                                    {
                                        medico = item.medico,
                                        nombre = item.nombre,
                                        especialidad = item.especialidad,
                                        fecha = textoFecha,
                                        count = item.count,
                                        telefonica = item.telefonica,
                                        presencial = item.presencial,
                                    };

                                    resultsDet1.Add(resultDet);
                                }


                                string json2 = JsonConvert.SerializeObject(resultsDet1);
                                string path2 = Server.MapPath("~/json/");
                                System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);


                                return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                            }
                            else
                            {
                                //MEDEROS
                                if (User.IsInRole("Productividad6"))
                                {
                                    var expediente = (from q in db.expediente
                                                      where q.ip_realiza.StartsWith("148.234.64")
                                                      where q.fecha >= fecha_minDate
                                                      where q.fecha <= fecha_maxDate
                                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                                      from especiIn in especiX.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          clave = especiIn.CLAVE,
                                                          //fecha = q.fecha,
                                                          especialidad = especiIn.DESCRIPCION,
                                                          consultadistancia = q.consultadistancia
                                                      })
                                                  .ToList().Select(x => new
                                                  {
                                                      clave = x.clave,
                                                      especialidad = x.especialidad,
                                                      //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                                      consultadistancia = x.consultadistancia
                                                  })
                                                  .ToList().Select(z => new
                                                  {
                                                      clave = z.clave,
                                                      especialidad = z.especialidad,
                                                      //fecha = DateTime.Parse(z.fecha),
                                                      consultadistancia = z.consultadistancia
                                                  })
                                                  .GroupBy(p => new
                                                  {
                                                      p.especialidad,
                                                      //p.fecha,
                                                      p.clave,
                                                  })
                                                 .Select(g => new
                                                 {
                                                     clave = g.Key.clave,
                                                     especialidad = g.Key.especialidad,
                                                     fecha = textoFecha,
                                                     count = g.Count(),
                                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                                 })
                                                 //.OrderBy(g => g.fecha)
                                                 .OrderBy(g => g.especialidad)
                                                 .ToList();

                                    //System.Diagnostics.Debug.WriteLine(expedi_den);

                                    var results1 = new List<IndEspList>();

                                    foreach (var item in expediente)
                                    {
                                        var result = new IndEspList
                                        {
                                            clave = item.especialidad,
                                            especialidad = item.especialidad,
                                            fecha = textoFecha,
                                            count = item.count,
                                            telefonica = item.telefonica,
                                            presencial = item.presencial,
                                        };

                                        results1.Add(result);
                                    }


                                    string json = JsonConvert.SerializeObject(results1);
                                    string path = Server.MapPath("~/json/");
                                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad_busqueda.json", json);



                                    //DETALLES
                                    var expediedet = (from ex in db.expediente
                                                      where ex.ip_realiza.StartsWith("148.234.64")
                                                      where ex.fecha >= fecha_minDate
                                                      where ex.fecha <= fecha_maxDate
                                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                                      from medicosIn in mediX.DefaultIfEmpty()
                                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                                      from especiIn in especiX.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          //clave = especiIn.CLAVE,
                                                          medico = ex.medico,
                                                          //fecha = q.fecha,
                                                          especialidad = especiIn.DESCRIPCION,
                                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                                          consultadistancia = ex.consultadistancia
                                                      })
                                                  .ToList().Select(x => new
                                                  {
                                                      //clave = x.clave,
                                                      medico = x.medico,
                                                      nombre = x.nombre,
                                                      especialidad = x.especialidad,
                                                      //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                                      consultadistancia = x.consultadistancia
                                                  })
                                                  .ToList().Select(z => new
                                                  {
                                                      //clave = z.clave,
                                                      medico = z.medico,
                                                      nombre = z.nombre,
                                                      especialidad = z.especialidad,
                                                      //fecha = DateTime.Parse(z.fecha),
                                                      consultadistancia = z.consultadistancia
                                                  })
                                                  .GroupBy(p => new
                                                  {
                                                      p.nombre,
                                                      //p.fecha,
                                                      //p.clave,
                                                      p.medico,
                                                      p.especialidad
                                                  })
                                                 .Select(g => new
                                                 {
                                                     //clave = g.Key.clave,
                                                     medico = g.Key.medico,
                                                     nombre = g.Key.nombre,
                                                     especialidad = g.Key.especialidad,
                                                     fecha = textoFecha,
                                                     count = g.Count(),
                                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                                 })
                                                 //.OrderBy(g => g.fecha)
                                                 .OrderBy(g => g.medico)
                                                 .ToList();

                                    var resultsDet1 = new List<IndEspListDet>();

                                    foreach (var item in expediedet)
                                    {
                                        var resultDet = new IndEspListDet
                                        {
                                            medico = item.medico,
                                            nombre = item.nombre,
                                            especialidad = item.especialidad,
                                            fecha = textoFecha,
                                            count = item.count,
                                            telefonica = item.telefonica,
                                            presencial = item.presencial,
                                        };

                                        resultsDet1.Add(resultDet);
                                    }


                                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                                    string path2 = Server.MapPath("~/json/");
                                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);


                                    return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                                }
                                else
                                {
                                    //DENTAL
                                    if (User.IsInRole("Productividad7"))
                                    {

                                        string query = "SELECT COUNT(*) AS count FROM( SELECT expediente, fecha, medico, COUNT(*) AS count FROM expediente_dental WHERE fecha >= '" + minDate + "T00:00:00.000' and  fecha <= '" + maxDate + "T00:00:00.000' group by expediente, fecha, medico )AS Dertbl";
                                        var resultDen = db.Database.SqlQuery<Dental>(query);
                                        var expedi_den = resultDen.ToList();

                                        var results1 = new List<IndEspList>();

                                        foreach (var item in expedi_den)
                                        {
                                            var result2 = new IndEspList
                                            {
                                                clave = "04",
                                                especialidad = "DENTAL",
                                                fecha = textoFecha,
                                                count = item.count,
                                                telefonica = 0,
                                                presencial = item.count,
                                            };

                                            results1.Add(result2);

                                        }

                                        string json = JsonConvert.SerializeObject(results1);
                                        string path = Server.MapPath("~/json/");
                                        System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad_busqueda.json", json);


                                        //detalles
                                        string queryDet = "SELECT Dertbl.nombre as nombre, Dertbl.medico as medico, COUNT(*) AS count FROM ( SELECT fecha, expediente, medico, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as nombre FROM expediente_dental INNER JOIN MEDICOS ON MEDICOS.Numero = expediente_dental.medico WHERE fecha >= '" + minDate + "T00:00:00.000' and  fecha <= '" + maxDate + "T00:00:00.000' group by fecha, expediente, medico, MEDICOS.Nombre, MEDICOS.Apaterno, MEDICOS.Amaterno ) AS Dertbl GROUP BY Dertbl.medico, Dertbl.nombre;";
                                        var resultDenDet = db.Database.SqlQuery<Dental>(queryDet);
                                        var expedi_den_det = resultDenDet.ToList();


                                        var resultsDet1 = new List<IndEspListDet>();

                                        foreach (var item in expedi_den_det)
                                        {
                                            var resultDet2 = new IndEspListDet
                                            {
                                                medico = item.medico,
                                                nombre = item.nombre,
                                                especialidad = "DENTAL",
                                                fecha = textoFecha,
                                                count = item.count,
                                                telefonica = 0,
                                                presencial = item.count,
                                            };

                                            resultsDet1.Add(resultDet2);

                                        }

                                        string json2 = JsonConvert.SerializeObject(resultsDet1);
                                        string path2 = Server.MapPath("~/json/");
                                        System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);



                                        var expediente = "";
                                        return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                                    }
                                    else
                                    {
                                        if (User.IsInRole("Productividad8"))
                                        {
                                            var expediente = (from q in db.expediente
                                                              where !q.ip_realiza.StartsWith("148.234.64")
                                                              where q.ip_realiza != "148.234.143.203"
                                                              where q.ip_realiza != "148.234.140.9"
                                                              where q.fecha >= fecha_minDate
                                                              where q.fecha <= fecha_maxDate
                                                              join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                                              from especiIn in especiX.DefaultIfEmpty()
                                                                  //Ciguria General
                                                              where especiIn.CLAVE == "06"
                                                              select new
                                                              {
                                                                  clave = especiIn.CLAVE,
                                                                  //fecha = q.fecha,
                                                                  especialidad = especiIn.DESCRIPCION,
                                                                  consultadistancia = q.consultadistancia
                                                              })
                                                          .ToList().Select(x => new
                                                          {
                                                              clave = x.clave,
                                                              especialidad = x.especialidad,
                                                              //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                                              consultadistancia = x.consultadistancia
                                                          })
                                                          .ToList().Select(z => new
                                                          {
                                                              clave = z.clave,
                                                              especialidad = z.especialidad,
                                                              //fecha = DateTime.Parse(z.fecha),
                                                              consultadistancia = z.consultadistancia
                                                          })
                                                          .GroupBy(p => new
                                                          {
                                                              p.especialidad,
                                                              //p.fecha,
                                                              p.clave,
                                                          })
                                                         .Select(g => new
                                                         {
                                                             clave = g.Key.clave,
                                                             especialidad = g.Key.especialidad,
                                                             fecha = textoFecha,
                                                             count = g.Count(),
                                                             telefonica = g.Count(p => p.consultadistancia == "1"),
                                                             presencial = g.Count(p => p.consultadistancia != "1"),
                                                         })
                                                         //.OrderBy(g => g.fecha)
                                                         .OrderBy(g => g.especialidad)
                                                         .ToList();

                                            //System.Diagnostics.Debug.WriteLine(expedi_den);

                                            var results1 = new List<IndEspList>();

                                            foreach (var item in expediente)
                                            {
                                                var result = new IndEspList
                                                {
                                                    clave = item.especialidad,
                                                    especialidad = item.especialidad,
                                                    fecha = textoFecha,
                                                    count = item.count,
                                                    telefonica = item.telefonica,
                                                    presencial = item.presencial,
                                                };

                                                results1.Add(result);
                                            }


                                            string json = JsonConvert.SerializeObject(results1);
                                            string path = Server.MapPath("~/json/");
                                            System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad_busqueda.json", json);



                                            //DETALLES
                                            var expediedet = (from ex in db.expediente
                                                              where !ex.ip_realiza.StartsWith("148.234.64")
                                                              where ex.ip_realiza != "148.234.143.203"
                                                              where ex.ip_realiza != "148.234.140.9"
                                                              where ex.fecha >= fecha_minDate
                                                              where ex.fecha <= fecha_maxDate
                                                              //where ex.medico.Substring(0, 2) == espec.CLAVE
                                                              join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                                              from medicosIn in mediX.DefaultIfEmpty()
                                                              join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                                              from especiIn in especiX.DefaultIfEmpty()
                                                                  //Ciguria General
                                                              where especiIn.CLAVE == "06"
                                                              select new
                                                              {
                                                                  //clave = especiIn.CLAVE,
                                                                  medico = ex.medico,
                                                                  //fecha = q.fecha,
                                                                  especialidad = especiIn.DESCRIPCION,
                                                                  nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                                                  consultadistancia = ex.consultadistancia
                                                              })
                                                          .ToList().Select(x => new
                                                          {
                                                              //clave = x.clave,
                                                              medico = x.medico,
                                                              nombre = x.nombre,
                                                              especialidad = x.especialidad,
                                                              //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                                              consultadistancia = x.consultadistancia
                                                          })
                                                          .ToList().Select(z => new
                                                          {
                                                              //clave = z.clave,
                                                              medico = z.medico,
                                                              nombre = z.nombre,
                                                              especialidad = z.especialidad,
                                                              //fecha = DateTime.Parse(z.fecha),
                                                              consultadistancia = z.consultadistancia
                                                          })
                                                          .GroupBy(p => new
                                                          {
                                                              p.nombre,
                                                              //p.fecha,
                                                              //p.clave,
                                                              p.medico,
                                                              p.especialidad
                                                          })
                                                         .Select(g => new
                                                         {
                                                             //clave = g.Key.clave,
                                                             medico = g.Key.medico,
                                                             nombre = g.Key.nombre,
                                                             especialidad = g.Key.especialidad,
                                                             fecha = textoFecha,
                                                             count = g.Count(),
                                                             telefonica = g.Count(p => p.consultadistancia == "1"),
                                                             presencial = g.Count(p => p.consultadistancia != "1"),
                                                         })
                                                         //.OrderBy(g => g.fecha)
                                                         .OrderBy(g => g.medico)
                                                         .ToList();

                                            var resultsDet1 = new List<IndEspListDet>();

                                            foreach (var item in expediedet)
                                            {
                                                var resultDet = new IndEspListDet
                                                {
                                                    medico = item.medico,
                                                    nombre = item.nombre,
                                                    especialidad = item.especialidad,
                                                    fecha = textoFecha,
                                                    count = item.count,
                                                    telefonica = item.telefonica,
                                                    presencial = item.presencial,
                                                };

                                                resultsDet1.Add(resultDet);
                                            }


                                            string json2 = JsonConvert.SerializeObject(resultsDet1);
                                            string path2 = Server.MapPath("~/json/");
                                            System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);


                                            return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                                        }
                                        else
                                        {
                                            if (User.IsInRole("Productividad9"))
                                            {
                                                var expediente = (from q in db.expediente
                                                                  where !q.ip_realiza.StartsWith("148.234.64")
                                                                  where q.ip_realiza != "148.234.143.203"
                                                                  where q.ip_realiza != "148.234.140.9"
                                                                  where q.fecha >= fecha_minDate
                                                                  where q.fecha <= fecha_maxDate
                                                                  join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                                                  from especiIn in especiX.DefaultIfEmpty()
                                                                      //Mora Puga
                                                                  where q.medico == "14034"
                                                                  //Pediatria
                                                                  || especiIn.CLAVE == "03"
                                                                  //Alergia Pediatria
                                                                  || especiIn.CLAVE == "54"
                                                                  select new
                                                                  {
                                                                      clave = especiIn.CLAVE,
                                                                      //fecha = q.fecha,
                                                                      especialidad = especiIn.DESCRIPCION,
                                                                      consultadistancia = q.consultadistancia
                                                                  })
                                                              .ToList().Select(x => new
                                                              {
                                                                  clave = x.clave,
                                                                  especialidad = x.especialidad,
                                                                  //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                                                  consultadistancia = x.consultadistancia
                                                              })
                                                              .ToList().Select(z => new
                                                              {
                                                                  clave = z.clave,
                                                                  especialidad = z.especialidad,
                                                                  //fecha = DateTime.Parse(z.fecha),
                                                                  consultadistancia = z.consultadistancia
                                                              })
                                                              .GroupBy(p => new
                                                              {
                                                                  p.especialidad,
                                                                  //p.fecha,
                                                                  p.clave,
                                                              })
                                                             .Select(g => new
                                                             {
                                                                 clave = g.Key.clave,
                                                                 especialidad = g.Key.especialidad,
                                                                 fecha = textoFecha,
                                                                 count = g.Count(),
                                                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                                                 presencial = g.Count(p => p.consultadistancia != "1"),
                                                             })
                                                             //.OrderBy(g => g.fecha)
                                                             .OrderBy(g => g.especialidad)
                                                             .ToList();

                                                //System.Diagnostics.Debug.WriteLine(expedi_den);

                                                var results1 = new List<IndEspList>();

                                                foreach (var item in expediente)
                                                {
                                                    var result = new IndEspList
                                                    {
                                                        clave = item.especialidad,
                                                        especialidad = item.especialidad,
                                                        fecha = textoFecha,
                                                        count = item.count,
                                                        telefonica = item.telefonica,
                                                        presencial = item.presencial,
                                                    };

                                                    results1.Add(result);
                                                }


                                                string json = JsonConvert.SerializeObject(results1);
                                                string path = Server.MapPath("~/json/");
                                                System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad_busqueda.json", json);



                                                //DETALLES
                                                var expediedet = (from ex in db.expediente
                                                                  where !ex.ip_realiza.StartsWith("148.234.64")
                                                                  where ex.ip_realiza != "148.234.143.203"
                                                                  where ex.ip_realiza != "148.234.140.9"
                                                                  where ex.fecha >= fecha_minDate
                                                                  where ex.fecha <= fecha_maxDate
                                                                  //where ex.medico.Substring(0, 2) == espec.CLAVE
                                                                  join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                                                  from medicosIn in mediX.DefaultIfEmpty()
                                                                  join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                                                  from especiIn in especiX.DefaultIfEmpty()
                                                                      //Mora Puga
                                                                  where ex.medico == "14034"
                                                                  //Pediatria
                                                                  || especiIn.CLAVE == "03"
                                                                  //Alergia Pediatria
                                                                  || especiIn.CLAVE == "54"
                                                                  select new
                                                                  {
                                                                      //clave = especiIn.CLAVE,
                                                                      medico = ex.medico,
                                                                      //fecha = q.fecha,
                                                                      especialidad = especiIn.DESCRIPCION,
                                                                      nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                                                      consultadistancia = ex.consultadistancia
                                                                  })
                                                              .ToList().Select(x => new
                                                              {
                                                                  //clave = x.clave,
                                                                  medico = x.medico,
                                                                  nombre = x.nombre,
                                                                  especialidad = x.especialidad,
                                                                  //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                                                  consultadistancia = x.consultadistancia
                                                              })
                                                              .ToList().Select(z => new
                                                              {
                                                                  //clave = z.clave,
                                                                  medico = z.medico,
                                                                  nombre = z.nombre,
                                                                  especialidad = z.especialidad,
                                                                  //fecha = DateTime.Parse(z.fecha),
                                                                  consultadistancia = z.consultadistancia
                                                              })
                                                              .GroupBy(p => new
                                                              {
                                                                  p.nombre,
                                                                  //p.fecha,
                                                                  //p.clave,
                                                                  p.medico,
                                                                  p.especialidad
                                                              })
                                                             .Select(g => new
                                                             {
                                                                 //clave = g.Key.clave,
                                                                 medico = g.Key.medico,
                                                                 nombre = g.Key.nombre,
                                                                 especialidad = g.Key.especialidad,
                                                                 fecha = textoFecha,
                                                                 count = g.Count(),
                                                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                                                 presencial = g.Count(p => p.consultadistancia != "1"),
                                                             })
                                                             //.OrderBy(g => g.fecha)
                                                             .OrderBy(g => g.medico)
                                                             .ToList();

                                                var resultsDet1 = new List<IndEspListDet>();

                                                foreach (var item in expediedet)
                                                {
                                                    var resultDet = new IndEspListDet
                                                    {
                                                        medico = item.medico,
                                                        nombre = item.nombre,
                                                        especialidad = item.especialidad,
                                                        fecha = textoFecha,
                                                        count = item.count,
                                                        telefonica = item.telefonica,
                                                        presencial = item.presencial,
                                                    };

                                                    resultsDet1.Add(resultDet);
                                                }


                                                string json2 = JsonConvert.SerializeObject(resultsDet1);
                                                string path2 = Server.MapPath("~/json/");
                                                System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);


                                                return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                                            }
                                            else
                                            {
                                                if (User.IsInRole("Productividad10"))
                                                {
                                                    var expediente = (from q in db.expediente
                                                                      where q.medico == "52023" || q.medico == "52024" || q.medico == "52025" || q.medico == "52028" 
                                                                      || q.medico == "52029" || q.medico == "08053" || q.medico == "38119" || q.medico == "52033"
                                                                      where q.hora_termino != null
                                                                      where q.fecha >= fecha_minDate
                                                                      where q.fecha <= fecha_maxDate
                                                                      join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                                                      from especiIn in especiX.DefaultIfEmpty()
                                                                      select new
                                                                      {
                                                                          clave = especiIn.CLAVE,
                                                                          //fecha = q.fecha,
                                                                          especialidad = especiIn.DESCRIPCION,
                                                                          consultadistancia = q.consultadistancia
                                                                      })
                                                              .ToList().Select(x => new
                                                              {
                                                                  clave = x.clave,
                                                                  especialidad = x.especialidad,
                                                                  //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                                                  consultadistancia = x.consultadistancia
                                                              })
                                                              .ToList().Select(z => new
                                                              {
                                                                  clave = z.clave,
                                                                  especialidad = z.especialidad,
                                                                  //fecha = DateTime.Parse(z.fecha),
                                                                  consultadistancia = z.consultadistancia
                                                              })
                                                              .GroupBy(p => new
                                                              {
                                                                  p.especialidad,
                                                                  //p.fecha,
                                                                  p.clave,
                                                              })
                                                             .Select(g => new
                                                             {
                                                                 clave = g.Key.clave,
                                                                 especialidad = g.Key.especialidad,
                                                                 fecha = textoFecha,
                                                                 count = g.Count(),
                                                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                                                 presencial = g.Count(p => p.consultadistancia != "1"),
                                                             })
                                                             //.OrderBy(g => g.fecha)
                                                             .OrderBy(g => g.especialidad)
                                                             .ToList();

                                                    //System.Diagnostics.Debug.WriteLine(expedi_den);

                                                    var results1 = new List<IndEspList>();

                                                    foreach (var item in expediente)
                                                    {
                                                        var result = new IndEspList
                                                        {
                                                            clave = item.especialidad,
                                                            especialidad = item.especialidad,
                                                            fecha = textoFecha,
                                                            count = item.count,
                                                            telefonica = item.telefonica,
                                                            presencial = item.presencial,
                                                        };

                                                        results1.Add(result);
                                                    }


                                                    string json = JsonConvert.SerializeObject(results1);
                                                    string path = Server.MapPath("~/json/");
                                                    System.IO.File.WriteAllText(path + "indicadores/especialidades/productividad_busqueda.json", json);



                                                    //DETALLES
                                                    var expediedet = (from ex in db.expediente
                                                                      where ex.medico == "52023" || ex.medico == "52024" || ex.medico == "52025" || ex.medico == "52028" 
                                                                      || ex.medico == "52029" || ex.medico == "08053" || ex.medico == "38119" || ex.medico == "52033"
                                                                      where ex.hora_termino != null
                                                                      where ex.fecha >= fecha_minDate
                                                                      where ex.fecha <= fecha_maxDate
                                                                      //where ex.medico.Substring(0, 2) == espec.CLAVE
                                                                      join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                                                      from medicosIn in mediX.DefaultIfEmpty()
                                                                      join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                                                      from especiIn in especiX.DefaultIfEmpty()
                                                                      select new
                                                                      {
                                                                          //clave = especiIn.CLAVE,
                                                                          medico = ex.medico,
                                                                          //fecha = q.fecha,
                                                                          especialidad = especiIn.DESCRIPCION,
                                                                          nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                                                          consultadistancia = ex.consultadistancia
                                                                      })
                                                                  .ToList().Select(x => new
                                                                  {
                                                                      //clave = x.clave,
                                                                      medico = x.medico,
                                                                      nombre = x.nombre,
                                                                      especialidad = x.especialidad,
                                                                      //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                                                      consultadistancia = x.consultadistancia
                                                                  })
                                                                  .ToList().Select(z => new
                                                                  {
                                                                      //clave = z.clave,
                                                                      medico = z.medico,
                                                                      nombre = z.nombre,
                                                                      especialidad = z.especialidad,
                                                                      //fecha = DateTime.Parse(z.fecha),
                                                                      consultadistancia = z.consultadistancia
                                                                  })
                                                                  .GroupBy(p => new
                                                                  {
                                                                      p.nombre,
                                                                      //p.fecha,
                                                                      //p.clave,
                                                                      p.medico,
                                                                      p.especialidad
                                                                  })
                                                                 .Select(g => new
                                                                 {
                                                                     //clave = g.Key.clave,
                                                                     medico = g.Key.medico,
                                                                     nombre = g.Key.nombre,
                                                                     especialidad = g.Key.especialidad,
                                                                     fecha = textoFecha,
                                                                     count = g.Count(),
                                                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                                                 })
                                                                 //.OrderBy(g => g.fecha)
                                                                 .OrderBy(g => g.medico)
                                                                 .ToList();

                                                    var resultsDet1 = new List<IndEspListDet>();

                                                    foreach (var item in expediedet)
                                                    {

                                                        var especialidad = item.especialidad;

                                                        if (item.medico == "52023")
                                                        {
                                                            especialidad = "MEDICINA GENERAL";
                                                        }

                                                        if (item.medico == "52024")
                                                        {
                                                            especialidad = "MEDICINA INTERNA";
                                                        }

                                                        if (item.medico == "52025")
                                                        {
                                                            especialidad = "MEDICINA GENERAL";
                                                        }

                                                        if (item.medico == "52028")
                                                        {
                                                            especialidad = "PEDIATRIA";
                                                        }

                                                        if (item.medico == "52029")
                                                        {
                                                            especialidad = "GINECOLOGIA";
                                                        }

                                                        if (item.medico == "08053")
                                                        {
                                                            especialidad = "GINECOLOGIA";
                                                        }

                                                        if (item.medico == "38119")
                                                        {
                                                            especialidad = "MEDICINA INTERNA";
                                                        }

                                                        if (item.medico == "52033")
                                                        {
                                                            especialidad = "MEDICINA GENERAL";
                                                        }

                                                     

                                                        var resultDet = new IndEspListDet
                                                        {
                                                            medico = item.medico,
                                                            nombre = item.nombre,
                                                            especialidad = especialidad,
                                                            fecha = textoFecha,
                                                            count = item.count,
                                                            telefonica = item.telefonica,
                                                            presencial = item.presencial,
                                                        };

                                                        resultsDet1.Add(resultDet);
                                                    }


                                                    string json2 = JsonConvert.SerializeObject(resultsDet1);
                                                    string path2 = Server.MapPath("~/json/");
                                                    System.IO.File.WriteAllText(path2 + "indicadores/especialidades/productividad_detalles.json", json2);


                                                    return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                                                }
                                                else
                                                {
                                                    var expediente = "";
                                                    return new JsonResult { Data = expediente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                                                }

                                            }
                                        }


                                    }
                                }
                            }
                        }
                    }
                }


            }

        }


        public ActionResult IndicadoresLinares()
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            //EXPEDIENTES DE Linares
            var linares = (from q in db2.expediente
                               //where q.medico.StartsWith("51")
                               //where q.ip_realiza == "148.234.143.203" || q.ip_realiza == "148.234.140.9"
                           where q.hora_termino != null
                           //where q.folio_rc == null
                           //where q.Medico == "30001"
                           where q.medico == "52023" || q.medico == "52024" || q.medico == "52025" || q.medico == "52028" || q.medico == "52029" || q.medico == "08053" || q.medico == "38119" || q.medico == "52033"
                           select new
                           {
                               fechafecha = q.hora_termino,
                               medico = q.medico
                               //expexp = q.Expediente
                           })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpLi = JArray.FromObject(linares).Select(x => x.Values().ToList()).ToList();


            string jsonExpLi = JsonConvert.SerializeObject(valuesListExpLi, Formatting.Indented);

            string pathExpLi = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpLi + "expediente_linares.json", jsonExpLi);

            return View();
        }


        public ActionResult IndicadoresHomeOffice()
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            //EXPEDIENTES DE HomeOffice
            var homeoffice = (from q in db2.expediente
                              where q.hora_termino != null
                              where q.medico == "02161"
                              select new
                              {
                                  fechafecha = q.hora_termino,
                                  medico = q.medico
                                  //expexp = q.Expediente
                              })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpHO = JArray.FromObject(homeoffice).Select(x => x.Values().ToList()).ToList();


            string jsonExpHO = JsonConvert.SerializeObject(valuesListExpHO, Formatting.Indented);

            string pathExpHO = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpHO + "expediente_homeoffice.json", jsonExpHO);

            return View();
        }


        public ActionResult IndicadoresUrgenciasA()
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            //EXPEDIENTES DE Urgencias A
            var urgenciasa = (from q in db2.expediente
                              where q.hora_termino != null
                              //where q.medico.StartsWith("51")
                              where q.ip_realiza == "148.234.140.200" || q.ip_realiza == "148.234.140.95" || q.ip_realiza == "148.234.143.153" || q.ip_realiza == "148.234.143.166" || q.ip_realiza == "148.234.143.192" || q.ip_realiza == "148.234.143.203" || q.ip_realiza == "187.160.61.75" || q.ip_realiza == "189.175.114.33" || q.ip_realiza == "189.175.149.234" || q.ip_realiza == "189.175.152.13"

                              select new
                              {
                                  fechafecha = q.hora_termino,
                                  medico = q.medico
                                  //expexp = q.Expediente
                              })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpUA = JArray.FromObject(urgenciasa).Select(x => x.Values().ToList()).ToList();


            string jsonExpUA = JsonConvert.SerializeObject(valuesListExpUA, Formatting.Indented);

            string pathExpUA = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpUA + "expediente_urgenciasa.json", jsonExpUA);

            return View();
        }


        public ActionResult IndicadoresRecetasHoy()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha_hoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta_hoy = DateTime.Parse(fecha_hoy);

            //RECETAS NORMALES HOY
            var receta_normal_hoy = (from q in db.RECETA_EXP_2
                                     where q.Manual == null
                                     where q.folio_rc == null
                                     where q.Hora_Rcta >= fecha_correcta_hoy
                                     where q.Hora_Rcta != null
                                     //where q.Medico == "30001"
                                     select new
                                     {
                                         fechafecha = q.Hora_Rcta,
                                         //expexp = q.Expediente
                                     })
                                .ToList().Select(x => new
                                {
                                    datedate = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fechafecha),
                                })
                                 .ToList().Select(z => new
                                 {
                                     fecha_correcta = DateTime.Parse(z.datedate)
                                 })
                                 .ToList().Select(y => new
                                 {
                                     fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                                     //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds
                                 })
                                .GroupBy(p => p.fecha_correcta_correcta)
                                .Select(g => new
                                {
                                    Date = g.Key,
                                    Count = g.Select(p => p.fecha_correcta_correcta).Count()
                                })
                                .OrderBy(g => g.Date)
                                .ToList();

            var valuesListHoy = JArray.FromObject(receta_normal_hoy).Select(x => x.Values().ToList()).ToList();


            string jsonhoy = JsonConvert.SerializeObject(valuesListHoy, Formatting.Indented);

            string pathhoy = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathhoy + "receta_normal_hoy.json", jsonhoy);


            //RECETAS TARJETON HOY
            var tarjeton_hoy = (from q in db.RECETA_EXP_2
                                where q.Manual == null
                                where q.folio_rc != null
                                where q.Hora_Rcta >= fecha_correcta_hoy
                                where q.Hora_Rcta != null
                                //where q.Medico == "30001"
                                select new
                                {
                                    fechafecha = q.Hora_Rcta,
                                    //expexp = q.Expediente
                                })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListTjHoy = JArray.FromObject(tarjeton_hoy).Select(x => x.Values().ToList()).ToList();


            string jsonTjHoy = JsonConvert.SerializeObject(valuesListTjHoy, Formatting.Indented);

            string pathTjHoy = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathTjHoy + "tarjeton_hoy.json", jsonTjHoy);


            //RECETAS MANUALES HOY
            var manuales_hoy = (from q in db.RECETA_EXP_2
                                where q.Manual != null
                                where q.Fecha >= fecha_correcta_hoy
                                //where q.Medico == "30001"
                                select new
                                {
                                    fechafecha = q.Fecha,
                                    //expexp = q.Expediente
                                })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListMaHoy = JArray.FromObject(manuales_hoy).Select(x => x.Values().ToList()).ToList();


            string jsonMaHoy = JsonConvert.SerializeObject(valuesListMaHoy, Formatting.Indented);

            string pathMaHoy = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathMaHoy + "manuales_hoy.json", jsonMaHoy);

            return View();
        }


        public ActionResult IndicadoresHistorialRecetas()
        {
            Models.SMDEVEntities18 db = new Models.SMDEVEntities18();

            //RECETAS NORMALES
            var receta_normal = (from q in db.RECETA_EXP_2
                                 where q.Manual == null
                                 where q.folio_rc == null
                                 where q.Hora_Rcta != null
                                 //where q.Medico == "30001"
                                 select new
                                 {
                                     fechafecha = q.Hora_Rcta,
                                     //expexp = q.Expediente
                                 })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesList = JArray.FromObject(receta_normal).Select(x => x.Values().ToList()).ToList();


            string json = JsonConvert.SerializeObject(valuesList, Formatting.Indented);

            string path = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path + "receta_normal.json", json);


            //RECETAS TARJETON
            var tarjeton = (from q in db.RECETA_EXP_2
                            where q.Manual == null
                            where q.folio_rc != null
                            where q.Hora_Rcta != null
                            //where q.Medico == "30001"
                            select new
                            {
                                fechafecha = q.Hora_Rcta,
                                //expexp = q.Expediente
                            })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListTj = JArray.FromObject(tarjeton).Select(x => x.Values().ToList()).ToList();


            string jsonTj = JsonConvert.SerializeObject(valuesListTj, Formatting.Indented);

            string pathTj = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathTj + "tarjeton.json", jsonTj);


            //RECETAS MANUALES
            var manuales = (from q in db.RECETA_EXP_2
                            where q.Manual != null
                            //where q.Medico == "30001"
                            select new
                            {
                                fechafecha = q.Fecha,
                                //expexp = q.Expediente
                            })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListMa = JArray.FromObject(manuales).Select(x => x.Values().ToList()).ToList();


            string jsonMa = JsonConvert.SerializeObject(valuesListMa, Formatting.Indented);

            string pathMa = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathMa + "manuales.json", jsonMa);

            return View();


        }


        public class IndicadoresConsultasList
        {
            public double Date { get; set; }
            public int Count { get; set; }
        }

        public ActionResult IndicadoresConsultasHoy()
        {
            Models.SMDEVEntities18 db = new Models.SMDEVEntities18();
            var fecha_hoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta_hoy = DateTime.Parse(fecha_hoy);

            #region "Medicina General"

            //EXPEDIENTES MEDICO GENERAL HOY
            var expediente_mf_hoy = (from q in db.expediente
                                     where q.medico.StartsWith("02")
                                     where q.hora_termino != null
                                     where q.hora_termino >= fecha_correcta_hoy
                                     //where q.folio_rc == null
                                     where q.medico != "02101" && q.medico != "02307" && q.medico != "02212" && q.medico != "02085"
                                     select new
                                     {
                                         fechafecha = q.hora_termino,
                                         medico = q.medico
                                         //expexp = q.Expediente
                                     })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpMFHoy = JArray.FromObject(expediente_mf_hoy).Select(x => x.Values().ToList()).ToList();


            string jsonExpMFHoy = JsonConvert.SerializeObject(valuesListExpMFHoy, Formatting.Indented);

            string patheXPmfHoy = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(patheXPmfHoy + "expediente_medicina_familiar_hoy.json", jsonExpMFHoy);

            #endregion


            #region "Especialidades"

            //EXPEDIENTES ESPECIALISTA HOY
            var expediente_es_hoy = (from q in db.expediente
                                     where q.medico.StartsWith("02") == false || q.medico == "02307" || q.medico == "02212" || q.medico == "02085"
                                     where q.hora_termino != null
                                     where q.hora_termino >= fecha_correcta_hoy
                                     select new
                                     {
                                         fechafecha = q.hora_termino,
                                         medico = q.medico
                                     })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();


            var valuesListExpEsHoy = JArray.FromObject(expediente_es_hoy).Select(x => x.Values().ToList()).ToList();

            string jsonExpEsHoy = JsonConvert.SerializeObject(valuesListExpEsHoy, Formatting.Indented);

            string patheExpEsHoy = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(patheExpEsHoy + "expediente_especialistas_hoy.json", jsonExpEsHoy);

            #endregion


            return View();
        }



        public ActionResult IndicadoresHistorialConsultas()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            #region "Medico General"

            //EXPEDIENTES MEDICO FAMILIAR

            var expediente_mf = (from q in db.expediente
                                 where q.medico.StartsWith("02")
                                 where q.hora_termino != null
                                 where q.medico != "02101" /* && q.medico != "02307" && q.medico != "02212" && q.medico != "02085" */
                                 select new
                                 {
                                     fechafecha = q.hora_termino,
                                     medico = q.medico
                                     //expexp = q.Expediente
                                 })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpMF = JArray.FromObject(expediente_mf).Select(x => x.Values().ToList()).ToList();


            string jsonExpMF = JsonConvert.SerializeObject(valuesListExpMF, Formatting.Indented);

            string patheXPmf = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(patheXPmf + "expediente_medicina_familiar.json", jsonExpMF);

            #endregion


            #region "Especialidad"

            //EXPEDIENTES ESPECIALISTA
            var expediente_es = (from q in db.expediente
                                 where q.medico.StartsWith("02") == false /* || q.medico == "02307" || q.medico == "02212" || q.medico == "02085" */
                                 where q.hora_termino != null
                                 //where q.folio_rc == null
                                 //where q.Medico == "30001"
                                 select new
                                 {
                                     fechafecha = q.hora_termino,
                                     medico = q.medico
                                     //expexp = q.Expediente
                                 })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpEs = JArray.FromObject(expediente_es).Select(x => x.Values().ToList()).ToList();


            string jsonExpEs = JsonConvert.SerializeObject(valuesListExpEs, Formatting.Indented);

            string pathExpEs = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpEs + "expediente_especialistas.json", jsonExpEs);

            #endregion

            return View();
        }


        public ActionResult IndicadoresGonzalitos()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            //EXPEDIENTES GONZALITOS Medicina General
            var expediente = (from q in db.expediente
                              //Medicina General
                              where q.medico.StartsWith("02")
                              //Gonzalitos
                              //where q.ip_realiza.StartsWith("148.234.143")
                              //where q.fecha >= fecha_minDate
                              //where q.fecha <= fecha_maxDate
                              //Unidad ER
                              where q.ip_realiza != "148.234.143.169" && q.ip_realiza != "148.234.143.217" && q.ip_realiza != "148.234.143.151" && q.ip_realiza != "148.234.143.176" && q.ip_realiza != "148.234.143.185"

                              //Subrogados
                              && q.medico != "38115" && q.medico != "38112" && q.medico != "38113" && q.medico != "14037" && q.medico != "38111"
                              && q.medico != "38117" /*&& ex.medico != "45001"*/ && q.medico != "03132" && q.medico != "38110" && q.medico != "38114"
                              && q.medico != "19017" && q.medico != "38116" && q.medico != "38118" && q.medico != "38120" && q.medico != "15512"
                              && q.medico != "34019" && q.medico != "51035" && q.medico != "38121" && q.medico != "38122" && q.medico != "38123"
                              && q.medico != "38124" && q.medico != "38125"
                              where q.hora_termino != null
                              where q.medico != " "
                              //where ex.medico.Substring(0, 2) == espec.CLAVE
                              join medicos in db.MEDICOS on q.medico equals medicos.Numero into mediX
                              from medicosIn in mediX.DefaultIfEmpty()
                              join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                              from especiIn in especiX.DefaultIfEmpty()
                              where especiIn.CLAVE != "44"
                              where especiIn.CLAVE != "45"
                              where especiIn.CLAVE != "46"
                              where especiIn.CLAVE != "49"
                              where especiIn.CLAVE != "04"
                              where especiIn.CLAVE != "51"

                              select new
                              {
                                fechafecha = q.hora_termino,
                                medico = q.medico
                              })
                              .ToList().Select(x => new
                              {
                                  datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                              })
                              .ToList().Select(z => new
                              {
                                  fecha_correcta = DateTime.Parse(z.datedate)
                              })
                              .ToList().Select(y => new
                              {
                                  fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                              })
                             .GroupBy(p => p.fecha_correcta_correcta)
                             .Select(g => new
                             {
                                 Date = g.Key,
                                 Count = g.Select(p => p.fecha_correcta_correcta).Count()
                             })
                             .OrderBy(g => g.Date)
                             .ToList();

            var resultsGonz = new List<IndGrafica>();

            int fusion = 0;

            foreach (var item in expediente)
            {
                var result1 = new IndGrafica
                {
                    Date = item.Date,
                    Count = item.Count,
                };

                 resultsGonz.Add(result1);
            }


            //EXPEDIENTES GONZALITOS Especialistas
            var expediente2 = (from q in db.expediente
                              //Especialistas
                              where !q.medico.StartsWith("02")
                               //Gonzalitos
                               //where q.ip_realiza.StartsWith("148.234.143")
                               //where q.fecha >= fecha_minDate
                               //where q.fecha <= fecha_maxDate
                               //Unidad ER
                               where q.ip_realiza != "148.234.143.169" && q.ip_realiza != "148.234.143.217" && q.ip_realiza != "148.234.143.151" && q.ip_realiza != "148.234.143.176" && q.ip_realiza != "148.234.143.185"

                               //Subrogados
                               && q.medico != "38115" && q.medico != "38112" && q.medico != "38113" && q.medico != "14037" && q.medico != "38111"
                               && q.medico != "38117" /*&& ex.medico != "45001"*/ && q.medico != "03132" && q.medico != "38110" && q.medico != "38114"
                               && q.medico != "19017" && q.medico != "38116" && q.medico != "38118" && q.medico != "38120" && q.medico != "15512"
                               && q.medico != "34019" && q.medico != "51035" && q.medico != "38121" && q.medico != "38122" && q.medico != "38123"
                               && q.medico != "38124" && q.medico != "38125"
                               where q.hora_termino != null
                               where q.medico != " "
                               //where ex.medico.Substring(0, 2) == espec.CLAVE
                               join medicos in db.MEDICOS on q.medico equals medicos.Numero into mediX
                               from medicosIn in mediX.DefaultIfEmpty()
                               join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                               from especiIn in especiX.DefaultIfEmpty()
                               where especiIn.CLAVE != "44"
                               where especiIn.CLAVE != "45"
                               where especiIn.CLAVE != "46"
                               where especiIn.CLAVE != "49"
                               where especiIn.CLAVE != "04"
                               where especiIn.CLAVE != "51"
                               select new
                              {
                                  fechafecha = q.hora_termino,
                                  medico = q.medico
                              })
                              .ToList().Select(x => new
                              {
                                  datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                              })
                              .ToList().Select(z => new
                              {
                                  fecha_correcta = DateTime.Parse(z.datedate)
                              })
                              .ToList().Select(y => new
                              {
                                  fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                              })
                             .GroupBy(p => p.fecha_correcta_correcta)
                             .Select(g => new
                             {
                                 Date = g.Key,
                                 Count = g.Select(p => p.fecha_correcta_correcta).Count()
                             })
                             .OrderBy(g => g.Date)
                             .ToList();

            //System.Diagnostics.Debug.WriteLine(expediente2);


            var resultsGonz2 = new List<IndGrafica>();

            foreach (var item in expediente2)
            {

                System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dtDateTime = dtDateTime.AddMilliseconds(item.Date).ToLocalTime();
                var datetostring = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", dtDateTime);

                //System.Diagnostics.Debug.WriteLine(datetostring);

                string queryDet = "SELECT Dertbl.fecha as fecha, Count(*) as count  FROM (SELECT fecha as fecha, Count(*) as count, expediente as expediente FROM expediente_dental where fecha = '"+ datetostring + "' GROUP BY expediente, fecha) AS Dertbl GROUP BY Dertbl.fecha ORDER BY Dertbl.fecha";
                var resultDenDet = db.Database.SqlQuery<Dental>(queryDet);
                var expedi_den_det = resultDenDet.Count();

                //System.Diagnostics.Debug.WriteLine(expedi_den_det);

                var result3 = new IndGrafica
                {
                    Date = item.Date,
                    Count = item.Count + expedi_den_det,
                };

                resultsGonz2.Add(result3);
            }

            

            //var datetostring = string.Format("{0:yyyy-MM-dd}", item.fecha);
            //var stringtodate = DateTime.Parse(datetostring);
            //var miliseconds = stringtodate.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;


            //Medicina General
            var valuesListExpGonz = JArray.FromObject(resultsGonz).Select(x => x.Values().ToList()).ToList();
            string jsonExpGonz = JsonConvert.SerializeObject(valuesListExpGonz, Formatting.Indented);
            string pathExpGonz = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpGonz + "expediente_gonzalitos_general.json", jsonExpGonz);

            //Especialistas
            var valuesListExpGonz2 = JArray.FromObject(resultsGonz2).Select(x => x.Values().ToList()).ToList();
            string jsonExpGonz2 = JsonConvert.SerializeObject(valuesListExpGonz2, Formatting.Indented);
            string pathExpGonz2 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpGonz + "expediente_gonzalitos_especialistas.json", jsonExpGonz2);


            return View();

        }

        public ActionResult IndicadoresServiciosMedicos()
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            //EXPEDIENTES DE SM TELEFONICA
            var expediente_sm_telefonica = (from q in db2.expediente
                                            where !q.ip_realiza.StartsWith("148.234.64")
                                            where q.ip_realiza != "148.234.143.203"
                                            where q.ip_realiza != "148.234.140.9"
                                            where q.consultadistancia == "1"
                                            where q.hora_termino != null
                                            select new
                                            {
                                                fechafecha = q.hora_termino,
                                                medico = q.medico
                                                //expexp = q.Expediente
                                            })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpSMTEL = JArray.FromObject(expediente_sm_telefonica).Select(x => x.Values().ToList()).ToList();


            string jsonExpSMTEL = JsonConvert.SerializeObject(valuesListExpSMTEL, Formatting.Indented);

            string pathExpSMTEL = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpSMTEL + "expediente_sm_telefonica.json", jsonExpSMTEL);



            //EXPEDIENTES DE SM PRESENCIAL
            var expediente_sm_presencial = (from q in db2.expediente
                                            where !q.ip_realiza.StartsWith("148.234.64")
                                            where q.ip_realiza != "148.234.143.203"
                                            where q.ip_realiza != "148.234.140.9"
                                            where q.consultadistancia != "1"
                                            where q.hora_termino != null
                                            select new
                                            {
                                                fechafecha = q.hora_termino,
                                                medico = q.medico
                                                //expexp = q.Expediente
                                            })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpSMPRE = JArray.FromObject(expediente_sm_presencial).Select(x => x.Values().ToList()).ToList();


            string jsonExpSMPRE = JsonConvert.SerializeObject(valuesListExpSMPRE, Formatting.Indented);

            string pathExpSMPRE = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpSMPRE + "expediente_sm_presencial.json", jsonExpSMPRE);

            return View();

        }

        public ActionResult IndicadoresMederos()
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            //EXPEDIENTES DE MEDEROS TELEFÓNICAS
            var expediente_mederos_telefonica = (from q in db2.expediente
                                                 where q.consultadistancia == "1"
                                                 where q.ip_realiza.StartsWith("148.234.64")
                                                 where q.hora_termino != null
                                                 select new
                                                 {
                                                     fechafecha = q.hora_termino,
                                                     medico = q.medico
                                                     //expexp = q.Expediente
                                                 })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpMETEL = JArray.FromObject(expediente_mederos_telefonica).Select(x => x.Values().ToList()).ToList();


            string jsonExpMETEL = JsonConvert.SerializeObject(valuesListExpMETEL, Formatting.Indented);

            string pathExpMETEL = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpMETEL + "expediente_mederos_telefonica.json", jsonExpMETEL);


            //EXPEDIENTES DE MEDEROS PRESENCIAL
            var expediente_mederos_presencial = (from q in db2.expediente
                                                 where q.ip_realiza.StartsWith("148.234.64")
                                                 where q.consultadistancia != "1"
                                                 where q.hora_termino != null
                                                 select new
                                                 {
                                                     fechafecha = q.hora_termino,
                                                     medico = q.medico
                                                     //expexp = q.Expediente
                                                 })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpMEPRE = JArray.FromObject(expediente_mederos_presencial).Select(x => x.Values().ToList()).ToList();


            string jsonExpMEPRE = JsonConvert.SerializeObject(valuesListExpMEPRE, Formatting.Indented);

            string pathExpMEPRE = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpMEPRE + "expediente_mederos_presencial.json", jsonExpMEPRE);


            return View();

        }

        public ActionResult IndicadoresMederos2()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            //EXPEDIENTES MEDEROS MEDICINA GENERAL
            var expediente = (from q in db.expediente
                              //Medicina General
                              where q.medico.StartsWith("02")
                              //Mederos
                              where q.ip_realiza.StartsWith("148.234.64")
                              //Medico de prueba
                              where q.medico != "02101"

                              where q.hora_termino != null
                              select new
                              {
                                  fechafecha = q.hora_termino,
                                  medico = q.medico
                              })
                              .ToList().Select(x => new
                              {
                                  datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                              })
                              .ToList().Select(z => new
                              {
                                  fecha_correcta = DateTime.Parse(z.datedate)
                              })
                              .ToList().Select(y => new
                              {
                                  fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                              })
                             .GroupBy(p => p.fecha_correcta_correcta)
                             .Select(g => new
                             {
                                 Date = g.Key,
                                 Count = g.Select(p => p.fecha_correcta_correcta).Count()
                             })
                             .OrderBy(g => g.Date)
                             .ToList();

            var resultsGonz = new List<IndGrafica>();

            int fusion = 0;

            foreach (var item in expediente)
            {
                var result1 = new IndGrafica
                {
                    Date = item.Date,
                    Count = item.Count,
                };

                resultsGonz.Add(result1);
            }


            //EXPEDIENTES MEDEROS ESPECIALISTAS
            var expediente2 = (from q in db.expediente
                               //Especialistas
                               where !q.medico.StartsWith("02")
                               //Mederos
                               where q.ip_realiza.StartsWith("148.234.64")
                               //Medico de prueba
                               where q.medico != "02101"

                               where q.hora_termino != null
                               select new
                               {
                                   fechafecha = q.hora_termino,
                                   medico = q.medico
                               })
                               .ToList().Select(x => new
                               {
                                   datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                               })
                               .ToList().Select(z => new
                               {
                                   fecha_correcta = DateTime.Parse(z.datedate)
                               })
                               .ToList().Select(y => new
                               {
                                  fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                               })
                               .GroupBy(p => p.fecha_correcta_correcta)
                               .Select(g => new
                               {
                                   Date = g.Key,
                                   Count = g.Select(p => p.fecha_correcta_correcta).Count()
                               })
                               .OrderBy(g => g.Date)
                               .ToList();




            var resultsGonz2 = new List<IndGrafica>();

            int fusion2 = 0;

            foreach (var item in expediente2)
            {
                var result3 = new IndGrafica
                {
                    Date = item.Date,
                    Count = item.Count,
                };

                resultsGonz2.Add(result3);
            }


            var valuesListExpGonz = JArray.FromObject(resultsGonz).Select(x => x.Values().ToList()).ToList();
            string jsonExpGonz = JsonConvert.SerializeObject(valuesListExpGonz, Formatting.Indented);
            string pathExpGonz = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpGonz + "expediente_mederos_general.json", jsonExpGonz);

            var valuesListExpGonz2 = JArray.FromObject(resultsGonz2).Select(x => x.Values().ToList()).ToList();
            string jsonExpGonz2 = JsonConvert.SerializeObject(valuesListExpGonz2, Formatting.Indented);
            string pathExpGonz2 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpGonz + "expediente_mederos_especialistas.json", jsonExpGonz2);


            return View();

        }

        public ActionResult IndicadoresConsultaInternados()
        {
            //Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            DalExpedientes_hu exp_hu = new DalExpedientes_hu();


            List<DateCount> listDt = new List<DateCount>();


            var Egr = exp_hu.ListaEgr().ToList().Select(x => new
            {
                datedate = string.Format("{0:yyyy-MM-dd}", x.FechaHora),
            })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();
            var valuesListEgr = JArray.FromObject(Egr).Select(x => x.Values().ToList()).ToList();

            string jsonEgr = JsonConvert.SerializeObject(valuesListEgr, Formatting.Indented);

            string path = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path + "expedientes_egresos.json", jsonEgr);



            var Ing = exp_hu.ListaIng().ToList().Select(x => new
            {
                datedate = string.Format("{0:yyyy-MM-dd}", x.FechaHora),
            })
                        .ToList().Select(z => new
                        {
                            fecha_correcta = DateTime.Parse(z.datedate)
                        })
                        .ToList().Select(y => new
                        {
                            fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                            //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                        })
                       .GroupBy(p => p.fecha_correcta_correcta)
                       .Select(g => new
                       {
                           Date = g.Key,
                           Count = g.Select(p => p.fecha_correcta_correcta).Count()
                       })
                       .OrderBy(g => g.Date)
                       .ToList();
            var valuesList = JArray.FromObject(Ing).Select(x => x.Values().ToList()).ToList();

            string json = JsonConvert.SerializeObject(valuesList, Formatting.Indented);


            System.IO.File.WriteAllText(path + "expedientes_ingresos.json", json);



            var Internados = exp_hu.ListaInternados().ToList().Select(x => new
            {
                datedate = string.Format("{0:yyyy-MM-dd}", x.FechaHora),
            })
                       .ToList().Select(z => new
                       {
                           fecha_correcta = DateTime.Parse(z.datedate)
                       })
                       .ToList().Select(y => new
                       {
                           fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                           //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                       })
                      .GroupBy(p => p.fecha_correcta_correcta)
                      .Select(g => new
                      {
                          Date = g.Key,
                          Count = g.Select(p => p.fecha_correcta_correcta).Count()
                      })
                      .OrderBy(g => g.Date)
                      .ToList();
            var valuesListInternados = JArray.FromObject(Internados).Select(x => x.Values().ToList()).ToList();

            string jsonInternados = JsonConvert.SerializeObject(valuesListInternados, Formatting.Indented);


            System.IO.File.WriteAllText(path + "expedientes_internados.json", jsonInternados);

            return View();
        }


        public ActionResult IndicadoresUrgenciasSM()
        {
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            //EXPEDIENTES DE Urgencias SM
            var urgenciassm = (from q in db2.expediente
                               where q.hora_termino != null
                               //Estos son de servicios medicos
                               where q.medico == "02030" || q.medico == "02295" || q.medico == "02298" || q.medico == "02291" /*|| q.medico == "02255"*/ || q.medico == "02302" || q.medico == "02286"
                               //Estos son los de HU
                               //where q.medico == "51018" || q.medico == "51028" || q.medico == "51022" || q.medico == "51029" || q.medico == "51020" || q.medico == "51031" || q.medico == "51024"
                               select new
                               {
                                   fechafecha = q.hora_termino,
                                   medico = q.medico
                                   //expexp = q.Expediente
                               })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpUSM = JArray.FromObject(urgenciassm).Select(x => x.Values().ToList()).ToList();


            string jsonExpUSM = JsonConvert.SerializeObject(valuesListExpUSM, Formatting.Indented);

            string pathExpUSM = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpUSM + "expediente_urgenciassm.json", jsonExpUSM);

            return View();
        }


        public JsonResult DescargarExcel(string minDate, string maxDate)
        {

            //System.Diagnostics.Debug.WriteLine(minDate);

            var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

            //System.Diagnostics.Debug.WriteLine(fechaHoy);


            var fecha_minDate = DateTime.Parse(minDate);
            var fecha_maxDate = DateTime.Parse(maxDate);

            //System.Diagnostics.Debug.WriteLine(fecha_minDate);

            var textoFecha = "";

            if (minDate == maxDate)
            {
                textoFecha = minDate;
            }
            else
            {
                textoFecha = "De: " + minDate + " Hasta: " + maxDate;
            }

            //System.Diagnostics.Debug.WriteLine(fecha_minDate);


            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();


            #region  "ServiciosMedicos"

            var expediedet = (from ex in db.expediente
                              where !ex.ip_realiza.StartsWith("148.234.64")
                              where ex.ip_realiza != "148.234.143.203"
                              where ex.ip_realiza != "148.234.140.9"
                              where ex.fecha >= fecha_minDate
                              where ex.fecha <= fecha_maxDate
                              where ex.hora_termino != null
                              //where ex.medico.Substring(0, 2) == espec.CLAVE
                              join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                              from medicosIn in mediX.DefaultIfEmpty()
                              join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                              from especiIn in especiX.DefaultIfEmpty()
                              where especiIn.CLAVE != "44"
                              where especiIn.CLAVE != "45"
                              where especiIn.CLAVE != "46"
                              where especiIn.CLAVE != "49"
                              where especiIn.CLAVE != "04"
                              select new
                              {
                                  //clave = especiIn.CLAVE,
                                  medico = ex.medico,
                                  //fecha = q.fecha,
                                  especialidad = especiIn.DESCRIPCION,
                                  nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  consultadistancia = ex.consultadistancia
                              })
                              .ToList().Select(x => new
                              {
                                  //clave = x.clave,
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  //fecha = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fecha),
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  //clave = z.clave,
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  //p.fecha,
                                  //p.clave,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = textoFecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

            //Detales
            //string queryDet = "SELECT Dertbl.especialidad as especialidad, Dertbl.medico as medico, Dertbl.nombre as nombre, Count(*) as count, Count(*) as telefonica, Count(*) as presencial  FROM ( SELECT expediente_dental.medico as medico, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as nombre, ESPECIALIDADES.DESCRIPCION as especialidad, COUNT(*) as count FROM expediente_dental, ESPECIALIDADES, MEDICOS WHERE ESPECIALIDADES.CLAVE = Left(expediente_dental.medico, 2) and expediente_dental.fecha >= '" + minDate + "T00:00:00.000' and  expediente_dental.fecha <= '" + maxDate + "T00:00:00.000' AND MEDICOS.Numero = expediente_dental.medico GROUP BY ESPECIALIDADES.DESCRIPCION, medico, MEDICOS.Nombre, MEDICOS.Apaterno, MEDICOS.Amaterno, expediente_dental.expediente) AS Dertbl GROUP BY Dertbl.medico, Dertbl.nombre, Dertbl.especialidad; ";
            string queryDet = "SELECT Dertbl.nombre as nombre, Dertbl.medico as medico, COUNT(*) AS count FROM ( SELECT fecha, expediente, medico, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as nombre FROM expediente_dental INNER JOIN MEDICOS ON MEDICOS.Numero = expediente_dental.medico WHERE fecha >= '" + minDate + "T00:00:00.000' and  fecha <= '" + maxDate + "T00:00:00.000' group by fecha, expediente, medico, MEDICOS.Nombre, MEDICOS.Apaterno, MEDICOS.Amaterno ) AS Dertbl GROUP BY Dertbl.medico, Dertbl.nombre;";

            var resultDenDet = db.Database.SqlQuery<Dental>(queryDet);
            var expedi_den_det = resultDenDet.ToList();


            var resultsDet1 = new List<IndEspListDet>();

            foreach (var item in expediedet)
            {
                var resultDet = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = textoFecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                };

                resultsDet1.Add(resultDet);
            }

            foreach (var item in expedi_den_det)
            {
                var resultDet2 = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = "DENTAL",
                    fecha = textoFecha,
                    count = item.count,
                    telefonica = 0,
                    presencial = item.count,
                };

                resultsDet1.Add(resultDet2);

            }

            //System.Diagnostics.Debug.WriteLine(resultsDet1);

            #endregion




            return new JsonResult { Data = resultsDet1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult IndicadoresResutimiento()
        {
            return View();
        }

        public class IndicadoresResList
        {
            public string medico { get; set; }
            public string especialidad { get; set; }
            public string paciente { get; set; }
            public string fecha { get; set; }
            public int meses_surtir { get; set; }
            public int expediente { get; set; }
        }


        public JsonResult IndicadoresRes()
        {
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
            var fechaUnAnios = DateTime.Now.AddMonths(-1).AddDays(-13).ToString("yyyy-MM-dd");
            var fechaUnAniosDT = DateTime.Parse(fechaUnAnios);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            var recetasres = (from re in db.receta_exp_crn
                                  //join recetaesxp in db.receta_exp_crn on re.folio_rc equals recetaesxp.folio_rc into recetaesxpX
                                  //from recetaesxpIn in recetaesxpX.DefaultIfEmpty()
                                  //join dhabientes in db.DHABIENTES on recetaesxpIn.expediente equals dhabientes.NUMEMP into dhabX
                                  //from dhabIn in dhabX.DefaultIfEmpty()
                                  //join medicos in db.MEDICOS on recetaesxpIn.medico_crea equals medicos.Numero into mediX
                                  //from medicosIn in mediX.DefaultIfEmpty()
                                  //join especi in db.ESPECIALIDADES on recetaesxpIn.medico_crea.Substring(0, 2) equals especi.CLAVE into especiX
                                  //from especiIn in especiX.DefaultIfEmpty()
                              where re.fecha_crea >= fechaUnAniosDT
                              where re.terminada != 0
                              select new
                              {
                                  //medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  //especialidad = especiIn.DESCRIPCION,
                                  //paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                  //fecha = recetaesxpIn.fecha_crea,
                                  //meses_surtir = re.meses_surtir,
                                  //folio_rc = re.folio_rc,
                                  expediente = re.expediente,
                              })
                              .GroupBy(p => new
                              {
                                  //p.medico,
                                  //p.especialidad,
                                  //p.paciente,
                                  //p.fecha,
                                  //p.meses_surtir,
                                  //p.folio_rc,
                                  p.expediente,
                              })
                              .Select(g => new
                              {
                                  //medico = g.Key.medico,
                                  //especialidad = g.Key.especialidad,
                                  //paciente = g.Key.paciente,
                                  //fecha = g.Key.fecha,
                                  //meses_surtir = g.Key.meses_surtir,
                                  //folio_rc = g.Key.folio_rc,
                                  expediente = g.Key.expediente,
                              })
                              .ToList();

            //System.Diagnostics.Debug.WriteLine(recetasres);


            //var resultsRecRes = new List<IndicadoresResList>();
            var count = 0;
            foreach (var item in recetasres)
            {
                //Buscar si su evento aún esta activo
                var recetasresFar = (from re in db2.Receta_Exp
                                         //where re.folio_rc == item.folio_rc
                                     where re.Estado == "2" || re.Estado == "3"
                                     select re).Count();

                var eventos = recetasresFar + 1;

                //if (eventos < item.meses_surtir)
                //{
                //count++;

                /*var resultRR = new IndicadoresResList
                {
                    medico = item.medico,
                    especialidad = item.especialidad,
                    paciente = item.paciente,
                    fecha = string.Format("{0:yyyy-MM-dd}", item.fecha)
                };

                resultsRecRes.Add(resultRR);*/
                //}
            }

            //System.Diagnostics.Debug.WriteLine(count);


            return new JsonResult { Data = recetasres, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult IndicadoresRecetasHU()
        {

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            //Recetas de HU pasadas por el kiosco
            var recetashu = (from q in db.Receta_Exp
                             where q.folio_rcta_hu != null
                             select new
                             {
                                 fechafecha = q.Fecha,
                                 //expexp = q.Expediente
                             })
                           .ToList().Select(x => new
                           {
                               datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                           })
                           .ToList().Select(z => new
                           {
                               fecha_correcta = DateTime.Parse(z.datedate)
                           })
                           .ToList().Select(y => new
                           {
                               fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                           })
                           .GroupBy(p => p.fecha_correcta_correcta)
                           .Select(g => new
                           {
                               Date = g.Key,
                               Count = g.Select(p => p.fecha_correcta_correcta).Count()
                           })
                          .OrderBy(g => g.Date)
                          .ToList();

            var valuesListRHU = JArray.FromObject(recetashu).Select(x => x.Values().ToList()).ToList();


            string jsonRHU = JsonConvert.SerializeObject(valuesListRHU, Formatting.Indented);

            string pathRHU = Server.MapPath("~/json/indicadores/recetas/");
            System.IO.File.WriteAllText(pathRHU + "hu.json", jsonRHU);

            return View();
        }



        public ActionResult IndicadoresOrdenesInt()
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            //Ordenes de int de Servicios Medico
            var ordenesIntSM = (from q in db.Orden_Int
                                where q.tipo == 1
                                where q.proveedor == "012"
                                where q.medico != "02101"
                                select new
                                {
                                    fechafecha = q.fecha_registro,
                                    //expexp = q.Expediente
                                })
                                 .ToList().Select(x => new
                                 {
                                     datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                                 })
                                 .ToList().Select(z => new
                                 {
                                     fecha_correcta = DateTime.Parse(z.datedate)
                                 })
                                 .ToList().Select(y => new
                                 {
                                     fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                                 })
                                 .GroupBy(p => p.fecha_correcta_correcta)
                                 .Select(g => new
                                 {
                                     Date = g.Key,
                                     Count = g.Select(p => p.fecha_correcta_correcta).Count()
                                 })
                                .OrderBy(g => g.Date)
                                .ToList();


            var valuesListOISM = JArray.FromObject(ordenesIntSM).Select(x => x.Values().ToList()).ToList();


            string jsonOISM = JsonConvert.SerializeObject(valuesListOISM, Formatting.Indented);

            string pathOISM = Server.MapPath("~/json/indicadores/ordenes/");
            System.IO.File.WriteAllText(pathOISM + "internamiento_sm.json", jsonOISM);


            //Ordenes de int de San Jeronimo
            var ordenesIntSJ = (from q in db.Orden_Int
                                where q.tipo == 1
                                where q.proveedor == "193"
                                where q.medico != "02101"
                                select new
                                {
                                    fechafecha = q.fecha_registro,
                                    //expexp = q.Expediente
                                })
                                 .ToList().Select(x => new
                                 {
                                     datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                                 })
                                 .ToList().Select(z => new
                                 {
                                     fecha_correcta = DateTime.Parse(z.datedate)
                                 })
                                 .ToList().Select(y => new
                                 {
                                     fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                                 })
                                 .GroupBy(p => p.fecha_correcta_correcta)
                                 .Select(g => new
                                 {
                                     Date = g.Key,
                                     Count = g.Select(p => p.fecha_correcta_correcta).Count()
                                 })
                                .OrderBy(g => g.Date)
                                .ToList();


            var valuesListOISJ = JArray.FromObject(ordenesIntSJ).Select(x => x.Values().ToList()).ToList();


            string jsonOISJ = JsonConvert.SerializeObject(valuesListOISJ, Formatting.Indented);

            string pathOISJ = Server.MapPath("~/json/indicadores/ordenes/");
            System.IO.File.WriteAllText(pathOISJ + "internamiento_sj.json", jsonOISJ);



            return View();
        }


        public ActionResult IndicadoresValoracion()
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha_correcta = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            //Urgencias Pensionistas Internados A
            var ordenesIntSM = (from q in db.Orden_Int
                                where q.tipo == 0
                                where q.enviar_a == 1
                                where q.medico != "02101"
                                //where q.fecha_registro != null
                                where q.fecha_registro <= fecha_correcta
                                select new
                                {
                                    fechafecha = q.fecha_registro,
                                    //expexp = q.Expediente
                                })
                                 .ToList().Select(x => new
                                 {
                                     datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                                 })
                                 .ToList().Select(z => new
                                 {
                                     fecha_correcta = DateTime.Parse(z.datedate)
                                 })
                                 .ToList().Select(y => new
                                 {
                                     fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                                 })
                                 .GroupBy(p => p.fecha_correcta_correcta)
                                 .Select(g => new
                                 {
                                     Date = g.Key,
                                     Count = g.Select(p => p.fecha_correcta_correcta).Count()
                                 })
                                .OrderBy(g => g.Date)
                                .ToList();


            var valuesListOISM = JArray.FromObject(ordenesIntSM).Select(x => x.Values().ToList()).ToList();


            string jsonOISM = JsonConvert.SerializeObject(valuesListOISM, Formatting.Indented);

            string pathOISM = Server.MapPath("~/json/indicadores/ordenes/");
            System.IO.File.WriteAllText(pathOISM + "internadosa.json", jsonOISM);


            //Urgencias Shock Trauma HU
            var ordenesIntSJ = (from q in db.Orden_Int
                                where q.tipo == 0
                                where q.enviar_a == 2 || q.enviar_a == null
                                where q.medico != "02101"
                                //where q.fecha_registro != null
                                where q.fecha_registro <= fecha_correcta
                                select new
                                {
                                    fechafecha = q.fecha_registro,
                                    //expexp = q.Expediente
                                })
                                 .ToList().Select(x => new
                                 {
                                     datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                                 })
                                 .ToList().Select(z => new
                                 {
                                     fecha_correcta = DateTime.Parse(z.datedate)
                                 })
                                 .ToList().Select(y => new
                                 {
                                     fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                                 })
                                 .GroupBy(p => p.fecha_correcta_correcta)
                                 .Select(g => new
                                 {
                                     Date = g.Key,
                                     Count = g.Select(p => p.fecha_correcta_correcta).Count()
                                 })
                                .OrderBy(g => g.Date)
                                .ToList();


            var valuesListOISJ = JArray.FromObject(ordenesIntSJ).Select(x => x.Values().ToList()).ToList();


            string jsonOISJ = JsonConvert.SerializeObject(valuesListOISJ, Formatting.Indented);

            string pathOISJ = Server.MapPath("~/json/indicadores/ordenes/");
            System.IO.File.WriteAllText(pathOISJ + "shocktrauma.json", jsonOISJ);



            return View();
        }


        /*public ActionResult IndicadoresCitas()
        {
            //var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            //var fecha_correcta = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            //Urgencias Pensionistas Internados A
            var ordenesIntSM = (from q in db.Orden_Int
                                    //where q.tipo == 0
                                    //where q.enviar_a == 1
                                where q.medico != "02101"
                                //where q.fecha_registro != null
                                //where q.fecha_registro <= fecha_correcta
                                select new
                                {
                                    fechafecha = q.fecha_registro,
                                    //expexp = q.Expediente
                                })
                                 .ToList().Select(x => new
                                 {
                                     datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                                 })
                                 .ToList().Select(z => new
                                 {
                                     fecha_correcta = DateTime.Parse(z.datedate)
                                 })
                                 .ToList().Select(y => new
                                 {
                                     fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                                 })
                                 .GroupBy(p => p.fecha_correcta_correcta)
                                 .Select(g => new
                                 {
                                     Date = g.Key,
                                     Count = g.Select(p => p.fecha_correcta_correcta).Count()
                                 })
                                .OrderBy(g => g.Date)
                                .ToList();


            var valuesListOISM = JArray.FromObject(ordenesIntSM).Select(x => x.Values().ToList()).ToList();


            string jsonOISM = JsonConvert.SerializeObject(valuesListOISM, Formatting.Indented);

            string pathOISM = Server.MapPath("~/json/indicadores/ordenes/");
            System.IO.File.WriteAllText(pathOISM + "internadosa.json", jsonOISM);


            //Urgencias Shock Trauma HU
            var ordenesIntSJ = (from q in db.Orden_Int
                                    //where q.tipo == 0
                                    //where q.enviar_a == 2 || q.enviar_a == null
                                where q.medico != "02101"
                                //where q.fecha_registro != null
                                //where q.fecha_registro <= fecha_correcta
                                select new
                                {
                                    fechafecha = q.fecha_registro,
                                    //expexp = q.Expediente
                                })
                                 .ToList().Select(x => new
                                 {
                                     datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                                 })
                                 .ToList().Select(z => new
                                 {
                                     fecha_correcta = DateTime.Parse(z.datedate)
                                 })
                                 .ToList().Select(y => new
                                 {
                                     fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                                 })
                                 .GroupBy(p => p.fecha_correcta_correcta)
                                 .Select(g => new
                                 {
                                     Date = g.Key,
                                     Count = g.Select(p => p.fecha_correcta_correcta).Count()
                                 })
                                .OrderBy(g => g.Date)
                                .ToList();


            var valuesListOISJ = JArray.FromObject(ordenesIntSJ).Select(x => x.Values().ToList()).ToList();


            string jsonOISJ = JsonConvert.SerializeObject(valuesListOISJ, Formatting.Indented);

            string pathOISJ = Server.MapPath("~/json/indicadores/ordenes/");
            System.IO.File.WriteAllText(pathOISJ + "shocktrauma.json", jsonOISJ);


            return View();

        }*/


        public ActionResult IndicadoresConsultasUER()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("2021-07-26T00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            //EXPEDIENTES MEDICO FAMILIAR
            var expedienteUER = (from q in db.expediente
                                     //where q.medico.StartsWith("02")
                                 where q.hora_termino != null
                                 //where q.folio_rc == null
                                 where q.fecha >= fechaDT
                                 where q.medico == "03130"
                                 || q.medico == "02309"
                                 || q.medico == "02293"
                                 || q.medico == "13031"
                                 || q.medico == "02310"
                                 || q.medico == "02311"
                                 select new
                                 {
                                     fechafecha = q.hora_termino,
                                     medico = q.medico
                                     //expexp = q.Expediente
                                 })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListExpUER = JArray.FromObject(expedienteUER).Select(x => x.Values().ToList()).ToList();

            //System.Diagnostics.Debug.WriteLine(valuesListExpUER);


            string jsonExpUER = JsonConvert.SerializeObject(valuesListExpUER, Formatting.Indented);

            string pathUER = Server.MapPath("~/json/indicadores/consultas/");
            System.IO.File.WriteAllText(pathUER + "expediente_uer.json", jsonExpUER);

            return View();

        }


        public class CitasList
        {
            public int id { get; set; }
            public double fecha { get; set; }
            public int count { get; set; }
        }


        public ActionResult IndicadoresCitas()
        {
            var fecha = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-ddT00:00:00");
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();


            #region  "Kiosco Tarde"
            //citas con retardo por el kiosco
            string query = "select Fecha as start, count(*) as count FROM CITAS WHERE fecha_tarde is not null and Fecha >= '" + fecha + "' and Fecha <= '" + fechaHoy + "' group by Fecha";
            var result = db.Database.SqlQuery<Citas>(query);
            var res = result.ToList();

            //System.Diagnostics.Debug.WriteLine(res);

            var resultsFeTarde = new List<CitasList>();


            foreach (var item in res)
            {
                var fechast = string.Format("{0:yyyy-MM-dd}", item.start);
                var fechaDT = DateTime.Parse(fechast);

                var result2 = new CitasList
                {
                    fecha = fechaDT.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds,
                    count = item.count,
                };

                resultsFeTarde.Add(result2);

            }

            //System.Diagnostics.Debug.WriteLine(resultsFeTarde);

            var valuesListCitasFT = JArray.FromObject(resultsFeTarde).Select(x => x.Values().ToList()).ToList();

            string jsonCitasFT = JsonConvert.SerializeObject(valuesListCitasFT, Formatting.Indented);

            string pathCFT = Server.MapPath("~/json/citas/");
            System.IO.File.WriteAllText(pathCFT + "retrasos.json", jsonCitasFT);

            #endregion


            return View();
        }


        public ActionResult IndicadoresPruebaAntigenos()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("2021-07-26T00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            var fechaDT2 = DateTime.Parse("2021-09-06T00:00:00.000");
            var Fechamil2 = fechaDT2.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;

            # region "Negativos"
            //Prueba antigenos Negativos
            var antigenosNeg = (from q in db.PruebaAntigenos
                                where q.usuario != "02101"
                                where q.resultado == 0
                                //where q.fecha <= fechaDT2

                                select new
                                {
                                    fechafecha = q.fecha,
                                })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();


            var antigenosNeg2 = (from q in db.CovidSolicitud
                                 where q.UsuarioRealiza != "02101"
                                 where q.Resultado == 0
                                 select new
                                 {
                                     fechafecha = q.FechaResultado,
                                 })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            //System.Diagnostics.Debug.WriteLine(antigenosNeg);

            //System.Diagnostics.Debug.WriteLine(antigenosNeg2);

            var resultsNeg = new List<IndGrafica>();

            int fusion = 0;

            foreach (var item in antigenosNeg)
            {
                if (item.Date == Fechamil2)
                {
                    //System.Diagnostics.Debug.WriteLine(item.Date);
                    fusion = item.Count;
                }
                else
                {
                    var result1 = new IndGrafica
                    {
                        Date = item.Date,
                        Count = item.Count,
                    };

                    resultsNeg.Add(result1);
                }



            }

            foreach (var item in antigenosNeg2)
            {

                if (item.Date == Fechamil2)
                {
                    var result1 = new IndGrafica
                    {
                        Date = item.Date,
                        Count = item.Count + fusion,
                    };

                    resultsNeg.Add(result1);
                }
                else
                {
                    var result1 = new IndGrafica
                    {
                        Date = item.Date,
                        Count = item.Count,
                    };

                    resultsNeg.Add(result1);
                }


            }

            //System.Diagnostics.Debug.WriteLine(resultsNeg);

            var valuesListAntiNeg = JArray.FromObject(resultsNeg).Select(x => x.Values().ToList()).ToList();


            string jsonAntiNeg = JsonConvert.SerializeObject(valuesListAntiNeg, Formatting.Indented);

            string pathAntiNeg = Server.MapPath("~/json/indicadores/consultas/");
            System.IO.File.WriteAllText(pathAntiNeg + "prueba_antigenos_negativos.json", jsonAntiNeg);

            #endregion


            # region "Positivos"
            //Prueba antigenos Negativos
            var antigenosNPos2 = (from q in db.PruebaAntigenos
                                  where q.usuario != "02101"
                                  where q.resultado == 1
                                  select new
                                  {
                                      fechafecha = q.fecha,
                                  })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();


            var antigenosNPos = (from q in db.CovidSolicitud
                                 where q.UsuarioRealiza != "02101"
                                 where q.Resultado == 1
                                 select new
                                 {
                                     fechafecha = q.FechaResultado,
                                 })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();


            var resultsPos = new List<IndGrafica>();


            int fusion2 = 0;

            foreach (var item in antigenosNPos2)
            {


                if (item.Date == Fechamil2)
                {
                    //System.Diagnostics.Debug.WriteLine(item.Date);
                    fusion2 = item.Count;
                }
                else
                {
                    var result3 = new IndGrafica
                    {
                        Date = item.Date,
                        Count = item.Count,
                    };

                    resultsPos.Add(result3);
                }
            }


            foreach (var item in antigenosNPos)
            {

                if (item.Date == Fechamil2)
                {
                    var result3 = new IndGrafica
                    {
                        Date = item.Date,
                        Count = item.Count + fusion2,
                    };

                    resultsPos.Add(result3);
                }
                else
                {
                    var result3 = new IndGrafica
                    {
                        Date = item.Date,
                        Count = item.Count,
                    };

                    resultsPos.Add(result3);
                }

            }


            var valuesListAntiPos = JArray.FromObject(resultsPos).Select(x => x.Values().ToList()).ToList();


            string jsonAntiPos = JsonConvert.SerializeObject(valuesListAntiPos, Formatting.Indented);

            string pathAntiPos = Server.MapPath("~/json/indicadores/consultas/");
            System.IO.File.WriteAllText(pathAntiPos + "prueba_antigenos_positivos.json", jsonAntiPos);

            #endregion



            return View();

        }


        public ActionResult IndicadoresPruebaAntigenosDts()
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            //var fecha = DateTime.Now.ToString("2021-07-26T00:00:00.000");
            //var fechaDT = DateTime.Parse(fecha);

            //var fechaDT2 = DateTime.Parse("2021-09-06T00:00:00.000");
            //var Fechamil2 = fechaDT2.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;

            #region  "PruebaAntigenos"

            //DETALLES

            var expediedet = (from ex in db.CovidSolicitud
                              join medicos in db.MEDICOS on ex.Medico equals medicos.Numero into mediX
                              from medicosIn in mediX.DefaultIfEmpty()
                              join dhab in db.DHABIENTES on ex.NumEmp equals dhab.NUMEMP into dhabX
                              from dhabIn in dhabX.DefaultIfEmpty()
                              join afil in db.AFILIADOS on dhabIn.NUMAFIL equals afil.num_contrato into afilX
                              from afilIn in afilX.DefaultIfEmpty()
                              join dep in db.DEPENDENCIAS on afilIn.CVEDEP equals dep.CVEDEP into depX
                              from depIn in depX.DefaultIfEmpty()
                              where ex.FechaResultado >= fechaDT
                              where ex.Resultado != null
                              select new
                              {
                                  medico = ex.Medico,
                                  fecha = ex.FechaResultado,
                                  nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                  dependencia = depIn.DESCR,
                                  resultado = ex.Resultado
                              })
                              .ToList().Select(x => new
                              {
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  paciente = x.paciente,
                                  dependencia = x.dependencia,
                                  resultado = x.resultado
                              })
                              .ToList().Select(z => new
                              {
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  fecha = z.fecha,
                                  paciente = z.paciente,
                                  dependencia = z.dependencia,
                                  resultado = z.resultado
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  p.medico,
                                  p.paciente,
                                  p.dependencia,
                                  p.resultado,
                              })
                             .Select(g => new
                             {
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 fecha = g.Key.fecha,
                                 paciente = g.Key.paciente,
                                 dependencia = g.Key.dependencia,
                                 resultado = g.Key.resultado,
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();



            var expediedet2 = (from ex in db.PruebaAntigenos
                               join medicos in db.MEDICOS on ex.usuario equals medicos.Numero into mediX
                               from medicosIn in mediX.DefaultIfEmpty()
                               join dhab in db.DHABIENTES on ex.numexp equals dhab.NUMEMP into dhabX
                               from dhabIn in dhabX.DefaultIfEmpty()
                               join afil in db.AFILIADOS on dhabIn.NUMAFIL equals afil.num_contrato into afilX
                               from afilIn in afilX.DefaultIfEmpty()
                               join dep in db.DEPENDENCIAS on afilIn.CVEDEP equals dep.CVEDEP into depX
                               from depIn in depX.DefaultIfEmpty()
                               where ex.fecha >= fechaDT
                               where ex.resultado != null
                               select new
                               {
                                   medico = ex.usuario,
                                   fecha = ex.fecha,
                                   nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                   paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                   dependencia = depIn.DESCR,
                                   resultado = ex.resultado
                               })
                              .ToList().Select(x => new
                              {
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  paciente = x.paciente,
                                  dependencia = x.dependencia,
                                  resultado = x.resultado
                              })
                              .ToList().Select(z => new
                              {
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  fecha = z.fecha,
                                  paciente = z.paciente,
                                  dependencia = z.dependencia,
                                  resultado = z.resultado
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  p.medico,
                                  p.paciente,
                                  p.dependencia,
                                  p.resultado,
                              })
                             .Select(g => new
                             {
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 fecha = g.Key.fecha,
                                 paciente = g.Key.paciente,
                                 dependencia = g.Key.dependencia,
                                 resultado = g.Key.resultado,
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();



            var resultsDet1 = new List<ListAntigenosDet>();

            //Fecha que comenzo lo de la unidad ER
            //var fechaUER = DateTime.Now.ToString("2021-07-28");
            //var fechaUERDT = DateTime.Parse(fechaUER);

            //CONSULTAS NORMALES

            foreach (var item in expediedet)
            {

                var pruebaRes = "";

                if (item.resultado == 1)
                {
                    pruebaRes = "Positivo";
                }
                else
                {
                    if (item.resultado == 0)
                    {
                        pruebaRes = "Negativo";
                    }
                }

                var resultDet = new ListAntigenosDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    paciente = item.paciente,
                    dependencia = item.dependencia,
                    fecha = item.fecha,
                    resultado = pruebaRes
                };

                resultsDet1.Add(resultDet);
            }


            foreach (var item in expediedet2)
            {

                var pruebaRes = "";

                if (item.resultado == 1)
                {
                    pruebaRes = "Positivo";
                }
                else
                {
                    if (item.resultado == 0)
                    {
                        pruebaRes = "Negativo";
                    }
                }

                var resultDet = new ListAntigenosDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    paciente = item.paciente,
                    dependencia = item.dependencia,
                    fecha = item.fecha,
                    resultado = pruebaRes
                };

                resultsDet1.Add(resultDet);
            }


            string json2 = JsonConvert.SerializeObject(resultsDet1);
            string path2 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2 + "indicadores/especialidades/detalles_antigenos.json", json2);

            #endregion



            return View();
        }


        public JsonResult ChangeIndicadoresPruebaAntDts(string minDate, string maxDate)
        {
            //System.Diagnostics.Debug.WriteLine(minDate);
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            //var fecha = DateTime.Now.ToString("2021-07-26T00:00:00.000");
            //var fechaDT = DateTime.Parse(fecha);

            //var fechaDT2 = DateTime.Parse("2021-09-06T00:00:00.000");
            //var Fechamil2 = fechaDT2.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;

            var minDateTot = minDate + "T00:00:00.000";
            var maxDateTot = maxDate + "T23:59:59.000";

            var fecha_minDate = DateTime.Parse(minDateTot);
            var fecha_maxDate = DateTime.Parse(maxDateTot);

            #region  "PruebaAntigenos"

            //DETALLES

            var expediedet = (from ex in db.CovidSolicitud
                              where ex.Resultado != null
                              join medicos in db.MEDICOS on ex.Medico equals medicos.Numero into mediX
                              from medicosIn in mediX.DefaultIfEmpty()
                              join dhab in db.DHABIENTES on ex.NumEmp equals dhab.NUMEMP into dhabX
                              from dhabIn in dhabX.DefaultIfEmpty()
                              join afil in db.AFILIADOS on dhabIn.NUMAFIL equals afil.num_contrato into afilX
                              from afilIn in afilX.DefaultIfEmpty()
                              join dep in db.DEPENDENCIAS on afilIn.CVEDEP equals dep.CVEDEP into depX
                              from depIn in depX.DefaultIfEmpty()
                              where ex.FechaResultado >= fecha_minDate
                              where ex.FechaResultado <= fecha_maxDate
                              select new
                              {
                                  medico = ex.Medico,
                                  fecha = ex.FechaResultado,
                                  nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                  dependencia = depIn.DESCR,
                                  resultado = ex.Resultado
                              })
                              .ToList().Select(x => new
                              {
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  paciente = x.paciente,
                                  dependencia = x.dependencia,
                                  resultado = x.resultado
                              })
                              .ToList().Select(z => new
                              {
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  fecha = z.fecha,
                                  paciente = z.paciente,
                                  dependencia = z.dependencia,
                                  resultado = z.resultado
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  p.medico,
                                  p.paciente,
                                  p.dependencia,
                                  p.resultado,
                              })
                             .Select(g => new
                             {
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 fecha = g.Key.fecha,
                                 paciente = g.Key.paciente,
                                 dependencia = g.Key.dependencia,
                                 resultado = g.Key.resultado,
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();



            var expediedet2 = (from ex in db.PruebaAntigenos
                               join medicos in db.MEDICOS on ex.usuario equals medicos.Numero into mediX
                               from medicosIn in mediX.DefaultIfEmpty()
                               join dhab in db.DHABIENTES on ex.numexp equals dhab.NUMEMP into dhabX
                               from dhabIn in dhabX.DefaultIfEmpty()
                               join afil in db.AFILIADOS on dhabIn.NUMAFIL equals afil.num_contrato into afilX
                               from afilIn in afilX.DefaultIfEmpty()
                               join dep in db.DEPENDENCIAS on afilIn.CVEDEP equals dep.CVEDEP into depX
                               from depIn in depX.DefaultIfEmpty()
                               where ex.fecha >= fecha_minDate
                               where ex.fecha <= fecha_maxDate
                               where ex.resultado != null
                               select new
                               {
                                   medico = ex.usuario,
                                   fecha = ex.fecha,
                                   nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                   paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                   dependencia = depIn.DESCR,
                                   resultado = ex.resultado
                               })
                              .ToList().Select(x => new
                              {
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  paciente = x.paciente,
                                  dependencia = x.dependencia,
                                  resultado = x.resultado
                              })
                              .ToList().Select(z => new
                              {
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  fecha = z.fecha,
                                  paciente = z.paciente,
                                  dependencia = z.dependencia,
                                  resultado = z.resultado
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.fecha,
                                  p.medico,
                                  p.paciente,
                                  p.dependencia,
                                  p.resultado,
                              })
                             .Select(g => new
                             {
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 fecha = g.Key.fecha,
                                 paciente = g.Key.paciente,
                                 dependencia = g.Key.dependencia,
                                 resultado = g.Key.resultado,
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();



            var resultsDet1 = new List<ListAntigenosDet>();

            //Fecha que comenzo lo de la unidad ER
            //var fechaUER = DateTime.Now.ToString("2021-07-28");
            //var fechaUERDT = DateTime.Parse(fechaUER);

            //CONSULTAS NORMALES

            foreach (var item in expediedet)
            {

                var pruebaRes = "";

                if (item.resultado == 1)
                {
                    pruebaRes = "Positivo";
                }
                else
                {
                    if (item.resultado == 0)
                    {
                        pruebaRes = "Negativo";
                    }
                }

                var resultDet = new ListAntigenosDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    paciente = item.paciente,
                    dependencia = item.dependencia,
                    fecha = item.fecha,
                    resultado = pruebaRes
                };

                resultsDet1.Add(resultDet);
            }


            foreach (var item in expediedet2)
            {

                var pruebaRes = "";

                if (item.resultado == 1)
                {
                    pruebaRes = "Positivo";
                }
                else
                {
                    if (item.resultado == 0)
                    {
                        pruebaRes = "Negativo";
                    }
                }

                var resultDet = new ListAntigenosDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    paciente = item.paciente,
                    dependencia = item.dependencia,
                    fecha = item.fecha,
                    resultado = pruebaRes
                };

                resultsDet1.Add(resultDet);
            }


            string json2 = JsonConvert.SerializeObject(resultsDet1);
            string path2 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2 + "indicadores/especialidades/detalles_antigenos.json", json2);

            #endregion


            return new JsonResult { Data = expediedet, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        public ActionResult Productividad10()
        {
            if (User.IsInRole("Productividad10")) {


                var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
                var fecha_correcta = DateTime.Parse(fecha);

                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                #region  "Linares"
                //LINARES
                var linares = (from q in db.expediente
                               where q.medico == "52023" || q.medico == "52024" || q.medico == "52025" || q.medico == "52028" || q.medico == "52029" || q.medico == "08053" || q.medico == "38119"
                               where q.fecha >= fecha_correcta
                               where q.hora_termino != null
                               join especi in db.ESPECIALIDADES on q.medico.Substring(0, 2) equals especi.CLAVE into especiX
                               from especiIn in especiX.DefaultIfEmpty()
                               select new
                               {
                                   clave = especiIn.CLAVE,
                                   fecha = q.fecha,
                                   especialidad = especiIn.DESCRIPCION,
                                   consultadistancia = q.consultadistancia
                               })
                                  .ToList().Select(x => new
                                  {
                                      clave = x.clave,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.especialidad,
                                      p.fecha,
                                      p.clave,
                                  //p.consultadistancia,
                              })
                                 .Select(g => new
                                 {
                                     clave = g.Key.clave,
                                     especialidad = g.Key.especialidad,
                                     //especialidad = "Linares",
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 .OrderBy(g => g.especialidad)
                                 .ToList();

                string jsonLi = JsonConvert.SerializeObject(linares);
                string pathLi = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(pathLi + "indicadores/especialidades/expediente_linares.json", jsonLi);



                //LINARES DETALLES
                var linares_detalles = (from ex in db.expediente
                                        where ex.medico == "52023" || ex.medico == "52024" || ex.medico == "52025" || ex.medico == "52028" || ex.medico == "52029" || ex.medico == "08053" || ex.medico == "38119"
                                        where ex.fecha >= fecha_correcta
                                        where ex.hora_termino != null
                                        join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                        from medicosIn in mediX.DefaultIfEmpty()
                                        join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                        from especiIn in especiX.DefaultIfEmpty()
                                        select new
                                        {
                                            //clave = especiIn.CLAVE,
                                            medico = ex.medico,
                                            fecha = ex.fecha,
                                            especialidad = especiIn.DESCRIPCION,
                                            nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                            consultadistancia = ex.consultadistancia
                                        })
                                  .ToList().Select(x => new
                                  {
                                  //clave = x.clave,
                                  medico = x.medico,
                                      nombre = x.nombre,
                                      especialidad = x.especialidad,
                                      fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                      consultadistancia = x.consultadistancia
                                  })
                                  .ToList().Select(z => new
                                  {
                                  //clave = z.clave,
                                  medico = z.medico,
                                      nombre = z.nombre,
                                      especialidad = z.especialidad,
                                      fecha = z.fecha,
                                  //fecha = DateTime.Parse(z.fecha),
                                  consultadistancia = z.consultadistancia
                                  })
                                  .GroupBy(p => new
                                  {
                                      p.nombre,
                                      p.fecha,
                                  //p.clave,
                                  p.medico,
                                      p.especialidad
                                  })
                                 .Select(g => new
                                 {
                                 //clave = g.Key.clave,
                                 medico = g.Key.medico,
                                     nombre = g.Key.nombre,
                                     especialidad = g.Key.especialidad,
                                     //especialidad = "Linares",
                                     fecha = g.Key.fecha,
                                     count = g.Count(),
                                     telefonica = g.Count(p => p.consultadistancia == "1"),
                                     presencial = g.Count(p => p.consultadistancia != "1"),
                                 })
                                 //.OrderBy(g => g.fecha)
                                 .OrderBy(g => g.medico)
                                 .ToList();

                var resultsDet1 = new List<IndEspListDet>();

                foreach (var item in linares_detalles)
                {

                    var especialidad = item.especialidad;

                    if (item.medico == "52023")
                    {
                        especialidad = "MEDICINA GENERAL";
                    }

                    if (item.medico == "52024")
                    {
                        especialidad = "MEDICINA INTERNA";
                    }

                    if (item.medico == "52025")
                    {
                        especialidad = "MEDICINA GENERAL";
                    }

                    if (item.medico == "52028")
                    {
                        especialidad = "PEDIATRIA";
                    }

                    if (item.medico == "52029")
                    {
                        especialidad = "GINECOLOGIA";
                    }

                    if (item.medico == "08053")
                    {
                        especialidad = "GINECOLOGIA";
                    }

                    if (item.medico == "38119")
                    {
                        especialidad = "MEDICINA INTERNA";
                    }

                    var resultDet = new IndEspListDet
                    {
                        medico = item.medico,
                        nombre = item.nombre,
                        especialidad = especialidad,
                        fecha = item.fecha,
                        count = item.count,
                        telefonica = item.telefonica,
                        presencial = item.presencial,
                    };

                    resultsDet1.Add(resultDet);
                }


                string json2_li = JsonConvert.SerializeObject(resultsDet1);
                string path2_li = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(path2_li + "indicadores/especialidades/detalles_linares.json", json2_li);

                #endregion

                #region "Subrogados"


                var expesubro = (from ex in db.MEDICOS
                                     //Subrogados
                                 join especi in db.ESPECIALIDADES on ex.Numero.Substring(0, 2) equals especi.CLAVE into especiX
                                 from especiIn in especiX.DefaultIfEmpty()
                                 where ex.Numero == "38115" || ex.Numero == "38112" || ex.Numero == "38113" || ex.Numero == "14037" || ex.Numero == "38111"
                                 || ex.Numero == "38117" /*|| ex.Numero == "45001"*/ || ex.Numero == "03132" || ex.Numero == "38110" || ex.Numero == "38114"
                                 || ex.Numero == "19017" || ex.Numero == "38116" || ex.Numero == "38118" || ex.Numero == "38120" || ex.Numero == "15512"
                                 || ex.Numero == "34019" || ex.Numero == "51035" || ex.Numero == "38121" || ex.Numero == "38122" || ex.Numero == "38123"
                                 || ex.Numero == "38124" || ex.Numero == "38125"
                                 select new
                                 {
                                     nombre = ex.Nombre + " " + ex.Apaterno + " " + ex.Amaterno,
                                     especialidad = especiIn.DESCRIPCION,
                                     numero = ex.Numero,
                                 })
                                 .ToList();



                var resultsDetSub = new List<IndEspListDet>();

                //CONSULTAS NORMALES

                foreach (var item in expesubro)
                {

                    var expediente = (from ex in db.expediente
                                      where ex.fecha >= fecha_correcta
                                      where ex.hora_termino != null
                                      //Subrogados
                                      where ex.medico == item.numero
                                      select ex)
                                  .Count();

                    var espe = item.especialidad;

                    if (item.numero == "38115")
                    {
                        espe = "GASTRO PEDIATRA";
                    }

                    if (item.numero == "38112")
                    {
                        espe = "ENDOCRINOLOGÍA PEDIÁTRICA";
                    }

                    if (item.numero == "38113")
                    {
                        espe = "NEUMOLOGÍA PEDIÁTRICA";
                    }

                    if (item.numero == "14037")
                    {
                        espe = "NEUROPSICOLOGÍA CLÍNICA";
                    }

                    if (item.numero == "38111")
                    {
                        espe = "HEMATOLOGÍA PEDIÁTRICA";
                    }

                    if (item.numero == "38117")
                    {
                        espe = "CIRUGÍA PEDIÁTRICA";
                    }

                    if (item.numero == "03132")
                    {
                        espe = "REUMATOLOGÍA PEDIÁTRICA";
                    }

                    if (item.numero == "38110")
                    {
                        espe = "TERAPEUTA DE LENGUAJE";
                    }

                    if (item.numero == "38114")
                    {
                        espe = "COLOPROCTOLOGÍA";
                    }

                    if (item.numero == "19017")
                    {
                        espe = "OFTALMOLOGÍA";
                    }

                    if (item.numero == "38116")
                    {
                        espe = "GASTROENTEROLOGÍA PEDIÁTRICA";
                    }

                    if (item.numero == "38118")
                    {
                        espe = "UROLOGÍA PEDIÁTRICA";
                    }

                    if (item.numero == "38120")
                    {
                        espe = "NEUROLOGÍA";
                    }

                    if (item.numero == "15512")
                    {
                        espe = "NEFROLOGÍA PEDIÁTRICA";
                    }

                    if (item.numero == "34019")
                    {
                        espe = "CARDIOLOGÍA PEDIÁTRICA";
                    }

                    if (item.numero == "51035")
                    {
                        espe = "NEONATOLOGÍA";
                    }

                    if (item.numero == "38121")
                    {
                        //espe = "ENDOCRINOLOGÍA";
                        espe = "HU";
                    }

                    if (item.numero == "38122")
                    {
                        espe = "OFTALMOLOGIA";
                    }

                    if (item.numero == "38123")
                    {
                        espe = "CIRUGÍA BARIÁTRICA Y LAPAROSCOPÍA";
                    }

                    if (item.numero == "38124")
                    {
                        espe = "OTORINOLARINGOLOGÍA";
                    }

                    if (item.numero == "38125")
                    {
                        espe = "PSICOLOGÍA";
                    }




                    if (expediente != 0)
                    {
                        var resultSub = new IndEspListDet
                        {
                            medico = item.numero,
                            nombre = item.nombre,
                            especialidad = espe,
                            fecha = fechaHoy,
                            count = expediente,
                            telefonica = 0,
                            presencial = expediente,
                        };

                        resultsDetSub.Add(resultSub);
                    }
                    else
                    {
                        var resultSub = new IndEspListDet
                        {
                            medico = item.numero,
                            nombre = item.nombre,
                            especialidad = espe,
                            fecha = fechaHoy,
                            count = 0,
                            telefonica = 0,
                            presencial = 0,
                        };

                        resultsDetSub.Add(resultSub);
                    }




                }

                string jsonSub = JsonConvert.SerializeObject(resultsDetSub);
                string pathSub = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(pathSub + "indicadores/especialidades/detallessubrogados.json", jsonSub);


                #endregion

                #region "Foraneos"


                var expeFor2 = (from ex in db.MEDICOS
                                //Subrogados
                                join especi in db.ESPECIALIDADES on ex.Numero.Substring(0, 2) equals especi.CLAVE into especiX
                                from especiIn in especiX.DefaultIfEmpty()
                                where ex.Numero != "02101"
                                where ex.Numero == "02319" || ex.Numero == "02316" || ex.Numero == "02318" || ex.Numero == "02317" || ex.Numero == "38126" || ex.Numero == "02321" 
                                || ex.Numero == "02324" || ex.Numero == "38128" || ex.Numero == "38129" || ex.Numero == "38127" || ex.Numero == "02347"
                                //Montemorelos
                                || ex.Numero == "03139" || ex.Numero == "05041" || ex.Numero == "08058" || ex.Numero == "08059" || ex.Numero == "19018" || ex.Numero == "03140"
                                || ex.Numero == "02334" || ex.Numero == "13032" || /*ex.Numero == "41026" ||*/ ex.Numero == "06026" || ex.Numero == "02340"
                                //Cerralvo
                                || ex.Numero == "02333"
                                //Allende
                                || ex.Numero == "02321" || ex.Numero == "07017" || ex.Numero == "08060"
                                || ex.Numero == "05042" || ex.Numero == "14038" || ex.Numero == "05043" || ex.Numero == "18015" || ex.Numero == "06027"
                                || ex.Numero == "02338" || ex.Numero == "02336" || ex.Numero == "02337" || ex.Numero == "13035"
                                //Sabinas 
                                || ex.Numero == "02341"
                                //Cadereyta
                            || ex.Numero == "52034" || ex.Numero == "02349" || ex.Numero == "02350" || ex.Numero == "02351" || ex.Numero == "02352"
                            || ex.Numero == "02353" || ex.Numero == "02354" || ex.Numero == "02355" || ex.Numero == "02356" || ex.Numero == "02357"

                                select new
                                {
                                     nombre = ex.Nombre + " " + ex.Apaterno + " " + ex.Amaterno,
                                     especialidad = especiIn.DESCRIPCION,
                                     numero = ex.Numero,
                                })
                                .ToList();



                var resultsDetFor2 = new List<IndEspListDet>();

                //CONSULTAS NORMALES

                foreach (var item in expeFor2)
                {

                    var expediente = (from ex in db.expediente
                                      where ex.fecha >= fecha_correcta
                                      where ex.hora_termino != null
                                      //Subrogados
                                      where ex.medico == item.numero
                                      select ex)
                                  .Count();

                    var lugar = "";
                    var total = 0;
                    var espe = "";

                    if (item.numero == "02319")
                    {
                        total = expediente;
                        lugar = "Hidalgo";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02316")
                    {
                        total = expediente;
                        lugar = "General Terán";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02318")
                    {
                        total = expediente;
                        lugar = "Marín";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02317")
                    {
                        total = expediente;
                        lugar = "Santiago";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "38126")
                    {
                        total = expediente;
                        lugar = "Doctor Arroyo";
                        espe = "MEDICINA GENERAL";
                    }

                    //var count1 = 0;
                    if (item.numero == "03135" || item.numero == "02324")
                    {
                        var expediente2 = (from ex in db.expediente
                                          where ex.fecha >= fecha_correcta
                                          where ex.hora_termino != null
                                          //Subrogados
                                          where ex.medico == "03135" || ex.medico == "02324"
                                          select ex)
                                          .Count();
                        total = expediente2;
                        lugar = "Sabinas";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02341")
                    {
                        total = expediente;
                        lugar = "Sabinas";
                        espe = "MEDICINA GENERAL";
                    }


                    if (item.numero == "38128")
                    {
                        total = expediente;
                        lugar = "García";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "38129")
                    {
                        total = expediente;
                        lugar = "China";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02347")
                    {
                        total = expediente;
                        lugar = "China";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "38127")
                    {
                        total = expediente;
                        lugar = "Anahuac";
                        espe = "MEDICINA GENERAL";
                    }

                    //Montemorelos
                    if (item.numero == "02334")
                    {
                        total = expediente;
                        lugar = "Montemorelos - Clínica San Mateo";
                        espe = "MEDICINA GENERAL";
                    }


                    if (item.numero == "06026")
                    {
                        total = expediente;
                        lugar = "Montemorelos - Clínica San Mateo";
                        espe = "CIRUGÍA GENERAL";
                    }


                    if (item.numero == "03139" || item.numero == "03140")
                    {
                        total = expediente;
                        lugar = "Montemorelos - Clínica San Mateo";
                        espe = "PEDIATRÍA";
                    }

                    if (item.numero == "05041")
                    {
                        total = expediente;
                        lugar = "Montemorelos - Clínica San Mateo";
                        espe = "TRAUMATOLOGÍA Y ORTOPEDIA";
                    }


                    if (item.numero == "08058" || item.numero == "08059")
                    {
                        total = expediente;
                        lugar = "Montemorelos - Clínica San Mateo";
                        espe = "GINECOLOGÍA";
                    }


                    if (item.numero == "19018")
                    {
                        total = expediente;
                        lugar = "Montemorelos - Clínica San Mateo";
                        espe = "OFTALMOLOGÍA";
                    }


                    /*if (item.numero == "41026")
                    {
                        total = expediente;
                        lugar = "Montemorelos - Clínica San Mateo";
                        espe = "RADIOLOGÍA";
                    }*/


                    if (item.numero == "13032")
                    {
                        total = expediente;
                        lugar = "Montemorelos - Clínica San Mateo";
                        espe = "MEDICINA INTERNA";
                    }


                    if (item.numero == "02333")
                    {
                        total = expediente;
                        lugar = "Ceralvo";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02340")
                    {
                        total = expediente;
                        lugar = "Montemorelos - Clínica San Mateo";
                        espe = "MEDICINA GENERAL";
                    }


                    //Allende
                    if (item.numero == "02321")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "07017")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "OTORRINOLARINGOLOGIA";
                    }

                    if (item.numero == "08060")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "GINECOLOGIA Y OBSTETRICIA";
                    }

                    if (item.numero == "05042")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "TRAUMATOLOGIA";
                    }

                    if (item.numero == "14038")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "NEUROLOGIA";
                    }

                    if (item.numero == "05043")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "TRAUMATOLOGIA";
                    }

                    if (item.numero == "18015")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "UROLOGIA";
                    }

                    if (item.numero == "06027")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "CIRUGIA GENERAL";
                    }

                    if (item.numero == "02338")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02336")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02337")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "13035")
                    {
                        total = expediente;
                        lugar = "Allende";
                        espe = "MEDICINA INTERNA";
                    }

                    //Cadereyta
                    if (item.numero == "52034")
                    {
                        total = expediente;
                        lugar = "Cadereyta";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02349")
                    {
                        total = expediente;
                        lugar = "Cadereyta";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02350")
                    {
                        total = expediente;
                        lugar = "Cadereyta";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02351")
                    {
                        total = expediente;
                        lugar = "Cadereyta";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02352")
                    {
                        total = expediente;
                        lugar = "Cadereyta";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02353")
                    {
                        total = expediente;
                        lugar = "Cadereyta";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02354")
                    {
                        total = expediente;
                        lugar = "Cadereyta";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02355")
                    {
                        total = expediente;
                        lugar = "Cadereyta";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02356")
                    {
                        total = expediente;
                        lugar = "Cadereyta";
                        espe = "MEDICINA GENERAL";
                    }

                    if (item.numero == "02357")
                    {
                        total = expediente;
                        lugar = "Cadereyta";
                        espe = "MEDICINA GENERAL";
                    }


                    if (expediente != 0)
                    {
                        var resultFor2 = new IndEspListDet
                        {
                            medico = item.numero,
                            nombre = item.nombre,
                            especialidad = espe,
                            fecha = fechaHoy,
                            count = expediente,
                            lugar = lugar,
                            telefonica = 0,
                            presencial = expediente,
                        };

                        resultsDetFor2.Add(resultFor2);
                    }
                    else
                    {
                        var resultFor2 = new IndEspListDet
                        {
                            medico = item.numero,
                            nombre = item.nombre,
                            especialidad = espe,
                            fecha = fechaHoy,
                            count = 0,
                            lugar = lugar,
                            telefonica = 0,
                            presencial = 0,
                        };

                        resultsDetFor2.Add(resultFor2);
                    }




                }

                string jsonFor2 = JsonConvert.SerializeObject(resultsDetFor2);
                string pathFor2 = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(pathFor2 + "indicadores/especialidades/detalles_foraneos.json", jsonFor2);


                #endregion


                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public class IncidentesList
        {
            public string area { get; set; }
            public string coordinador { get; set; }
            public string jefe { get; set; }
            public string fecha { get; set; }
            public string tipo { get; set; }
        }



        public ActionResult IncidentesArea()
        {
            Models.incidentesEntities3 db = new Models.incidentesEntities3();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            #region  "Incidentes por area"

            //Incidentes

            var incidentes = (from inci in db.SOL_SERVICIO
                              join areaIncidente in db.sis_areas on inci.area_incidente equals areaIncidente.id_area into areaX
                              join tipoIncidente in db.tipo_incidente on inci.tipo_incidente equals tipoIncidente.clave into tipoX
                              from areaIn in areaX.DefaultIfEmpty()
                              from tipoIn in tipoX.DefaultIfEmpty()
                              where inci.fecha_incidente >= fechaDT
                              select new
                              {
                                  area = areaIn.descripcion,
                                  coordinador = areaIn.coordmedico,
                                  jefe = areaIn.COORDINADOR,
                                  fecha = inci.fecha_incidente,
                                  tipo = tipoIn.descripcion
                              })
                              .ToList().Select(x => new
                              {
                                  area = x.area,
                                  coordinador = x.coordinador,
                                  fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  jefe = x.jefe,
                                  tipo = x.tipo
                              })
                              .ToList().Select(z => new
                              {
                                  area = z.area,
                                  coordinador = z.coordinador,
                                  fecha = z.fecha,
                                  jefe = z.jefe,
                                  tipo = z.tipo
                              })
                              .GroupBy(p => new
                              {
                                  p.area,
                                  p.fecha,
                                  p.coordinador,
                                  p.jefe,
                                  p.tipo,
                              })
                             .Select(g => new
                             {
                                 area = g.Key.area,
                                 coordinador = g.Key.coordinador,
                                 fecha = g.Key.fecha,
                                 jefe = g.Key.jefe,
                                 tipo = g.Key.tipo,
                                 count = g.Count(),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.area)
                             .ToList();


            string json2 = JsonConvert.SerializeObject(incidentes);
            string path2 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2 + "incidentes/area_detalles.json", json2);

            #endregion

            return View();
            //return new JsonResult { Data = incidentes, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult IncidentesTipo()
        {

            Models.incidentesEntities3 db = new Models.incidentesEntities3();

            #region "Quejas"
            //QUEJAS
            var incidentes_queja = (from q in db.SOL_SERVICIO
                                    where q.tipo_incidente == 1
                                    select new
                                    {
                                        fechafecha = q.fecha_incidente,
                                    })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var values1 = JArray.FromObject(incidentes_queja).Select(x => x.Values().ToList()).ToList();


            string json1 = JsonConvert.SerializeObject(values1, Formatting.Indented);

            string path1 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path1 + "incidentes/quejas.json", json1);

            #endregion

            #region "Recomendaciones"

            //RECOMENDACION
            var incidentes_reco = (from q in db.SOL_SERVICIO
                                    where q.tipo_incidente == 2
                                    select new
                                    {
                                        fechafecha = q.fecha_incidente,
                                    })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();


            var values2 = JArray.FromObject(incidentes_reco).Select(x => x.Values().ToList()).ToList();

            string json2 = JsonConvert.SerializeObject(values2, Formatting.Indented);

            string path2 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2 + "incidentes/recomendaciones.json", json2);

            #endregion

            #region "Felicitaciones"

            //RECOMENDACION
            var incidentes_feli = (from q in db.SOL_SERVICIO
                                   where q.tipo_incidente == 3
                                   select new
                                   {
                                       fechafecha = q.fecha_incidente,
                                   })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();


            var values3 = JArray.FromObject(incidentes_feli).Select(x => x.Values().ToList()).ToList();

            string json3 = JsonConvert.SerializeObject(values3, Formatting.Indented);

            string path3 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path3 + "incidentes/felicitaciones.json", json3);

            #endregion

            #region "Inconformidades"

            //RECOMENDACION
            var incidentes_inco = (from q in db.SOL_SERVICIO
                                   where q.tipo_incidente == 4
                                   select new
                                   {
                                       fechafecha = q.fecha_incidente,
                                   })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();


            var values4 = JArray.FromObject(incidentes_inco).Select(x => x.Values().ToList()).ToList();

            string json4 = JsonConvert.SerializeObject(values4, Formatting.Indented);

            string path4 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path4 + "incidentes/inconformidades.json", json4);

            #endregion

            return View();
        }


        public JsonResult ChangeIndicadores(string minDate, string maxDate)
        {

            //System.Diagnostics.Debug.WriteLine(minDate);

            var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

            //System.Diagnostics.Debug.WriteLine(fechaHoy);


            var fecha_minDate = DateTime.Parse(minDate);
            var fecha_maxDate = DateTime.Parse(maxDate);

            //System.Diagnostics.Debug.WriteLine(fecha_minDate);

            var textoFecha = "";

            if (minDate == maxDate)
            {
                textoFecha = minDate;
            }
            else
            {
                textoFecha = "De: " + minDate + " Hasta: " + maxDate;
            }

            //System.Diagnostics.Debug.WriteLine(fecha_minDate);


            Models.incidentesEntities3 db = new Models.incidentesEntities3();

            #region  "Incidentes por area"

            //Incidentes

            var incidentes = (from inci in db.SOL_SERVICIO
                              join areaIncidente in db.sis_areas on inci.area_incidente equals areaIncidente.id_area into areaX
                              join tipoIncidente in db.tipo_incidente on inci.tipo_incidente equals tipoIncidente.clave into tipoX
                              from areaIn in areaX.DefaultIfEmpty()
                              from tipoIn in tipoX.DefaultIfEmpty()
                              where inci.fecha_incidente >= fecha_minDate
                              where inci.fecha_incidente <= fecha_maxDate
                              select new
                              {
                                  area = areaIn.descripcion,
                                  coordinador = areaIn.coordmedico,
                                  jefe = areaIn.COORDINADOR,
                                  //fecha = inci.fecha_incidente,
                                  tipo = tipoIn.descripcion
                              })
                              .ToList().Select(x => new
                              {
                                  area = x.area,
                                  coordinador = x.coordinador,
                                  //fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                                  jefe = x.jefe,
                                  tipo = x.tipo
                              })
                              .ToList().Select(z => new
                              {
                                  area = z.area,
                                  coordinador = z.coordinador,
                                  //fecha = z.fecha,
                                  jefe = z.jefe,
                                  tipo = z.tipo
                              })
                              .GroupBy(p => new
                              {
                                  p.area,
                                 // p.fecha,
                                  p.coordinador,
                                  p.jefe,
                                  p.tipo,
                              })
                             .Select(g => new
                             {
                                 area = g.Key.area,
                                 coordinador = g.Key.coordinador,
                                 fecha = textoFecha,
                                 jefe = g.Key.jefe,
                                 tipo = g.Key.tipo,
                                 count = g.Count(),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.area)
                             .ToList();


            string json2 = JsonConvert.SerializeObject(incidentes);
            string path2 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2 + "incidentes/area_detalles.json", json2);

            #endregion


            return new JsonResult { Data = incidentes, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public ActionResult IndicadoresCU()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("2021-10-13T00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);


            var expCU = (from q in db.expediente
                         where q.consultadistancia == "1"
                         where q.hora_termino != null
                         //where q.folio_rc == null
                         where q.fecha >= fechaDT
                         where q.medico == "22023" || q.medico == "02235" || q.medico == "62003" || q.medico == "08055" || q.medico == "05038"
                         select new
                         {
                            fechafecha = q.hora_termino,
                            medico = q.medico
                         })
                         .ToList().Select(x => new
                         {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                         })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListCU1 = JArray.FromObject(expCU).Select(x => x.Values().ToList()).ToList();


            string jsonExpCU1 = JsonConvert.SerializeObject(valuesListCU1, Formatting.Indented);

            string pathExpCU1 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpCU1 + "expediente_cu_telefonica.json", jsonExpCU1);


            //EXPEDIENTES CU PRESENCIAL
            var expCU2 = (from q in db.expediente
                         where q.consultadistancia != "1"
                         where q.hora_termino != null
                         //where q.folio_rc == null
                         where q.fecha >= fechaDT
                         where q.medico == "22023" || q.medico == "02235" || q.medico == "62003" || q.medico == "08055" || q.medico == "05038"
                         select new
                         {
                             fechafecha = q.hora_termino,
                             medico = q.medico
                         })
                        .ToList().Select(x => new
                        {
                            datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                        .ToList().Select(z => new
                        {
                            fecha_correcta = DateTime.Parse(z.datedate)
                        })
                        .ToList().Select(y => new
                        {
                            fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds

                        })
                       .GroupBy(p => p.fecha_correcta_correcta)
                       .Select(g => new
                       {
                           Date = g.Key,
                           Count = g.Select(p => p.fecha_correcta_correcta).Count()
                       })
                       .OrderBy(g => g.Date)
                       .ToList();

            var valuesListExpCU2 = JArray.FromObject(expCU2).Select(x => x.Values().ToList()).ToList();

            string jsonExpCU2 = JsonConvert.SerializeObject(valuesListExpCU2, Formatting.Indented);

            string pathExpCU2 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpCU2 + "expediente_cu_presencial.json", jsonExpCU2);

            return View();
        }


        public ActionResult IndicadoresRecetasCU()
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("2021-10-13T00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);


            var expCU = (from q in db.Receta_Exp
                         where q.Hora_Rcta != null
                         where q.Dir_Ip.StartsWith("148.234.218")
                         //where q.folio_rc == null
                         where q.Fecha >= fechaDT
                         where q.Medico == "22023" || q.Medico == "02235" || q.Medico == "62003" || q.Medico == "08055" || q.Medico == "05038"
                         select new
                         {
                             fechafecha = q.Hora_Rcta,
                             medico = q.Medico
                         })
                         .ToList().Select(x => new
                         {
                             datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                         })
                         .ToList().Select(z => new
                         {
                             fecha_correcta = DateTime.Parse(z.datedate)
                         })
                         .ToList().Select(y => new
                         {
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds

                         })
                        .GroupBy(p => p.fecha_correcta_correcta)
                        .Select(g => new
                        {
                            Date = g.Key,
                            Count = g.Select(p => p.fecha_correcta_correcta).Count()
                        })
                        .OrderBy(g => g.Date)
                        .ToList();

            var valuesListCU1 = JArray.FromObject(expCU).Select(x => x.Values().ToList()).ToList();

            string jsonExpCU1 = JsonConvert.SerializeObject(valuesListCU1, Formatting.Indented);

            string pathExpCU1 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpCU1 + "indicadores/recetas/cu.json", jsonExpCU1);


            return View();
        }


        public class RecetasEdad
        {
            public string nombre { get; set; }
            public string medico { get; set; }
            public string numexp { get; set; }
            public int folio_rcta { get; set; }
            public string medicamento { get; set; }
            public float? precio { get; set; }
            public int edad { get; set; }
            public string edad2 { get; set; }
            public string fecha { get; set; }
        }

        public ActionResult IndicadoresRecetasPacientes()
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_correcta = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            #region "Precio Recetas"

            var recetas = (from rc in db.Receta_Detalle_2
                               //where ex.medico != "02101"
                           join recetaExp in db.RECETA_EXP_2 on rc.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                           from receIn in reExp.DefaultIfEmpty()
                           join sustancia in db.Sustancia on rc.Codigo equals sustancia.Clave into sustanciaX
                           from sustanciaIn in sustanciaX.DefaultIfEmpty()
                           join dhabientes in db.DHABIENTES on receIn.Expediente equals dhabientes.NUMEMP into dhabientesX
                           from dhabientesIn in dhabientesX.DefaultIfEmpty()
                           where receIn.Medico != "02101"
                           where receIn.Fecha >= fecha_correcta
                           where receIn.Hora_Rcta != null
                           where receIn.Estado == "2" || receIn.Estado == "3"
                           select new
                           {
                               numexp = receIn.Expediente,
                               folio_rcta = receIn.Folio_Rcta,
                               medicamento = rc.Codigo,
                               precio = sustanciaIn.cto_promedio,
                               fnac = dhabientesIn.FNAC,
                               fecha = receIn.Fecha,
                           })
                         .ToList().Select(x => new
                         {
                             fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                             numexp = x.numexp,
                             precio = x.precio
                         })
                         .ToList().Select(z => new
                         {
                             fecha = DateTime.Parse(z.fecha),
                             numexp = z.numexp,
                             precio = z.precio
                         })
                         .GroupBy(p => new
                         {
                             //p.folio_rcta,
                             p.fecha,
                             p.numexp,
                         })
                         .Select(
                            g => new
                            {
                                //folio_rcta = g.Key.folio_rcta,
                                precioTot = g.Sum(s => s.precio),
                                fecha = g.Key.fecha,
                                numexp = g.Key.numexp,
                                //numexp = g.First().numexp,
                                //fecha = g.First().fecha,
                                //Category = g.First().Category
                            })
                         //.OrderByDescending(g => g.folio_rcta)
                         .ToList();

            //System.Diagnostics.Debug.WriteLine(recetas);

            var resultRecetasEdad = new List<RecetasEdad>();

            foreach (var item in recetas)
            {

                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == item.numexp
                                  select a).FirstOrDefault();

                var today = item.fecha;
                DateTime fnac = (DateTime)dhabientes.FNAC;
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



                var resultRE = new RecetasEdad
                {
                    nombre = dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO,
                    numexp = item.numexp,
                    //folio_rcta = item.folio_rcta,
                    precio = item.precioTot,
                    //edad2 = "60 a 65",
                    edad = Years,
                    fecha = string.Format("{0:yyyy-MM-dd}", item.fecha)
                };

                resultRecetasEdad.Add(resultRE);

            }

            //System.Diagnostics.Debug.WriteLine(resultRecetasEdad);


            string jsonCU = JsonConvert.SerializeObject(resultRecetasEdad);
            string pathCU = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathCU + "recetas/pacientes.json", jsonCU);


            #endregion


            return View();


        }


        public JsonResult ChangeIndicadoresRctPac(string minDate, string maxDate)
        {

            var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

            var maxdateFinal = maxDate + "T23:59:59.000";

            var fecha_minDate = DateTime.Parse(minDate);
            var fecha_maxDate = DateTime.Parse(maxdateFinal);

            var textoFecha = "";

            if (minDate == maxDate)
            {
                textoFecha = minDate;
            }
            else
            {
                textoFecha = "De: " + minDate + " Hasta: " + maxDate;
            }

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            #region "Precio Recetas"

            var recetas = (from rc in db.Receta_Detalle_2
                           //where ex.medico != "02101"
                           join recetaExp in db.RECETA_EXP_2 on rc.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                           from receIn in reExp.DefaultIfEmpty()
                           join sustancia in db.Sustancia on rc.Codigo equals sustancia.Clave into sustanciaX
                           from sustanciaIn in sustanciaX.DefaultIfEmpty()
                           join dhabientes in db.DHABIENTES on receIn.Expediente equals dhabientes.NUMEMP into dhabientesX
                           from dhabientesIn in dhabientesX.DefaultIfEmpty()
                           where receIn.Medico != "02101"
                           where receIn.Fecha >= fecha_minDate
                           where receIn.Fecha <= fecha_maxDate
                           where receIn.Hora_Rcta != null
                           where receIn.Estado == "2" || receIn.Estado == "3"
                           select new
                           {
                               numexp = receIn.Expediente,
                               folio_rcta = receIn.Folio_Rcta,
                               medicamento = rc.Codigo,
                               precio = sustanciaIn.cto_promedio,
                               fnac = dhabientesIn.FNAC,
                               fecha = receIn.Fecha,
                           })
                         .ToList().Select(x => new
                         {
                             fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                             numexp = x.numexp,
                             precio = x.precio
                         })
                         .ToList().Select(z => new
                         {
                             fecha = DateTime.Parse(z.fecha),
                             numexp = z.numexp,
                             precio = z.precio
                         })
                         .GroupBy(p => new
                         {
                             //p.folio_rcta,
                             p.fecha,
                             p.numexp,
                         })
                         .Select(
                            g => new
                            {
                                //folio_rcta = g.Key.folio_rcta,
                                precioTot = g.Sum(s => s.precio),
                                fecha = g.Key.fecha,
                                numexp = g.Key.numexp,
                                //numexp = g.First().numexp,
                                //fecha = g.First().fecha,
                                //Category = g.First().Category
                            })
                         //.OrderByDescending(g => g.folio_rcta)
                         .ToList();

            //System.Diagnostics.Debug.WriteLine(recetas);

            var resultRecetasEdad = new List<RecetasEdad>();

            foreach (var item in recetas)
            {

                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == item.numexp
                                  select a).FirstOrDefault();

                var today = item.fecha;
                DateTime fnac = (DateTime)dhabientes.FNAC;
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



                var resultRE = new RecetasEdad
                {
                    nombre = dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO,
                    numexp = item.numexp,
                    //folio_rcta = item.folio_rcta,
                    precio = item.precioTot,
                    //edad2 = "60 a 65",
                    edad = Years,
                    fecha = textoFecha
                };

                resultRecetasEdad.Add(resultRE);

            }

            //System.Diagnostics.Debug.WriteLine(resultRecetasEdad);


            string jsonCU = JsonConvert.SerializeObject(resultRecetasEdad);
            string pathCU = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathCU + "recetas/pacientes.json", jsonCU);


            #endregion

            return new JsonResult { Data = recetas, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult IndicadoresRecetasMedicos()
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_correcta = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            #region "Precio Recetas"

            var recetas = (from rc in db.Receta_Detalle_2
                               //where ex.medico != "02101"
                           join recetaExp in db.RECETA_EXP_2 on rc.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                           from receIn in reExp.DefaultIfEmpty()
                           join sustancia in db.Sustancia on rc.Codigo equals sustancia.Clave into sustanciaX
                           from sustanciaIn in sustanciaX.DefaultIfEmpty()
                           join medico in db.MEDICOS on receIn.Medico equals medico.Numero into medicoX
                           from medicoIn in medicoX.DefaultIfEmpty()
                           where receIn.Medico != "02101"
                           where receIn.Fecha >= fecha_correcta
                           where receIn.Hora_Rcta != null
                           where receIn.Estado == "2" || receIn.Estado == "3"
                           select new
                           {
                               medico = receIn.Medico,
                               //folio_rcta = receIn.Folio_Rcta,
                               medicamento = rc.Codigo,
                               precio = sustanciaIn.cto_promedio,
                               //fnac = medicoIn.FNAC,
                               fecha = receIn.Fecha,
                           })
                         .ToList().Select(x => new
                         {
                             //folio_rcta = x.folio_rcta,
                             fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                             medico = x.medico,
                             precio = x.precio
                         })
                         .ToList().Select(z => new
                         {
                             //folio_rcta = z.folio_rcta,
                             fecha = DateTime.Parse(z.fecha),
                             medico = z.medico,
                             precio = z.precio
                         })
                         .GroupBy(p => new
                         {
                             //p.folio_rcta,
                             p.fecha,
                             p.medico,
                         })
                         .Select(
                            g => new
                            {
                                //folio_rcta = g.Key.folio_rcta,
                                precioTot = g.Sum(s => s.precio),
                                fecha = g.Key.fecha,
                                medico = g.Key.medico,
                                //folio_rcta = g.First().folio_rcta,
                                //fecha = g.First().fecha,
                                //Category = g.First().Category
                            })
                         //.OrderByDescending(g => g.folio_rcta)
                         .ToList();

            //System.Diagnostics.Debug.WriteLine(recetas);

            var resultRecetasEdad = new List<RecetasEdad>();

            foreach (var item in recetas)
            {

                var medicoNom = "";

                if (item.medico == "51000")
                {
                    //Tomar el folio de hu
                    medicoNom = "Médico HU";

                }
                else
                {
                    var medicoInfo = (from a in db.MEDICOS
                                      where a.Numero == item.medico
                                      select a).FirstOrDefault();

                    medicoNom = medicoInfo.Nombre + " " + medicoInfo.Apaterno + " " + medicoInfo.Amaterno;
                }




                var resultRE = new RecetasEdad
                {
                    nombre = medicoNom,
                    medico = item.medico,
                    //folio_rcta = item.folio_rcta,
                    precio = item.precioTot,
                    //edad2 = "60 a 65",
                    //edad = Years,
                    fecha = string.Format("{0:yyyy-MM-dd}", item.fecha)
                };

                resultRecetasEdad.Add(resultRE);

            }

            //System.Diagnostics.Debug.WriteLine(resultRecetasEdad);


            string jsonCU = JsonConvert.SerializeObject(resultRecetasEdad);
            string pathCU = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathCU + "recetas/medicos.json", jsonCU);


            #endregion


            return View();


        }


        public JsonResult ChangeIndicadoresRctMed(string minDate, string maxDate)
        {

            var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");

            var maxdateFinal = maxDate + "T23:59:59.000";

            var fecha_minDate = DateTime.Parse(minDate);
            var fecha_maxDate = DateTime.Parse(maxdateFinal);

            var textoFecha = "";

            if (minDate == maxDate)
            {
                textoFecha = minDate;
            }
            else
            {
                textoFecha = "De: " + minDate + " Hasta: " + maxDate;
            }

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();


            #region "Precio Recetas"

            var recetas = (from rc in db.Receta_Detalle_2
                               //where ex.medico != "02101"
                           join recetaExp in db.RECETA_EXP_2 on rc.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                           from receIn in reExp.DefaultIfEmpty()
                           join sustancia in db.Sustancia on rc.Codigo equals sustancia.Clave into sustanciaX
                           from sustanciaIn in sustanciaX.DefaultIfEmpty()
                           join medico in db.MEDICOS on receIn.Medico equals medico.Numero into medicoX
                           from medicoIn in medicoX.DefaultIfEmpty()
                           where receIn.Medico != "02101"
                           where receIn.Fecha >= fecha_minDate
                           where receIn.Fecha <= fecha_maxDate
                           where receIn.Hora_Rcta != null
                           where receIn.Estado == "2" || receIn.Estado == "3"
                           select new
                           {
                               medico = receIn.Medico,
                               folio_rcta = receIn.Folio_Rcta,
                               medicamento = rc.Codigo,
                               precio = sustanciaIn.cto_promedio,
                               //fnac = medicoIn.FNAC,
                               fecha = receIn.Fecha,
                           })
                         .ToList().Select(x => new
                         {
                             folio_rcta = x.folio_rcta,
                             fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                             medico = x.medico,
                             precio = x.precio
                         })
                         .ToList().Select(z => new
                         {
                             folio_rcta = z.folio_rcta,
                             fecha = DateTime.Parse(z.fecha),
                             medico = z.medico,
                             precio = z.precio
                         })
                         .GroupBy(p => new
                         {
                             p.folio_rcta,
                             p.fecha,
                             p.medico,
                         })
                         .Select(
                            g => new
                            {
                                folio_rcta = g.Key.folio_rcta,
                                precioTot = g.Sum(s => s.precio),
                                fecha = g.Key.fecha,
                                medico = g.Key.medico,
                                //numexp = g.First().numexp,
                                //fecha = g.First().fecha,
                                //Category = g.First().Category
                            })
                         //.OrderByDescending(g => g.folio_rcta)
                         .ToList();

            //System.Diagnostics.Debug.WriteLine(recetas);

            var resultRecetasEdad = new List<RecetasEdad>();

            foreach (var item in recetas)
            {
                var medicoNom = "";

                if (item.medico == "51000")
                {
                    //Tomar el folio de hu
                    medicoNom = "Médico HU";

                }
                else
                {
                    var medicoInfo = (from a in db.MEDICOS
                                      where a.Numero == item.medico
                                      select a).FirstOrDefault();

                    medicoNom = medicoInfo.Nombre + " " + medicoInfo.Apaterno + " " + medicoInfo.Amaterno;
                }


                var resultRE = new RecetasEdad
                {
                    nombre = medicoNom,
                    medico = item.medico,
                    //folio_rcta = item.folio_rcta,
                    precio = item.precioTot,
                    //edad2 = "60 a 65",
                    //edad = Years,
                    fecha = textoFecha
                };

                resultRecetasEdad.Add(resultRE);

            }

            //System.Diagnostics.Debug.WriteLine(resultRecetasEdad);


            string jsonCU = JsonConvert.SerializeObject(resultRecetasEdad);
            string pathCU = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathCU + "recetas/medicos.json", jsonCU);


            #endregion



            return new JsonResult { Data = recetas, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



    }
}