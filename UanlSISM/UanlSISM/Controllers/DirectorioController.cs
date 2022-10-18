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
    [Authorize]
    public class DirectorioController : Controller
    {
        SMDEVDirectorio db = new SMDEVDirectorio();
        public ActionResult List()
        {
            var res = db.Directorio.ToList();
            return View(res);
        }

        public ActionResult PacientesHospital()
        {
            return View();
        }

    }
}
