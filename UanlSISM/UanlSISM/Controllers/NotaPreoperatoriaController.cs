using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;
using Microsoft.AspNet.Identity;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class NotaPreoperatoriaController : Controller
    {
        private SMDEVNotaPreoperatoria db = new SMDEVNotaPreoperatoria();


        public ActionResult Index(string med, string expd,int folio)
        {

            if (med == null || expd == null)
            {
                return RedirectToAction("Index", "Home");

            }

            Models.SMDEVEntities20 dbDh = new Models.SMDEVEntities20();
            var dhabientes = (from a in dbDh.DHABIENTES
                              where a.NUMEMP == expd
                              select a).FirstOrDefault();

            var notas = (from n in db.NotaPreoperatoria
                         where
                         n.Medico == med &&
                         n.NumEmp == expd &&
                         n.OrdenFolio == folio
                         select n).ToList();

            var notaPreop = new NotaPreoperatoria
            {
                OrdenFolio= folio
            };

            NotadePreop NotaPreop = new NotadePreop();
            NotaPreop.DHabiente = dhabientes;
            NotaPreop.NotaPreoperatoria = notaPreop;
            NotaPreop.NotasPreoperatoria = notas;


            return View(NotaPreop);
        }



        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotaPreoperatoria notaPreoperatoria = db.NotaPreoperatoria.Find(id);
            if (notaPreoperatoria == null)
            {
                return HttpNotFound();
            }
            return View(notaPreoperatoria);
        }


        public ActionResult Create(string med, string expd,int folio)
        {
            Models.SMDEVNotaIngreso dbNotaIng = new Models.SMDEVNotaIngreso();
            var notaIng = (from notaIngreso in dbNotaIng.NotaIngreso
                           select notaIngreso).ToList().LastOrDefault();

            if(notaIng == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var res = (from n in db.NotaPreoperatoria
                       where
                       n.IngresoId == notaIng.IngresoId
                       select n).ToList();

   
            if (med == null || expd == null || res.Count > 0)
            {
                return RedirectToAction("Index", "Home");

            }

            Models.SMDEVEntities20 dbDh = new Models.SMDEVEntities20();
            var dhabientes = (from a in dbDh.DHABIENTES
                              where a.NUMEMP == expd
                              select a).FirstOrDefault();

            var nota = new NotaPreoperatoria();
            nota.OrdenFolio =folio;
            nota.IngresoId = notaIng.IngresoId;
            nota.Medico = med;
            nota.NumEmp = expd;
            nota.Fecha = DateTime.Now;
            nota.UsuarioCreacion = User.Identity.GetUserName();


            NotadePreop NotaPreop = new NotadePreop();
            NotaPreop.DHabiente = dhabientes;
            NotaPreop.NotaPreoperatoria = nota;

            return View(NotaPreop);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PreOperatoriaId,Medico,NumEmp,Fecha,Diagnostico,DiagnosticoDesc,PlanesCuidados,PlanQuirurgico,FactoresRiesgo,RiesgoQuirurgico,Pronostico,OrdenFolio,UsuarioCreacion,IngresoId")] NotaPreoperatoria notaPreoperatoria)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    db.NotaPreoperatoria.Add(notaPreoperatoria);
                    db.SaveChanges();
                    return RedirectToAction("ProcAmbOrdenes", "ServiciosMedicos", new { id = notaPreoperatoria.NumEmp});
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index", new { med = notaPreoperatoria.Medico, expd = notaPreoperatoria.NumEmp, folio = notaPreoperatoria.OrdenFolio });
        }


        public ActionResult Edit(int id)
        {
            if ( id == 0)
            {
                return RedirectToAction("Index", "Home");

            }

            NotaPreoperatoria notaPreoperatoria = db.NotaPreoperatoria.Find(id);

          
            if (notaPreoperatoria == null)
            {
                return RedirectToAction("Index", "Home");
            }


            Models.SMDEVEntities20 dbDh = new Models.SMDEVEntities20();
            var dhabientes = (from a in dbDh.DHABIENTES
                                where a.NUMEMP == notaPreoperatoria.NumEmp
                                select a).FirstOrDefault();


            NotadePreop NotaPreop = new NotadePreop();
            NotaPreop.DHabiente = dhabientes;

            NotaPreop.NotaPreoperatoria = notaPreoperatoria;

            return View(NotaPreop);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PreOperatoriaId,Medico,NumEmp,Fecha,Diagnostico,DiagnosticoDesc,PlanesCuidados,PlanQuirurgico,FactoresRiesgo,RiesgoQuirurgico,Pronostico,OrdenFolio,UsuarioCreacion,IngresoId")] NotaPreoperatoria notaPreoperatoria)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(notaPreoperatoria).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ProcAmbOrdenes", "ServiciosMedicos", new { id = notaPreoperatoria.NumEmp });

                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index", new { med = notaPreoperatoria.Medico, expd = notaPreoperatoria.NumEmp, folio = notaPreoperatoria.OrdenFolio });

        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotaPreoperatoria notaPreoperatoria = db.NotaPreoperatoria.Find(id);
            if (notaPreoperatoria == null)
            {
                return HttpNotFound();
            }
            return View(notaPreoperatoria);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NotaPreoperatoria notaPreoperatoria = db.NotaPreoperatoria.Find(id);
            db.NotaPreoperatoria.Remove(notaPreoperatoria);
            db.SaveChanges();
            return RedirectToAction("Index", "NotaPreoperatoria");
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
