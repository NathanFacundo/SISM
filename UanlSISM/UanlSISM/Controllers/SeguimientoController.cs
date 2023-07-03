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
        SISM_SIST_MEDEntities ConBD = new SISM_SIST_MEDEntities();      //BD NUEVA PRODUCTIVA

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
            public string ProveedorOC { get; internal set; }
        }

        public ActionResult ObtenerReporte(string FechaInicio, string FechaFin, string ClaveMed)
        {
            try
            {
                #region Fechas
                var fechaI = FechaInicio + " 00:00:00";
                var fechaIn = DateTime.Parse(fechaI);

                var fechaF = FechaFin + " 23:59:59";
                var fechaFi = DateTime.Parse(fechaF);

                var results1 = new List<Propiedades>();
                #endregion

                //Buscar las partidas en las Requis con la clave ingresada      BD NUEVA
                var PartidasRequis = (from R in ConBD.SISM_REQUISICION
                                      join DR in ConBD.SISM_DET_REQUISICION on R.Id_Requicision equals DR.Id_Requicision
                                      where R.Fecha >= fechaIn && R.Fecha <= fechaFi
                                      where DR.Clave == ClaveMed
                                      select new {
                                          IdRequi = R.Id_Requicision,
                                          Clave = DR.Clave,
                                          FolioR = R.claveOLD,
                                          FechaR = R.Fecha,
                                          CanR = DR.Cantidad,
                                          Desc = DR.Descripcion,
                                          IdSus = DR.Id_Sustancia
                                      }).ToList();

                //
                foreach (var a in PartidasRequis)
                {
                    //Recorremos la lista de partidas encontradas con esos filtros para vi si ya se encuentran o no en OC
                    var PartidaEnOC = (from O in ConBD.SISM_ORDEN_COMPRA
                                       join DO in ConBD.SISM_DETALLE_OC on O.Id equals DO.Id_OrdenCompra
                                       join R in ConBD.SISM_REQUISICION on O.Id_Requisicion equals R.Id_Requicision
                                       join DR in ConBD.SISM_DET_REQUISICION on R.Id_Requicision equals DR.Id_Requicision
                                       where O.Id_Requisicion == a.IdRequi && DO.Id_Sustencia == a.IdSus
                                       where DR.Id_Sustancia == a.IdSus
                                        select new
                                        {
                                            FolioOC = O.Clave,
                                            FechaOC = O.Fecha_HacerOC,
                                            CanOC = DO.Cantidad,
                                            PU = DO.PreUnit,
                                            Prov = O.NombreProveedor,
                                            Clave = DO.ClaveMedicamento,
                                            FolioR = R.claveOLD,
                                            FechaR = R.Fecha,
                                            CanR = DR.Cantidad,
                                            Desc = DO.Descripcion
                                        }).FirstOrDefault();

                    if (PartidaEnOC != null)//Si ya se encuentra en alguna OC la agregamos a la lista con la info de la OC
                    {
                        var Partidas = new Propiedades
                        {
                            FolioOC = PartidaEnOC.FolioOC,
                            FechaOC = string.Format("{0:d/M/yyyy hh:mm tt}", PartidaEnOC.FechaOC),
                            CanOC = (int)PartidaEnOC.CanOC,
                            PrecioUnitario = (double)PartidaEnOC.PU,
                            ProveedorOC = PartidaEnOC.Prov,
                            Clave = PartidaEnOC.Clave,
                            FolioRequisicion = PartidaEnOC.FolioR,
                            FechaRequisicion = string.Format("{0:d/M/yyyy hh:mm tt}", PartidaEnOC.FechaR),
                            CanReq = (int)PartidaEnOC.CanR,
                            Descripcion = PartidaEnOC.Desc
                        };
                        results1.Add(Partidas);
                    }
                    else//Si no tiene OC aún así agregamos la partida a la lista con la info solo de la Requi
                    {
                        var Partidas = new Propiedades
                        {
                            Clave = a.Clave,
                            FolioRequisicion = a.FolioR,
                            FechaRequisicion = string.Format("{0:d/M/yyyy hh:mm tt}", a.FechaR),
                            CanReq = (int)a.CanR,
                            Descripcion = a.Desc
                        };
                        results1.Add(Partidas);
                    }
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
