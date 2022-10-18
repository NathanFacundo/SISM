using System;
using System.Collections.Generic;
using System.Linq;
using UanlSISM.Models;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace UanlSISM.Controllers
{
    //[Authorize]
    public class VisorMedicamentosController : Controller
    {        
        SERVMEDEntities5 db = new SERVMEDEntities5();
        public ActionResult VisorMed()
        {
            return View();
        }
        public ActionResult VistaReporte()
        {
             //var repsustancia = db.Database.SqlQuery<_ConsumoMederos>("EXEC Sp_ReporteMederos").ToList();
             //var ListaMed = db.Database.SqlQuery<Store_Visor>("EXEC Sp_GetInfInv").ToList();
            var query = db.Sp_GetInfInv1().ToList();
            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ListaMed(string Codigo)
        {
            var IdSustancia = (from a in db.Sustancia
                               where a.Clave == Codigo
                               select new
                               {
                                   a.Id
                               }
                         ).First();

            var ListaMed = (from a in db.CodigoBarras
                            join b in db.Sustancia on a.Id_Sustancia equals b.Id
                            where a.Id_Sustancia == IdSustancia.Id
                            select new
                            {
                                b.descripcion_21,
                                a.Codigo_Barra,
                                a.Nombre_Comercial

                            }).ToList();

            return new JsonResult { Data = ListaMed, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetGrupo_21()
        {
            var Grupo = (from a in db.grupo_21
                         select new
                         {
                             a.clave,
                             a.descripcion

                         }).ToList();

            return new JsonResult { Data = Grupo, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public void GuadarDetalleMed(string Clave,string SobranteInv2022,int IdGrupo,int IdNivel)
        {
            try
            {
                var Sustancia = (from a in db.Sustancia
                                 where a.Clave == Clave
                                 select a
                                ).FirstOrDefault();

                if (SobranteInv2022 =="0")
                { }
                else
                {
                    Sustancia.SobranteInv2022 = SobranteInv2022;
                }
                if(IdGrupo==0)
                { }
                else
                {
                    Sustancia.id_grupo_21 = IdGrupo;
                }
                if(IdNivel==0)
                { }
                else
                {
                    Sustancia.Nivel_21 = IdNivel;
                }
                db.SaveChanges();
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
        }
        public JsonResult GetDatos(string Clave)
        {
            var Medicamento = (from a in db.Sustancia
                               join b in db.grupo_21 on a.id_grupo_21 equals b.id
                               where a.Clave == Clave 
                               select new
                               {
                                   a.SobranteInv2022,
                                   b.clave,
                                   a.Nivel_21

                               }).ToList();

            return new JsonResult { Data = Medicamento, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}