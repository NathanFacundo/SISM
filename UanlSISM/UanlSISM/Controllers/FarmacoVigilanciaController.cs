using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class FarmacoVigilanciaController : Controller
    {
        SMDEVEntities33 db = new SMDEVEntities33();
        SMDEVEntities38 fv = new SMDEVEntities38();
        TblNotaFarmaco Nota = new TblNotaFarmaco();
        DateTime fecha = DateTime.Now;
        string localIP;
        int Respuesta;
        string Id;
        // GET: FarmacoVigilancia
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetDatos(string NumEmp)
        {
            var query = (from a in db.DHABIENTES
                         where a.NUMEMP == NumEmp
                         select new
                         {
                             a.NOMBRES,
                             a.APATERNO,
                             a.AMATERNO

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetNotasFarmaco(string NumEmp)
        {
            var query = fv.Sp_NotasFarmaco(NumEmp).ToList();
            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public int InsertNota(string Nombre, string AP, string AM, string NotaFV, string NumEmp,int Nivel,string IP)
        {
            //IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            //foreach (IPAddress ip in host.AddressList)
            //{
            //    if (ip.AddressFamily.ToString() == "InterNetwork")
            //    {
            //        localIP = ip.ToString();
            //    }
            //}

            var query = (from a in db.MEDICOS
                         where a.Nombre.Contains(Nombre) && a.Apaterno.Contains(AP) && a.Amaterno.Contains(AM)
                         select a.Numero).First();

            Id = query;

            try
            {
                Nota.NotaFarmaco = NotaFV;
                Nota.MedicoRealizo = Id;
                Nota.IP_Realizo = IP;
                Nota.FechaNotaFarmaco = fecha;
                Nota.NUMEMP = NumEmp;
                Nota.Riesgo = Nivel;

                fv.TblNotaFarmaco.Add(Nota);
                fv.SaveChanges();

                Respuesta = 1;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            return Respuesta;
        }
    }
}