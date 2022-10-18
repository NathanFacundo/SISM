using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    [DataContract(IsReference = true)]

    public class InventarioController : Controller
    {
        UanlSISM.Models.SERVMEDEntities1 Inv = new Models.SERVMEDEntities1();
        public ActionResult InvRel()
        {
            var res = Inv.Database.SqlQuery<ReporteConsumo>("EXEC [dbo].[Sp_ReporteAbastecimiento] @Año = 2019").ToList();
            DateTime dt = DateTime.Now;

            foreach (var item in res)
            {
                var Ult6Meses = new List<Tuple<string, Nullable<int>>>();

                for (int i = 1; i < 2; i++)
                {
                  
                    /*
                    if (dt.AddMonths(-i).ToString("MMMM") == "abril")
                    {
                        if (item.abril != null)
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("abril", item.abril.Value));
                        else
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("abril", 0));
                    }
                    */
                    //if (dt.AddMonths(-i).ToString("MMMM") == "mayo")
                    //{
                    //    if (item.mayo != null)
                    //        Ult6Meses.Add(new Tuple<string, Nullable<int>>("mayo", item.mayo.Value));
                    //    else
                    //        Ult6Meses.Add(new Tuple<string, Nullable<int>>("mayo", 0));
                    //}               
                }
                //item.Ult6Meses = Ult6Meses;
            }
            return View(res);
        }
        public ActionResult ReporteConsumoInv()
        {
            UanlSISM.Models.SERVMEDEntities1 Inv = new Models.SERVMEDEntities1();
            UanlSISM.Models.SERVMEDInventario ReqOC = new Models.SERVMEDInventario();
            UanlSISM.Models.ReporteAbastecimeinto reporteConsumo = new Models.ReporteAbastecimeinto();

            var repsustancia = Inv.Database.SqlQuery<ReporteAbastecimeinto>("EXEC [dbo].[Sp_ReporteAbastecimiento] @Año =" + DateTime.Now.Year).ToList();
            var fechamenos1mes = DateTime.Now.AddMonths(-1);

            foreach (var item in repsustancia)
            {
                var requisiciones =
                (from req in ReqOC.Requisicion
                 join detreq in ReqOC.DetalleReq on req.id equals detreq.Id_Requisicion
                 join OC in ReqOC.OrdenCompra on req.id equals OC.Id_Requisicion
                 join DetalleOC in ReqOC.DetalleOC on OC.Id equals DetalleOC.Id_OrdenCompra
                 join ValeEntrada in ReqOC.ValeEntrada on OC.Id equals ValeEntrada.id_ordencompra
                 select new { req, detreq, OC, DetalleOC, ValeEntrada }).Where(x => x.detreq.Id_Sustancia == item.SustanciaId && x.req.Id_Tipo == 2 && x.req.Fecha > fechamenos1mes && x.DetalleOC.Id_Sustancia == item.SustanciaId).ToList();

                item.Requisiciones = requisiciones.Select(x => x.req).ToList();
                item.OrdenesCompras = requisiciones.Select(x => x.OC).ToList();
                item.ValeEntrada = requisiciones.Select(x => x.ValeEntrada).ToList();
                var Ult6Meses = new List<Tuple<string, Nullable<int>>>();
                item.Ult6Meses = Ult6Meses;

            }
            return View(repsustancia);
        }
        public JsonResult GetReporteConsumoInv()
        {
            UanlSISM.Models.SERVMEDEntities1 Inv = new Models.SERVMEDEntities1();
            UanlSISM.Models.SERVMEDInventario ReqOC = new Models.SERVMEDInventario();
            UanlSISM.Models.ReporteAbastecimeinto reporteConsumo = new Models.ReporteAbastecimeinto();
            var repsustancia = Inv.Database.SqlQuery<ReporteAbastecimeinto>("EXEC Sp_ReporteAbastecimiento @Año =" + DateTime.Now.Year).ToList();
            foreach (var item in repsustancia)
            {
                var Ult6Meses = new List<Tuple<string, Nullable<int>>>();
                for (int i = 1; i < 3; i++)
                {
                    var dt = DateTime.Now;
                    var newMes = new Meses();
                    if (dt.AddMonths(-i).ToString("MMMM") == "abril")
                    {
                        if (item.abril != null)
                        {
                            //newMes.Item2 = item.abril;
                            //newMes.Item1 = "abril";
                        }
                        else
                        {
                            //newMes.Item2 = 0;
                            //newMes.Item1 = "abril";
                        }
                    }
                    if (dt.AddMonths(-i).ToString("MMMM") == "mayo")
                    {
                        if (item.mayo != null)
                        {
                            //newMes.Item2 = item.mayo;
                            //newMes.Item1 = "mayo";
                        }
                        else
                        {
                            //newMes.Item2 = 0;
                            //newMes.Item1 = "mayo";
                        }
                    }
                }
                item.Ult6Meses = Ult6Meses;
            }
            var setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var jsonreqs = JsonConvert.SerializeObject(repsustancia, setting);
            return new JsonResult { Data = jsonreqs, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
        }
        public ActionResult ReporteConsumoFarmacia()
        {
            var res =  Inv.Database.SqlQuery<ReporteAbastecimeinto>("EXEC Sp_ReporteConsumoFarm").ToList();
            DateTime dt = DateTime.Now;
            foreach (var item in res)
            {
                var Ult6Meses = new List<Tuple<string, Nullable<int>>>();

                for (int i = 0; i < 3; i++)
                {              
                    if (dt.AddMonths(-i).ToString("MMMM") == "abril")
                    {
                        if (item.abril != null)
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("abril", item.abril.Value));
                        else
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("abril", 0));
                    }

                    if (dt.AddMonths(-i).ToString("MMMM") == "mayo")
                    {
                        if (item.mayo != null)
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("mayo", item.mayo.Value));
                        else
                            Ult6Meses.Add(new Tuple<string, Nullable<int>>("mayo", 1));
                    }
                }
                item.Ult6Meses = Ult6Meses;
            }
            return View(res);
        }
        public JsonResult GetReporteConsumoFarm()
        {
            //UanlSISM.Models.SERVMEDEntities1 Inv = new Models.SERVMEDEntities1();
            //UanlSISM.Models.SERVMEDInventario ReqOC = new Models.SERVMEDInventario();
            //UanlSISM.Models.ReporteAbastecimeinto reporteConsumo = new Models.ReporteAbastecimeinto();

            //var repsustancia = Inv.Database.SqlQuery<ReporteAbastecimeinto>("EXEC [dbo].[Sp_ReporteConsumoFarm] @Año =" + DateTime.Now.Year).ToList();
            //var fechamenos1mes = DateTime.Now.AddMonths(-1);


            //foreach (var item in repsustancia)
            //{
            //    var requisiciones =
            //    (from req in ReqOC.Requisicion
            //     join detreq in ReqOC.DetalleReq on req.id equals detreq.Id_Requisicion
            //     join OC in ReqOC.OrdenCompra on req.id equals OC.Id_Requisicion
            //     join DetalleOC in ReqOC.DetalleOC on OC.Id equals DetalleOC.Id_OrdenCompra
            //     join ValeEntrada in ReqOC.ValeEntrada on OC.Id equals ValeEntrada.id_ordencompra
            //     select new { req, detreq, OC, DetalleOC, ValeEntrada }).Where(x => x.detreq.Id_Sustancia == item.SustanciaId && x.req.Id_Tipo == 2 && x.req.Fecha > fechamenos1mes && x.DetalleOC.Id_Sustancia == item.SustanciaId).ToList();

            //    item.Requisiciones = requisiciones.Select(x => x.req).ToList();
            //    item.OrdenesCompras = requisiciones.Select(x => x.OC).ToList();
            //    item.ValeEntrada = requisiciones.Select(x => x.ValeEntrada).ToList();

            //    var Ult6Meses = new List<Tuple<string, Nullable<int>>>();

            //    //for (int i = 1; i < 7; i++)
            //    //{

            //    //}
            //    var newMes = new Meses
            //    {
            //        Item2 = 100,
            //        Item1 = "enero"
            //    };

            //    //Ult6Meses.Add(newMes);
            //    //Ult6Meses.Add(newMes);
            //    //Ult6Meses.Add(newMes);
            //    //Ult6Meses.Add(newMes);
            //    //Ult6Meses.Add(newMes);
            //    //Ult6Meses.Add(newMes);

            //    item.Ult6Meses = Ult6Meses;
            //}

            //var setting = new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //};

            //var jsonreqs = JsonConvert.SerializeObject(repsustancia, setting);

            //return new JsonResult { Data = jsonreqs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            return null;
        }
        public JsonResult GetRequisicionesOC(int SustanciaId)
        {
            UanlSISM.Models.SERVMEDInventario ReqOC = new Models.SERVMEDInventario();

            var fechamenos1mes = DateTime.Now.AddMonths(-3);
            var requisiciones =
                   (from req in ReqOC.Requisicion
                    join detreq in ReqOC.DetalleReq on req.id equals detreq.Id_Requisicion
                    join OC in ReqOC.OrdenCompra on req.id equals OC.Id_Requisicion
                    join Prov in ReqOC.Proveedor on OC.Id_Proveedor equals Prov.Id
                    join DetalleOC in ReqOC.DetalleOC on OC.Id equals DetalleOC.Id_OrdenCompra
                    join VEntrada in ReqOC.ValeEntrada on OC.Id equals VEntrada.id_ordencompra
                    join DetalleVE in ReqOC.Detalle_VE on VEntrada.id equals DetalleVE.Id_ve
                    join CodBarras in ReqOC.CodigoBarras on DetalleVE.Id_CodigoBarras equals CodBarras.Id
                    select new { req, detreq, OC, DetalleOC, Prov, VEntrada, DetalleVE, CodBarras }).Where(x => x.detreq.Id_Sustancia == SustanciaId && x.req.Id_Tipo == 2 && x.req.Fecha > fechamenos1mes && x.DetalleOC.Id_Sustancia == SustanciaId).ToList();

            var setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var jsonreqs = JsonConvert.SerializeObject(requisiciones, setting);

            return new JsonResult { Data = jsonreqs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetRequisicionesOC1(int SustanciaId)
        {
            UanlSISM.Models.SERVMEDInventario ReqOC = new Models.SERVMEDInventario();
            var fechamenos1mes = DateTime.Now.AddMonths(-3);    
            var query = (from a in ReqOC.Requisicion
                         join b in ReqOC.DetalleReq on a.id equals b.Id_Requisicion
                         where b.Id_Sustancia == SustanciaId && a.Id_Tipo == 1 && a.Fecha > fechamenos1mes
                         select new
                         {
                             a.id,
                             a.Fecha,
                             b.C_Solicitada,

                         }).ToList();

            var setting = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var jsonreqs = JsonConvert.SerializeObject(query, setting);

            return new JsonResult { Data = jsonreqs, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}