using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class OrdenCompraController : Controller
    {
        SISM_SIST_MEDEntities ConBD = new SISM_SIST_MEDEntities();
        BD_Almacen ConBD2 = new BD_Almacen();

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
            public string Descripcion { get; set; }
            public int Cantidad { get; set; }
            public string Folio { get; set; }
            public string FolioRequisicion { get; set; }
            public string Fecha1 { get; set; }
            public string Usuario { get; set; }
            public int? Existencia { get; set; }
            public string Compendio { get; set; }
            public double PrecioUnitario { get; set; }
            public double Total { get; set; }
            public int Id_Sustancia { get; set; }
            public string NombreProveedor { get; set; }
            public double Total_OC { get; set; }
            public int Id_OC { get; set; }
            public string Validar { get; set; }
            public string DescripcionOC { get; set; }
            public string Estatus_OC { get; set; }
            
            public string DescO { get; internal set; }
            public int? CBarra { get; internal set; }
            public int? Can { get; internal set; }
            public int Id_Requicision { get; internal set; }
        }

        public ActionResult ObtenerRequisInicio()
        {
            try
            {
                var query = (from a in ConBD2.SISM_REQUISICION
                             where a.EstatusOC == "0" || a.EstatusOC == null
                             select a).ToList();

                var results1 = new List<ListCampos>();

                foreach (var q in query)
                {
                    var resultado = new ListCampos
                    {
                        Clave = q.claveOLD,
                        Fecha = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha),
                        EstatusContrato = q.EstatusContrato,
                        Id_User = q.Id_User,
                        FechaRequisicion = string.Format("{0:yyyy/M/d hh:mm tt}", q.Fecha, new CultureInfo("es-ES"))
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

                var query = (from a in ConBD2.SISM_REQUISICION
                             join det in ConBD2.SISM_DET_REQUISICION on a.Id_Requicision equals det.Id_Requicision
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
                                 det.Total,
                                 det.Id_Sustancia
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
                            PrecioUnitario = (double)q.PrecioUnitario,
                            Total = (double)q.Total,
                            Id_Sustancia = q.Id_Sustancia
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

        decimal? SubTotal_OC = 0.00m;
        public JsonResult GenerarOC(List<SISM_DET_REQUISICION> ListaOC, int FolioRequi, string ProveedorReq)
        {
            var UsuarioRegistra = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;
            var IdUsuarioCifrado = User.Identity.GetUserId();
            //return null;
            try
            {
                //PROVEEDOR de la Requi(Orden)
                var Prov = (from a in db.Proveedor
                            where a.Prov_Nombre == ProveedorReq
                            select a
                            ).FirstOrDefault();
                //REQUI que se hará Orden
                var Requi = (from a in ConBD2.SISM_REQUISICION
                             where a.claveOLD == FolioRequi.ToString()
                             select a
                            ).FirstOrDefault();

                var UsuarioOld = User.Identity.GetUserName();
                var UsuarioOLD = (from a in db.Usuario
                                  where a.Usu_User == UsuarioOld
                                  select a).FirstOrDefault();

                #region TODA LA ACCION
                //CREAR ORDEN NUEVA a partir de una Requi
                SISM_ORDEN_COMPRA OC = new SISM_ORDEN_COMPRA();
                //OC.Clave = CLAVE.ToString();
                OC.Id_Requisicion = Requi.Id_Requicision;
                OC.Id_Proveedor = Prov.Id;
                OC.Fecha = fechaDT;
                OC.FechaMod = fechaDT;
                OC.Forma_Pago = "";
                OC.Folio = "";
                OC.Status = false;
                OC.UsuarioId = UsuarioOLD.UsuarioId;
                OC.Cerrado = true;
                OC.Cuadro = 1;
                OC.UsuarioNuevo = UsuarioRegistra;
                OC.IP_User = ip_realiza;
                OC.NombreProveedor = Prov.Prov_Nombre;
                OC.OC_PorValidar = "1"; //LA OC nace en 1, tiene que validarse para pasar a 2 (validada) despues tiene que Generarse la OC y pasa a 3, el Status cambiará a True
                ConBD2.SISM_ORDEN_COMPRA.Add(OC);
                ConBD2.SaveChanges();
                //ACTUALIZAMOS LA REQUI EN SU COLUMNA 'EstatusOC' poniendo 1 ya que esa requi se hará O.C
                ConBD2.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET EstatusOC = '1' WHERE Id_Requicision='" + Requi.Id_Requicision + "';");
                //obtener la ultima 'clave' de la tabla OrdenCompra actualBD vieja) para que inserte un nuevo registro en la nueva BD CONSECUTIVO de la clave
                var Clave = (from a in db.OrdenCompra
                             select new
                             {
                                 clave = a.clave
                             }).OrderByDescending(u => u.clave).FirstOrDefault();
                var AñoMes_Actual = string.Format("{0:yyMM}", fechaDT);
                var UltimoConsecutivo_Clave = Convert.ToInt32(Clave.clave.Substring(4));
                var ConsecutivoNuevo = ((UltimoConsecutivo_Clave) + 1);
                var ConsecutivoNuevoTxt = "";
                if (ConsecutivoNuevo < 100)
                {
                    if (ConsecutivoNuevo < 9)
                    {
                        ConsecutivoNuevoTxt = "00" + ConsecutivoNuevo;
                    }
                    else
                    {
                        ConsecutivoNuevoTxt = "0" + ConsecutivoNuevo;
                    }
                }
                else
                {
                    ConsecutivoNuevoTxt = "" + ConsecutivoNuevo;
                }
                //Obtenemos la ultima O.C guardada(que es esta) para guardar su detalle
                var IdOC = (from a in ConBD2.SISM_ORDEN_COMPRA
                            where a.UsuarioNuevo == UsuarioRegistra
                            where a.Fecha == fechaDT
                            select a).OrderByDescending(u => u.Id).FirstOrDefault();
                //ACTUALIZAMOS LA CLAVE DE LA O'C 
                ConBD2.Database.ExecuteSqlCommand("UPDATE SISM_ORDEN_COMPRA SET Clave = '" + AñoMes_Actual + ConsecutivoNuevoTxt + "' WHERE Id='" + IdOC.Id + "';");
                //RECORREMOS  para guardar en la tabla DETALLE ORDEN
                foreach (var item in ListaOC)
                {
                    //CREAR EL DETALLE DE LA NUEVA ORDEN
                    SISM_DETALLE_OC DetalleOC = new SISM_DETALLE_OC();
                    //BUSCAMOS EN LA TABLA "CodigoBarras" el Id del Codigo de Barras, buscando por el Id de la Sustancia
                    var CodigoBarras = (from a in SISMFarmacia.CodigoBarras
                                        where a.Id_Sustancia == item.Id_Sustancia
                                        select a).FirstOrDefault();
                    //Obtenemos la info de SUSTANCIA de la tabla Detalle_Requi
                    var Sustancia = (from a in ConBD2.SISM_DET_REQUISICION
                                     where a.Clave == item.Clave
                                     select a).FirstOrDefault();
                    DetalleOC.Id_OrdenCompra = IdOC.Id;
                    DetalleOC.Id_CodigoBarrar = CodigoBarras.Id;
                    DetalleOC.Obsequio = 0;
                    DetalleOC.Status = false;
                    DetalleOC.Id_Sustencia = item.Id_Sustancia;
                    DetalleOC.Descripcion = Sustancia.Descripcion;
                    DetalleOC.ClaveMedicamento = Sustancia.Clave;
                    //1 quiere decir que el item está pendiente, 0 quiere decir que si se surtió el item
                    DetalleOC.ItemPendiente = item.CB_ELIMINAR;
                    //En DetalleOC.Cantidad se guarda la Cantidad de pzas que pide Almacen siempre
                    DetalleOC.Cantidad = item.Cantidad;

                    if (item.CANTIDAD_NUEVA > 0)
                    {
                        //DetalleOC.CantidadOC = item.CANTIDAD_NUEVA;
                        //DetalleOC.CantidadItema_OC = item.CANTIDAD_NUEVA;
                        if (item.CANTIDAD_NUEVA < item.Cantidad)
                        {
                            DetalleOC.ItemPendiente = true;
                            DetalleOC.CantidadPendiente = item.Cantidad - item.CANTIDAD_NUEVA;
                            DetalleOC.CantidadOC = item.CANTIDAD_NUEVA;
                        }
                    }
                    else
                    {
                        DetalleOC.CantidadOC = item.Cantidad;
                    }

                    //Se valida si el PRECIO UNITARIO se modificó
                    if (item.PREUNIT_NUEVA > 0)
                    {
                        DetalleOC.PreUnit = item.PREUNIT_NUEVA;
                    }
                    else
                    {
                        DetalleOC.PreUnit = item.PrecioUnitario;
                    }
                    //Se valida si se ingresó una NUEVA CANTIDAD o un NUEVO PRECIO UNITARIO     $$TOTAL$$ 
                    if (item.CANTIDAD_NUEVA > 0 || item.PREUNIT_NUEVA > 0)
                    {
                        if (item.CANTIDAD_NUEVA > 0 && item.PREUNIT_NUEVA > 0)
                        {
                            DetalleOC.Total = (double?)decimal.Round((decimal)(item.CANTIDAD_NUEVA * item.PREUNIT_NUEVA), 2);
                        }
                        else
                        {
                            if (item.CANTIDAD_NUEVA > 0)
                            {
                                DetalleOC.Total = (double?)decimal.Round((decimal)(item.CANTIDAD_NUEVA * item.PrecioUnitario), 2);
                            }
                            if (item.PREUNIT_NUEVA > 0)
                            {
                                DetalleOC.Total = (double?)decimal.Round((decimal)(item.Cantidad * item.PREUNIT_NUEVA), 2);
                            }
                        }
                    }
                    else
                    {
                        DetalleOC.Total = (double?)decimal.Round((decimal)(DetalleOC.Cantidad * DetalleOC.PreUnit), 2);
                    }

                    ConBD2.SISM_DETALLE_OC.Add(DetalleOC);
                    ConBD2.SaveChanges();

                    if (item.CB_ELIMINAR == false)
                    {
                        SubTotal_OC += Decimal.Round((decimal)(DetalleOC.Total), 2);
                        OC.Total_OC = (double?)SubTotal_OC;
                    }



                    ConBD2.SaveChanges();
                }

                //Buscar en los items(medicamentos) del Detalle de la OC si alguno está pendiente (lo eliminaron a la hora de genera las OC)
                var ItemPendiente_DetalleOC = (from a in ConBD2.SISM_DETALLE_OC
                                               where a.ItemPendiente == true
                                               select a
                                    ).ToList();
                //Poner la OC como 'Parcial' ya que tiene items pendientes
                if (ItemPendiente_DetalleOC.Count > 0)
                {
                    OC.Estatus_OC = "OC Parcial";
                }
                else
                {
                    OC.Estatus_OC = "OC Completa";
                }
                ConBD2.SaveChanges();


                //, INFO = FolioNuevo_Oc.Clave
                return Json(new { MENSAJE = "Succe: Se generó la O.C" }, JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerUltima_OC()
        {
            try
            {
                var UsuarioRegistra = User.Identity.GetUserName();
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
                var fechaDT = DateTime.Parse(fecha);

                var IdOC = (from a in ConBD2.SISM_ORDEN_COMPRA
                            where a.UsuarioNuevo == UsuarioRegistra
                            where a.Fecha >= fechaDT
                            select a).OrderByDescending(u => u.Id).FirstOrDefault();

                db.SaveChanges();
                
                return Json(new { MENSAJE = "Succe: ", CLAVE = IdOC.Clave }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Respuesta;
        }

        //LISTADO DE LAS ORDENES DE COMPRA GENERADAS
        public ActionResult ObtenerOCInicio()
        {
            try
            {
                var query = (from a in ConBD2.SISM_ORDEN_COMPRA
                             join req in ConBD2.SISM_REQUISICION on a.Id_Requisicion equals req.Id_Requicision
                             select new { 
                             a.Id,
                             a.Clave,
                             a.Fecha,
                             a.UsuarioNuevo,
                             IdReq = req.claveOLD,
                             FReq = req.Fecha,
                             Cont = req.EstatusContrato,
                             a.OC_PorValidar,
                             a.Estatus_OC
                             }).ToList();

                var results1 = new List<ListCampos>();

                foreach (var q in query)
                {
                    var resultado = new ListCampos
                    {
                        Id_OC = q.Id,
                        Clave = q.Clave,
                        Fecha = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha),
                        Id_User = q.UsuarioNuevo,
                        Id_Requisicion = Convert.ToInt32(q.IdReq),
                        Fecha1 = string.Format("{0:d/M/yyyy hh:mm tt}", q.FReq),
                        EstatusContrato = q.Cont,
                        Validar = q.OC_PorValidar,
                        Estatus_OC = q.Estatus_OC
                    };
                    results1.Add(resultado);
                }
                return Json(new { MENSAJE = "FOUND", OCS = results1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ObtenerDetalleOC_Generada(int FolioOC, int FolioRequi)
        {
            try
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                var fechaDT = DateTime.Parse(fecha);

                var IdRequi = (from a in ConBD2.SISM_REQUISICION
                               where a.claveOLD == FolioRequi.ToString()
                               select new { 
                               a.Id_Requicision
                               }).FirstOrDefault();
                var IdOC = (from a in ConBD2.SISM_ORDEN_COMPRA
                               where a.Clave == FolioOC.ToString()
                               select a
                               ).FirstOrDefault();
                //PROVEEDOR de la Requi(Orden)
                var Prov = (from a in db.Proveedor
                            where a.Id == IdOC.Id_Proveedor
                            select a
                            ).FirstOrDefault();

                var query = (from a in ConBD2.SISM_ORDEN_COMPRA
                             join DetOC in ConBD2.SISM_DETALLE_OC on a.Id equals DetOC.Id_OrdenCompra
                             join Requi in ConBD2.SISM_REQUISICION on a.Id_Requisicion equals Requi.Id_Requicision
                             where a.Clave == FolioOC.ToString()
                             where DetOC.ItemPendiente == false
                             select new
                             {
                                 Folio = a.Clave,
                                 Fecha = a.Fecha,
                                 Usuario = a.UsuarioNuevo,
                                 Cantidad = DetOC.CantidadOC,
                                 Precio = DetOC.PreUnit,
                                 Descripcion = DetOC.Descripcion,
                                 ClaveMed = DetOC.ClaveMedicamento,
                                 PU = DetOC.PreUnit,
                                 Total = DetOC.Total,
                                 NombreProveedor = a.NombreProveedor,
                                 a.Total_OC,
                                 Desc = a.Descripcion,
                                 Pendiente = DetOC.ItemPendiente,
                                 FolioR = Requi.claveOLD
                             }).ToList();

                //ViewBag.NombreProvedor = Prov.Prov_Nombre;

                var results1 = new List<ListCampos>();
                //LISTA DE LA O.C Y SU DETALLE
                foreach (var q in query)
                {
                    var resultado = new ListCampos
                    {
                        Folio = q.Folio,
                        Fecha = string.Format("{0:d/M/yy hh:mm tt}", q.Fecha),
                        Fecha1 = string.Format("{0:d/M/yy hh:mm tt}", fechaDT),
                        Usuario = q.Usuario,
                        Descripcion = q.Descripcion,
                        Clave = q.ClaveMed,
                        Cantidad = (int)q.Cantidad,
                        PrecioUnitario = (double)q.PU,
                        Total = (double)q.Total,
                        NombreProveedor = q.NombreProveedor,
                        Total_OC = (double)q.Total_OC,
                        DescripcionOC = q.Desc,
                        FolioRequisicion = q.FolioR
                    };
                    results1.Add(resultado);
                }
                
                return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EliminarOC(int Id_OC)
        {
            try
            {
                var IdRequi = (from a in ConBD2.SISM_ORDEN_COMPRA
                               where a.Id == Id_OC
                               select a).FirstOrDefault();

                IdRequi.Status = false;
                ConBD2.SaveChanges();

                return Json(new { MENSAJE = "Succe: Se eliminó la O.C" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult OrdenCompraPorValidar()
        {
            return View();
        }

        public ActionResult ObtenerOC_PorValidar()
        {
            try
            {
                var query = (from a in ConBD2.SISM_ORDEN_COMPRA
                             join req in ConBD2.SISM_REQUISICION on a.Id_Requisicion equals req.Id_Requicision
                             //where a.OC_PorValidar == "1" || a.OC_PorValidar == "4"
                             select new
                             {
                                 a.Id,
                                 a.Clave,
                                 a.Fecha,
                                 a.UsuarioNuevo,
                                 IdReq = req.claveOLD,
                                 FReq = req.Fecha,
                                 Cont = req.EstatusContrato,
                                 Val = a.OC_PorValidar,
                                 Desc = a.Descripcion
                             }).ToList();

                var results1 = new List<ListCampos>();

                foreach (var q in query)
                {
                    var resultado = new ListCampos
                    {
                        Id_OC = q.Id,
                        Clave = q.Clave,
                        Fecha = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha),
                        Id_User = q.UsuarioNuevo,
                        Id_Requisicion = Convert.ToInt32(q.IdReq),
                        Fecha1 = string.Format("{0:d/M/yyyy hh:mm tt}", q.FReq),
                        EstatusContrato = q.Cont,
                        Validar = q.Val,
                        DescripcionOC = q.Desc
                    };
                    results1.Add(resultado);
                }
                return Json(new { MENSAJE = "FOUND", OCS = results1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ValidarOC(int Clave_OC, string DescripcionOC)
        {
            try
            {
                var OC = (from a in ConBD2.SISM_ORDEN_COMPRA
                               where a.Clave == Clave_OC.ToString()
                               select a).FirstOrDefault();

                OC.OC_PorValidar = "2";// 2 Quiere decir que se Validó la O.C porque la OC nace como 1 (Generada) al validarla (2) el usuario de Compras puede Aprobarla/Generarla y el Status en BD cambia a True y OC_PorValidar puede cambiar a 3
                OC.Descripcion = DescripcionOC;
                ConBD2.SaveChanges();

                return Json(new { MENSAJE = "Succe: Se autorizó la O.C" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult HACER_OC(int Clave_OC)
        {
            try
            {
                //ORDEN DE COMPRA
                var OC = (from a in ConBD2.SISM_ORDEN_COMPRA
                          where a.Clave == Clave_OC.ToString()
                          select a).FirstOrDefault();

                OC.Status = true;
                OC.OC_PorValidar = "3";// el usuario de Compras puede Aprobarla/Generarla y el Status en BD cambia a True y OC_PorValidar puede cambiar a 3
                ConBD2.SaveChanges();

                //DETALLE DE LA O.C
                var DetalleOC = (from a in ConBD2.SISM_DETALLE_OC
                                    where a.Id_OrdenCompra == OC.Id
                                    select a
                            ).ToList();

                foreach (var q in DetalleOC)
                {
                    q.Status = true;
                    ConBD2.SaveChanges();
                }

                return Json(new { MENSAJE = "Succe: Se generó la O.C" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CancelarOC(int Clave_OC, string DescripcionOC)
        {
            try
            {
                var OC = (from a in ConBD2.SISM_ORDEN_COMPRA
                          where a.Clave == Clave_OC.ToString()
                          select a).FirstOrDefault();

                OC.OC_PorValidar = "4";// 4 Quiere decir que se Canceló la O.C 
                OC.Descripcion = DescripcionOC;
                ConBD2.SaveChanges();

                return Json(new { MENSAJE = "Succe: Se canceló la O.C" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //------------------                            PARCIALES      INICIO ----------------------
        [Authorize]
        public ActionResult OrdenCompraParcial()
        {
            ViewBag.PROVEEDORES = new SelectList(db.Proveedor.ToList(), "Id", "Prov_Nombre");

            return View();
        }

        //LISTADO DE LAS ORDENES DE COMPRA **PARCIALES**    PANTALLA INICIO
        public ActionResult ObtenerOCInicio_Parciales()
        {
            try
            {
                var query = (from a in ConBD2.SISM_ORDEN_COMPRA
                             join req in ConBD2.SISM_REQUISICION on a.Id_Requisicion equals req.Id_Requicision
                             where a.Estatus_OC == "OC Parcial"
                             select new
                             {
                                 a.Id,
                                 a.Clave,
                                 a.Fecha,
                                 a.UsuarioNuevo
                             }).ToList();

                var results1 = new List<ListCampos>();

                foreach (var q in query)
                {
                    var resultado = new ListCampos
                    {
                        Id_OC = q.Id,
                        Clave = q.Clave,
                        Fecha = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha),
                        Id_User = q.UsuarioNuevo
                    };
                    results1.Add(resultado);
                }
                return Json(new { MENSAJE = "FOUND", OCS = results1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ObtenerDetalleParcial(string Id_OC)
        {
            try
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                var fechaDT = DateTime.Parse(fecha);

                var query = (from a in ConBD2.SISM_ORDEN_COMPRA
                             join DetOC in ConBD2.SISM_DETALLE_OC on a.Id equals DetOC.Id_OrdenCompra
                             join Requi in ConBD2.SISM_REQUISICION on a.Id_Requisicion equals Requi.Id_Requicision
                             where a.Clave == Id_OC
                             where DetOC.ItemPendiente == true
                             select new
                             {
                                 Folio =        a.Clave,
                                 Fecha =        a.Fecha,
                                 Usuario =      a.UsuarioNuevo,
                                 Cantidad =     DetOC.CantidadPendiente,
                                 Precio =       DetOC.PreUnit,
                                 Descripcion =  DetOC.Descripcion,
                                 ClaveMed =     DetOC.ClaveMedicamento,
                                 PU =           DetOC.PreUnit,
                                 Total =        DetOC.Total,
                                 NombreProveedor = a.NombreProveedor,
                                 a.             Total_OC,
                                 Pendiente =    DetOC.ItemPendiente,

                                 Requi =        a.Id_Requisicion,
                                 DescO =        a.Descripcion,
                                 CBarra =       DetOC.Id_CodigoBarrar,
                                 Can =          DetOC.Cantidad,
                                 Sustancia =    DetOC.Id_Sustencia,
                                 ClaveReq =     Requi.claveOLD
                             }).ToList();

                var results1 = new List<ListCampos>();
                //LISTA DE LA O.C Y SU DETALLE
                foreach (var q in query)
                {
                    var resultado = new ListCampos
                    {
                        Folio = q.Folio,
                        Fecha = string.Format("{0:d/M/yy hh:mm tt}", q.Fecha),
                        Fecha1 = string.Format("{0:d/M/yy hh:mm tt}", fechaDT),
                        Usuario = q.Usuario,
                        Descripcion = q.Descripcion,
                        Clave = q.ClaveMed,
                        Cantidad = (int)q.Cantidad,
                        PrecioUnitario = (double)q.PU,
                        Total = (double)q.Total,
                        NombreProveedor = q.NombreProveedor,
                        Total_OC = (double)q.Total_OC,

                        Id_Requicision = (int)q.Requi,
                        DescO = q.DescO,
                        CBarra = q.CBarra,
                        Can = q.Can,
                        Id_Sustancia = (int)q.Sustancia,
                        FolioRequisicion = q.ClaveReq
                    };
                    results1.Add(resultado);
                }

                return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GenerarOC_Parcial(List<SISM_DET_REQUISICION> ListaOC, string FolioOC/*, string ProveedorReq*/, int FolioRequi)
        {
            var UsuarioRegistra = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;
            var IdUsuarioCifrado = User.Identity.GetUserId();
            //return null;
            try
            {
                //Obtenemos la OC "madre" (la que es Parcial) para poner su Id en la Nueva OC que se creará a partir de la OC Parcial
                var OC_Anterior = (from a in ConBD2.SISM_ORDEN_COMPRA
                            where a.Clave == FolioOC
                          select a).OrderByDescending(u => u.Id).FirstOrDefault();

                //PROVEEDOR de la Requi(Orden)
                //var Prov = (from a in db.Proveedor
                //            where a.Prov_Nombre == ProveedorReq
                //            select a
                //            ).FirstOrDefault();
                //REQUI que se hará Orden
                var Requi = (from a in ConBD2.SISM_REQUISICION
                             where a.claveOLD == FolioRequi.ToString()
                             select a
                            ).FirstOrDefault();

                var UsuarioOld = User.Identity.GetUserName();
                var UsuarioOLD = (from a in db.Usuario
                                  where a.Usu_User == UsuarioOld
                                  select a).FirstOrDefault();

                #region TODA LA ACCION
                //CREAR ORDEN NUEVA a partir de una Requi
                SISM_ORDEN_COMPRA OC = new SISM_ORDEN_COMPRA();
                //OC.Clave = CLAVE.ToString();
                OC.Id_Requisicion = Requi.Id_Requicision;
                //OC.Id_Proveedor = Prov.Id;
                OC.Fecha = fechaDT;
                OC.FechaMod = fechaDT;
                OC.Forma_Pago = "";
                OC.Folio = "";
                OC.Status = false;
                OC.UsuarioId = UsuarioOLD.UsuarioId;
                OC.Cerrado = true;
                OC.Cuadro = 1;
                OC.UsuarioNuevo = UsuarioRegistra;
                OC.IP_User = ip_realiza;
                //OC.NombreProveedor = Prov.Prov_Nombre;
                OC.OC_PorValidar = "1"; //LA OC nace en 1, tiene que validarse para pasar a 2 (validada) despues tiene que Generarse la OC y pasa a 3, el Status cambiará a True
                OC.EsParcial_IdOC = OC_Anterior.Id;
                ConBD2.SISM_ORDEN_COMPRA.Add(OC);
                ConBD2.SaveChanges();
                //ACTUALIZAMOS LA REQUI EN SU COLUMNA 'EstatusOC' poniendo 1 ya que esa requi se hará O.C
                //ConBD2.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET EstatusOC = '1' WHERE Id_Requicision='" + Requi.Id_Requicision + "';");

                //obtener la ultima 'clave' de la tabla OrdenCompra actualBD vieja) para que inserte un nuevo registro en la nueva BD CONSECUTIVO de la clave
                var Clave = (from a in db.OrdenCompra
                             select new
                             {
                                 clave = a.clave
                             }).OrderByDescending(u => u.clave).FirstOrDefault();
                var AñoMes_Actual = string.Format("{0:yyMM}", fechaDT);
                //var UltimoConsecutivo_Clave = Convert.ToInt32(Clave.clave.Substring(4));
                var UltimoConsecutivo_Clave = Convert.ToInt32(OC_Anterior.Clave.Substring(4));
                var ConsecutivoNuevo = ((UltimoConsecutivo_Clave) + 1);
                var ConsecutivoNuevoTxt = "";
                if (ConsecutivoNuevo < 100)
                {
                    if (ConsecutivoNuevo < 9)
                    {
                        ConsecutivoNuevoTxt = "00" + ConsecutivoNuevo;
                    }
                    else
                    {
                        ConsecutivoNuevoTxt = "0" + ConsecutivoNuevo;
                    }
                }
                else
                {
                    ConsecutivoNuevoTxt = "" + ConsecutivoNuevo;
                }
                //Obtenemos la ultima O.C guardada(que es esta) para guardar su detalle
                var IdOC = (from a in ConBD2.SISM_ORDEN_COMPRA
                            where a.UsuarioNuevo == UsuarioRegistra
                            where a.Fecha == fechaDT
                            select a).OrderByDescending(u => u.Id).FirstOrDefault();
                //ACTUALIZAMOS LA CLAVE DE LA O'C 
                ConBD2.Database.ExecuteSqlCommand("UPDATE SISM_ORDEN_COMPRA SET Clave = '" + AñoMes_Actual + ConsecutivoNuevoTxt + "' WHERE Id='" + IdOC.Id + "';");
                //RECORREMOS  para guardar en la tabla DETALLE ORDEN
                foreach (var item in ListaOC)
                {
                    //CREAR EL DETALLE DE LA NUEVA ORDEN
                    SISM_DETALLE_OC DetalleOC = new SISM_DETALLE_OC();
                    //BUSCAMOS EN LA TABLA "CodigoBarras" el Id del Codigo de Barras, buscando por el Id de la Sustancia
                    var CodigoBarras = (from a in SISMFarmacia.CodigoBarras
                                        where a.Id_Sustancia == item.Id_Sustancia
                                        select a).FirstOrDefault();
                    //Obtenemos la info de SUSTANCIA de la tabla Detalle_Requi
                    var Sustancia = (from a in ConBD2.SISM_DET_REQUISICION
                                     where a.Clave == item.Clave
                                     select a).FirstOrDefault();
                    DetalleOC.Id_OrdenCompra = IdOC.Id;
                    DetalleOC.Id_CodigoBarrar = CodigoBarras.Id;
                    DetalleOC.Obsequio = 0;
                    DetalleOC.Status = false;
                    DetalleOC.Id_Sustencia = item.Id_Sustancia;
                    DetalleOC.Descripcion = Sustancia.Descripcion;
                    DetalleOC.ClaveMedicamento = Sustancia.Clave;
                    //1 quiere decir que el item está pendiente, 0 quiere decir que si se surtió el item
                    DetalleOC.ItemPendiente = item.CB_ELIMINAR;

                    //En DetalleOC.Cantidad se guarda la Cantidad de pzas que pide Almacen siempre
                    DetalleOC.Cantidad = item.Cantidad;

                    //if (item.CANTIDAD_NUEVA > 0)
                    //{
                    //    //Aqui se guardan las pzas que modificó Compras 
                    //    DetalleOC.CantidadOC = item.CANTIDAD_NUEVA;
                    //    //Se guarda la cantidad de items total (al modificarse la cantidad esa nueva cantidad pasa a ser la oficial)
                    //    DetalleOC.CantidadItema_OC = item.CANTIDAD_NUEVA;

                    //    if (item.CANTIDAD_NUEVA < item.Cantidad)
                    //    {
                    //        DetalleOC.ItemPendiente = true;
                    //        //OC.Estatus_OC = "OC Parcial";
                    //    }
                    //}
                    //else
                    //{
                    //    //Si no se modificó la CANTIDAD_NUEVA, la cantidad por default pasa a ser la oficial
                    //    DetalleOC.CantidadItema_OC = item.Cantidad;
                    //}

                    //Se valida si el PRECIO UNITARIO se modificó
                    if (item.PREUNIT_NUEVA > 0)
                    {
                        DetalleOC.PreUnit = item.PREUNIT_NUEVA;
                    }
                    else
                    {
                        DetalleOC.PreUnit = item.PrecioUnitario;
                    }
                    //Se valida si se ingresó una NUEVA CANTIDAD o un NUEVO PRECIO UNITARIO     $$TOTAL$$ 
                    if (item.CANTIDAD_NUEVA > 0 || item.PREUNIT_NUEVA > 0)
                    {
                        if (item.CANTIDAD_NUEVA > 0 && item.PREUNIT_NUEVA > 0)
                        {
                            DetalleOC.Total = (double?)decimal.Round((decimal)(item.CANTIDAD_NUEVA * item.PREUNIT_NUEVA), 2);
                        }
                        else
                        {
                            if (item.CANTIDAD_NUEVA > 0)
                            {
                                DetalleOC.Total = (double?)decimal.Round((decimal)(item.CANTIDAD_NUEVA * item.PrecioUnitario), 2);
                            }
                            if (item.PREUNIT_NUEVA > 0)
                            {
                                DetalleOC.Total = (double?)decimal.Round((decimal)(item.Cantidad * item.PREUNIT_NUEVA), 2);
                            }
                        }
                    }
                    else
                    {
                        DetalleOC.Total = (double?)decimal.Round((decimal)(DetalleOC.Cantidad * DetalleOC.PreUnit), 2);
                    }

                    ConBD2.SISM_DETALLE_OC.Add(DetalleOC);
                    ConBD2.SaveChanges();

                    if (item.CB_ELIMINAR == false)
                    {
                        SubTotal_OC += Decimal.Round((decimal)(DetalleOC.Total), 2);
                        OC.Total_OC = (double?)SubTotal_OC;
                    }



                    ConBD2.SaveChanges();
                }

                //Buscar en los items(medicamentos) del Detalle de la OC si alguno está pendiente (lo eliminaron a la hora de genera las OC)
                var ItemPendiente_DetalleOC = (from a in ConBD2.SISM_DETALLE_OC
                                               where a.ItemPendiente == true
                                               select a
                                    ).ToList();
                //Poner la OC como 'Parcial' ya que tiene items pendientes
                //if (ItemPendiente_DetalleOC.Count > 0)
                //{
                //    OC.Estatus_OC = "OC Parcial";
                //}
                //else
                //{
                //    OC.Estatus_OC = "OC Completa";
                //}
                ConBD2.SaveChanges();


                //, INFO = FolioNuevo_Oc.Clave
                return Json(new { MENSAJE = "Succe: Se generó la O.C" }, JsonRequestBehavior.AllowGet);
                #endregion
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        //------------------                            PARCIALES      FIN ----------------------
    }
}
