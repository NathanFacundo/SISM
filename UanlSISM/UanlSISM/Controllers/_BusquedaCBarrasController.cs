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
    public class _BusquedaCBarrasController : Controller
    {
        SERVMEDEntities5 db = new SERVMEDEntities5();
        // GET: _BusquedaCBarras
        public ActionResult _BusquedaCBarras()
        {
            var repsustancia = db.Database.SqlQuery<Sp_GetListaMed_Result>("EXEC Sp_GetListaMed").ToList();
            return PartialView(repsustancia);
        }
    }
}