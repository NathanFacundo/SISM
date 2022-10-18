using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class VisorRequisicionController : Controller
    {
        Entities Compras =new Entities();
        // GET: VisorRequisicion
        public ActionResult VisorRequisicion()
        {
            return View();
        }
        public JsonResult GetRequisicion()
        {
            var query = Compras.Sp_Requisiciones().ToList();
            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetDetalleRequisicion()
        {
            var query = Compras.Sp_DetalleRequisiciones().ToList();
            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}