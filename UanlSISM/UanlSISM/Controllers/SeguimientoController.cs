using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class SeguimientoController : Controller
    {
        SERVMEDEntities8 db1 = new SERVMEDEntities8();               //Req, OC, Sustancia, Vale
        SERVMEDEntities5 db2 = new SERVMEDEntities5();               //Sustancia

        public ActionResult Trazabilidad()
        {
            return View();
        }

        public class Propiedades
        {
            public string Clave { get; set; }
            public string FolioRequisicion { get; set; }
            public string FechaRequisicion { get; set; }
            public int CanReq { get; set; }
            public string FolioOC { get; set; }
            public string FechaOC { get; set; }
            public int CanOC { get; set; }
            public double PrecioUnitario { get; set; }
            public double Subtotal { get; set; }
            public string Descripcion { get; set; }
        }

        public ActionResult ObtenerReporte(string FechaInicio, string FechaFin, string ClaveMed)
        {
            try
            {
                var fechaI = FechaInicio + " 00:00:00";
                var fechaIn = DateTime.Parse(fechaI);

                var fechaF = FechaFin + " 23:59:59";
                var fechaFi = DateTime.Parse(fechaF);

                var results1 = new List<Propiedades>();

                //Detalles Requis de la Clave enviada
                var Lista_Requis = (from Req in db1.Requisicion
                             join Dreq in db1.DetalleReq on Req.id equals Dreq.Id_Requisicion
                             join Sus in db1.Sustancia on Dreq.Id_Sustancia equals Sus.Id
                             where Req.Fecha >= fechaIn && Req.Fecha <= fechaFi
                             where Sus.Clave == ClaveMed && Req.Id_Tipo == 2
                             select new
                             {
                                 IdRequi = Req.id,
                                 Clave = Sus.Clave,
                                 FolioR = Req.clave,
                                 FechaR = Req.Fecha,
                                 CanR = Dreq.C_Solicitada,
                                 Desc = Sus.descripcion_21,
                                 IdSus = Sus.Id
                             }).ToList();

                foreach (var q in Lista_Requis)
                {
                    var PartidaOC = (from OC in db1.OrdenCompra 
                                    join Doc in db1.DetalleOC on OC.Id equals Doc.Id_OrdenCompra
                                    where OC.Id_Requisicion == q.IdRequi
                                    where Doc.Id_Sustancia == q.IdSus
                                    select new
                                    {
                                        FolioOC = OC.clave,
                                        FechaOC = OC.Fecha,
                                        CanOC = Doc.Cantidad,
                                        PU = Doc.PreUnit,
                                    }).FirstOrDefault();

                    if (PartidaOC != null)
                    {
                        var Partidas = new Propiedades
                        {
                            FolioOC = PartidaOC.FolioOC,
                            FechaOC = string.Format("{0:d/M/yyyy hh:mm tt}", PartidaOC.FechaOC),
                            CanOC = PartidaOC.CanOC,
                            PrecioUnitario = PartidaOC.PU,
                            //
                            //Clave = PartidaOC.Clave,
                            //FolioRequisicion = PartidaOC.FolioR,
                            //FechaRequisicion = string.Format("{0:d/M/yyyy hh:mm tt}", PartidaOC.FechaR),
                            //CanReq = PartidaOC.CanR,
                            //Descripcion = PartidaOC.Desc
                        };
                        results1.Add(Partidas);
                    }
                }

                foreach (var item in Lista_Requis)
                {
                    var Partidas = new Propiedades
                    {
                        Clave = item.Clave,
                        FolioRequisicion = item.FolioR,
                        FechaRequisicion = string.Format("{0:d/M/yyyy hh:mm tt}", item.FechaR),
                        CanReq = item.CanR,
                        Descripcion = item.Desc
                    };
                    results1.Add(Partidas);
                }

                return Json(new { MENSAJE = "FOUND", REP = results1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
