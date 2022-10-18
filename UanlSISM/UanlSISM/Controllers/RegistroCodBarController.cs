using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UanlSISM.Models;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace UanlSISM.Controllers
{
    public class RegistroCodBarController : Controller
    {
        SERVMEDEntities5 db = new SERVMEDEntities5();
        int Respuesta;
        // GET: RegistroCodBar
        public ActionResult RegistroCodBar()
        {
            return View();
        }
        public JsonResult GetCodBar2022()
        {
            var query = db.SP_ViewCodBar2022();
            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public int UpdateCodigoBarras(string Clave, string NomComercial,string CodBar)
        {
            try
            {
                var Sustancia = (from a in db.Sustancia
                                 where a.Clave == Clave
                                 select new {
                                     a.Id
                                 }).FirstOrDefault();

                var query = (from a in db.CodigoBarras
                             where a.Id_Sustancia == Sustancia.Id && a.Nombre_Comercial == NomComercial
                             select a).FirstOrDefault();

                query.Codigo_Barra = CodBar;
                query.Contenido = 1;
                query.Status = true;
                query.Fecha = DateTime.Now; 

                db.SaveChanges();
                Respuesta = 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return Respuesta;
        }
        public JsonResult ValidaExistencia(string Codigo)
        {
            var query = (from a in db.CodigoBarras
                         where a.Codigo_Barra == Codigo
                         select new
                         {
                             a.Nombre_Comercial

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ValidarNuevo(string NomComercial, string CodBar)
        {
            var Valida = (from a in db.CodigoBarras
                          where a.Codigo_Barra.Contains(CodBar) && a.Nombre_Comercial.EndsWith(NomComercial)
                          select a).ToList();

            return new JsonResult { Data = Valida, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public void SaveCodigoNuevo(string Clave, string NomComercial, string CodBar)
        {
            var query = db.SP_MedNuevos(Clave, NomComercial, CodBar);
        }
    }
}