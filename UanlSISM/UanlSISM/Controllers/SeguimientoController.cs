
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
                #endregion

                var query = db3.SP_Trazabilidad(FechaInicio, FechaFin, ClaveMed).ToList();

                return Json(new { MENSAJE = "FOUND", REP = query }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerReporteOC(string FechaInicio, string FechaFin, string ClaveMed)
        {
            try
            {
                #region Fechas
                var fechaI = FechaInicio + " 00:00:00";
                //var fechaIn = DateTime.Parse(fechaI);

                var fechaF = FechaFin + " 23:59:59";
                //var fechaFi = DateTime.Parse(fechaF);
                #endregion

                //var query = db3.SP_Trazabilidad_OC(FechaInicio, FechaFin, ClaveMed).ToList();
                var query = db3.SP_Trazabilidad_OC(fechaI, fechaF, ClaveMed).ToList();

                return Json(new { MENSAJE = "FOUND", REP = query }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ObtenerReporteVE(int IdOC, int IdSus)
        {
            try
            {
                var query = db3.SP_Trazabilidad_VE(IdOC, IdSus).ToList();

                return Json(new { MENSAJE = "FOUND", R = query }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Trazabilidad_Completa(string FechaInicio, string FechaFin, string ClaveMed)
        {
            try
            {
                #region Fechas
                var fechaI = FechaInicio + " 00:00:00";
                var fechaIn = DateTime.Parse(fechaI);

                var fechaF = FechaFin + " 23:59:59";
                var fechaFi = DateTime.Parse(fechaF);
                #endregion

                var query = db3.SP_Trazabilidad_Completa(FechaInicio, FechaFin, ClaveMed).ToList();

                return Json(new { MENSAJE = "FOUND", REP = query }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
