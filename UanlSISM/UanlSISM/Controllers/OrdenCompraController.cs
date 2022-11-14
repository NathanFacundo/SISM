﻿using System;
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
                             where a.EstatusOC == "0"
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
    }
}