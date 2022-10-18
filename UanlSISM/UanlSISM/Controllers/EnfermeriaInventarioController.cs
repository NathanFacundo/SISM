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
    public class EnfermeriaInventarioController : Controller
    {
        private SMDEVEnfermeriaInventario db = new SMDEVEnfermeriaInventario();

        // GET: EnfermeriaInventario
        public ActionResult Index()
        {
            return View(db.EnfermeriaInventario.ToList());
        }

        // GET: EnfermeriaInventario/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnfermeriaInventario enfermeriaInventario = db.EnfermeriaInventario.Find(id);
            if (enfermeriaInventario == null)
            {
                return HttpNotFound();
            }
            return View(enfermeriaInventario);
        }

        // GET: EnfermeriaInventario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EnfermeriaInventario/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodProducto,Descripcion,Status,Tipo,Entradas,Salidas,Stock")] EnfermeriaInventario enfermeriaInventario)
        {
            if (ModelState.IsValid)
            {
                db.EnfermeriaInventario.Add(enfermeriaInventario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(enfermeriaInventario);
        }

        // GET: EnfermeriaInventario/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnfermeriaInventario enfermeriaInventario = db.EnfermeriaInventario.Find(id);
            if (enfermeriaInventario == null)
            {
                return HttpNotFound();
            }
            return View(enfermeriaInventario);
        }

        // POST: EnfermeriaInventario/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodProducto,Descripcion,Status,Tipo,Entradas,Salidas,Stock")] EnfermeriaInventario enfermeriaInventario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enfermeriaInventario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enfermeriaInventario);
        }

        // GET: EnfermeriaInventario/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EnfermeriaInventario enfermeriaInventario = db.EnfermeriaInventario.Find(id);
            if (enfermeriaInventario == null)
            {
                return HttpNotFound();
            }
            return View(enfermeriaInventario);
        }

        // POST: EnfermeriaInventario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            EnfermeriaInventario enfermeriaInventario = db.EnfermeriaInventario.Find(id);
            db.EnfermeriaInventario.Remove(enfermeriaInventario);
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
