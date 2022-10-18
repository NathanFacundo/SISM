using System;
using System.Linq;
using System.Web.Mvc;
using UanlSISM.Models;
using System.Diagnostics;
using System.Data.Entity.Validation;
using Microsoft.AspNet.Identity;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class CatalogoMedicamentoController : Controller
    {
        SERVMEDEntities5 db = new SERVMEDEntities5();
        SMDEVEntities25 db1 = new SMDEVEntities25();
        Sustancia T = new Sustancia();
        int Respuesta;
        // GET: CatalogoMedicamento
        public ActionResult CatalogoMedicamento()
        {
            return View();
        }
        public JsonResult GetGrupoTerapeutico()
        {
            var query = (from a in db.grupo_21
                         orderby a.descripcion ascending
                         select new
                         {
                             a.clave,
                             a.descripcion
                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult PrincipioActivo()
        {
            var query = (from a in db.principioactivo_21
                         orderby a.descripcion ascending
                         select new
                         {
                             a.clave,
                             _Descp = a.descripcion.Trim()

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult CantidadEnvaces()
        {
            var query = (from a in db.cantidadenvases_21
                         orderby a.descripcion ascending
                         select new
                         {
                             a.clave,
                             _Descp = a.descripcion

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult TipoEnvace()
        {
            var query = (from a in db.tipodeenvase_21
                         orderby a.descripcion ascending
                         select new
                         {
                             a.clave,
                             _Descp = a.descripcion.Trim()
                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult UnidadesLicitacion()
        {
            var query = (from a in db.unidadeslicitacion_21
                         orderby a.descripcion ascending
                         select new
                         {
                             a.clave,
                             _Descp = a.descripcion
                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult UnidadesMedida()
        {
            var query = (from a in db.unidadesmedida_21
                         orderby a.descripcion ascending
                         select new
                         {
                             a.clave,
                             _Descp = a.descripcion.Trim()
                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult FormaFarm()
        {
            var query = (from a in db.formafarm_21
                         orderby a.descripcion ascending
                         select new
                         {
                             a.clave,
                             _Descp = a.descripcion.Trim()
                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Concentracion()
        {
            var query = (from a in db.concentracion_21
                         orderby a.descripcion ascending
                         select new
                         {
                             a.clave,
                             _Descp = a.descripcion.Trim()
                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ValidaExistencia(string Descripcion, int Grupo)
        {
            var query = (from a in db.Sustancia
                         where a.descripcion_21 == Descripcion && a.id_grupo_21 == Grupo
                         select new
                         {
                             a.id_grupo_21,
                             a.descripcion_21

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public int GuardaNuevaDesc(string Descripcion, int IdGrupo, int IdPrincipio, int IdForma, int IdConcentracion, int IdLicitacion, int IdMedida, int IdCantidad, int TipoEnvace, string Cod21)
        {
            try
            {
                var query = (from a in db.Sustancia select a.Clave).Max();
                int ClaveNueva = Convert.ToInt32(query) + 1;

                T.Id_Grupo = 1;
                T.Id_Sal = 1;
                T.Id_Nivel = 1;
                T.clave_presentacion = "01";
                T.Presentacion = "Sin descripcion";
                T.Clave = ClaveNueva.ToString();
                T.Status = false;
                T.Consultorio = "0";
                T.cto_promedio = 0;
                T.cto_ultimo = 0;
                T.descripcion_21 = Descripcion;
                T.id_grupo_21 = IdGrupo;
                T.id_principioactivo_21 = IdPrincipio;
                T.id_formafarm_21 = IdForma;
                T.id_concentracion_21 = IdConcentracion;
                T.id_unidadeslicitacion_21 = IdLicitacion;
                T.id_unidadesmedida_21 = IdMedida;
                T.id_cantidadenvases_21 = IdCantidad;
                T.id_tipoenvase_21 = TipoEnvace;
                T.codigo_21 = Cod21;

                db.Sustancia.Add(T);
                db.SaveChanges();

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
        public JsonResult ValidaCodigo(string Codigo)
        {
            var query = (from a in db.Sustancia
                         where a.Clave == Codigo
                         select new
                         {
                             a.codigo_21,

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public int UpdateDescripcion(string Codigo, string Descripcion, int IdGrupo, int IdPrincipio, int IdForma, int IdConcentracion, int IdLicitacion, int IdMedida, int IdCantidad, int TipoEnvace, string Cod21,string SobranteInv2022,string IP)
        {

            try
            {
                var query = (from a in db.Sustancia
                             where a.Clave == Codigo
                             select a).FirstOrDefault();

                query.descripcion_21 = Descripcion;
                query.id_grupo_21 = IdGrupo;
                query.id_principioactivo_21 = IdPrincipio;
                query.id_formafarm_21 = IdForma;
                query.id_concentracion_21 = IdConcentracion;
                query.id_unidadeslicitacion_21 = IdLicitacion;
                query.id_unidadesmedida_21 = IdMedida;
                query.id_cantidadenvases_21 = IdCantidad;
                query.id_tipoenvase_21 = TipoEnvace;
                query.codigo_21 = Cod21;
                query.SobranteInv2022 = SobranteInv2022;
                query.IP = IP;
                query.FechaMed = DateTime.Now;
                query.UserId = User.Identity.GetUserId();

                db.SaveChanges();
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
        public JsonResult DescripcionAnt(string Codigo)
        {
            var query = (from a in db1.InventarioFarmacia
                         where a.Clave == Codigo
                         select new
                         {
                             a.Clave,
                             a.Descripcion_Sal,
                             a.Presentacion

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}