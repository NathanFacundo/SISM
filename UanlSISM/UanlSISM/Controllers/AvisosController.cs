using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class AvisosController : Controller
    {
        // GET: Avisos
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult MostrarAviso()
        {
            //Avisos en el sistema
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();

            /*var avisos = (from au in db.Aviso_Usuario
                          where au.usuario == username
                          join aviso in db.Avisos on au.id_aviso equals aviso.id into avisoX
                          from avisoIn in avisoX.DefaultIfEmpty()
                          where avisoIn.fecha == fechaDT
                          where avisoIn.estado == 1
                          select new
                          {
                              aviso = avisoIn.aviso,
                              //talla_r = r.talla_r,
                          })
                          .FirstOrDefault();*/

            //Buscar si el usuario ya vio el aviso
            var avisoUsu = (from au in db.ComunicadoUsuario
                            join comunicado in db.Comunicados on au.id_comunicado equals comunicado.id into comunicadoX
                            from comunicadoIn in comunicadoX.DefaultIfEmpty()
                            where au.id_comunicado == 5
                            where au.usuario == username
                            select au)
                          .FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(avisoUsu);

            //Solo medicos
            if (User.Identity.Name == "02101" || User.Identity.Name.Substring(0, 2) == "05" || User.Identity.Name.Substring(0, 2) == "29" || User.Identity.Name.Substring(0, 2) == "14" || User.Identity.Name.Substring(0, 2) == "60" || User.Identity.Name.Substring(0, 2) == "23" || User.Identity.Name.Substring(0, 2) == "09")
            {
                //Si no lo ha visto
                if (avisoUsu == null)
                {
                    var aviso = (from av in db.Comunicados
                                 where av.id == 5
                                 select new
                                 {
                                     aviso = av.aviso,
                                     id = av.id,
                                     //talla_r = r.talla_r,
                                 })
                                .FirstOrDefault();

                    return new JsonResult { Data = aviso, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    string aviso = "";

                    return new JsonResult { Data = aviso, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            else
            {
                string aviso = "";

                return new JsonResult { Data = aviso, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }



            //System.Diagnostics.Debug.WriteLine(avisoUsu);




        }


        public JsonResult ConfirmarAviso()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            //var fechaDT = DateTime.Parse(fecha);
            var username = User.Identity.GetUserName();

            db.Database.ExecuteSqlCommand("INSERT INTO ComunicadoUsuario (usuario, id_comunicado, fecha, estado) VALUES('" + username + "', 5, '" + fecha + "', 1)");

            return new JsonResult { Data = fecha, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


    }
}