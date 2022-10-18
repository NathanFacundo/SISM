using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UanlSISM.Controllers
{
    public class MenuRHController : Controller
    {
        // GET: MenuRH
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuReportes()
        {
            return View();
        }
        public ActionResult MenuCatalogosRH()
        {
            return View();        
        }
    }
}