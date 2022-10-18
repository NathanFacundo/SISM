using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UanlSISM.Controllers
{
    public class CuadroBasicoController : Controller
    {
        [Authorize]
        public ActionResult CuadroBasico()
        {
            return View();
        }

        Models.SERVMEDEntities4 DAM = new Models.SERVMEDEntities4();

        public ActionResult ObtenerMedicamentos_CB()
        {
            try
            {
                var query = (from a in DAM.Sustancia
                             join g in DAM.grupo_21 on a.id_grupo_21 equals g.id
                             where a.LicitacionStatus == 1 
                             select new
                             {
                                 a.Clave,
                                 a.descripcion_21,
                                 //a.id_grupo_21,
                                 grupo = g.descripcion,
                                 a.Nivel_21
                             }).ToList();

                
                return Json(new { MENSAJE = "FOUND", MED = query }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
