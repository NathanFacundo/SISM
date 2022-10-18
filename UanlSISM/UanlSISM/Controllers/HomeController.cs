using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using UanlSISM.Models;
using System.Data.Entity;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Net.Mail;
using Postal;
using System.Net;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Globalization;
using System.Drawing.Imaging;

namespace UanlSISM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var dhabientes = (from a in db.DHABIENTES
                              where a.FVIGENCIA >= fechaDT
                              where a.BAJA != "02"
                              where a.BAJA != "04"
                              select a).Count();

            System.Diagnostics.Debug.WriteLine(dhabientes);*/

            return View();
        }


        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult Incidente(string id)
        {

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                Models.incidentesEntities3 db = new Models.incidentesEntities3();
                var nombreusuario = User.Identity.GetUserName();

                var area = int.Parse(id);

                var inci = (from a in db.SOL_SERVICIO
                            where a.folio == area
                            //where a.folio == area
                            join areaIncidente in db.sis_areas on DbFunctions.Right("0" + a.area_incidente, 2) equals areaIncidente.clave into areaX
                            join tipoIncidente in db.tipo_incidente on a.tipo_incidente equals tipoIncidente.clave into tipoX
                            from areaIn in areaX.DefaultIfEmpty()
                            from tipoIn in tipoX.DefaultIfEmpty()
                            orderby a.folio descending
                            select new
                            {
                                fecha_incidente = a.fecha_incidente,
                                expediente = a.expediente,
                                area_incidente = areaIn.descripcion,
                                area = a.area_incidente,
                                coordinador = areaIn.COORDINADOR,
                                tipo_incidente = tipoIn.descripcion,
                                persona_reporta = a.persona_reporta,
                                folio = a.folio,
                                desc_sol = a.DESC_SOL,
                                fecha_seguimiento = a.fecha_seguimiento,
                                seguimiento = a.seguimiento,

                                //expexp = q.Expediente
                            }).ToList();


                var sis_areas = (from r in db.sis_areas
                                     //where r.Clave == search
                                 select new
                                 {
                                     //clave = r.clave,
                                     id_area = r.id_area,
                                     descripcion = r.descripcion,
                                 })
                          .ToList();

                string json = JsonConvert.SerializeObject(inci);
                string path = Server.MapPath("~/json/incidentes/");
                System.IO.File.WriteAllText(path + "incidentes_folio.json", json);

                string json2 = JsonConvert.SerializeObject(sis_areas);
                string path2 = Server.MapPath("~/json/incidentes/");
                System.IO.File.WriteAllText(path2 + "sis_areas.json", json2);

                return View();
            }
        }


        [HttpPost]
        public ActionResult AsignarIncidente(SOL_SERVICIO model)
        {
            //System.Diagnostics.Debug.WriteLine(model.area_incidente);

            Models.incidentesEntities3 db = new Models.incidentesEntities3();
            var sis_area = (from a in db.sis_areas
                            where a.id_area == model.area_incidente
                            select a).FirstOrDefault();

            var sol_servicio = (from a in db.SOL_SERVICIO
                                where a.folio == model.folio
                                select a).FirstOrDefault();


            dynamic email = new Email("Example");
            email.To = "demian_hdzr@hotmail.com";
            //email.To = sis_area.EMAIL;
            email.Subject = "Incidente en: " + sis_area.descripcion;
            email.Descripcion = sol_servicio.DESC_SOL;
            email.Folio = sol_servicio.folio;
            email.Send();

            return Redirect(Request.UrlReferrer.ToString());
        }


        public ActionResult Email()
        {
            dynamic email = new Email("Example");
            email.To = "demian_hdzr@hotmail.com";
            email.FunnyLink = "holi";
            email.Send();

            return View();
        }


        public JsonResult GetNombreExpediente(string search)
        {
            //System.Diagnostics.Debug.WriteLine(search);

            Models.SMDEVEntities9 db = new Models.SMDEVEntities9();
            var dhabientes = (from a in db.DHABIENTES
                              where a.NUMEMP == search
                              select a).FirstOrDefault();

            var resultado = new
            {
                numemp = dhabientes.NUMEMP,
                nombres = dhabientes.NOMBRES,
                apaterno = dhabientes.APATERNO,
                amaterno = dhabientes.AMATERNO,
                sexo = dhabientes.SEXO,
                fnac = dhabientes.FNAC.ToString(),
                reghu = dhabientes.REGHU,
                curp = dhabientes.CURP,
            };

            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult CredencialDigital(string id)
        {
            ///System.Diagnostics.Debug.WriteLine(id);
            ///
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.FVIGENCIA >= fechaDT
                                  where a.NUMAFIL == id
                                  select a).ToList();



                var results1 = new List<DHABIENTES>();

                var foto = "";

                foreach (var dhab in dhabientes)
                {

                    var fnacimiento = dhab.FNAC.ToString();

                    var fnacSp = fnacimiento.ToString(new CultureInfo("es-ES"));

                    var fnacSpDT = DateTime.Parse(fnacSp);

                    if (dhab.foto != null)
                    {
                        //foto = "/Imagenes/dhabientes/" +  dhab.foto;
                        foto = "/SISM-Medico/Imagenes/dhabientes/" + dhab.foto;
                    }
                    else
                    {
                        foto = "http://148.234.143.204/smuanl_web/fotos_dhab/" + dhab.NUMEMP + ".jpg";
                    }
                    var resultado = new DHABIENTES
                    {
                        NUMEMP = dhab.NUMEMP,
                        NUMAFIL = dhab.NUMAFIL,
                        NOMBRES = dhab.NOMBRES,
                        APATERNO = dhab.APATERNO,
                        AMATERNO = dhab.AMATERNO,
                        FNAC = fnacSpDT,
                        foto = foto,
                    };

                    results1.Add(resultado);
                }

                return View(results1);
            }

            //return View();


        }

        public ActionResult FormatoCD(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fechaDT = DateTime.Parse(fecha);

                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.FVIGENCIA >= fechaDT
                                  where a.NUMEMP == id
                                  select a).FirstOrDefault();

                var foto = "";

                if (dhabientes.foto != null)
                {
                    foto = "/SISM-Medico/Imagenes/dhabientes/" +  dhabientes.foto;
                    //foto = "http://148.234.143.203/SISM-Medico/Imagenes/dhabientes/" + dhabientes.foto;
                }
                else
                {
                    using (WebClient webClient = new WebClient())
                    {
                        byte[] data = webClient.DownloadData("http://148.234.143.204/smuanl_web/fotos_dhab/" + dhabientes.NUMEMP + ".jpg");

                        using (MemoryStream mem = new MemoryStream(data))
                        {
                            using (var yourImage = Image.FromStream(mem))
                            {
                                // If you want it as Jpeg
                                String pathFile = Server.MapPath("~/Imagenes/dhabientes/" + dhabientes.NUMEMP);

                                yourImage.Save(pathFile + ".jpg", ImageFormat.Jpeg);

                            }
                        }

                    }


                    foto = "/SISM-Medico/Imagenes/dhabientes/" + dhabientes.NUMEMP + ".jpg";
                    //foto = "http://148.234.143.203/SISM-Medico/Imagenes/dhabientes/" + dhabientes.NUMEMP + ".jpg";
                }

                //System.Diagnostics.Debug.WriteLine(foto);

                var resultado = new DHABIENTES
                {
                    NUMEMP = dhabientes.NUMEMP,
                    NUMAFIL = dhabientes.NUMAFIL,
                    NOMBRES = dhabientes.NOMBRES,
                    APATERNO = dhabientes.APATERNO,
                    AMATERNO = dhabientes.AMATERNO,
                    FNAC = dhabientes.FNAC,
                    foto = foto,
                };


                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es");


                return View(resultado);
            }
        }


        private static Random random = new Random();
        public static string RandomString()
        {
            int length = 16;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public ActionResult FormatoCD2(string id)
        {

            /*var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT2 = DateTime.Parse(fecha2);

            //DERECHOABIENTES DE SERVICIOS MEDICOS
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            //string query = "SELECT DHABIENTES.NUMAFIL as NUMAFIL FROM DHABIENTES INNER JOIN AFILIADOS on DHABIENTES.NUMAFIL = AFILIADOS.num_contrato INNER JOIN DEPENDENCIAS ON AFILIADOS.CVEDEP = DEPENDENCIAS.CVEDEP WHERE DEPENDENCIAS.CVEDEP IN(2316,2317,2308,2204,2303) AND PARIENTE = 1 AND AFILIADOS.MOTBAJA != '02' AND AFILIADOS.MOTBAJA != '04' AND DHABIENTES.FVIGENCIA >= GETDATE() ORDER BY DEPENDENCIAS.DESCR";
            string query = "SELECT DHABIENTES.NUMAFIL as NUMAFIL FROM DHABIENTES INNER JOIN AFILIADOS on DHABIENTES.NUMAFIL = AFILIADOS.num_contrato INNER JOIN DEPENDENCIAS ON AFILIADOS.CVEDEP = DEPENDENCIAS.CVEDEP WHERE DEPENDENCIAS.CVEDEP IN(1310,2628,2610,4101,2630,9999,2620,1304,1204,2659,1102,4108,2127,2658,1110,1207,2652,1403,2201, 2629,4203,2626,2608,1206,2668,2632,1203,2633,1109,2648,2662,1202,2617,2113,2114,2115,2102,2103, 2104,2105,2106,2108,2111,2112,2116,2117,2118,2119,2120,2121,2122,2123,2124,2301,2604,2324,1308, 2322,2306,2321,2312,2325,2601,2318,1201,2326,1306,2323,4107,1103,1305,1602,1501,1302,4102,2607, 2311,2125,2309,2603,4105,1104,4104,2634,1307) AND PARIENTE = 1 AND AFILIADOS.MOTBAJA != '02' AND AFILIADOS.MOTBAJA != '04' AND DHABIENTES.FVIGENCIA >= GETDATE() ORDER BY DEPENDENCIAS.DESCR";
            var result = db2.Database.SqlQuery<DHABIENTES2>(query);
            var res = result.ToList();


            System.Diagnostics.Debug.WriteLine(res);

            
            foreach (var item2 in res)
            {
                var clave = RandomString();

                var dhabientes = (from a in db2.DHABIENTES
                                  where a.credencial_exp == clave
                                  select a)
                                  .FirstOrDefault();


                if (dhabientes == null)
                {
                    var vigentes = (from a in db2.DHABIENTES
                                    where a.NUMAFIL == item2.NUMAFIL
                                    where a.FVIGENCIA >= fechaDT2
                                    where a.BAJA != "02" || a.BAJA != "04"
                                    where a.credencial_exp == null
                                    select a)
                              .ToList();

                    foreach (var vig in vigentes)
                    {
                        db2.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET credencial_exp = '" + clave + "' WHERE NUMEMP = '" + vig.NUMEMP + "'");
                    }

                    //db2.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET credencial_exp = '" + clave + "' WHERE NUMAFIL = '" + item2.NUMAFIL + "' and FVIGENCIA >= '"+ fecha2 + "' and credencial_exp is null");
                }
                else
                {
                    clave = RandomString();

                    var vigentes = (from a in db2.DHABIENTES
                                    where a.NUMAFIL == item2.NUMAFIL
                                    where a.FVIGENCIA >= fechaDT2
                                    where a.BAJA != "02" || a.BAJA != "04"
                                    where a.credencial_exp == null
                                    select a)
                                  .ToList();

                    foreach (var vig in vigentes)
                    {
                        db2.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET credencial_exp = '" + clave + "' WHERE NUMEMP = '" + vig.NUMEMP + "'");
                    }
                }

            }

            System.Diagnostics.Debug.WriteLine("listo");*/

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fechaDT = DateTime.Parse(fecha);

                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.FVIGENCIA >= fechaDT
                                  where a.credencial_exp == id
                                  select a)
                                  .OrderBy(g => g.PARIENTE)
                                  .ToList();

                var results1 = new List<DHABIENTES>();

                foreach (var item in dhabientes)
                {

                    var foto = "";

                    if (item.foto != null)
                    {
                        //foto = "/Imagenes/dhabientes/" + item.foto;
                        foto = "/SISM-Medico/Imagenes/dhabientes/" + item.foto;
                    }
                    else
                    {
                        using (WebClient webClient = new WebClient())
                        {


                            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://148.234.143.204/smuanl_web/fotos_dhab/" + item.NUMEMP + ".jpg");
                            request.Method = "HEAD";

                            bool exists;
                            try
                            {
                                request.GetResponse();
                                exists = true;
                            }
                            catch
                            {
                                exists = false;
                            }


                            if (exists == true)
                            {
                                byte[] data = webClient.DownloadData("http://148.234.143.204/smuanl_web/fotos_dhab/" + item.NUMEMP + ".jpg");


                                using (MemoryStream mem = new MemoryStream(data))
                                {
                                    using (var yourImage = Image.FromStream(mem))
                                    {
                                        // If you want it as Jpeg
                                        String pathFile = Server.MapPath("~/Imagenes/dhabientes/" + item.NUMEMP);

                                        yourImage.Save(pathFile + ".jpg", ImageFormat.Jpeg);

                                    }
                                }

                                //foto = "/Imagenes/dhabientes/" + item.NUMEMP + ".jpg";
                                foto = "/SISM-Medico/Imagenes/dhabientes/" + item.NUMEMP + ".jpg";

                            }
                            else
                            {
                                foto = "sin foto";
                            }

                        }

                    }

                    //System.Diagnostics.Debug.WriteLine(foto);

                    var resultado = new DHABIENTES
                    {
                        NUMEMP = item.NUMEMP,
                        NUMAFIL = item.NUMAFIL,
                        NOMBRES = item.NOMBRES,
                        APATERNO = item.APATERNO,
                        AMATERNO = item.AMATERNO,
                        FNAC = item.FNAC,
                        foto = foto,
                    };

                    results1.Add(resultado);

                }


                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es");


                return View(results1);

            }
        }


        public JsonResult DescargaCD(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT= DateTime.Parse(fecha);

            //System.Diagnostics.Debug.WriteLine(expediente);

            var dhab = (from a in db.DHABIENTES
                        where a.NUMEMP == expediente
                        select new
                        {
                            dhabiente = a.NOMBRES + " " + a.APATERNO + " " + a.AMATERNO,
                            fecha_credencial = a.fecha_credencial,
                        })
                        .FirstOrDefault();

            if(dhab.fecha_credencial == null)
            {
                db.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET fecha_credencial = '" + fecha + "' WHERE NUMEMP = '" + expediente + "' ");
            }

            return new JsonResult { Data = dhab, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult ActualizarCredencial(string numexp, HttpPostedFileBase file, string id)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            //System.Diagnostics.Debug.WriteLine(file);

            if (file != null)
            {
                //System.Diagnostics.Debug.WriteLine(id);
                var ext = Path.GetExtension(file.FileName);
                //System.Diagnostics.Debug.WriteLine(ext);
                //var path = Path.Combine(Server.MapPath("~/Imagenes/dhabientes/"), filename);
                //file.SaveAs(path);

                String filenameFile = id + ext;
                String pathFile = Server.MapPath("~/Imagenes/dhabientes/" + filenameFile);
                file.SaveAs(pathFile);


                db.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET foto = '" + filenameFile + "' WHERE NUMEMP = '" + id + "'");
            }



            //return RedirectToAction("Recetas/" + numexp, "ServiciosMedicos");
            return Redirect(Request.UrlReferrer.ToString());
        }


        public ActionResult PantallaCitasCU()
        {
            return View();
        }

        
        public class ListCitasCU
        {
            public string info { get; set; }
            public string hora_recepcion { get; set; }
        }


        public JsonResult ListaCitasCU()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            //Models.INVENTARIOEntities db3 = new Models.INVENTARIOEntities();
            
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            string queryCita = "select DHABIENTES.NOMBRES+' '+DHABIENTES.APATERNO+' '+DHABIENTES.AMATERNO as title, CITAS.Fecha as start, CITAS.hora_recepcion as hora_recepcion, CITAS.hr_consultorio as hr_consultorio , CITAS.hr_llamado as hr_llamado, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as Medico, MEDICOS.Numero as Numero_Medico, CITAS.NumEmp as NumEmp from CITAS INNER JOIN DHABIENTES ON CITAS.NumEmp=DHABIENTES.NUMEMP INNER JOIN MEDICOS ON CITAS.Medico = MEDICOS.Numero where CITAS.Fecha = '" + fecha + "' AND ip_kiosco like '148.234.218%' AND fecha_kiosco is not null";
            var resultCita = db.Database.SqlQuery<Citas>(queryCita);
            var resCita = resultCita.ToList();

            //System.Diagnostics.Debug.WriteLine(resCita);

            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();


            //COLORES DE CITAS
            //Nuevas en blan

            var results = new List<ListCitasCU>();

            foreach (var item in resCita)
            {
                var color = "";
                var color2 = "";
                var estatus = "";
                var hora = "";
                var consultorio = "";

                var medico_info = (from a in db.MEDICOS
                                   where a.Numero == item.Numero_Medico
                                   //orderby a.medico descending
                                   select a).FirstOrDefault();

                //Para la prueba, revisar si ya tiene una receta terminada

                /*var recetas_exp = (from q in db2.Receta_Exp
                                   where q.Expediente == item.NumEmp
                                   //Si no se le dio, se quedo con estado de kiosko
                                   where q.Medico == item.Numero_Medico
                                   where q.Hora_Rcta != null
                                   where q.Fecha == fechaDT
                                   select q).Count();*/

                //System.Diagnostics.Debug.WriteLine(recetas_exp);

                if (item.hr_consultorio == null) { 
                    if (item.hr_llamado != null)
                    {
                        color = "green";
                        color2 = "#e4f7e4";
                        estatus = "PASAR AL CONSULTORIO " + medico_info.consultorio_mt;
                        hora = "Hora de llamado: <b>" + item.hr_llamado + "</b>";
                    }
                    else
                    {
                        if (item.hora_recepcion != null)
                        {
                            color = "#FBC43D";
                            color2 = "#fff7e3";
                            estatus = "EN ESPERA";
                            hora = "Hora de recepción:  <b>" + item.hora_recepcion + "</b>";
                        }
                        else
                        {
                            color = "#b0b0b0";
                            color2 = "";
                            estatus = "SIN CONRFIRMAR";
                            hora = "Sin hora";
                        }
                    }

                    var result = new ListCitasCU
                    {
                        info = item.title + "*" + item.Medico + "*" + hora + "*" + color + "*" + color2 + "*" + estatus,
                        hora_recepcion = item.hora_recepcion,

                    };

                    results.Add(result);
                }

                
            }

            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }




        public ActionResult FormatoCD3(string id)
        {

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fechaDT = DateTime.Parse(fecha);

                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                var dhabientes = (from a in db.DHABIENTES
                                  join afi in db.AFILIADOS on a.NUMAFIL equals afi.num_contrato into afiX
                                  from afiIn in afiX.DefaultIfEmpty()
                                  where a.FVIGENCIA >= fechaDT
                                  where a.credencial_exp == id
                                  select a)
                                  .OrderBy(g => g.PARIENTE)
                                  .ToList();

                var results1 = new List<CredencialDigital>();

                foreach (var item in dhabientes)
                {

                    var foto = "";

                    if (item.foto != null)
                    {
                        foto = "/SISM-Medico/Imagenes/dhabientes/" + item.foto;
                        //foto = "http://sism.uanl.mx/SISM-Medico/Imagenes/dhabientes/" + item.foto;
                    }
                    else
                    {
                        using (WebClient webClient = new WebClient())
                        {


                            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://148.234.143.204/smuanl_web/fotos_dhab/" + item.NUMEMP + ".jpg");
                            request.Method = "HEAD";

                            bool exists;
                            try
                            {
                                request.GetResponse();
                                exists = true;
                            }
                            catch
                            {
                                exists = false;
                            }


                            if (exists == true)
                            {
                                byte[] data = webClient.DownloadData("http://148.234.143.204/smuanl_web/fotos_dhab/" + item.NUMEMP + ".jpg");


                                using (MemoryStream mem = new MemoryStream(data))
                                {
                                    using (var yourImage = Image.FromStream(mem))
                                    {
                                        // If you want it as Jpeg
                                        String pathFile = Server.MapPath("~/Imagenes/dhabientes/" + item.NUMEMP);

                                        yourImage.Save(pathFile + ".jpg", ImageFormat.Jpeg);

                                    }
                                }

                                foto = "/SISM-Medico/Imagenes/dhabientes/" + item.NUMEMP + ".jpg";
                                //foto = "http://sism.uanl.mx/SISM-Medico/Imagenes/dhabientes/" + item.NUMEMP + ".jpg";

                            }
                            else
                            {
                                foto = "sin foto";
                            }

                        }

                    }

                    //System.Diagnostics.Debug.WriteLine(foto);

                    var afiliadoTipo = (from a in db.AFILIADOS
                                      where a.num_contrato == item.NUMAFIL
                                      select a).FirstOrDefault();

                    var fondo = "";

                    if(afiliadoTipo.TIPOAFIL == "01")
                    {
                        fondo = "/SISM-Medico/Imagenes/dhabientes/formato-credencial-2.png";
                    }
                    else
                    {
                        if (afiliadoTipo.TIPOAFIL == "04")
                        {
                            if (item.PARIENTE == "1")
                            {
                                fondo = "/SISM-Medico/Imagenes/dhabientes/formato-credencial-3.png";
                            }
                            else
                            {
                                fondo = "/SISM-Medico/Imagenes/dhabientes/formato-credencial-2.png";
                            }
                        }
                        else
                        {
                            if (afiliadoTipo.TIPOAFIL == "06")
                            {
                                fondo = "/SISM-Medico/Imagenes/dhabientes/formato-credencial-4.png";
                            }
                            else
                            {
                                fondo = "/SISM-Medico/Imagenes/dhabientes/formato-credencial-2.png";
                            }
                        }
                    }

                    var resultado = new CredencialDigital
                    {
                        NUMEMP = item.NUMEMP,
                        NUMAFIL = item.NUMAFIL,
                        NOMBRES = item.NOMBRES,
                        APATERNO = item.APATERNO,
                        AMATERNO = item.AMATERNO,
                        FNAC = item.FNAC,
                        foto = foto,
                        fondo = fondo,
                        //TIPOAFIL = afiliadoTipo.TIPOAFIL
                    };

                    results1.Add(resultado);

                }


                //CredencialDigital ordint = new CredencialDigital();
                //ordint.DHabiente = results1;


                Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("es");


                return View(results1);

            }
        }


    }
}