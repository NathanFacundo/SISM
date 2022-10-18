using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class ForaneosController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        // GET: Foraneos

        public static string GetLocalIPAddress()
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();

            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();
        }


        public ActionResult Index()
        {
            if (User.IsInRole("Foraneos"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult HojaFrontal(string id)
        {
            if (User.IsInRole("Foraneos"))
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public ActionResult HistorialPadecimientos(string id)
        {
            if (User.IsInRole("Foraneos"))
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
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
                                      join diagnosticos1 in db3.DIAGNOSTICOS on q.diagnostico equals diagnosticos1.Clave
                                      join diagnosticos2 in db3.DIAGNOSTICOS on q.diagnostico2 equals diagnosticos2.Clave
                                      join diagnosticos3 in db3.DIAGNOSTICOS on q.diagnostico3 equals diagnosticos3.Clave
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
                                          diagnostico = diagnosticos1.DesCorta + " + " + diagnosticos2.DesCorta + " + " + diagnosticos3.DesCorta,
                                          edd_anio = q.edd_anio,
                                          edd_mes = q.edd_mes,
                                          peso_r = q.peso_r,
                                          talla_r = q.talla_r,
                                          ta = q.ta1 + "-" + q.ta2,
                                          temp_r = q.temp_r,
                                          fresp = q.fresp,
                                          fecha = q.fecha,
                                      })
                                        .OrderByDescending(g => g.fecha)
                                        .ToList();


                    ViewData["expediente"] = expediente;
                    string json = JsonConvert.SerializeObject(expediente);
                    string path = Server.MapPath("~/json/historial_padecimientos/");
                    System.IO.File.WriteAllText(path + "historial_expediente_" + id + ".json", json);

                    return View(dhabientes);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public ActionResult SOAP(string id)
        {
            if (User.IsInRole("Foraneos"))
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    //return RedirectToAction("Index", "Manage");
                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();

                    Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();
                    /*var diagnosticos = (from a in db2.DIAGNOSTICOS
                                        where a.DescCompleta != null
                                        where a.Clave != null
                                        select a).ToList();*/

                    var diagnosticos = (from q in db2.DIAGNOSTICOS
                                        where q.DescCompleta != null
                                        where q.Clave != null
                                        //where q.Medico == "30001"
                                        select new
                                        {
                                            CheckBox = "",
                                            Clave = q.Clave,
                                            DescCompleta = q.DescCompleta,
                                            DesCorta = q.DesCorta
                                            //expexp = q.Expediente
                                        })
                            .ToList();

                    string json = JsonConvert.SerializeObject(diagnosticos);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                    return View(dhabientes);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }



        [HttpPost]
        public ActionResult GuardarSOAP(expediente model)
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var ip_realiza = GetLocalIPAddress();

            Models.SMDEVEntities24 db = new Models.SMDEVEntities24();
            var expCount = (from a in db.expediente
                            where a.numemp == model.numemp
                            where a.medico == model.medico
                            where a.fecha == fecha_correcta
                            //where a.hora_termino == null
                            select a).Count();

            if (expCount > 0)
            {
                db.Database.ExecuteSqlCommand("UPDATE expediente SET numemp = '" + model.numemp + "', medico = '" + model.medico + "', diagnostico = '" + model.diagnostico + "', diagnostico2 = '" + model.diagnostico2 + "', diagnostico3 = '" + model.diagnostico3 + "', tipconsulta = '" + model.tipconsulta + "', tipconsulta2 = '" + model.tipconsulta2 + "', tipconsulta3 = '" + model.tipconsulta3 + "', s_exp = '" + model.s_exp + "', o_exp = '" + model.o_exp + "', p_exp = '" + model.p_exp + "', fecha = '" + fecha + "', peso_r = '" + model.peso_r + "', talla_r = '" + model.talla_r + "', temp_r = '" + model.temp_r + "', fresp = '" + model.fresp + "', fcard = '" + model.fcard + "', ta1 = '" + model.ta1 + "', ta2 = '" + model.ta2 + "', dstx = '" + model.dstx + "', ip_realiza = '" + ip_realiza + "' WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and fecha = '" + fecha + "'");
            }
            else
            {
                db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, diagnostico, diagnostico2, diagnostico3, tipconsulta, tipconsulta2, tipconsulta3,  s_exp, o_exp, p_exp, fecha, peso_r, talla_r, temp_r, fresp, fcard, ta1, ta2, dstx, ip_realiza, consultadistancia) VALUES('" + model.numemp + "', '" + model.medico + "', '" + model.diagnostico + "', '" + model.diagnostico2 + "', '" + model.diagnostico3 + "', '" + model.tipconsulta + "', '" + model.tipconsulta2 + "', '" + model.tipconsulta3 + "', '" + model.s_exp + "', '" + model.o_exp + "', '" + model.p_exp + "', '" + fecha + "', '" + model.peso_r + "', '" + model.talla_r + "', '" + model.temp_r + "', '" + model.fresp + "', '" + model.fcard + "', '" + model.ta1 + "', '" + model.ta2 + "', '" + model.dstx + "', '" + ip_realiza + "', '0')");
            }

            if (model.p_exp == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                var fecha_termina = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

                db.Database.ExecuteSqlCommand("UPDATE expediente SET hora_termino = '" + fecha_termina + "' WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and fecha = '" + fecha + "'");

                TempData["message_success"] = "Consulta terminada con éxito";

                return RedirectToAction("SOAP/" + model.numemp, "Foraneos");
            }

            //return Redirect(Request.UrlReferrer.ToString());
        }


    }
}