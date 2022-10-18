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
    public class EstudioMSController : Controller
    {
        private SMDEVEstudioMS db = new SMDEVEstudioMS();

        // GET: EstudioMS
        public ActionResult Index()
        {
            return View(db.EstudioMedicoSocial.ToList());
        }

        // GET: EstudioMS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstudioMedicoSocial estudioMedicoSocial = db.EstudioMedicoSocial.Find(id);
            if (estudioMedicoSocial == null)
            {
                return HttpNotFound();
            }
            return View(estudioMedicoSocial);
        }

        // GET: EstudioMS/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstudioMS/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EstudioId,Empleado,CveFam,Depcia,Fecha,FechaIngreso,Nombre,Edad,Sexo,EdoCivil,Domicilio,Col,Municipio,Tel,Escolaridad,Ocupacion,Religion,ViveCon,CuidadorPrincipal,TipoFamilia,FaseFamilia,Eutimia,Hipertimia,Distimia,DiagnosticoMedicoIngreso,MedicoAtiende,FrecuenciaAcude,MedicoEspecialidadAcude,Quemaduras,Infarto,Accidentes,Derrames,Cancer,Traumatismo,Embolia,Ninguna,InsuficienciaRenal,Diabetes,Hipertension,EnfermedadesCardiacas,FiebreRemautica,ArtritisDegenerativa,TumoresCerebrales,MalformacionesCongenitas,PxTransplantado,FibrosisQuistica,LupusEritematoso,EsclerosisLateral,EsclerosisMultiple,Hidrocefalia,EspinaBifida,IntervencionCrisisPaciente,CasoSocial,ConsultaAsesoria,VisitaDomiciliaria,IntervencionFamiliar,CanalizacionPsicologica,OrientacionTramitesAdmin,CanalizacionOtraInstitucion,OtraIntervencionTipo,ValoracionPsicoSocial,Diagnostico,EstrategiaTrabajo,ResumenCaso,Observaciones")] EstudioMedicoSocial estudioMedicoSocial)
        {
            if (ModelState.IsValid)
            {
                db.EstudioMedicoSocial.Add(estudioMedicoSocial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estudioMedicoSocial);
        }

        // GET: EstudioMS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstudioMedicoSocial estudioMedicoSocial = db.EstudioMedicoSocial.Find(id);
            if (estudioMedicoSocial == null)
            {
                return HttpNotFound();
            }
            return View(estudioMedicoSocial);
        }

        // POST: EstudioMS/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EstudioId,Empleado,CveFam,Depcia,Fecha,FechaIngreso,Nombre,Edad,Sexo,EdoCivil,Domicilio,Col,Municipio,Tel,Escolaridad,Ocupacion,Religion,ViveCon,CuidadorPrincipal,TipoFamilia,FaseFamilia,Eutimia,Hipertimia,Distimia,DiagnosticoMedicoIngreso,MedicoAtiende,FrecuenciaAcude,MedicoEspecialidadAcude,Quemaduras,Infarto,Accidentes,Derrames,Cancer,Traumatismo,Embolia,Ninguna,InsuficienciaRenal,Diabetes,Hipertension,EnfermedadesCardiacas,FiebreRemautica,ArtritisDegenerativa,TumoresCerebrales,MalformacionesCongenitas,PxTransplantado,FibrosisQuistica,LupusEritematoso,EsclerosisLateral,EsclerosisMultiple,Hidrocefalia,EspinaBifida,IntervencionCrisisPaciente,CasoSocial,ConsultaAsesoria,VisitaDomiciliaria,IntervencionFamiliar,CanalizacionPsicologica,OrientacionTramitesAdmin,CanalizacionOtraInstitucion,OtraIntervencionTipo,ValoracionPsicoSocial,Diagnostico,EstrategiaTrabajo,ResumenCaso,Observaciones")] EstudioMedicoSocial estudioMedicoSocial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estudioMedicoSocial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estudioMedicoSocial);
        }

        // GET: EstudioMS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EstudioMedicoSocial estudioMedicoSocial = db.EstudioMedicoSocial.Find(id);
            if (estudioMedicoSocial == null)
            {
                return HttpNotFound();
            }
            return View(estudioMedicoSocial);
        }

        // POST: EstudioMS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EstudioMedicoSocial estudioMedicoSocial = db.EstudioMedicoSocial.Find(id);
            db.EstudioMedicoSocial.Remove(estudioMedicoSocial);
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
