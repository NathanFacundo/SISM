using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class _ReporteAlmacenController : Controller
    {
        // GET: _ReporteAlmacen
        public ActionResult _ReporteAlmacen()
        {
            SERVMEDEntities5 Inv = new SERVMEDEntities5();
            var repsustancia = Inv.Database.SqlQuery<_ConsumoAlm>("EXEC Sp_ReporteAbastecimiento").ToList();
            DateTime dt = DateTime.Now;
            foreach (var item in repsustancia)
            {
                var Ult6Meses = new List<Tuple<string, Nullable<int>>>();
                for (int i = 0; i < 7; i++)
                {

                    if (dt.AddDays(-i).ToString("dddd") == "lunes")
                    {
                        if (item.Dia_1 != null)
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("lunes", item.Dia_1.Value));
                        else
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("lunes", 0));
                    }
                    if (dt.AddDays(-i).ToString("dddd") == "martes")
                    {
                        if (item.Dia_2 != null)
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("martes", item.Dia_2.Value));
                        else
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("martes", 0));
                    }
                    if (dt.AddDays(-i).ToString("dddd") == "miércoles")
                    {
                        if (item.Dia_3 != null)
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("miércoles", item.Dia_3.Value));
                        else
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("miércoles", 0));
                    }
                    if (dt.AddDays(-i).ToString("dddd") == "jueves")
                    {
                        if (item.Dia_4 != null)
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("jueves", item.Dia_4.Value));
                        else
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("jueves", 0));
                    }
                    if (dt.AddDays(-i).ToString("dddd") == "viernes")
                    {
                        if (item.Dia_5 != null)
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("viernes", item.Dia_5.Value));
                        else
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("viernes", 0));
                    }
                    if (dt.AddDays(-i).ToString("dddd") == "sábado")
                    {
                        if (item.Dia_6 != null)
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("sábado", item.Dia_6.Value));
                        else
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("sábado", 0));
                    }
                    if (dt.AddDays(-i).ToString("dddd") == "domingo")
                    {
                        if (item.Dia_7 != null)
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("domingo", item.Dia_7.Value));
                        else
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("domingo", 0));
                    }
                }
                item.Ult6Meses = Ult6Meses;
            }
            return PartialView(repsustancia);
        }
    }
}