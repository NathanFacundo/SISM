
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
        SISM_SIST_MEDEntities ConBD = new SISM_SIST_MEDEntities();   //BD NUEVA PRODUCTIVA
        SERVMEDEntities6 db3 = new SERVMEDEntities6();               //SP Temporales

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
            public string ProveedorOC { get; set; }
            public int IdSus { get; set; }
            public int IdVE { get; set; }
            public string FolioVE { get; set; }
            public string FechaVE { get; set; }
            public string FacturaVE { get; set; }
            public int CantSurVE { get; set; }
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

                //Hacer match de la Requi con OC
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
                                            Desc = DO.Descripcion,
                                            IdSus = DR.Id_Sustancia
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
                            Descripcion = PartidaEnOC.Desc,
                            IdSus = PartidaEnOC.IdSus
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
                            Descripcion = a.Desc,
                            IdSus = a.IdSus
                        };
                        results1.Add(Partidas);
                    }
                }

                ////Hacer match: Requi > OC > VE
                //foreach (var a in results1)
                //{
                //    //Obtener el Id de la Requi en la vieja BD      SERVMED
                //    var Req = (from R in db1.Requisicion
                //               where R.clave == a.FolioRequisicion
                //               select R).FirstOrDefault();

                //    //Recorrer la lista que hemos obtenido para buscar C/U en los VE y ver si ya tiene un VE
                //    var VE = (from V in db1.ValeEntrada
                //              join DV in db1.Detalle_VE on V.id equals DV.Id_ve

                //              join R in db1.Requisicion on V.id_Requisicion equals R.id
                //              join DR in db1.DetalleReq on R.id equals DR.Id_Requisicion
                //              join O in db1.OrdenCompra on V.id_ordencompra equals O.Id
                //              join DO in db1.DetalleOC on O.Id equals DO.Id_OrdenCompra

                //              where V.id_Requisicion == Req.id && DV.Id_Sustancia == a.IdSus
                //              select new { 
                //                  IdVE = V.id,
                //                  FolioVE = V.clave,
                //                  FechaVE = V.Fecha,
                //                  FacturaVE = V.Factura,
                //                  CantSurVE = DV.Cant_Surtida
                //                  ,
                //                  FolioOC = O.clave,
                //                  FechaOC = O.Fecha,
                //                  CanOC = DO.Cantidad,
                //                  PU = DO.PreUnit,
                //                  Prov = O.Id_Proveedor,//
                //                  Clave = DO.Id_Sustancia,//
                //                  FolioR = R.clave,
                //                  FechaR = R.Fecha,
                //                  CanR = DR.C_Solicitada,
                //                  Desc = DO.Id_Sustancia,//
                //                  IdSus = DR.Id_Sustancia
                //              }).FirstOrDefault();

                //    if(VE != null)//Si el medicamento ya se encuentra en algun VE lo agregamos a la lista con la info del VE
                //    {
                //        var Partidas = new Propiedades
                //        {
                //            IdVE = VE.IdVE,
                //            FolioVE = VE.FolioVE,
                //            FechaVE = VE.FechaVE,
                //            FacturaVE = VE.FacturaVE,
                //            CantSurVE = VE.CantSurVE
                //            ,
                //            FolioOC = VE.FolioOC,
                //            FechaOC = string.Format("{0:d/M/yyyy hh:mm tt}", VE.FechaOC),
                //            CanOC = (int)VE.CanOC,
                //            PrecioUnitario = (double)VE.PU,
                //            ProveedorOC = VE.Prov.ToString(),//
                //            Clave = VE.Clave.ToString(),//
                //            FolioRequisicion = VE.FolioR,
                //            FechaRequisicion = string.Format("{0:d/M/yyyy hh:mm tt}", VE.FechaR),
                //            CanReq = (int)VE.CanR,
                //            Descripcion = VE.Desc.ToString(),//
                //            IdSus = VE.IdSus
                //        };
                //        results1.Add(Partidas);
                //    }
                //    else//Si el medicamento no se encuentra en algún VE aún así volvemos a agregar la info de la Requi & OC
                //    {
                //        var Partidas = new Propiedades
                //        {
                //            //Requi
                //            Clave = a.Clave.ToString(),//
                //            FolioRequisicion = a.FolioRequisicion,
                //            FechaRequisicion = string.Format("{0:d/M/yyyy hh:mm tt}", a.FechaRequisicion),
                //            CanReq = (int)a.CanReq,
                //            Descripcion = a.Descripcion.ToString(),//
                //            IdSus = a.IdSus
                //        };
                //        results1.Add(Partidas);

                //        if (a.FolioOC != null)
                //        {
                //            var Partidas1 = new Propiedades
                //            {
                //                //OC
                //                FolioOC = a.FolioOC,
                //                FechaOC = string.Format("{0:d/M/yyyy hh:mm tt}", a.FechaOC),
                //                CanOC = (int)a.CanOC,
                //                PrecioUnitario = (double)a.PrecioUnitario,
                //                ProveedorOC = a.ProveedorOC.ToString(),//
                //            };
                //            results1.Add(Partidas1);
                            
                //        }
                //    }
                //}

                return Json(new { MENSAJE = "FOUND", REP = results1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        public ActionResult ObtenerReporte1(string FechaInicio, string FechaFin, string ClaveMed)
        {
            try
            {
                #region Fechas
                var fechaI = FechaInicio + " 00:00:00";
                var fechaIn = DateTime.Parse(fechaI);

                var fechaF = FechaFin + " 23:59:59";
                var fechaFi = DateTime.Parse(fechaF);
                #endregion

                var query = db3.SP_Trazabilidad(FechaInicio, FechaFin, ClaveMed).ToList();

                //var results1 = new List<Propiedades>();

                //foreach (var q in query)
                //{
                //    var resultado = new Propiedades
                //    {
                //        Clave = q.Clave_Med_,
                //        FolioRequisicion = q.Folio_Req_,
                //        FechaRequisicion = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha_Req_),
                //        CanReq = (int)q.Cant__Req_,
                //        FolioOC = q.Folio_OC_,
                //        FechaOC = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha_OC_),
                //        CanOC = (int)q.Cant__OC_,
                //        PrecioUnitario = (double)q.P_U__OC,
                //        FolioVE = q.Folio_VE_,
                //        FechaVE = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha_VE_),
                //        CantSurVE = (int)q.Cant__VE_,
                //        FacturaVE = q.Factura
                //    };
                //    results1.Add(resultado);
                //}

                return Json(new { MENSAJE = "FOUND", REP = query }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }





    }
}
