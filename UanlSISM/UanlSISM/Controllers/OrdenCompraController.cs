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
        SISM_SIST_MEDEntities ConBD = new SISM_SIST_MEDEntities();      //BD NUEVA PRODUCTIVA
        BD_Almacen ConBD2 = new BD_Almacen();                           //BD NUEVA PRUEBAS(mía)
        SERVMEDEntities4 DAM = new SERVMEDEntities4();                  //TBL SUSTANCIAS
        SERVMEDEntities8 db = new SERVMEDEntities8();                   //TBLS PROV,REQ,OC
        SERVMEDEntities5 SISMFarmacia = new SERVMEDEntities5();         //TBL CODIGOBARRAS
        //SERVMEDEntities5 db2 = new SERVMEDEntities5();
        SERVMEDEntities8 RequisicionDB = new SERVMEDEntities8();        //BD TABLAS viejas

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
            public int Id_Detalle_Req { get; internal set; }
            public int? CantidadPendiente_OC { get; internal set; }
            public int? Cantidad_OC { get; internal set; }
            public string Estatus_OC_Parcial { get; internal set; }
            public string FechaAcuse { get; internal set; }
            public string DirProv { get; internal set; }
            public string TelProv { get; internal set; }
            public string UanlProv { get; internal set; }
            public string NombreUsu { get; internal set; }
            public string FechaAutorizaOC { get; internal set; }
            public string UsuarioAutorizaOC { get; internal set; }
            public string Fecha_OC { get; internal set; }
            public int IdDetOC { get; internal set; }
            public bool? PartidaPendiente { get; internal set; }
            public string FechaOC { get; internal set; }
            public string OC_PorValidar { get; internal set; }
            public double UltimoPrecio { get; internal set; }
        }

        //----------------------------------------------------- Pantalla ORDEN COMPRA   --------------  INICIO
        [Authorize]
        public ActionResult OrdenCompra()
        {
            ViewBag.PROVEEDORES = new SelectList(db.Proveedor.ToList(), "Id", "Prov_Nombre");
            //ViewBag.PROVEEDORES = new SelectList(db.usuario.Include(ñ => ñ.rol).Where(u => u.rol.Nombre == "Cliente").ToList(), "Id", "Nombre");

            return View();
        }

        public ActionResult ObtenerRequisInicio()
        {
            try
            {
                var query = (from a in ConBD.SISM_REQUISICION
                                 //where a.EstatusOC == "0" || a.EstatusOC == null
                                 //where a.Estatus_OC_Parcial == "Incompleta" || a.Estatus_OC_Parcial == null
                             select a).ToList();

                var results1 = new List<ListCampos>();

                foreach (var q in query)
                {
                    var resultado = new ListCampos
                    {
                        Clave = q.claveOLD,
                        Fecha = string.Format("{0:d/M/yyyy HH:mm tt}", q.Fecha),
                        EstatusContrato = q.EstatusContrato,
                        Id_User = q.Id_User,
                        Estatus_OC = q.Estatus_OC_Parcial,
                        FechaRequisicion = string.Format("{0:yyyy/M/d HH:mm tt}", q.Fecha, new CultureInfo("es-ES"))
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
            public double PreUnit { get; internal set; }
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
                             //where det.PartidaPendiente_OC == false || det.PartidaPendiente_OC == null
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
                                 det.Id_Sustancia,
                                 a.Id_Requicision,
                                 det.Id_Detalle_Req,
                                 det.CantidadPendiente_OC,
                                 det.Cantidad_OC,
                                 a.Estatus_OC_Parcial,
                                 det.PartidaPendiente_OC
                             }).ToList();
                var results1 = new List<ListCampos>();

                foreach (var q in query)
                {
                    var sus = (from a in DAM.Sustancia
                               where a.Clave == q.Clave
                               select new { 
                               a.Id
                               }).FirstOrDefault();

                    if (sus != null)
                    {
                        //Nuevo inventario: Farmacia-85, Almacen-84
                        string query2 = "select ManejoDisponible as ManejoDisponible from InvAlmFarm WHERE Id_Sustancia = " + sus.Id + " and InvAlmId = 84";
                        var result2 = db.Database.SqlQuery<InvAlmFarm>(query2);
                        var res2 = result2.FirstOrDefault();

                        //string UP = "select D_OC.PreUnit as PreUnit "+
                        //    "from OrdenCompra OC " +
                        //    "INNER JOIN DetalleOC D_OC ON OC.Id = D_OC.Id_OrdenCompra " +
                        //    "INNER JOIN Sustancia Sus ON D_OC.Id_Sustancia = Sus.Id " +
                        //    "WHERE OC.Fecha >= '2020-01-01T00:00:00' AND D_OC.Id_Sustancia = " + sus.Id + " ";
                        //var PU = DAM.Database.SqlQuery<InvAlmFarm>(UP);
                        //var Precio = PU.FirstOrDefault();

                        var resultado = new ListCampos
                        {
                            Clave = q.Clave,
                            Existencia = res2.ManejoDisponible,
                            Descripcion = q.Descripcion.Trim(),
                            Cantidad = (int)q.Cantidad,
                            Folio = q.Folio,
                            Fecha = string.Format("{0:d/M/yy hh:mm tt}", q.Fecha),
                            Usuario = q.Id_User,
                            Fecha1 = string.Format("{0:d/M/yy hh:mm tt}", fechaDT),
                            EstatusContrato = q.EstatusContrato,
                            Compendio = q.Compendio,
                            PrecioUnitario = (double)q.PrecioUnitario,
                            Total = (double)q.Total,
                            Id_Sustancia = q.Id_Sustancia,
                            Id_Requicision = q.Id_Requicision,
                            Id_Detalle_Req = q.Id_Detalle_Req,
                            CantidadPendiente_OC = q.CantidadPendiente_OC,
                            Cantidad_OC = q.Cantidad_OC,
                            Estatus_OC_Parcial = q.Estatus_OC_Parcial,
                            PartidaPendiente = q.PartidaPendiente_OC,
                            //UltimoPrecio = Precio.PreUnit
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

        decimal? SubTotal_OC = 0.00m;
        public JsonResult GenerarOC(List<SISM_DET_REQUISICION> ListaOC, int FolioRequi, string ProveedorReq)
        {
            var UsuarioRegistra = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;
            var IdUsuarioCifrado = User.Identity.GetUserId();

            //Si la CANTIDAD_NUEVA es mayor a cero (0) el Checkbox se pondrá en True (eso quiere decir que esos itemas o partidas se incluirán en la O.C) ya que solo se harán los 
            //Updates solo si el CB_ELIMINAR es True.
            foreach (var Partida in ListaOC)
            {
                if (Partida.CANTIDAD_NUEVA > 0)
                {
                    Partida.CB_ELIMINAR = true;
                }
                else
                {
                    Partida.CB_ELIMINAR = false;
                }
            }

            //---------------------------------------------  Editar tablas de REQUIS    (parcialidades)       --------------------  INICIO  --------------------------------------
            #region REQUIS

            //RECORREMOS EL DETALLE DE LA REQUI QUE RECIBIMOS(LOS NUEVOS DATOS) PARA BUSCAR ESE DETALLE EN LAS TABLAS Y ACTUALIZARLO CON LOS NUEVOS DATOS RECIBIDOS
            foreach (var DetRequi in ListaOC)
            {
                //BUSCAMOS EL DETALLE DE LA REQUI QUE SE ACTUALIZARÁ
                var RequiDetalle_Actualizar = (from a in ConBD.SISM_REQUISICION
                                               join det in ConBD.SISM_DET_REQUISICION on a.Id_Requicision equals det.Id_Requicision
                                               where det.Id_Detalle_Req == DetRequi.Id_Detalle_Req
                                               select new
                                               {
                                                   det.Id_Detalle_Req,
                                                   det.Cantidad,
                                                   det.PrecioUnitario,
                                                   det.Total,
                                                   det.Cantidad_OC,
                                                   det.CantidadPendiente_OC,
                                                   det.PartidaPendiente_OC
                                               }).FirstOrDefault();

                //                                  DETALLE REQUI =>    CANTIDADES
                if (DetRequi.CANTIDAD_NUEVA > 0)
                {
                    if (DetRequi.CB_ELIMINAR == true)
                    {
                        if (DetRequi.CANTIDAD_NUEVA < DetRequi.Cantidad)
                        {
                            if (DetRequi.CantidadPendiente_OC == null || DetRequi.CantidadPendiente_OC == 0)
                            {
                                var CantidadPendiente = DetRequi.Cantidad - DetRequi.CANTIDAD_NUEVA;
                                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Cantidad_OC = '" + DetRequi.CANTIDAD_NUEVA + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET CantidadPendiente_OC = '" + CantidadPendiente + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                            }
                            else
                            {
                            }
                        }
                        if (DetRequi.CANTIDAD_NUEVA == DetRequi.Cantidad)
                        {
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Cantidad_OC = '" + DetRequi.Cantidad + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET CantidadPendiente_OC = '" + 0 + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                        }
                        if (DetRequi.CANTIDAD_NUEVA == DetRequi.CantidadPendiente_OC)
                        {
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Cantidad_OC = '" + DetRequi.CANTIDAD_NUEVA + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET CantidadPendiente_OC = '" + 0 + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                        }
                        if (DetRequi.CANTIDAD_NUEVA < DetRequi.CantidadPendiente_OC)
                        {
                            var CantidadPendiente = DetRequi.CantidadPendiente_OC - DetRequi.CANTIDAD_NUEVA;
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Cantidad_OC = '" + DetRequi.CANTIDAD_NUEVA + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET CantidadPendiente_OC = '" + CantidadPendiente + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                        }
                    }
                }
                else
                {
                    if (DetRequi.CB_ELIMINAR == true)
                    {
                        if (DetRequi.CANTIDAD_NUEVA == 0 && DetRequi.CantidadPendiente_OC > 0)
                        {
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Cantidad_OC = '" + DetRequi.CantidadPendiente_OC + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET CantidadPendiente_OC = '" + 0 + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                        }
                        else
                        {
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Cantidad_OC = '" + DetRequi.Cantidad + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET CantidadPendiente_OC = '" + 0 + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                        }
                    }
                }

                //                                  DETALLE REQUI =>    PRECIO UNITARIO
                if (DetRequi.PREUNIT_NUEVA > 0)
                {
                    if (DetRequi.CB_ELIMINAR == true)
                    {
                        ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET PrecioUnitario = '" + DetRequi.PREUNIT_NUEVA + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                    }
                }

                //                                  DETALLE REQUI =>             NUEVA CANTIDAD y/o NUEVO PRECIO UNITARIO     $$TOTAL$$ 
                if (DetRequi.CANTIDAD_NUEVA > 0 || DetRequi.PREUNIT_NUEVA > 0)
                {
                    if (DetRequi.CANTIDAD_NUEVA > 0 && DetRequi.PREUNIT_NUEVA > 0)
                    {
                        var Total = (double?)decimal.Round((decimal)(DetRequi.CANTIDAD_NUEVA * DetRequi.PREUNIT_NUEVA), 2);
                        if (DetRequi.CB_ELIMINAR == true)
                        {
                            ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Total = '" + Total + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                        }
                    }
                    else
                    {
                        if (DetRequi.CANTIDAD_NUEVA > 0)
                        {
                            var Total = (double?)decimal.Round((decimal)(DetRequi.CANTIDAD_NUEVA * DetRequi.PrecioUnitario), 2);
                            if (DetRequi.CB_ELIMINAR == true)
                            {
                                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Total = '" + Total + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                            }
                        }
                        if (DetRequi.PREUNIT_NUEVA > 0)
                        {
                            if (DetRequi.CantidadPendiente_OC > 0)
                            {
                                var Total = (double?)decimal.Round((decimal)(DetRequi.CantidadPendiente_OC * DetRequi.PREUNIT_NUEVA), 2);
                                if (DetRequi.CB_ELIMINAR == true)
                                {
                                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Total = '" + Total + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                                }
                            }
                            else
                            {
                                var Total = (double?)decimal.Round((decimal)(DetRequi.Cantidad * DetRequi.PREUNIT_NUEVA), 2);
                                if (DetRequi.CB_ELIMINAR == true)
                                {
                                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Total = '" + Total + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                                }
                            }
                            //var Total = (double?)decimal.Round((decimal)(DetRequi.Cantidad * DetRequi.PREUNIT_NUEVA), 2);
                            //if (DetRequi.CB_ELIMINAR == true)
                            //{
                            //    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Total = '" + Total + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                            //}
                        }
                    }
                }
                else
                {
                    var Total = (double?)decimal.Round((decimal)(DetRequi.Cantidad * DetRequi.PrecioUnitario), 2);
                    if (DetRequi.CB_ELIMINAR == true)
                    {
                        ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Total = '" + Total + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                    }
                }

                // SE OBTIENE LA REQUI Y SU DETALLE CON EL QUE SE ESTÁ TRABAJANDO AHORA CON LOS NUEVOS DATOS QUE SE HAN IDO ACTUALIZANDO EN ESTA MISMA FUNCIÓN
                var RequiDetalle_Nueva = (from a in ConBD.SISM_REQUISICION
                                          join det in ConBD.SISM_DET_REQUISICION on a.Id_Requicision equals det.Id_Requicision
                                          where det.Id_Detalle_Req == DetRequi.Id_Detalle_Req
                                          select new
                                          {
                                              det.Id_Detalle_Req,
                                              det.Cantidad,
                                              det.PrecioUnitario,
                                              det.Total,
                                              det.Cantidad_OC,
                                              det.CantidadPendiente_OC,
                                              det.PartidaPendiente_OC
                                          }).FirstOrDefault();

                //                                  DETALLE REQUI =>    PARTIDA PENDIENTE
                //Si "CantidadPendiente_OC" es mayor a 0 quiere decir que esa partida o item aún tiene Cantidades por surtir entonces esa Requi sigue siendo una        REQUI PARCIAL
                if (RequiDetalle_Nueva.CantidadPendiente_OC > 0 || RequiDetalle_Nueva.CantidadPendiente_OC == null)
                {
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET PartidaPendiente_OC = '" + false + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                }
                else
                {
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET PartidaPendiente_OC = '" + true + "' WHERE Id_Detalle_Req='" + RequiDetalle_Actualizar.Id_Detalle_Req + "';");
                }
            }

            //  **NOTA => EN CAMPO 'PartidaPendiente_OC' DE LA TABLA DetalleRequi, EL '1' QUIERE DECIR QUE ESE ITEM SE AGREGÓ A LA O'C (O SE COMPLETO) Y SE IRÁ EN LA O'C. 
            //EL '0' QUIERE DECIR QUE ESE ITEM ESTÁ PENDIENTE

            //OBTENER EL "Id" DE LA REQUI CON LA QUE SE ESTÁ TRABAJANDO
            var RequiDetalle_Nueva_X2 = (from a in ConBD.SISM_REQUISICION
                                         where a.claveOLD == FolioRequi.ToString()
                                         select a).FirstOrDefault();

            //BUSCAMOS SI HAY 'DETALLES' CON 'False' EN EL CAMPO 'PartidaPendiente_OC' O SEA: PARTIDAS PENDIENTES
            var Registro = (from a in ConBD.SISM_REQUISICION
                            join det in ConBD.SISM_DET_REQUISICION on a.Id_Requicision equals det.Id_Requicision
                            where a.claveOLD == FolioRequi.ToString()
                            where det.PartidaPendiente_OC == false
                            select a).FirstOrDefault();

            //SI HAY PARTIDAS PENDIENTES, LA REQUI SE PONDRÁ COMO "Incompleta" EN EL CAMPO 'Estatus_OC_Parcial' EN TBL Requisicion
            if (Registro != null)
            {
                var Incompleta = "Parcial";
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET Estatus_OC_Parcial = '" + Incompleta + "' WHERE Id_Requicision='" + RequiDetalle_Nueva_X2.Id_Requicision + "';");
            }
            else //SI NO HAY PARTIDAS PENDIENTES QUIERE DECIR QUE LA OC SE HIZO COMPLETA
            {
                var Completa = "Completa";
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET Estatus_OC_Parcial = '" + Completa + "' WHERE Id_Requicision='" + RequiDetalle_Nueva_X2.Id_Requicision + "';");
            }

            #endregion
            //---------------------------------------------  Editar tablas de REQUIS    (parcialidades)       --------------------  FIN     --------------------------------------


            //---------------------------------------------     Crear nueva ORDEN DE COMPRA     ---------------------------------   INICIO  -----------------------------------
            #region CREAR ORDEN DE COMPRA
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

                var UsuarioOld = User.Identity.GetUserName();
                var UsuarioOLD = (from a in db.Usuario
                                  where a.Usu_User == UsuarioOld
                                  select a).FirstOrDefault();


                //CREAR ORDEN NUEVA a partir de una Requi
                SISM_ORDEN_COMPRA OC = new SISM_ORDEN_COMPRA();
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
                OC.NombreProveedor = Prov.Prov_Nombre;
                OC.OC_PorValidar = "1"; //LA OC nace en 1, tiene que validarse para pasar a 2 (validada) despues tiene que Generarse la OC y pasa a 3, el Status cambiará a True

                //foreach (var DetRequi2 in ListaOC)
                //{
                //    if (DetRequi2.CB_ELIMINAR == true)
                //    {
                //        ConBD.SISM_ORDEN_COMPRA.Add(OC);
                //    }
                //}

                ConBD.SISM_ORDEN_COMPRA.Add(OC);
                ConBD.SaveChanges();

                //ACTUALIZAMOS LA REQUI EN SU COLUMNA 'EstatusOC' poniendo 1 ya que esa requi se hará O.C
                //ConBD.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET EstatusOC = '1' WHERE Id_Requicision='" + Requi.Id_Requicision + "';");

                ////-----------------------------------------     FOLIO/CLAVE O'C     ----------------------------------------------------------------------------    INICIO  ----------
                ////obtener la ultima 'clave' de la tabla OrdenCompra actual BD (vieja) para que inserte un nuevo registro en la nueva BD CONSECUTIVO de la clave
                //var Clave = (from a in db.OrdenCompra
                //             select new
                //             {
                //                 clave = a.clave
                //             }).OrderByDescending(u => u.clave).FirstOrDefault();
                //var AñoMes_Actual = string.Format("{0:yyMM}", fechaDT);
                //var UltimoConsecutivo_Clave = Convert.ToInt32(Clave.clave.Substring(4));
                //var ConsecutivoNuevo = ((UltimoConsecutivo_Clave) + 1);
                //var ConsecutivoNuevoTxt = "";
                //if (ConsecutivoNuevo < 100)
                //{
                //    if (ConsecutivoNuevo < 9)
                //    {
                //        ConsecutivoNuevoTxt = "00" + ConsecutivoNuevo;
                //    }
                //    else
                //    {
                //        ConsecutivoNuevoTxt = "0" + ConsecutivoNuevo;
                //    }
                //}
                //else
                //{
                //    ConsecutivoNuevoTxt = "" + ConsecutivoNuevo;
                //}

                ////Obtenemos la ultima O.C guardada(que es esta) para guardar su detalle
                //var IdOC = (from a in ConBD.SISM_ORDEN_COMPRA
                //            where a.UsuarioNuevo == UsuarioRegistra
                //            where a.Fecha == fechaDT
                //            select a).OrderByDescending(u => u.Id).FirstOrDefault();

                ////ACTUALIZAMOS LA 'CLAVE' DE LA O'C 
                //ConBD.Database.ExecuteSqlCommand("UPDATE SISM_ORDEN_COMPRA SET Clave = '" + AñoMes_Actual + ConsecutivoNuevoTxt + "' WHERE Id='" + IdOC.Id + "';");
                ////-----------------------------------------     FOLIO/CLAVE O'C     ----------------------------------------------------------------------------    FIN  ----------
                //Obtenemos la ultima O.C guardada(que es esta) para guardar su detalle
                var IdOC = (from a in ConBD.SISM_ORDEN_COMPRA
                            where a.UsuarioNuevo == UsuarioRegistra
                            where a.Fecha == fechaDT
                            select a).OrderByDescending(u => u.Id).FirstOrDefault();


                //RECORREMOS  para guardar en la tabla DETALLE ORDEN        **DETALLE ORDEN DE COMPRA**
                foreach (var item in ListaOC)
                {
                    //CREAR EL DETALLE DE LA NUEVA ORDEN
                    SISM_DETALLE_OC DetalleOC = new SISM_DETALLE_OC();

                    //BUSCAMOS EN LA TABLA "CodigoBarras" el Id del Codigo de Barras, buscando por el Id de la Sustancia
                    var CodigoBarras = (from a in SISMFarmacia.CodigoBarras
                                        where a.Id_Sustancia == item.Id_Sustancia
                                        select a).FirstOrDefault();

                    //Obtenemos la info de SUSTANCIA de la tabla Detalle_Requi
                    var Sustancia = (from a in ConBD.SISM_DET_REQUISICION
                                     where a.Clave == item.Clave
                                     select a).FirstOrDefault();
                    if (item.CB_ELIMINAR == true)
                    {

                        DetalleOC.Id_OrdenCompra = IdOC.Id;
                        DetalleOC.Id_CodigoBarrar = CodigoBarras.Id;
                        DetalleOC.Obsequio = 0;
                        DetalleOC.Status = false;
                        DetalleOC.Id_Sustencia = item.Id_Sustancia;
                        DetalleOC.Descripcion = Sustancia.Descripcion;
                        DetalleOC.ClaveMedicamento = Sustancia.Clave;

                        //                                              *CANTIDAD*
                        if (item.CANTIDAD_NUEVA > 0)
                        {
                            DetalleOC.Cantidad = item.CANTIDAD_NUEVA;
                        }
                        else
                        {
                            if (item.CANTIDAD_NUEVA == 0 && item.CantidadPendiente_OC > 0)
                            {
                                DetalleOC.Cantidad = item.CantidadPendiente_OC;
                            }
                            else
                            {
                                DetalleOC.Cantidad = item.Cantidad;
                            }
                        }

                        DetalleOC.Pendiente = DetalleOC.Cantidad;

                        //Se valida si el PRECIO UNITARIO se modificó   *PRECIO UNITARIO*
                        if (item.PREUNIT_NUEVA > 0)
                        {
                            DetalleOC.PreUnit = item.PREUNIT_NUEVA;
                        }
                        else
                        {
                            DetalleOC.PreUnit = item.PrecioUnitario;
                        }

                        //Se valida si se ingresó una NUEVA CANTIDAD o un NUEVO PRECIO UNITARIO     *$TOTAL$*
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
                                    if (item.CantidadPendiente_OC > 0)
                                    {
                                        DetalleOC.Total = (double?)decimal.Round((decimal)(item.CantidadPendiente_OC * item.PREUNIT_NUEVA), 2);
                                    }
                                    else
                                    {
                                        DetalleOC.Total = (double?)decimal.Round((decimal)(item.Cantidad * item.PREUNIT_NUEVA), 2);
                                    }
                                    //DetalleOC.Total = (double?)decimal.Round((decimal)(item.Cantidad * item.PREUNIT_NUEVA), 2);
                                }
                            }
                        }
                        else
                        {
                            DetalleOC.Total = (double?)decimal.Round((decimal)(DetalleOC.Cantidad * DetalleOC.PreUnit), 2);
                        }

                        ConBD.SISM_DETALLE_OC.Add(DetalleOC);
                        ConBD.SaveChanges();

                        //                                                              *$TOTAL$ TBL O'C*
                        SubTotal_OC += Decimal.Round((decimal)(DetalleOC.Total), 2);
                        OC.Total_OC = (double?)SubTotal_OC;

                        ConBD.SaveChanges();

                    }

                }

                return Json(new { MENSAJE = "Succe: Se generó la O.C" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            #endregion
            //---------------------------------------------     Crear nueva ORDEN DE COMPRA     ---------------------------------   FIN  -----------------------------------

            //return null;
        }

        //----------------------------------------------------- Pantalla ORDEN COMPRA   --------------  FIN

        //-------------------------------------------------------------------- Pantalla ORDENES COMPRA   --------------  INICIO

        [Authorize]
        public ActionResult OrdenesCompra()
        {
            return View();
        }

        //LISTADO DE LAS ORDENES DE COMPRA GENERADAS
        public ActionResult ObtenerOCInicio()
        {
            try
            {
                var query = (from a in ConBD.SISM_ORDEN_COMPRA
                             join req in ConBD.SISM_REQUISICION on a.Id_Requisicion equals req.Id_Requicision
                             select new
                             {
                                 a.Id,
                                 a.Clave,
                                 a.Fecha,
                                 a.UsuarioNuevo,
                                 IdReq = req.claveOLD,
                                 FReq = req.Fecha,
                                 Cont = req.EstatusContrato,
                                 a.OC_PorValidar,
                                 //a.Estatus_OC
                                 a.Fecha_HacerOC
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
                        //Estatus_OC = q.Estatus_OC
                        FechaRequisicion = string.Format("{0:yyyy/M/d hh:mm tt}", q.Fecha, new CultureInfo("es-ES")),
                        Fecha_OC = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha_HacerOC),
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

        public JsonResult ObtenerDetalleOC_Generada(int Id_OC, int FolioRequi)
        {
            try
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                var fechaDT = DateTime.Parse(fecha);

                var IdRequi = (from a in ConBD.SISM_REQUISICION
                               where a.claveOLD == FolioRequi.ToString()
                               select new
                               {
                                   a.Id_Requicision
                               }).FirstOrDefault();
                var IdOC = (from a in ConBD.SISM_ORDEN_COMPRA
                            where a.Id == Id_OC
                            select a
                               ).FirstOrDefault();
                //PROVEEDOR de Orden
                var Prov = (from a in db.Proveedor
                            where a.Id == IdOC.Id_Proveedor
                            select a
                            ).FirstOrDefault();
                var Usuario = (from a in db.Usuario
                               where a.UsuarioId == IdOC.UsuarioId
                               select a
                            ).FirstOrDefault();

                var query = (from a in ConBD.SISM_ORDEN_COMPRA
                             join DetOC in ConBD.SISM_DETALLE_OC on a.Id equals DetOC.Id_OrdenCompra
                             join Requi in ConBD.SISM_REQUISICION on a.Id_Requisicion equals Requi.Id_Requicision
                             where a.Id == Id_OC
                             //where DetOC.ItemPendiente == false
                             select new
                             {
                                 Folio = a.Clave,
                                 Id_OC = a.Id,
                                 Fecha = a.Fecha,
                                 Usuario = a.UsuarioNuevo,
                                 //Cantidad = DetOC.CantidadOC,
                                 Cantidad = DetOC.Cantidad,
                                 Precio = DetOC.PreUnit,
                                 Descripcion = DetOC.Descripcion,
                                 ClaveMed = DetOC.ClaveMedicamento,
                                 PU = DetOC.PreUnit,
                                 Total = DetOC.Total,
                                 NombreProveedor = a.NombreProveedor,
                                 a.Total_OC,
                                 Desc = a.Descripcion,
                                 //Pendiente = DetOC.ItemPendiente,
                                 FolioR = Requi.claveOLD,
                                 FechaAcuse = a.Fecha_Acuse,
                                 FechaAutorizaOC = a.Fecha_AutorizaOC,
                                 UsuarioAutorizaOC = a.Usuario_AutorizaOC,
                                 IdDetOC = DetOC.Id,
                                 FechaOC = a.Fecha_HacerOC,
                                 PorValidar = a.OC_PorValidar
                             }).ToList();

                //ViewBag.NombreProvedor = Prov.Prov_Nombre;

                var results1 = new List<ListCampos>();
                //LISTA DE LA O.C Y SU DETALLE
                foreach (var q in query)
                {
                    var resultado = new ListCampos
                    {
                        Folio = q.Folio,
                        Id_OC = q.Id_OC,
                        Fecha = string.Format("{0:d/M/yy hh:mm tt}", q.Fecha),
                        Fecha1 = string.Format("{0:d/M/yy hh:mm tt}", fechaDT),
                        Usuario = q.Usuario,
                        Descripcion = q.Descripcion,
                        Clave = q.ClaveMed,
                        Cantidad = (int)q.Cantidad,
                        PrecioUnitario = (double)q.PU,
                        Total = (double)q.Total,
                        NombreProveedor = q.NombreProveedor,
                        Total_OC = (double)q.Total_OC,//(double)Decimal.Round((decimal)(q.Total_OC), 2),//(double)q.Total_OC,
                        DescripcionOC = q.Desc,
                        FolioRequisicion = q.FolioR,
                        FechaAcuse = string.Format("{0:d/M/yy hh:mm tt}", q.FechaAcuse),
                        DirProv = Prov.Prov_Direccion,
                        TelProv = Prov.Prov_Telefono,
                        UanlProv = Prov.Prov_uanl,
                        NombreUsu = Usuario.Usu_Nombre,
                        FechaAutorizaOC = string.Format("{0:d/M/yy hh:mm tt}", q.FechaAutorizaOC),
                        UsuarioAutorizaOC = q.UsuarioAutorizaOC,
                        IdDetOC = q.IdDetOC,
                        FechaOC = string.Format("{0:d/M/yy hh:mm tt}", q.FechaOC),
                        OC_PorValidar = q.PorValidar
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

        public JsonResult HACER_OC(int Id_OC, int Folio_Requi)
        {
            var UsuarioRegistra = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);

            try
            {
                //------------------------------------------------------------------------ BASE DE DATOS NUEVA  -------------------------   INICIO  ------
                //  Se Actualizan las tablas nuevas: OrdenCompra - DetalleOrdenCompra - Cotizaciones

                //ORDEN DE COMPRA
                var OC = (from a in ConBD.SISM_ORDEN_COMPRA
                          where a.Id == Id_OC
                          select a).FirstOrDefault();

                OC.Status = true;
                OC.OC_PorValidar = "3";// el usuario de Compras puede Aprobarla/Generarla y el Status en BD cambia a True y OC_PorValidar puede cambiar a 3
                OC.Fecha_HacerOC = fechaDT;// Esta es la Fecha cuando Compras hace la OC una vez que la Autoriza Coordinacion
                ConBD.SaveChanges();

                //DETALLE DE LA O.C
                var DetalleOC = (from a in ConBD.SISM_DETALLE_OC
                                 where a.Id_OrdenCompra == OC.Id
                                 select a
                            ).ToList();

                foreach (var q in DetalleOC)
                {
                    q.Status = true;
                    ConBD.SaveChanges();
                }

                #region Crear e Insertar FOLIO/CLAVE en la BD Nueva
                //obtener la ultima 'clave' de la tabla OrdenCompra actual BD (vieja) para que inserte un nuevo registro en la nueva BD CONSECUTIVO de la clave
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

                //ACTUALIZAMOS LA 'CLAVE' DE LA O'C NUEVA
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_ORDEN_COMPRA SET Clave = '" + AñoMes_Actual + ConsecutivoNuevoTxt + "' WHERE Id='" + OC.Id + "';");
                #endregion

                #region Actualizar tbl COTIZACIONES (NUEVA BD)

                //CONSULTAR LA TBL COTIZACIONES CON LA OC QUE SE ESTÁ TRABAJANDO 
                var DetalleCotizacion = (from Cot in ConBD.SISM_COTIZACIONES
                                         join Oc in ConBD.SISM_ORDEN_COMPRA on Cot.Id_Requicision equals Oc.Id_Requisicion
                                         join DetOc in ConBD.SISM_DETALLE_OC on Oc.Id equals DetOc.Id_OrdenCompra
                                         where Oc.Id == Id_OC
                                         where DetOc.Id_Sustencia == Cot.Id_Sustancia
                                         select new
                                         {
                                             IdCot = Cot.Id,
                                             IdSus = Cot.Id_Sustancia,
                                             IdProv = Cot.Id_Prov_1,
                                             CantAsig = Cot.Cant_Asig_1,
                                             CostUnit = Cot.CostoUnit_1,
                                             Status = Cot.Status,
                                             FechaCrea = Cot.FechaCrea,
                                             FechaMod = Cot.FechaMod,
                                             Usu = Cot.Id_Usuario,
                                             Cuadro = Cot.Cuadro,
                                             IdReq = Cot.Id_Requicision,
                                             IdDetReq = DetOc.Id
                                         }).ToList();

                //RECORREMOS LAS COTIZACIONES PARA ACTUALIZARLAS
                foreach (var DetCotizacion in DetalleCotizacion)
                {
                    //Obtenemos el Detalle de la OC con el que se actualizará el item de la tbl Cotizaciones
                    var OC_Detalle = (from Oc in ConBD.SISM_ORDEN_COMPRA
                                      join Det_Oc in ConBD.SISM_DETALLE_OC on Oc.Id equals Det_Oc.Id_OrdenCompra
                                      where Oc.Id == Id_OC
                                      where Det_Oc.Id == DetCotizacion.IdDetReq
                                      select new
                                      {
                                          IdOc = Oc.Id,
                                          ClaveOC = Oc.Clave,
                                          IdReq = Oc.Id_Requisicion,
                                          IdProv = Oc.Id_Proveedor,
                                          FechaOC = Oc.Fecha,
                                          UsuIdOC = Oc.UsuarioId,
                                          IdDetR = Det_Oc.Id,
                                          IdCB = Det_Oc.Id_CodigoBarrar,
                                          Cantidad = Det_Oc.Cantidad,
                                          PU = Det_Oc.PreUnit,
                                          IdSus = Det_Oc.Id_Sustencia
                                      }).FirstOrDefault();

                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_COTIZACIONES SET Id_Prov_1 = '" + OC_Detalle.IdProv + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_COTIZACIONES SET Cant_Asig_1 = '" + OC_Detalle.Cantidad + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_COTIZACIONES SET CostoUnit_1 = '" + OC_Detalle.PU + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_COTIZACIONES SET Status = '" + true + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                }

                #endregion

                //------------------------------------------------------------------------ BASE DE DATOS NUEVA  -------------------------   FIN  ------


                ////-----------------------------------------------------------  BASE DE DATOS VIEJA  -------------------------   INICIO  ------
                #region

                //  Se crea la NuevaO.C en la BD VIEJA, en la tabla OrdenCompra, DetalleOC y se busca la Requi en la tbl Cotizaciones (vieja bd) y se actualizarán sus datos

                //Obtener la Requi de la BD VIEJITA
                var RequiVieja = (from a in RequisicionDB.Requisicion
                                  where a.clave == Folio_Requi.ToString()
                                  select a).OrderByDescending(u => u.id).FirstOrDefault();

                //Se crea una NUEVA O.C en la B.D VIEJA, en base a la O.C creada en la NUEVA B.D
                OrdenCompra NuevaOC_BDVieja = new OrdenCompra();
                NuevaOC_BDVieja.clave = AñoMes_Actual + ConsecutivoNuevoTxt;
                //NuevaOC_BDVieja.Id_Requisicion = OC.Id_Requisicion;
                NuevaOC_BDVieja.Id_Requisicion = RequiVieja.id;
                NuevaOC_BDVieja.Id_Proveedor = (int)OC.Id_Proveedor;
                NuevaOC_BDVieja.Fecha = OC.Fecha_HacerOC;
                NuevaOC_BDVieja.FecMod = OC.Fecha_HacerOC;
                NuevaOC_BDVieja.Forma_Pago = OC.Forma_Pago;
                NuevaOC_BDVieja.Folio = OC.Folio;
                NuevaOC_BDVieja.Status = true;
                NuevaOC_BDVieja.UsuarioId = OC.UsuarioId;
                NuevaOC_BDVieja.Cerrado = (bool)OC.Cerrado;
                NuevaOC_BDVieja.Cuadro = OC.Cuadro;
                RequisicionDB.OrdenCompra.Add(NuevaOC_BDVieja);
                RequisicionDB.SaveChanges();

                //Obtener la ultima OC en la BD VIEJA (que es esta OC)
                var OCvieja_Nueva = (from a in RequisicionDB.OrdenCompra
                                         //where a.Fecha == OC.Fecha
                                     where a.clave == NuevaOC_BDVieja.clave
                                     select a).OrderByDescending(u => u.Id).FirstOrDefault();

                //Se crea el nuevo DetalleOC en la BD VIEJA
                foreach (var NuevoDetalle in DetalleOC)
                {
                    DetalleOC NuevoDetalleOC_Vieja = new DetalleOC();
                    NuevoDetalleOC_Vieja.Id_OrdenCompra = OCvieja_Nueva.Id;
                    NuevoDetalleOC_Vieja.Id_CodigoBarras = (int)NuevoDetalle.Id_CodigoBarrar;
                    NuevoDetalleOC_Vieja.Cantidad = (int)NuevoDetalle.Cantidad;
                    NuevoDetalleOC_Vieja.PreUnit = (double)NuevoDetalle.PreUnit;
                    NuevoDetalleOC_Vieja.Obsequio = NuevoDetalle.Obsequio;
                    NuevoDetalleOC_Vieja.Status = true;
                    NuevoDetalleOC_Vieja.Id_Sustancia = NuevoDetalle.Id_Sustencia;
                    NuevoDetalleOC_Vieja.Pendiente = NuevoDetalle.Pendiente;
                    RequisicionDB.DetalleOC.Add(NuevoDetalleOC_Vieja);
                    RequisicionDB.SaveChanges();
                }

                //Obtener las Cotizaciones que se actualizarán de la tbl VIEJA
                var CotizacionesViejas = (from Cot in RequisicionDB.Cotizaciones
                                          join Oc in RequisicionDB.OrdenCompra on Cot.Id_Requisicion equals Oc.Id_Requisicion
                                          join DetOc in RequisicionDB.DetalleOC on Oc.Id equals DetOc.Id_OrdenCompra
                                          where Oc.Id == OCvieja_Nueva.Id
                                          where DetOc.Id_Sustancia == Cot.Id_Sustancia
                                          select new
                                          {
                                              IdCot = Cot.Id,
                                              IdSus = Cot.Id_Sustancia,
                                              IdProv = Cot.Id_Prov_1,
                                              CantAsig = Cot.Cant_Asig_1,
                                              CostUnit = Cot.CostoUnit_1,
                                              Status = Cot.Status,
                                              FechaCrea = Cot.FechaCrea,
                                              FechaMod = Cot.FechaMod,
                                              Usu = Cot.Id_Usuario,
                                              Cuadro = Cot.Cuadro,
                                              IdReq = Cot.Id_Requisicion,
                                              IdDetReq = DetOc.Id
                                          }).ToList();

                //Se actualiza la tbl Vieja Cotizaciones con los datos de la Nueva OC
                foreach (var CotVieja in CotizacionesViejas)
                {
                    //Se obtiene el DetalleOC de la BD VIEJA con el que se actualizará su registro correspondiente en tbl Cotizaciones Viejas
                    var OCDetalle_Nueva = (from Oc in RequisicionDB.OrdenCompra
                                           join Det_Oc in RequisicionDB.DetalleOC on Oc.Id equals Det_Oc.Id_OrdenCompra
                                           where Oc.Id == OCvieja_Nueva.Id
                                           where Det_Oc.Id == CotVieja.IdDetReq
                                           select new
                                           {
                                               IdOc = Oc.Id,
                                               ClaveOC = Oc.clave,
                                               IdReq = Oc.Id_Requisicion,
                                               IdProv = Oc.Id_Proveedor,
                                               FechaOC = Oc.Fecha,
                                               UsuIdOC = Oc.UsuarioId,
                                               IdDetR = Det_Oc.Id,
                                               IdCB = Det_Oc.Id_CodigoBarras,
                                               Cantidad = Det_Oc.Cantidad,
                                               PU = Det_Oc.PreUnit,
                                               IdSus = Det_Oc.Id_Sustancia
                                           }).FirstOrDefault();

                    RequisicionDB.Database.ExecuteSqlCommand("UPDATE Cotizaciones SET Id_Prov_1 = '" + OCDetalle_Nueva.IdProv + "' WHERE Id_Sustancia='" + OCDetalle_Nueva.IdSus + "';");
                    RequisicionDB.Database.ExecuteSqlCommand("UPDATE Cotizaciones SET Cant_Asig_1 = '" + OCDetalle_Nueva.Cantidad + "' WHERE Id_Sustancia='" + OCDetalle_Nueva.IdSus + "';");
                    RequisicionDB.Database.ExecuteSqlCommand("UPDATE Cotizaciones SET CostoUnit_1 = '" + OCDetalle_Nueva.PU + "' WHERE Id_Sustancia='" + OCDetalle_Nueva.IdSus + "';");
                    RequisicionDB.Database.ExecuteSqlCommand("UPDATE Cotizaciones SET Status = '" + true + "' WHERE Id_Sustancia='" + OCDetalle_Nueva.IdSus + "';");
                }
                #endregion
                ////-----------------------------------------------------------  BASE DE DATOS VIEJA  -------------------------   FIN  ------

                return Json(new { MENSAJE = "Succe: Se generó la O.C" }, JsonRequestBehavior.AllowGet);
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
                //Obtener OC y su Detalle
                var OC = (from a in ConBD.SISM_ORDEN_COMPRA
                          join DetOC in ConBD.SISM_DETALLE_OC on a.Id equals DetOC.Id_OrdenCompra
                          where a.Id == Id_OC
                          select new
                          {
                              Id_OC = a.Id,
                              Total_OC = a.Total_OC,
                              IdDet_OC = DetOC.Id,
                              Sustancia_OC = DetOC.Id_Sustencia,
                              SubTotal_OC = DetOC.Total,
                              Id_Requi = a.Id_Requisicion,
                              FolioOC = a.Clave,
                              FechaOC = a.Fecha
                          }).FirstOrDefault();

                //Obtener REQUI y su Detalle
                var REQUI = (from Requi in ConBD.SISM_REQUISICION
                             join DetRequi in ConBD.SISM_DET_REQUISICION on Requi.Id_Requicision equals DetRequi.Id_Requicision
                             where OC.Id_Requi == Requi.Id_Requicision
                             select new
                             {
                                 Id_Requi = Requi.Id_Requicision,
                                 Estatus_Requi = Requi.Estatus_OC_Parcial,
                                 IdDet_Requi = DetRequi.Id_Detalle_Req,
                                 Sustancia_DetReq = DetRequi.Id_Sustancia,
                                 CantidadOC_DetReq = DetRequi.Cantidad_OC,
                                 CanPendienteOC_DetReq = DetRequi.CantidadPendiente_OC,
                                 PartidaPteOC_DetReq = DetRequi.PartidaPendiente_OC,
                                 Cantidad = DetRequi.Cantidad
                             }).ToList();

                //  "VACIAR" Partidas de la tbl DetalleRequisicion que Existen en la OC que se eliminará
                foreach (var item in REQUI)
                {
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET PrecioUnitario = '" + 0 + "' WHERE Id_Detalle_Req='" + item.IdDet_Requi + "';");
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Total = '" + 0 + "' WHERE Id_Detalle_Req='" + item.IdDet_Requi + "';");
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Cantidad_OC = '" + null + "' WHERE Id_Detalle_Req='" + item.IdDet_Requi + "';");
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET CantidadPendiente_OC = '" + item.Cantidad + "' WHERE Id_Detalle_Req='" + item.IdDet_Requi + "';");
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET PartidaPendiente_OC = '" + false + "' WHERE Id_Detalle_Req='" + item.IdDet_Requi + "';");
                }
                //Poner el "Estatus_OC_Parcial = Parcial" 
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET Estatus_OC_Parcial = '" + "Parcial" + "' WHERE Id_Requicision='" + OC.Id_Requi + "';");

                //  Obtener Cotizaciones que Existen de la OC que se eliminará en BD NUEVA
                var DetalleCotizacion = (from Cot in ConBD.SISM_COTIZACIONES
                                         join Oc in ConBD.SISM_ORDEN_COMPRA on Cot.Id_Requicision equals Oc.Id_Requisicion
                                         join DetOc in ConBD.SISM_DETALLE_OC on Oc.Id equals DetOc.Id_OrdenCompra
                                         where Oc.Id == Id_OC
                                         where DetOc.Id_Sustencia == Cot.Id_Sustancia
                                         select new
                                         {
                                             IdCot = Cot.Id,
                                             IdSus = Cot.Id_Sustancia,
                                             IdProv = Cot.Id_Prov_1,
                                             CantAsig = Cot.Cant_Asig_1,
                                             CostUnit = Cot.CostoUnit_1,
                                             Status = Cot.Status,
                                             FechaCrea = Cot.FechaCrea,
                                             FechaMod = Cot.FechaMod,
                                             Usu = Cot.Id_Usuario,
                                             Cuadro = Cot.Cuadro,
                                             IdReq = Cot.Id_Requicision,
                                             IdDetReq = DetOc.Id
                                         }).ToList();

                //  "VACIAR" Partidas de la tbl Cotizaciones que Existen en la OC que se eliminará
                foreach (var item in DetalleCotizacion)
                {
                    //Obtenemos el Detalle de la OC 
                    var OC_Detalle = (from Oc in ConBD.SISM_ORDEN_COMPRA
                                      join Det_Oc in ConBD.SISM_DETALLE_OC on Oc.Id equals Det_Oc.Id_OrdenCompra
                                      where Oc.Id == Id_OC
                                      where Det_Oc.Id == item.IdDetReq
                                      select new
                                      {
                                          IdOc = Oc.Id,
                                          ClaveOC = Oc.Clave,
                                          IdReq = Oc.Id_Requisicion,
                                          IdProv = Oc.Id_Proveedor,
                                          FechaOC = Oc.Fecha,
                                          UsuIdOC = Oc.UsuarioId,
                                          IdDetR = Det_Oc.Id,
                                          IdCB = Det_Oc.Id_CodigoBarrar,
                                          Cantidad = Det_Oc.Cantidad,
                                          PU = Det_Oc.PreUnit,
                                          IdSus = Det_Oc.Id_Sustencia
                                      }).FirstOrDefault();

                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_COTIZACIONES SET Id_Prov_1 = '" + 0 + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_COTIZACIONES SET Cant_Asig_1 = '" + 0 + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_COTIZACIONES SET CostoUnit_1 = '" + 0 + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                    ConBD.Database.ExecuteSqlCommand("UPDATE SISM_COTIZACIONES SET Status = '" + false + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                }

                //ELIMINAR OC y su DETALLE
                ConBD.Database.ExecuteSqlCommand("DELETE FROM SISM_DETALLE_OC WHERE Id_OrdenCompra= '" + OC.Id_OC + "';");
                ConBD.Database.ExecuteSqlCommand("DELETE FROM SISM_ORDEN_COMPRA WHERE Id= '" + OC.Id_OC + "';");

                //--------------------------------------------------------------------------  BASE DE DATOS VIEJA  -------------ELIMINAR O.C------------   INICIO  ------

                var OC_VIEJA = (from OC_V in RequisicionDB.OrdenCompra
                                where OC_V.clave == OC.FolioOC
                                select OC_V).FirstOrDefault();

                //  Obtener Cotizaciones que Existen de la OC que se eliminará de la BD VIEJA
                var DetalleCotizacion_BDvieja = (from Cot in RequisicionDB.Cotizaciones
                                                 join Oc in RequisicionDB.OrdenCompra on Cot.Id_Requisicion equals Oc.Id_Requisicion
                                                 join DetOc in RequisicionDB.DetalleOC on Oc.Id equals DetOc.Id_OrdenCompra
                                                 where Oc.Id == OC_VIEJA.Id
                                                 where DetOc.Id_Sustancia == Cot.Id_Sustancia
                                                 select new
                                                 {
                                                     IdCot = Cot.Id,
                                                     IdSus = Cot.Id_Sustancia,
                                                     IdProv = Cot.Id_Prov_1,
                                                     CantAsig = Cot.Cant_Asig_1,
                                                     CostUnit = Cot.CostoUnit_1,
                                                     Status = Cot.Status,
                                                     FechaCrea = Cot.FechaCrea,
                                                     FechaMod = Cot.FechaMod,
                                                     Usu = Cot.Id_Usuario,
                                                     Cuadro = Cot.Cuadro,
                                                     IdReq = Cot.Id_Requisicion,
                                                     IdDetReq = DetOc.Id
                                                 }).ToList();

                //Si la OC tiene Folio(clave) quiere decir que el Usuario de Compras si generó la OC despues de haberla autorizado
                //por lo tanto si existe la OC en la BD VIEJA, entonces entraríamos para eliminarla tambien de la BDVIEJA
                //si no, entonces no entramos al IF y solo se eliminará de la BNUEVA ya que ahí se guarda la Pre-Orden y la Orden
                if (OC.FolioOC == null || OC.FolioOC == "")
                {
                }
                else
                {//Eliminar OC en la BD Vieja: tbls Cotizaciones, DetalleOC y OC

                    //  "VACIAR" Partidas de la tbl Cotizaciones que Existen en la OC que se eliminará BD VIEJA
                    foreach (var item in DetalleCotizacion_BDvieja)
                    {
                        //Obtenemos el Detalle de la OC 
                        var OC_Detalle = (from Oc in RequisicionDB.OrdenCompra
                                          join Det_Oc in RequisicionDB.DetalleOC on Oc.Id equals Det_Oc.Id_OrdenCompra
                                          where Oc.Id == OC_VIEJA.Id
                                          where Det_Oc.Id == item.IdDetReq
                                          select new
                                          {
                                              IdOc = Oc.Id,
                                              ClaveOC = Oc.clave,
                                              IdReq = Oc.Id_Requisicion,
                                              IdProv = Oc.Id_Proveedor,
                                              FechaOC = Oc.Fecha,
                                              UsuIdOC = Oc.UsuarioId,
                                              IdDetR = Det_Oc.Id,
                                              IdCB = Det_Oc.Id_CodigoBarras,
                                              Cantidad = Det_Oc.Cantidad,
                                              PU = Det_Oc.PreUnit,
                                              IdSus = Det_Oc.Id_Sustancia
                                          }).FirstOrDefault();

                        RequisicionDB.Database.ExecuteSqlCommand("UPDATE Cotizaciones SET Id_Prov_1 = '" + 0 + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                        RequisicionDB.Database.ExecuteSqlCommand("UPDATE Cotizaciones SET Cant_Asig_1 = '" + 0 + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                        RequisicionDB.Database.ExecuteSqlCommand("UPDATE Cotizaciones SET CostoUnit_1 = '" + 0 + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                        RequisicionDB.Database.ExecuteSqlCommand("UPDATE Cotizaciones SET Status = '" + false + "' WHERE Id_Sustancia='" + OC_Detalle.IdSus + "';");
                    }
                }

                //ELIMINAR OC y su DETALLE de la BD VIEJA
                RequisicionDB.Database.ExecuteSqlCommand("DELETE FROM DetalleOC WHERE Id_OrdenCompra= '" + OC_VIEJA.Id + "';");
                RequisicionDB.Database.ExecuteSqlCommand("DELETE FROM OrdenCompra WHERE Id= '" + OC_VIEJA.Id + "';");

                //--------------------------------------------------------------------------  BASE DE DATOS VIEJA  --------------ELIMINAR O.C-----------   FIN  ------

                return Json(new { MENSAJE = "Succe: Se eliminó la O.C" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GuardarFecha_Acuse(int Id_OC, string FechaAcuse)
        {
            try
            {
                var fechaDT = DateTime.Parse(FechaAcuse);

                var OC = (from a in ConBD.SISM_ORDEN_COMPRA
                          where a.Id == Id_OC
                          select a).FirstOrDefault();

                OC.Fecha_Acuse = fechaDT;
                ConBD.SaveChanges();

                return Json(new { MENSAJE = "Succe: Se guardó la fecha" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EliminarPartidaOC(int Id_OC, int Id_DetOc)
        {
            try
            {
                //Obtener OC y su Detalle de la Partida que se eliminará
                var OC = (from a in ConBD.SISM_ORDEN_COMPRA
                          join DetOC in ConBD.SISM_DETALLE_OC on a.Id equals DetOC.Id_OrdenCompra
                          where a.Id == Id_OC
                          where DetOC.Id == Id_DetOc
                          select new
                          {
                              Id_OC = a.Id,
                              Total_OC = a.Total_OC,
                              IdDet_OC = DetOC.Id,
                              Sustancia_OC = DetOC.Id_Sustencia,
                              SubTotal_OC = DetOC.Total,
                          }).FirstOrDefault();

                //Obtener REQUI y su Detalle de la Partida que se eliminará
                var REQUI = (from Requi in ConBD.SISM_REQUISICION
                             join DetRequi in ConBD.SISM_DET_REQUISICION on Requi.Id_Requicision equals DetRequi.Id_Requicision
                             where OC.Sustancia_OC == DetRequi.Id_Sustancia
                             select new
                             {
                                 Id_Requi = Requi.Id_Requicision,
                                 Estatus_Requi = Requi.Estatus_OC_Parcial,
                                 IdDet_Requi = DetRequi.Id_Detalle_Req,
                                 Sustancia_DetReq = DetRequi.Id_Sustancia,
                                 CantidadOC_DetReq = DetRequi.Cantidad_OC,
                                 CanPendienteOC_DetReq = DetRequi.CantidadPendiente_OC,
                                 PartidaPteOC_DetReq = DetRequi.PartidaPendiente_OC,
                                 Cantidad = DetRequi.Cantidad
                             }).FirstOrDefault();

                //  "VACIAR" Partida de la tbl DetalleRequisicion
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET PrecioUnitario = '" + 0 + "' WHERE Id_Detalle_Req='" + REQUI.IdDet_Requi + "';");
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Total = '" + 0 + "' WHERE Id_Detalle_Req='" + REQUI.IdDet_Requi + "';");
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET Cantidad_OC = '" + null + "' WHERE Id_Detalle_Req='" + REQUI.IdDet_Requi + "';");
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET CantidadPendiente_OC = '" + REQUI.Cantidad + "' WHERE Id_Detalle_Req='" + REQUI.IdDet_Requi + "';");
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_DET_REQUISICION SET PartidaPendiente_OC = '" + false + "' WHERE Id_Detalle_Req='" + REQUI.IdDet_Requi + "';");
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET Estatus_OC_Parcial = '" + "Parcial" + "' WHERE Id_Requicision='" + REQUI.Id_Requi + "';");

                //ELIMINAR Partida de la tbl DetalleOrdenCompra
                ConBD.Database.ExecuteSqlCommand("DELETE FROM SISM_DETALLE_OC WHERE Id= '" + Id_DetOc + "';");

                //Volver a hacer el RECALCULO del GranTotal de la OrdenCompra
                var NuevoGranTotal_OC = (double?)decimal.Round((decimal)(OC.Total_OC - OC.SubTotal_OC), 2);
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_ORDEN_COMPRA SET Total_OC = '" + NuevoGranTotal_OC + "' WHERE Id='" + Id_OC + "';");

                return Json(new { MENSAJE = "Succe: Se eliminó la Partida de la O.C" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //-------------------------------------------------------------------- Pantalla ORDENES COMPRA   --------------  FIN

        //----------------------------------------------------------------------------------- Pantalla ORDENES COMPRA POR VALIDAR   --------------  INICIO

        [Authorize]
        public ActionResult OrdenCompraPorValidar()
        {
            return View();
        }

        public ActionResult ObtenerOC_PorValidar()
        {
            try
            {
                var query = (from a in ConBD.SISM_ORDEN_COMPRA
                             join req in ConBD.SISM_REQUISICION on a.Id_Requisicion equals req.Id_Requicision
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
                                 Desc = a.Descripcion,
                                 a.Fecha_HacerOC
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
                        DescripcionOC = q.Desc,
                        FechaRequisicion = string.Format("{0:yyyy/M/d hh:mm tt}", q.Fecha, new CultureInfo("es-ES")),
                        Fecha_OC = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha_HacerOC),
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

        public JsonResult ValidarOC(int Id_OC, string DescripcionOC)
        {
            try
            {
                var UsuarioRegistra = User.Identity.GetUserName();
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                var fechaDT = DateTime.Parse(fecha);

                var OC = (from a in ConBD.SISM_ORDEN_COMPRA
                          where a.Id == Id_OC
                          select a).FirstOrDefault();

                OC.OC_PorValidar = "2";// 2 Quiere decir que se Validó la O.C porque la OC nace como 1 (Generada) al validarla (2) el usuario de Compras puede Aprobarla/Generarla y el Status en BD cambia a True y OC_PorValidar puede cambiar a 3
                OC.Descripcion = DescripcionOC;
                OC.Fecha_AutorizaOC = fechaDT;
                OC.Usuario_AutorizaOC = UsuarioRegistra;
                ConBD.SaveChanges();

                return Json(new { MENSAJE = "Succe: Se autorizó la Pre-Orden de Compra" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CancelarOC(int Id_OC, string DescripcionOC)
        {
            try
            {
                var OC = (from a in ConBD.SISM_ORDEN_COMPRA
                          where a.Id == Id_OC
                          select a).FirstOrDefault();

                OC.Status = false;
                OC.OC_PorValidar = "4";// 4 Quiere decir que se Canceló la O.C 
                OC.Descripcion = DescripcionOC;
                ConBD.SaveChanges();

                return Json(new { MENSAJE = "Succe: Se canceló la O.C" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //----------------------------------------------------------------------------------- Pantalla ORDENES COMPRA POR VALIDAR   --------------  FIN

        #region PARCIALES VIEJAS

        [Authorize]
        public ActionResult OrdenCompraParcial()
        {
            ViewBag.PROVEEDORES = new SelectList(db.Proveedor.ToList(), "Id", "Prov_Nombre");

            return View();
        }

        //LISTADO DE LAS ORDENES DE COMPRA **PARCIALES**    PANTALLA INICIO
        //public ActionResult ObtenerOCInicio_Parciales()
        //{
        //    try
        //    {
        //        var query = (from a in ConBD.SISM_ORDEN_COMPRA
        //                     join req in ConBD.SISM_REQUISICION on a.Id_Requisicion equals req.Id_Requicision
        //                     where a.Estatus_OC == "OC Parcial"
        //                     select new
        //                     {
        //                         a.Id,
        //                         a.Clave,
        //                         a.Fecha,
        //                         a.UsuarioNuevo
        //                     }).ToList();

        //        var results1 = new List<ListCampos>();

        //        foreach (var q in query)
        //        {
        //            var resultado = new ListCampos
        //            {
        //                Id_OC = q.Id,
        //                Clave = q.Clave,
        //                Fecha = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha),
        //                Id_User = q.UsuarioNuevo
        //            };
        //            results1.Add(resultado);
        //        }
        //        return Json(new { MENSAJE = "FOUND", OCS = results1 }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult ObtenerDetalleParcial(string Id_OC)
        //{
        //    try
        //    {
        //        var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        //        var fechaDT = DateTime.Parse(fecha);

        //        var query = (from a in ConBD.SISM_ORDEN_COMPRA
        //                     join DetOC in ConBD.SISM_DETALLE_OC on a.Id equals DetOC.Id_OrdenCompra
        //                     join Requi in ConBD.SISM_REQUISICION on a.Id_Requisicion equals Requi.Id_Requicision
        //                     where a.Clave == Id_OC
        //                     where DetOC.ItemPendiente == true
        //                     select new
        //                     {
        //                         Folio =        a.Clave,
        //                         Fecha =        a.Fecha,
        //                         Usuario =      a.UsuarioNuevo,
        //                         Cantidad =     DetOC.CantidadPendiente,
        //                         Precio =       DetOC.PreUnit,
        //                         Descripcion =  DetOC.Descripcion,
        //                         ClaveMed =     DetOC.ClaveMedicamento,
        //                         PU =           DetOC.PreUnit,
        //                         Total =        DetOC.Total,
        //                         NombreProveedor = a.NombreProveedor,
        //                         a.             Total_OC,
        //                         Pendiente =    DetOC.ItemPendiente,

        //                         Requi =        a.Id_Requisicion,
        //                         DescO =        a.Descripcion,
        //                         CBarra =       DetOC.Id_CodigoBarrar,
        //                         Can =          DetOC.Cantidad,
        //                         Sustancia =    DetOC.Id_Sustencia,
        //                         ClaveReq =     Requi.claveOLD
        //                     }).ToList();

        //        var results1 = new List<ListCampos>();
        //        //LISTA DE LA O.C Y SU DETALLE
        //        foreach (var q in query)
        //        {
        //            var resultado = new ListCampos
        //            {
        //                Folio = q.Folio,
        //                Fecha = string.Format("{0:d/M/yy hh:mm tt}", q.Fecha),
        //                Fecha1 = string.Format("{0:d/M/yy hh:mm tt}", fechaDT),
        //                Usuario = q.Usuario,
        //                Descripcion = q.Descripcion,
        //                Clave = q.ClaveMed,
        //                Cantidad = (int)q.Cantidad,
        //                PrecioUnitario = (double)q.PU,
        //                Total = (double)q.Total,
        //                NombreProveedor = q.NombreProveedor,
        //                Total_OC = (double)q.Total_OC,

        //                Id_Requicision = (int)q.Requi,
        //                DescO = q.DescO,
        //                CBarra = q.CBarra,
        //                Can = q.Can,
        //                Id_Sustancia = (int)q.Sustancia,
        //                FolioRequisicion = q.ClaveReq
        //            };
        //            results1.Add(resultado);
        //        }

        //        return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public JsonResult GenerarOC_Parcial(List<SISM_DET_REQUISICION> ListaOC, string FolioOC/*, string ProveedorReq*/, int FolioRequi)
        //{
        //    var UsuarioRegistra = User.Identity.GetUserName();
        //    var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        //    var fechaDT = DateTime.Parse(fecha);
        //    var ip_realiza = Request.UserHostAddress;
        //    var IdUsuarioCifrado = User.Identity.GetUserId();
        //    //return null;
        //    try
        //    {
        //        //Obtenemos la OC "madre" (la que es Parcial) para poner su Id en la Nueva OC que se creará a partir de la OC Parcial
        //        var OC_Anterior = (from a in ConBD.SISM_ORDEN_COMPRA
        //                    where a.Clave == FolioOC
        //                  select a).OrderByDescending(u => u.Id).FirstOrDefault();

        //        //PROVEEDOR de la Requi(Orden)
        //        //var Prov = (from a in db.Proveedor
        //        //            where a.Prov_Nombre == ProveedorReq
        //        //            select a
        //        //            ).FirstOrDefault();
        //        //REQUI que se hará Orden
        //        var Requi = (from a in ConBD.SISM_REQUISICION
        //                     where a.claveOLD == FolioRequi.ToString()
        //                     select a
        //                    ).FirstOrDefault();

        //        var UsuarioOld = User.Identity.GetUserName();
        //        var UsuarioOLD = (from a in db.Usuario
        //                          where a.Usu_User == UsuarioOld
        //                          select a).FirstOrDefault();

        //        #region TODA LA ACCION
        //        //CREAR ORDEN NUEVA a partir de una Requi
        //        SISM_ORDEN_COMPRA OC = new SISM_ORDEN_COMPRA();
        //        //OC.Clave = CLAVE.ToString();
        //        OC.Id_Requisicion = Requi.Id_Requicision;
        //        //OC.Id_Proveedor = Prov.Id;
        //        OC.Fecha = fechaDT;
        //        OC.FechaMod = fechaDT;
        //        OC.Forma_Pago = "";
        //        OC.Folio = "";
        //        OC.Status = false;
        //        OC.UsuarioId = UsuarioOLD.UsuarioId;
        //        OC.Cerrado = true;
        //        OC.Cuadro = 1;
        //        OC.UsuarioNuevo = UsuarioRegistra;
        //        OC.IP_User = ip_realiza;
        //        //OC.NombreProveedor = Prov.Prov_Nombre;
        //        OC.OC_PorValidar = "1"; //LA OC nace en 1, tiene que validarse para pasar a 2 (validada) despues tiene que Generarse la OC y pasa a 3, el Status cambiará a True
        //        OC.EsParcial_IdOC = OC_Anterior.Id;
        //        ConBD.SISM_ORDEN_COMPRA.Add(OC);
        //        ConBD.SaveChanges();
        //        //ACTUALIZAMOS LA REQUI EN SU COLUMNA 'EstatusOC' poniendo 1 ya que esa requi se hará O.C
        //        //ConBD.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET EstatusOC = '1' WHERE Id_Requicision='" + Requi.Id_Requicision + "';");

        //        //obtener la ultima 'clave' de la tabla OrdenCompra actualBD vieja) para que inserte un nuevo registro en la nueva BD CONSECUTIVO de la clave
        //        var Clave = (from a in db.OrdenCompra
        //                     select new
        //                     {
        //                         clave = a.clave
        //                     }).OrderByDescending(u => u.clave).FirstOrDefault();
        //        var AñoMes_Actual = string.Format("{0:yyMM}", fechaDT);
        //        //var UltimoConsecutivo_Clave = Convert.ToInt32(Clave.clave.Substring(4));
        //        var UltimoConsecutivo_Clave = Convert.ToInt32(OC_Anterior.Clave.Substring(4));
        //        var ConsecutivoNuevo = ((UltimoConsecutivo_Clave) + 1);
        //        var ConsecutivoNuevoTxt = "";
        //        if (ConsecutivoNuevo < 100)
        //        {
        //            if (ConsecutivoNuevo < 9)
        //            {
        //                ConsecutivoNuevoTxt = "00" + ConsecutivoNuevo;
        //            }
        //            else
        //            {
        //                ConsecutivoNuevoTxt = "0" + ConsecutivoNuevo;
        //            }
        //        }
        //        else
        //        {
        //            ConsecutivoNuevoTxt = "" + ConsecutivoNuevo;
        //        }
        //        //Obtenemos la ultima O.C guardada(que es esta) para guardar su detalle
        //        var IdOC = (from a in ConBD.SISM_ORDEN_COMPRA
        //                    where a.UsuarioNuevo == UsuarioRegistra
        //                    where a.Fecha == fechaDT
        //                    select a).OrderByDescending(u => u.Id).FirstOrDefault();
        //        //ACTUALIZAMOS LA CLAVE DE LA O'C 
        //        ConBD.Database.ExecuteSqlCommand("UPDATE SISM_ORDEN_COMPRA SET Clave = '" + AñoMes_Actual + ConsecutivoNuevoTxt + "' WHERE Id='" + IdOC.Id + "';");
        //        //RECORREMOS  para guardar en la tabla DETALLE ORDEN
        //        foreach (var item in ListaOC)
        //        {
        //            //CREAR EL DETALLE DE LA NUEVA ORDEN
        //            SISM_DETALLE_OC DetalleOC = new SISM_DETALLE_OC();
        //            //BUSCAMOS EN LA TABLA "CodigoBarras" el Id del Codigo de Barras, buscando por el Id de la Sustancia
        //            var CodigoBarras = (from a in SISMFarmacia.CodigoBarras
        //                                where a.Id_Sustancia == item.Id_Sustancia
        //                                select a).FirstOrDefault();
        //            //Obtenemos la info de SUSTANCIA de la tabla Detalle_Requi
        //            var Sustancia = (from a in ConBD.SISM_DET_REQUISICION
        //                             where a.Clave == item.Clave
        //                             select a).FirstOrDefault();
        //            DetalleOC.Id_OrdenCompra = IdOC.Id;
        //            DetalleOC.Id_CodigoBarrar = CodigoBarras.Id;
        //            DetalleOC.Obsequio = 0;
        //            DetalleOC.Status = false;
        //            DetalleOC.Id_Sustencia = item.Id_Sustancia;
        //            DetalleOC.Descripcion = Sustancia.Descripcion;
        //            DetalleOC.ClaveMedicamento = Sustancia.Clave;
        //            //1 quiere decir que el item está pendiente, 0 quiere decir que si se surtió el item
        //            DetalleOC.ItemPendiente = item.CB_ELIMINAR;

        //            //En DetalleOC.Cantidad se guarda la Cantidad de pzas que pide Almacen siempre
        //            DetalleOC.Cantidad = item.Cantidad;

        //            //if (item.CANTIDAD_NUEVA > 0)
        //            //{
        //            //    //Aqui se guardan las pzas que modificó Compras 
        //            //    DetalleOC.CantidadOC = item.CANTIDAD_NUEVA;
        //            //    //Se guarda la cantidad de items total (al modificarse la cantidad esa nueva cantidad pasa a ser la oficial)
        //            //    DetalleOC.CantidadItema_OC = item.CANTIDAD_NUEVA;

        //            //    if (item.CANTIDAD_NUEVA < item.Cantidad)
        //            //    {
        //            //        DetalleOC.ItemPendiente = true;
        //            //        //OC.Estatus_OC = "OC Parcial";
        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    //Si no se modificó la CANTIDAD_NUEVA, la cantidad por default pasa a ser la oficial
        //            //    DetalleOC.CantidadItema_OC = item.Cantidad;
        //            //}

        //            if (item.CANTIDAD_NUEVA > 0)
        //            {
        //                if (item.CANTIDAD_NUEVA < item.Cantidad)
        //                {
        //                    DetalleOC.ItemPendiente = true;
        //                    DetalleOC.CantidadPendiente = item.Cantidad - item.CANTIDAD_NUEVA;
        //                    DetalleOC.CantidadOC = item.CANTIDAD_NUEVA;
        //                }
        //            }
        //            else
        //            {
        //                DetalleOC.CantidadOC = item.Cantidad;
        //            }


        //            //Se valida si el PRECIO UNITARIO se modificó
        //            if (item.PREUNIT_NUEVA > 0)
        //            {
        //                DetalleOC.PreUnit = item.PREUNIT_NUEVA;
        //            }
        //            else
        //            {
        //                DetalleOC.PreUnit = item.PrecioUnitario;
        //            }
        //            //Se valida si se ingresó una NUEVA CANTIDAD o un NUEVO PRECIO UNITARIO     $$TOTAL$$ 
        //            if (item.CANTIDAD_NUEVA > 0 || item.PREUNIT_NUEVA > 0)
        //            {
        //                if (item.CANTIDAD_NUEVA > 0 && item.PREUNIT_NUEVA > 0)
        //                {
        //                    DetalleOC.Total = (double?)decimal.Round((decimal)(item.CANTIDAD_NUEVA * item.PREUNIT_NUEVA), 2);
        //                }
        //                else
        //                {
        //                    if (item.CANTIDAD_NUEVA > 0)
        //                    {
        //                        DetalleOC.Total = (double?)decimal.Round((decimal)(item.CANTIDAD_NUEVA * item.PrecioUnitario), 2);
        //                    }
        //                    if (item.PREUNIT_NUEVA > 0)
        //                    {
        //                        DetalleOC.Total = (double?)decimal.Round((decimal)(item.Cantidad * item.PREUNIT_NUEVA), 2);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                DetalleOC.Total = (double?)decimal.Round((decimal)(DetalleOC.Cantidad * DetalleOC.PreUnit), 2);
        //            }

        //            ConBD.SISM_DETALLE_OC.Add(DetalleOC);
        //            ConBD.SaveChanges();

        //            if (item.CB_ELIMINAR == false)
        //            {
        //                SubTotal_OC += Decimal.Round((decimal)(DetalleOC.Total), 2);
        //                OC.Total_OC = (double?)SubTotal_OC;
        //            }



        //            ConBD.SaveChanges();
        //        }

        //        //Buscar en los items(medicamentos) del Detalle de la OC si alguno está pendiente (lo eliminaron a la hora de genera las OC)
        //        var ItemPendiente_DetalleOC = (from a in ConBD.SISM_DETALLE_OC
        //                                       where a.ItemPendiente == true
        //                                       select a
        //                            ).ToList();
        //        //Poner la OC como 'Parcial' ya que tiene items pendientes
        //        //if (ItemPendiente_DetalleOC.Count > 0)
        //        //{
        //        //    OC.Estatus_OC = "OC Parcial";
        //        //}
        //        //else
        //        //{
        //        //    OC.Estatus_OC = "OC Completa";
        //        //}
        //        ConBD.SaveChanges();


        //        //, INFO = FolioNuevo_Oc.Clave
        //        return Json(new { MENSAJE = "Succe: Se generó la O.C" }, JsonRequestBehavior.AllowGet);
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        #endregion

        #region ObtenerUltima_OC
        //public ActionResult ObtenerUltima_OC()
        //{
        //    try
        //    {
        //        var UsuarioRegistra = User.Identity.GetUserName();
        //        var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");
        //        var fechaDT = DateTime.Parse(fecha);

        //        var IdOC = (from a in ConBD.SISM_ORDEN_COMPRA
        //                    where a.UsuarioNuevo == UsuarioRegistra
        //                    where a.Fecha >= fechaDT
        //                    select a).OrderByDescending(u => u.Id).FirstOrDefault();

        //        db.SaveChanges();

        //        return Json(new { MENSAJE = "Succe: ", CLAVE = IdOC.Clave }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //    //return Respuesta;
        //}
        #endregion

    }
}
