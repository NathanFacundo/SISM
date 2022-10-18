using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Postal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;


        public class BuscarDhabList
        {
            public string numemp { get; set; }
            public string numexp { get; set; }
            public string paciente { get; set; }
            public string edad { get; set; }
            public string sexo { get; set; }
            public string info { get; set; }
            public string fecha { get; set; }
            public string medico { get; set; }

        }

        public ActionResult Medico()
        {
            /*dynamic email = new Email("Example");
            email.To = "demian_hdzr@hotmail.com";
            //email.To = sis_area.emailcoordmed;
            email.Subject = "Comunicate - Actualizacion de seguimiento en: " + "prueba sistemas";
            email.Descripcion = "prueba sistemas";
            email.Folio = "prueba sistemas";
            email.Nota = "prueba sistemas";
            email.Seguimiento = "prueba sistemas";
            email.Send();*/

            /*
            Models.SMDEVEntities33 db3 = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("2021-01-01T00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            var fecha2 = DateTime.Now.ToString("2021-12-31T23:59:59.000");
            var fechaDT2 = DateTime.Parse(fecha2);

            var expediente = (from q in db3.expediente
                              //where q.medico.Substring(0, 2) == "21" 
                              where q.medico.Substring(0, 2) == "15"
                              join dhabientes in db3.DHABIENTES on q.numemp equals dhabientes.NUMEMP into dhabientesX
                              from dhabientesIn in dhabientesX.DefaultIfEmpty()
                              where q.fecha >= fechaDT
                              where q.fecha <= fechaDT2
                              where q.hora_termino != null
                              select new
                              {
                                  numemp = q.numemp,
                              })
                              .GroupBy(p => new
                              {
                                  p.numemp,
                              })
                              .Select(g => new
                              {
                                  numemp = g.Key.numemp,
                              })
                              .ToList();

            var results1 = new List<BuscarDhabList>();
            
            foreach (var item in expediente)
            {

                var dhabiente = (from q in db3.DHABIENTES
                                 where q.NUMEMP == item.numemp
                                 select q).
                                 FirstOrDefault();

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


                if(edad <= 17) { 

                    var resultado = new BuscarDhabList
                    {
                        numexp = item.numemp,
                        //medico = item.medico,
                        edad = edad + " años con " + meses,
                    };

                    results1.Add(resultado);

                }

            }
            

            System.Diagnostics.Debug.WriteLine(results1);

            */


            return View();
        }

        public ActionResult EncuestaCovid()
        {
            return View();
        }

        public class Primera
        {
            public int uno { get; set; }
            public int dos { get; set; }
            public int tres { get; set; }
            public int cuatro { get; set; }
            public int cinco { get; set; }
            public int seis { get; set; }
            public int siete { get; set; }
            public int ocho { get; set; }
            public int nueve { get; set; }
            public int diez { get; set; }
            public int once { get; set; }
            public int doce { get; set; }
            public int trece { get; set; }
            public int catorce { get; set; }
            public int quince { get; set; }
            public int dieciseis { get; set; }
            public int dieciciete { get; set; }
            public int dieciocho { get; set; }
            public int diecinueve { get; set; }
            public int veinte { get; set; }
            public int dos_si { get; set; }
            public int dos_no { get; set; }
            public int dos_null { get; set; }
            public int count_all { get; set; }
            public int tres_casa { get; set; }
            public int tres_traslado { get; set; }
            public int tres_null { get; set; }

            public int cuatro_si { get; set; }
            public int cuatro_no { get; set; }
            public int cuatro_null { get; set; }

            public int cinco_si { get; set; }
            public int cinco_no { get; set; }
            public int cinco_null { get; set; }

            public int seis_si { get; set; }
            public int seis_no { get; set; }
            public int seis_null { get; set; }

            public int siete_si { get; set; }
            public int siete_no { get; set; }
            public int siete_null { get; set; }

            public int ocho_si { get; set; }
            public int ocho_no { get; set; }
            public int ocho_null { get; set; }

            public int nueve_si { get; set; }
            public int nueve_no { get; set; }
            public int nueve_null { get; set; }

            public int diez_si_si { get; set; }
            public int diez_si_no { get; set; }
            public int diez_no_si { get; set; }
            public int diez_no_no { get; set; }

            public int once_si { get; set; }
            public int once_no { get; set; }

            public int si_count { get; set; }
            public int no_count { get; set; }
        }

        public ActionResult PrimerPregunta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            var encuestas = (from a in db.encuesta_covid
                             orderby a.medico descending
                             select a).ToList();
            //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
            //return View(db.DHABIENTES.ToList());
            //return View(db.encuesta_covid.ToList());
            return View(encuestas);
        }


        public ActionResult GetDataPrimera()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities2 db = new Models.laravel_sqlEntities2();
            int uno = db.encuesta_covid.Where(x => x.dias_prueba == 1).Count();
            int dos = db.encuesta_covid.Where(x => x.dias_prueba == 2).Count();
            int tres = db.encuesta_covid.Where(x => x.dias_prueba == 3).Count();
            int cuatro = db.encuesta_covid.Where(x => x.dias_prueba == 4).Count();
            int cinco = db.encuesta_covid.Where(x => x.dias_prueba == 5).Count();
            int seis = db.encuesta_covid.Where(x => x.dias_prueba == 6).Count();
            int siete = db.encuesta_covid.Where(x => x.dias_prueba == 7).Count();
            int ocho = db.encuesta_covid.Where(x => x.dias_prueba == 8).Count();
            int nueve = db.encuesta_covid.Where(x => x.dias_prueba == 9).Count();
            int diez = db.encuesta_covid.Where(x => x.dias_prueba == 10).Count();
            int once = db.encuesta_covid.Where(x => x.dias_prueba == 11).Count();
            int doce = db.encuesta_covid.Where(x => x.dias_prueba == 12).Count();
            int trece = db.encuesta_covid.Where(x => x.dias_prueba == 13).Count();
            int catorce = db.encuesta_covid.Where(x => x.dias_prueba == 14).Count();
            int quince = db.encuesta_covid.Where(x => x.dias_prueba == 15).Count();
            int dieciseis = db.encuesta_covid.Where(x => x.dias_prueba == 16).Count();
            int dieciciete = db.encuesta_covid.Where(x => x.dias_prueba == 17).Count();
            int dieciocho = db.encuesta_covid.Where(x => x.dias_prueba == 18).Count();
            int diecinueve = db.encuesta_covid.Where(x => x.dias_prueba == 19).Count();
            int veinte = db.encuesta_covid.Where(x => x.dias_prueba == 20).Count();
            System.Diagnostics.Debug.WriteLine(veinte);
            int count_all = db.encuesta_covid.Count();
            Primera primera = new Primera();
            //primera.uno = (uno * 100) / count_all;
            primera.uno = uno;
            //primera.dos = (dos * 100) / count_all;
            primera.dos = dos;
            //primera.tres = (tres * 100) / count_all;
            primera.tres = tres;
            //primera.cuatro = (cuatro * 100) / count_all;
            primera.cuatro = cuatro;
            //primera.cinco = (cinco * 100) / count_all;
            primera.cinco = cinco;
            //primera.seis = (seis * 100) / count_all;
            primera.seis = seis;
            //primera.siete = (siete * 100) / count_all;
            primera.siete = siete;
            //primera.ocho = (ocho * 100) / count_all;
            primera.ocho = ocho;
            //primera.nueve = (nueve * 100) / count_all;
            primera.nueve = nueve;
            //primera.diez = (diez * 100) / count_all;
            primera.diez = diez;
            //primera.once = (once * 100) / count_all;
            primera.once = once;
            //primera.doce = (doce * 100) / count_all;
            primera.doce = doce;
            //primera.trece = (trece * 100) / count_all;
            primera.trece = trece;
            //primera.catorce = (catorce * 100) / count_all;
            primera.catorce = catorce;
            //primera.quince = (quince * 100) / count_all;
            primera.quince = quince;
            //primera.dieciseis = (dieciseis * 100) / count_all;
            primera.dieciseis = dieciseis;
            //primera.dieciciete = (dieciciete * 100) / count_all;
            primera.dieciciete = dieciciete;
            //primera.dieciocho = (dieciocho * 100) / count_all;
            primera.dieciocho = dieciocho;
            //primera.diecinueve = (diecinueve * 100) / count_all;
            primera.diecinueve = diecinueve;
            //primera.veinte = (veinte * 100) / count_all;
            primera.veinte = veinte;

            primera.count_all = count_all;

            //return View(db.DHABIENTES.ToList());
            return Json(primera, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SegundaPregunta()
        {
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2(); laravel_sqlEntities
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            var encuestas = (from a in db.encuesta_covid
                             orderby a.medico descending
                             select a).ToList();
            return View(encuestas);
            //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
            //return View(db.DHABIENTES.ToList());
            //return View(db.encuesta_covid.ToList());

        }


        public ActionResult GetDataSegunda()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities2 db = new Models.laravel_sqlEntities2();
            int dos_si = db.encuesta_covid.Where(x => x.gpo_riesgo == "1").Count();
            int dos_no = db.encuesta_covid.Where(x => x.gpo_riesgo == "2").Count();
            int dos_null = db.encuesta_covid.Where(x => x.gpo_riesgo == null).Count();
            int count_all = db.encuesta_covid.Count();

            //System.Diagnostics.Debug.WriteLine(dos_si);

            Primera segunda = new Primera();
            segunda.si_count = dos_si;
            segunda.no_count = dos_no;
            segunda.dos_si = (dos_si * 100) / count_all;
            segunda.dos_no = (dos_no * 100) / count_all;
            segunda.dos_null = (dos_null * 100) / count_all;
            segunda.count_all = count_all;
            //return View(db.DHABIENTES.ToList());
            return Json(segunda, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TerceraPregunta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
            //return View(db.DHABIENTES.ToList());
            var encuestas = (from a in db.encuesta_covid
                             orderby a.medico descending
                             select a).ToList();
            return View(encuestas);

            //return View(db.encuesta_covid.ToList());

        }


        public ActionResult GetDataTercera()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities2 db = new Models.laravel_sqlEntities2();
            int tres_casa = db.encuesta_covid.Where(x => x.trabajo_ubica == "1").Count();
            int tres_traslado = db.encuesta_covid.Where(x => x.trabajo_ubica == "2").Count();
            int tres_null = db.encuesta_covid.Where(x => x.trabajo_ubica == null).Count();
            int count_all = db.encuesta_covid.Count();

            //System.Diagnostics.Debug.WriteLine(dos_si);

            Primera tercera = new Primera();
            tercera.si_count = tres_casa;
            tercera.no_count = tres_traslado;
            tercera.tres_casa = (tres_casa * 100) / count_all;
            tercera.tres_traslado = (tres_traslado * 100) / count_all;
            tercera.tres_null = (tres_null * 100) / count_all;
            tercera.count_all = count_all;

            //return View(db.DHABIENTES.ToList());
            return Json(tercera, JsonRequestBehavior.AllowGet);
        }



        public ActionResult CuartaPregunta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
            //return View(db.DHABIENTES.ToList());
            var encuestas = (from a in db.encuesta_covid
                             orderby a.medico descending
                             select a).ToList();
            return View(encuestas);

            //return View(db.encuesta_covid.ToList());
        }

        public ActionResult GetDataCuarta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities2 db = new Models.laravel_sqlEntities2();
            int cuatro_si = db.encuesta_covid.Where(x => x.aislamiento == "1").Count();
            int cuatro_no = db.encuesta_covid.Where(x => x.aislamiento == "2").Count();
            int cuatro_null = db.encuesta_covid.Where(x => x.aislamiento == null).Count();
            int count_all = db.encuesta_covid.Count();

            //System.Diagnostics.Debug.WriteLine(dos_si);

            Primera cuarta = new Primera();
            cuarta.si_count = cuatro_si;
            cuarta.no_count = cuatro_no;
            cuarta.cuatro_si = (cuatro_si * 100) / count_all;
            cuarta.cuatro_no = (cuatro_no * 100) / count_all;
            cuarta.cuatro_null = (cuatro_null * 100) / count_all;
            cuarta.count_all = count_all;

            //return View(db.DHABIENTES.ToList());
            return Json(cuarta, JsonRequestBehavior.AllowGet);
        }


        public ActionResult QuintaPregunta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
            //return View(db.DHABIENTES.ToList());
            var encuestas = (from a in db.encuesta_covid
                             orderby a.medico descending
                             select a).ToList();
            return View(encuestas);


            //return View(db.encuesta_covid.ToList());
        }

        public ActionResult GetDataQuinta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities2 db = new Models.laravel_sqlEntities2();
            int cinco_si = db.encuesta_covid.Where(x => x.int_requerido == "1").Count();
            int cinco_no = db.encuesta_covid.Where(x => x.int_requerido == "2").Count();
            int cinco_null = db.encuesta_covid.Where(x => x.int_requerido == null).Count();
            int count_all = db.encuesta_covid.Count();

            //System.Diagnostics.Debug.WriteLine(dos_si);

            Primera quinta = new Primera();
            quinta.si_count = cinco_si;
            quinta.no_count = cinco_no;
            quinta.cinco_si = (cinco_si * 100) / count_all;
            quinta.cinco_no = (cinco_no * 100) / count_all;
            quinta.cinco_null = (cinco_null * 100) / count_all;
            quinta.count_all = count_all;

            //return View(db.DHABIENTES.ToList());
            return Json(quinta, JsonRequestBehavior.AllowGet);
        }



        public ActionResult SextaPregunta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
            //return View(db.DHABIENTES.ToList());
            var encuestas = (from a in db.encuesta_covid
                             orderby a.medico descending
                             select a).ToList();
            return View(encuestas);

            //return View(db.encuesta_covid.ToList());
        }

        public ActionResult GetDataSexta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities2 db = new Models.laravel_sqlEntities2();
            int seis_si = db.encuesta_covid.Where(x => x.oxi_requerido == "1").Count();
            int seis_no = db.encuesta_covid.Where(x => x.oxi_requerido == "2").Count();
            int seis_null = db.encuesta_covid.Where(x => x.oxi_requerido == null).Count();
            int count_all = db.encuesta_covid.Count();

            //System.Diagnostics.Debug.WriteLine(dos_si);

            Primera sexta = new Primera();
            sexta.si_count = seis_si;
            sexta.no_count = seis_no;
            sexta.seis_si = (seis_si * 100) / count_all;
            sexta.seis_no = (seis_no * 100) / count_all;
            sexta.seis_null = (seis_null * 100) / count_all;
            sexta.count_all = count_all;

            //return View(db.DHABIENTES.ToList());
            return Json(sexta, JsonRequestBehavior.AllowGet);
        }



        public ActionResult SeptimaPregunta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
            //return View(db.DHABIENTES.ToList());
            var encuestas = (from a in db.encuesta_covid
                             orderby a.medico descending
                             select a).ToList();
            return View(encuestas);

            //return View(db.encuesta_covid.ToList());
        }

        public ActionResult GetDatSeptima()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities2 db = new Models.laravel_sqlEntities2();
            int siete_si = db.encuesta_covid.Where(x => x.vent_asistida == "1").Count();
            int siete_no = db.encuesta_covid.Where(x => x.vent_asistida == "2").Count();
            int siete_null = db.encuesta_covid.Where(x => x.vent_asistida == null).Count();
            int count_all = db.encuesta_covid.Count();

            //System.Diagnostics.Debug.WriteLine(dos_si);

            Primera septima = new Primera();
            septima.si_count = siete_si;
            septima.no_count = siete_no;
            septima.siete_si = (siete_si * 100) / count_all;
            septima.siete_no = (siete_no * 100) / count_all;
            septima.siete_null = (siete_null * 100) / count_all;
            septima.count_all = count_all;

            //return View(db.DHABIENTES.ToList());
            return Json(septima, JsonRequestBehavior.AllowGet);
        }


        public ActionResult OctavaPregunta()
        {
            if (User.Identity.GetUserId() == "3c54a289-bafc-4918-8a06-d8046984f132" || User.Identity.GetUserId() != "b73f014b-fd6e-4524-9f94-e682cc2c35b8" || User.Identity.GetUserId() != "6dac2f91-88cb-4b85-95e8-8f9cd47f70a1")
            {
                Models.SMDEVEntities db = new Models.SMDEVEntities();
                //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
                //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
                //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
                //return View(db.DHABIENTES.ToList());
                var encuestas = (from a in db.encuesta_covid
                                 orderby a.medico descending
                                 select a).ToList();
                return View(encuestas);
            }
            else
            {
                return RedirectToAction("Index");
            }

            //return View(db.encuesta_covid.ToList());
        }

        public ActionResult GetDataOctava()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities2 db = new Models.laravel_sqlEntities2();
            int ocho_si = db.encuesta_covid.Where(x => x.alta == "1").Count();
            int ocho_no = db.encuesta_covid.Where(x => x.alta == "2").Count();
            int ocho_null = db.encuesta_covid.Where(x => x.alta == null).Count();
            int count_all = db.encuesta_covid.Count();

            //System.Diagnostics.Debug.WriteLine(dos_si);

            Primera octava = new Primera();
            octava.si_count = ocho_si;
            octava.no_count = ocho_no;
            octava.ocho_si = (ocho_si * 100) / count_all;
            octava.ocho_no = (ocho_no * 100) / count_all;
            octava.ocho_null = (ocho_null * 100) / count_all;
            octava.count_all = count_all;

            //return View(db.DHABIENTES.ToList());
            return Json(octava, JsonRequestBehavior.AllowGet);
        }


        public ActionResult NovenaPregunta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
            //return View(db.DHABIENTES.ToList());
            var encuestas = (from a in db.encuesta_covid
                             orderby a.medico descending
                             select a).ToList();
            return View(encuestas);

            //return View(db.encuesta_covid.ToList());
        }

        public ActionResult GetDataNovena()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities2 db = new Models.laravel_sqlEntities2();
            int nueve_si = db.encuesta_covid.Where(x => x.fam_sintomas == "1").Count();
            int nueve_no = db.encuesta_covid.Where(x => x.fam_sintomas == "2").Count();

            //Tiene familiar con sintoma y es derecho habiente
            int diez_si_si = db.encuesta_covid.Where(x => x.derechohabiente == "1").Where(x => x.fam_sintomas == "1").Count();
            //Tiene familiar con sintoma y no es derecho habiente
            int diez_no_si = db.encuesta_covid.Where(x => x.derechohabiente == "2").Where(x => x.fam_sintomas == "1").Count();
            //No tiene familiar con sintoma y no es derecho habiente
            int diez_no_no = db.encuesta_covid.Where(x => x.derechohabiente == "2").Where(x => x.fam_sintomas == "2").Count();
            //No tiene familiar con sintoma y es derecho habiente
            int diez_si_no = db.encuesta_covid.Where(x => x.derechohabiente == "1").Where(x => x.fam_sintomas == "2").Count();


            int nueve_null = db.encuesta_covid.Where(x => x.fam_sintomas == null).Count();
            int diez_null = db.encuesta_covid.Where(x => x.derechohabiente == null).Count();
            int count_all = db.encuesta_covid.Count();

            //System.Diagnostics.Debug.WriteLine(dos_si);

            Primera novena = new Primera();
            novena.nueve_si = (nueve_si * 100) / count_all;
            novena.nueve_no = (nueve_no * 100) / count_all;
            novena.nueve_null = (nueve_null * 100) / count_all;


            novena.diez_si_si = (diez_si_si * 100) / count_all;
            novena.diez_si_no = (diez_si_no * 100) / count_all;
            novena.diez_no_si = (diez_no_si * 100) / count_all;
            novena.diez_no_no = (diez_no_no * 100) / count_all;


            novena.count_all = count_all;



            //return View(db.DHABIENTES.ToList());
            return Json(novena, JsonRequestBehavior.AllowGet);
        }


        public ActionResult OnceavaPregunta()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
            //return View(db.DHABIENTES.ToList());
            var encuestas = (from a in db.encuesta_covid
                             orderby a.medico descending
                             select a).ToList();
            return View(encuestas);

            //return View(db.encuesta_covid.ToList());
        }

        public ActionResult GetDataOnceava()
        {
            Models.SMDEVEntities db = new Models.SMDEVEntities();
            //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            //Models.laravel_sqlEntities2 db = new Models.laravel_sqlEntities2();
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha__actual_correcta = DateTime.Parse(fecha_actual);
            //Pacientes que aún no vence su incapacidad
            int once_si = db.encuesta_covid.Where(x => x.inc_vence > fecha__actual_correcta).Count();
            //Pacientes que ya no estan incapacitados
            int once_no = db.encuesta_covid.Where(x => x.inc_vence <= fecha__actual_correcta).Count();
            int count_all = db.encuesta_covid.Count();

            //System.Diagnostics.Debug.WriteLine(dos_si);

            Primera once = new Primera();
            once.once_si = (once_si * 100) / count_all;
            once.once_no = (once_no * 100) / count_all;
            once.count_all = count_all;
            //return View(db.DHABIENTES.ToList());
            return Json(once, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Comentarios()
        {
            if (User.Identity.GetUserId() == "3c54a289-bafc-4918-8a06-d8046984f132" || User.Identity.GetUserId() != "b73f014b-fd6e-4524-9f94-e682cc2c35b8" || User.Identity.GetUserId() != "6dac2f91-88cb-4b85-95e8-8f9cd47f70a1")
            {
                Models.SMDEVEntities db = new Models.SMDEVEntities();
                //Models.SMDEVEntities2 db = new Models.SMDEVEntities2();
                //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
                //Models.laravel_sqlEntities1 db = new Models.laravel_sqlEntities1();
                //return View(db.DHABIENTES.ToList());

                return View(db.encuesta_covid.ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        public ActionResult SegundaEncuesta()
        {
            return View();
        }

        public JsonResult BuscarExpSegundaEnc(string search)
        {
            Models.SMDEVEntities9 db2 = new Models.SMDEVEntities9();
            var info_paciente = (from a in db2.DHABIENTES
                                 where a.NUMEMP == search
                                 select a).FirstOrDefault();

            var resultado = new { nombres = info_paciente.NOMBRES, fecha_nacimiento = info_paciente.FNAC.ToString() };
            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult SaveSegundaEncuesta(encuesta_covid_segunda model)
        {
            try
            {
                encuesta_covid_segunda encuesta_segunda = new encuesta_covid_segunda();
                encuesta_segunda.nombre = model.nombre;
                encuesta_segunda.edad = model.edad;
                encuesta_segunda.expediente = model.expediente;
                encuesta_segunda.antecedentes_medicos = model.antecedentes_medicos;
                encuesta_segunda.fiebre = model.fiebre;
                encuesta_segunda.tos = model.tos;
                encuesta_segunda.anosmia = model.anosmia;
                encuesta_segunda.diarrea = model.diarrea;
                encuesta_segunda.mialgias_atralgias = model.mialgias_atralgias;
                encuesta_segunda.hta_cardivascular = model.hta_cardivascular;
                encuesta_segunda.diabetes_mellitus = model.diabetes_mellitus;
                encuesta_segunda.tabaquismo = model.tabaquismo;
                encuesta_segunda.enf_inmunologica = model.enf_inmunologica;
                encuesta_segunda.enf_reumatologia = model.enf_reumatologia;
                encuesta_segunda.enf_neoplasica = model.enf_neoplasica;
                encuesta_segunda.irc = model.irc;
                encuesta_segunda.edad_mayo = model.edad_mayo;
                encuesta_segunda.fr = model.fr;
                encuesta_segunda.disnea = model.disnea;
                encuesta_segunda.saturacion_oxigeno = model.saturacion_oxigeno;
                encuesta_segunda.hipotension = model.hipotension;
                encuesta_segunda.alteracion_estado = model.alteracion_estado;
                encuesta_segunda.alteracion_pulmonares = model.alteracion_pulmonares;
                encuesta_segunda.notas = model.notas;
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                var fecha_correcta = DateTime.Parse(fecha);
                encuesta_segunda.fecha = fecha_correcta;

                Models.SMDEVEntities10 db = new Models.SMDEVEntities10();
                db.encuesta_covid_segunda.Add(encuesta_segunda);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //TempData["message"] = "Información agregada con éxito";
            TempData["nombre"] = model.nombre;
            TempData["edad"] = model.edad;
            TempData["expediente"] = model.expediente;
            TempData["antecedentes_medicos"] = model.antecedentes_medicos;
            if (model.fiebre == 1) { TempData["fiebre"] = "Si"; }
            if (model.tos == 1) { TempData["tos"] = "Si"; }
            if (model.anosmia == 1) { TempData["anosmia"] = "Si"; }
            if (model.diarrea == 1) { TempData["diarrea"] = "Si"; }
            if (model.mialgias_atralgias == 1) { TempData["mialgias_atralgias"] = "Si"; }
            if (model.hta_cardivascular == 1) { TempData["hta_cardivascular"] = "Si"; }
            if (model.diabetes_mellitus == 1) { TempData["diabetes_mellitus"] = "Si"; }
            if (model.tabaquismo == 1) { TempData["tabaquismo"] = "Si"; }
            if (model.enf_inmunologica == 1) { TempData["enf_inmunologica"] = "Si"; }
            if (model.enf_reumatologia == 1) { TempData["enf_reumatologia"] = "Si"; }
            if (model.enf_neoplasica == 1) { TempData["enf_neoplasica"] = "Si"; }
            if (model.irc == 1) { TempData["irc"] = "Si"; }
            if (model.edad_mayo == 1) { TempData["edad_mayo"] = "Si"; }
            if (model.fr == 1) { TempData["fr"] = "Si"; }
            if (model.disnea == 1) { TempData["disnea"] = "Si"; }
            if (model.saturacion_oxigeno == 1) { TempData["saturacion_oxigeno"] = "Si"; }
            if (model.hipotension == 1) { TempData["hipotension"] = "Si"; }
            if (model.alteracion_estado == 1) { TempData["alteracion_estado"] = "Si"; }
            if (model.alteracion_pulmonares == 1) { TempData["alteracion_pulmonares"] = "Si"; }
            TempData["notas"] = model.notas;


            var fecha2 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha_correcta2 = DateTime.Parse(fecha2);
            TempData["fecha"] = fecha_correcta2;

            return RedirectToAction("../Manage/SegundaEncuesta");
        }

        public ActionResult Reportes()
        {
            if (User.IsInRole("Reportes"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult verDhabientes()
        {
            //Models.SMDEVEntities9 db = new Models.SMDEVEntities9();
            //List<DHABIENTES> list = db.DHABIENTES.ToList();
            //return View(db.DHABIENTES.Where(x=>x.NUMEMP.StartsWith(search) || search == null).ToList().ToPagedList(i ?? 1,10));
            Models.SMDEVEntities15 db3 = new Models.SMDEVEntities15();
            return View(db3.NUEVOLEON.Where(x => x.clave_col != "00000").ToList());
        }

        public JsonResult verDhabientesInfo(string search)
        {
            //Models.SMDEVEntities14 db2 = new Models.SMDEVEntities14();
            //Models.SMDEVEntities15 db3 = new Models.SMDEVEntities15();
            Models.SMDEVEntities16 db = new Models.SMDEVEntities16();
            var info_afiliados = (from a in db.AFILIADOS
                                  where a.NUMEMP == search
                                  select a).FirstOrDefault();

            var colonia = (from a in db.NUEVOLEON
                           where a.clave_col == info_afiliados.COLONIA
                           select a).FirstOrDefault();

            var resultado = new { numemp = info_afiliados.NUMEMP, nombres = info_afiliados.NOMBRES,
                telefonos = info_afiliados.TELEFONOS, tel_celular = info_afiliados.tel_celular,
                email = info_afiliados.email, d_mnpio = colonia.d_mnpio, d_estado = colonia.d_estado,
                d_ciudad = colonia.d_ciudad, COLONIA = info_afiliados.COLONIA,
                domicilio = info_afiliados.DOMICILIO, d_asenta = colonia.d_asenta,
                apaterno = info_afiliados.APATERNO, amaterno = info_afiliados.AMATERNO,
            };
            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public async Task<ActionResult> editDhabientes(AFILIADOS data)
        {
            //Models.SMDEVEntities14 db = new Models.SMDEVEntities14();
            Models.SMDEVEntities16 db = new Models.SMDEVEntities16();
            //Models.SMDEVEntities15 db3 = new Models.SMDEVEntities15();

            //System.Diagnostics.Debug.WriteLine(data.NOMBRES);

            var editar_info = (from a in db.AFILIADOS
                               where a.NUMEMP == data.NUMEMP
                               //where a.folio_rc == null
                               select a).FirstOrDefault();

            var direccion = (from a in db.NUEVOLEON
                             where a.clave_col == editar_info.COLONIA
                             //where a.folio_rc == null
                             select a).FirstOrDefault();

            //data.NOMBRES = editar_info.NOMBRES;

            db.Database.ExecuteSqlCommand("Update AFILIADOS SET NOMBRES = '" + data.NOMBRES + "', APATERNO = '" + data.APATERNO + "', AMATERNO = '" + data.AMATERNO + "', TELEFONOS = '" + data.TELEFONOS + "', tel_celular = '" + data.tel_celular + "', email = '" + data.email + "', DOMICILIO = '" + data.DOMICILIO + "'  WHERE NUMEMP = '" + data.NUMEMP + "'");
            db.Database.ExecuteSqlCommand("Update NUEVOLEON SET d_mnpio = '" + direccion.d_mnpio + "', d_estado = '" + direccion.d_estado + "', d_ciudad = '" + direccion.d_ciudad + "'  WHERE clave_col = '" + direccion.clave_col + "'");

            return RedirectToAction("verDhabientes", "Manage");


        }


        public ActionResult EncuestaCovidSegunda()
        {
            return View();
        }


        public ActionResult IndicadoresInternacionDetalles(string id)
        {
            if (id != "") {
                DalHojaFrontal hf = new DalHojaFrontal();
                HojaFrontal hoja = hf.BuscarTitular(id);
                ViewData["hoja"] = hoja;
            }

            return View();
        }

        public ActionResult ResumenGeneralDos()
        {
            Models.SMDEVEntities10 db = new Models.SMDEVEntities10();
            //Models.laravel_sqlEntities db = new Models.laravel_sqlEntities();
            var encuestas_segunda = (from a in db.encuesta_covid_segunda
                                     orderby a.id descending
                                     select a).ToList();
            return View(encuestas_segunda);
        }
        public void Indicadores_exp_hu()
        {
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




        }
        public ActionResult IndicadoresInternacion()
        {
            Indicadores_exp_hu();

            return View();
        }
        public ActionResult Indicadores()
        {
            var fecha = DateTime.Now.AddDays(-4).ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha_correcta = DateTime.Parse(fecha);

            var fecha_hoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta_hoy = DateTime.Parse(fecha_hoy);

            Indicadores_exp_hu();
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


            //EXPEDIENTES MEDICO FAMILIAR
            var expediente_mf = (from q in db.expediente
                                 where q.medico.StartsWith("02")
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

            var valuesListExpMF = JArray.FromObject(expediente_mf).Select(x => x.Values().ToList()).ToList();


            string jsonExpMF = JsonConvert.SerializeObject(valuesListExpMF, Formatting.Indented);

            string patheXPmf = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(patheXPmf + "expediente_medicina_familiar.json", jsonExpMF);


            //EXPEDIENTES PEDIATRIA
            var expediente_pe = (from q in db.expediente
                                 where q.medico.StartsWith("03")
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

            var valuesListExpPe = JArray.FromObject(expediente_pe).Select(x => x.Values().ToList()).ToList();


            string jsonExpPe = JsonConvert.SerializeObject(valuesListExpPe, Formatting.Indented);

            string pathExpPe = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathExpPe + "expediente_pediatria.json", jsonExpPe);


            //EXPEDIENTES ESPECIALISTA
            var expediente_es = (from q in db.expediente
                                 where q.medico.StartsWith("03") == false
                                 where q.medico.StartsWith("02") == false
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


            //EXPEDIENTES MEDICO FAMILIAR HOY
            var expediente_mf_hoy = (from q in db.expediente
                                     where q.medico.StartsWith("02")
                                     where q.hora_termino != null
                                     where q.hora_termino >= fecha_correcta_hoy
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


            //EXPEDIENTES PEDIATRIA HOY
            var expediente_pe_hoy = (from q in db.expediente
                                     where q.medico.StartsWith("03")
                                     where q.hora_termino != null
                                     where q.hora_termino >= fecha_correcta_hoy
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

            var valuesListExpPeHoy = JArray.FromObject(expediente_pe_hoy).Select(x => x.Values().ToList()).ToList();


            string jsonExpPeHoy = JsonConvert.SerializeObject(valuesListExpPeHoy, Formatting.Indented);

            string patheExpPeHoy = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(patheExpPeHoy + "expediente_pediatria_hoy.json", jsonExpPeHoy);


            //EXPEDIENTES ESPECIALISTA HOY
            var expediente_es_hoy = (from q in db.expediente
                                     where q.medico.StartsWith("03") == false
                                     where q.medico.StartsWith("02") == false
                                     where q.hora_termino != null
                                     where q.hora_termino >= fecha_correcta_hoy
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

            var valuesListExpEsHoy = JArray.FromObject(expediente_es_hoy).Select(x => x.Values().ToList()).ToList();


            string jsonExpEsHoy = JsonConvert.SerializeObject(valuesListExpEsHoy, Formatting.Indented);

            string patheExpEsHoy = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(patheExpEsHoy + "expediente_especialistas_hoy.json", jsonExpEsHoy);


            Models.SMDEVEntities29 db2 = new Models.SMDEVEntities29();
            //EXPEDIENTES DE HU
            var expediente_hu = (from q in db2.expediente
                                     //where q.medico.StartsWith("51")
                                 where q.ip_realiza == "148.234.143.203" || q.ip_realiza == "148.234.140.9"
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

            var valuesListExpHU = JArray.FromObject(expediente_hu).Select(x => x.Values().ToList()).ToList();


            string jsonExpHU = JsonConvert.SerializeObject(valuesListExpHU, Formatting.Indented);

            string patheXPHU = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(patheXPHU + "expediente_hu.json", jsonExpHU);



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


        public ActionResult IndicadoresProductividadFar()
        {
            var fecha = DateTime.Now.AddDays(-5).ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha_correcta = DateTime.Parse(fecha);

            var fecha_hoy = DateTime.Now.AddDays(-5).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta_hoy = DateTime.Parse(fecha_hoy);

            Models.SMDEVEntities18 db = new Models.SMDEVEntities18();

            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

            //Recetas Normales
            var pro_no = (from q in db2.Receta_Exp
                          where q.Manual == null
                          where q.folio_rc == null
                          where q.Hora_Rcta >= fecha_correcta_hoy
                          where q.Hora_Rcta != null
                          where q.UserFar != null
                          join usuar in db2.Usuario on q.UserFar equals usuar.UsuarioId into usuX
                          from usuIn in usuX.DefaultIfEmpty()
                          select new
                          {
                              fechafecha = q.Hora_Rcta,
                              folio = q.Folio_Rcta,
                              userName = usuIn.Usu_Nombre,
                              user = q.UserFar,
                          })
                        .ToList().Select(x => new
                        {
                            userName = x.userName,
                            folio = x.folio,
                            user = x.user,
                            datedate = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fechafecha),
                            datedate2 = string.Format("{0:yyyy-MM-ddTHH:59:00.000}", x.fechafecha),
                            //datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             userName = z.userName,
                             folio = z.folio,
                             user = z.user,
                             fecha_correcta = DateTime.Parse(z.datedate),
                             fecha_correcta2 = DateTime.Parse(z.datedate2)
                         })
                         .ToList().Select(y => new
                         {
                             userName = y.userName,
                             folio = y.folio,
                             user = y.user,
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds,
                             fecha_correcta_correcta2 = y.fecha_correcta2.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        //.GroupBy(p => p.fecha_correcta_correcta)
                        //.Where(v => v.fecha_correcta_correcta2 <= v.fecha_correcta_correcta)
                        //.Where(v => v.fecha_correcta_correcta2 >= v.fecha_correcta_correcta2)
                        .GroupBy(p => new
                        {
                            p.user,
                            p.userName,
                            p.fecha_correcta_correcta,
                            p.fecha_correcta_correcta2,
                        })
                        .Select(g => new
                        {
                            y = g.Key.user,
                            x = g.Key.fecha_correcta_correcta,
                            x2 = g.Key.fecha_correcta_correcta2,
                            color = "#004a8f",
                            count = g.Count(),
                            userName = g.Key.userName,
                            //count = g.Where(p =>  p.fecha_correcta_correcta2 <= p.fecha_correcta_correcta).Select(p => p.fecha_correcta_correcta).Count()
                            //Folio = g.Key,
                            //y = g.user,
                            //x = g.fecha_correcta_correcta,
                            //x2 = g.fecha_correcta_correcta2,
                            //color = "#004a8f",
                            //count = g.Select(p => p.user).Count(),
                            //Date = g.Key,
                            //User = g.Select(p => p.user),
                            //User = g.Key,
                            //Count = g.Select(p => p.user).Count(),
                            //Date = g.Select(p => p.fecha_correcta_correcta),
                        })
                        .OrderBy(g => g.y)
                        .ToList();

            //var valuesListProNo = JArray.FromObject(pro_no).Select(x => x.Values().ToList()).ToList();
            //string jsonProNo = JsonConvert.SerializeObject(valuesListProNo, Formatting.Indented);
            string jsonProNo = JsonConvert.SerializeObject(pro_no);
            string pathProNo = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathProNo + "pro_no.json", jsonProNo);


            //Recetas Manuales
            var pro_man = (from q in db2.Receta_Exp
                           where q.Manual != null
                           where q.Hora_Rcta >= fecha_correcta_hoy
                           where q.Hora_Rcta != null
                           where q.UsuarioId != null
                           join usuar in db2.Usuario on q.UsuarioId equals usuar.UsuarioId into usuX
                           from usuIn in usuX.DefaultIfEmpty()
                           select new
                           {
                               fechafecha = q.Hora_Rcta,
                               userName = usuIn.Usu_Nombre,
                               user = q.UsuarioId,
                           })
                        .ToList().Select(x => new
                        {
                            userName = x.userName,
                            user = x.user,
                            datedate = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fechafecha),
                            datedate2 = string.Format("{0:yyyy-MM-ddTHH:59:00.000}", x.fechafecha),
                            //datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             userName = z.userName,
                             user = z.user,
                             fecha_correcta = DateTime.Parse(z.datedate),
                             fecha_correcta2 = DateTime.Parse(z.datedate2)
                         })
                         .ToList().Select(y => new
                         {
                             userName = y.userName,
                             user = y.user,
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds,
                             fecha_correcta_correcta2 = y.fecha_correcta2.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        //.GroupBy(p => p.fecha_correcta_correcta)
                        .GroupBy(p => new
                        {
                            p.user,
                            p.userName,
                            p.fecha_correcta_correcta,
                            p.fecha_correcta_correcta2,
                        })
                        .Select(g => new
                        {
                            y = g.Key.user,
                            x = g.Key.fecha_correcta_correcta,
                            x2 = g.Key.fecha_correcta_correcta2,
                            color = "#009e0d",
                            count = g.Count(),
                            userName = g.Key.userName,
                        })
                        .OrderBy(g => g.y)
                        .ToList();

            //var valuesListProMan = JArray.FromObject(pro_man).Select(x => x.Values().ToList()).ToList();
            //string jsonProMan = JsonConvert.SerializeObject(valuesListProMan, Formatting.Indented);
            string jsonProMan = JsonConvert.SerializeObject(pro_man);
            string pathProMan = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathProMan + "pro_man.json", jsonProMan);

            // Recetas Tarjeton
            var pro_tar = (from q in db2.Receta_Exp
                           where q.Manual == null
                           where q.folio_rc != null
                           where q.Hora_Rcta >= fecha_correcta_hoy
                           where q.Hora_Rcta != null
                           where q.UsuarioId != null
                           join usuar in db2.Usuario on q.UsuarioId equals usuar.UsuarioId into usuX
                           from usuIn in usuX.DefaultIfEmpty()
                           select new
                           {
                               fechafecha = q.Hora_Rcta,
                               userName = usuIn.Usu_Nombre,
                               user = q.UsuarioId,
                           })
                        .ToList().Select(x => new
                        {
                            userName = x.userName,
                            user = x.user,
                            datedate = string.Format("{0:yyyy-MM-ddTHH:00:00.000}", x.fechafecha),
                            datedate2 = string.Format("{0:yyyy-MM-ddTHH:59:00.000}", x.fechafecha)
                            //datedate = string.Format("{0:yyyy-MM-dd}", x.fechafecha),
                        })
                         .ToList().Select(z => new
                         {
                             userName = z.userName,
                             user = z.user,
                             fecha_correcta = DateTime.Parse(z.datedate),
                             fecha_correcta2 = DateTime.Parse(z.datedate2)
                         })
                         .ToList().Select(y => new
                         {
                             userName = y.userName,
                             user = y.user,
                             fecha_correcta_correcta = y.fecha_correcta.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds,
                             fecha_correcta_correcta2 = y.fecha_correcta2.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds
                             //fecha_correcta_correcta = y.fecha_correcta.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds

                         })
                        //.GroupBy(p => p.fecha_correcta_correcta)
                        .GroupBy(p => new
                        {
                            p.user,
                            p.userName,
                            p.fecha_correcta_correcta,
                            p.fecha_correcta_correcta2,
                        })
                        .Select(g => new
                        {
                            y = g.Key.user,
                            x = g.Key.fecha_correcta_correcta,
                            x2 = g.Key.fecha_correcta_correcta2,
                            color = "#FBC43D",
                            count = g.Count(),
                            userName = g.Key.userName,
                        })
                        .OrderBy(g => g.x)
                        .ToList();

            //var valuesListProTar = JArray.FromObject(pro_tar).Select(x => x.Values().ToList()).ToList();
            //string jsonProTar = JsonConvert.SerializeObject(valuesListProTar, Formatting.Indented);
            string jsonProTar = JsonConvert.SerializeObject(pro_tar);
            string pathProTar = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathProTar + "pro_tar.json", jsonProTar);


            return View();
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
            public string info { get; set; }
            public int ordenint { get; set; }
            public int espacios { get; set; }
            public string productividad { get; set; }
        }

        public class CirugiaList
        {
            public string medico { get; set; }
            public string nombre { get; set; }
            public string especialidad { get; set; }
        }

        public ActionResult IndicadoresEspecialidades()
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-dd");
            var fecha_correcta = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            #region  "ServicioMedicos"

            //DETALLES

            var expediedet = (from ex in db.expediente
                              //Gonzalitos
                              //where ex.ip_realiza.StartsWith("148.234.143")
                              where ex.fecha >= fecha_correcta
                              where ex.hora_termino != null

                              //Mederos
                              where !ex.ip_realiza.StartsWith("148.234.64")

                              //Linares
                              where ex.medico != "52023" && ex.medico != "52024" && ex.medico != "52025" && ex.medico != "52028"
                              && ex.medico != "08053" && ex.medico != "38119" && ex.medico != "52033"

                              //Home Office
                              && ex.medico != "02161"

                              //Foraneos
                              && ex.medico != "02319" && ex.medico != "02316" && ex.medico != "02318" && ex.medico != "02317" && ex.medico != "38126"
                              && ex.medico != "02324" && ex.medico != "38128" && ex.medico != "38129" && ex.medico != "38127" && ex.medico != "02347"
                              //Montemorelos
                              && ex.medico != "03139" && ex.medico != "05041" && ex.medico != "08058" && ex.medico != "08059" && ex.medico != "19018"
                              && ex.medico != "02334" && ex.medico != "13032" /*&& ex.medico != "41026"*/ && ex.medico != "06026" && ex.medico != "03140" 
                              && ex.medico != "02340"
                              //Cerralvo
                              && ex.medico != "02333"
                              //Allende
                              && ex.medico != "02321" && ex.medico != "05039" && ex.medico != "07017" && ex.medico != "08060"
                              && ex.medico != "05042" && ex.medico != "14038" && ex.medico != "05043" && ex.medico != "18015" && ex.medico != "06027"
                              && ex.medico != "02338" && ex.medico != "02336" && ex.medico != "02337" && ex.medico != "13035"
                              //Sabinas
                              && ex.medico != "02341"
                              //Cadereyta
                              && ex.medico != "52034" && ex.medico != "02349" && ex.medico != "02350" && ex.medico != "02351" && ex.medico != "02352"
                              && ex.medico != "02353" && ex.medico != "02354" && ex.medico != "02355" && ex.medico != "02356" && ex.medico != "02357"

                              //UrgenciasA
                              && ex.medico != "27004" && ex.medico != "27005"


                              //Urgencias SM
                              && ex.medico != "02030" && ex.medico != "02295" && ex.medico != "02298" && ex.medico != "02291"
                              && ex.medico != "02302" && ex.medico != "02286"

                              //Unidad ER
                              where ex.ip_realiza != "148.234.143.169" && ex.ip_realiza != "148.234.143.217" && ex.ip_realiza != "148.234.143.151" && ex.ip_realiza != "148.234.143.176" && ex.ip_realiza != "148.234.143.185"


                              //Subrogados
                              where ex.medico != "38115" && ex.medico != "38112" && ex.medico != "38113" && ex.medico != "14037" && ex.medico != "38111"
                              && ex.medico != "38117" && ex.medico != "03132" && ex.medico != "38110" && ex.medico != "38114"
                              && ex.medico != "19017" && ex.medico != "38116" && ex.medico != "38118" && ex.medico != "38120" && ex.medico != "15512"
                              && ex.medico != "34019" && ex.medico != "51035" && ex.medico != "38121" && ex.medico != "38122" && ex.medico != "38123"
                              && ex.medico != "38124" && ex.medico != "38125"

                              //Ciudad Universitaria
                              where !ex.ip_realiza.StartsWith("148.234.218")

                              //SEMAC UER
                              where ex.medico != "02327"

                              && ex.medico != ""

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
                              where especiIn.CLAVE != "51"
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


            string queryDet = "SELECT Dertbl.especialidad as especialidad, Dertbl.medico as medico, Dertbl.nombre as nombre, Dertbl.fecha as fecha, Count(*) as count, Count(*) as telefonica, Count(*) as presencial  FROM ( SELECT expediente_dental.medico as medico, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as nombre, ESPECIALIDADES.DESCRIPCION as especialidad, expediente_dental.fecha, COUNT(*) as count FROM expediente_dental, ESPECIALIDADES, MEDICOS WHERE ESPECIALIDADES.CLAVE = Left(expediente_dental.medico, 2) and expediente_dental.fecha = '" + fecha + "' AND MEDICOS.Numero = expediente_dental.medico GROUP BY fecha, ESPECIALIDADES.DESCRIPCION, medico, MEDICOS.Nombre, MEDICOS.Apaterno, MEDICOS.Amaterno, expediente_dental.expediente) AS Dertbl GROUP BY Dertbl.medico, Dertbl.fecha, Dertbl.nombre, Dertbl.especialidad;";

            var resultDenDet = db.Database.SqlQuery<Dental>(queryDet);
            var expedi_den_det = resultDenDet.ToList();


            var resultsDet1 = new List<IndEspListDet>();

            //Fecha que comenzo lo de la unidad ER
            //var fechaUER = DateTime.Now.ToString("2021-07-28");
            //var fechaUERDT = DateTime.Parse(fechaUER);

            //CONSULTAS NORMALES

            foreach (var item in expediedet)
            {
                var numeroMed = item.medico.Substring(0, 2);

                var especi = "";

                if (numeroMed == "02")
                {
                    if (item.medico == "02307" || item.medico == "02212" || item.medico == "02085")
                    {
                        especi = "MEDICINA FAMILIAR";
                    }
                    else
                    {
                        especi = "MEDICINA GENERAL";
                    }
                }
                else
                {
                    especi = item.especialidad;
                }

                //Ordenes de internamiento
                var ordenCount = (from a in db.Orden_Int
                                where a.medico == item.medico
                                where a.fecha_registro >= fecha_correcta
                                //where a.hora_termino == null
                                select a).Count();

                //Espacios
                string citas = "select Medico as Medico from CITAS WHERE Medico = '" + item.medico + "' and Fecha = '" + fecha + "' and Tipo != '00' and NumEmp != '99999100'";
                var citasResult = db.Database.SqlQuery<Citas>(citas);
                var citasRes = citasResult.Count();

                //Productividad
                var productividad = "";
                if (citasRes != 0)
                {
                    productividad = ((item.count * 100) / citasRes) + " %";
                }
                else
                {
                    productividad = "Sin agenda";
                }
                

                //System.Diagnostics.Debug.WriteLine(citasRes);


                var resultDet = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = especi,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    //ordenint = ordenCount,
                    ordenint = citasRes,
                    productividad = productividad,
                };

                resultsDet1.Add(resultDet);
            }

            //DETALLES DENTAL

            foreach (var item in expedi_den_det)
            {
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_correcta
                                  //where a.hora_termino == null
                                  select a).Count();

                //Espacios
                string citas = "select tipo as tipo from CITAS WHERE Medico = '" + item.medico + "' and Fecha = '" + fecha + "'";
                var citasResult = db.Database.SqlQuery<Citas>(citas);
                var citasRes = citasResult.Count();

                //Productividad
                var productividad = "";
                if (citasRes != 0)
                {
                    productividad = ((item.count * 100) / citasRes) + " %";
                }
                else
                {
                    productividad = "Sin agenda";
                }


                var resultDet2 = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = "DENTAL",
                    fecha = fechaHoy,
                    count = item.count,
                    telefonica = 0,
                    presencial = item.presencial,
                    //ordenint = ordenCount,
                    ordenint = citasRes,
                    productividad = productividad,
                };

                resultsDet1.Add(resultDet2);

            }

            //System.Diagnostics.Debug.WriteLine(resultsDet1);


            string json2 = JsonConvert.SerializeObject(resultsDet1);
            string path2 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2 + "indicadores/especialidades/detalles.json", json2);

            #endregion

            #region  "Mederos"

            var expediedet_me = (from ex in db.expediente
                                 where ex.ip_realiza.StartsWith("148.234.64")
                                 where ex.fecha >= fecha_correcta
                                 where ex.hora_termino != null
                                 //where ex.medico.Substring(0, 2) == espec.CLAVE
                                 join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                 from medicosIn in mediX.DefaultIfEmpty()
                                 join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                 from especiIn in especiX.DefaultIfEmpty()
                                 where especiIn.CLAVE != "44"
                                 && especiIn.CLAVE != "45"
                                 && especiIn.CLAVE != "46"
                                 && especiIn.CLAVE != "49"
                                 && especiIn.CLAVE != "04"
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


            var resultsDetMe = new List<IndEspListDet>();

            foreach (var item in expediedet_me)
            {

                //Ordenes de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_correcta
                                  //where a.hora_termino == null
                                  select a).Count();

                //Espacios
                string citas = "select Medico as Medico from CITAS WHERE Medico = '" + item.medico + "' and Fecha = '" + fecha + "' and Tipo != '00' and NumEmp != '99999100'";
                var citasResult = db.Database.SqlQuery<Citas>(citas);
                var citasRes = citasResult.Count();

                //Productividad
                var productividad = "";
                if (citasRes != 0)
                {
                    productividad = ((item.count * 100) / citasRes) + " %";
                }
                else
                {
                    productividad = "Sin agenda";
                }

                var resultMe = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    //ordenint = ordenCount,
                    ordenint = citasRes,
                    productividad = productividad,
                };

                resultsDetMe.Add(resultMe);
            }

            string json2_me = JsonConvert.SerializeObject(resultsDetMe);
            string path2_me = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_me + "indicadores/especialidades/detalles_me.json", json2_me);

            #endregion

            #region  "Linares"

            //LINARES DETALLES
            var linares_detalles = (from ex in db.expediente
                                    where ex.medico == "52023" || ex.medico == "52024" || ex.medico == "52025" || ex.medico == "52028" 
                                    || ex.medico == "08053" || ex.medico == "38119" || ex.medico == "52033"
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
                                  //especialidad = "Linares",
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
                                 //especialidad = "Linares",
                                 especialidad = g.Key.especialidad,
                                 fecha = g.Key.fecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();


            var resultsDetLi = new List<IndEspListDet>();

            foreach (var item in linares_detalles)
            {

                //Ordenes de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_correcta
                                  //where a.hora_termino == null
                                  select a).Count();

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

                var resultLi = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetLi.Add(resultLi);
            }

            string json2_li = JsonConvert.SerializeObject(resultsDetLi);
            string path2_li = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_li + "indicadores/especialidades/detalles_linares.json", json2_li);

            #endregion

            #region  "HomeOffice"

            //HomeOffice detalles
            var homeoffice_detalles = (from ex in db.expediente
                                       where ex.medico == "02161"
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


            var resultsDetHO = new List<IndEspListDet>();

            foreach (var item in homeoffice_detalles)
            {

                //Ordenes de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_correcta
                                  //where a.hora_termino == null
                                  select a).Count();

                var resultHO = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetHO.Add(resultHO);
            }


            string json2_ho = JsonConvert.SerializeObject(resultsDetHO);
            string path2_ho = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_ho + "indicadores/especialidades/detalles_homeoffice.json", json2_ho);

            #endregion

            #region  "UrgenciasA"

            //UrgenciasA
            var urgenciasa_detalles = (from ex in db.expediente
                                       where ex.medico == "27004" || ex.medico == "27005"
                                       where ex.fecha >= fecha_correcta
                                       where ex.hora_termino != null
                                       join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                       from medicosIn in mediX.DefaultIfEmpty()
                                       join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                       from especiIn in especiX.DefaultIfEmpty()
                                       where especiIn.CLAVE == "51"
                                       //where ex.ip_realiza == "148.234.140.200" || ex.ip_realiza == "148.234.140.95" || ex.ip_realiza == "148.234.143.153" || ex.ip_realiza == "148.234.143.166" || ex.ip_realiza == "148.234.143.192" || ex.ip_realiza == "148.234.143.203" || ex.ip_realiza == "187.160.61.75" || ex.ip_realiza == "189.175.114.33" || ex.ip_realiza == "189.175.149.234" || ex.ip_realiza == "189.175.152.13"
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


            var resultsDetUA = new List<IndEspListDet>();

            foreach (var item in urgenciasa_detalles)
            {
                var espe = item.especialidad;

                if (item.medico == "51009")
                {
                    espe = "TRAUMATOLOGÍA";
                }

                //Ordenes de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_correcta
                                  //where a.hora_termino == null
                                  select a).Count();

                var resultUA = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = espe,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetUA.Add(resultUA);
            }



            string json2_ua = JsonConvert.SerializeObject(resultsDetUA);
            string path2_ua = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_ua + "indicadores/especialidades/detalles_urgenciasa.json", json2_ua);

            #endregion

            #region  "UrgenciasSM"

            //Urgencias SM detalles
            var urgenciassm_detalles = (from ex in db.expediente
                                            //where ex.medico == "51018" || ex.medico == "51028" || ex.medico == "51022" || ex.medico == "51029" || ex.medico == "51020" || ex.medico == "51031" || ex.medico == "51024"
                                        where ex.medico == "02030" || ex.medico == "02295" || ex.medico == "02298" || ex.medico == "02291" /*|| ex.medico == "02255"*/ || ex.medico == "02302" || ex.medico == "02286"
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


            var resultsDetUSM = new List<IndEspListDet>();

            foreach (var item in urgenciassm_detalles)
            {

                //Ordenes de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_correcta
                                  //where a.hora_termino == null
                                  select a).Count();

                var resultUSM = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetUSM.Add(resultUSM);
            }


            string json2_usm = JsonConvert.SerializeObject(resultsDetUSM);
            string path2_usm = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_usm + "indicadores/especialidades/detalles_urgenciassm.json", json2_usm);

            #endregion

            #region  "UnidadER"

            var fecha2 = DateTime.Now.ToString("2021-07-28T00:00:00.000");
            var fechaDT = DateTime.Parse(fecha2);

            //Unidad ER detalles
            var unidader_detalles = (from ex in db.expediente
                                     //where ex.medico == "03130" || ex.medico == "02309" || ex.medico == "02293" || ex.medico == "13031" || ex.medico == "02310" 
                                     //|| ex.medico == "02311" || ex.medico == "02312" || ex.medico == "02290" || ex.medico == "13031" || ex.medico == "02215"
                                     where ex.fecha >= fecha_correcta
                                     where ex.ip_realiza == "148.234.143.169" || ex.ip_realiza == "148.234.143.217" || ex.ip_realiza == "148.234.143.151" || ex.ip_realiza == "148.234.143.176" || ex.ip_realiza == "148.234.143.185"
                                     where ex.hora_termino != null
                                     join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                     from medicosIn in mediX.DefaultIfEmpty()
                                     join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                     from especiIn in especiX.DefaultIfEmpty()
                                     select new
                                     {
                                         medico = ex.medico,
                                         fecha = ex.fecha,
                                         especialidad = "Unidad ER",
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

            var resultsDetUER = new List<IndEspListDet>();

            foreach (var item in unidader_detalles)
            {

                //Ordenes de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_correcta
                                  //where a.hora_termino == null
                                  select a).Count();

                var resultUER = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetUER.Add(resultUER);
            }

            string json2_uer = JsonConvert.SerializeObject(resultsDetUER);
            string path2_uer = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_uer + "indicadores/especialidades/detalles_unidader.json", json2_uer);

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


                //Ordenes de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.numero
                                  where a.fecha_registro >= fecha_correcta
                                  //where a.hora_termino == null
                                  select a).Count();


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
                        info = item.numero + "*" + fechaHoy,
                        ordenint = ordenCount,
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
                        info = item.numero + "*" + fechaHoy,
                        ordenint = ordenCount,
                    };

                    resultsDetSub.Add(resultSub);
                }


                

            }

            string jsonSub = JsonConvert.SerializeObject(resultsDetSub);
            string pathSub = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathSub + "indicadores/especialidades/detallessubrogados.json", jsonSub);


            #endregion

            #region "Ciudad Universitaria"

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

            //System.Diagnostics.Debug.WriteLine(expCu);

            var resultsDetCU = new List<IndEspListDet>();

            foreach (var item in expCu)
            {
                var espe = item.especialidad;

                if (item.medico == "02235" || item.medico == "02294")
                {
                    espe = "MEDICINA GENERAL";
                }

                //Ordenes de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_correcta
                                  //where a.hora_termino == null
                                  select a).Count();

                var resultCU = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = espe,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetCU.Add(resultCU);
            }

            string jsonCU = JsonConvert.SerializeObject(resultsDetCU);
            string pathCU = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathCU + "indicadores/especialidades/detalles_cu.json", jsonCU);


            #endregion

            #region "Foraneos"

            var expeFor2 = (from ex in db.MEDICOS
                            //Foraneos
                            join especi in db.ESPECIALIDADES on ex.Numero.Substring(0, 2) equals especi.CLAVE into especiX
                            from especiIn in especiX.DefaultIfEmpty()
                            where ex.Numero != "02101"
                            where ex.Numero == "02319" || ex.Numero == "02316" || ex.Numero == "02318" || ex.Numero == "02317" || ex.Numero == "38126" 
                            || ex.Numero == "02324" || ex.Numero == "38128" || ex.Numero == "38129" || ex.Numero == "38127" || ex.Numero == "02347"
                            //Montemorelos
                            || ex.Numero == "03139" || ex.Numero == "05041" || ex.Numero == "08058" || ex.Numero == "08059" || ex.Numero == "19018" || ex.Numero == "03140"
                            || ex.Numero == "02334" || ex.Numero == "13032" || /* ex.Numero == "41026" ||*/ ex.Numero == "06026" || ex.Numero == "02340"
                            //Cerralvo
                            || ex.Numero == "02333"
                            //Allende
                            || ex.Numero == "02321" || ex.Numero == "07017" || ex.Numero == "08060"
                            || ex.Numero == "05042" || ex.Numero == "14038" || ex.Numero == "05043" || ex.Numero == "18015" | ex.Numero == "06027"
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

                //Sabinas
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


                /*if(item.numero == "41026")
                {
                    total = expediente;
                    lugar = "Montemorelos - Clínica San Mateo";
                    espe = "RADIOLOGÍA";
                }*/


                if(item.numero == "13032")
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

            #region  "Unidad ER SEMAC"

            var fecha3 = DateTime.Now.ToString("2022-02-01T00:00:00.000");
            var fechaDT3 = DateTime.Parse(fecha3);

            //Unidad ER SEMAC
            var uersemac_detalles = (from ex in db.expediente
                                     where ex.medico == "02327"
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
                                         especialidad = "Unidad ER SEMAC",
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

            var uerSEMAC = new List<IndEspListDet>();

            foreach (var item in uersemac_detalles)
            {

                //Ordenes de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_correcta
                                  //where a.hora_termino == null
                                  select a).Count();

                var uerSEMACList = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                uerSEMAC.Add(uerSEMACList);
            }

            string json_uersemac = JsonConvert.SerializeObject(uerSEMAC);
            string path_uersemac = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path_uersemac + "indicadores/especialidades/detalles_uersemac.json", json_uersemac);

            #endregion

            #region "Cirugia"


            //SELECT AspNetUsers.UserName FROM AspNetUsers INNER JOIN AspNetUserRoles ON AspNetUserRoles.UserId = AspNetUsers.Id where AspNetUserRoles.RoleId = 72

            string query = "SELECT AspNetUsers.UserName as medico , MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno AS nombre, ESPECIALIDADES.DESCRIPCION as especialidad FROM AspNetUsers INNER JOIN AspNetUserRoles ON AspNetUserRoles.UserId = AspNetUsers.Id INNER JOIN MEDICOS ON MEDICOS.Numero = AspNetUsers.UserName INNER JOIN ESPECIALIDADES ON ESPECIALIDADES.CLAVE = SUBSTRING(AspNetUsers.UserName, 1, 2) where AspNetUserRoles.RoleId = 72 and AspNetUsers.UserName != 02101";
            var result = db.Database.SqlQuery<CirugiaList>(query);
            var cirugias = result.ToList();



            var resultsDetCiru = new List<IndEspListDet>();

            //

            foreach (var item in cirugias)
            {


                var notaquiru = (from ex in db.NotaQuirurgica
                                  join orden in db.Orden_Int on ex.id_orden equals orden.id_orden into ordenX
                                  from ordenIn in ordenX.DefaultIfEmpty()
                                 //join medico in db.MEDICOS on ordenIn.medico equals medico.Numero into medicoX
                                 //from medicoIn in medicoX.DefaultIfEmpty()
                                 where ex.fecha_registro >= fecha_correcta
                                  //where ex.hora_termino != null
                                  //Subrogados
                                  where ordenIn.medico == item.medico
                                 select ex)
                              .Count();

                var lugar = "";
                var total = 0;
                var espe = "";

                if (notaquiru != 0)
                {
                    var resultCiru = new IndEspListDet
                    {
                        medico = item.medico,
                        nombre = item.nombre,
                        especialidad = item.especialidad,
                        fecha = fechaHoy,
                        count = notaquiru,
                        lugar = lugar,
                        telefonica = 0,
                        presencial = notaquiru,
                    };

                    resultsDetCiru.Add(resultCiru);
                }
                else
                {
                    var resultCiru = new IndEspListDet
                    {
                        medico = item.medico,
                        nombre = item.nombre,
                        especialidad = item.especialidad,
                        fecha = fechaHoy,
                        count = 0,
                        lugar = lugar,
                        telefonica = 0,
                        presencial = 0,
                    };

                    resultsDetCiru.Add(resultCiru);
                }




            }

            //System.Diagnostics.Debug.WriteLine(resultsDetCiru);

            string jsonCiru = JsonConvert.SerializeObject(resultsDetCiru);
            string pathCiru = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathCiru + "indicadores/especialidades/cirugias.json", jsonCiru);


            #endregion


            return View();

        }

        public JsonResult DetallesProductividad(string medio, string fecha)
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fechaDT = DateTime.Parse(fecha);

            var expesubro = (from ex in db.expediente
                             join dhabientes in db.DHABIENTES on ex.numemp equals dhabientes.NUMEMP into dhabX
                             from dhabIn in dhabX.DefaultIfEmpty()

                             where ex.medico == medio
                             where ex.fecha == fechaDT
                             select new
                             {
                                 paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                             })
                             .ToList();

            System.Diagnostics.Debug.WriteLine(expesubro);

            return new JsonResult { Data = expesubro, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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


            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            #region  "ServiciosMedicos"

            var expediedet = (from ex in db.expediente
                              //Gonzalitos
                              where ex.fecha >= fecha_minDate
                              where ex.fecha <= fecha_maxDate
                              where ex.hora_termino != null

                              //Mederos
                              where !ex.ip_realiza.StartsWith("148.234.64")

                              //Linares
                              where ex.medico != "52023" && ex.medico != "52024" && ex.medico != "52025" && ex.medico != "52028"
                              && ex.medico != "08053" && ex.medico != "38119" && ex.medico != "52033"

                              //Home Office
                              && ex.medico != "02161"

                              //Foraneos
                              && ex.medico != "02319" && ex.medico != "02316" && ex.medico != "02318" && ex.medico != "02317" && ex.medico != "38126"
                              && ex.medico != "02324" && ex.medico != "38128" && ex.medico != "38129" && ex.medico != "38127" && ex.medico != "02347"
                              //Montemorelos
                              && ex.medico != "03139" && ex.medico != "05041" && ex.medico != "08058" && ex.medico != "08059" && ex.medico != "19018"
                              && ex.medico != "02334" && ex.medico != "13032" /*&& ex.medico != "41026"*/ && ex.medico != "06026" && ex.medico != "03140" 
                              && ex.medico != "02340"
                              //Cerralvo
                              && ex.medico != "02333"
                              //Allende
                              && ex.medico != "02321" && ex.medico != "05039" && ex.medico != "07017" && ex.medico != "08060"
                              && ex.medico != "05042" && ex.medico != "14038" && ex.medico != "05043" && ex.medico != "18015" && ex.medico != "06027"
                              && ex.medico != "02338" && ex.medico != "02336" && ex.medico != "02337" && ex.medico != "13035"
                              //Sabinas
                              && ex.medico != "02341"
                              //Cadereyta
                              && ex.medico != "52034" && ex.medico != "02349" && ex.medico != "02350" && ex.medico != "02351" && ex.medico != "02352"
                              && ex.medico != "02353" && ex.medico != "02354" && ex.medico != "02355" && ex.medico != "02356" && ex.medico != "02357"


                              //UrgenciasA
                              && ex.medico != "27004" && ex.medico != "27005"


                              //Urgencias SM
                              && ex.medico != "02030" && ex.medico != "02295" && ex.medico != "02298" && ex.medico != "02291"
                              && ex.medico != "02302" && ex.medico != "02286"

                              //Unidad ER
                              where ex.ip_realiza != "148.234.143.169" && ex.ip_realiza != "148.234.143.217" && ex.ip_realiza != "148.234.143.151" && ex.ip_realiza != "148.234.143.176" && ex.ip_realiza != "148.234.143.185"


                              //Subrogados
                              where ex.medico != "38115" && ex.medico != "38112" && ex.medico != "38113" && ex.medico != "14037" && ex.medico != "38111"
                              && ex.medico != "38117" && ex.medico != "03132" && ex.medico != "38110" && ex.medico != "38114"
                              && ex.medico != "19017" && ex.medico != "38116" && ex.medico != "38118" && ex.medico != "38120" && ex.medico != "15512"
                              && ex.medico != "34019" && ex.medico != "51035" && ex.medico != "38121" && ex.medico != "38122" && ex.medico != "38123"
                              && ex.medico != "38124" && ex.medico != "38125"

                              //Ciudad Universitaria
                              where !ex.ip_realiza.StartsWith("148.234.218")

                              //SEMAC UER
                              where ex.medico != "02327"

                              && ex.medico != ""

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
                              where especiIn.CLAVE != "51"
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

            //Fecha que comenzo lo de la unidad ER
            var fechaUER = DateTime.Now.ToString("2021-07-28");
            var fechaUERDT = DateTime.Parse(fechaUER);

            var fechaUER2 = DateTime.Now.ToString("2022-01-02");
            var fechaUERDT2 = DateTime.Parse(fechaUER2);

            #region "Unidad ER"
            var expedieUER = (from ex in db.expediente
                              where !ex.ip_realiza.StartsWith("148.234.64")
                              where ex.ip_realiza != "148.234.143.203"
                              where ex.ip_realiza != "148.234.140.9"
                              where ex.fecha >= fecha_minDate
                              where ex.fecha <= fechaUERDT
                              //Desde esa fecha no tomar en cuenta estos medicos ya que son de Unidad ER, quitar cuando termine
                              where ex.medico == "03130" || ex.medico == "02309" || ex.medico == "02293" || ex.medico == "13031" || ex.medico == "02310" || ex.medico == "02311" || ex.medico == "02312"
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
                                 fecha = "De: " + minDate + " Hasta: 2021-07-28",
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();

            var expedieUER2 = (from ex in db.expediente
                              where !ex.ip_realiza.StartsWith("148.234.64")
                              where ex.ip_realiza != "148.234.143.203"
                              where ex.ip_realiza != "148.234.140.9"
                              where ex.fecha >= fecha_minDate
                              where ex.fecha <= fechaUERDT2
                              //Desde esa fecha no tomar en cuenta estos medicos ya que son de Unidad ER, quitar cuando termine
                              where ex.medico == "02290"
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
                                 fecha = "De: " + minDate + " Hasta: 2022-01-02",
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();


            //System.Diagnostics.Debug.WriteLine(expedieUER2);


            #endregion

            //Detales
            //string queryDet = "SELECT Dertbl.especialidad as especialidad, Dertbl.medico as medico, Dertbl.nombre as nombre, Count(*) as count, Count(*) as telefonica, Count(*) as presencial  FROM ( SELECT expediente_dental.medico as medico, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as nombre, ESPECIALIDADES.DESCRIPCION as especialidad, COUNT(*) as count FROM expediente_dental, ESPECIALIDADES, MEDICOS WHERE ESPECIALIDADES.CLAVE = Left(expediente_dental.medico, 2) and expediente_dental.fecha >= '" + minDate + "T00:00:00.000' and  expediente_dental.fecha <= '" + maxDate + "T00:00:00.000' AND MEDICOS.Numero = expediente_dental.medico GROUP BY ESPECIALIDADES.DESCRIPCION, medico, MEDICOS.Nombre, MEDICOS.Apaterno, MEDICOS.Amaterno, expediente_dental.expediente) AS Dertbl GROUP BY Dertbl.medico, Dertbl.nombre, Dertbl.especialidad; ";
            string queryDet = "SELECT Dertbl.nombre as nombre, Dertbl.medico as medico, COUNT(*) AS count FROM ( SELECT fecha, expediente, medico, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as nombre FROM expediente_dental INNER JOIN MEDICOS ON MEDICOS.Numero = expediente_dental.medico WHERE fecha >= '" + minDate + "T00:00:00.000' and  fecha <= '" + maxDate + "T00:00:00.000' group by fecha, expediente, medico, MEDICOS.Nombre, MEDICOS.Apaterno, MEDICOS.Amaterno ) AS Dertbl GROUP BY Dertbl.medico, Dertbl.nombre;";

            var resultDenDet = db.Database.SqlQuery<Dental>(queryDet);
            var expedi_den_det = resultDenDet.ToList();

            //CONSULTAS NORMALES

            var resultsDet1 = new List<IndEspListDet>();

            foreach (var item in expediedet)
            {
                var numeroMed = item.medico.Substring(0, 2);



                var especi = "";

                if (numeroMed == "02")
                {
                    if (item.medico == "02307" || item.medico == "02212" || item.medico == "02085")
                    {
                        especi = "MEDICINA FAMILIAR";
                    }
                    else
                    {
                        especi = "MEDICINA GENERAL";
                    }
                }
                else
                {
                    especi = item.especialidad;
                }

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();

                //Espacios
                string citas = "select Medico as Medico from CITAS WHERE Medico = '" + item.medico + "' and Fecha >= '" + minDate + "T00:00:00.000' and Fecha <= '" + maxDate + "T00:00:00.000' and Tipo != '00' and NumEmp != '99999100'";
                var citasResult = db.Database.SqlQuery<Citas>(citas);
                var citasRes = citasResult.Count();

                //System.Diagnostics.Debug.WriteLine(citasRes);

                //Productividad
                var productividad = "";
                if (citasRes != 0)
                {
                    productividad = ((item.count * 100) / citasRes) + " %";
                }
                else
                {
                    productividad = "Sin agenda";
                }

                var resultDet = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = especi,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = citasRes,
                    productividad = productividad,
                };

                resultsDet1.Add(resultDet);
            }

            //UNIDAD ER ANTES DE QUE SE FUERAN PARA ALLÁ
            foreach (var item in expedieUER)
            {

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();


                //Espacios
                string citas = "select Medico as Medico from CITAS WHERE Medico = '" + item.medico + "' and Fecha >= '" + minDate + "T00:00:00.000' and Fecha <= '" + maxDate + "T00:00:00.000' and Tipo != '00' and NumEmp != '99999100'";
                var citasResult = db.Database.SqlQuery<Citas>(citas);
                var citasRes = citasResult.Count();

                //System.Diagnostics.Debug.WriteLine(citasRes);

                //Productividad
                var productividad = "";
                if (citasRes != 0)
                {
                    productividad = ((item.count * 100) / citasRes) + " %";
                }
                else
                {
                    productividad = "Sin agenda";
                }


                var resultDet = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = citasRes,
                    productividad = productividad,
                };

                resultsDet1.Add(resultDet);
            }

            foreach (var item in expedieUER2)
            {

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();


                //Espacios
                string citas = "select Medico as Medico from CITAS WHERE Medico = '" + item.medico + "' and Fecha >= '" + minDate + "T00:00:00.000' and Fecha <= '" + maxDate + "T00:00:00.000' and Tipo != '00' and NumEmp != '99999100'";
                var citasResult = db.Database.SqlQuery<Citas>(citas);
                var citasRes = citasResult.Count();

                //System.Diagnostics.Debug.WriteLine(citasRes);

                //Productividad
                var productividad = "";
                if (citasRes != 0)
                {
                    productividad = ((item.count * 100) / citasRes) + " %";
                }
                else
                {
                    productividad = "Sin agenda";
                }


                var resultDet = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = citasRes,
                    productividad = productividad,
                };

                resultsDet1.Add(resultDet);
            }


            //DENTAL
            foreach (var item in expedi_den_det)
            {

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();

                //Espacios
                string citas = "select Medico as Medico from CITAS WHERE Medico = '" + item.medico + "' and Fecha >= '" + minDate + "T00:00:00.000' and Fecha <= '" + maxDate + "T00:00:00.000'";
                var citasResult = db.Database.SqlQuery<Citas>(citas);
                var citasRes = citasResult.Count();

                //System.Diagnostics.Debug.WriteLine(citasRes);

                //Productividad
                var productividad = "";
                if (citasRes != 0)
                {
                    productividad = ((item.count * 100) / citasRes) + " %";
                }
                else
                {
                    productividad = "Sin agenda";
                }


                var resultDet2 = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = "DENTAL",
                    fecha = textoFecha,
                    count = item.count,
                    telefonica = 0,
                    presencial = item.count,
                    ordenint = citasRes,
                    productividad = productividad,
                };

                resultsDet1.Add(resultDet2);

            }

            string json2 = JsonConvert.SerializeObject(resultsDet1);
            string path2 = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2 + "indicadores/especialidades/detalles.json", json2);

            #endregion

            #region  "Mederos"
            //MEDEROS

            //MEDEROS DETALLES
            var expediedet_me = (from ex in db.expediente
                              where ex.ip_realiza.StartsWith("148.234.64")
                              where ex.fecha >= fecha_minDate
                              where ex.fecha <= fecha_maxDate
                              where ex.hora_termino != null
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

            var resultsDetMe = new List<IndEspListDet>();

            foreach (var item in expediedet_me)
            {
                var numeroMed = item.medico.Substring(0, 2);

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();

                var resultDetMe = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetMe.Add(resultDetMe);
            }


            string json2_me = JsonConvert.SerializeObject(resultsDetMe);
            string path2_me = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_me + "indicadores/especialidades/detalles_me.json", json2_me);

            #endregion

            #region  "Linares"
            //LINARES DETALLES
            var expediedet_li = (from ex in db.expediente
                                 where ex.medico == "52023" || ex.medico == "52024" || ex.medico == "52025" || ex.medico == "52028" 
                                 || ex.medico == "52029" || ex.medico == "08053" || ex.medico == "38119" || ex.medico == "52033"
                                 where ex.fecha >= fecha_minDate
                                 where ex.fecha <= fecha_maxDate
                                 where ex.hora_termino != null
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
                                 //especialidad = "Linares",
                                 especialidad = g.Key.especialidad,
                                 fecha = textoFecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             //.OrderBy(g => g.fecha)
                             .OrderBy(g => g.medico)
                             .ToList();


            var resultsDetLi = new List<IndEspListDet>();

            foreach (var item in expediedet_li)
            {

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();

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
                    especialidad = "MEDICINA INTERNA";
                }

                var resultDetLi = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetLi.Add(resultDetLi);
            }

            string json2_li = JsonConvert.SerializeObject(resultsDetLi);
            string path2_li = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_li + "indicadores/especialidades/detalles_linares.json", json2_li);

            #endregion

            #region  "HomeOffice"
            //HomeOffice

            //HOMEOFFICE DETALLES
            var expediedet_ho = (from ex in db.expediente
                                 where ex.medico == "02161"
                                 where ex.fecha >= fecha_minDate
                                 where ex.fecha <= fecha_maxDate
                                 where ex.hora_termino != null
                                 join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                 from medicosIn in mediX.DefaultIfEmpty()
                                 join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                 from especiIn in especiX.DefaultIfEmpty()
                                 select new
                                 {
                                     medico = ex.medico,
                                     especialidad = especiIn.DESCRIPCION,
                                     nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                     consultadistancia = ex.consultadistancia
                                 })
                              .ToList().Select(x => new
                              {
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = textoFecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             .OrderBy(g => g.medico)
                             .ToList();


            var resultsDetHO = new List<IndEspListDet>();

            foreach (var item in expediedet_ho)
            {

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();

                var resultDetHO = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetHO.Add(resultDetHO);
            }



            string json2_ho = JsonConvert.SerializeObject(resultsDetHO);
            string path2_ho = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_ho + "indicadores/especialidades/detalles_homeoffice.json", json2_ho);

            #endregion

            #region  "UrgenciasA"

            //URGENCIAS A DETALLES
            var expediedet_ua = (from ex in db.expediente
                                 //where ex.medico == "51018" || ex.medico == "51028" || ex.medico == "51022" || ex.medico == "51029" || ex.medico == "51020" || ex.medico == "51031"  || ex.medico == "51024"
                                 where ex.fecha >= fecha_minDate
                                 where ex.fecha <= fecha_maxDate
                                 where ex.hora_termino != null
                                 join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                 from medicosIn in mediX.DefaultIfEmpty()
                                 join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                 from especiIn in especiX.DefaultIfEmpty()
                                 where especiIn.CLAVE == "51"
                                 //where ex.ip_realiza == "148.234.140.200" || ex.ip_realiza == "148.234.140.95" || ex.ip_realiza == "148.234.143.153" || ex.ip_realiza == "148.234.143.166" || ex.ip_realiza == "148.234.143.192" || ex.ip_realiza == "148.234.143.203" || ex.ip_realiza == "187.160.61.75" || ex.ip_realiza == "189.175.114.33" || ex.ip_realiza == "189.175.149.234" || ex.ip_realiza == "189.175.152.13"
                                 select new
                                 {
                                     medico = ex.medico,
                                     especialidad = especiIn.DESCRIPCION,
                                     nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                     consultadistancia = ex.consultadistancia
                                 })
                              .ToList().Select(x => new
                              {
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = textoFecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             .OrderBy(g => g.medico)
                             .ToList();

            var resultsDetUA = new List<IndEspListDet>();

            foreach (var item in expediedet_ua)
            {
                var espe = item.especialidad;

                if (item.medico == "51009")
                {
                    espe = "TRAUMATOLOGÍA";
                }

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();

                var resultUA = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = espe,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetUA.Add(resultUA);
            }

            string json2_ua = JsonConvert.SerializeObject(resultsDetUA);
            string path2_ua = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_ua + "indicadores/especialidades/detalles_urgenciasa.json", json2_ua);

            #endregion

            #region  "UrgenciasSM"
            //URGENCIAS Servicios Médicos
            //URGENCIAS Servicios Médicos
            var expediedet_usm = (from ex in db.expediente
                                 where ex.medico == "02030" || ex.medico == "02295" || ex.medico == "02298" || ex.medico == "02291" /*|| ex.medico == "02255"*/ || ex.medico == "02302" || ex.medico == "02286"
                                  //where ex.medico == "51018" || ex.medico == "51028" || ex.medico == "51022" || ex.medico == "51029" || ex.medico == "51020" || ex.medico == "51031" || ex.medico == "51024"
                                  where ex.fecha >= fecha_minDate
                                 where ex.fecha <= fecha_maxDate
                                 where ex.hora_termino != null
                                 join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                 from medicosIn in mediX.DefaultIfEmpty()
                                 join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                 from especiIn in especiX.DefaultIfEmpty()
                                 select new
                                 {
                                     medico = ex.medico,
                                     especialidad = especiIn.DESCRIPCION,
                                     nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                     consultadistancia = ex.consultadistancia
                                 })
                              .ToList().Select(x => new
                              {
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = textoFecha,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             .OrderBy(g => g.medico)
                             .ToList();


            var resultsDetUSM = new List<IndEspListDet>();

            foreach (var item in expediedet_usm)
            {

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();

                var resultUSM = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                resultsDetUSM.Add(resultUSM);
            }




            string json2_usm = JsonConvert.SerializeObject(resultsDetUSM);
            string path2_usm = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path2_usm + "indicadores/especialidades/detalles_urgenciassm.json", json2_usm);

            #endregion

            #region  "Unidad ER"
            //Unidad ER

            var expediedet_uer = (from ex in db.expediente
                                  //where ex.medico == "03130" || ex.medico == "02309" || ex.medico == "02293" || ex.medico == "13031" || ex.medico == "02310" || ex.medico == "02311" || ex.medico == "02312"
                                  //|| ex.medico == "02290"
                                  where ex.ip_realiza == "148.234.143.169" || ex.ip_realiza == "148.234.143.217" || ex.ip_realiza == "148.234.143.151" || ex.ip_realiza == "148.234.143.176" || ex.ip_realiza == "148.234.143.185"
                                  //where ex.fecha >= fecha2DT
                                  where ex.fecha >= fecha_minDate
                                  where ex.fecha <= fecha_maxDate
                                  where ex.hora_termino != null
                                  join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                  from medicosIn in mediX.DefaultIfEmpty()
                                  join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                  from especiIn in especiX.DefaultIfEmpty()
                                  select new
                                  {
                                      medico = ex.medico,
                                      especialidad = "Unidad ER",
                                      nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                      consultadistancia = ex.consultadistancia,
                                      //fecha = ex.fecha
                                  })
                               .ToList().Select(x => new
                               {
                                   medico = x.medico,
                                   nombre = x.nombre,
                                   especialidad = x.especialidad,
                                   consultadistancia = x.consultadistancia,
                                   //fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                               })
                               .ToList().Select(z => new
                               {
                                   medico = z.medico,
                                   nombre = z.nombre,
                                   especialidad = z.especialidad,
                                   consultadistancia = z.consultadistancia,
                                   //fecha = z.fecha
                               })
                               .GroupBy(p => new
                               {
                                   p.nombre,
                                   p.medico,
                                   p.especialidad,
                                   //p.fecha
                               })
                              .Select(g => new
                              {
                                  medico = g.Key.medico,
                                  nombre = g.Key.nombre,
                                  especialidad = g.Key.especialidad,
                                  fecha = textoFecha,
                                  count = g.Count(),
                                  telefonica = g.Count(p => p.consultadistancia == "1"),
                                  presencial = g.Count(p => p.consultadistancia != "1"),
                              })
                              .OrderBy(g => g.medico)
                              .ToList();

            //System.Diagnostics.Debug.WriteLine(expediedet_uer2);

            var resultsDetUER = new List<IndEspListDet>();

                foreach (var item in expediedet_uer)
                {

                    //Orden de internamiento
                    var ordenCount = (from a in db.Orden_Int
                                      where a.medico == item.medico
                                      where a.fecha_registro >= fecha_minDate
                                      where a.fecha_registro <= fecha_maxDate
                                      select a).Count();

                    var resultUER = new IndEspListDet
                    {
                        medico = item.medico,
                        nombre = item.nombre,
                        especialidad = item.especialidad,
                        fecha = item.fecha,
                        count = item.count,
                        telefonica = item.telefonica,
                        presencial = item.presencial,
                        ordenint = ordenCount,
                    };

                    resultsDetUER.Add(resultUER);
                }
            
                string json2_uer = JsonConvert.SerializeObject(resultsDetUER);
                string path2_uer = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(path2_uer + "indicadores/especialidades/detalles_unidader.json", json2_uer);



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
                                  where ex.fecha >= fecha_minDate
                                  where ex.fecha <= fecha_maxDate
                                  where ex.hora_termino != null
                                  //Subrogados
                                  where ex.medico == item.numero
                                  select ex)
                                  .Count();

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.numero
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();

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
                        fecha = textoFecha,
                        count = expediente,
                        telefonica = 0,
                        presencial = expediente,
                        ordenint = ordenCount,
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
                        fecha = textoFecha,
                        count = 0,
                        telefonica = 0,
                        presencial = 0,
                        ordenint = ordenCount,
                    };

                    resultsDetSub.Add(resultSub);
                }




            }

            string jsonSub = JsonConvert.SerializeObject(resultsDetSub);
            string pathSub = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathSub + "indicadores/especialidades/detallessubrogados.json", jsonSub);


            #endregion

            #region  "Ciudad Universitaria"
            //Unidad ER

            var fechaCU = DateTime.Now.ToString("2021-10-13T00:00:00.000");
            var fechaCUDT = DateTime.Parse(fechaCU);

            //System.Diagnostics.Debug.WriteLine(fechaCUDT);
            //System.Diagnostics.Debug.WriteLine(fecha_minDate);

            var textoFechaCU = "";
            if (fecha_minDate < fechaCUDT)
            {
                textoFechaCU = "De: 2021-10-13 Hasta: " + maxDate;

                var expCU = (from ex in db.expediente
                              where ex.ip_realiza.StartsWith("148.234.218")
                             where ex.medico != "02101"
                             //where ex.medico == "22023" || ex.medico == "02235" || ex.medico == "62003" || ex.medico == "08055" || ex.medico == "05038" || ex.medico == "02294"
                             where ex.fecha >= fechaCUDT
                              //where ex.fecha >= fecha_minDate
                              where ex.fecha <= fecha_maxDate
                              where ex.hora_termino != null
                              join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                              from medicosIn in mediX.DefaultIfEmpty()
                              join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                              from especiIn in especiX.DefaultIfEmpty()
                              select new
                              {
                                  medico = ex.medico,
                                  especialidad = especiIn.DESCRIPCION,
                                  nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  consultadistancia = ex.consultadistancia
                              })
                              .ToList().Select(x => new
                              {
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = textoFechaCU,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             .OrderBy(g => g.medico)
                             .ToList();


                var resultsDetCU = new List<IndEspListDet>();

                foreach (var item in expCU)
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


                string jsonCU = JsonConvert.SerializeObject(resultsDetCU);
                string pathCU = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(pathCU + "indicadores/especialidades/detalles_cu.json", jsonCU);

            }
            else
            {
                textoFechaCU = textoFecha;
                //System.Diagnostics.Debug.WriteLine(fecha_minDate);

                var expCU = (from ex in db.expediente
                             where ex.ip_realiza.StartsWith("148.234.218")
                             where ex.medico != "02101"
                             //where ex.medico == "22023" || ex.medico == "02235" || ex.medico == "62003" || ex.medico == "08055" || ex.medico == "05038" || ex.medico == "02294"
                             where ex.fecha >= fecha_minDate
                             where ex.fecha <= fecha_maxDate
                             where ex.hora_termino != null
                             join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                             from medicosIn in mediX.DefaultIfEmpty()
                             join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                             from especiIn in especiX.DefaultIfEmpty()
                             select new
                              {
                                  medico = ex.medico,
                                  especialidad = especiIn.DESCRIPCION,
                                  nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  consultadistancia = ex.consultadistancia
                              })
                              .ToList().Select(x => new
                              {
                                  medico = x.medico,
                                  nombre = x.nombre,
                                  especialidad = x.especialidad,
                                  consultadistancia = x.consultadistancia
                              })
                              .ToList().Select(z => new
                              {
                                  medico = z.medico,
                                  nombre = z.nombre,
                                  especialidad = z.especialidad,
                                  consultadistancia = z.consultadistancia
                              })
                              .GroupBy(p => new
                              {
                                  p.nombre,
                                  p.medico,
                                  p.especialidad
                              })
                             .Select(g => new
                             {
                                 medico = g.Key.medico,
                                 nombre = g.Key.nombre,
                                 especialidad = g.Key.especialidad,
                                 fecha = textoFechaCU,
                                 count = g.Count(),
                                 telefonica = g.Count(p => p.consultadistancia == "1"),
                                 presencial = g.Count(p => p.consultadistancia != "1"),
                             })
                             .OrderBy(g => g.medico)
                             .ToList();


                var resultsDetCU = new List<IndEspListDet>();

                foreach (var item in expCU)
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


                string jsonCU = JsonConvert.SerializeObject(resultsDetCU);
                string pathCU = Server.MapPath("~/json/");
                System.IO.File.WriteAllText(pathCU + "indicadores/especialidades/detalles_cu.json", jsonCU);
            }

            #endregion

            #region "Foraneos"

            var expeFor2 = (from ex in db.MEDICOS
                            //Foraneos
                            join especi in db.ESPECIALIDADES on ex.Numero.Substring(0, 2) equals especi.CLAVE into especiX
                            from especiIn in especiX.DefaultIfEmpty()
                            where ex.Numero != "02101"
                            where ex.Numero == "02319" || ex.Numero == "02316" || ex.Numero == "02318" || ex.Numero == "02317" || ex.Numero == "38126" 
                            || ex.Numero == "02321" || ex.Numero == "02324" || ex.Numero == "38128" || ex.Numero == "38129" || ex.Numero == "38127" || ex.Numero == "02347"
                            //Montemorelos
                            || ex.Numero == "03139" || ex.Numero == "05041" || ex.Numero == "08058" || ex.Numero == "08059" || ex.Numero == "19018" || ex.Numero == "03140"
                            || ex.Numero == "02334" || ex.Numero == "13032" /*|| ex.Numero == "41026"*/ || ex.Numero == "06026" || ex.Numero == "02340"
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

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.numero
                                  where a.fecha_registro >= fecha_minDate
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();

                var expediente = (from ex in db.expediente
                                  where ex.fecha >= fecha_minDate
                                  where ex.fecha <= fecha_maxDate
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

                //Sabinas
                //var count1 = 0;
                if (item.numero == "03135" || item.numero == "02324")
                {
                    var expediente2 = (from ex in db.expediente
                                       where ex.fecha >= fecha_minDate
                                       where ex.fecha <= fecha_maxDate
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

                // Montemorelos
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
                        fecha = textoFecha,
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
                        fecha = textoFecha,
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

            #region  "Unidad ER SEMAC"

            //Unidad ER SEMAC

            var fecha3 = DateTime.Now.ToString("2022-02-01T00:00:00.000");
            var fecha3String = DateTime.Now.ToString("2022-02-01");
            var fechaDT3 = DateTime.Parse(fecha3);
            DateTime fechafinal;

            if (fecha_minDate <= fechaDT3)
            {
                //TOMA EL VALOR DE LA FECHA DESDE QUE EMPEZO EN LA UNIDAD
                fechafinal = fechaDT3;

                textoFecha = "De: " + fecha3String + " Hasta: " + maxDate;
            }
            else
            {
                fechafinal = fecha_minDate;
                if(minDate == maxDate)
                {
                    textoFecha = minDate;
                }
                else
                {
                    textoFecha = "De: " + minDate + " Hasta: " + maxDate;
                }
                
            }

            var uer_semac = (from ex in db.expediente
                             where ex.medico == "02327"
                                  //where ex.fecha >= fecha2DT
                                  where ex.fecha >= fechafinal
                                  where ex.fecha <= fecha_maxDate
                                  where ex.hora_termino != null
                                  join medicos in db.MEDICOS on ex.medico equals medicos.Numero into mediX
                                  from medicosIn in mediX.DefaultIfEmpty()
                                  join especi in db.ESPECIALIDADES on ex.medico.Substring(0, 2) equals especi.CLAVE into especiX
                                  from especiIn in especiX.DefaultIfEmpty()
                                  select new
                                  {
                                      medico = ex.medico,
                                      especialidad = "Unidad ER SEMAC",
                                      nombre = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                      consultadistancia = ex.consultadistancia,
                                      //fecha = ex.fecha
                                  })
                               .ToList().Select(x => new
                               {
                                   medico = x.medico,
                                   nombre = x.nombre,
                                   especialidad = x.especialidad,
                                   consultadistancia = x.consultadistancia,
                                   //fecha = string.Format("{0:yyyy-MM-dd}", x.fecha),
                               })
                               .ToList().Select(z => new
                               {
                                   medico = z.medico,
                                   nombre = z.nombre,
                                   especialidad = z.especialidad,
                                   consultadistancia = z.consultadistancia,
                                   //fecha = z.fecha
                               })
                               .GroupBy(p => new
                               {
                                   p.nombre,
                                   p.medico,
                                   p.especialidad,
                                   //p.fecha
                               })
                              .Select(g => new
                              {
                                  medico = g.Key.medico,
                                  nombre = g.Key.nombre,
                                  especialidad = g.Key.especialidad,
                                  fecha = textoFecha,
                                  count = g.Count(),
                                  telefonica = g.Count(p => p.consultadistancia == "1"),
                                  presencial = g.Count(p => p.consultadistancia != "1"),
                              })
                              .OrderBy(g => g.medico)
                              .ToList();

            //System.Diagnostics.Debug.WriteLine(expediedet_uer2);

            var uerSEMAC = new List<IndEspListDet>();

            foreach (var item in uer_semac)
            {

                //Orden de internamiento
                var ordenCount = (from a in db.Orden_Int
                                  where a.medico == item.medico
                                  where a.fecha_registro >= fechafinal
                                  where a.fecha_registro <= fecha_maxDate
                                  select a).Count();

                var uerSEMACList = new IndEspListDet
                {
                    medico = item.medico,
                    nombre = item.nombre,
                    especialidad = item.especialidad,
                    fecha = item.fecha,
                    count = item.count,
                    telefonica = item.telefonica,
                    presencial = item.presencial,
                    ordenint = ordenCount,
                };

                uerSEMAC.Add(uerSEMACList);
            }

            string json_uersemac = JsonConvert.SerializeObject(uerSEMAC);
            string path_uersemac = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path_uersemac + "indicadores/especialidades/detalles_uersemac.json", json_uersemac);



            #endregion

            #region "Cirugia"


            //SELECT AspNetUsers.UserName FROM AspNetUsers INNER JOIN AspNetUserRoles ON AspNetUserRoles.UserId = AspNetUsers.Id where AspNetUserRoles.RoleId = 72

            string query = "SELECT AspNetUsers.UserName as medico , MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno AS nombre, ESPECIALIDADES.DESCRIPCION as especialidad FROM AspNetUsers INNER JOIN AspNetUserRoles ON AspNetUserRoles.UserId = AspNetUsers.Id INNER JOIN MEDICOS ON MEDICOS.Numero = AspNetUsers.UserName INNER JOIN ESPECIALIDADES ON ESPECIALIDADES.CLAVE = SUBSTRING(AspNetUsers.UserName, 1, 2) where AspNetUserRoles.RoleId = 72 and AspNetUsers.UserName != 02101";
            var result = db.Database.SqlQuery<CirugiaList>(query);
            var cirugias = result.ToList();

            var resultsDetCiru = new List<IndEspListDet>();

            foreach (var item in cirugias)
            {

                var notaquiru = (from ex in db.NotaQuirurgica
                                 join orden in db.Orden_Int on ex.id_orden equals orden.id_orden into ordenX
                                 from ordenIn in ordenX.DefaultIfEmpty()
                                     //join medico in db.MEDICOS on ordenIn.medico equals medico.Numero into medicoX
                                     //from medicoIn in medicoX.DefaultIfEmpty()
                                 where ex.fecha_registro >= fecha_minDate
                                 where ex.fecha_registro <= fecha_maxDate
                                 //where ex.hora_termino != null
                                 //Subrogados
                                 where ordenIn.medico == item.medico
                                 select ex)
                              .Count();

                var lugar = "";
                var total = 0;
                var espe = "";

                if (notaquiru != 0)
                {
                    var resultCiru = new IndEspListDet
                    {
                        medico = item.medico,
                        nombre = item.nombre,
                        especialidad = item.especialidad,
                        fecha = textoFecha,
                        count = notaquiru,
                        lugar = lugar,
                        telefonica = 0,
                        presencial = notaquiru,
                    };

                    resultsDetCiru.Add(resultCiru);
                }
                else
                {
                    var resultCiru = new IndEspListDet
                    {
                        medico = item.medico,
                        nombre = item.nombre,
                        especialidad = item.especialidad,
                        fecha = textoFecha,
                        count = 0,
                        lugar = lugar,
                        telefonica = 0,
                        presencial = 0,
                    };

                    resultsDetCiru.Add(resultCiru);
                }




            }

            //System.Diagnostics.Debug.WriteLine(resultsDetCiru);

            string jsonCiru = JsonConvert.SerializeObject(resultsDetCiru);
            string pathCiru = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(pathCiru + "indicadores/especialidades/cirugias.json", jsonCiru);


            #endregion


            return new JsonResult { Data = expediedet, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult IndicadoresCount()
        {
            var fecha_hoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta_hoy = DateTime.Parse(fecha_hoy);
            SMDEVEntities18 db = new SMDEVEntities18();

            var receta_normal = (from rn in db.RECETA_EXP_2
                                 where rn.Manual == null
                                 where rn.folio_rc == null
                                 where rn.Hora_Rcta != null
                                 select rn).Count();

            var tarjeton = (from t in db.RECETA_EXP_2
                            where t.Manual == null
                            where t.folio_rc != null
                            where t.Hora_Rcta != null
                            select t).Count();

            var manuales = (from m in db.RECETA_EXP_2
                            where m.Manual != null
                            select m).Count();

            var receta_normal_hoy = (from rnh in db.RECETA_EXP_2
                                     where rnh.Manual == null
                                     where rnh.folio_rc == null
                                     where rnh.Hora_Rcta >= fecha_correcta_hoy
                                     where rnh.Hora_Rcta != null
                                     select rnh).Count();

            var tarjeton_hoy = (from th in db.RECETA_EXP_2
                                where th.Manual == null
                                where th.folio_rc != null
                                where th.Hora_Rcta >= fecha_correcta_hoy
                                where th.Hora_Rcta != null
                                select th).Count();

            var manuales_hoy = (from mh in db.RECETA_EXP_2
                                where mh.Manual != null
                                where mh.Fecha >= fecha_correcta_hoy
                                select mh).Count();

            var expediente_mf = (from emf in db.expediente
                                 where emf.medico.StartsWith("02")
                                 where emf.hora_termino != null
                                 select emf).Count();

            var expediente_pe = (from epe in db.expediente
                                 where epe.medico.StartsWith("03")
                                 where epe.hora_termino != null
                                 select epe).Count();

            var expediente_es = (from ees in db.expediente
                                 where ees.medico.StartsWith("03") == false
                                 where ees.medico.StartsWith("02") == false
                                 where ees.hora_termino != null
                                 select ees).Count();

            var expediente_mf_hoy = (from emfh in db.expediente
                                     where emfh.medico.StartsWith("02")
                                     where emfh.hora_termino != null
                                     where emfh.hora_termino >= fecha_correcta_hoy
                                     select emfh).Count();

            var expediente_pe_hoy = (from epeh in db.expediente
                                     where epeh.medico.StartsWith("03")
                                     where epeh.hora_termino != null
                                     where epeh.hora_termino >= fecha_correcta_hoy
                                     select epeh).Count();

            var expediente_es_hoy = (from eesh in db.expediente
                                     where eesh.medico.StartsWith("03") == false
                                     where eesh.medico.StartsWith("02") == false
                                     where eesh.hora_termino != null
                                     where eesh.hora_termino >= fecha_correcta_hoy
                                     select eesh).Count();

            Models.SMDEVEntities29 db2 = new Models.SMDEVEntities29();

            var expediente_hu = (from ehu in db2.expediente
                                 where ehu.ip_realiza == "148.234.143.203" || ehu.ip_realiza == "148.234.140.9"
                                 where ehu.hora_termino != null
                                 select ehu).Count();

            var expediente_mederos_telefonica = (from emt in db2.expediente
                                                 where emt.ip_realiza.StartsWith("148.234.64")
                                                 where emt.consultadistancia == "0"
                                                 where emt.hora_termino != null
                                                 select emt).Count();

            var expediente_mederos_presencial = (from emp in db2.expediente
                                                 where emp.ip_realiza.StartsWith("148.234.64")
                                                 where emp.consultadistancia != "0"
                                                 where emp.hora_termino != null
                                                 select emp).Count();

            var expediente_sm_telefonica = (from esmt in db2.expediente
                                            where !esmt.ip_realiza.StartsWith("148.234.64")
                                            where esmt.ip_realiza != "148.234.143.203"
                                            where esmt.ip_realiza != "148.234.140.9"
                                            where esmt.consultadistancia == "1"
                                            where esmt.hora_termino != null
                                            select esmt).Count();

            var expediente_sm_presencial = (from esmp in db2.expediente
                                            where !esmp.ip_realiza.StartsWith("148.234.64")
                                            where esmp.ip_realiza != "148.234.143.203"
                                            where esmp.ip_realiza != "148.234.140.9"
                                            where esmp.consultadistancia != "1"
                                            where esmp.hora_termino != null
                                            select esmp).Count();


            var result = new {
                receta_normal_total = receta_normal,
                tarjeton_total = tarjeton,
                manuales_total = manuales,
                total_recetas = receta_normal + tarjeton + manuales,


                total_recetas_hoy = receta_normal_hoy + tarjeton_hoy + manuales_hoy,
                receta_normal_hoy_total = receta_normal_hoy,
                tarjeton_hoy_total = tarjeton_hoy,
                manuales_hoy_total = manuales_hoy,


                total_consultas = expediente_mf + expediente_pe + expediente_es,
                total_consultas_hoy = expediente_mf_hoy + expediente_pe_hoy + expediente_es_hoy,

                total_expediente_hu = expediente_hu,

                total_expediente_mederos = expediente_mederos_telefonica + expediente_mederos_presencial,

                total_expediente_sm = expediente_sm_telefonica + expediente_sm_presencial,
            };

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Monitores()
        {
            return View();
        }

        public ActionResult Direccion()
        {
            return View();
        }

        public ActionResult Farmacia()
        {
            return View();
        }

        public ActionResult Incapacidades(string expediente) {
            DalIncapacidades Inc = new DalIncapacidades();
            ViewData["listaDig"] = Inc.DiagnoticoCMB();
            ViewData["listaDep"] = Inc.DependenciasCMB();

            ViewData["listaFrm"] = Inc.frmIncapacidad(expediente);
            return View();
        }

        /*Consultas*/
        public ActionResult Consultas()
        {
            return View();
        }

        public JsonResult GetMedicoNombres()
        {
            var nombreusuario = User.Identity.GetUserName();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var medico_info = (from a in db.MEDICOS
                               where a.Numero == nombreusuario
                               //orderby a.medico descending
                               select a).FirstOrDefault();

            if (medico_info != null)
            {
                var especialidades = (from b in db.ESPECIALIDADES
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
                string query = "SELECT NOMBRE AS NOMBRE FROM ACCESO WHERE USUARIO = '" + nombreusuario + "'";
                var result = db.Database.SqlQuery<Accesos>(query);
                var res = result.FirstOrDefault();

                var resultado = new
                {
                    nombres = res.NOMBRE,
                    numero = "",
                    apellido_paterno = "",
                    apellido_materno = "",
                    cedula = "",
                    cedula_esp = "",
                    especialidad = ""
                };

                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }

        public ActionResult HU()
        {
            // var today = DateTime.Today.Year;

            // Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            // var dhabientes = (from q in db.DHABIENTES
            //                   where (q.BAJA != "01" && q.BAJA != "03" && q.NUMAFIL != "" && q.NUMAFIL != null)

            //                   select new
            //                   {
            //                       NUMEMP = q.NUMEMP,
            //                       NOMBRES = q.NOMBRES,
            //                       APATERNO = q.APATERNO,
            //                       AMATERNO = q.AMATERNO,
            //                       SEXO = q.SEXO,
            //                       FNAC = q.FNAC,
            //                       FECALTA = q.FECALTA,
            //                       FBAJA = q.FBAJA
            //                   })
            //                   .ToList().Select(x => new
            //                   {
            //                       NUMEMP = x.NUMEMP,
            //                       NOMBRES = x.NOMBRES,
            //                       APATERNO = x.APATERNO,
            //                       AMATERNO = x.AMATERNO,
            //                       SEXO = x.SEXO,
            //                       FNAC = string.Format("{0:yyyy}", x.FNAC),
            //                       FECALTA = x.FECALTA,
            //                       FBAJA = x.FBAJA
            //                   })
            //                   .ToList();



            //   DalHojaFrontal dalObj = new DalHojaFrontal();
            ////  string json = JsonConvert.SerializeObject(dhabientes);

            //string json = JsonConvert.SerializeObject(dalObj.BuscarHU());
            // string path = Server.MapPath("~/json/");
            // System.IO.File.WriteAllText(path + "dhabientes.json", json);

            return View();
        }

        public ActionResult BsqHU(string v_NUMEMP, string v_nombres, string v_apellido, string v_apellidoM)
        {
            var today = DateTime.Today.Year;



            DalHojaFrontal dalObj = new DalHojaFrontal();
            //  string json = JsonConvert.SerializeObject(dhabientes);



            return new JsonResult { Data = dalObj.BuscarHU(v_NUMEMP, v_nombres, v_apellido, v_apellidoM), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult GrillaIncapcidades(string v_dig, List<string> v_dep, string v_ini, string v_fin)
        {

            DalIncapacidades dalObj = new DalIncapacidades();

            return new JsonResult { Data = dalObj.BuscarExel1(v_dig, v_dep ?? new List<string>(), v_ini, v_fin), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public void RpExcel()
        {


            try
            {
                List<string> noo = new List<string>();
                //  noo.Add("Concepto");




                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment;filename=Excel.xls");
                Response.AddHeader("Content-Type", "application/excel");

                GridView grid = new GridView();
                StringWriter stringWriter = new StringWriter();
                HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);
                Dalexcel exe1 = new Dalexcel();
                DataTable dts = ConvertToDataTable((exe1.BuscarExel1()), noo);



                // TITULO
                //  htmlTextWriter.Write("<div> </div> <table border='1' style='margin-top: 10px;'  >    <thead>   <tr><th style='background-color: #ddd;' rowspan='5' colspan='14'><img src='http://www.hardsoft.com.ar/images/logo.png'></th></tr>  </thead></table> ");

                grid.DataSource = dts;
                grid.DataBind();
                grid.RenderControl(htmlTextWriter);
                Response.Write(stringWriter.ToString());

                htmlTextWriter.Close();
                stringWriter.Close();


                Response.End();


            }
            catch (Exception)
            {

                throw;
            }


        }

        public static System.Data.DataTable ConvertToDataTable<T>(IList<T> data, List<string> listNo)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            foreach (PropertyDescriptor prop in properties)
            {
                string s = "n";
                foreach (var item in listNo)
                {
                    if (prop.Name == item)
                    {
                        s = "s";
                    }
                }
                if (s == "n")
                {
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }

            foreach (T item in data)
            {
                string s = "n";
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    s = "n";
                    foreach (var item2 in listNo)
                    {
                        if (prop.Name == item2)
                        {
                            s = "s";
                        }
                    }
                    if (s == "n")
                    {
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    }
                }

                table.Rows.Add(row);

            }
            return table;

        }


        public ActionResult BuscarHUHistorial(string v_NUMEMP, string v_nombres, string v_apellido, string v_apellidoM)
        {
            var today = DateTime.Today.Year;

            var nombreusuario = User.Identity.GetUserName();

            DalHojaFrontal dalObj = new DalHojaFrontal();
            //  string json = JsonConvert.SerializeObject(dhabientes);



            return new JsonResult { Data = dalObj.BuscarHUHistorial(v_NUMEMP, v_nombres, v_apellido, v_apellidoM, nombreusuario), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

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
        public ActionResult ConsultaExpediente(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //var host = Dns.GetHostEntry(Dns.GetHostName());
                /*foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        string ip_realiza = ip.ToString();
                    }
                    else
                    {
                        string ip_realiza = "";
                    }
                }*/


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

        public JsonResult GetLastSoap(string search)
        {
            Models.SMDEVEntities29 db = new Models.SMDEVEntities29();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();

            var last_soap = (from r in db.expediente
                             where r.numemp == search
                             where r.medico == nombreusuario
                             join diagnosticoNombre in db.DIAGNOSTICOS on r.diagnostico equals diagnosticoNombre.Clave
                             join diagnostico2Nombre in db.DIAGNOSTICOS on r.diagnostico2 equals diagnostico2Nombre.Clave
                             join diagnostico3Nombre in db.DIAGNOSTICOS on r.diagnostico3 equals diagnostico3Nombre.Clave
                             where r.fecha >= fecha_correcta
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
                                 s_exp = r.s_exp,
                                 o_exp = r.o_exp,
                                 diagnostico = r.diagnostico,
                                 diagnosticoNombre = diagnosticoNombre.DesCorta,
                                 diagnostico2 = r.diagnostico2,
                                 diagnostico2Nombre = diagnostico2Nombre.DesCorta,
                                 diagnostico3 = r.diagnostico3,
                                 diagnostico3Nombre = diagnostico3Nombre.DesCorta,
                                 tipconsulta = r.tipconsulta,
                                 tipconsulta2 = r.tipconsulta2,
                                 tipconsulta3 = r.tipconsulta3,
                                 p_exp = r.p_exp,
                                 hora_termino = r.hora_termino,
                                 referido = r.referido,
                                 ref_exp = r.ref_exp,
                                 referido_urg = r.referido_urg,
                                 consultadistancia = r.consultadistancia,
                             })
                          .ToList();

            //System.Diagnostics.Debug.WriteLine(last_soap.s_exp);
            if (last_soap == null)
            {
                var sinresultado = ' ';
                return new JsonResult { Data = sinresultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult { Data = last_soap, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        [HttpPost]
        public ActionResult GuardarExpediente(expediente model)
        {
            //System.Diagnostics.Debug.WriteLine(model.numemp);
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            //string ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            //System.Diagnostics.Debug.WriteLine(ip);

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

                return RedirectToAction("Recetas/" + model.numemp, "Manage");
            }

            //return Redirect(Request.UrlReferrer.ToString());
        }



        [HttpPost]
        public ActionResult GuardarReceta(Receta_Exp model, Receta_Detalle model2)
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var dir_ip = GetLocalIPAddress();

            //var dir_ip = Request.UserHostAddress;

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            var receta_hoy = (from a in db.Receta_Exp
                              where a.Expediente == model.Expediente
                              where a.Fecha == fecha_correcta
                              where a.Hora_Rcta == null
                              select a).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(receta_hoy);

            if (receta_hoy == null)
            {
                db.Database.ExecuteSqlCommand("INSERT INTO Receta_Exp (Expediente, Medico, Estado, Dir_Ip, Fecha) VALUES('" + model.Expediente + "','" + model.Medico + "','" + 9 + "','" + dir_ip + "','" + fecha + "')");
            }

            //Tomar el Folio_Rcta de la ultima receta con el mismo expediente y medico
            var ultima_receta_folio = db.Receta_Exp.Where(v => v.Medico == model.Medico).Where(v => v.Expediente == model.Expediente).Where(v => v.Estado == "9").Where(v => v.Hora_Rcta == null).OrderByDescending(v => v.Fecha)
            .Select(ultima_folio => new
            {
                Folio_Rcta = ultima_folio.Folio_Rcta,
            }).FirstOrDefault();

            //Revisar si ese medicamento ya existe en la receta
            var receta_detalles_hoy = (from a in db.Receta_Detalle
                                       where a.Codigo == model2.Codigo
                                       where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                       select a).FirstOrDefault();

            var receta_detalles_hoy_count = (from a in db.Receta_Detalle
                                                 //where a.Codigo == model2.Codigo
                                             where a.Folio_Rcta == ultima_receta_folio.Folio_Rcta
                                             select a).Count();

            //System.Diagnostics.Debug.WriteLine(receta_detalles_hoy_count);


            if (receta_detalles_hoy == null)
            {
                if (receta_detalles_hoy_count < 3)
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES('" + ultima_receta_folio.Folio_Rcta + "','" + model2.Codigo + "','" + model2.Cantidad + "','" + model2.Dosis + "','" + model2.Indicaciones + "')");
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
                db.Database.ExecuteSqlCommand("UPDATE Receta_Detalle SET Codigo = '" + model2.Codigo + "', Cantidad = '" + model2.Cantidad + "', Dosis = '" + model2.Dosis + "', Indicaciones = '" + model2.Indicaciones + "' WHERE Folio_Rcta = '" + receta_detalles_hoy.Folio_Rcta + "' AND Codigo = '" + receta_detalles_hoy.Codigo + "'");
                TempData["message_success"] = "Medicamento editado con éxito";
            }
            //return RedirectToAction("Recetas/" + model.numemp, "Manage");
            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        public ActionResult TerminarReceta(Receta_Exp model)
        {
            //System.Diagnostics.Debug.WriteLine(model.Folio_Rcta);
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha__actual_correcta = DateTime.Parse(fecha_actual);
            db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '7' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");
            TempData["message_success"] = "¡Receta terminada con éxito!";
            return RedirectToAction("ListaRecetas/" + model.Expediente, "Manage");
            //return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult NuevaReceta(Receta_Exp model)
        {
            //System.Diagnostics.Debug.WriteLine(model.Folio_Rcta);
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha__actual_correcta = DateTime.Parse(fecha_actual);
            db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");
            //TempData["message_success"] = "¡Receta terminada con éxito!";
            return Redirect(Request.UrlReferrer.ToString());
        }


        public ActionResult HojaFrontal(string id)
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

        [HttpGet]
        public ActionResult HistorialPadecimientos(string id)
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


        [HttpGet]
        public ActionResult HistorialRecetas(string id)
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

                Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();

                var recetas = (from q in db2.Receta_Exp
                               where q.Expediente == id
                               //join detalles in db2.Receta_Detalle on q.Folio_Rcta equals detalles.Folio_Rcta
                               /*join diagnosticos1 in db3.DIAGNOSTICOS on q.diagnostico equals diagnosticos1.Clave
                               join diagnosticos2 in db3.DIAGNOSTICOS on q.diagnostico2 equals diagnosticos2.Clave
                               join diagnosticos3 in db3.DIAGNOSTICOS on q.diagnostico3 equals diagnosticos3.Clave
                               join espe in db3.ESPECIALIDADES on q.medico.Substring(0, 2) equals espe.CLAVE*/
                               //where q.Medico == "30001"
                               select new
                               {
                                   Folio_Rcta = q.Folio_Rcta,
                                   Expediente = q.Expediente,
                                   Medico = q.Medico,
                                   Turno = q.Turno,
                                   Estado = q.Estado,
                                   Manual = q.Manual,
                                   folio_rc = q.folio_rc,
                                   UsuarioId = q.UsuarioId,
                                   UserFar = q.UserFar,
                                   Fecha = q.Fecha,
                               })
                                    .OrderByDescending(g => g.Fecha)
                                    .ToList();


                string json = JsonConvert.SerializeObject(recetas);
                string path = Server.MapPath("~/json/historial_recetas/");
                System.IO.File.WriteAllText(path + "historial_recetas_" + id + ".json", json);

                return View(dhabientes);
            }

        }


        public JsonResult GetMedicoNombresRecetas(string search)
        {
            Models.SMDEVEntities19 db = new Models.SMDEVEntities19();
            var medico_info = (from a in db.MEDICOS
                               where a.Numero == search
                               //orderby a.medico descending
                               select a).FirstOrDefault();

            /*Models.SMDEVEntities22 db2 = new Models.SMDEVEntities22();
            var especialidades = (from b in db2.ESPECIALIDADES
                                  where b.CLAVE == medico_info.Numero.Substring(0, 2)
                                  //orderby a.medico descending
                                  select b).FirstOrDefault();*/

            var resultado = new
            {
                numero = medico_info.Numero,
                nombres = medico_info.Nombre,
                apellido_paterno = medico_info.Apaterno,
                apellido_materno = medico_info.Amaterno,
                cedula = medico_info.Cedula,
                cedula_esp = medico_info.cedula_esp,
                //especialidad = especialidades.DESCRIPCION
            };

            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult SetListDiagnosticos(string search)
        {
            Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();
            var diagnosticos = (from a in db2.DIAGNOSTICOS
                                where a.DescCompleta.Contains(search)
                                select a).ToList();

            string json = JsonConvert.SerializeObject(diagnosticos);
            string path = Server.MapPath("~/json/");
            System.IO.File.WriteAllText(path + "diagnosticos.json", json);

            return new JsonResult { Data = diagnosticos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult BuscarConsulta(string search)
        {
            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();

            var dhabientes = db.DHABIENTES.Where(x => x.NUMEMP.Equals(search) || x.NOMBRES.Equals(search) || x.APATERNO.Equals(search) || x.AMATERNO.Equals(search)).
                    Select(x => "<div class='row m-0 p-2'><div class='col-md-3 m-auto p-1'><img style='width:100px;' src='http://148.234.143.204/smuanl_web/fotos_dhab/" + x.NUMEMP + ".jpg' onerror='imgError(this);'></div><div class='col-md-6 m-auto p-1'><h5>" + x.NOMBRES + " " + x.APATERNO + " " + x.AMATERNO + "</h5></div><div class='col-md-3 m-auto p-1'><a href='/Manage/ConsultaExpediente/" + x.NUMEMP + "'><button class='btn actualizar'>Asignar</button></div></div>").ToList();


            return new JsonResult { Data = dhabientes, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetDiagnosticos(string search)
        {
            Models.SMDEVEntities23 db = new Models.SMDEVEntities23();

            var diagnosticos = db.DIAGNOSTICOS.Where(x => x.DescCompleta.Contains(search)).
                    Select(x => "<div class='col-md-12 m-auto p-2 diag-box'><h5>" + x.DescCompleta + "</h5></div>").ToList();


            return new JsonResult { Data = diagnosticos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
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

                    Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                    var inventariofarmacia = (from q in db2.InventarioFarmacia
                                              where q.Clave_Nivel == "1" || q.Clave_Nivel == "2"
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

                    string json = JsonConvert.SerializeObject(inventariofarmacia);
                    string path = Server.MapPath("~/json/inventario_farmacia/");
                    System.IO.File.WriteAllText(path + "inventario_farmacia_hu.json", json);

                    return View(dhabientes);
                }
                else
                {
                    TempData["numemp"] = exp.numemp;
                    return RedirectToAction("ConsultaExpediente/" + exp.numemp, "Manage");
                }
            }
        }


        [HttpGet]
        public ActionResult ListaRecetas(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
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
                    return View(dhabientes);
                }
                else
                {
                    TempData["numemp"] = exp.numemp;
                    return RedirectToAction("ConsultaExpediente/" + exp.numemp, "Manage");
                }
            }
        }


        public JsonResult GetLastReceta(string search)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();

            var receta = (from r in db.Receta_Exp
                          where r.Medico == nombreusuario
                          where r.Expediente == search
                          where r.Fecha == fecha_correcta
                          where r.unidad_medica != 3
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

        public JsonResult GetAllRecetas(string search)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();

            var receta = (from r in db.Receta_Exp
                          where r.Medico == nombreusuario
                          where r.Expediente == search
                          where r.Fecha == fecha_correcta
                          where r.Estado == "9" || r.Estado == "7"
                          select new
                          {
                              Folio_Rcta = r.Folio_Rcta,
                              Estado = r.Estado,
                              Hora_Rcta = r.Hora_Rcta,
                              Fecha = r.Fecha,
                          })
                          .ToList();

            return new JsonResult { Data = receta, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        public JsonResult GetAllRecetasDetalles(int search)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();

            var receta_detalles = (from r in db.Receta_Detalle
                                   where r.Folio_Rcta == search
                                   select new
                                   {
                                       Folio_Rcta = r.Folio_Rcta,
                                       Codigo = r.Codigo,
                                       Cantidad = r.Cantidad,
                                       Dosis = r.Dosis,
                                       Indicaciones = r.Indicaciones,
                                   })
                          .ToList();

            return new JsonResult { Data = receta_detalles, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


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


        public JsonResult GetNombreMedicamentos(string search)
        {
            Models.SMDEVEntities25 db = new Models.SMDEVEntities25();

            //System.Diagnostics.Debug.WriteLine(search);

            var nombre_medicamento = (from r in db.InventarioFarmacia
                                      where r.Clave == search
                                      select new
                                      {
                                          Clave = r.Clave,
                                          Descripcion_Sal = r.Descripcion_Sal,
                                          Descripcion_Grupo = r.Descripcion_Grupo,
                                          Presentacion = r.Presentacion,
                                      })
                          .ToList();

            var results = new List<Result>();

            foreach (var item in nombre_medicamento)
            {
                Models.SERVMEDEntities4 db6 = new Models.SERVMEDEntities4();

                //revisa en la tabla de receta_exp cuantas se le han dado
                var sustancia = (from q in db6.Sustancia
                                 join grupo in db6.grupo_21 on q.id_grupo_21 equals grupo.id into grupoX
                                 from grupoIn in grupoX.DefaultIfEmpty()
                                 where q.Clave == item.Clave
                                 where q.descripcion_21 != null || q.descripcion_21 != ""
                                 select new
                                 {
                                     Descripcion_21 = q.descripcion_21,
                                     Descripcion_Grupo = grupoIn.descripcion,
                                 }).FirstOrDefault();

                if (sustancia != null)
                {

                    var result = new Result
                    {
                        Clave = item.Clave,
                        Descripcion_Sal = item.Descripcion_Sal,
                        Descripcion_Grupo = item.Descripcion_Grupo,
                        Presentacion = sustancia.Descripcion_21,
                    };

                    results.Add(result);

                }

            }


            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public JsonResult DeleteMedicamento(int folio, string codigo)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var recetas_count = (from r in db.Receta_Detalle
                                 where r.Folio_Rcta == folio
                                 //where r.Codigo == codigo
                                 select r).Count();

            //System.Diagnostics.Debug.WriteLine(recetas_count);

            if (recetas_count > 1)
            {
                db.Database.ExecuteSqlCommand("DELETE FROM Receta_Detalle WHERE Folio_Rcta = " + folio + " AND Codigo = '" + codigo + "' ");
                var claveDeleted = codigo;
                return new JsonResult { Data = claveDeleted, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                db.Database.ExecuteSqlCommand("DELETE FROM Receta_Detalle WHERE Folio_Rcta = " + folio + " AND Codigo = '" + codigo + "' ");
                db.Database.ExecuteSqlCommand("DELETE FROM Receta_Exp WHERE Folio_Rcta = " + folio + "");
                var claveDeleted = codigo;
                return new JsonResult { Data = claveDeleted, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            //return Redirect(Request.UrlReferrer.ToString());
        }

        public JsonResult CheckLastReceta(string expediente, string codigo)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            var fecha_tres_meses = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_tres_meses_correcta = DateTime.Parse(fecha);
            var nombreusuario = User.Identity.GetUserName();
            var receta_info = "";

            var receta = (from r in db.Receta_Exp
                          where r.Expediente == expediente
                          where r.Fecha >= fecha_tres_meses_correcta
                          //where r.Estado == "2" || r.Estado == "3"
                          select new
                          {
                              Folio_Rcta = r.Folio_Rcta,
                              Fecha = r.Fecha,
                          }).ToList();

            foreach (var item in receta)
            {
                var receta_detalles = (from d in db.Receta_Detalle
                                       where d.Folio_Rcta == item.Folio_Rcta
                                       where d.Codigo == codigo
                                       select d).FirstOrDefault();


                if (receta_detalles != null)
                {
                    var split = receta_detalles.Dosis.Split(' ');
                    //var tomar = Int64.Parse(split[1]);
                    //var horas = Int64.Parse(split[3]);
                    //var dias = Int64.Parse(split[6]);
                    //var horasDiv = 24 / horas;
                    //var cantidadtotal = tomar * dias * horasDiv;
                    receta_info = "Este medicamento se le ha recetado al paciente el día " + item.Fecha.ToString("yyyy-MM-dd");
                    return new JsonResult { Data = receta_info, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult { Data = receta_info, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            return new JsonResult { Data = receta_info, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpGet]
        public ActionResult ConsultaHU(string id)
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

                return View(dhabientes);
            }
        }


        public JsonResult CheckSoapComplete(string expediente)
        {
            Models.SMDEVEntities24 db = new Models.SMDEVEntities24();
            var nombreusuario = User.Identity.GetUserName();
            var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha = DateTime.Parse(fecha_string);

            var exp = (from a in db.expediente
                           //where a.numemp == expediente
                       where a.medico == nombreusuario
                       where a.hora_termino == null
                       where a.fecha == fecha
                       select a).FirstOrDefault();

            if (exp != null)
            {
                var resultado = new
                {
                    numemp = exp.numemp,
                };

                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                var resultado = "";
                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            //System.Diagnostics.Debug.WriteLine(resultado.numemp);
        }

        //Mostrar incidentes dependiento del coordinador
        public ActionResult Incidentes()
        {
            Models.incidentesEntities3 db = new Models.incidentesEntities3();
            var nombreusuario = User.Identity.GetUserName();

            var user = UserManager.FindById(User.Identity.GetUserId());
            var emailusuario = user.Email.ToString();

            var area_inci = (from a in db.sis_areas
                             where a.EMAIL == emailusuario || a.emailcoordmed == emailusuario
                             select a).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(area_inci.descripcion);

            //var tipo_usuario = 0;

            if (area_inci == null)
            {
                if (nombreusuario == "jorge.gutierrez" || nombreusuario == "direccion" || nombreusuario == "arturo.martinez" || nombreusuario == "coordinacion")
                {


                    var inci = (from a in db.SOL_SERVICIO
                                    //join areaIncidente in db.sis_areas on emailusuario equals areaIncidente.emailcoordmed into areaX
                                    //where a.fecha_cierre == null
                                join areaIncidente in db.sis_areas on a.area_incidente equals areaIncidente.id_area into areaX
                                join tipoIncidente in db.tipo_incidente on a.tipo_incidente equals tipoIncidente.clave into tipoX
                                from areaIn in areaX.DefaultIfEmpty()
                                from tipoIn in tipoX.DefaultIfEmpty()
                                orderby a.fecha_incidente descending
                                select new
                                {
                                    fecha_incidente = a.fecha_incidente,
                                    expediente = a.expediente,
                                    area_incidente = areaIn.descripcion,
                                    tipoarea_incidente = areaIn.tipo_area,
                                    emailcoordmed = areaIn.emailcoordmed,
                                    emailjefe = areaIn.EMAIL,
                                    area = a.area_incidente,
                                    jefe = areaIn.COORDINADOR,
                                    coordinador = areaIn.coordmedico,
                                    tipo_incidente = tipoIn.descripcion,
                                    persona_reporta = a.persona_reporta,
                                    folio = a.folio,
                                    desc_sol = a.DESC_SOL,
                                    fecha_seguimiento = a.fecha_seguimiento,
                                    emp_asignado = a.emp_asignado,
                                    seguimiento = a.seguimiento,
                                    seguimiento_jefe = a.seguimiento_jefe,
                                    jefe_asignado = a.jefe_asignado,
                                    tipo_area = areaIn.tipo_area,
                                    fecha_coord_notifica = a.fecha_coord_notifica,
                                    fecha_cierre = a.fecha_cierre,
                                    fecha_seg_jefe = a.fecha_seg_jefe
                                    //expexp = q.Expediente
                                }).ToList().Select(x => new
                                {
                                    nombreusuario = nombreusuario,
                                    emailusuario = emailusuario,
                                    fecha_incidente = x.fecha_incidente,
                                    expediente = x.expediente,
                                    area_incidente = x.area_incidente,
                                    tipoarea_incidente = x.tipo_area,
                                    emailcoordmed = x.emailcoordmed,
                                    emailjefe = x.emailjefe,
                                    area = x.area,
                                    jefe = x.jefe,
                                    coordinador = x.coordinador,
                                    tipo_incidente = x.tipo_incidente,
                                    persona_reporta = x.persona_reporta,
                                    folio = x.folio,
                                    desc_sol = x.desc_sol,
                                    fecha_seguimiento = x.fecha_seguimiento,
                                    emp_asignado = x.emp_asignado,
                                    seguimiento = x.seguimiento,
                                    seguimiento_jefe = x.seguimiento_jefe,
                                    jefe_asignado = x.jefe_asignado,
                                    tipo_area = x.tipo_area,
                                    fecha_coord_notifica = x.fecha_coord_notifica,
                                    fecha_cierre = x.fecha_cierre,
                                    fecha_seg_jefe = x.fecha_seg_jefe
                                })
                         .OrderByDescending(g => g.fecha_incidente)
                         .ToList();

                    //System.Diagnostics.Debug.WriteLine(inci);

                    string json = JsonConvert.SerializeObject(inci);
                    string path = Server.MapPath("~/json/incidentes/");
                    System.IO.File.WriteAllText(path + "incidentes_coord_" + nombreusuario + ".json", json);

                }

            }
            else
            {

                var inci = (from a in db.SOL_SERVICIO
                                //join areaIncidente in db.sis_areas on emailusuario equals areaIncidente.emailcoordmed into areaX
                            //where a.folio == 25
                            join areaIncidente in db.sis_areas on a.area_incidente equals areaIncidente.id_area into areaX
                            join tipoIncidente in db.tipo_incidente on a.tipo_incidente equals tipoIncidente.clave into tipoX
                            from areaIn in areaX.DefaultIfEmpty()
                            from tipoIn in tipoX.DefaultIfEmpty()
                            orderby a.fecha_incidente descending
                            select new
                            {
                                fecha_incidente = a.fecha_incidente,
                                expediente = a.expediente,
                                area_incidente = areaIn.descripcion,
                                tipoarea_incidente = areaIn.tipo_area,
                                emailcoordmed = areaIn.emailcoordmed,
                                emailjefe = areaIn.EMAIL,
                                area = a.area_incidente,
                                jefe = areaIn.COORDINADOR,
                                coordinador = areaIn.coordmedico,
                                tipo_incidente = tipoIn.descripcion,
                                persona_reporta = a.persona_reporta,
                                folio = a.folio,
                                desc_sol = a.DESC_SOL,
                                fecha_seguimiento = a.fecha_seguimiento,
                                emp_asignado = a.emp_asignado,
                                seguimiento = a.seguimiento,
                                seguimiento_jefe = a.seguimiento_jefe,
                                jefe_asignado = a.jefe_asignado,
                                tipo_area = areaIn.tipo_area,
                                fecha_coord_notifica = a.fecha_coord_notifica,
                                fecha_cierre = a.fecha_cierre,
                                fecha_seg_jefe = a.fecha_seg_jefe
                                //expexp = q.Expediente
                            }).ToList().Where(v => v.emailcoordmed == emailusuario || v.emailjefe == emailusuario).Select(x => new
                            {
                                nombreusuario = nombreusuario,
                                emailusuario = emailusuario,
                                fecha_incidente = x.fecha_incidente,
                                expediente = x.expediente,
                                area_incidente = x.area_incidente,
                                tipoarea_incidente = x.tipo_area,
                                emailcoordmed = x.emailcoordmed,
                                emailjefe = x.emailjefe,
                                area = x.area,
                                jefe = x.jefe,
                                coordinador = x.coordinador,
                                tipo_incidente = x.tipo_incidente,
                                persona_reporta = x.persona_reporta,
                                folio = x.folio,
                                desc_sol = x.desc_sol,
                                fecha_seguimiento = x.fecha_seguimiento,
                                emp_asignado = x.emp_asignado,
                                seguimiento = x.seguimiento,
                                seguimiento_jefe = x.seguimiento_jefe,
                                jefe_asignado = x.jefe_asignado,
                                tipo_area = x.tipo_area,
                                fecha_coord_notifica = x.fecha_coord_notifica,
                                fecha_cierre = x.fecha_cierre,
                                fecha_seg_jefe = x.fecha_seg_jefe
                            })
                    .OrderByDescending(g => g.fecha_incidente)
                    .ToList();

                //System.Diagnostics.Debug.WriteLine(inci);

                string json = JsonConvert.SerializeObject(inci);
                string path = Server.MapPath("~/json/incidentes/");
                System.IO.File.WriteAllText(path + "incidentes_coord_" + nombreusuario + ".json", json);
                //System.Diagnostics.Debug.WriteLine(tipo_usuario);


            }

            return View();
        }

        public ActionResult Incidentes2()
        {
            return View();
        }

        public ActionResult Incidentes3()
        {
            return View();
        }

        public class ListInicide
        {
            public int tipo { get; set; }
            public string fecha_incidente { get; set; }
            public string expediente { get; set; }
            public string area_incidente { get; set; }
            public string tipoarea_incidente { get; set; }
            public string emailcoordmed { get; set; }
            public string emailjefe { get; set; }
            public decimal? area { get; set; }
            public string jefe { get; set; }
            public string coordinador { get; set; }
            public string tipo_incidente { get; set; }
            public string persona_reporta { get; set; }
            public decimal folio { get; set; }
            public string desc_sol { get; set; }
            public DateTime? fecha_seguimiento { get; set; }
            public string emp_asignado { get; set; }
            public string seguimiento { get; set; }
            public string seguimiento_jefe { get; set; }
            public int? jefe_asignado { get; set; }
            public string tipo_area { get; set; }
            public DateTime? fecha_coord_notifica { get; set; }
            public DateTime? fecha_cierre { get; set; }
            public DateTime? fecha_seg_jefe { get; set; }
            public string nombreusuario { get; set; }
            public string emailusuario { get; set; }
            public string fondo1 { get; set; }
            public string fondo2 { get; set; }
            public string telefonos { get; set; }
            public int edad { get; set; }
            public string sexo { get; set; }
            public string boton { get; set; }
            public decimal incidente_numero { get; set; }
            public string info { get; set; }
            public string fecha { get; set; }
        }

        public JsonResult ListIncidentes()
        {
            Models.incidentesEntities3 db = new Models.incidentesEntities3();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var nombreusuario = User.Identity.GetUserName();

            var user = UserManager.FindById(User.Identity.GetUserId());
            var emailusuario = user.Email.ToString();

            var area_inci = (from a in db.sis_areas
                             where a.EMAIL == emailusuario || a.emailcoordmed == emailusuario
                             select a).FirstOrDefault();

            var fecha = DateTime.Now.AddDays(-4).ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);


            var inci = (from a in db.SOL_SERVICIO
                        join areaIncidente in db.sis_areas on a.area_incidente equals areaIncidente.id_area into areaX
                        join tipoIncidente in db.tipo_incidente on a.tipo_incidente equals tipoIncidente.clave into tipoX
                        from areaIn in areaX.DefaultIfEmpty()
                        from tipoIn in tipoX.DefaultIfEmpty()
                        where a.area_reasignada == null
                        orderby a.fecha_incidente descending
                        select new
                        {
                            fecha_incidente = a.fecha_incidente,
                            expediente = a.expediente,
                            area_incidente = areaIn.descripcion,
                            tipoarea_incidente = areaIn.tipo_area,
                            emailcoordmed = areaIn.emailcoordmed,
                            emailjefe = areaIn.EMAIL,
                            area = a.area_incidente,
                            jefe = areaIn.COORDINADOR,
                            coordinador = areaIn.coordmedico,
                            tipo_incidente = tipoIn.descripcion,
                            tipo_inci = tipoIn.clave,
                            persona_reporta = a.persona_reporta,
                            folio = a.folio,
                            desc_sol = a.DESC_SOL,
                            fecha_seguimiento = a.fecha_seguimiento,
                            emp_asignado = a.emp_asignado,
                            seguimiento = a.seguimiento,
                            seguimiento_jefe = a.seguimiento_jefe,
                            jefe_asignado = a.jefe_asignado,
                            tipo_area = areaIn.tipo_area,
                            fecha_coord_notifica = a.fecha_coord_notifica,
                            fecha_cierre = a.fecha_cierre,
                            fecha_seg_jefe = a.fecha_seg_jefe,
                            nombreusuario = nombreusuario,
                            emailusuario = emailusuario,
                            //expexp = q.Expediente
                        })
                        .OrderByDescending(g => g.fecha_incidente)
                        .ToList();

            //COLORES DE INCIDENTES
            var fondo1 = "";
            var fondo2 = "";
            var boton = "";

            if (area_inci == null)
            {
                //Coordinacion

                if (nombreusuario == "jorge.gutierrez" || nombreusuario == "direccion" || nombreusuario == "arturo.martinez" || nombreusuario == "coordinacion")
                {
                    var results = new List<ListInicide>();

                    foreach (var item in inci)
                    {
                        //Tomar el nombre del paciente

                        var dhabientes = (from a in db2.DHABIENTES
                                          join af in db2.AFILIADOS on a.NUMAFIL equals af.num_contrato into afili
                                          from afiliado in afili.DefaultIfEmpty()
                                          where a.NUMEMP == item.expediente
                                          select new
                                          {
                                              NOMBRES = a.NOMBRES,
                                              AMATERNO = a.AMATERNO,
                                              APATERNO = a.APATERNO,
                                              FNAC = a.FNAC,
                                              SEXO = a.SEXO,
                                              telefonos = afiliado.TELEFONOS,
                                          }).FirstOrDefault();


                        //EDAD
                        var today = DateTime.Today;
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
                        //EDAD


                        //Si es una felicitacion
                        if (item.tipo_inci == 3)
                        {
                            //Morado
                            fondo1 = "#f2e8ff";
                            fondo2 = "#8950d4";
                            boton = "Ver";
                        }
                        else
                        {
                            //Si ya esta cerrado
                            if (item.fecha_cierre != null)
                            {
                                //Verde
                                fondo1 = "#d9ffdd";
                                fondo2 = "green";
                                boton = "Ver";
                            }
                            else
                            {
                                if (fechaDT > item.fecha_incidente)
                                {
                                    //Rojo
                                    fondo1 = "#ffdee1";
                                    fondo2 = "#dc3545";
                                    boton = "Ver";
                                }
                                else
                                {
                                    if (item.seguimiento != null && item.seguimiento_jefe != null)
                                    {
                                        //Amarrillo
                                        fondo1 = "#fff7e3";
                                        fondo2 = "#FBC43D";
                                        boton = "Ver";
                                    }
                                    else
                                    {
                                        if(item.seguimiento != null)
                                        {
                                            //Azul
                                            fondo1 = "#d4eaff";
                                            fondo2 = "#004a8f";
                                            boton = "Ver";
                                        }
                                        else
                                        {
                                            //Blanco
                                            fondo1 = "#fff";
                                            fondo2 = "#b0b0b0";
                                            boton = "Ver";
                                        }
                                    }
                                }
                            }
                        }

                        var result = new ListInicide
                        {
                            tipo = 2,
                            fecha_incidente = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha_incidente, new CultureInfo("es-ES")),
                            expediente = item.expediente,
                            area_incidente = item.area_incidente,
                            tipoarea_incidente = item.tipoarea_incidente,
                            emailcoordmed = item.emailcoordmed,
                            emailjefe = item.emailjefe,
                            area = item.area,
                            jefe = item.jefe,
                            coordinador = item.coordinador,
                            tipo_incidente = item.tipo_incidente,
                            //persona_reporta = item.persona_reporta,
                            persona_reporta = dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO,
                            folio = item.folio,
                            desc_sol = item.desc_sol,
                            fecha_seguimiento = item.fecha_seguimiento,
                            emp_asignado = item.emp_asignado,
                            seguimiento = item.seguimiento,
                            seguimiento_jefe = item.seguimiento_jefe,
                            jefe_asignado = item.jefe_asignado,
                            tipo_area = item.tipo_area,
                            fecha_coord_notifica = item.fecha_coord_notifica,
                            fecha_cierre = item.fecha_cierre,
                            fecha_seg_jefe = item.fecha_seg_jefe,
                            nombreusuario = item.nombreusuario,
                            emailusuario = item.emailusuario,
                            fondo1 = fondo1,
                            fondo2 = fondo2,
                            telefonos = dhabientes.telefonos,
                            edad = Years,
                            sexo = dhabientes.SEXO,
                            boton = boton,
                        };

                        results.Add(result);

                    }

                    return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                else
                {
                    var results = "";
                    return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

            }
            else
            {
                //Usuario normales
                var results = new List<ListInicide>();

                foreach (var item in inci)
                {

                    //Tomar el nombre del paciente
                    
                    var dhabientes = (from a in db2.DHABIENTES
                                      join af in db2.AFILIADOS on a.NUMAFIL equals af.num_contrato into afili
                                      from afiliado in afili.DefaultIfEmpty()
                                      where a.NUMEMP == item.expediente
                                      select new
                                      {
                                          NOMBRES = a.NOMBRES,
                                          AMATERNO = a.AMATERNO,
                                          APATERNO = a.APATERNO,
                                          FNAC = a.FNAC,
                                          SEXO = a.SEXO,
                                          telefonos = afiliado.TELEFONOS,
                                      }).FirstOrDefault();


                    //EDAD
                    var today = DateTime.Today;
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
                    //EDAD

                    if (emailusuario == item.emailcoordmed || emailusuario == item.emailjefe)
                    {
                        //Si es una felicitacion
                        if (item.tipo_inci == 3)
                        {
                            //Celeste
                            fondo1 = "#edfffe";
                            fondo2 = "#97f0eb";
                            boton = "Ver";
                        }
                        else
                        {
                            //Si es una recomendacion
                            if (item.tipo_inci == 2)
                            {
                                //Lila
                                fondo1 = "#fff2ff";
                                fondo2 = "#f2c4f2";
                                boton = "Ver";
                            }
                            else
                            {
                                //Si es una inconformidad
                                if (item.tipo_inci == 4)
                                {
                                    //Naranja
                                    fondo1 = "#fff7e6";
                                    fondo2 = "#ffa91f";
                                    boton = "Ver";
                                }
                                else
                                {
                                    if (item.fecha_cierre != null)
                                    {
                                        //Verde
                                        fondo1 = "#d9ffdd";
                                        fondo2 = "green";
                                        boton = "Ver";
                                    }
                                    else
                                    {
                                        //Si ya se paso más de 3 días y no ha contestado el jefe
                                        if (fechaDT > item.fecha_seguimiento && item.seguimiento_jefe != null)
                                        {
                                            //Rojo
                                            fondo1 = "#ffdee1";
                                            fondo2 = "#dc3545";
                                            boton = "Ver";
                                        }
                                        //Si ya se paso más de 3 días y no ha contestado el coordinador
                                        if(fechaDT > item.fecha_incidente && item.seguimiento == null)
                                        {
                                            //Rojo
                                            fondo1 = "#ffdee1";
                                            fondo2 = "#dc3545";
                                            boton = "Ver";
                                        }
                                        else
                                        {
                                            if (fechaDT > item.fecha_seguimiento && item.seguimiento_jefe == null)
                                            {
                                                //Rojo
                                                fondo1 = "#ffdee1";
                                                fondo2 = "#dc3545";
                                                boton = "Ver";
                                            }
                                            else
                                            {
                                                if(item.seguimiento != null)
                                                {
                                                    //Beige
                                                    fondo1 = "#fff5e6";
                                                    fondo2 = "#f5d7ab";
                                                    boton = "Ver";
                                                }
                                                else
                                                {
                                                    //Amarillo
                                                    fondo1 = "#fff7e3";
                                                    fondo2 = "#FBC43D";
                                                    boton = "Ver";
                                                }
                                                
                                            }
                                        }
                                        
                                    }
                                }
                            }
                        }
                        
                        var result = new ListInicide
                        {
                            tipo = 2,
                            fecha_incidente = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha_incidente, new CultureInfo("es-ES")),
                            expediente = item.expediente,
                            area_incidente = item.area_incidente,
                            tipoarea_incidente = item.tipoarea_incidente,
                            emailcoordmed = item.emailcoordmed,
                            emailjefe = item.emailjefe,
                            area = item.area,
                            jefe = item.jefe,
                            coordinador = item.coordinador,
                            tipo_incidente = item.tipo_incidente,
                            incidente_numero = item.tipo_inci,
                            //persona_reporta = item.persona_reporta,
                            persona_reporta = dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO,
                            folio = item.folio,
                            desc_sol = item.desc_sol,
                            fecha_seguimiento = item.fecha_seguimiento,
                            emp_asignado = item.emp_asignado,
                            seguimiento = item.seguimiento,
                            seguimiento_jefe = item.seguimiento_jefe,
                            jefe_asignado = item.jefe_asignado,
                            tipo_area = item.tipo_area,
                            fecha_coord_notifica = item.fecha_coord_notifica,
                            fecha_cierre = item.fecha_cierre,
                            fecha_seg_jefe = item.fecha_seg_jefe,
                            nombreusuario = item.nombreusuario,
                            emailusuario = item.emailusuario,
                            fondo1 = fondo1,
                            fondo2 = fondo2,
                            telefonos = dhabientes.telefonos,
                            edad = Years,
                            sexo = dhabientes.SEXO,
                            boton = boton,
                        };

                        results.Add(result);
                    }
   
                }

                return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            

        }


        public JsonResult ListIncidentes2()
        {
            Models.incidentesEntities3 db = new Models.incidentesEntities3();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var nombreusuario = User.Identity.GetUserName();

            var user = UserManager.FindById(User.Identity.GetUserId());
            var emailusuario = user.Email.ToString();

            var area_inci = (from a in db.sis_areas
                             where a.EMAIL == emailusuario || a.emailcoordmed == emailusuario
                             select a).FirstOrDefault();

            var fecha = DateTime.Now.AddDays(-4).ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            var fecha2 = DateTime.Now.ToString("2022-02-28T00:00:00.000");
            var fechaDT2 = DateTime.Parse(fecha2);

            var fecha3 = DateTime.Now.ToString("2022-09-13T00:00:00.000");
            var fechaDT3 = DateTime.Parse(fecha3);


            var inci = (from a in db.SOL_SERVICIO
                        //where a.fecha_incidente >= fechaDT2
                        join areaIncidente in db.sis_areas on a.area_incidente equals areaIncidente.id_area into areaX
                        join tipoIncidente in db.tipo_incidente on a.tipo_incidente equals tipoIncidente.clave into tipoX
                        from areaIn in areaX.DefaultIfEmpty()
                        from tipoIn in tipoX.DefaultIfEmpty()
                        where a.fecha_incidente >= fechaDT3
                        orderby a.fecha_incidente descending
                        select new
                        {
                            fecha_incidente = a.fecha_incidente,
                            expediente = a.expediente,
                            area_incidente = areaIn.descripcion,
                            tipoarea_incidente = areaIn.tipo_area,
                            emailcoordmed = areaIn.emailcoordmed,
                            emailjefe = areaIn.EMAIL,
                            area = a.area_incidente,
                            jefe = areaIn.COORDINADOR,
                            coordinador = areaIn.coordmedico,
                            tipo_incidente = tipoIn.descripcion,
                            tipo_inci = tipoIn.clave,
                            persona_reporta = a.persona_reporta,
                            folio = a.folio,
                            desc_sol = a.DESC_SOL,
                            fecha_seguimiento = a.fecha_seguimiento,
                            emp_asignado = a.emp_asignado,
                            seguimiento = a.seguimiento,
                            seguimiento_jefe = a.seguimiento_jefe,
                            jefe_asignado = a.jefe_asignado,
                            tipo_area = areaIn.tipo_area,
                            fecha_coord_notifica = a.fecha_coord_notifica,
                            fecha_cierre = a.fecha_cierre,
                            fecha_seg_jefe = a.fecha_seg_jefe,
                            nombreusuario = nombreusuario,
                            emailusuario = emailusuario,
                            //expexp = q.Expediente
                        })
                        .OrderByDescending(g => g.fecha_incidente)
                        .ToList();

            //COLORES DE INCIDENTES
            var fondo1 = "";
            var fondo2 = "";
            var boton = "";

            if (area_inci == null)
            {
                //Coordinacion

                if (nombreusuario == "jorge.gutierrez" || nombreusuario == "direccion" || nombreusuario == "arturo.martinez" || nombreusuario == "coordinacion")
                {
                    var results = new List<ListInicide>();

                    foreach (var item in inci)
                    {
                        //Tomar el nombre del paciente

                        var dhabientes = (from a in db2.DHABIENTES
                                          join af in db2.AFILIADOS on a.NUMAFIL equals af.num_contrato into afili
                                          from afiliado in afili.DefaultIfEmpty()
                                          where a.NUMEMP == item.expediente
                                          select new
                                          {
                                              NOMBRES = a.NOMBRES,
                                              AMATERNO = a.AMATERNO,
                                              APATERNO = a.APATERNO,
                                              FNAC = a.FNAC,
                                              SEXO = a.SEXO,
                                              telefonos = afiliado.TELEFONOS,
                                          }).FirstOrDefault();


                        //EDAD
                        var today = DateTime.Today;
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
                        //EDAD


                        //Si es una felicitacion
                        if (item.tipo_inci == 3)
                        {
                            //Morado
                            fondo1 = "#f2e8ff";
                            fondo2 = "#8950d4";
                            boton = "Ver";
                        }
                        else
                        {
                            //Si ya esta cerrado
                            if (item.fecha_cierre != null)
                            {
                                //Verde
                                fondo1 = "#d9ffdd";
                                fondo2 = "green";
                                boton = "Ver";
                            }
                            else
                            {
                                if (fechaDT > item.fecha_incidente)
                                {
                                    //Rojo
                                    fondo1 = "#ffdee1";
                                    fondo2 = "#dc3545";
                                    boton = "Ver";
                                }
                                else
                                {
                                    if (item.seguimiento != null && item.seguimiento_jefe != null)
                                    {
                                        //Amarrillo
                                        fondo1 = "#fff7e3";
                                        fondo2 = "#FBC43D";
                                        boton = "Ver";
                                    }
                                    else
                                    {
                                        if (item.seguimiento != null)
                                        {
                                            //Azul
                                            fondo1 = "#d4eaff";
                                            fondo2 = "#004a8f";
                                            boton = "Ver";
                                        }
                                        else
                                        {
                                            //Blanco
                                            fondo1 = "#fff";
                                            fondo2 = "#b0b0b0";
                                            boton = "Ver";
                                        }
                                    }
                                }
                            }
                        }

                        var result = new ListInicide
                        {
                            tipo = 2,
                            fecha_incidente = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha_incidente, new CultureInfo("es-ES")),
                            expediente = item.expediente,
                            area_incidente = item.area_incidente,
                            tipoarea_incidente = item.tipoarea_incidente,
                            emailcoordmed = item.emailcoordmed,
                            emailjefe = item.emailjefe,
                            area = item.area,
                            jefe = item.jefe,
                            coordinador = item.coordinador,
                            tipo_incidente = item.tipo_incidente,
                            //persona_reporta = item.persona_reporta,
                            persona_reporta = dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO,
                            folio = item.folio,
                            desc_sol = item.desc_sol,
                            fecha_seguimiento = item.fecha_seguimiento,
                            emp_asignado = item.emp_asignado,
                            seguimiento = item.seguimiento,
                            seguimiento_jefe = item.seguimiento_jefe,
                            jefe_asignado = item.jefe_asignado,
                            tipo_area = item.tipo_area,
                            fecha_coord_notifica = item.fecha_coord_notifica,
                            fecha_cierre = item.fecha_cierre,
                            fecha_seg_jefe = item.fecha_seg_jefe,
                            nombreusuario = item.nombreusuario,
                            emailusuario = item.emailusuario,
                            fondo1 = fondo1,
                            fondo2 = fondo2,
                            telefonos = dhabientes.telefonos,
                            edad = Years,
                            sexo = dhabientes.SEXO,
                            boton = boton,
                            info = fondo1 + "*" + fondo2 + "*" + string.Format("{0:dddd, dd MMMM yyyy}", item.fecha_incidente, new CultureInfo("es-ES")) + "*" + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + "*" + dhabientes.SEXO + "*" + Years + "*" + dhabientes.telefonos + "*" + item.area_incidente + "*" + item.coordinador + "*" + item.jefe + "*" + item.tipo_incidente + "*" + item.folio + "*" + boton + "*" + item.expediente
                        };

                        results.Add(result);

                    }

                    return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                else
                {
                    var results = "";
                    return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

            }
            else
            {
                //Usuario normales
                var results = new List<ListInicide>();

                foreach (var item in inci)
                {

                    //Tomar el nombre del paciente

                    var dhabientes = (from a in db2.DHABIENTES
                                      join af in db2.AFILIADOS on a.NUMAFIL equals af.num_contrato into afili
                                      from afiliado in afili.DefaultIfEmpty()
                                      where a.NUMEMP == item.expediente
                                      select new
                                      {
                                          NOMBRES = a.NOMBRES,
                                          AMATERNO = a.AMATERNO,
                                          APATERNO = a.APATERNO,
                                          FNAC = a.FNAC,
                                          SEXO = a.SEXO,
                                          telefonos = afiliado.TELEFONOS,
                                      }).FirstOrDefault();

                    if(dhabientes == null)
                    {
                        System.Diagnostics.Debug.WriteLine(item);
                    }

                    //EDAD
                    var today = DateTime.Today;
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
                    //EDAD

                    if (emailusuario == item.emailcoordmed || emailusuario == item.emailjefe)
                    {
                        //Si es una felicitacion / recomendacion / inconformidad
                        if (item.fecha_cierre != null)
                        {
                            //Verde
                            fondo1 = "#d9ffdd";
                            fondo2 = "green";
                            boton = "Ver";
                        }
                        else
                        {
                            if (item.tipo_inci == 2 || item.tipo_inci == 3 || item.tipo_inci == 4)
                            {
                                //Azul
                                fondo1 = "#d9eafa";
                                fondo2 = "#004a8f";
                                boton = "Ver";
                            }
                            else
                            {
                                //Si ya se paso más de 3 días y no ha contestado el jefe
                                if (fechaDT > item.fecha_seguimiento && item.seguimiento_jefe != null)
                                {
                                    //Rojo
                                    fondo1 = "#ffdee1";
                                    fondo2 = "#dc3545";
                                    boton = "Dar seguimiento";
                                }
                                //Si ya se paso más de 3 días y no ha contestado el coordinador
                                if (fechaDT > item.fecha_incidente && item.seguimiento == null)
                                {
                                    //Rojo
                                    fondo1 = "#ffdee1";
                                    fondo2 = "#dc3545";
                                    boton = "Dar seguimiento";
                                }
                                else
                                {
                                    if (fechaDT > item.fecha_seguimiento && item.seguimiento_jefe == null)
                                    {
                                        //Rojo
                                        fondo1 = "#ffdee1";
                                        fondo2 = "#dc3545";
                                        boton = "Dar seguimiento";
                                    }
                                    else
                                    {
                                        if (item.seguimiento != null)
                                        {
                                            if(item.emailjefe == emailusuario && item.seguimiento_jefe == null)
                                            {
                                                //Aun no contesta el jefe
                                                //Amarillo
                                                fondo1 = "#fff7e6";
                                                fondo2 = "#ffa91f";
                                                boton = "Dar seguimiento";
                                            }
                                            else
                                            {
                                                //Celeste
                                                fondo1 = "#edfffe";
                                                fondo2 = "#97f0eb";
                                                boton = "Ver";
                                            }
                                            
                                        }
                                        else
                                        {
                                            //Amarillo
                                            fondo1 = "#fff7e6";
                                            fondo2 = "#ffa91f";
                                            boton = "Dar seguimiento";
                                        }

                                    }
                                }

                            }
                        }

                        var result = new ListInicide
                        {
                            tipo = 2,
                            fecha_incidente = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha_incidente, new CultureInfo("es-ES")),
                            expediente = item.expediente,
                            area_incidente = item.area_incidente,
                            tipoarea_incidente = item.tipoarea_incidente,
                            emailcoordmed = item.emailcoordmed,
                            emailjefe = item.emailjefe,
                            area = item.area,
                            jefe = item.jefe,
                            coordinador = item.coordinador,
                            tipo_incidente = item.tipo_incidente,
                            incidente_numero = item.tipo_inci,
                            //persona_reporta = item.persona_reporta,
                            persona_reporta = dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO,
                            folio = item.folio,
                            desc_sol = item.desc_sol,
                            fecha_seguimiento = item.fecha_seguimiento,
                            emp_asignado = item.emp_asignado,
                            seguimiento = item.seguimiento,
                            seguimiento_jefe = item.seguimiento_jefe,
                            jefe_asignado = item.jefe_asignado,
                            tipo_area = item.tipo_area,
                            fecha_coord_notifica = item.fecha_coord_notifica,
                            fecha_cierre = item.fecha_cierre,
                            fecha_seg_jefe = item.fecha_seg_jefe,
                            nombreusuario = item.nombreusuario,
                            emailusuario = item.emailusuario,
                            fondo1 = fondo1,
                            fondo2 = fondo2,
                            telefonos = dhabientes.telefonos,
                            edad = Years,
                            sexo = dhabientes.SEXO,
                            boton = boton,
                            info = fondo1 + "*" + fondo2 + "*" + string.Format("{0:dddd, dd MMMM yyyy}", item.fecha_incidente, new CultureInfo("es-ES")) + "*" + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + "*" + dhabientes.SEXO + "*" + Years + "*" + dhabientes.telefonos + "*" + item.area_incidente + "*" + item.coordinador + "*" + item.jefe + "*" + item.tipo_incidente + "*" + item.folio + "*" + boton + "*" + item.expediente,
                            fecha = string.Format("{0:yyyy-MM-dd}", item.fecha_incidente),

                        };

                        results.Add(result);
                    }

                }

                return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



        }


        public JsonResult DetalleIncidente(int folio)
        {
            Models.incidentesEntities3 db = new Models.incidentesEntities3();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var inci = (from a in db.SOL_SERVICIO
                        join areaIncidente in db.sis_areas on a.area_incidente equals areaIncidente.id_area into areaX
                        join tipoIncidente in db.tipo_incidente on a.tipo_incidente equals tipoIncidente.clave into tipoX
                        from areaIn in areaX.DefaultIfEmpty()
                        from tipoIn in tipoX.DefaultIfEmpty()
                        where a.folio == folio
                        select new
                        {
                            fecha_incidente = a.fecha_incidente,
                            expediente = a.expediente,
                            area_incidente = areaIn.descripcion,
                            tipoarea_incidente = areaIn.tipo_area,
                            emailcoordmed = areaIn.emailcoordmed,
                            emailjefe = areaIn.EMAIL,
                            area = a.area_incidente,
                            jefe = areaIn.COORDINADOR,
                            coordinador = areaIn.coordmedico,
                            tipo_incidente = tipoIn.descripcion,
                            tipo_inci = tipoIn.clave,
                            persona_reporta = a.persona_reporta,
                            folio = a.folio,
                            desc_sol = a.DESC_SOL,
                            fecha_seguimiento = a.fecha_seguimiento,
                            emp_asignado = a.emp_asignado,
                            seguimiento_coordinador = a.seguimiento,
                            seguimiento_jefe = a.seguimiento_jefe,
                            jefe_asignado = a.jefe_asignado,
                            tipo_area = areaIn.tipo_area,
                            fecha_coord_notifica = a.fecha_coord_notifica,
                            fecha_cierre = a.fecha_cierre,
                            fecha_seg_jefe = a.fecha_seg_jefe,
                            empleado = a.emp_atiende,
                        })
                        .FirstOrDefault();

            //Tomar el nombre del paciente

            var dhabientes = (from a in db2.DHABIENTES
                              join af in db2.AFILIADOS on a.NUMAFIL equals af.num_contrato into afili
                              from afiliado in afili.DefaultIfEmpty()
                              where a.NUMEMP == inci.expediente
                              select new
                              {
                                  NOMBRES = a.NOMBRES,
                                  AMATERNO = a.AMATERNO,
                                  APATERNO = a.APATERNO,
                                  FNAC = a.FNAC,
                                  SEXO = a.SEXO,
                                  telefonos = afiliado.TELEFONOS,
                              }).FirstOrDefault();

            //EDAD
            var today = DateTime.Today;
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
            //EDAD

            var cerrado = "";
            if(inci.fecha_cierre != null)
            {
                cerrado = "Cerrado";
            }

            var user = UserManager.FindById(User.Identity.GetUserId());
            var emailusuario = user.Email.ToString();
            var nombreusuario = User.Identity.GetUserName();

            var result = new Object();

            var seguimiento_coordinador = "";

            if (inci.emailcoordmed == inci.emailjefe && inci.seguimiento_coordinador == "")
            {
                seguimiento_coordinador = inci.seguimiento_jefe;
            }
            else
            {
                seguimiento_coordinador = inci.seguimiento_coordinador;
            }

            result = new
            {
                folio = inci.folio,
                paciente = dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO,
                tipo_incidente = inci.tipo_incidente,
                persona_reporta = inci.persona_reporta,
                desc_sol = inci.desc_sol,
                emailcoordmed = inci.emailcoordmed,
                emailjefe = inci.emailjefe,
                fecha_incidente = string.Format("{0:dddd, dd MMMM yyyy}", inci.fecha_incidente, new CultureInfo("es-ES")),
                edad = Years,
                sexo = dhabientes.SEXO,
                telefonos = dhabientes.telefonos,
                cerrado = cerrado,
                seguimiento_coordinador = seguimiento_coordinador,
                seguimiento_jefe = inci.seguimiento_jefe,
                emailusuario = emailusuario,
                nombreusuario = nombreusuario,
                empleado = inci.empleado,
            };

            //System.Diagnostics.Debug.WriteLine(inci);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        [HttpPost]
        public ActionResult GuardarIncidente(int folio, string nota_coordinador, string nota_jefe, string area_incidente)
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            //var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            Models.incidentesEntities3 db = new Models.incidentesEntities3();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var incidente = (from a in db.SOL_SERVICIO
                            join areaIncidente in db.sis_areas on a.area_incidente equals areaIncidente.id_area into areaX
                            join tipoIncidente in db.tipo_incidente on a.tipo_incidente equals tipoIncidente.clave into tipoX
                            from areaIn in areaX.DefaultIfEmpty()
                            from tipoIn in tipoX.DefaultIfEmpty()
                            where a.folio == folio
                            select new
                            {
                                emailcoordmed = areaIn.emailcoordmed,
                                emailjefe = areaIn.EMAIL,
                            })
                            .FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(area_reasignada);

            var user = UserManager.FindById(User.Identity.GetUserId());
            var emailusuario = user.Email.ToString();

            if(incidente.emailcoordmed == incidente.emailjefe)
            {
                if(area_incidente != null)
                {
                    db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET area_incidente = '" + area_incidente + "' WHERE folio = '" + folio + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET seguimiento_jefe = '" + nota_coordinador + "', fecha_seg_jefe = '" + fecha + "', seguimiento = '" + nota_coordinador + "', fecha_seguimiento = '" + fecha + "', coord_notifica = 1, fecha_coord_notifica = '" + fecha + "' WHERE folio = '" + folio + "'");
                }

            }
            else
            {
                //Si solo es coordinador
                if(incidente.emailcoordmed == emailusuario)
                {
                    db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET seguimiento = '" + nota_coordinador + "', fecha_seguimiento = '" + fecha + "', coord_notifica = 1, fecha_coord_notifica   = '" + fecha + "' WHERE folio = '" + folio + "'");
                }
                else
                {
                    //Es jefe
                    db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET seguimiento_jefe = '" + nota_jefe + "', fecha_seg_jefe = '" + fecha + "' WHERE folio = '" + folio + "'");

                }
            }


            return Redirect(Request.UrlReferrer.ToString());
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

                var user = UserManager.FindById(User.Identity.GetUserId());
                var emailusuario = user.Email.ToString();

                var folio = int.Parse(id);

                var incidente_folio = (from a in db.SOL_SERVICIO
                                       where a.folio == folio
                                       select a).FirstOrDefault();

                //Error aqui
                var area_inci = (from a in db.sis_areas
                                 where a.id_area == incidente_folio.area_incidente
                                 select a).FirstOrDefault();


                if (nombreusuario == "jorge.gutierrez" || nombreusuario == "direccion"  || nombreusuario == "arturo.martinez" || nombreusuario == "coordinacion")
                {
                    var inci = (from a in db.SOL_SERVICIO
                                where a.folio == folio
                                //where a.fecha_cierre == null
                                join areaIncidente in db.sis_areas on a.area_incidente equals areaIncidente.id_area into areaX
                                join tipoIncidente in db.tipo_incidente on a.tipo_incidente equals tipoIncidente.clave into tipoX
                                from areaIn in areaX.DefaultIfEmpty()
                                from tipoIn in tipoX.DefaultIfEmpty()
                                orderby a.fecha_incidente descending
                                select new
                                {
                                    fecha_incidente = a.fecha_incidente,
                                    expediente = a.expediente,
                                    area_incidente = areaIn.descripcion,
                                    tipoarea_incidente = areaIn.tipo_area,
                                    emailcoordmed = areaIn.emailcoordmed,
                                    emailjefe = areaIn.EMAIL,
                                    area = a.area_incidente,
                                    jefe = areaIn.COORDINADOR,
                                    coordinador = areaIn.coordmedico,
                                    tipo_incidente = tipoIn.descripcion,
                                    persona_reporta = a.persona_reporta,
                                    folio = a.folio,
                                    desc_sol = a.DESC_SOL,
                                    fecha_seguimiento = a.fecha_seguimiento,
                                    emp_asignado = a.emp_asignado,
                                    seguimiento = a.seguimiento,
                                    seguimiento_jefe = a.seguimiento_jefe,
                                    jefe_asignado = a.jefe_asignado,
                                    tipo_area = areaIn.tipo_area,
                                    fecha_coord_notifica = a.fecha_coord_notifica,
                                    fecha_cierre = a.fecha_cierre,
                                    fecha_seg_jefe = a.fecha_seg_jefe
                                    //expexp = q.Expediente
                                }).ToList().Select(x => new
                                {
                                    nombreusuario = nombreusuario,
                                    emailusuario = emailusuario,
                                    fecha_incidente = x.fecha_incidente,
                                    expediente = x.expediente,
                                    area_incidente = x.area_incidente,
                                    tipoarea_incidente = x.tipo_area,
                                    emailcoordmed = x.emailcoordmed,
                                    emailjefe = x.emailjefe,
                                    area = x.area,
                                    jefe = x.jefe,
                                    coordinador = x.coordinador,
                                    tipo_incidente = x.tipo_incidente,
                                    persona_reporta = x.persona_reporta,
                                    folio = x.folio,
                                    desc_sol = x.desc_sol,
                                    fecha_seguimiento = x.fecha_seguimiento,
                                    emp_asignado = x.emp_asignado,
                                    seguimiento = x.seguimiento,
                                    seguimiento_jefe = x.seguimiento_jefe,
                                    jefe_asignado = x.jefe_asignado,
                                    tipo_area = x.tipo_area,
                                    fecha_coord_notifica = x.fecha_coord_notifica,
                                    fecha_cierre = x.fecha_cierre,
                                    fecha_seg_jefe = x.fecha_seg_jefe
                                })
                            .OrderByDescending(g => g.fecha_incidente)
                            .ToList();

                    //System.Diagnostics.Debug.WriteLine(inci);

                    string json = JsonConvert.SerializeObject(inci);
                    string path = Server.MapPath("~/json/incidentes/");
                    System.IO.File.WriteAllText(path + "incidentes_folio.json", json);
                }
                else
                {

                    var inci = (from a in db.SOL_SERVICIO
                                where a.folio == folio
                                //where a.fecha_cierre == null
                                join areaIncidente in db.sis_areas on a.area_incidente equals areaIncidente.id_area into areaX
                                join tipoIncidente in db.tipo_incidente on a.tipo_incidente equals tipoIncidente.clave into tipoX
                                from areaIn in areaX.DefaultIfEmpty()
                                from tipoIn in tipoX.DefaultIfEmpty()
                                orderby a.fecha_incidente descending
                                select new
                                {
                                    fecha_incidente = a.fecha_incidente,
                                    expediente = a.expediente,
                                    area_incidente = areaIn.descripcion,
                                    tipoarea_incidente = areaIn.tipo_area,
                                    emailcoordmed = areaIn.emailcoordmed,
                                    emailjefe = areaIn.EMAIL,
                                    area = a.area_incidente,
                                    jefe = areaIn.COORDINADOR,
                                    coordinador = areaIn.coordmedico,
                                    tipo_incidente = tipoIn.descripcion,
                                    persona_reporta = a.persona_reporta,
                                    folio = a.folio,
                                    desc_sol = a.DESC_SOL,
                                    fecha_seguimiento = a.fecha_seguimiento,
                                    emp_asignado = a.emp_asignado,
                                    seguimiento = a.seguimiento,
                                    seguimiento_jefe = a.seguimiento_jefe,
                                    jefe_asignado = a.jefe_asignado,
                                    tipo_area = areaIn.tipo_area,
                                    fecha_coord_notifica = a.fecha_coord_notifica,
                                    fecha_cierre = a.fecha_cierre,
                                    fecha_seg_jefe = a.fecha_seg_jefe
                                    //expexp = q.Expediente
                                }).ToList().Where(v => v.emailcoordmed == emailusuario || v.emailjefe == emailusuario).Select(x => new
                                {
                                    nombreusuario = nombreusuario,
                                    emailusuario = emailusuario,
                                    fecha_incidente = x.fecha_incidente,
                                    expediente = x.expediente,
                                    area_incidente = x.area_incidente,
                                    tipoarea_incidente = x.tipo_area,
                                    emailcoordmed = x.emailcoordmed,
                                    emailjefe = x.emailjefe,
                                    area = x.area,
                                    jefe = x.jefe,
                                    coordinador = x.coordinador,
                                    tipo_incidente = x.tipo_incidente,
                                    persona_reporta = x.persona_reporta,
                                    folio = x.folio,
                                    desc_sol = x.desc_sol,
                                    fecha_seguimiento = x.fecha_seguimiento,
                                    emp_asignado = x.emp_asignado,
                                    seguimiento = x.seguimiento,
                                    seguimiento_jefe = x.seguimiento_jefe,
                                    jefe_asignado = x.jefe_asignado,
                                    tipo_area = x.tipo_area,
                                    fecha_coord_notifica = x.fecha_coord_notifica,
                                    fecha_cierre = x.fecha_cierre,
                                    fecha_seg_jefe = x.fecha_seg_jefe
                                })
                            .OrderByDescending(g => g.fecha_incidente)
                            .ToList();

                    //System.Diagnostics.Debug.WriteLine(inci);

                    string json = JsonConvert.SerializeObject(inci);
                    string path = Server.MapPath("~/json/incidentes/");
                    System.IO.File.WriteAllText(path + "incidentes_folio.json", json);
                }



                return View(incidente_folio);
            }
        }


        [HttpPost]
        public ActionResult SeguimientoIncidentes(Models.SOL_SERVICIO model)
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            //System.Diagnostics.Debug.WriteLine(model.numemp);
            Models.incidentesEntities3 db = new Models.incidentesEntities3();

            var sis_area = (from a in db.sis_areas
                            where a.id_area == model.area_incidente
                            select a).FirstOrDefault();

            var sol_servicio = (from a in db.SOL_SERVICIO
                                where a.fecha_cierre == null
                                where a.folio == model.folio
                                select a).FirstOrDefault();

            var user = UserManager.FindById(User.Identity.GetUserId());
            var emailusuario = user.Email.ToString();

            //Si esta respondiendo un jefe de area
            if (emailusuario == sis_area.EMAIL)
            {
                db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET seguimiento_jefe = '" + model.seguimiento_jefe + "', fecha_seg_jefe = '" + fecha + "' WHERE folio = '" + model.folio + "'");

                //Se le envia un correo al email de el Coordinador
                /*dynamic email = new Email("Example");
                //email.To = "demian_hdzr@hotmail.com";
                email.To = sis_area.emailcoordmed;
                email.Subject = "Comunicate - Actualizacion de seguimiento en: " + sis_area.descripcion;
                email.Descripcion = sol_servicio.DESC_SOL;
                email.Folio = sol_servicio.folio;
                email.Nota = sol_servicio.seguimiento;
                email.Seguimiento = model.seguimiento_jefe;
                email.Send();*/

                TempData["message_success"] = "Incidente actualizado con éxito";
            }
            else
            {
                TempData["message_error"] = "Error al actualizar incidente";
            }


            if (emailusuario == sis_area.emailcoordmed)
            {
                if (model.area_reasignada != null)
                {
                    db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET jefe_asignado = 1, area_reasignada = '" + model.area_reasignada + "', seguimiento = '" + model.seguimiento + "', fecha_seguimiento = '" + fecha + "'  WHERE folio = '" + model.folio + "'");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET jefe_asignado = 1, seguimiento = '" + model.seguimiento + "', fecha_seguimiento = '" + fecha + "'  WHERE folio = '" + model.folio + "'");
                }

                /*dynamic email = new Email("Example");
                //email.To = "demian_hdzr@hotmail.com";
                email.To = sis_area.EMAIL;
                email.Subject = "Comunicate - Actualizacion de seguimiento en: " + sis_area.descripcion;
                email.Descripcion = sol_servicio.DESC_SOL;
                email.Folio = sol_servicio.folio;
                email.Nota = model.seguimiento;
                email.Seguimiento = sol_servicio.seguimiento_jefe;
                email.Send();*/

                TempData["message_success"] = "Incidente actualizado con éxito";
            }
            else
            {
                TempData["message_error"] = "Error al actualizar incidente";
            }


            //Si esta respondiendo un coordinador
            /*if (emailusuario == sis_area.emailcoordmed)
            {
                //Si se va a reasignar el area
                if (model.area_reasignada != null)
                {
                    //Si tiene seguimiento
                    if (model.seguimiento != null)
                    {
                        db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET jefe_asignado = 1, area_reasignada = '" + model.area_reasignada + "', seguimiento = '" + model.seguimiento + "', fecha_seguimiento = '" + fecha + "'  WHERE folio = '" + model.folio + "'");

                        dynamic email = new Email("Example");
                        //email.To = "demian_hdzr@hotmail.com";
                        email.To = sis_area.EMAIL;
                        email.Subject = "Comunicate - Actualizacion de seguimiento en: " + sis_area.descripcion;
                        email.Descripcion = sol_servicio.DESC_SOL;
                        email.Folio = sol_servicio.folio;
                        email.Nota = model.seguimiento;
                        email.Seguimiento = sol_servicio.seguimiento_jefe;
                        email.Send();
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET jefe_asignado = 1, area_reasignada = '" + model.area_reasignada + "' WHERE folio = '" + model.folio + "'");

                        dynamic email = new Email("Example");
                        //email.To = "demian_hdzr@hotmail.com";
                        email.To = sis_area.EMAIL;
                        email.Subject = "Comunicate - Actualizacion de seguimiento en: " + sis_area.descripcion;
                        email.Descripcion = sol_servicio.DESC_SOL;
                        email.Folio = sol_servicio.folio;
                        email.Nota = sol_servicio.seguimiento;
                        email.Seguimiento = sol_servicio.seguimiento_jefe;
                        email.Send();
                    }
                }
                else
                {
                    if (model.seguimiento != null)
                    {
                        db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET jefe_asignado = 1, seguimiento = '" + model.seguimiento + "', fecha_seguimiento = '" + fecha + "'   WHERE folio = '" + model.folio + "'");

                        dynamic email = new Email("Example");
                        //email.To = "demian_hdzr@hotmail.com";
                        email.To = sis_area.EMAIL;
                        email.Subject = "Comunicate - Actualizacion de seguimiento en: " + sis_area.descripcion;
                        email.Descripcion = sol_servicio.DESC_SOL;
                        email.Folio = sol_servicio.folio;
                        email.Nota = model.seguimiento;
                        email.Seguimiento = sol_servicio.seguimiento_jefe;
                        email.Send();
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET jefe_asignado = 1  WHERE folio = '" + model.folio + "'");

                        dynamic email = new Email("Example");
                        //email.To = "demian_hdzr@hotmail.com";
                        email.To = sis_area.EMAIL;
                        email.Subject = "Comunicate - Actualizacion de seguimiento en: " + sis_area.descripcion;
                        email.Descripcion = sol_servicio.DESC_SOL;
                        email.Folio = sol_servicio.folio;
                        email.Nota = sol_servicio.seguimiento;
                        email.Seguimiento = sol_servicio.seguimiento_jefe;
                        email.Send();
                    }
                }

                TempData["message_success"] = "Incidente actualizado con éxito";

            }
            else
            {
                TempData["message_error"] = "Error al actualizar incidente";
            }*/

            /*if (model.jefe_asignado == null)
            {
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

                if (model.area_reasignada != null)
                {
                    db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET jefe_asignado = 1, area_reasignada = '" + model.area_reasignada + "', seguimiento = '" + model.seguimiento + "'  WHERE folio = '" + model.folio + "'");

                }
                else
                {
                    db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET jefe_asignado = 1, seguimiento = '" + model.seguimiento + "'  WHERE folio = '" + model.folio + "'");
                }

            }
            else
            {
                var sis_area = (from a in db.sis_areas
                                where a.id_area == model.area_incidente
                                select a).FirstOrDefault();

                var sol_servicio = (from a in db.SOL_SERVICIO
                                    where a.folio == model.folio
                                    select a).FirstOrDefault();


                dynamic email = new Email("Example");
                email.To = "demian_hdzr@hotmail.com";
                //email.To = sis_area.emailcoordmed;
                email.Subject = "Seguimiento de incidente en: " + sis_area.descripcion;
                email.Descripcion = sol_servicio.DESC_SOL;
                email.Seguimiento = sol_servicio.seguimiento_jefe;
                email.Folio = sol_servicio.folio;
                email.Send();

                db.Database.ExecuteSqlCommand("Update SOL_SERVICIO SET seguimiento_jefe = '" + model.seguimiento_jefe + "', fecha_seg_jefe = '" + fecha + "'  WHERE folio = '" + model.folio + "'");
                TempData["message_success"] = "Incidente actualizado con éxito";
            }*/


            return Redirect(Request.UrlReferrer.ToString());
        }


        public JsonResult GetNombreExpediente(string search)
        {
            System.Diagnostics.Debug.WriteLine(search);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var dhabientes = (from a in db.DHABIENTES
                              where a.NUMEMP == search
                              join af in db.AFILIADOS on a.NUMAFIL equals af.num_contrato into afili
                              from afiliado in afili.DefaultIfEmpty()
                              join de in db.DEPENDENCIAS on afiliado.CVEDEP equals de.CVEDEP into depe
                              from dependencia in depe.DefaultIfEmpty()
                              select new
                              {
                                  NOMBRES = a.NOMBRES,
                                  AMATERNO = a.AMATERNO,
                                  APATERNO = a.APATERNO,
                                  NUMEMP = a.NUMEMP,
                                  SEXO = a.SEXO,
                                  FNAC = a.FNAC,
                                  FVIGENCIA = a.FVIGENCIA,
                                  REGHU = a.REGHU,
                                  depen = dependencia.DESCR,
                                  CURP = a.CURP,
                                  telefonos = afiliado.TELEFONOS,
                              })
                            .FirstOrDefault();

            var afiliados = (from a in db.AFILIADOS
                             where a.NUMEMP == "0"+search.Substring(0, 5)
                             //join numero_emp in db.AFILIADOS on a.NUMEMP.Substring(0, 5) equals numero_emp.NUMEMP
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
                telefono = dhabientes.telefonos,
                fvigencia = dhabientes.FVIGENCIA.ToString(),
                dependencia = dhabientes.depen,
            };

            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult SolicitarOrdenInternamiento()
        {
            return View();
        }

        public ActionResult ValoracionesPensionistas()
        {
            return View();
        }

        public ActionResult SolicitarValoracionPensionistas()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ValoracionPensionistas(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                var nombreusuario = User.Identity.GetUserName();
                var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fecha = DateTime.Parse(fecha_string);

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

        [HttpGet]
        public JsonResult GetDhabientesInfo(string search)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var dhabientes = (from a in db.DHABIENTES
                              where a.NUMEMP == search
                              join af in db.AFILIADOS on a.NUMAFIL equals af.num_contrato into afili
                              from afiliado in afili.DefaultIfEmpty()
                              join de in db.DEPENDENCIAS on afiliado.CVEDEP equals de.CVEDEP into depe
                              from dependencia in depe.DefaultIfEmpty()
                              select new
                              {
                                  NOMBRES = a.NOMBRES,
                                  AMATERNO = a.AMATERNO,
                                  APATERNO = a.APATERNO,
                                  NUMEMP = a.NUMEMP,
                                  SEXO = a.SEXO,
                                  FNAC = a.FNAC,
                                  FVIGENCIA = a.FVIGENCIA,
                                  REGHU = a.REGHU,
                                  depen = dependencia.DESCR,
                              })
                            .FirstOrDefault();

            return new JsonResult { Data = dhabientes, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult OrdenesInternamiento()
        {
            if (User.IsInRole("Especialistas"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Consultas", "Manage");
            }
        }

        [HttpGet]
        public ActionResult OrdenInternamiento(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var nombreusuario = User.Identity.GetUserName();

                var splitnombre = nombreusuario.Substring(0, 2);

                if (User.IsInRole("Especialistas"))
                {
                    Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                    //var nombreusuario = User.Identity.GetUserName();
                    var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                    var fecha = DateTime.Parse(fecha_string);

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
        }



        [HttpPost]
        public ActionResult GuardarOrdenInternamiento(Orden_Int model)
        {
            try
            {
                Models.SMDEVEntities32 db = new Models.SMDEVEntities32();

                //System.Diagnostics.Debug.WriteLine(ultima_orden.id_orden);

                Orden_Int orden_internamiento = new Orden_Int();
                //orden_internamiento.folio = "OI-" + folio.ToString();
                orden_internamiento.numemp = model.numemp;
                orden_internamiento.medico = model.medico;
                orden_internamiento.fecha_internamiento = model.fecha_internamiento;
                //orden_internamiento.fecha_alta = model.fecha_alta;
                orden_internamiento.proveedor = model.proveedor;
                orden_internamiento.estudios = model.estudios;
                //orden_internamiento.motivo = model.motivo;
                orden_internamiento.indicaciones = model.indicaciones;
                if (model.diagnostico1 != null) { orden_internamiento.diagnostico1 = model.diagnostico1; }
                if (model.diagnostico2 != null) { orden_internamiento.diagnostico2 = model.diagnostico2; }
                if (model.diagnostico3 != null) { orden_internamiento.diagnostico3 = model.diagnostico3; }

                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                var fecha_registro = DateTime.Parse(fecha);
                orden_internamiento.fecha_registro = fecha_registro;
                orden_internamiento.tipo = 1;

                db.Orden_Int.Add(orden_internamiento);
                db.SaveChanges();


                //toma el último id_orden para el folio que se agregó
                var ultima_orden = (from uo in db.Orden_Int
                                    where uo.numemp == model.numemp
                                    where uo.medico == model.medico
                                    orderby uo.id_orden descending
                                    select uo).FirstOrDefault();

                var folio = "OI-" + ultima_orden.id_orden;

                db.Database.ExecuteSqlCommand("UPDATE Orden_Int SET folio = '" + folio + "' WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and id_orden = '" + ultima_orden.id_orden + "'");


                var proveedores = (from a in db.prov_subrrogados
                                   where a.clave == model.proveedor
                                   select a).FirstOrDefault();


                TempData["message"] = "Orden de internamiento generado con éxito";
                TempData["folio"] = folio.ToString();
                TempData["fecha_internamiento"] = model.fecha_internamiento;
                TempData["proveedor"] = proveedores.nombre;
                TempData["proveedor_direccion"] = proveedores.direccion;
                TempData["proveedor_telefono"] = proveedores.telefono1;
                TempData["estudios"] = model.estudios;
                //TempData["motivo"] = model.motivo;
                TempData["indicaciones"] = model.indicaciones;
                TempData["diagnostico1"] = model.diagnostico1;
                TempData["diagnostico2"] = model.diagnostico2;
                TempData["diagnostico3"] = model.diagnostico3;


                return Redirect(Request.UrlReferrer.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPost]
        public ActionResult GuardarValoracionPensionistas(Orden_Int model)
        {
            try
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                //toma el último id_orden para el folio que se va a agregar
                /*var ultima_orden = (from uo in db.Orden_Int
                                    orderby uo.id_orden descending
                                    select uo).FirstOrDefault();*/

                //System.Diagnostics.Debug.WriteLine(ultima_orden.id_orden);

                //var folio = ultima_orden.id_orden + 1;

                Orden_Int orden_internamiento = new Orden_Int();
                //orden_internamiento.folio = "OI-" + folio.ToString();
                orden_internamiento.numemp = model.numemp;
                orden_internamiento.medico = model.medico;

                var proveedores = (from a in db.prov_subrrogados
                                   where a.clave == "012"
                                   select a).FirstOrDefault();

                orden_internamiento.proveedor = proveedores.clave;
                if (model.estudios != null) { orden_internamiento.estudios = model.estudios; }
                //orden_internamiento.motivo = model.motivo;
                if (model.resumen_medico != null) { orden_internamiento.resumen_medico = model.resumen_medico; }
                orden_internamiento.indicaciones = model.indicaciones;
                if (model.diagnostico1 != null) { orden_internamiento.diagnostico1 = model.diagnostico1; }
                if (model.diagnostico2 != null) { orden_internamiento.diagnostico2 = model.diagnostico2; }
                if (model.diagnostico3 != null) { orden_internamiento.diagnostico3 = model.diagnostico3; }

                var fecha2 = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                var fecha_registro = DateTime.Parse(fecha2);
                orden_internamiento.fecha_registro = fecha_registro;
                orden_internamiento.fecha_registro = fecha_registro;
                orden_internamiento.tipo = 0;
                orden_internamiento.enviar_a = model.enviar_a;
                db.Orden_Int.Add(orden_internamiento);
                db.SaveChanges();


                //toma el último id_orden para el folio que se agregó
                var ultima_orden = (from uo in db.Orden_Int
                                    join enviarA in db.Orden_Int_Enviar on uo.enviar_a equals enviarA.id into enviarAX
                                    from enviarAIn in enviarAX.DefaultIfEmpty()
                                    where uo.numemp == model.numemp
                                    where uo.medico == model.medico
                                    orderby uo.id_orden descending
                                    select new
                                    {
                                        id_orden = uo.id_orden,
                                        enviarA = enviarAIn.ubicacion
                                    }).FirstOrDefault();


                var folio = "OI-" + ultima_orden.id_orden;

                db.Database.ExecuteSqlCommand("UPDATE Orden_Int SET folio = '" + folio + "' WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and id_orden = '" + ultima_orden.id_orden + "'");


                TempData["message"] = "Valoración " + ultima_orden.enviarA + " generada con éxito";
                TempData["folio"] = folio.ToString();
                TempData["fecha_registro"] = fecha_registro;
                TempData["estudios"] = model.estudios;
                TempData["proveedor"] = proveedores.nombre;
                TempData["proveedor_direccion"] = proveedores.direccion;
                TempData["proveedor_telefono"] = proveedores.telefono1;
                //TempData["motivo"] = model.motivo;
                TempData["resumen_medico"] = model.resumen_medico;
                TempData["indicaciones"] = model.indicaciones;
                TempData["diagnostico1"] = model.diagnostico1;
                TempData["diagnostico2"] = model.diagnostico2;
                TempData["diagnostico3"] = model.diagnostico3;
                TempData["enviarA"] = ultima_orden.enviarA;


                return Redirect(Request.UrlReferrer.ToString());

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public ActionResult VerOrdenesInternamiento(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                if (User.IsInRole("Especialistas"))
                {
                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();

                    //Orden de internamiento
                    Models.SMDEVEntities32 db2 = new Models.SMDEVEntities32();


                    var orden_int = (from q in db2.Orden_Int
                                     where q.numemp == id
                                     join medicos in db2.MEDICOS on q.medico equals medicos.Numero
                                     join dhabi in db2.DHABIENTES on q.numemp equals dhabi.NUMEMP
                                     join diag1 in db2.DIAGNOSTICOS on q.diagnostico1 equals diag1.Clave into dx1
                                     join diag2 in db2.DIAGNOSTICOS on q.diagnostico2 equals diag2.Clave into dx2
                                     join diag3 in db2.DIAGNOSTICOS on q.diagnostico3 equals diag3.Clave into dx3
                                     join prov in db2.prov_subrrogados on q.proveedor equals prov.clave into provee
                                     from diagnostico1 in dx1.DefaultIfEmpty()
                                     from diagnostico2 in dx2.DefaultIfEmpty()
                                     from diagnostico3 in dx3.DefaultIfEmpty()
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
                                         diagnostico1 = diagnostico1.DesCorta,
                                         diagnostico2 = diagnostico2.DesCorta,
                                         diagnostico3 = diagnostico3.DesCorta,
                                         fecha_registro = q.fecha_registro,
                                         tipo = q.tipo,
                                     })
                                        .OrderByDescending(g => g.fecha_registro)
                                        .ToList();


                    string json = JsonConvert.SerializeObject(orden_int);
                    string path = Server.MapPath("~/json/historial_orden_int/");
                    System.IO.File.WriteAllText(path + "historial_orden_int_" + id + ".json", json);

                    return View(dhabientes);
                }
                else
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
        }

        [HttpGet]
        public ActionResult TodasOrdenesInternamiento()
        {
            if (User.IsInRole("Especialistas"))
            {
                var fecha = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-ddT00:00:00.000");
                var fecha_correcta = DateTime.Parse(fecha);
                //Orden de internamiento
                Models.SMDEVEntities32 db2 = new Models.SMDEVEntities32();

                if (User.IsInRole("RecepcionQuirofano"))
                {
                    var orden_int = (from q in db2.Orden_Int
                                     where q.fecha_registro > fecha_correcta
                                     where q.tipo == 1
                                     join medicos in db2.MEDICOS on q.medico equals medicos.Numero
                                     join dhabi in db2.DHABIENTES on q.numemp equals dhabi.NUMEMP
                                     join diag1 in db2.DIAGNOSTICOS on q.diagnostico1 equals diag1.Clave into dx1
                                     join diag2 in db2.DIAGNOSTICOS on q.diagnostico2 equals diag2.Clave into dx2
                                     join diag3 in db2.DIAGNOSTICOS on q.diagnostico3 equals diag3.Clave into dx3
                                     join prov in db2.prov_subrrogados on q.proveedor equals prov.clave into provee
                                     from diagnostico1 in dx1.DefaultIfEmpty()
                                     from diagnostico2 in dx2.DefaultIfEmpty()
                                     from diagnostico3 in dx3.DefaultIfEmpty()
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
                                         diagnostico1 = diagnostico1.DesCorta,
                                         diagnostico2 = diagnostico2.DesCorta,
                                         diagnostico3 = diagnostico3.DesCorta,
                                         fecha_registro = q.fecha_registro,
                                         tipo = q.tipo,
                                     })
                                     .OrderByDescending(g => g.fecha_registro)
                                     .ToList();

                    string json = JsonConvert.SerializeObject(orden_int);
                    string path = Server.MapPath("~/json/historial_orden_int/");
                    System.IO.File.WriteAllText(path + "historial_orden_int.json", json);
                }
                else
                {
                    var nombreusuario = User.Identity.GetUserName();

                    var orden_int = (from q in db2.Orden_Int
                                     where q.medico == nombreusuario
                                     where q.fecha_registro > fecha_correcta
                                     where q.tipo == 1
                                     join medicos in db2.MEDICOS on q.medico equals medicos.Numero
                                     join dhabi in db2.DHABIENTES on q.numemp equals dhabi.NUMEMP
                                     join diag1 in db2.DIAGNOSTICOS on q.diagnostico1 equals diag1.Clave into dx1
                                     join diag2 in db2.DIAGNOSTICOS on q.diagnostico2 equals diag2.Clave into dx2
                                     join diag3 in db2.DIAGNOSTICOS on q.diagnostico3 equals diag3.Clave into dx3
                                     join prov in db2.prov_subrrogados on q.proveedor equals prov.clave into provee
                                     from diagnostico1 in dx1.DefaultIfEmpty()
                                     from diagnostico2 in dx2.DefaultIfEmpty()
                                     from diagnostico3 in dx3.DefaultIfEmpty()
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
                                         diagnostico1 = diagnostico1.DesCorta,
                                         diagnostico2 = diagnostico2.DesCorta,
                                         diagnostico3 = diagnostico3.DesCorta,
                                         fecha_registro = q.fecha_registro,
                                         tipo = q.tipo,
                                     })
                                     .OrderByDescending(g => g.fecha_registro)
                                     .ToList();

                    string json = JsonConvert.SerializeObject(orden_int);
                    string path = Server.MapPath("~/json/historial_orden_int/");
                    System.IO.File.WriteAllText(path + "historial_orden_int.json", json);

                }
              

                return View();
            }
            else
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
        }


        [HttpGet]
        public ActionResult EditarOrdenInternamiento(int id)
        {
            if (id < 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                if (User.IsInRole("Especialistas"))
                {
                    //System.Diagnostics.Debug.WriteLine(id_orden);
                    //Orden de internamiento
                    Models.SMDEVEntities32 db = new Models.SMDEVEntities32();

                    var orden_interna = (from a in db.Orden_Int
                                         where a.id_orden == id
                                         select a).FirstOrDefault();

                    var fecha = DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    var fecha_correcta = DateTime.Parse(fecha);

                    if (orden_interna.fecha_registro >= fecha_correcta)
                    {
                        if (orden_interna.tipo == 0)
                        {
                            return RedirectToAction("EditarValoracionPensionistas/" + orden_interna.id_orden, "Manage");
                        }
                        else
                        {
                            return View(orden_interna);
                        }

                    }
                    else
                    {
                        TempData["message_error"] = "No es posible modificar esta orden";
                        return RedirectToAction("TodasOrdenesInternamiento", "Manage");
                    }

                }
                else
                {
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }

        }

        [HttpPost]
        public ActionResult ModificarOrdenInternamiento(Orden_Int model)
        {
            Models.SMDEVEntities32 db = new Models.SMDEVEntities32();

            Orden_Int or = (from x in db.Orden_Int
                          where x.id_orden == model.id_orden
                          select x).First();
            or.proveedor = model.proveedor;
            or.estudios = model.estudios;
            or.fecha_internamiento = model.fecha_internamiento;
            //or.fecha_alta = model.fecha_alta;
            or.motivo = model.motivo;
            or.indicaciones = model.indicaciones;
            or.diagnostico1 = model.diagnostico1;
            or.diagnostico2 = model.diagnostico2;
            or.diagnostico3 = model.diagnostico3;
            db.SaveChanges();

            return RedirectToAction("TodasOrdenesInternamiento/", "Manage");
        }


        [HttpGet]
        public ActionResult EditarValoracionPensionistas(int id)
        {
            if (id < 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine(id_orden);
                //Orden de internamiento
                Models.SMDEVEntities32 db = new Models.SMDEVEntities32();

                var orden_interna = (from a in db.Orden_Int
                                     where a.id_orden == id
                                     select a).FirstOrDefault();

                var fecha = DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss.fff");
                var fecha_correcta = DateTime.Parse(fecha);

                if (orden_interna.fecha_registro >= fecha_correcta)
                {
                    return View(orden_interna);
                }
                else
                {
                    TempData["message_error"] = "No es posible modificar esta orden";
                    return RedirectToAction("TodasOrdenesInternamiento", "Manage");
                }
            }

        }

        [HttpPost]
        public ActionResult ModificarValoracionPensionistas(Orden_Int model)
        {
            Models.SMDEVEntities32 db = new Models.SMDEVEntities32();

            Orden_Int or = (from x in db.Orden_Int
                            where x.id_orden == model.id_orden
                            select x).First();
            or.resumen_medico = model.resumen_medico;
            or.fecha_registro = model.fecha_registro;
            //or.fecha_alta = model.fecha_alta;
            or.motivo = model.motivo;
            or.indicaciones = model.indicaciones;
            or.diagnostico1 = model.diagnostico1;
            or.diagnostico2 = model.diagnostico2;
            or.diagnostico3 = model.diagnostico3;
            db.SaveChanges();

            return RedirectToAction("TodasValoracionesPensionistas/", "Manage");
        }


        [HttpGet]
        public ActionResult TodasValoracionesPensionistas()
        {
                var fecha = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-ddT00:00:00.000");
                var fecha_correcta = DateTime.Parse(fecha);
                //Orden de internamiento
                Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

                var nombreusuario = User.Identity.GetUserName();

                var orden_int = (from q in db2.Orden_Int
                                 where q.medico == nombreusuario
                                 where q.fecha_registro > fecha_correcta
                                 where q.tipo == 0
                                 join medicos in db2.MEDICOS on q.medico equals medicos.Numero
                                 join dhabi in db2.DHABIENTES on q.numemp equals dhabi.NUMEMP
                                 join diag1 in db2.DIAGNOSTICOS on q.diagnostico1 equals diag1.Clave into dx1
                                 join diag2 in db2.DIAGNOSTICOS on q.diagnostico2 equals diag2.Clave into dx2
                                 join diag3 in db2.DIAGNOSTICOS on q.diagnostico3 equals diag3.Clave into dx3
                                 join prov in db2.prov_subrrogados on q.proveedor equals prov.clave into provee
                                 from diagnostico1 in dx1.DefaultIfEmpty()
                                 from diagnostico2 in dx2.DefaultIfEmpty()
                                 from diagnostico3 in dx3.DefaultIfEmpty()
                                 from proveedor in provee.DefaultIfEmpty()
                                 join enviarA in db2.Orden_Int_Enviar on q.enviar_a equals enviarA.id into enviarAX
                                 from enviarAIn in enviarAX.DefaultIfEmpty()
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
                                     estudios = q.estudios,
                                     resumen_medico = q.resumen_medico,
                                     motivo = q.motivo,
                                     indicaciones = q.indicaciones,
                                     diagnostico1 = diagnostico1.DesCorta,
                                     diagnostico2 = diagnostico2.DesCorta,
                                     diagnostico3 = diagnostico3.DesCorta,
                                     fecha_registro = q.fecha_registro,
                                     tipo = q.tipo,
                                     enviar_a = enviarAIn.ubicacion,
                                 })
                                    .OrderByDescending(g => g.fecha_registro)
                                    .ToList();


                string json = JsonConvert.SerializeObject(orden_int);
                string path = Server.MapPath("~/json/historial_valoracion_pen/");
                System.IO.File.WriteAllText(path + "historial_valoracion_pen.json", json);

                return View();
        }

        [HttpGet]
        public JsonResult GetProveedoresOrdenInt()
        {
            Models.SMDEVEntities32 db = new Models.SMDEVEntities32();

            var proveedores = (from a in db.prov_subrrogados
                               where a.clave == "012" || a.clave == "150"
                               select a).ToList();

            return new JsonResult { Data = proveedores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ValoracionEnviarA()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var proveedores = (from a in db.Orden_Int_Enviar
                               select a).ToList();

            return new JsonResult { Data = proveedores, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetNombreDiagnosticos(string search)
        {
            Models.SMDEVEntities23 db = new Models.SMDEVEntities23();

            //System.Diagnostics.Debug.WriteLine(search);

            var diagnosticos = (from a in db.DIAGNOSTICOS
                              where a.Clave == search
                              select a).FirstOrDefault();

            var resultado = new
            {
                clave = diagnosticos.Clave,
                descorta = diagnosticos.DesCorta,
                descompleta = diagnosticos.DescCompleta,
            };

            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ServiciosMedicos()
        {
            var fecha = DateTime.Now.AddDays(-7).ToString("yyyy-MM-ddT00:00:00.000");
            //Models.SMDEVEntities19 db = new Models.SMDEVEntities19();
            //db.Database.ExecuteSqlCommand("SELECT * FROM CITAS WHERE Fecha = '"+ fecha + "'");

            using (var db = new SMDEVEntities19())
            {
                string query = "select DHABIENTES.NOMBRES+' '+DHABIENTES.APATERNO+' '+DHABIENTES.AMATERNO as title, '/ServiciosMedicos/SOAP/'+DHABIENTES.NUMEMP as url, CITAS.Fecha as start from CITAS INNER JOIN DHABIENTES ON CITAS.NumEmp=DHABIENTES.NUMEMP where CITAS.Fecha >= '"+fecha+"' AND CITAS.Medico = '02101'";
                var result = db.Database.SqlQuery<Citas>(query);
                var res = result.ToList();
                var json = JsonConvert.SerializeObject(res);
                string path = Server.MapPath("~/json/citas/");
                System.IO.File.WriteAllText(path + "citas.json", json);
                //return jsonResult;
                return View();
            }
   
        }



        public ActionResult Manuales()
        {
            return View();
        }

        public ActionResult Tutoriales()
        {
            if (User.IsInRole("Consultas"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult CapacitacionInterna()
        {
            return View();
        }

        public ActionResult Citas()
        {
            return View();
        }


        //ADMINISTRATIVO

        public ActionResult Administrativo()
        {
            return View();
        }

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Su contraseña se ha cambiado."
                : message == ManageMessageId.SetPasswordSuccess ? "Su contraseña se ha establecido."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Su proveedor de autenticación de dos factores se ha establecido."
                : message == ManageMessageId.Error ? "Se ha producido un error."
                : message == ManageMessageId.AddPhoneSuccess ? "Se ha agregado su número de teléfono."
                : message == ManageMessageId.RemovePhoneSuccess ? "Se ha quitado su número de teléfono."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generar el token y enviarlo
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Su código de seguridad es: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Enviar un SMS a través del proveedor de SMS para verificar el número de teléfono
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // Si llegamos a este punto, es que se ha producido un error, volvemos a mostrar el formulario
            ModelState.AddModelError("", "No se ha podido comprobar el teléfono");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // Si llegamos a este punto, es que se ha producido un error, volvemos a mostrar el formulario
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "Se ha quitado el inicio de sesión externo."
                : message == ManageMessageId.Error ? "Se ha producido un error."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Solicitar la redirección al proveedor de inicio de sesión externo para vincular un inicio de sesión para el usuario actual
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
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
                                && (labexp.fecha_crea > lastmonth)
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
            var FechaActual = DateTime.Now.ToString("");

            Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();
            try
            {
                db.Database.ExecuteSqlCommand("insert into lab_exp (expediente,medico_crea,fecha_crea,estado,urgente,observaciones) values ('" + NewLabExp.expediente + "','" + NewLabExp.medico_crea + "',GETDATE(),0,'" + NewLabExp.urgente + "','" + NewLabExp.observaciones + "')");
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

        public RedirectResult WebForm1()
        {
            return Redirect("~/WebForm1.aspx");
        }

        #endregion

        #region RayosX

        public ActionResult RayosX(string id,string medico)
        {

            if (id == null || medico == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == id
                                  select a).FirstOrDefault();
  
                Models.SMDEVEntitiesRayosX db3 = new Models.SMDEVEntitiesRayosX();

               var prestaciones =  db3.Database.SqlQuery<PrestacionesRayosX>("SELECT sp.codigo,sp.cantidad ,sp.numemp,sp.medico,ISNULL(sp.fecha_sol, GETDATE() - GETDATE()) As 'fecha_sol',sp.urgente_sol,ISNULL(sp.fecha_realiza, GETDATE() - GETDATE()) As 'fecha_realiza',sp.sexo,sp.edad ,sp.turno,sp.dx_sol,sp.proveedor,sp.prim_sub,sp.nota,sp.interp,sp.rea_interp,ISNULL(sp.fec_interp, GETDATE() - GETDATE()) As 'fec_interp',sp.realiza,srx2.codigo as 'rxcodigo', srx2.tipo, srx2.descripcion FROM servicio_rayosx sp INNER JOIN serv_rx_n2 srx2 ON srx2.codigo = sp.codigo WHERE sp.numemp='" + id + "' AND sp.medico=" + medico + "ORDER BY sp.fecha_realiza DESC").ToList();
                var histprestaciones = db3.Database.SqlQuery<PrestacionesRayosX>("SELECT sp.codigo,sp.cantidad ,sp.numemp,sp.medico,ISNULL(sp.fecha_sol, GETDATE() - GETDATE()) As 'fecha_sol',sp.urgente_sol,ISNULL(sp.fecha_realiza, GETDATE() - GETDATE()) As 'fecha_realiza',sp.sexo,sp.edad ,sp.turno,sp.dx_sol,sp.proveedor,sp.prim_sub,sp.nota,sp.interp,sp.rea_interp,ISNULL(sp.fec_interp, GETDATE() - GETDATE()) As 'fec_interp',sp.realiza,srx2.codigo as 'rxcodigo', srx2.tipo, srx2.descripcion FROM servicio_rayosx sp INNER JOIN serv_rx_n2 srx2 ON srx2.codigo = sp.codigo WHERE sp.numemp='" + id + "' ORDER BY sp.fecha_realiza DESC").ToList();

                ServicioPrestaciones dHabientesPrestaciones = new ServicioPrestaciones();
                dHabientesPrestaciones.DHabiente = dhabientes;
                dHabientesPrestaciones.Prestaciones = prestaciones;
                dHabientesPrestaciones.HistorialPrestaciones = histprestaciones;

                return View(dHabientesPrestaciones);
            }
        }


        public JsonResult GetCategoriasRX()
        {
            var clavemedico = User.Identity.GetUserName().Substring(0, 2);


            if (clavemedico == "02")
            {
                Models.SMDEVEntitiesRayosX db = new Models.SMDEVEntitiesRayosX();
                var CatergoriasRX = db.Database.SqlQuery<serv_rx_n1>("SELECT * FROM serv_rx_n1 WHERE clave NOT IN('005','002')").ToList();
                return new JsonResult { Data = CatergoriasRX, JsonRequestBehavior = JsonRequestBehavior.AllowGet, RecursionLimit = 100 };
            }

            else
            {

                Models.SMDEVEntitiesRayosX db = new Models.SMDEVEntitiesRayosX();
                var CatergoriasRX = db.Database.SqlQuery<serv_rx_n1>("SELECT * FROM serv_rx_n1 ").ToList();
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

        public JsonResult CrearSolicitudRX(string newSolRX,string urgencia)
        {
            PrestacionesRayosX NewLabRX = JsonConvert.DeserializeObject<PrestacionesRayosX>(newSolRX);
            Models.SMDEVEntitiesRayosX db = new Models.SMDEVEntitiesRayosX();

            try
            {
                db.Database.ExecuteSqlCommand("INSERT INTO servicio_rayosx (codigo,cantidad,numemp,medico,fecha_sol,sexo,edad,turno,nota,realiza,urgente_sol) values ('" + NewLabRX.codigo + "','" + NewLabRX.cantidad + "','" + NewLabRX.numemp +"','" + NewLabRX.medico +"',GETDATE(),'" + NewLabRX.sexo + "','" + NewLabRX.edad + "','" + NewLabRX.turno + "','" + NewLabRX.nota + "','" + NewLabRX.realiza +"','"+urgencia +"');");
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
                return new JsonResult { Data =  ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        #endregion

        #region Asistentes
        // Se usan para protección XSRF al agregar inicios de sesión externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}