using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    [Authorize]

    public class NotaOperatoriaController : Controller
    {
        private SMDEVNotaOperatoria db = new SMDEVNotaOperatoria();

  
        public ActionResult Index(string med, string expd, int folio)
        {
            if (med == null || expd == null || folio == 0)
            {
                return RedirectToAction("Index", "Home");

            }

            Models.SMDEVEntities20 dbDh = new Models.SMDEVEntities20();
            var dhabientes = (from a in dbDh.DHABIENTES
                              where a.NUMEMP == expd
                              select a).FirstOrDefault();

            var notas = (from n in db.NotaOperatoria
                         where
                         n.Medico == med &&
                         n.NumEmp == expd &&
                         n.OrdenFolio == folio
                         select n).ToList();

            NotasOperatorias notaOperatoria = new NotasOperatorias();
            NotaOperatoria NotaOper = new NotaOperatoria();
            NotaOper.OrdenFolio = folio;

            notaOperatoria.DHabiente = dhabientes;
            notaOperatoria.NotasOperatoria = notas;
            notaOperatoria.NotaOperatoria = NotaOper;

            return View(notaOperatoria);
        }

      
        public ActionResult Create(string med, string expd, int folio)
        {

            //Models.SMDEVNotaPreoperatoria dbNotaPreop = new Models.SMDEVNotaPreoperatoria();
            //var notaPreoperatoria = (from notaPreop in dbNotaPreop.NotaPreoperatoria
            //               select notaPreop).ToList().LastOrDefault();

            //if (notaPreoperatoria == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //var res = (from n in db.NotaOperatoria
            //           where
            //           n.PreOperatoriaId == notaPreoperatoria.PreOperatoriaId
            //           select n).ToList();


            if (med == null || expd == null)
            {
                return RedirectToAction("Index", "Home");

            }

            Models.SMDEVEntities20 dbDh = new Models.SMDEVEntities20();
            var dhabientes = (from a in dbDh.DHABIENTES
                              where a.NUMEMP == expd
                              select a).FirstOrDefault();

            var nota = new NotaOperatoria();
            nota.OrdenFolio = folio;
            nota.PreOperatoriaId = 1;
            nota.Medico = med;
            nota.NumEmp = expd;
            nota.FechaCreacion = DateTime.Now;
            nota.UsuarioCreacion = User.Identity.GetUserName();


            NotasOperatorias notasOperatorias = new NotasOperatorias();
            notasOperatorias.DHabiente = dhabientes;
            notasOperatorias.NotaOperatoria = nota;

            return View(notasOperatorias);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OperatoriaId,OrdenFolio,PreOperatoriaId,Medico,NumEmp,ResumenClinico,ResEstudiosDiagnosticos,DiagnosticoIngreso,DiagnosticoIngresoDesc,PlanTerCirujiaPlaneada,RiesgoQuirurgico,CirujiaRealizada,DiagnosticoEgreso,DiagnosticoEgresoDesc,TipoAnestecia,DescTecnicaQuirurgicaTerapeutica,Hallazgos,Incidentes,Sangrado,MotivoEgreso,EstadoActualProblemasClinicos,PlanManejo,Pronostico,UsuarioCreacion,FechaCreacion")] NotaOperatoria notaOperatoria)
        {

            try
            {

                if (ModelState.IsValid)
                {
                    db.NotaOperatoria.Add(notaOperatoria);
                    db.SaveChanges();
                    return RedirectToAction("ProcAmbOrdenes", "ServiciosMedicos", new { id = notaOperatoria.NumEmp });
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index", "Home");
        }

     
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NotaOperatoria notaOperatoria = db.NotaOperatoria.Find(id);

            if (notaOperatoria == null)
            {
                return HttpNotFound();
            }


            Models.SMDEVEntities20 dbDh = new Models.SMDEVEntities20();
            var dhabientes = (from a in dbDh.DHABIENTES
                              where a.NUMEMP == notaOperatoria.NumEmp
                              select a).FirstOrDefault();


            NotasOperatorias NotaOp = new NotasOperatorias();
            NotaOp.DHabiente = dhabientes;
            NotaOp.NotaOperatoria = notaOperatoria;

            return View(NotaOp);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NotaOperatoria notaOperatoria = db.NotaOperatoria.Find(id);

            if (notaOperatoria == null)
            {
                return HttpNotFound();
            }


            Models.SMDEVEntities20 dbDh = new Models.SMDEVEntities20();
            var dhabientes = (from a in dbDh.DHABIENTES
                              where a.NUMEMP == notaOperatoria.NumEmp
                              select a).FirstOrDefault();


            NotasOperatorias NotaOp = new NotasOperatorias();
            NotaOp.DHabiente = dhabientes;
            NotaOp.NotaOperatoria = notaOperatoria;

            return View(NotaOp);


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OperatoriaId,OrdenFolio,PreOperatoriaId,Medico,NumEmp,ResumenClinico,ResEstudiosDiagnosticos,DiagnosticoIngreso,DiagnosticoIngresoDesc,PlanTerCirujiaPlaneada,RiesgoQuirurgico,CirujiaRealizada,DiagnosticoEgreso,DiagnosticoEgresoDesc,TipoAnestecia,DescTecnicaQuirurgicaTerapeutica,Hallazgos,Incidentes,Sangrado,MotivoEgreso,EstadoActualProblemasClinicos,PlanManejo,Pronostico,UsuarioCreacion,FechaCreacion")] NotaOperatoria notaOperatoria)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(notaOperatoria).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ProcAmbOrdenes", "ServiciosMedicos", new { id = notaOperatoria.NumEmp });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Index", "Home");
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NotaOperatoria notaOperatoria = db.NotaOperatoria.Find(id);
            if (notaOperatoria == null)
            {
                return HttpNotFound();
            }
            return View(notaOperatoria);
        }

     
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NotaOperatoria notaOperatoria = db.NotaOperatoria.Find(id);
            db.NotaOperatoria.Remove(notaOperatoria);
            db.SaveChanges();
            return RedirectToAction("Index");
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
