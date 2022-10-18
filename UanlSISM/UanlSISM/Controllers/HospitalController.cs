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
    [Authorize]
    public class HospitalController : Controller
    {
        // GET: Hospital
        public ActionResult PacientesHospital()
        {
            if (User.IsInRole("PacientesHospital"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public class PacientesLst
        {
            public int id { get; set; }
            public string folio { get; set; }
            public string paciente { get; set; }
            public string fecha_registro { get; set; }
            public string boton { get; set; }
        }


        public JsonResult ListaPacientes()
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            string usuario = User.Identity.GetUserName();

            //System.Diagnostics.Debug.WriteLine(id);

            var altaurg = (from q in db.AltaUrgencias
                               join dhabientes in db.DHABIENTES on q.numexp equals dhabientes.NUMEMP into dhabientesX
                               from dhabientesIn in dhabientesX.DefaultIfEmpty()
                               where q.medico == usuario
                               //where q.tipo == 1
                               select new
                               {
                                   paciente = dhabientesIn.NOMBRES + " " + dhabientesIn.APATERNO + " " + dhabientesIn.AMATERNO,
                                   fecha_registro = q.fecha_registro,
                                   id = q.id,

                               })
                              .ToList();

            //System.Diagnostics.Debug.WriteLine(res);

            var ordenesList = new List<PacientesLst>();

            foreach (var item in altaurg)
            {

                var boton = "";

                boton = "<a href='/Hospital/Captura/" + item.id + "' class='btn btn-boton'>Capturar</a>";


                var ordenes = new PacientesLst
                {
                    id = item.id,
                    paciente = item.paciente,
                    fecha_registro = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha_registro, new CultureInfo("es-ES")),
                    boton = boton,

                };

                ordenesList.Add(ordenes);





            }

            return new JsonResult { Data = ordenesList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Captura(int id)
        {
            if (User.IsInRole("PacientesHospital"))
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var alturg = (from a in db.AltaUrgencias
                              where a.id == id
                              select a).FirstOrDefault();

                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == alturg.numexp
                                  select a).FirstOrDefault();

                PacientesHosp ordint = new PacientesHosp();
                ordint.AltaUrg = alturg;
                ordint.DHabiente = dhabientes;
                //dHabientesPrestaciones.Prestaciones = prestaciones;
                //dHabientesPrestaciones.HistorialPrestaciones = histprestaciones;

                return View(ordint);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult Recetas(int id)
        {
            if (User.IsInRole("PacientesHospital"))
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var alturg = (from a in db.AltaUrgencias
                              where a.id == id
                              select a).FirstOrDefault();

                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == alturg.numexp
                                  select a).FirstOrDefault();

                PacientesHosp ordint = new PacientesHosp();
                ordint.AltaUrg = alturg;
                ordint.DHabiente = dhabientes;
                //dHabientesPrestaciones.Prestaciones = prestaciones;
                //dHabientesPrestaciones.HistorialPrestaciones = histprestaciones;

                return View(ordint);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult ListaRecetas(int id)
        {
            if (User.IsInRole("PacientesHospital"))
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var alturg = (from a in db.AltaUrgencias
                              where a.id == id
                              select a).FirstOrDefault();

                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == alturg.numexp
                                  select a).FirstOrDefault();

                PacientesHosp ordint = new PacientesHosp();
                ordint.AltaUrg = alturg;
                ordint.DHabiente = dhabientes;
                //dHabientesPrestaciones.Prestaciones = prestaciones;
                //dHabientesPrestaciones.HistorialPrestaciones = histprestaciones;

                return View(ordint);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult Alta(int id)
        {
            if (User.IsInRole("PacientesHospital"))
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var alturg = (from a in db.AltaUrgencias
                              where a.id == id
                              select a).FirstOrDefault();

                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == alturg.numexp
                                  select a).FirstOrDefault();

                PacientesHosp ordint = new PacientesHosp();
                ordint.AltaUrg = alturg;
                ordint.DHabiente = dhabientes;
                //dHabientesPrestaciones.Prestaciones = prestaciones;
                //dHabientesPrestaciones.HistorialPrestaciones = histprestaciones;

                return View(ordint);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }



        [HttpPost]
        public ActionResult GuardarNota(int id, string nota, string fecha_nota)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var fecha = DateTime.Now.ToString(fecha_nota + "T00:00:00.000");

            //Revisar si la orden ya tiene nota quirurgica
            var altaurg = (from a in db.AltaUrgencias
                         where a.id == id
                         select a).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(orden);

            if (altaurg != null)
            {

                db.Database.ExecuteSqlCommand("UPDATE AltaUrgencias SET nota = '" + nota + "', fecha_nota = '" + fecha + "'  WHERE id = '" + id + "' ");


                TempData["message_success"] = "Nota de egreso actualizada con éxito";
            }

            return Redirect(Request.UrlReferrer.ToString());
        }


        [HttpPost]
        public ActionResult GuardarAlta(int id, string nota_alta, string fecha_alta)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var fecha = DateTime.Now.ToString(fecha_alta + "T00:00:00.000");

            //Revisar si la orden ya tiene nota quirurgica
            var altaurg = (from a in db.AltaUrgencias
                           where a.id == id
                           select a).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(orden);

            if (altaurg != null)
            {

                db.Database.ExecuteSqlCommand("UPDATE AltaUrgencias SET nota_alta = '" + nota_alta + "', fecha_alta = '" + fecha + "'  WHERE id = '" + id + "' ");


                TempData["message_success"] = "Nota de egreso actualizada con éxito";
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}