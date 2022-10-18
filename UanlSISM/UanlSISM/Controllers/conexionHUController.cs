using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using UanlSISM.Models;
using UanlSISM.ViewModels;

namespace UanlSISM.Controllers
{
    [Authorize(Roles = "Coordinador, Admin")]
    public class conexionHUController : Controller
    {
        //string connectionString = ConfigurationManager.ConnectionStrings["ConexionHU"].ConnectionString;
        //Models.ConexionHU objHU = new Models.ConexionHU();

        // GET: conexionHU
        public ActionResult Index()
        {
            return View();
        }
           
        // no se esta usando pero comentarlo por seguridad
        //public JsonResult GetDatos()
        //{
        //    var query = HU.sp_CensoPensionistax().ToList();
        //    return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}

        public ActionResult CensoPensionistas()
        {
            using (SIHUEntities context = new SIHUEntities())
            {
                //var query = "execute sp_CensoPensionistax";
                var query = context.sp_CensoPensionistax().ToList();

                // por buena practica es necesario hacer un viewModel
                var ViewModel = new CensoPensionistasViewModel()
                {
                    censoLista = query
                };

                return View(ViewModel);
            }
        }
    }
}