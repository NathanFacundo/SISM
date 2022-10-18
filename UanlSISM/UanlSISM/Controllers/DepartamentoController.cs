using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class DepartamentoController : Controller
    {
        SISMEntities1 RH = new SISMEntities1();
        // GET: Departamento
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetDepartamento()
        {
            var query = (from a in RH.TblDepartamento
                         orderby a.NomDepartamento ascending
                         select new
                         {
                             _Clave = a.IdDepartamento,
                             _Departamento = a.NomDepartamento

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}