using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;


namespace UanlSISM.Controllers
{
    public class RequisicionController : Controller
    {

        [Authorize]
        public ActionResult Requisicion()
        {
            return View();
        }

        public ActionResult Requisicion2()
        {
            return View();
        }

        [Authorize]
        public ActionResult BorradorRequisicion()
        {
            return View();
        }

        SERVMEDEntities5 db2 = new SERVMEDEntities5();

        public class LstInv
        {
            public int Id_Sustancia { get; set; }
            public int Cantidad { get; set; }
            public string Clave { get; set; }
            public string Descripcion { get; set; }
            public int CANTIDAD { get; set; }
            public string Compendio { get; set; }

        }

        public JsonResult BuscarSustancia(string Clave, int Id_FolioBorrador)
        {

            var SUSTANCIA = new List<Sustancia>();
            SUSTANCIA = db2.Sustancia.Where(s => s.Clave == Clave).ToList();

            //BUSCAR LOS DETALLES POR EL FOLIO QUE RECIBIRÉ COMO PARAMETRO
            var DETALLES = new List<SISM_DETALLE_BORRADOR_REQUI>();
            DETALLES = ConBD.SISM_DETALLE_BORRADOR_REQUI.Where(u => u.Id_BorradorRequi == Id_FolioBorrador).ToList();

            var medicamentos = new List<LstInv>();

            foreach (var item in SUSTANCIA)
            {
                var listamedicamentos = new LstInv
                {
                    Id_Sustancia = item.Id,
                    Cantidad = 0,
                    Clave = item.Clave,
                    Descripcion = item.descripcion_21,
                    Compendio = item.Compendio
                };

                medicamentos.Add(listamedicamentos);
            }

            //RECORREMOS LA LISTA DE LOS DETALLES
            foreach (var item in DETALLES)
            {
                var listamedicamentos = new LstInv
                {
                    Id_Sustancia = (int)item.Id_Sustancia,
                    Cantidad = (int)item.Cantidad,
                    Clave = item.Clave,
                    Descripcion = item.Descripcion,
                    Compendio = item.Compendio
                };

                medicamentos.Add(listamedicamentos);
            }

            return Json(new { SUSTANCIAS = medicamentos }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarSustancia2(string Clave)
        {

            var SUSTANCIA = new List<Sustancia>();
            SUSTANCIA = db2.Sustancia.Where(s => s.Clave == Clave).ToList();



            return Json(new { SUSTANCIAS = SUSTANCIA }, JsonRequestBehavior.AllowGet);
        }

        SERVMEDEntities8 db = new SERVMEDEntities8();
        //SERVMEDEntities12 SPM = new SERVMEDEntities12();
        

        public class LstInv1
        {
            public string Clave { get; set; }
            public string Descripcion { get; set; }
            public int Existencia { get; set; }
            public string Compendio { get; set; }
            public int Licitacion { get; set; }
        }

        Models.SERVMEDEntities4 DAM = new Models.SERVMEDEntities4();

        public ActionResult ObtenerSustanciasModal()
        {
            try
            {
                //var query = db.SP_ObtenerSustancias_Milton();
                string query = "SELECT Sus.Clave AS Clave, Sus.descripcion_21 AS Descripcion, Inv.ManejoDisponible AS Existencia, Sus.Compendio As Compendio, Sus.LicitacionStatus As Licitacion " +
                               "FROM Sustancia AS Sus " +
                               "INNER JOIN InvAlmFarm AS Inv ON Sus.Id = Inv.Id_Sustancia " +
                               "WHERE(Sus.descripcion_21 IS NOT NULL) " +
                               "AND(Sus.descripcion_21 <> '') " +
                               "AND(Inv.InvAlmId = 76) " +
                               "AND(Sus.LicitacionStatus IS NOT NULL) ";

                var result = DAM.Database.SqlQuery<LstInv1>(query);
                var res = result.ToList();

                return Json(new { MENSAJE = "FOUND", SUSTANCIAS = res }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        SISM_SIST_MEDEntities ConBD = new SISM_SIST_MEDEntities();
        public JsonResult GenerarBorradorRequi(List<Sustancia> ListaSustanciasBorrador, string StatusContrato)
        {
            //return null;
           
            var UsuarioRegistra = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;
            try
            {
                //int i = 0, j = 0;
                //i = i / j;
                //Creamos borrador de la Requi
                SISM_BORRADOR_REQUI BorradorRequi = new SISM_BORRADOR_REQUI();
                BorradorRequi.Fecha = fechaDT;
                BorradorRequi.UsuarioRegistra = UsuarioRegistra;
                BorradorRequi.Estatus = "Borrador Generado";
                BorradorRequi.ip_realiza = ip_realiza;
                BorradorRequi.EstatusContrato = StatusContrato;
                ConBD.SISM_BORRADOR_REQUI.Add(BorradorRequi);
                ConBD.SaveChanges();

                var IdBorrador = (from a in ConBD.SISM_BORRADOR_REQUI
                                  where a.UsuarioRegistra == UsuarioRegistra
                                  select a).OrderByDescending(u => u.Id_BorradorRequi).FirstOrDefault();

                foreach (var item in ListaSustanciasBorrador)
                {
                    SISM_DETALLE_BORRADOR_REQUI nuevoDetalle = new SISM_DETALLE_BORRADOR_REQUI();
                    nuevoDetalle.Cantidad = item.CANTIDAD;
                    nuevoDetalle.Clave = item.Clave;
                    nuevoDetalle.Id_Sustancia = item.Id;
                    nuevoDetalle.Id_BorradorRequi = IdBorrador.Id_BorradorRequi;
                    nuevoDetalle.Descripcion = item.descripcion_21;
                    nuevoDetalle.Compendio = item.Compendio;

                    ConBD.SISM_DETALLE_BORRADOR_REQUI.Add(nuevoDetalle);
                    ConBD.SaveChanges();
                }

                return Json(new { MENSAJE = "Succe: Se guardó con éxito el Borrador de Requisición" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerBorradores()
        {
            try
            {
                var query = (from a in ConBD.SISM_BORRADOR_REQUI
                             where a.Estatus == "Borrador Generado"
                             select new
                             {
                                 a.Id_BorradorRequi,
                                 a.Fecha,
                                 a.UsuarioRegistra,
                                 a.EstatusContrato
                             }).ToList();

                var results1 = new List<BorradorList>();

                foreach (var q in query)
                {
                    var resultado = new BorradorList
                    {
                        Id_BorradorRequi = q.Id_BorradorRequi,
                        Fecha = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha),
                        UsuarioRegistra = q.UsuarioRegistra,
                        EstatusContrato = q.EstatusContrato
                    };
                    results1.Add(resultado);
                }
                return Json(new { MENSAJE = "FOUND", BORRADORES = results1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public class BorradorList
        {
            public int Id_BorradorRequi { get; set; }
            public string Fecha { get; set; }
            public string Clave { get; set; }
            public string UsuarioRegistra { get; set; }
            public int Id_Requisicion { get; set; }
            public string FechaRequisicion { get; set; }
            public string Id_User { get; set; }
            public string EstatusContrato { get; set; }
        }

        public JsonResult LlenarDetalleBorrador(int Id_Borrador)
        {
            try
            {
                var query = (from a in ConBD.SISM_BORRADOR_REQUI
                             join det in ConBD.SISM_DETALLE_BORRADOR_REQUI on a.Id_BorradorRequi equals det.Id_BorradorRequi
                             where a.Id_BorradorRequi == Id_Borrador
                             select new
                             {
                                 Id_Sustancia = det.Id_Sustancia,
                                 Cantidad = det.Cantidad,
                                 Clave = det.Clave,
                                 Descripcion = det.Descripcion,
                                 det.Id_Detalle_BorradorRequi,
                                 Compendio = det.Compendio
                             }
                             ).ToList();

                return Json(new { MENSAJE = "FOUND", DETALLES = query }, JsonRequestBehavior.AllowGet);
                //return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GenerarRequi(List<SISM_DETALLE_BORRADOR_REQUI> ListaSustanciasBorrador, int Id_FolioBorrador)
        {
            var UsuarioRegistra = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;
            var ff = string.Format("{0:ddMMyy}", fechaDT);
            var IdUsuarioCifrado = User.Identity.GetUserId();

            try
            {
                //***GUARDAREMOS AHORA EL BORRADOR Y EL DETALLE BORRADOR COMO UNA NUEVA REQUISICION Y DETALLE REQUISICION***

                //Creamos la nueva requisición y comenzamos a guardar los datos
                SISM_REQUISICION Requisicion = new SISM_REQUISICION();
                Requisicion.Fecha = fechaDT;
                Requisicion.Id_User = UsuarioRegistra;
                Requisicion.IP_User = ip_realiza;

                //obtener la ultima 'clave' de tipo 2 de la tabla REQUISICONES (tabla actual) y se le agrega uno (1) para que inserte un nuevo registro (consecutivo de la clave)
                var ClaveID = (from a in RequisicionDB.Requisicion
                               where a.Id_Tipo == 2
                               select new
                               {
                                   a.clave
                               }).OrderByDescending(u => u.clave).FirstOrDefault();

                int ClaveNueva = Convert.ToInt32(ClaveID.clave) + 1;

                Requisicion.claveOLD = Convert.ToString(ClaveNueva);

                //Obtenemos el borrador que se convertirá a Requi para cambiarle su Estatus y que ya no aparezca en Borradores
                var Borrador = (from a in ConBD.SISM_BORRADOR_REQUI
                                where a.Id_BorradorRequi == Id_FolioBorrador
                                select a).FirstOrDefault();

                Requisicion.EstatusContrato = Borrador.EstatusContrato;
                Requisicion.EstatusOC = "0";
                ConBD.SISM_REQUISICION.Add(Requisicion);
                ConBD.SaveChanges();
                //-----------

                Borrador.Estatus = "Borrador a Requisicion";

                //Obtenemos al usuario que generó la Requi para despúes en la tabla Detalle obtener y guardar el Id de la Requi
                var IdRequisicion = (from a in ConBD.SISM_REQUISICION
                                  where a.Id_User == UsuarioRegistra
                                  select a).OrderByDescending(u => u.Id_Requicision).FirstOrDefault();

                //ACTUALIZAMOS LA CLAVE DE LA REQUI PARA CONCATENARLA LA NOMENCLATURA Y ASÍ GUARDAR EL FOLIO
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET Clave = 'RAC-" + ff + "-" + IdRequisicion.Id_Requicision + "' WHERE Id_Requicision='" + IdRequisicion.Id_Requicision + "';");

                foreach (var item in ListaSustanciasBorrador)
                {
                    SISM_DET_REQUISICION detalleRequisicion = new SISM_DET_REQUISICION();
                    detalleRequisicion.Id_Requicision = IdRequisicion.Id_Requicision;
                    detalleRequisicion.Id_Sustancia = (int)item.Id_Sustancia;

                    if (item.CANTIDAD_NUEVA > 0)
                    {
                        if (item.CANTIDAD_NUEVA > item.Cantidad || item.CANTIDAD_NUEVA == item.Cantidad)
                            detalleRequisicion.Cantidad = item.Cantidad + item.CANTIDAD_NUEVA;
                        
                        if(item.CANTIDAD_NUEVA < item.Cantidad)
                            detalleRequisicion.Cantidad = item.Cantidad - item.CANTIDAD_NUEVA;
                    }
                    else
                    {
                        detalleRequisicion.Cantidad = item.Cantidad;
                    }

                    detalleRequisicion.Clave = item.Clave;
                    detalleRequisicion.Descripcion = item.Descripcion;
                    detalleRequisicion.Compendio = item.Compendio;
                                        
                    ConBD.SISM_DET_REQUISICION.Add(detalleRequisicion);
                    ConBD.SaveChanges();
                }

                //Obtenemos el Usuario que se logueó para hacer el join (buscarlo) en la tabla Usuario y así obtener el Id de esa tabla
                var UsuarioOld = User.Identity.GetUserName();
                var UsuarioOLD = (from a in RequisicionDB.Usuario
                                  where a.Usu_User == UsuarioOld
                                  select a).FirstOrDefault();

                //Volvemos a recorrer la lista (detalle de la requi) ahora para guardar en la tabla Cotizaciones     
                foreach (var item in ListaSustanciasBorrador)
                {
                    SISM_COTIZACIONES Cotizacion = new SISM_COTIZACIONES();
                    Cotizacion.Id_Sustancia = (int)item.Id_Sustancia;
                    Cotizacion.Id_Requicision = IdRequisicion.Id_Requicision;
                    Cotizacion.Id_Prov_1 = 0;
                    Cotizacion.Cant_Asig_1 = 0;
                    Cotizacion.CostoUnit_1 = 0;
                    Cotizacion.Id_Prov_2 = 0;
                    Cotizacion.Cant_Asig_2 = 0;
                    Cotizacion.CostoUnit_2 = 0;
                    Cotizacion.Id_Prov_3 = 0;
                    Cotizacion.Cant_Asig_3 = 0;
                    Cotizacion.CostoUnit_3 = 0;
                    Cotizacion.Status = false;
                    Cotizacion.FechaCrea = fechaDT;
                    Cotizacion.FechaMod = fechaDT;
                    Cotizacion.Id_Usuario = UsuarioOLD.UsuarioId;
                    Cotizacion.Cuadro = 0;
                    Cotizacion.UserId = IdUsuarioCifrado;

                    ConBD.SISM_COTIZACIONES.Add(Cotizacion);
                    ConBD.SaveChanges();
                }


                //--------------------------------------------------------------------------------------------------------------------------------
                /*  GUARDAR LA REQUISICION Y SU DETALLE EN LAS TABLAS DE REQUISICION Y DETALLE REQUISICION  */
                //Requisicion_1 Req = new Requisicion_1();
                //Req.Id_Tipo = 2;
                //Req.Fecha = fechaDT;
                //Req.Status = true;
                //Req.cerrado = false;

                //Req.UserId = IdUsuarioCifrado;

                ////Obtenemos el Usuario que se logueó para hacer el join (buscarlo) en la tabla Usuario y así obtener el Id de esa tabla
                ////var UsuarioOld = User.Identity.GetUserName();
                ////var UsuarioOLD = (from a in RequisicionDB.Usuario
                ////                  where a.Usu_User == UsuarioOld
                ////                  select a).FirstOrDefault();

                ////Guardamos el Id del usuario de la tabla Usuario en el campo 'UsuarioId' en la tabla Requisiciones
                //if (UsuarioOLD == null)
                //{
                //}
                //else
                //{
                //    if (UsuarioOLD.Usu_User == null)
                //    {
                //    }
                //    else
                //    {
                //        Req.Id_Usuario = UsuarioOLD.UsuarioId;
                //    }
                //}

                //Req.clave = Convert.ToString(ClaveNueva);
                //Req.EstatusContrato = Borrador.EstatusContrato;

                //RequisicionDB.Requisicion.Add(Req);
                //RequisicionDB.SaveChanges();

                //var ID = (from a in RequisicionDB.Requisicion
                //          where a.Fecha == fechaDT
                //          where a.Id_Tipo == 2
                //          select a).OrderByDescending(u => u.id).FirstOrDefault();

                //foreach (var item in ListaSustanciasBorrador)
                //{
                //    DetalleReq detRequi = new DetalleReq();
                //    detRequi.Id_Requisicion = ID.id;
                //    detRequi.Id_Sustancia = (int)item.Id_Sustancia;
                //    detRequi.C_Recibida = 0;
                //    detRequi.Status = false;
                //    //detRequi.C_Solicitada = (int)item.Cantidad;

                //    if (item.CANTIDAD_NUEVA > 0)
                //    {
                //        if (item.CANTIDAD_NUEVA > item.Cantidad || item.CANTIDAD_NUEVA == item.Cantidad)
                //            detRequi.C_Solicitada = (int)item.Cantidad + item.CANTIDAD_NUEVA;

                //        if (item.CANTIDAD_NUEVA < item.Cantidad)
                //            detRequi.C_Solicitada = (int)item.Cantidad - item.CANTIDAD_NUEVA;
                //    }
                //    else
                //    {
                //        detRequi.C_Solicitada = (int)item.Cantidad;
                //    }

                //    RequisicionDB.DetalleReq.Add(detRequi);
                //    RequisicionDB.SaveChanges();
                //}

                //foreach (var item in ListaSustanciasBorrador)
                //{
                //    Cotizaciones Coti = new Cotizaciones();
                //    Coti.Id_Sustancia = (int)item.Id_Sustancia;
                //    Coti.Id_Requisicion = ID.id;
                //    Coti.Id_Prov_1 = 0;
                //    Coti.Cant_Asig_1 = 0;
                //    Coti.CostoUnit_1 = 0;
                //    Coti.Id_Prov_2 = 0;
                //    Coti.Cant_Asig_2 = 0;
                //    Coti.CostoUnit_2 = 0;
                //    Coti.Id_Prov_3 = 0;
                //    Coti.Cant_Asig_3 = 0;
                //    Coti.CostoUnit_3 = 0;
                //    Coti.Status = false;
                //    Coti.FechaCrea = ID.Fecha;
                //    Coti.FechaMod = ID.Fecha;
                //    Coti.Id_Usuario = ID.Id_Usuario;
                //    Coti.Cuadro = 0;
                //    Coti.UserId = IdUsuarioCifrado;

                //    RequisicionDB.Cotizaciones.Add(Coti);
                //    RequisicionDB.SaveChanges();
                //}
                //--------------------------------------------------------------------------------------------------------------------------------

                return Json(new { MENSAJE = "Succe: Se generó con éxito la Requisición" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult ObtenerRequisInicio()
        {
            try
            {
                var query = (from a in ConBD.SISM_REQUISICION
                             select a).ToList();

                var results1 = new List<BorradorList>();

                foreach (var q in query)
                {
                    var resultado = new BorradorList
                    {
                        //Id_Requisicion = q.Id_Requicision,
                        Clave = q.claveOLD,
                        Fecha = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha),
                        Id_User = q.Id_User,
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
                //string f = string.Format("{0:d/M/yyyy hh:mm tt}", fechaDT);
                //ViewData["FyH"] = f;

                var query = (from a in ConBD.SISM_REQUISICION
                             join det in ConBD.SISM_DET_REQUISICION on a.Id_Requicision equals det.Id_Requicision
                             //where a.Id_Requicision == Id_Requi
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
                                 det.Compendio
                             }).ToList();

                var results1 = new List<Detalle>();

                foreach (var q in query)
                {
                    
                    var sus = (from a in DAM.Sustancia
                                    where a.Clave == q.Clave
                                    select a).FirstOrDefault();
                    
                    if(sus != null)
                    {
                        string query2 = "select ManejoDisponible as ManejoDisponible from InvAlmFarm WHERE Id_Sustancia = " + sus.Id + " and InvAlmId = 76";
                        var result2 = db.Database.SqlQuery<InvAlmFarm>(query2);
                        var res2 = result2.FirstOrDefault();

                        var resultado = new Detalle
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
                            Compendio = q.Compendio
                        };
                        results1.Add(resultado);
                    }
                    
                }

                //return Json(new { MENSAJE = "FOUND", REQUIS = query }, JsonRequestBehavior.AllowGet);
                return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public class Detalle
        {
            public string Clave { get; set; }
            public string Descripcion { get; set; }
            public int Cantidad { get; set; }
            public string Folio { get; set; }
            public string Fecha { get; set; }
            public string Fecha1 { get; set; }
            public string Usuario { get; set; }
            public int? Existencia { get; set; }
            public string EstatusContrato { get; set; }
            public string Compendio { get; set; }
        }

        public JsonResult EliminarBorrador( int Id_FolioBorrador)
        {
            
            try
            {
                var Borrador = (from a in ConBD.SISM_BORRADOR_REQUI
                                where a.Id_BorradorRequi == Id_FolioBorrador
                                select a).FirstOrDefault();

                Borrador.Estatus = "Borrador Cancelado";

                ConBD.SaveChanges();
                
                return Json(new { MENSAJE = "Succe: Se eliminó el Borrador" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        SERVMEDEntities8 RequisicionDB = new SERVMEDEntities8();
        
        public JsonResult GenerarRequisiciondirecta(List<Sustancia> ListaSustanciasRequiDirecta, string StatusContrato)
        {
            var UsuarioRegistra = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;
            //var ff = string.Format("{0:d/M/yy}", fechaDT);
            var ff = string.Format("{0:ddMMyy}", fechaDT);

            var IdUsuarioCifrado = User.Identity.GetUserId();

            try
            {
                //obtener la ultima 'clave' de tipo 2 y se le agrega uno (1) para que inserte un nuevo registro (consecutivo de la clave)
                var ClaveID = (from a in RequisicionDB.Requisicion
                               where a.Id_Tipo == 2
                               select new
                               {
                                   a.clave
                               }).OrderByDescending(u => u.clave).FirstOrDefault();

                int ClaveNueva = Convert.ToInt32(ClaveID.clave) + 1;

                //Creamos una nueva Requi
                SISM_REQUISICION NuevaRequi = new SISM_REQUISICION();
                NuevaRequi.Fecha = fechaDT;
                NuevaRequi.Id_User = UsuarioRegistra;
                NuevaRequi.IP_User = ip_realiza;
                //NuevaRequi.Estatus = "Re";
                NuevaRequi.claveOLD = Convert.ToString(ClaveNueva);
                NuevaRequi.EstatusContrato = StatusContrato;
                NuevaRequi.EstatusOC = "0";

                ConBD.SISM_REQUISICION.Add(NuevaRequi);
                ConBD.SaveChanges();

                //Obtenemos la ultima requi guardada(que es esta) para guardar su detalle
                var IdRequi = (from a in ConBD.SISM_REQUISICION
                               where a.Id_User == UsuarioRegistra
                               where a.Fecha == fechaDT
                               select a).OrderByDescending(u => u.Id_Requicision).FirstOrDefault();

                //ACTUALIZAMOS EL ID DE LA REQUI PARA CONCATENARLA LA NOMENCLATURA Y ASÍ GUARDAR EL FOLIO
                //ConBD.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET Clave = 'RAC-" + IdRequi.Id_Requicision + "' WHERE Id_Requicision='" + IdRequi.Id_Requicision + "';");
                ConBD.Database.ExecuteSqlCommand("UPDATE SISM_REQUISICION SET Clave = 'RAC-" + ff + "-" + IdRequi.Id_Requicision + "' WHERE Id_Requicision='" + IdRequi.Id_Requicision + "';");

                //Recorremos que lista que recibimos como parámetro para guardar el detalle en la Requi
                foreach (var item in ListaSustanciasRequiDirecta)
                {
                    SISM_DET_REQUISICION nuevoDetalle = new SISM_DET_REQUISICION();

                    nuevoDetalle.Id_Requicision = IdRequi.Id_Requicision;
                    nuevoDetalle.Id_Sustancia = item.Id;
                    nuevoDetalle.Cantidad = item.CANTIDAD;
                    nuevoDetalle.Clave = item.Clave;
                    nuevoDetalle.Descripcion = item.descripcion_21;
                    nuevoDetalle.Compendio = item.Compendio;

                    if (StatusContrato != "Sin Contrato")
                    {
                        var ARTICULO = (from a in ConBD.SISM_COSTEO_LICITACION
                                        where a.Id_Sustancia == item.Id
                                        select a).FirstOrDefault();

                        if (ARTICULO != null)
                        {
                            nuevoDetalle.PrecioUnitario = ARTICULO.PrecioUnitario;

                            //nuevoDetalle.Total = nuevoDetalle.PrecioUnitario * nuevoDetalle.Cantidad;
                            nuevoDetalle.Total = (double?)decimal.Round((decimal)(nuevoDetalle.Cantidad * nuevoDetalle.PrecioUnitario), 2);
                        }
                        else
                        {
                        }
                    }

                    ConBD.SISM_DET_REQUISICION.Add(nuevoDetalle);
                    ConBD.SaveChanges();
                }

                //Obtenemos el Usuario que se logueó para hacer el join (buscarlo) en la tabla Usuario y así obtener el Id de esa tabla
                var UsuarioOld = User.Identity.GetUserName();
                var UsuarioOLD = (from a in RequisicionDB.Usuario
                                  where a.Usu_User == UsuarioOld
                                  select a).FirstOrDefault();

                //Volvemos a recorrer la lista (detalle de la requi) ahora para guardar en la tabla Cotizaciones     
                foreach (var item in ListaSustanciasRequiDirecta)
                {
                    SISM_COTIZACIONES Cotizacion = new SISM_COTIZACIONES();
                    Cotizacion.Id_Sustancia = item.Id;
                    Cotizacion.Id_Requicision = IdRequi.Id_Requicision;
                    Cotizacion.Id_Prov_1 = 0;
                    Cotizacion.Cant_Asig_1 = 0;
                    Cotizacion.CostoUnit_1 = 0;
                    Cotizacion.Id_Prov_2 = 0;
                    Cotizacion.Cant_Asig_2 = 0;
                    Cotizacion.CostoUnit_2 = 0;
                    Cotizacion.Id_Prov_3 = 0;
                    Cotizacion.Cant_Asig_3 = 0;
                    Cotizacion.CostoUnit_3 = 0;
                    Cotizacion.Status = false;
                    Cotizacion.FechaCrea = fechaDT;
                    Cotizacion.FechaMod = fechaDT;
                    Cotizacion.Id_Usuario = UsuarioOLD.UsuarioId;
                    Cotizacion.Cuadro = 0;
                    Cotizacion.UserId = IdUsuarioCifrado;

                    ConBD.SISM_COTIZACIONES.Add(Cotizacion);
                    ConBD.SaveChanges();
                }

                //--------------------------------------------------------------------------------------------------------------------------------  TABLAS VIEJAS
                /*  GUARDAR LA REQUISICION Y SU DETALLE EN LAS TABLAS DE REQUISICION Y DETALLE REQUISICION  */
                //Requisicion_1 Req = new Requisicion_1();
                //Req.Id_Tipo = 2;
                //Req.Fecha = fechaDT;
                //Req.Status = true;
                //Req.cerrado = false;


                //Req.UserId = IdUsuarioCifrado;

                ////Guardamos el Id del usuario de la tabla Usuario en el campo 'UsuarioId' en la tabla Requisiciones
                //if (UsuarioOLD == null)
                //{

                //}
                //else
                //{
                //    if (UsuarioOLD.Usu_User == null)
                //    {

                //    }
                //    else
                //    {
                //        Req.Id_Usuario = UsuarioOLD.UsuarioId;
                //    }
                //}

                //Req.clave = Convert.ToString(ClaveNueva);
                //Req.EstatusContrato = StatusContrato;

                //RequisicionDB.Requisicion.Add(Req);
                //RequisicionDB.SaveChanges();

                ////Obtener Requisición recién generada en las tablas viejas
                //var ID = (from a in RequisicionDB.Requisicion
                //          where a.Fecha == fechaDT
                //          where a.Id_Tipo == 2
                //          select a).OrderByDescending(u => u.id).FirstOrDefault();

                //foreach (var item in ListaSustanciasRequiDirecta)
                //{
                //    DetalleReq detRequi = new DetalleReq();
                //    detRequi.Id_Requisicion = ID.id;
                //    detRequi.Id_Sustancia = item.Id;
                //    detRequi.C_Solicitada = (int)item.CANTIDAD;
                //    detRequi.C_Recibida = 0;
                //    detRequi.Status = false;

                //    RequisicionDB.DetalleReq.Add(detRequi);
                //    RequisicionDB.SaveChanges();
                //}

                //foreach (var item in ListaSustanciasRequiDirecta)
                //{
                //    Cotizaciones Coti = new Cotizaciones();
                //    Coti.Id_Sustancia = item.Id;
                //    Coti.Id_Requisicion = ID.id;
                //    Coti.Id_Prov_1 = 0;
                //    Coti.Cant_Asig_1 = 0;
                //    Coti.CostoUnit_1 = 0;
                //    Coti.Id_Prov_2 = 0;
                //    Coti.Cant_Asig_2 = 0;
                //    Coti.CostoUnit_2 = 0;
                //    Coti.Id_Prov_3 = 0;
                //    Coti.Cant_Asig_3 = 0;
                //    Coti.CostoUnit_3 = 0;
                //    Coti.Status = false;
                //    Coti.FechaCrea = ID.Fecha;
                //    Coti.FechaMod = ID.Fecha;
                //    Coti.Id_Usuario = ID.Id_Usuario;
                //    Coti.Cuadro = 0;
                //    Coti.UserId = IdUsuarioCifrado;

                //    RequisicionDB.Cotizaciones.Add(Coti);
                //    RequisicionDB.SaveChanges();
                //}
                //--------------------------------------------------------------------------------------------------------------------------------



                return Json(new { MENSAJE = "Succe: Se generó la Requisición" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //BORRADOR DEL BORRADOR
        public JsonResult GenerarBorradorX2(List<SISM_DETALLE_BORRADOR_REQUI> ListaSustanciasBorrador, int Id_FolioBorrador)
        {
            
            var UsuarioRegistra = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;
            try
            {
                //Buscamos el borrador (este) para cambiarle el estatus y que ya no aparesca ya que se hará borrador del borrador
                var Borrador = (from a in ConBD.SISM_BORRADOR_REQUI
                                where a.Id_BorradorRequi == Id_FolioBorrador
                                select a).FirstOrDefault();

                Borrador.Estatus = "Borrador del Borrador";
                ConBD.SaveChanges();

                //Creamos borrador del borrador (nuevo borrador)
                SISM_BORRADOR_REQUI BorradorRequi = new SISM_BORRADOR_REQUI();
                BorradorRequi.Fecha = fechaDT;
                BorradorRequi.UsuarioRegistra = UsuarioRegistra;
                BorradorRequi.Estatus = "Borrador Generado";
                BorradorRequi.ip_realiza = ip_realiza;
                BorradorRequi.EstatusContrato = Borrador.EstatusContrato;
                ConBD.SISM_BORRADOR_REQUI.Add(BorradorRequi);
                ConBD.SaveChanges();

                var IdBorrador = (from a in ConBD.SISM_BORRADOR_REQUI
                                  where a.UsuarioRegistra == UsuarioRegistra
                                  select a).OrderByDescending(u => u.Id_BorradorRequi).FirstOrDefault();

                foreach (var item in ListaSustanciasBorrador)
                {
                    SISM_DETALLE_BORRADOR_REQUI nuevoDetalle = new SISM_DETALLE_BORRADOR_REQUI();
                    //nuevoDetalle.Cantidad = item.CANTIDAD;

                    if (item.CANTIDAD_NUEVA > 0)
                    {
                        if (item.CANTIDAD_NUEVA > item.Cantidad || item.CANTIDAD_NUEVA == item.Cantidad)
                            nuevoDetalle.Cantidad = item.Cantidad + item.CANTIDAD_NUEVA;

                        if (item.CANTIDAD_NUEVA < item.Cantidad)
                            nuevoDetalle.Cantidad = item.Cantidad - item.CANTIDAD_NUEVA;
                    }
                    else
                    {
                        nuevoDetalle.Cantidad = item.Cantidad;
                    }

                    nuevoDetalle.Clave = item.Clave;
                    nuevoDetalle.Id_Sustancia = item.Id_Sustancia;
                    nuevoDetalle.Id_BorradorRequi = IdBorrador.Id_BorradorRequi;
                    nuevoDetalle.Descripcion = item.Descripcion;
                    nuevoDetalle.Compendio = item.Compendio;

                    ConBD.SISM_DETALLE_BORRADOR_REQUI.Add(nuevoDetalle);
                    ConBD.SaveChanges();
                }

                return Json(new { MENSAJE = "Succe: Se guardó con éxito el Borrador" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
