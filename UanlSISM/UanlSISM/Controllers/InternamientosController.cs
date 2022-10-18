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
    public class InternamientosController : Controller
    {
        // GET: Internamientos
        public ActionResult Index()
        {
            if (User.IsInRole("Internamientos"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
                
        }

        public class OrdenInt
        {
            public int id_orden { get; set; }
            public string folio { get; set; }
            public string paciente { get; set; }
            public string fecha_registro { get; set; }
            public string boton { get; set; }
        }


        public JsonResult ListaOrdenes()
        {

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            string usuario = User.Identity.GetUserName();

            //System.Diagnostics.Debug.WriteLine(id);

            var medicamento = (from q in db.Orden_Int
                               join dhabientes in db.DHABIENTES on q.numemp equals dhabientes.NUMEMP into dhabientesX
                               from dhabientesIn in dhabientesX.DefaultIfEmpty()
                               where q.medico == usuario
                               where q.tipo == 1
                               select new
                               {
                                   id_orden = q.id_orden,
                                   numemp = q.numemp,
                                   folio = q.folio,
                                   paciente = dhabientesIn.NOMBRES + " " + dhabientesIn.APATERNO + " " + dhabientesIn.AMATERNO,
                                   fecha_registro = q.fecha_registro,

                               })
                              .ToList();

            //System.Diagnostics.Debug.WriteLine(res);

            var ordenesList = new List<OrdenInt>();

            foreach (var item in medicamento)
            {

                //buscar si ya hay una nota quirurgica realizada
                var quirurgica = (from a in db.NotaQuirurgica
                             where a.id_orden == item.id_orden
                             select a).FirstOrDefault();

                var boton = "";

                boton = "<a href='/SISM-Medico/Internamientos/Captura/" + item.id_orden + "' class='btn btn-boton'>Capturar</a>";

                if (quirurgica == null)
                {

                    var ordenes = new OrdenInt
                    {
                        id_orden = item.id_orden,
                        folio = item.folio,
                        paciente = item.paciente,
                        fecha_registro = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha_registro, new CultureInfo("es-ES")),
                        boton = boton,

                    };

                    ordenesList.Add(ordenes);
                }
                else
                {
                    var fechaOI = DateTime.Now.AddDays(-2).ToString("yyyy-MM-ddT00:00:00.000");
                    var fechaOIDT = DateTime.Parse(fechaOI);
                    if (quirurgica.fecha_registro > fechaOIDT)
                    {
                        var ordenes = new OrdenInt
                        {
                            id_orden = item.id_orden,
                            folio = item.folio,
                            paciente = item.paciente,
                            fecha_registro = string.Format("{0:dddd, dd MMMM yyyy}", item.fecha_registro, new CultureInfo("es-ES")),
                            boton = boton,

                        };

                        ordenesList.Add(ordenes);
                    }
                }

                

               
            }

            return new JsonResult { Data = ordenesList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Captura(int id)
        {
            if (User.IsInRole("Internamientos"))
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var internamiento = (from a in db.Orden_Int
                                  where a.id_orden == id
                                  select a).FirstOrDefault();

                var notaquirur = (from a in db.NotaQuirurgica
                                  where a.id_orden == internamiento.id_orden
                                  select a).FirstOrDefault();

                var dhabientes = (from a in db.DHABIENTES
                                     where a.NUMEMP == internamiento.numemp
                                     select a).FirstOrDefault();

                OrdenesInternamientos ordint = new OrdenesInternamientos();
                ordint.OrInternamiento = internamiento;
                ordint.DHabiente = dhabientes;
                if(notaquirur != null)
                {
                    ordint.Notaquir = notaquirur;
                }
                
                //dHabientesPrestaciones.Prestaciones = prestaciones;
                //dHabientesPrestaciones.HistorialPrestaciones = histprestaciones;

                return View(ordint);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public JsonResult Diagnosticos(string clave1, string clave2, string clave3)
        {

            //System.Diagnostics.Debug.WriteLine(clave1);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var diag1 = "";
            if(clave1 != "" && clave1 != null)
            {
                var diagnostico = (from a in db.DIAGNOSTICOS
                                    where a.Clave == clave1
                                    select a).FirstOrDefault();

                diag1 = diagnostico.DescCompleta;
            }

            var diag2 = "";
            if (clave2 != "" && clave2 != null)
            {
                var diagnostico = (from a in db.DIAGNOSTICOS
                                    where a.Clave == clave2
                                    select a).FirstOrDefault();

                diag2 = diagnostico.DescCompleta;
            }

            var diag3 = "";
            if (clave3 != "" && clave3 != null)
            {
                var diagnostico = (from a in db.DIAGNOSTICOS
                                   where a.Clave == clave3
                                   select a).FirstOrDefault();

                diag3 = diagnostico.DescCompleta;
            }




            var resultado = new
            {
                diagnostico1 = diag1,
                clave1 = clave1,
                diagnostico2 = diag2,
                clave2 = clave2,
                diagnostico3 = diag3,
                clave3 = clave3,
            };

            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult Recetas(int id)
        {
            if (User.IsInRole("Internamientos"))
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var internamiento = (from a in db.Orden_Int
                                     where a.id_orden == id
                                     select a).FirstOrDefault();

                var notaquirur = (from a in db.NotaQuirurgica
                                  where a.id_orden == internamiento.id_orden
                                  select a).FirstOrDefault();

                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == internamiento.numemp
                                  select a).FirstOrDefault();

                OrdenesInternamientos ordint = new OrdenesInternamientos();
                ordint.OrInternamiento = internamiento;
                ordint.DHabiente = dhabientes;
                if (notaquirur != null)
                {
                    ordint.Notaquir = notaquirur;
                }

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
            if (User.IsInRole("Internamientos"))
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var internamiento = (from a in db.Orden_Int
                                     where a.id_orden == id
                                     select a).FirstOrDefault();

                var notaquirur = (from a in db.NotaQuirurgica
                                  where a.id_orden == internamiento.id_orden
                                  select a).FirstOrDefault();

                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == internamiento.numemp
                                  select a).FirstOrDefault();

                OrdenesInternamientos ordint = new OrdenesInternamientos();
                ordint.OrInternamiento = internamiento;
                ordint.DHabiente = dhabientes;
                if (notaquirur != null)
                {
                    ordint.Notaquir = notaquirur;
                }

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
        public ActionResult TerminarReceta(Receta_Exp model, receta_exp_crn model2, int id)
        {
            //System.Diagnostics.Debug.WriteLine(model.Folio_Rcta);
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fecha_actual = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fecha__actual_correcta = DateTime.Parse(fecha_actual);

            if (model2.folio_rc != 0)
            {
                //Tomar el costo por medicamento recetado
                var costoMedicamento = (from a in db2.receta_detalle_crn
                                        where a.folio_rc == model2.folio_rc
                                        select a).ToList();

                float? costoTotal = 0;

                foreach (var cstMed in costoMedicamento)
                {
                    costoTotal = costoTotal + cstMed.costo;
                }


                db2.Database.ExecuteSqlCommand("UPDATE receta_exp_crn SET terminada = 1, costo = '" + costoTotal + "' WHERE folio_rc = '" + model2.folio_rc + "' ");
                //db2.Database.ExecuteSqlCommand("UPDATE receta_exp_crn SET terminada = 1 WHERE folio_rc = '" + model2.folio_rc + "' ");

                //TempData["message_success"] = "¡Receta de resurtimiento activada con éxito! Favor de indicarle al paciente que pasé a recepción para agendar su próxima cita.";
                TempData["message_success"] = "¡Receta de resurtimiento activada con éxito!";

            }

            if (model.Folio_Rcta != 0)
            {
                //Tomar el costo por medicamento recetado
                var costoMedicamento = (from a in db.Receta_Detalle
                                        where a.Folio_Rcta == model.Folio_Rcta
                                        select a).ToList();

                float? costoTotal = 0;

                foreach (var cstMed in costoMedicamento)
                {
                    costoTotal = costoTotal + cstMed.costo;
                }

                //System.Diagnostics.Debug.WriteLine(costoTotal);


                //Unidad Medica
                if (model.unidad_medica == 1)
                {
                    db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '7', costo = '" + costoTotal + "', unidad_medica = '" + model.unidad_medica + "' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");

                }
                else
                {
                    if (model.unidad_medica == 2)
                    {
                        db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '8', costo = '" + costoTotal + "', unidad_medica = '" + model.unidad_medica + "' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");

                    }
                    else
                    {
                        if (model.unidad_medica == 3)
                        {
                            db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '8', costo = '" + costoTotal + "', unidad_medica = '" + model.unidad_medica + "' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");

                        }
                    }
                }
                //db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Hora_Rcta = '" + fecha_actual + "', Estado = '7' WHERE Folio_Rcta = '" + model.Folio_Rcta + "' ");

                //TempData["message_success"] = "¡Receta terminada con éxito!";
            }

            return RedirectToAction("ListaRecetas/" + id, "Internamientos");

        }

        [HttpPost]
        public ActionResult GuardarNotaQuiru(int id_orden, string cirugia_planeada, string cirugia_realizada, string tipo_anestesia, string descripcion, string hallazgo, string incidentes, string sangrado, string estado_actual, string plan_manejo, string pronostico, int tiempo, string clave1, string clave2, string clave3)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);
            //string usuario = User.Identity.GetUserName();
            var ip_realiza = Request.UserHostAddress;


            //Revisar si la orden ya tiene nota quirurgica
            var orden = (from a in db.NotaQuirurgica
                         where a.id_orden == id_orden
                         //where a.nota_quirurgica != null
                         select a).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(orden);

            if (orden == null)
            {

                //System.Diagnostics.Debug.WriteLine(id_orden);

                NotaQuirurgica notaqui = new NotaQuirurgica();
                notaqui.id_orden = id_orden;
                notaqui.cirugia_planeada = cirugia_planeada;
                notaqui.cirugia_realizada = cirugia_realizada;
                notaqui.tipo_anestesia = tipo_anestesia;
                notaqui.descripcion = descripcion;
                notaqui.hallazgo = hallazgo;
                notaqui.incidentes = incidentes;
                notaqui.sangrado = sangrado;
                notaqui.estado_actual = estado_actual;
                notaqui.plan_manejo = plan_manejo;
                notaqui.pronostico = pronostico;
                notaqui.tiempo = tiempo;
                notaqui.diagnostico1 = clave1;
                if(clave2 != "" && clave2 != null)
                {
                    notaqui.diagnostico2 = clave2;
                }
                if (clave3 != "" && clave3 != null)
                {
                    notaqui.diagnostico3 = clave3;
                }
                notaqui.fecha_registro = fechaDT;
                notaqui.ip_realiza = ip_realiza;
                db.NotaQuirurgica.Add(notaqui);
                db.SaveChanges();


                //guardar
                //db.Database.ExecuteSqlCommand("UPDATE Orden_Int SET nota_quirurgica = '" + cirugia_planeada + "', cirugia_realizada = '" + cirugia_realizada + "', tipo_anestesia = '" + tipo_anestesia + "', descripcion = '" + descripcion + "', hallazgo = '" + hallazgo + "', incidentes = '" + incidentes + "', sangrado = '" + sangrado + "', estado_actual = '" + estado_actual + "', plan_manejo = '" + plan_manejo + "', pronostico = '" + pronostico + "', tiempo = " + tiempo + ", diagnostico1 = '" + clave1 + "' fecha_registro = '" + fecha + "', ip_realiza = '" + ip_realiza + "'   WHERE id_orden = '" + id_orden + "' ");


                TempData["message_success"] = "Nota quirúrgica terminada con éxito";
            }
            else
            {
                if(clave1 != null && clave1 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE NotaQuirurgica SET cirugia_planeada = '" + cirugia_planeada + "', cirugia_realizada = '" + cirugia_realizada + "', tipo_anestesia = '" + tipo_anestesia + "', descripcion = '" + descripcion + "', hallazgo = '" + hallazgo + "', incidentes = '" + incidentes + "', sangrado = '" + sangrado + "', estado_actual = '" + estado_actual + "', plan_manejo = '" + plan_manejo + "', pronostico = '" + pronostico + "', tiempo = " + tiempo + ", diagnostico1 = '" + clave1 + "', fecha_registro = '" + fecha + "', ip_realiza = '" + ip_realiza + "'   WHERE id_orden = '" + id_orden + "' ");
                }

                if (clave2 != null && clave2 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE NotaQuirurgica SET cirugia_planeada = '" + cirugia_planeada + "', cirugia_realizada = '" + cirugia_realizada + "', tipo_anestesia = '" + tipo_anestesia + "', descripcion = '" + descripcion + "', hallazgo = '" + hallazgo + "', incidentes = '" + incidentes + "', sangrado = '" + sangrado + "', estado_actual = '" + estado_actual + "', plan_manejo = '" + plan_manejo + "', pronostico = '" + pronostico + "', tiempo = " + tiempo + ", diagnostico2 = '" + clave2 + "', fecha_registro = '" + fecha + "', ip_realiza = '" + ip_realiza + "'   WHERE id_orden = '" + id_orden + "' ");
                }

                if (clave3 != null && clave3 != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE NotaQuirurgica SET cirugia_planeada = '" + cirugia_planeada + "', cirugia_realizada = '" + cirugia_realizada + "', tipo_anestesia = '" + tipo_anestesia + "', descripcion = '" + descripcion + "', hallazgo = '" + hallazgo + "', incidentes = '" + incidentes + "', sangrado = '" + sangrado + "', estado_actual = '" + estado_actual + "', plan_manejo = '" + plan_manejo + "', pronostico = '" + pronostico + "', tiempo = " + tiempo + ", diagnostico3 = '" + clave3 + "', fecha_registro = '" + fecha + "', ip_realiza = '" + ip_realiza + "'   WHERE id_orden = '" + id_orden + "' ");
                }

                //System.Diagnostics.Debug.WriteLine(id_orden);

                db.Database.ExecuteSqlCommand("UPDATE NotaQuirurgica SET cirugia_planeada = '" + cirugia_planeada + "', cirugia_realizada = '" + cirugia_realizada + "', tipo_anestesia = '" + tipo_anestesia + "', descripcion = '" + descripcion + "', hallazgo = '" + hallazgo + "', incidentes = '" + incidentes + "', sangrado = '" + sangrado + "', estado_actual = '" + estado_actual + "', plan_manejo = '" + plan_manejo + "', pronostico = '" + pronostico + "', tiempo = " + tiempo + ", fecha_registro = '" + fecha + "', ip_realiza = '" + ip_realiza + "'   WHERE id_orden = '" + id_orden + "' ");


                TempData["message_success"] = "Nota quirúrgica actualizada con éxito";
            }


            
            return Redirect(Request.UrlReferrer.ToString());
        }


        public ActionResult Alta(int id)
        {
            if (User.IsInRole("Internamientos"))
            {
                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

                var internamiento = (from a in db.Orden_Int
                                     where a.id_orden == id
                                     select a).FirstOrDefault();

                var notaquirur = (from a in db.NotaQuirurgica
                                  where a.id_orden == internamiento.id_orden
                                  select a).FirstOrDefault();

                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == internamiento.numemp
                                  select a).FirstOrDefault();

                OrdenesInternamientos ordint = new OrdenesInternamientos();
                ordint.OrInternamiento = internamiento;
                ordint.DHabiente = dhabientes;
                if (notaquirur != null)
                {
                    ordint.Notaquir = notaquirur;
                }

                //dHabientesPrestaciones.Prestaciones = prestaciones;
                //dHabientesPrestaciones.HistorialPrestaciones = histprestaciones;

                return View(ordint);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult GuardarAlta(int id_orden, string fecha_alta, string motivo_alta)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();


            db.Database.ExecuteSqlCommand("UPDATE Orden_Int SET fecha_alta = '" + fecha_alta + "T00:00:00.000', motivo_alta = '" + motivo_alta + "'   WHERE id_orden = '" + id_orden + "' ");


            TempData["message_success"] = "Alta generada con éxito";

            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}