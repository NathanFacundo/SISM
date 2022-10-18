using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class AlmacenController : Controller
    {
        // GET: Almacen
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult InventarioLista()
        {
            if (User.IsInRole("Almacen"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public class LstInv
        {
            public int id_sustancia { get; set; }
            public int id { get; set; }
            public int sal { get; set; }
            public int inv_act { get; set; }
            public int manejodisponible { get; set; }
            public string clave { get; set; }
            public string descripcion_grupo { get; set; }
            public string descripcion_sal { get; set; }
            public string presentacion { get; set; }
            public string clave_nivel { get; set; }
            public string usuario_registra { get; set; }
        }


        public class LstInv2
        {
            public int id_sustancia { get; set; }
            public string clave { get; set; }
            public string descripcion_sal { get; set; }
            public string presentacion { get; set; }
            public string descripcion_grupo { get; set; }
            public int inv_act { get; set; }
            public int manejodisponible { get; set; }
            public string usuario_registra { get; set; }
            public string boton { get; set; }
        }



        public JsonResult ListInvent()
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            string usuario = User.Identity.GetUserName();

            //System.Diagnostics.Debug.WriteLine(id);
            
            string query = "SELECT InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Disp AS inv_act, InvFarm_1.ManejoDisponible AS manejodisponible, Sustancia_1.Clave AS clave, Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel as descripcion_nivel FROM SERVMED.dbo.grupo_21 AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvAlmFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvAlmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 2) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != '' ";
            var result = db.Database.SqlQuery<LstInv>(query);
            var res = result.ToList();

            //System.Diagnostics.Debug.WriteLine(res);
            
            var invLista = new List<LstInv2>();

            foreach (var item in res)
            {
                var boton = "";

                /*if (User.Identity.Name == "22017" || User.Identity.Name == "direccion" || User.Identity.Name == "coordinacion" || User.Identity.Name == "22011" || User.Identity.Name == "sistemas" || User.Identity.Name == "jorge") {
                    boton = "...";
                }
                else
                {*/
                    if (item.usuario_registra != null)
                    {
                        boton = "<button data-id='" + item.id + "' class='btn btn-subir' style='background-color:green !important;'>Actualizar</button>";
                    }
                    else
                    {
                        boton = "<button data-id='" + item.id + "' class='btn btn-subir'>Subir</button>";
                    }
                /*}*/

                var inventario = new LstInv2
                {
                    id_sustancia = item.id_sustancia,
                    clave = item.clave,
                    descripcion_sal = item.descripcion_sal,
                    presentacion = item.presentacion,
                    descripcion_grupo = item.descripcion_grupo,
                    manejodisponible = item.manejodisponible,
                    boton = boton,
                };

                invLista.Add(inventario);
            }


            return new JsonResult { Data = invLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult SubirInventario(int id, int total)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var usuario = User.Identity.GetUserName();

            //System.Diagnostics.Debug.WriteLine(total);

            db.Database.ExecuteSqlCommand("UPDATE InvAlmFarm SET Inv_Ini = " + total + ", Inv_Disp = " + total + ", ManejoDisponible = " + total + ", Usuario_Registra = '" + usuario + "' WHERE Id = '" + id + "' ");

            //System.Diagnostics.Debug.WriteLine(medicamento);

            var res = "";
            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public class Inventario
        {
            public Int16 id { get; set; }
            public string nom_invent { get; set; }
            public bool status { get; set; }
            public byte tipo { get; set; }

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


        [HttpPost]
        public ActionResult GenerarInventario() {

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var usuario = User.Identity.GetUserName();

            var today = DateTime.Today;
            DateTime? date = DateTime.Now;
            var mes = date.Value.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            var mes2 = mes.Substring(0, 3);
            var mesfinal = mes2.ToUpper();
            var year = today.Year;

            

            //Tomar el inventario que esta activo
            string query = "select id as id, nom_invent as nom_invent, status as status, tipo as tipo from Inventario WHERE status = 1 and tipo = 2";
            var result = db.Database.SqlQuery<Inventario>(query);
            var res = result.FirstOrDefault();


            //Crear nuevo inventario, se pondrá en tipo 4 por lo pronto
            db.Database.ExecuteSqlCommand("INSERT INTO Inventario (nom_invent, status, tipo) VALUES('Alm" + mesfinal + year + "', 0, 4)");

            //Hacer lista del inventario que esta activo
            /*string query2 = "select Id as Id, InvAlmId as InvAlmId, Fecha as Fecha, Id_Sustancia as Id_Sustancia, Inv_Ini as Inv_Ini, Inv_Ent as Inv_Ent, Inv_Sal as Inv_Sal, Inv_Disp as Inv_Disp, Inv_Reorden as Inv_Reorden, ManejoDisponible as ManejoDisponible, Usuario_Registra as Usuario_Registra from InvAlmFarm WHERE InvAlmId = " + res.id + " ";
            var result2 = db.Database.SqlQuery<InvAlmFarm>(query2);
            var res2 = result2.ToList();*/
            var sustancia = (from q in db.Sustancia
                                 //where q.Clave == medicamento.Clave
                             where q.descripcion_21 != null && q.descripcion_21 != ""
                             select q).ToList();


            //Buscar idinventario con tipo = 4
            string query3 = "select id as id, nom_invent as nom_invent, status as status, tipo as tipo from Inventario WHERE status = 0 and tipo = 4";
            var result3 = db.Database.SqlQuery<Inventario>(query3);
            var res3 = result3.FirstOrDefault();



            //Insertar todos de nuevo pero con el nuevo Id del inventario y con Inv_Ini, Inv_Ent y Inv_Sal en 0
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:00");
            foreach (var item in sustancia)
            {
                db.Database.ExecuteSqlCommand("INSERT INTO InvAlmFarm(InvAlmId, Fecha, Id_Sustancia, Inv_Ini, Inv_Ent, Inv_Sal, Inv_Disp, Inv_Reorden, ManejoDisponible) VALUES("+ res3.id + ", '" + fecha + "', "+ item.Id + ", 0, 0, 0, 0, 0, 0)");
            }

            //Desactivar el inventario actual
            db.Database.ExecuteSqlCommand("UPDATE Inventario SET status = 0 WHERE Id = '" + res.id + "' ");

            //Activar el creado
            db.Database.ExecuteSqlCommand("UPDATE Inventario SET status = 1, tipo = 2 WHERE Id = '" + res3.id + "' ");


            TempData["message_success"] = "Inventario generado con éxito";
            return Redirect(Request.UrlReferrer.ToString());

        }


        public JsonResult InventarioActivo()
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            //Tomar el inventario que esta activo
            string query = "select id as id, nom_invent as nom_invent, status as status, tipo as tipo from Inventario WHERE status = 1 and tipo = 2";
            var result = db.Database.SqlQuery<Inventario>(query);
            var res = result.FirstOrDefault();
            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult EntradaAlmacen()
        {

            return View();
        }
    }
}