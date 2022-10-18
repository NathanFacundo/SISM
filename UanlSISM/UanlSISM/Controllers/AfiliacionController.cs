using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class AfiliacionController : Controller
    {
        // GET: Afiliacion
        public ActionResult Index()
        {
            if (User.IsInRole("Afiliacion"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult CredencialDigital()
        {
            if (User.IsInRole("Afiliacion"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public JsonResult LinkCredencial(string expediente)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);

            //cs

            var dhab = (from a in db.DHABIENTES
                        where a.NUMEMP == expediente
                        select new
                        {
                            credencial_exp = a.credencial_exp,
                            numafil = a.NUMAFIL,
                        })
                        .FirstOrDefault();

            

            return new JsonResult { Data = dhab, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        private static Random random = new Random();
        public static string RandomString()
        {
            int length = 16;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        [HttpPost]
        public ActionResult GenerarCredencial(string numafil)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);

            


            var clave = RandomString();

            var dhabientes = (from a in db.DHABIENTES
                              where a.credencial_exp == clave
                              select a)
                              .FirstOrDefault();


            if (dhabientes == null)
            {
                //Tomar numemp si no tiene baja y esta vigente
                var vigentes = (from a in db.DHABIENTES
                                  where a.NUMAFIL == numafil
                                  where a.FVIGENCIA >= fechaDT
                                  where a.BAJA != "02" || a.BAJA != "04"
                                where a.credencial_exp == null
                                select a)
                              .ToList();
                foreach(var vig in vigentes)
                {
                    db.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET credencial_exp = '" + clave + "' WHERE NUMEMP = '" + vig.NUMEMP + "'");
                }

                //System.Diagnostics.Debug.WriteLine(vigentes);
                //db.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET credencial_exp = '" + clave + "' WHERE NUMAFIL = '" + numafil + "' and FVIGENCIA >= '" + fecha + "' and credencial_exp is null and BAJA != '02' and BAJA !=  '04'");
                return Redirect("http://sism.uanl.mx/SISM-Medico/Home/FormatoCD3/" + clave);
            }
            else
            {
                clave = RandomString();

                var vigentes = (from a in db.DHABIENTES
                                where a.NUMAFIL == numafil
                                where a.FVIGENCIA >= fechaDT
                                where a.BAJA != "02" || a.BAJA != "04"
                                where a.credencial_exp == null
                                select a)
                              .ToList();
                foreach (var vig in vigentes)
                {
                    db.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET credencial_exp = '" + clave + "' WHERE NUMEMP = '" + vig.NUMEMP + "'");
                }

                return Redirect("http://sism.uanl.mx/SISM-Medico/Home/FormatoCD3/" + clave);
            }



            

        }

        public ActionResult Buscar()
        {
            if (User.IsInRole("Afiliacion"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult Kardex()
        {
            if (User.IsInRole("Afiliacion"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

    }
}
