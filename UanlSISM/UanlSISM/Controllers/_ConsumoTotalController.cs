using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class _ConsumoTotalController : Controller
    {
        // GET: _ConsumoTotal
        public ActionResult _ConsumoTotal()
        {
            SERVMEDEntities5 Inv = new SERVMEDEntities5();
            var repsustancia = Inv.Database.SqlQuery<_TotalConsumo>("EXEC Sp_ConsumoTotal").ToList();
            return PartialView(repsustancia);
        }
    }
}