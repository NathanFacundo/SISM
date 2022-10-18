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
    public class SolicitudLaboratorioController : Controller
    {
        private SMDEVEntities39 db = new SMDEVEntities39();

        // GET: SolicitudLaboratorio
        public ActionResult Index(string expediente, string medico)
        {

            if (expediente == null || medico == null)
            {
                return RedirectToAction("Index", "Home");
            }


            if (User.IsInRole("SinAgenda") == false)
            {
                var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + expediente + "' and Medico = '" + User.Identity.Name + "' and Fecha = '" + fecha2 + "'";
                var result = db4.Database.SqlQuery<Citas>(query);
                var res = result.FirstOrDefault();

                if (res != null)
                {
                    Models.SMDEVEntities39 db = new Models.SMDEVEntities39();
                    Models.SMDEVEntities20 db2 = new Models.SMDEVEntities20();

                    var dhabiente = (from a in db2.DHABIENTES
                                     where a.NUMEMP == expediente
                                     select a).FirstOrDefault();


                    var listLabExpd = new Lab_Solicitud();

                    var resultLabExp = (from labexp in db.SolicitudLab
                                        where (labexp.Solicitud_Expediente == expediente)
                                        && (labexp.Solicitud_Medico_Id == medico)
                                        select labexp).ToList();


                    listLabExpd.NUMEMP = dhabiente.NUMEMP;
                    listLabExpd.expediente = expediente;
                    listLabExpd.LabSolicitud = resultLabExp;
                    listLabExpd.DHabiente = dhabiente;
                    return View(listLabExpd);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else

            {
                Models.SMDEVEntities39 db = new Models.SMDEVEntities39();
                Models.SMDEVEntities20 db2 = new Models.SMDEVEntities20();

                var dhabiente = (from a in db2.DHABIENTES
                                 where a.NUMEMP == expediente
                                 select a).FirstOrDefault();


                var listLabExpd = new Lab_Solicitud();

                var resultLabExp = (from labexp in db.SolicitudLab
                                    where (labexp.Solicitud_Expediente == expediente)
                                      && (labexp.Solicitud_Medico_Id == medico)
                                    select labexp).ToList();


                listLabExpd.NUMEMP = dhabiente.NUMEMP;
                listLabExpd.expediente = expediente;
                listLabExpd.LabSolicitud = resultLabExp;
                listLabExpd.DHabiente = dhabiente;

                return View(listLabExpd);
            }

        }

        // GET: SolicitudLaboratorio/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SolicitudLab solicitudLab = db.SolicitudLab.Find(id);
            if (solicitudLab == null)
            {
                return HttpNotFound();
            }
            return View(solicitudLab);
        }

        // GET: SolicitudLaboratorio/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SolicitudLaboratorio/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Solicitud_Id,Paciente_Id,Paciente_Nombre,Paciente_Apellido_Paterno,Paciente_Apellido_Materno,Paciente_Sexo,Paciente_Fecha_Nacimiento,Paciente_Telefono,Paciente_Correo,Paciente_Dependencia_Id,Paciente_Dependencia_Descripcion,Solicitud_Expediente,Solicitud_Medico_Id,Solicitud_Medico_Nombre,Solicitud_Diagnostico1_Id,Solicitud_Diagnostico1_Descripcion,Solicitud_Diagnostico2_Id,Solicitud_Diagnostico2_Descripcion,Solicitud_Diagnostico3_Id,Solicitud_Diagnostico3_Descripcion,Solicitud_Urgente,Solicitud_Consultorio,Solicitud_AreaMedica_Id,Solicitud_AreaMedica_Descripcion,Enterprise_Folio")] SolicitudLab solicitudLab)
        {
            if (ModelState.IsValid)
            {
                db.SolicitudLab.Add(solicitudLab);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(solicitudLab);
        }

        // GET: SolicitudLaboratorio/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SolicitudLab solicitudLab = db.SolicitudLab.Find(id);
            if (solicitudLab == null)
            {
                return HttpNotFound();
            }
            return View(solicitudLab);
        }

        // POST: SolicitudLaboratorio/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Solicitud_Id,Paciente_Id,Paciente_Nombre,Paciente_Apellido_Paterno,Paciente_Apellido_Materno,Paciente_Sexo,Paciente_Fecha_Nacimiento,Paciente_Telefono,Paciente_Correo,Paciente_Dependencia_Id,Paciente_Dependencia_Descripcion,Solicitud_Expediente,Solicitud_Medico_Id,Solicitud_Medico_Nombre,Solicitud_Diagnostico1_Id,Solicitud_Diagnostico1_Descripcion,Solicitud_Diagnostico2_Id,Solicitud_Diagnostico2_Descripcion,Solicitud_Diagnostico3_Id,Solicitud_Diagnostico3_Descripcion,Solicitud_Urgente,Solicitud_Consultorio,Solicitud_AreaMedica_Id,Solicitud_AreaMedica_Descripcion,Enterprise_Folio")] SolicitudLab solicitudLab)
        {
            if (ModelState.IsValid)
            {
                db.Entry(solicitudLab).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(solicitudLab);
        }

        // GET: SolicitudLaboratorio/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SolicitudLab solicitudLab = db.SolicitudLab.Find(id);
            if (solicitudLab == null)
            {
                return HttpNotFound();
            }
            return View(solicitudLab);
        }

        // POST: SolicitudLaboratorio/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SolicitudLab solicitudLab = db.SolicitudLab.Find(id);
            db.SolicitudLab.Remove(solicitudLab);
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
