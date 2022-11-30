using Microsoft.AspNet.Identity;
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
                        Id_User = q.Id_User
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
                            PrecioUnitario = (double)q.PrecioUnitario,
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
                //DETALLE DE LA REQUI -lista-
                var DetalleRequi = (from a in ConBD2.SISM_DET_REQUISICION
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
                //OC.Clave = CLAVE.ToString();
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
                    ConsecutivoNuevoTxt = "0" + ConsecutivoNuevo;
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

                //RECORREMOS DETALLE DE LA REQUI para guardar en la tabla DETALLE ORDEN
                foreach (var item in DetalleRequi)
                {
                    //CREAR EL DETALLE DE LA NUEVA ORDEN
                    SISM_DETALLE_OC DetalleOC = new SISM_DETALLE_OC();

                    //BUSCAMOS EN LA TABLA "CodigoBarras" el Id del Codigo de Barras, buscando por el Id de la Sustancia
                    var CodigoBarras = (from a in SISMFarmacia.CodigoBarras
                                        where a.Id_Sustancia == item.Id_Sustancia
                                        select a).FirstOrDefault();

                    //BUSCAMOS EN LA TABLA "Sustancia" la SUSTANCIA
                    var Sustancia = (from a in SISMFarmacia.Sustancia
                                     where a.Id == item.Id_Sustancia
                                     select a).FirstOrDefault();

                    DetalleOC.Id_OrdenCompra = IdOC.Id;
                    DetalleOC.Id_CodigoBarrar = CodigoBarras.Id;
                    DetalleOC.Obsequio = 0;
                    DetalleOC.Status = false;
                    DetalleOC.Id_Sustencia = item.Id_Sustancia;
                    DetalleOC.Descripcion = Sustancia.descripcion_21;
                    DetalleOC.ClaveMedicamento = Sustancia.Clave;

                    DetalleOC.Pendiente = item.Cantidad;//NOTA: Columna de Vic (pendiente) es el mismo dato que CANTIDAD
                    DetalleOC.Cantidad = item.Cantidad;
                    DetalleOC.PreUnit = item.PrecioUnitario;
                    DetalleOC.Total = (double?)decimal.Round((decimal)(DetalleOC.Cantidad * DetalleOC.PreUnit), 2);
                    

                    ConBD2.SISM_DETALLE_OC.Add(DetalleOC);
                    ConBD2.SaveChanges();
                }
                return Json(new { MENSAJE = "Succe: Se generó la O.C" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //LISTADO DE LAS ORDENES DE COMPRA GENERADAS
        public ActionResult ObtenerOCInicio()
        {
            try
            {
                var query = (from a in ConBD2.SISM_ORDEN_COMPRA
                             join req in ConBD2.SISM_REQUISICION on a.Id_Requisicion equals req.Id_Requicision
                             select new { 
                             a.Clave,
                             a.Fecha,
                             a.UsuarioNuevo,
                             IdReq = req.claveOLD,
                             FReq = req.Fecha
                             }).ToList();

                var results1 = new List<ListCampos>();

                foreach (var q in query)
                {
                    var resultado = new ListCampos
                    {
                        Clave = q.Clave,
                        Fecha = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha),
                        Id_User = q.UsuarioNuevo,
                        Id_Requisicion = Convert.ToInt32(q.IdReq),
                        Fecha1 = string.Format("{0:d/M/yyyy hh:mm tt}", q.FReq)
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
                             where a.Clave == FolioOC.ToString()
                             select new
                             {
                                 Folio = a.Clave,
                                 Fecha = a.Fecha,
                                 Usuario = a.UsuarioNuevo,
                                 Cantidad = DetOC.Cantidad,
                                 Precio = DetOC.PreUnit,
                                 Descripcion = DetOC.Descripcion,
                                 ClaveMed = DetOC.ClaveMedicamento,
                                 PU = DetOC.PreUnit,
                                 Total = DetOC.Total
                             }).ToList();

                ViewBag.NombreProvedor = Prov.Prov_Nombre;

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
                        Total = (double)q.Total
                        
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
    }
}
