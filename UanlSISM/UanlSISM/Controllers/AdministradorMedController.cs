using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class AdministradorMedController : Controller
    {
        SERVMEDEntities5 db = new SERVMEDEntities5();
        // GET: AdministradorMed
        public ActionResult AdministradorMed()
        {
            return View();
        }
        public void GuadarDetalleMed(string Clave, string SobranteInv2022, int IdGrupo, int IdNivel,int Licitado)
        {
            try
            {
                var Sustancia = (from a in db.Sustancia
                                 where a.Clave == Clave
                                 select a
                                ).FirstOrDefault();

                if (SobranteInv2022 == "0")
                { }
                else
                {
                    Sustancia.SobranteInv2022 = SobranteInv2022;
                }
                if (IdGrupo == 0)
                { }
                else
                {
                    Sustancia.id_grupo_21 = IdGrupo;
                }
                if (IdNivel == 0)
                { }
                else
                {
                    Sustancia.Nivel_21 = IdNivel;
                }
                if (Licitado == 0)
                { }
                else
                {
                    Sustancia.LicitacionStatus = Licitado;
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
    }
}