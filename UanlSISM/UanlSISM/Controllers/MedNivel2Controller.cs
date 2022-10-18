using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class MedNivel2Controller : Controller
    {
        //SERVMEDMedNivel2 db = new SERVMEDMedNivel2();
        SERVMEDEntities6 db = new SERVMEDEntities6();
        SERVMEDEntities5 db2 = new SERVMEDEntities5();

        public ActionResult MNivel2()
        {
            if (User.IsInRole("Almacen") || User.IsInRole("FarmaciaCatalogos")) {
                    ViewBag.Total = (from a in db2.Sustancia
                                 where a.SobranteInv2022 == "2"
                                 select a).Count();

                //ViewBag.Total = db2.Sustancia.Where(ñ=>ñ.SobranteInv2022 = 2).firstor
                return View();

            }
            else
            {
                return RedirectToAction("Medico", "Manage");
            }
        }

        [HttpPost]
        public ActionResult ObtenerNivel2(int idUnidad)
        {
            try
            {
                var query = db.SP_Temporales(idUnidad).ToList();

                return Json(new { MENSAJE = "FOUND", SUSTANCIAS = query }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}