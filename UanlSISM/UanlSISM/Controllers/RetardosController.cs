using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class RetardosController : Controller
    {
        SISMEntities1 SISMRH = new SISMEntities1();  
        // GET: ReporteRetardos
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetReporteFechas(DateTime FI, DateTime FF)
        {
            var query = SISMRH.SpReporteRetardos(FI, FF).ToList();
            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}