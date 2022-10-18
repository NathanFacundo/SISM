using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;
using UanlSISM.ViewModels;
using static UanlSISM.Controllers.ServiciosMedicosController;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class MedicinaLaboralController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        // GET: MedicinaLaboral

        public ActionResult Index()
        {
            if (User.IsInRole("MedicinaLaboral"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Index2()
        {
            if (User.IsInRole("MedicinaLaboral"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult BsqMedico(string v_NUMEM)
        {
            DalMisMedicos dalObj = new DalMisMedicos();


            return new JsonResult { Data = dalObj.BuscarMedico(v_NUMEM), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        public ActionResult ConsultarAgenda()
        {
            DalMisMedicos dalObj = new DalMisMedicos();
            ViewData["cmb"] = dalObj.cmbEspMed();
            return View();
        }
        public ActionResult ConsultaNotasPorMedico()
        {
            DalMisMedicos dalObj = new DalMisMedicos();
            ViewData["cmb"] = dalObj.cmbEspMed();
            return View();
        }

        [HttpPost]
        public ActionResult GetNotasPorMedico(string pClaveMedico, string cboMEDICOS, DateTime TxtFecha)
        {
            DateTime pFechaConsulta = new DateTime();

            NotasMedicoViewModel vm = new NotasMedicoViewModel();
            vm.NUMERO = pClaveMedico;
            vm.FECHACONSULTA = TxtFecha;
            vm.NOMBRESMEDICO = "";
            vm.TITULO = "";
            /*
            vm.ClaveMedico = pClaveMedico;
            vm.DataMedico = "juan perez";
            vm.FechaConsulta = pFechaConsulta;
            vm.Lista = new List<Med_Expediente>();
            */
            //public List<Med_Expediente> BuscarEspMedicoActivo(string pClaveMedico, string cboMEDICOS, DateTime TxtFecha)

            DalMisMedicos dalObj = new DalMisMedicos();

            vm.Lista = dalObj.BuscarEspMedicoActivo(pClaveMedico, cboMEDICOS, TxtFecha).ToList<Med_Expediente>();
            if (vm.Lista.Count == 0)
            {
                vm.NOMBRESMEDICO = "";
                vm.TITULO = "";
            }
            else
            {
                vm.NOMBRESMEDICO = vm.Lista.First().NOMBRESMEDICO;
                vm.TITULO = vm.Lista.First().TITULO;
            }


            return View("InformeNotasPorMedico", vm);
        }
        public ActionResult InformeNotasPorMedico()
        {

            return View();

        }



        //Reactivar citas canceladas al dia 
        public ActionResult Activar_citacancelada()
        {
            try
            {
                // usar using para cerrar la conexion con seguridad.
                using (SMDEVEntities33 db = new Models.SMDEVEntities33())
                {
                    var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                    var fecha_correcta = DateTime.Parse(fecha);

                    //var query = "SELECT * FROM CITAS INNER JOIN TIPOSCONSULTA ON CITAS.Tipo = TIPOSCONSULTA.Clave WHERE TIPOSCONSULTA.Clave = '00' and NumEmp is not null";
                    var query2 = "select CITAS.Medico as Numero_Medico, CITAS.NumEmp as NumEmp, CITAS.Fecha as start, CITAS.Id as Id ,CITAS.turno as turno ,CITAS.REGXDIA as regxdia, DHABIENTES.NOMBRES as Nombres, DHABIENTES.FNAC as fechanacimiento, DHABIENTES.SEXO as Sexo, DHABIENTES.APATERNO as Apaterno, DHABIENTES.AMATERNO as Amaterno, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as Medico, TIPOSCONSULTA.Descripcion as tipo, CITAS.hr_consultorio as hr_consultorio, CASE WHEN len(DHABIENTES.tel_consulta) > 0 THEN '52' + DHABIENTES.tel_consulta ELSE '52' + AFILIADOS.tel_celular END AS telefono from CITAS INNER JOIN MEDICOS ON MEDICOS.Numero = CITAS.Medico INNER JOIN DHABIENTES ON DHABIENTES.NUMEMP = CITAS.NumEmp INNER JOIN TIPOSCONSULTA " +
                        "ON TIPOSCONSULTA.Clave = CITAS.Tipo INNER JOIN AFILIADOS ON AFILIADOS.num_contrato = DHABIENTES.NUMAFIL where CITAS.Tipo = '00' AND CITAS.FECHA = CAST(CAST(GETDATE()  AS DATE) AS SMALLDATETIME)  AND (LEN(CITAS.NumEmp) > 0) AND (CITAS.NumEmp <> '99999100') ORDER BY Fecha DESC";

                    var result = db.Database.SqlQuery<CitasViewModel>(query2).ToList();

                    System.Diagnostics.Debug.WriteLine(result);

                    foreach (var cita in result)
                    {
                        //Buscar si ya tiene registrada una consulta para ver si ya termino la consulta o esta en proceso
                        var expe = (from a in db.expediente
                                    where a.medico == cita.Numero_Medico
                                    where a.numemp == cita.NumEmp
                                    where a.fecha == fecha_correcta
                                    //where a.hora_termino != null
                                    select a).FirstOrDefault();

                        var estatus = "";

                        //Si no hay una consulta
                        if (expe == null)
                        {
                            //Si no ha iniciado la consulta
                            if (cita.hr_consultorio != null)
                            {
                                estatus = "Programada";
                            }
                            else
                            {
                                estatus = "En espera";
                            }
                        }
                        else
                        {
                            //Si hay una pero no ha terminado
                            if (expe.hora_termino == null)
                            {
                                estatus = "En proceso";
                            }
                            //Si ya terminó
                            else
                            {
                                estatus = "Finalizada";
                            }
                        }

                        //var resultado = new MisCitasList
                        //{
                        //    Numero_Medico = cita.Numero_Medico + "*" + cita.NumEmp + "*" + cita.regxdia + "*" + string.Format("{0:yyyy-MM-ddT00:00:00.000}", cita.start),
                        //    Paciente = cita.Nombres + " " + cita.Apaterno + " " + cita.Amaterno + "<br><span><b>" + cita.telefono + "</b></span>",
                        //    Medico = cita.Medico,
                        //    tipo = cita.tipo,
                        //    Estatus = estatus,
                        //    Fecha = string.Format("{0:yyyy-MM-dd}", cita.start),
                        //    //Recetas = cita.Numero_Medico,
                        //    //Recetas = "",
                        //};

                        //results1.Add(resultado);

                        cita.estatus = estatus;

                    }

                    return View(result);

                }

            }
            catch
            {
                return View();
            }


        }

        [HttpPost]
        public JsonResult CambiarEstadoConsultaConRetardo(string IdCita)
        {
            // usar using para cerrar la conexion con seguridad.
            using (SMDEVEntities33 db = new Models.SMDEVEntities33())
            {
                // marcar bloqueado a 1 para que se bloquee en la vista
                db.Database.ExecuteSqlCommand("UPDATE CITAS SET Tipo = 13 WHERE Id = '" + IdCita + "'");
            }

            return Json("OK");
        }




        public ActionResult ConsultarAgendaCitas()
        {
            return View();
        }
        public ActionResult BsqEspMedico(string TipoBaja, string TipoEsp)
        {
            DalMisMedicos dalObj = new DalMisMedicos();


            return new JsonResult { Data = dalObj.BuscarEspMedico(TipoBaja, TipoEsp), JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public ActionResult AltaAgendaMedicos()
        {
            //if (User.IsInRole("MedicinaLaboral"))
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            DalMisMedicos dalObj = new DalMisMedicos();


            ViewData["cbohorario"] = dalObj.BuscarEspecialidades();
            ViewData["cbogpo"] = dalObj.Buscardir_medico();
            ViewData["cbouni"] = dalObj.BuscarUniversidades();

            return View();
        }



        public ActionResult Consulta_Historial()
        {
            //if (User.IsInRole("MedicinaLaboral"))
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            if (User.IsInRole("ConsultaHistCli"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Consulta_HistorialVerProcAmbOrdenes(string id)
        {

            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabientes = (from a in db.DHABIENTES
                              where a.NUMEMP == id
                              select a).FirstOrDefault();
            return View(dhabientes);

        }


        public class HistorialRecetasLista
        {
            public string medico { get; set; }
            public int? folio_rcta { get; set; }
            public string medicamento { get; set; }
            public string dosis { get; set; }
            public string indicaciones { get; set; }
            public DateTime fecha { get; set; }
            public int? cantidadsurtida { get; set; }
        }


        [HttpGet]
        public ActionResult Consulta_HistorialHistorialRecetas(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var fecha = DateTime.Now.AddYears(-2).ToString("yyyy-MM-ddT00:00:00.000");
                var fecha_correcta = DateTime.Parse(fecha);
                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == id
                                  select a).FirstOrDefault();

                Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
                Models.SERVMEDEntities4 db3 = new Models.SERVMEDEntities4();
                Models.SMDEVEntities28 db4 = new Models.SMDEVEntities28();

                var receta_resur_detalles = (from d in db3.Receta_Detalle
                                             join recetaExp in db3.Receta_Exp on d.Folio_Rcta equals recetaExp.Folio_Rcta into reExp
                                             from receExp in reExp.DefaultIfEmpty()
                                             where receExp.Expediente == id
                                             //where receExp.folio_rc == null
                                             where receExp.Fecha >= fecha_correcta
                                             select new
                                             {
                                                 folio_rcta = d.Folio_Rcta,
                                                 codigo = d.Codigo,
                                                 medico = receExp.Medico,
                                                 dosis = d.Dosis,
                                                 indicaciones = d.Indicaciones,
                                                 fecha = receExp.Fecha,
                                                 cantidadsurtida = d.CantSurtida,
                                                 //meses_surtir = receExp.meses_surtir,
                                             })
                                            .OrderByDescending(g => g.fecha)
                                            .ToList();

                //System.Diagnostics.Debug.WriteLine(receta_resur_detalles);


                var results = new List<HistorialRecetasLista>();

                foreach (var item in receta_resur_detalles)
                {
                    var inventariofarmacia = (from q in db2.InventarioFarmacia
                                                  //join sustan in db2.Sustancia on q.Clave equals sustan.Clave into susX
                                                  //from susIn in susX.DefaultIfEmpty()
                                                  //where susIn.Consultorio != "0"
                                              where q.Clave == item.codigo
                                              //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                              select new
                                              {
                                                  Clave = q.Clave,
                                                  Descripcion_Sal = q.Descripcion_Sal,
                                                  Descripcion_Grupo = q.Descripcion_Grupo,
                                                  Presentacion = q.Presentacion,
                                                  Inv_Act = q.Inv_Act,
                                              })
                                               .OrderByDescending(g => g.Descripcion_Grupo)
                                               .FirstOrDefault();

                    var medico = (from m in db4.MEDICOS
                                  where m.Numero == item.medico
                                  //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                  select new
                                  {
                                      nombre = m.Nombre + " " + m.Apaterno + " " + m.Amaterno,
                                      numero = m.Numero,
                                  })
                                   .FirstOrDefault();

                    var result = new HistorialRecetasLista
                    {
                        medico = "<h6><b>" + medico.nombre + "</b></h6><span>" + medico.numero + "</span>",
                        //folio_rcta = item.folio_rcta,
                        medicamento = "<h6><b>" + inventariofarmacia.Descripcion_Sal + "</b></h6><span>" + inventariofarmacia.Descripcion_Grupo + "</span><br><span>" + inventariofarmacia.Presentacion + "</span>",
                        dosis = item.dosis,
                        indicaciones = item.indicaciones,
                        fecha = item.fecha,
                        cantidadsurtida = item.cantidadsurtida
                    };

                    results.Add(result);
                }





                var receta_resur_detalles2 = (from d in db2.receta_detalle_crn
                                              join recetaExp in db2.receta_exp_crn on d.folio_rc equals recetaExp.folio_rc into reExp
                                              from receExp in reExp.DefaultIfEmpty()
                                              where receExp.expediente == id
                                              where receExp.fecha_crea >= fecha_correcta
                                              select new
                                              {
                                                  folio_rc = d.folio_rc,
                                                  codigo = d.codigo,
                                                  medico = receExp.medico_crea,
                                                  dosis = d.dosis,
                                                  indicaciones = d.indicaciones,
                                                  fecha = receExp.fecha_crea,
                                                  cantidadsurtida = d.cantidad,
                                                  meses_surtir = receExp.meses_surtir,
                                              })
                                         .ToList();


                foreach (var item in receta_resur_detalles2)
                {
                    Models.SERVMEDEntities4 db6 = new Models.SERVMEDEntities4();


                    var inventariofarmacia = (from q in db2.InventarioFarmacia
                                                  //join sustan in db2.Sustancia on q.Clave equals sustan.Clave into susX
                                                  //from susIn in susX.DefaultIfEmpty()
                                                  //where susIn.Consultorio != "0"
                                              where q.Clave == item.codigo
                                              //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                              select new
                                              {
                                                  Clave = q.Clave,
                                                  Descripcion_Sal = q.Descripcion_Sal,
                                                  Descripcion_Grupo = q.Descripcion_Grupo,
                                                  Presentacion = q.Presentacion,
                                                  Inv_Act = q.Inv_Act,
                                              })
                                           .OrderByDescending(g => g.Descripcion_Grupo)
                                           .FirstOrDefault();

                    var medico2 = (from m in db4.MEDICOS
                                   where m.Numero == item.medico
                                   //where q.Clave_Nivel != "2" || q.Clave_Nivel != "1"
                                   select new
                                   {
                                       nombre = m.Nombre + " " + m.Apaterno + " " + m.Amaterno,
                                       numero = m.Numero,
                                   })
                                   .FirstOrDefault();

                    var result2 = new HistorialRecetasLista
                    {
                        medico = "<h6><b>" + medico2.nombre + "</b></h6><span>" + medico2.numero + "</span>",
                        //folio_rcta = item.folio_rcta,
                        medicamento = "<h6><b>" + inventariofarmacia.Descripcion_Sal + "</b></h6><span>" + inventariofarmacia.Descripcion_Grupo + "</span><br><span>" + inventariofarmacia.Presentacion + "</span><br><span> <b> Meses a surtir:" + item.meses_surtir + "</b></span>",
                        dosis = item.dosis,
                        indicaciones = item.indicaciones,
                        fecha = item.fecha,
                        cantidadsurtida = item.cantidadsurtida
                    };

                    results.Add(result2);
                    //db.InventarioFarmacia.Add(result);
                }


                //System.Diagnostics.Debug.WriteLine(results);

                string json = JsonConvert.SerializeObject(results);
                string path = Server.MapPath("~/json/recetas/");
                System.IO.File.WriteAllText(path + "historial_recetas_" + id + ".json", json);

                return View(dhabientes);
            }

        }

        [HttpGet]
        public ActionResult Consulta_HistorialListaRecetas(string id)
        {

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                var nombreusuario = User.Identity.GetUserName();
                var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                var fecha = DateTime.Parse(fecha_string);

                var exp = (from a in db3.expediente
                               //where a.numemp == expediente
                           where a.medico == nombreusuario
                           where a.hora_termino == null
                           where a.fecha == fecha
                           select a).FirstOrDefault();

                if (exp == null)
                {
                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();
                    return View(dhabientes);
                }
                else
                {
                    TempData["numemp"] = exp.numemp;
                    return RedirectToAction("ConsultaExpediente/" + exp.numemp, "Manage");
                }
            }
        }


        [HttpGet]
        public ActionResult Consulta_HistorialPadecimientos(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
                var dhabientes = (from a in db.DHABIENTES
                                  where a.NUMEMP == id
                                  select a).FirstOrDefault();

                //Models.SMDEVEntities24 db2 = new Models.SMDEVEntities24();

                Models.SMDEVEntities28 db3 = new Models.SMDEVEntities28();

                var expediente = (from q in db3.expediente
                                  where q.numemp == id
                                  join medicos in db3.MEDICOS on q.medico equals medicos.Numero
                                  join diagnosticos1 in db3.DIAGNOSTICOS on q.diagnostico equals diagnosticos1.Clave
                                  join diagnosticos2 in db3.DIAGNOSTICOS on q.diagnostico2 equals diagnosticos2.Clave
                                  join diagnosticos3 in db3.DIAGNOSTICOS on q.diagnostico3 equals diagnosticos3.Clave
                                  join espe in db3.ESPECIALIDADES on q.medico.Substring(0, 2) equals espe.CLAVE
                                  //where q.Medico == "30001"
                                  select new
                                  {
                                      numemp = q.numemp,
                                      medico = medicos.Nombre + " " + medicos.Apaterno + " " + medicos.Amaterno,
                                      especialidad = espe.DESCRIPCION,
                                      s_exp = q.s_exp,
                                      a_exp = q.a_exp,
                                      o_exp = q.o_exp,
                                      p_exp = q.p_exp,
                                      diagnostico = diagnosticos1.DesCorta + " + " + diagnosticos2.DesCorta + " + " + diagnosticos3.DesCorta,
                                      edd_anio = q.edd_anio,
                                      edd_mes = q.edd_mes,
                                      peso_r = q.peso_r,
                                      talla_r = q.talla_r,
                                      ta = q.ta1 + "-" + q.ta2,
                                      temp_r = q.temp_r,
                                      fresp = q.fresp,
                                      fecha = q.fecha,
                                  })
                                    .OrderByDescending(g => g.fecha)
                                    .ToList();


                ViewData["expediente"] = expediente;
                string json = JsonConvert.SerializeObject(expediente);
                string path = Server.MapPath("~/json/historial_padecimientos/");
                System.IO.File.WriteAllText(path + "historial_expediente_" + id + ".json", json);

                return View(dhabientes);

            }

        }


        public ActionResult Consulta_HistorialRecetasCronicas(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("ServiciosMedicos"))
                {
                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();


                    if (res != null)
                    {
                        //Si es presencial
                        if (res.tipo == "11")
                        {
                            //Si no ha llegado el paciente a recepcion
                            if (res.hora_recepcion == null)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            //Si ya llego
                            else
                            {
                                //Se va a actualizar citas
                                var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                                if (res.hr_consultorio == null)
                                {
                                    db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = '" + hr_consultorio + "' WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                                }

                                Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                                var nombreusuario = User.Identity.GetUserName();
                                var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                                var fecha = DateTime.Parse(fecha_string);

                                var exp = (from a in db3.expediente
                                               //where a.numemp == expediente
                                           where a.medico == nombreusuario
                                           where a.hora_termino == null
                                           where a.fecha == fecha
                                           select a).FirstOrDefault();

                                if (exp == null)
                                {
                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    var especialidad = nombreusuario.Substring(0, 2);

                                    Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                                    if (especialidad == "02")
                                    {
                                        var inventariofarmacia = (from q in db2.InventarioFarmacia
                                                                  where q.Clave_Nivel == "1" || q.Clave_Nivel == "2"
                                                                  select new
                                                                  {
                                                                      Clave = q.Clave,
                                                                      Descripcion_Sal = q.Descripcion_Sal,
                                                                      Descripcion_Grupo = q.Descripcion_Grupo,
                                                                      Presentacion = q.Presentacion,
                                                                      Inv_Act = q.Inv_Act,
                                                                  })
                                                        .OrderByDescending(g => g.Descripcion_Grupo)
                                                        .ToList();

                                        string json = JsonConvert.SerializeObject(inventariofarmacia);
                                        string path = Server.MapPath("~/json/inventario_farmacia/");
                                        System.IO.File.WriteAllText(path + "inventario_farmacia_sm.json", json);
                                    }
                                    else
                                    {
                                        var inventariofarmacia = (from q in db2.InventarioFarmacia
                                                                  select new
                                                                  {
                                                                      Clave = q.Clave,
                                                                      Descripcion_Sal = q.Descripcion_Sal,
                                                                      Descripcion_Grupo = q.Descripcion_Grupo,
                                                                      Presentacion = q.Presentacion,
                                                                      Inv_Act = q.Inv_Act,
                                                                  })
                                                        .OrderByDescending(g => g.Descripcion_Grupo)
                                                        .ToList();

                                        string json = JsonConvert.SerializeObject(inventariofarmacia);
                                        string path = Server.MapPath("~/json/inventario_farmacia/");
                                        System.IO.File.WriteAllText(path + "inventario_farmacia_sm.json", json);
                                    }


                                    return View(dhabientes);
                                }
                                else
                                {
                                    TempData["numemp"] = exp.numemp;
                                    return RedirectToAction("SOAP/" + exp.numemp, "ServiciosMedicos");
                                }

                            }


                        }
                        //Si no es presencial entonces es telefonica
                        else
                        {
                            //Se va a actualizar citas
                            var hr_consultorio = DateTime.Now.ToString("HH:mm");
                            //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                            if (res.hr_consultorio == null)
                            {
                                db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = '" + hr_consultorio + "' WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                            }

                            Models.SMDEVEntities24 db3 = new Models.SMDEVEntities24();
                            var nombreusuario = User.Identity.GetUserName();
                            var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                            var fecha = DateTime.Parse(fecha_string);

                            var exp = (from a in db3.expediente
                                           //where a.numemp == expediente
                                       where a.medico == nombreusuario
                                       where a.hora_termino == null
                                       where a.fecha == fecha
                                       select a).FirstOrDefault();

                            if (exp == null)
                            {
                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                var especialidad = nombreusuario.Substring(0, 2);

                                Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                                if (especialidad == "02")
                                {
                                    var inventariofarmacia = (from q in db2.InventarioFarmacia
                                                              where q.Clave_Nivel == "1" || q.Clave_Nivel == "2"
                                                              select new
                                                              {
                                                                  Clave = q.Clave,
                                                                  Descripcion_Sal = q.Descripcion_Sal,
                                                                  Descripcion_Grupo = q.Descripcion_Grupo,
                                                                  Presentacion = q.Presentacion,
                                                                  Inv_Act = q.Inv_Act,
                                                              })
                                                    .OrderByDescending(g => g.Descripcion_Grupo)
                                                    .ToList();

                                    string json = JsonConvert.SerializeObject(inventariofarmacia);
                                    string path = Server.MapPath("~/json/inventario_farmacia/");
                                    System.IO.File.WriteAllText(path + "inventario_farmacia_sm.json", json);
                                }
                                else
                                {
                                    var inventariofarmacia = (from q in db2.InventarioFarmacia
                                                              select new
                                                              {
                                                                  Clave = q.Clave,
                                                                  Descripcion_Sal = q.Descripcion_Sal,
                                                                  Descripcion_Grupo = q.Descripcion_Grupo,
                                                                  Presentacion = q.Presentacion,
                                                                  Inv_Act = q.Inv_Act,
                                                              })
                                                    .OrderByDescending(g => g.Descripcion_Grupo)
                                                    .ToList();

                                    string json = JsonConvert.SerializeObject(inventariofarmacia);
                                    string path = Server.MapPath("~/json/inventario_farmacia/");
                                    System.IO.File.WriteAllText(path + "inventario_farmacia_sm.json", json);
                                }


                                return View(dhabientes);
                            }
                            else
                            {
                                TempData["numemp"] = exp.numemp;
                                return RedirectToAction("SOAP/" + exp.numemp, "ServiciosMedicos");
                            }

                        }

                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }
                else
                {
                    if (User.IsInRole("SinAgenda"))
                    {

                        Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                        var dhabientes = (from a in db.DHABIENTES
                                          where a.NUMEMP == id
                                          select a).FirstOrDefault();


                        Models.SMDEVEntities25 db2 = new Models.SMDEVEntities25();

                        var inventariofarmacia = (from q in db2.InventarioFarmacia
                                                  select new
                                                  {
                                                      Clave = q.Clave,
                                                      Descripcion_Sal = q.Descripcion_Sal,
                                                      Descripcion_Grupo = q.Descripcion_Grupo,
                                                      Presentacion = q.Presentacion,
                                                      Inv_Act = q.Inv_Act,
                                                  })
                                                  .OrderByDescending(g => g.Descripcion_Grupo)
                                                  .ToList();

                        string json = JsonConvert.SerializeObject(inventariofarmacia);
                        string path = Server.MapPath("~/json/inventario_farmacia/");
                        System.IO.File.WriteAllText(path + "inventario_farmacia_sm.json", json);



                        return View(dhabientes);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }

                }

            }
        }


        public ActionResult Consulta_HistorialHojaFrontal(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            DalHojaFrontal hf = new DalHojaFrontal();
            HojaFrontal hoja = hf.BuscarTitular(id);

            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabientes = (from a in db.DHABIENTES
                              where a.NUMEMP == id
                              select a).FirstOrDefault();

            ViewData["hoja"] = hoja;
            return View(dhabientes);
        }
        public ActionResult MisMedicos()
        {
            //if (User.IsInRole("MedicinaLaboral"))
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            return View();
        }

        public ActionResult Especialidades_directorio()
        {
            DalMisMedicos dalObj = new DalMisMedicos();

            List<Combos> lista = new List<Combos>();
            lista = dalObj.BuscarEspecialidades();
            ViewData["especialidades"] = lista;
            //if (User.IsInRole("MedicinaLaboral"))
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            return View();
        }

        public ActionResult Medicos_directorio()
        {
            DalMisMedicos dalObj = new DalMisMedicos();

            //List<Combos> lista = new List<Combos>();
            //lista = dalObj.BuscarEspecialidades();
            //ViewData["especialidades"] = lista;
            //if (User.IsInRole("MedicinaLaboral"))
            //{
            //    return View();
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            return View();
        }


        public ActionResult HojaFrontal(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("MedicinaLaboral"))
                {
                    DalHojaFrontal hf = new DalHojaFrontal();
                    HojaFrontal hoja = hf.BuscarTitular(id);

                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();

                    ViewData["hoja"] = hoja;
                    return View(dhabientes);

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

        }

        public ActionResult HistorialPadecimientos(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {

                if (User.IsInRole("MedicinaLaboral"))
                {

                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();

                    //Models.SMDEVEntities24 db2 = new Models.SMDEVEntities24();

                    Models.SMDEVEntities28 db3 = new Models.SMDEVEntities28();

                    var expediente = (from q in db3.expediente
                                      where q.numemp == id
                                      join medicos in db3.MEDICOS on q.medico equals medicos.Numero
                                      join diagnosticos1 in db3.DIAGNOSTICOS on q.diagnostico equals diagnosticos1.Clave
                                      join diagnosticos2 in db3.DIAGNOSTICOS on q.diagnostico2 equals diagnosticos2.Clave
                                      join diagnosticos3 in db3.DIAGNOSTICOS on q.diagnostico3 equals diagnosticos3.Clave
                                      join espe in db3.ESPECIALIDADES on q.medico.Substring(0, 2) equals espe.CLAVE
                                      //where q.Medico == "30001"
                                      select new
                                      {
                                          numemp = q.numemp,
                                          medico = medicos.Nombre + " " + medicos.Apaterno + " " + medicos.Amaterno,
                                          especialidad = espe.DESCRIPCION,
                                          s_exp = q.s_exp,
                                          a_exp = q.a_exp,
                                          o_exp = q.o_exp,
                                          p_exp = q.p_exp,
                                          diagnostico = diagnosticos1.DesCorta + " + " + diagnosticos2.DesCorta + " + " + diagnosticos3.DesCorta,
                                          edd_anio = q.edd_anio,
                                          edd_mes = q.edd_mes,
                                          peso_r = q.peso_r,
                                          talla_r = q.talla_r,
                                          ta = q.ta1 + "-" + q.ta2,
                                          temp_r = q.temp_r,
                                          fresp = q.fresp,
                                          fecha = q.fecha,
                                      })
                                        .OrderByDescending(g => g.fecha)
                                        .ToList();


                    ViewData["expediente"] = expediente;
                    string json = JsonConvert.SerializeObject(expediente);
                    string path = Server.MapPath("~/json/historial_padecimientos/");
                    System.IO.File.WriteAllText(path + "historial_expediente_" + id + ".json", json);

                    return View(dhabientes);

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }


            }

        }

        public ActionResult SOAP(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (User.IsInRole("MedicinaLaboral"))
                {
                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                    var dhabientes = (from a in db.DHABIENTES
                                      where a.NUMEMP == id
                                      select a).FirstOrDefault();

                    Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                    var diagnosticos = (from q in db2.DIAGNOSTICOS
                                        where q.DescCompleta != null
                                        where q.Clave != null
                                        where q.Clave != ""
                                        select new
                                        {
                                            CheckBox = "",
                                            Clave = q.Clave,
                                            DescCompleta = q.DescCompleta,
                                            DesCorta = q.DesCorta
                                        })
                            .ToList();

                    string json = JsonConvert.SerializeObject(diagnosticos);
                    string path = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(path + "diagnosticos.json", json);


                    Models.SMDEVEntities18 db5 = new Models.SMDEVEntities18();

                    var especialidades = (from q in db5.ESPECIALIDADES
                                          select new
                                          {
                                              clave = q.CLAVE,
                                              descripcion = q.DESCRIPCION,
                                          })
                                        .ToList();

                    string jsonEsp = JsonConvert.SerializeObject(especialidades);
                    string pathEsp = Server.MapPath("~/json/");
                    System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                    return View(dhabientes);

                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
        }


        [HttpPost]
        public ActionResult GuardarSOAP(expediente model, bool interCheck, bool urgenciaCheck)
        {

            //System.Diagnostics.Debug.WriteLine(interCheck);

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var ip_realiza = GetLocalIPAddress();

            Models.SMDEVEntities24 db = new Models.SMDEVEntities24();
            var expCount = (from a in db.expediente
                            where a.numemp == model.numemp
                            where a.medico == model.medico
                            where a.fecha == fecha_correcta
                            //where a.hora_termino == null
                            select a).Count();

            if (expCount > 0)
            {
                //Si se hace un referido a un especialista
                if (interCheck == true)
                {
                    //System.Diagnostics.Debug.WriteLine(urgenciaCheck);
                    //SI ES URGENTE
                    if (urgenciaCheck == true)
                    {
                        db.Database.ExecuteSqlCommand("UPDATE expediente SET numemp = '" + model.numemp + "', medico = '" + model.medico + "', diagnostico = '" + model.diagnostico + "', diagnostico2 = '" + model.diagnostico2 + "', diagnostico3 = '" + model.diagnostico3 + "', tipconsulta = '" + model.tipconsulta + "', tipconsulta2 = '" + model.tipconsulta2 + "', tipconsulta3 = '" + model.tipconsulta3 + "', s_exp = '" + model.s_exp + "', o_exp = '" + model.o_exp + "', p_exp = '" + model.p_exp + "', fecha = '" + fecha + "', peso_r = '" + model.peso_r + "', talla_r = '" + model.talla_r + "', temp_r = '" + model.temp_r + "', fresp = '" + model.fresp + "', fcard = '" + model.fcard + "', ta1 = '" + model.ta1 + "', ta2 = '" + model.ta2 + "', dstx = '" + model.dstx + "', ip_realiza = '" + ip_realiza + "', referido = '" + model.referido + "', ref_exp = '" + model.ref_exp + "', referido_urg = '1' WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and fecha = '" + fecha + "'");
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("UPDATE expediente SET numemp = '" + model.numemp + "', medico = '" + model.medico + "', diagnostico = '" + model.diagnostico + "', diagnostico2 = '" + model.diagnostico2 + "', diagnostico3 = '" + model.diagnostico3 + "', tipconsulta = '" + model.tipconsulta + "', tipconsulta2 = '" + model.tipconsulta2 + "', tipconsulta3 = '" + model.tipconsulta3 + "', s_exp = '" + model.s_exp + "', o_exp = '" + model.o_exp + "', p_exp = '" + model.p_exp + "', fecha = '" + fecha + "', peso_r = '" + model.peso_r + "', talla_r = '" + model.talla_r + "', temp_r = '" + model.temp_r + "', fresp = '" + model.fresp + "', fcard = '" + model.fcard + "', ta1 = '" + model.ta1 + "', ta2 = '" + model.ta2 + "', dstx = '" + model.dstx + "', ip_realiza = '" + ip_realiza + "', referido = '" + model.referido + "', ref_exp = '" + model.ref_exp + "', referido_urg = NULL WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and fecha = '" + fecha + "'");
                    }
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE expediente SET numemp = '" + model.numemp + "', medico = '" + model.medico + "', diagnostico = '" + model.diagnostico + "', diagnostico2 = '" + model.diagnostico2 + "', diagnostico3 = '" + model.diagnostico3 + "', tipconsulta = '" + model.tipconsulta + "', tipconsulta2 = '" + model.tipconsulta2 + "', tipconsulta3 = '" + model.tipconsulta3 + "', s_exp = '" + model.s_exp + "', o_exp = '" + model.o_exp + "', p_exp = '" + model.p_exp + "', fecha = '" + fecha + "', peso_r = '" + model.peso_r + "', talla_r = '" + model.talla_r + "', temp_r = '" + model.temp_r + "', fresp = '" + model.fresp + "', fcard = '" + model.fcard + "', ta1 = '" + model.ta1 + "', ta2 = '" + model.ta2 + "', dstx = '" + model.dstx + "', ip_realiza = '" + ip_realiza + "', referido = null, ref_exp = null WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and fecha = '" + fecha + "'");
                }
            }
            else
            {
                //Si se hace un referido a un especialista
                if (interCheck == true)
                {
                    if (urgenciaCheck == true)
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, diagnostico, diagnostico2, diagnostico3, tipconsulta, tipconsulta2, tipconsulta3,  s_exp, o_exp, p_exp, fecha, peso_r, talla_r, temp_r, fresp, fcard, ta1, ta2, dstx, ip_realiza, consultadistancia, referido, ref_exp, referido_urg) VALUES('" + model.numemp + "', '" + model.medico + "', '" + model.diagnostico + "', '" + model.diagnostico2 + "', '" + model.diagnostico3 + "', '" + model.tipconsulta + "', '" + model.tipconsulta2 + "', '" + model.tipconsulta3 + "', '" + model.s_exp + "', '" + model.o_exp + "', '" + model.p_exp + "', '" + fecha + "', '" + model.peso_r + "', '" + model.talla_r + "', '" + model.temp_r + "', '" + model.fresp + "', '" + model.fcard + "', '" + model.ta1 + "', '" + model.ta2 + "', '" + model.dstx + "', '" + ip_realiza + "', '0', '" + model.referido + "', '" + model.ref_exp + "','1')");
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, diagnostico, diagnostico2, diagnostico3, tipconsulta, tipconsulta2, tipconsulta3,  s_exp, o_exp, p_exp, fecha, peso_r, talla_r, temp_r, fresp, fcard, ta1, ta2, dstx, ip_realiza, consultadistancia, referido, ref_exp) VALUES('" + model.numemp + "', '" + model.medico + "', '" + model.diagnostico + "', '" + model.diagnostico2 + "', '" + model.diagnostico3 + "', '" + model.tipconsulta + "', '" + model.tipconsulta2 + "', '" + model.tipconsulta3 + "', '" + model.s_exp + "', '" + model.o_exp + "', '" + model.p_exp + "', '" + fecha + "', '" + model.peso_r + "', '" + model.talla_r + "', '" + model.temp_r + "', '" + model.fresp + "', '" + model.fcard + "', '" + model.ta1 + "', '" + model.ta2 + "', '" + model.dstx + "', '" + ip_realiza + "', '0', '" + model.referido + "', '" + model.ref_exp + "')");
                    }
                }
                else
                {
                    db.Database.ExecuteSqlCommand("INSERT INTO expediente (numemp, medico, diagnostico, diagnostico2, diagnostico3, tipconsulta, tipconsulta2, tipconsulta3,  s_exp, o_exp, p_exp, fecha, peso_r, talla_r, temp_r, fresp, fcard, ta1, ta2, dstx, ip_realiza, consultadistancia) VALUES('" + model.numemp + "', '" + model.medico + "', '" + model.diagnostico + "', '" + model.diagnostico2 + "', '" + model.diagnostico3 + "', '" + model.tipconsulta + "', '" + model.tipconsulta2 + "', '" + model.tipconsulta3 + "', '" + model.s_exp + "', '" + model.o_exp + "', '" + model.p_exp + "', '" + fecha + "', '" + model.peso_r + "', '" + model.talla_r + "', '" + model.temp_r + "', '" + model.fresp + "', '" + model.fcard + "', '" + model.ta1 + "', '" + model.ta2 + "', '" + model.dstx + "', '" + ip_realiza + "', '0')");
                }
            }

            if (model.p_exp == null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                var fecha_termina = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");

                db.Database.ExecuteSqlCommand("UPDATE expediente SET hora_termino = '" + fecha_termina + "' WHERE medico = '" + model.medico + "' and numemp = '" + model.numemp + "' and fecha = '" + fecha + "'");

                TempData["message_success"] = "Nota médica terminada con éxito";
                return Redirect(Request.UrlReferrer.ToString());
            }

            //return Redirect(Request.UrlReferrer.ToString());
        }


        public static string GetLocalIPAddress()
        {
            string strHostName = "";
            strHostName = Dns.GetHostName();

            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            return addr[addr.Length - 1].ToString();
        }


    }
}