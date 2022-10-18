using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{

    [Authorize]
    public class NotaIngresoController : Controller
    {

        public ActionResult Index(string med, string expd,int folio)
        {
            if (med == null || expd == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabientes = (from a in db.DHABIENTES
                              where a.NUMEMP == expd
                              select a).FirstOrDefault();

            Models.SMDEVNotaIngreso db2 = new Models.SMDEVNotaIngreso();
            var notasIng = (from nota in db2.NotaIngreso
                            where
                            nota.Medico == med &&
                            nota.NumEmp == expd &&
                            nota.OrdenFolio == folio
                            select nota).ToList();

            var notaIng = new NotaIngreso
            { OrdenFolio = folio };
            
            NotadeIngreso NotaIngreso = new NotadeIngreso();
            NotaIngreso.DHabiente = dhabientes;
            NotaIngreso.NotasIngreso = notasIng;
            NotaIngreso.NotaIngreso = notaIng;

            return View(NotaIngreso);
        }

        public ActionResult NotaIngreso(string id, string expd,int folio)
        {

            if (expd == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == expd
                                  select a).FirstOrDefault();

                Models.SMDEVNotaIngreso db2 = new Models.SMDEVNotaIngreso();

                var IngresoId = Convert.ToInt32(id);
                var notaIng = (from nota in db2.NotaIngreso
                               where nota.IngresoId == IngresoId
                               select nota).FirstOrDefault();
                if (notaIng == null)
                {
                    notaIng = new NotaIngreso
                    { OrdenFolio = folio };
                }

                NotadeIngreso NotaIngreso = new NotadeIngreso();
                NotaIngreso.DHabiente = dhabientes;
                NotaIngreso.NotaIngreso = notaIng;


                return View(NotaIngreso);
            }
        }

        public JsonResult CrearNotaIngreso(string NotaIng)
        {

            NotaIngreso NewNotaIng = JsonConvert.DeserializeObject<NotaIngreso>(NotaIng);
            Models.SMDEVNotaIngreso db = new Models.SMDEVNotaIngreso();

            try
            {
                if(ModelState.IsValid)
                {
                    NewNotaIng.Fecha = DateTime.Now;
                    NewNotaIng.UsuarioCreacion = User.Identity.GetUserName();
                    db.NotaIngreso.Add(NewNotaIng);
                    db.SaveChanges();
                }

                else
                {
                    return new JsonResult { Data = "Invalid Model", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        public JsonResult ActNotaIngreso(string NotaIng)
        {
            NotaIngreso NewLabRX = JsonConvert.DeserializeObject<NotaIngreso>(NotaIng);
            Models.SMDEVNotaIngreso db = new Models.SMDEVNotaIngreso();

            try
            {
                db.Database.ExecuteSqlCommand("UPDATE NotaIngreso set T='" + NewLabRX.T + "',A='" + NewLabRX.A + "',Pulso=" + NewLabRX.Pulso + ",Respiracion=" + NewLabRX.Respiracion + ",Temp=" + NewLabRX.Temp + ",MotivoIngreso='" + NewLabRX.MotivoIngreso + "',ResumenInterrogatorio='" + NewLabRX.ResumenInterrogatorio + "',ResLabGabinete='" + NewLabRX.ResLabGabinete + "',DiagnosticoPresuntivo='" + NewLabRX.DiagnosticoPresuntivo + "',PlanManejo='" + NewLabRX.PlanManejo + "',Pronostico='" + NewLabRX.Pronostico + "',DiagnosticoDesc='" + NewLabRX.DiagnosticoDesc + "'" + "WHERE IngresoId=" + NewLabRX.IngresoId);
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult Test(string id)
        {

            if(id== "1")
            {
                return new JsonResult { Data = "tiene", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult { Data = "no tiene", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

    }
}
