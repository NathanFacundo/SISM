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

        [Authorize]
        public ActionResult OrdenCompra()
        {
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
        }

        public ActionResult ObtenerRequisInicio()
        {
            try
            {
                var query = (from a in ConBD.SISM_REQUISICION
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
                                 det.Compendio
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
                            Compendio = q.Compendio
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
    }
}
