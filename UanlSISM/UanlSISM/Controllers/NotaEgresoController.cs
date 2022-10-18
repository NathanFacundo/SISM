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
    public class NotaEgresoController : Controller
    {
        private SMDEVNotaEgreso db = new SMDEVNotaEgreso();

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

            var notas = (from n in db.NotaEgreso
                         where
                         n.Medico == med &&
                         n.NumEmp == expd &&
                         n.OrdenFolio == folio
                         select n).ToList();

            NotadeEgreso NotadeEgreso = new NotadeEgreso();
            NotaEgreso notaEgreso = new NotaEgreso();
            notaEgreso.OrdenFolio = folio;

            NotadeEgreso.DHabiente = dhabientes;
            NotadeEgreso.NotasEgreso = notas;
            NotadeEgreso.NotaEgreso = notaEgreso;

            return View(NotadeEgreso);
        }


        public ActionResult Create(string med, string expd,int folio)
        {

             SMDEVNotaOperatoria dbNotaPreop = new SMDEVNotaOperatoria();
             var notaOp = (from n in dbNotaPreop.NotaOperatoria
                         where
                         n.Medico == med &&
                         n.NumEmp == expd &&
                         n.OrdenFolio == folio
                         select n).ToList().LastOrDefault();


            Models.SMDEVEntities20 dbDh = new Models.SMDEVEntities20();
            var dhabientes = (from a in dbDh.DHABIENTES
                              where a.NUMEMP == expd
                              select a).FirstOrDefault();

            var nota = new NotaEgreso();
            nota.OrdenFolio = folio;
            nota.OperatoriaId = 1;
            nota.Medico = med;
            nota.NumEmp = expd;
            nota.FechaCreacion = DateTime.Now;
            nota.UsuarioCreacion = User.Identity.GetUserName();

            NotadeEgreso notadeEgreso = new NotadeEgreso();
            notadeEgreso.DHabiente = dhabientes;
            notadeEgreso.NotaEgreso = nota;

            return View(notadeEgreso);
        }


        // POST: NotaEgresoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EgresoId,OrdenFolio,Medico,NumEmp,DiagnosticoFinal,DiagnosticoDesc,ResumenClinico,ProcedimientoRealizado,ProcProblemasPendientes,Medicacion,RecomendacionesIncapacidad,Pronostico,UsuarioCreacion,FechaCreacion,OperatoriaId")] NotaEgreso notaEgreso)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    db.NotaEgreso.Add(notaEgreso);
                    db.SaveChanges();
                    return RedirectToAction("ProcAmbOrdenes", "ServiciosMedicos", new { id = notaEgreso.NumEmp });
                }

            }

            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ProcAmbOrdenes", "ServiciosMedicos", new { id = notaEgreso.NumEmp });

        }

        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Home");

            }

            NotaEgreso nota = db.NotaEgreso.Find(id);

            if (nota == null)
            {
                return RedirectToAction("Index", "Home");
            }

            Models.SMDEVEntities20 dbDh = new Models.SMDEVEntities20();
            var dhabientes = (from a in dbDh.DHABIENTES
                              where a.NUMEMP == nota.NumEmp
                              select a).FirstOrDefault();

            NotadeEgreso NotaEgreso = new NotadeEgreso();
            NotaEgreso.DHabiente = dhabientes;
            NotaEgreso.NotaEgreso = nota;

            return View(NotaEgreso);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EgresoId,OrdenFolio,Medico,NumEmp,DiagnosticoFinal,DiagnosticoDesc,ResumenClinico,ProcedimientoRealizado,ProcProblemasPendientes,Medicacion,RecomendacionesIncapacidad,Pronostico,UsuarioCreacion,FechaCreacion,OperatoriaId")] NotaEgreso notaEgreso)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(notaEgreso).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("ProcAmbOrdenes", "ServiciosMedicos", new { id = notaEgreso.NumEmp });
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("ProcAmbOrdenes", "ServiciosMedicos", new { id = notaEgreso.NumEmp });
        }


        #region "AgendaTel"


        public ActionResult AgendaTel()
        {


            return View();
        }

        #endregion
    }
}