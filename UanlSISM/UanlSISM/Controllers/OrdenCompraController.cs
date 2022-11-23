﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class OrdenCompraController : Controller
    {
        SISM_SIST_MEDEntities ConBD = new SISM_SIST_MEDEntities();
        SERVMEDEntities4 DAM = new SERVMEDEntities4();
        SERVMEDEntities8 db = new SERVMEDEntities8();
        SERVMEDEntities5 SISMFarmacia = new SERVMEDEntities5();
        //SERVMEDEntities5 db2 = new SERVMEDEntities5();

        [Authorize]
        public ActionResult OrdenCompra()
        {
            ViewBag.PROVEEDORES = new SelectList(db.Proveedor.ToList(), "Id", "Prov_Nombre" );
            //ViewBag.PROVEEDORES = new SelectList(db.usuario.Include(ñ => ñ.rol).Where(u => u.rol.Nombre == "Cliente").ToList(), "Id", "Nombre");

            return View();
        }

        public class ListCampos
        {
            public int Id_BorradorRequi { get; set; }
            public string Fecha { get; set; }
            public string Clave { get; set; }
            public string UsuarioRegistra { get; set; }
            public int Id_Requisicion { get; set; }
            public string FechaRequisicion { get; set; }
            public string Id_User { get; set; }
            public string EstatusContrato { get; set; }
            //--------------------------------------------
            
            public string Descripcion { get; set; }
            public int Cantidad { get; set; }
            public string Folio { get; set; }
            
            public string Fecha1 { get; set; }
            public string Usuario { get; set; }
            public int? Existencia { get; set; }
            
            public string Compendio { get; set; }

            public double PrecioUnit { get; set; }

            public double Total { get; set; }
        }

        public ActionResult ObtenerRequisInicio()
        {
            try
            {
                var query = (from a in ConBD.SISM_REQUISICION
                             where a.EstatusOC == "0" || a.EstatusOC == null
                             select a).ToList();

                var results1 = new List<ListCampos>();

                foreach (var q in query)
                {
                    var resultado = new ListCampos
                    {
                        Clave = q.claveOLD,
                        Fecha = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha),
                        EstatusContrato = q.EstatusContrato
                    };
                    results1.Add(resultado);
                }
                return Json(new { MENSAJE = "FOUND", REQUIS = results1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public class InvAlmFarm
        {
            public int Id { get; set; }
            public Int16 InvAlmId { get; set; }
            public DateTime Fecha { get; set; }
            public int Id_Sustancia { get; set; }
            public int Inv_Ini { get; set; }
            public int Inv_Ent { get; set; }
            public int Inv_Sal { get; set; }
            public int Inv_Disp { get; set; }
            public int Inv_Reorden { get; set; }
            public int ManejoDisponible { get; set; }
            public string Usuario_Registra { get; set; }
        }

        public JsonResult ObtenerDetalleRequi(string Id_Requi)
        {
            try
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                var fechaDT = DateTime.Parse(fecha);

                //Obtener el último folio de la OC para sumar uno (será el nuevo folio de la nueva OC)
                //var FolioOC = (from a in db.OrdenCompra
                //               select a).OrderByDescending(u => u.clave).FirstOrDefault();

                //int FolioOC_Nuevo = Convert.ToInt32(FolioOC.clave) + 1;
                //ViewBag.Folio = FolioOC_Nuevo;
                //----------------

                var query = (from a in ConBD.SISM_REQUISICION
                             join det in ConBD.SISM_DET_REQUISICION on a.Id_Requicision equals det.Id_Requicision
                             where a.claveOLD == Id_Requi
                             select new
                             {
                                 det.Clave,
                                 det.Descripcion,
                                 det.Cantidad,
                                 Folio = a.claveOLD,
                                 a.Fecha,
                                 a.Id_User,
                                 EstatusContrato = a.EstatusContrato,
                                 det.Compendio,
                                 det.PrecioUnitario,
                                 det.Total
                             }).ToList();

                var results1 = new List<ListCampos>();

                foreach (var q in query)
                {

                    var sus = (from a in DAM.Sustancia
                               where a.Clave == q.Clave
                               select a).FirstOrDefault();

                    if (sus != null)
                    {
                        string query2 = "select ManejoDisponible as ManejoDisponible from InvAlmFarm WHERE Id_Sustancia = " + sus.Id + " and InvAlmId = 76";
                        var result2 = db.Database.SqlQuery<InvAlmFarm>(query2);
                        var res2 = result2.FirstOrDefault();

                        var resultado = new ListCampos
                        {
                            Clave = q.Clave,
                            Existencia = res2.ManejoDisponible,
                            Descripcion = q.Descripcion,
                            Cantidad = (int)q.Cantidad,
                            Folio = q.Folio,
                            Fecha = string.Format("{0:d/M/yy hh:mm tt}", q.Fecha),
                            Usuario = q.Id_User,
                            Fecha1 = string.Format("{0:d/M/yy hh:mm tt}", fechaDT),
                            EstatusContrato = q.EstatusContrato,
                            Compendio = q.Compendio,
                            PrecioUnit = (double)q.PrecioUnitario,
                            Total = (double)q.Total
                        };
                        results1.Add(resultado);
                    }

                }

                return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult OrdenesCompra()
        {
            return View();
        }

        public JsonResult GenerarOC(List<SISM_DET_REQUISICION> ListaOC, int FolioRequi, string ProveedorReq)
        {
            var UsuarioRegistra = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;
            var IdUsuarioCifrado = User.Identity.GetUserId();

            try
            {
                //PROVEEDOR de la Requi(Orden)
                var Prov = (from a in db.Proveedor
                            where a.Prov_Nombre == ProveedorReq
                            select a
                            ).FirstOrDefault();

                //REQUI que se hará Orden
                var Requi = (from a in ConBD.SISM_REQUISICION
                            where a.claveOLD == FolioRequi.ToString()
                             select a
                            ).FirstOrDefault();

                //DETALLE DE LA REQUI -lista-
                var DetalleRequi = (from a in ConBD.SISM_DET_REQUISICION
                             where a.Id_Requicision == Requi.Id_Requicision
                                    select a
                            ).ToList();

                //Obtenemos el Usuario que se logueó para hacer el join (buscarlo) en la tabla Usuario y así obtener el Id de esa tabla
                var UsuarioOld = User.Identity.GetUserName();
                var UsuarioOLD = (from a in db.Usuario
                                  where a.Usu_User == UsuarioOld
                                  select a).FirstOrDefault();

                //CREAR ORDEN NUEVA a partir de una Requi
                SISM_ORDEN_COMPRA OC = new SISM_ORDEN_COMPRA();
                OC.Clave = Requi.Clave;
                OC.Id_Requisicion = Requi.Id_Requicision;
                OC.Id_Proveedor = Prov.Id;
                OC.Fecha = fechaDT;
                OC.FechaMod = fechaDT;
                OC.Forma_Pago = "";
                OC.Folio = "";
                OC.Status = false;
                OC.UsuarioId = UsuarioOLD.UsuarioId;
                OC.Cerrado = false;
                OC.Cuadro = 1;
                OC.UsuarioNuevo = UsuarioRegistra;
                OC.IP_User = ip_realiza;

                ConBD.SISM_ORDEN_COMPRA.Add(OC);
                ConBD.SaveChanges();

                //Obtenemos la ultima ORDEN guardada(que es esta) para guardar su detalle
                var IdOC = (from a in ConBD.SISM_ORDEN_COMPRA
                               where a.UsuarioNuevo == UsuarioRegistra
                               where a.Fecha == fechaDT
                               select a).OrderByDescending(u => u.Id).FirstOrDefault();

                //RECORREMOS DETALLE DE LA REQUI para guardar en la tabla DETALLE ORDEN
                foreach (var item in DetalleRequi)
                {
                    //CREAR EL DETALLE DE LA NUEVA ORDEN
                    SISM_DETALLE_OC DetalleOC = new SISM_DETALLE_OC();

                    //BUSCAMOS EN LA TABLA "CodigoBarras" el Id del Codigo de Barras, buscando por el Id de la Sustancia
                    var CodigoBarras = (from a in SISMFarmacia.CodigoBarras
                                        where a.Id_Sustancia == item.Id_Sustancia
                                      select a).FirstOrDefault();

                    DetalleOC.Id_OrdenCompra = IdOC.Id;
                    DetalleOC.Id_CodigoBarrar = CodigoBarras.Id;
                    DetalleOC.Cantidad = item.Cantidad;
                    DetalleOC.PreUnit = item.PrecioUnitario;
                    DetalleOC.Obsequio = 0;
                    DetalleOC.Status = false;
                    DetalleOC.Id_Sustencia = item.Id_Sustancia;
                    //NOTA: Columna de Vic (pendiente) es el mismo dato que CANTIDAD
                    DetalleOC.Pendiente = item.Cantidad;


                    ConBD.SISM_DETALLE_OC.Add(DetalleOC);
                    ConBD.SaveChanges();
                }

                return Json(new { MENSAJE = "Succe: Se generó la O.C" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
