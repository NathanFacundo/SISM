using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class CitasController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        // GET: Citas
        public ActionResult Index()
        {
            if (User.IsInRole("CitasExternas"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult Canceladas()
        {
            return View();
        }


        public JsonResult TipoConsultas()
        {
            //Models.SERVMEDEntities2 db = new Models.SERVMEDEntities2();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var tipoConsulta = (from d in db2.TIPOSCONSULTA
                                where d.Clave == "00" || d.Clave == "01" || d.Clave == "02"
                                select new
                                {
                                    Clave = d.Clave,
                                    Descripcion = d.Descripcion,
                                })
                                .ToList();

            //System.Diagnostics.Debug.WriteLine(tipoConsulta);

            return new JsonResult { Data = tipoConsulta, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        public JsonResult GetTelefonoConsulta(string numemp)
        {
            //Models.SERVMEDEntities2 db = new Models.SERVMEDEntities2();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var dhabiente = (from d in db2.DHABIENTES
                             where d.NUMEMP == numemp
                             select d).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(tipoConsulta);

            return new JsonResult { Data = dhabiente, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        public JsonResult MisMedicos()
        {
            var nombreusuario = User.Identity.GetUserName();

            //Models.SERVMEDEntities2 db = new Models.SERVMEDEntities2();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var mismedicos = (from d in db.Citas_Medicos_Externos
                              join medicos in db.MEDICOS on d.medico equals medicos.Numero into medicosX
                              from medicosIn in medicosX.DefaultIfEmpty()
                              where d.usuario == nombreusuario
                              select new
                              {
                                  Medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                  Numero = medicosIn.Numero,
                              })
                              .ToList();

            //System.Diagnostics.Debug.WriteLine(tipoConsulta);

            return new JsonResult { Data = mismedicos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        [HttpPost]
        public ActionResult GuardarCitas(Citas model)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var turno = model.turno;
            var turnoSplit = turno.Split(':');
            var turnoFinal = turnoSplit[0] + turnoSplit[1];

            string query = "select NumEmp as NumEmp, Medico as Medico, Fecha as start from CITAS WHERE Medico = '" + model.Medico + "' and Fecha = '" + model.Fecha + "T00:00:00' and turno = '" + turnoFinal + "' and Tipo != '00'";
            var result = db.Database.SqlQuery<Citas>(query);
            var res = result.FirstOrDefault();

            //Revisar cuantas citas lleva hechas el usuario hoy
            var nombreusuario = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            //var fechaDate = DateTime.Parse(fecha);
            string query2 = "select FRealizo as frealizo, EmpRealizo as emprealizo from CITAS WHERE EmpRealizo = '" + nombreusuario + "' and FRealizo = '" + fecha + "' ";
            var result2 = db.Database.SqlQuery<Citas>(query2);
            var res2 = result2.Count();
            var countRes2 = res2 + 1;

            //System.Diagnostics.Debug.WriteLine(model.telefono);

            //Si no existe una Cita, agregarla
            if (res == null && model.modificar == 0)
            {
                var EmpRealizo = User.Identity.GetUserName();

                //var fecha_string = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                //var fecha = DateTime.Parse(fecha_string);

                db.Database.ExecuteSqlCommand("INSERT INTO Citas (NumEmp, Medico, Fecha, turno, Tipo, EmpRealizo, FRealizo, REGXDIA) VALUES('" + model.NumEmp + "', '" + model.Medico + "', '" + model.Fecha + "T00:00:00', '" + turnoFinal + "', '" + model.tipo + "', '" + EmpRealizo + "' , '" + fecha + "', '" + countRes2 + "')");

                //ACTUALIZAR TELEFONO DE DHABIENTES
                db.Database.ExecuteSqlCommand("Update DHABIENTES SET tel_consulta = '" + model.telefono + "' WHERE NumEmp = '" + model.NumEmp + "'");

                TempData["message_success"] = "Cita agendada con éxito";

                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                //Si ya existe
                if (model.modificar == 0)
                {
                    //Hacer lo de si esta canclada o no
                    //Mostrar que ya hay una cita
                    TempData["message_warning"] = "Ya existe una cita agendada en este horario";
                }
                else
                {
                    //Si ya existe pero quieres editarlo
                    //Toma el usuario, el regxdia, el numemp y el frealizo
                    db.Database.ExecuteSqlCommand("Update CITAS SET Fecha = '" + model.Fecha + "T00:00:00', Turno = '" + turnoFinal + "', Tipo = '" + model.tipo + "', Medico = '" + model.Medico + "' WHERE NumEmp = '" + model.NumEmp + "' and REGXDIA = '" + model.regxdia + "' and FRealizo = '" + fecha + "' and EmpRealizo = '" + nombreusuario + "'");

                    //ACTUALIZAR TELEFONO DE DHABIENTES
                    db.Database.ExecuteSqlCommand("Update DHABIENTES SET tel_consulta = '" + model.telefono + "' WHERE NumEmp = '" + model.NumEmp + "'");

                    TempData["message_success"] = "Cita editada con éxito";
                }


                return Redirect(Request.UrlReferrer.ToString());
            }
        }


        public class MisCitasList
        {
            public string NumEmp { get; set; }
            public string Paciente { get; set; }
            public string Numero_Medico { get; set; }
            public string Medico { get; set; }
            public string tipo { get; set; }
            public string Estatus { get; set; }
            public string Recetas { get; set; }
            public string Fecha { get; set; }
        }


        public JsonResult MisCitas()
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);
            //Models.SERVMEDEntities2 db = new Models.SERVMEDEntities2();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var EmpRealizo = User.Identity.GetUserName();

            string query = "select CITAS.Medico as Numero_Medico, CITAS.NumEmp as NumEmp, CITAS.Fecha as start, CITAS.REGXDIA as regxdia, DHABIENTES.NOMBRES as Nombres, DHABIENTES.APATERNO as Apaterno, DHABIENTES.AMATERNO as Amaterno, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as Medico, TIPOSCONSULTA.Descripcion as tipo, CITAS.hr_consultorio as hr_consultorio, CASE WHEN len(DHABIENTES.tel_consulta) > 0 THEN '52' + DHABIENTES.tel_consulta ELSE '52' + AFILIADOS.tel_celular END AS telefono from CITAS INNER JOIN MEDICOS ON MEDICOS.Numero = CITAS.Medico INNER JOIN DHABIENTES ON DHABIENTES.NUMEMP = CITAS.NumEmp INNER JOIN TIPOSCONSULTA ON TIPOSCONSULTA.Clave = CITAS.Tipo INNER JOIN AFILIADOS ON AFILIADOS.num_contrato = DHABIENTES.NUMAFIL where CITAS.EmpRealizo = '" + EmpRealizo + "' ORDER BY Fecha DESC";
            var result = db.Database.SqlQuery<Citas>(query);
            var miscitas = result.ToList();

            var results1 = new List<MisCitasList>();

            foreach (var cita in miscitas)
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

                var resultado = new MisCitasList
                {
                    Numero_Medico = cita.Numero_Medico + "*" + cita.NumEmp + "*" + cita.regxdia + "*" + string.Format("{0:yyyy-MM-ddT00:00:00.000}", cita.start),
                    Paciente = cita.Nombres + " " + cita.Apaterno + " " + cita.Amaterno + "<br><span><b>" + cita.telefono + "</b></span>",
                    Medico = cita.Medico,
                    tipo = cita.tipo,
                    Estatus = estatus,
                    Fecha = string.Format("{0:yyyy-MM-dd}", cita.start),
                    //Recetas = cita.Numero_Medico,
                    //Recetas = "",
                };

                results1.Add(resultado);

            }

            //System.Diagnostics.Debug.WriteLine(tipoConsulta);

            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        public JsonResult GetCita(string medico, string numemp, int regxdia, string fecha)
        {
            //var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var EmpRealizo = User.Identity.GetUserName();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            string query = "select CITAS.Fecha as start, CITAS.Medico as Numero_Medico, CITAS.NumEmp as NumEmp, CITAS.REGXDIA as regxdia, DHABIENTES.NOMBRES as Nombres, DHABIENTES.APATERNO as Apaterno, DHABIENTES.AMATERNO as Amaterno, DHABIENTES.SEXO as sexo, DHABIENTES.FNAC as fnac, MEDICOS.Nombre+' '+MEDICOS.Apaterno+' '+MEDICOS.Amaterno as Medico, TIPOSCONSULTA.Clave as tipo, CITAS.turno as turno from CITAS INNER JOIN MEDICOS ON MEDICOS.Numero = CITAS.Medico INNER JOIN DHABIENTES ON DHABIENTES.NUMEMP = CITAS.NumEmp INNER JOIN TIPOSCONSULTA ON TIPOSCONSULTA.Clave=CITAS.Tipo where CITAS.NumEmp = '" + numemp + "' and CITAS.Medico = '" + medico + "' and CITAS.EmpRealizo = '" + EmpRealizo + "' and CITAS.FRealizo = '" + fecha + "'  and CITAS.REGXDIA = '" + regxdia + "'";
            var result = db.Database.SqlQuery<Citas>(query);
            var cita = result.FirstOrDefault();

            var dhabiente = (from r in db.DHABIENTES
                             where r.NUMEMP == numemp
                             select new
                             {
                                 telefono = r.tel_consulta
                             }).FirstOrDefault();


            DateTime now = DateTime.Today;
            int edad = now.Year - cita.fnac.Year;
            //Mes de cumple menos mes ahorita 12 - resta
            //System.Diagnostics.Debug.WriteLine(cita.start.Date.ToString());
            var fechaCita = cita.start.Date.ToString();
            var fechaCitaSplit1 = fechaCita.Split(' ');
            var fechaCitaSplit2 = fechaCitaSplit1[0].Split('/');
            //System.Diagnostics.Debug.WriteLine(fechaCitaSplit1[0]);

            var resultado = new
            {
                medico = cita.Medico,
                paciente = cita.Nombres + " " + cita.Apaterno + " " + cita.Amaterno,
                edad = edad,
                sexo = cita.sexo,

                turno = cita.turno,
                tipo = cita.tipo,
                fecha = fechaCitaSplit2[2] + "-" + fechaCitaSplit2[1] + "-" + fechaCitaSplit2[0],
                regxdia = cita.regxdia,
                telefono = dhabiente.telefono,
            };

            //System.Diagnostics.Debug.WriteLine(cita.fnac);

            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public class RecetaLista
        {
            public string medico { get; set; }
            public string paciente { get; set; }
            public string especialidad { get; set; }
            public string expediente { get; set; }
            public string fecha { get; set; }
            public int Folio_Rcta { get; set; }
            public string Estado { get; set; }
            public string cedula { get; set; }
            public string cedula_esp { get; set; }
        }


        public JsonResult GetReceta(string medico, string numemp, string fecha)
        {
            //var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var recetas = (from r in db.RECETA_EXP_2
                           join medi in db.MEDICOS on r.Medico equals medi.Numero into medicoX
                           from medicoIn in medicoX.DefaultIfEmpty()
                           join dhab in db.DHABIENTES on r.Expediente equals dhab.NUMEMP into dhabX
                           from dhabIn in dhabX.DefaultIfEmpty()
                           join espe in db.ESPECIALIDADES on medicoIn.Numero.Substring(0, 2) equals espe.CLAVE into espeX
                           from espeIn in espeX.DefaultIfEmpty()
                               //join afi in db.AFILIADOS on dhabIn.NUMAFIL equals afi.num_contrato into afiX
                               //from afiIn in afiX.DefaultIfEmpty()
                           where r.Expediente == numemp
                           where r.Medico == medico
                           where r.Fecha == fecha_correcta
                           select new
                           {
                               medico = medicoIn.Nombre + " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                               //paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO + "<br><span><b>"+ afiIn.TELEFONOS + "</b></span>",
                               paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                               expediente = dhabIn.NUMEMP,
                               especialidad = espeIn.DESCRIPCION,
                               fecha = r.Fecha,
                               Folio_Rcta = r.Folio_Rcta,
                               Estado = r.Estado,
                               cedula = medicoIn.Cedula,
                               cedula_esp = medicoIn.cedula_esp
                           }).ToList();

            //System.Diagnostics.Debug.WriteLine(recetas);

            var resultadoRes = new List<RecetaLista>();


            foreach (var receta in recetas)
            {
                //Buscar si ya tiene registrada una consulta para ver si ya termino la consulta o esta en proceso

                var resultado = new RecetaLista
                {
                    medico = receta.medico,
                    paciente = receta.paciente,
                    expediente = receta.expediente,
                    especialidad = receta.especialidad,
                    fecha = string.Format("{0:yyyy-MM-ddT00:00:00.000}", receta.fecha),
                    Folio_Rcta = receta.Folio_Rcta,
                    Estado = receta.Estado,
                    cedula = receta.cedula,
                    cedula_esp = receta.cedula_esp,

                    //Recetas = cita.Numero_Medico,
                    //Recetas = "",
                };

                resultadoRes.Add(resultado);

            }

            //System.Diagnostics.Debug.WriteLine(resultadoRes);


            /*var recetas = (from r in db.Receta_Detalle_2
                          join receta in db.RECETA_EXP_2 on r.Folio_Rcta equals receta.Folio_Rcta into recetaX
                          from recetaIn in recetaX.DefaultIfEmpty()
                          join medi in db.MEDICOS on recetaIn.Medico equals medi.Numero into medicoX
                          from medicoIn in medicoX.DefaultIfEmpty()
                          join dhab in db.DHABIENTES on recetaIn.Expediente equals dhab.NUMEMP into dhabX
                          from dhabIn in dhabX.DefaultIfEmpty()
                          join inven in db.InventarioFarmacia on r.Codigo equals inven.Clave into invenX
                          from invenIn in invenX.DefaultIfEmpty()
                          join espe in db.ESPECIALIDADES on medicoIn.Numero.Substring(0, 2) equals espe.CLAVE into espeX
                          from espeIn in espeX.DefaultIfEmpty()
                          where dhabIn.NUMEMP == numemp
                          where medicoIn.Numero == medico
                          where recetaIn.Fecha == fecha_correcta
                           //where q.folio_rc == null
                           select new
                          {
                               //medico = medicoIn.Nombre+ " " + medicoIn.Apaterno + " " + medicoIn.Amaterno,
                               //paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                               //especialidad = espeIn.DESCRIPCION,
                               fecha = recetaIn.Fecha.ToString(),
                               folio_rcta = recetaIn.Folio_Rcta,
                               //codigo = r.Codigo,
                               //medicamento = invenIn.Descripcion_Sal + ", " + invenIn.Presentacion,
                               //dosis = r.Dosis,
                               //cedula = medicoIn.Cedula,
                               //cedula_esp = medicoIn.cedula_esp,
                           }).ToList();*/


            //System.Diagnostics.Debug.WriteLine(recetas);


            return new JsonResult { Data = resultadoRes, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult GetRecetaDetalles(int folio_rcta)
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var recetas = (from r in db.Receta_Detalle_2
                           where r.Folio_Rcta == folio_rcta
                           join inven in db.InventarioFarmacia on r.Codigo equals inven.Clave into invenX
                           from invenIn in invenX.DefaultIfEmpty()
                           select new
                           {
                               medicamento = "<b style='padding-top:5px;'>" + invenIn.Descripcion_Sal + ", " + invenIn.Presentacion + "</b><br><span>" + r.Dosis + "</span><br><span>" + r.Indicaciones + "</span><br>",
                           }).ToList();

            return new JsonResult { Data = recetas, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Laboratorio()
        {
            return View();
        }

        public class Calendario
        {
            public string title { get; set; }
            public string url { get; set; }
            public string start { get; set; }
            public string end { get; set; }
            public string color { get; set; }
            public string className { get; set; }
            public string textColor { get; set; }
            public string description { get; set; }

        }

        public JsonResult CalendarioLaboratorio()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            //var username = User.Identity.GetUserName();

            //string query = "select Fecha as start from CITAS WHERE Fecha >= '" + fecha + "'";
            string query = "select CITAS.Fecha as start, CITAS.NumEmp as NumEmp, CITAS.REGXDIA as regxdia, DHABIENTES.NOMBRES as Nombres, DHABIENTES.APATERNO as Apaterno, DHABIENTES.AMATERNO as Amaterno, DHABIENTES.SEXO as sexo, DHABIENTES.FNAC as fnac, TIPOSCONSULTA.Clave as tipo, CITAS.turno as turno from CITAS INNER JOIN DHABIENTES ON DHABIENTES.NUMEMP = CITAS.NumEmp INNER JOIN TIPOSCONSULTA ON TIPOSCONSULTA.Clave=CITAS.Tipo where CITAS.Fecha >= '" + fecha + "' and CITAS.Tipo = '39' ";
            var result = db.Database.SqlQuery<Citas>(query);
            var res = result.ToList();


            var results1 = new List<Calendario>();

            foreach (var cita in res)
            {

                var hora = cita.turno.Substring(0, 2);
                var minuto = cita.turno.Substring(2, 2);



                //Agregar una hora al turno
                var fechaCita = string.Format("{0:yyyy-MM-dd}", cita.start);
                var fechaCitaFt = DateTime.Now.ToString(fechaCita + "T" + hora + ":" + minuto + ":00.000");
                var fechaCitaDT = DateTime.Parse(fechaCitaFt);
                DateTime x1horadespues = fechaCitaDT.AddHours(1);
                var turnoFinalizado = string.Format("{0:yyyy-MM-ddThh:mm:00.000}", x1horadespues);
                //System.Diagnostics.Debug.WriteLine(turnoFinalizado);


                //var horaFinalizada = turnoFinalizado.Substring(0, 2);
                //var minutoFinalizada = turnoFinalizado.Substring(2, 2);
                var color = "";
                if (hora == "06")
                {
                    color = "#72b575";
                }
                else
                {
                    color = "#ed857e";
                }

                var resultado = new Calendario
                {
                    title = cita.Nombres + " " + cita.Apaterno + " " + cita.Amaterno,
                    //url para editar o cancelar
                    //url = "/ServiciosMedicos/SOAP/" + cita.NumEmp,
                    start = string.Format("{0:yyyy-MM-ddT" + hora + ":" + minuto + ":00.000}", cita.start),
                    end = turnoFinalizado,
                    color = color,
                    textColor = "white"
                };

                results1.Add(resultado);

            }


            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        public JsonResult ContarCitasxDia()
        {
            //Contar citas de hoy hasta dentro de 2 meses (?)
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
            var fecha_correcta = DateTime.Parse(fecha);
            var fechax2m = DateTime.Now.AddMonths(1).ToString("yyyy-MM-ddT00:00:00");
            var fechax2m_correcta = DateTime.Parse(fechax2m);


            string query = "SELECT Dertbl.fecha as start FROM( SELECT Fecha  FROM CITAS WHERE((DATEPART(dw, Fecha) + @@DATEFIRST) % 7) NOT IN(0, 1) and Fecha >= '" + fecha + "' and Fecha <= '" + fechax2m + "') AS Dertbl GROUP BY Dertbl.Fecha";
            var result = db.Database.SqlQuery<Citas>(query);
            var res = result.ToList();

            var results1 = new List<Calendario>();

            foreach (var cita in res)
            {
                var fechaCita = string.Format("{0:yyyy-MM-ddT00:00:00}", cita.start);
                //System.Diagnostics.Debug.WriteLine(fechaCita);


                //CITAS DE LAS 06:30
                string querySeis = "SELECT CITAS.Fecha as start FROM CITAS WHERE Fecha = '" + fechaCita + "' and Tipo = '39' AND turno = '0630'";
                var resultSeis = db.Database.SqlQuery<Citas>(querySeis);
                var resSeis = resultSeis.Count();

                //Todas las fechas estan disponibles
                if (resSeis == 0)
                {
                    var resultado = new Calendario
                    {
                        title = "Agendar (Todos los espacios disponibles)",
                        //url para agregar
                        //url = "/ServiciosMedicos/SOAP/",
                        start = string.Format("{0:yyyy-MM-ddT06:30:00}", cita.start),
                        className = "agendar",
                        description = "Lecture",
                        //html = "<i>some html</i>"
                    };

                    results1.Add(resultado);
                }
                else
                {
                    var restantes = 70 - resSeis;
                    if (restantes == 0)
                    {
                        var resultado = new Calendario
                        {
                            title = "Sin espacios disponibles",
                            //url para agregar
                            //url = "/ServiciosMedicos/SOAP/",
                            start = string.Format("{0:yyyy-MM-ddT06:30:00}", cita.start),
                            className = "noagendar",
                            description = "Lecture",
                        };
                        results1.Add(resultado);
                    }
                    else
                    {
                        var resultado = new Calendario
                        {
                            title = "Agendar (" + restantes + " espacios disponibles)",
                            //url para agregar
                            //url = "/ServiciosMedicos/SOAP/",
                            start = string.Format("{0:yyyy-MM-ddT06:30:00}", cita.start),
                            className = "agendar",
                        };

                        results1.Add(resultado);
                    }

                }




                //CITAS DE LAS 07:30
                string querySiete = "SELECT CITAS.Fecha as start FROM CITAS WHERE Fecha = '" + fechaCita + "' and Tipo = '39' AND turno = '0730'";
                var resultSiete = db.Database.SqlQuery<Citas>(querySiete);
                var resSiete = resultSiete.Count();

                //Todas las fechas estan disponibles
                if (resSiete == 0)
                {
                    var resultado = new Calendario
                    {
                        title = "Agendar (Todos los espacios disponibles)",
                        //url para agregar
                        //url = "/ServiciosMedicos/SOAP/",
                        start = string.Format("{0:yyyy-MM-ddT07:30:00}", cita.start),
                        className = "agendar",
                        description = "Lecture",
                        //html = "<i>some html</i>"
                    };

                    results1.Add(resultado);
                }
                else
                {
                    var restantes = 70 - resSiete;
                    if (restantes == 0)
                    {
                        var resultado = new Calendario
                        {
                            title = "Sin espacios disponibles",
                            //url para agregar
                            //url = "/ServiciosMedicos/SOAP/",
                            start = string.Format("{0:yyyy-MM-ddT07:30:00}", cita.start),
                            className = "noagendar",
                            description = "Lecture",
                        };
                        results1.Add(resultado);
                    }
                    else
                    {
                        var resultado = new Calendario
                        {
                            title = "Agendar (" + restantes + " espacios disponibles)",
                            //url para agregar
                            //url = "/ServiciosMedicos/SOAP/",
                            start = string.Format("{0:yyyy-MM-ddT07:30:00}", cita.start),
                            className = "agendar",
                        };

                        results1.Add(resultado);
                    }

                }


            }

            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };



        }


        public class CitaLabLista
        {
            public string numemp { get; set; }
            public string paciente { get; set; }
            public string medico { get; set; }
            public string fecha { get; set; }

        }

        public JsonResult BuscarCitaLab(string exp, string nombre, string apaterno, string amaterno)
        {
            Models.SMDEVEntities16 db = new Models.SMDEVEntities16();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.AddDays(-15).ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            //string query = "SELECT * FROM DHABIENTES WHERE NUMEMP LIKE '%"+ expTxt + "%' AND NOMBRES like '%" + nombreTxt + "%' COLLATE Latin1_General_CI_AI AND APATERNO like '%" + apaternoTxt + "%' COLLATE Latin1_General_CI_AI AND AMATERNO like '%" + amaternoTxt + "%' COLLATE Latin1_General_CI_AI";
            string query = " SELECT top 300  NUMEMP FROM DHABIENTES  WHERE FVIGENCIA != '1900-01-01T00:00:00' AND NUMAFIL IS NOT NULL AND IsNull(BAJA,'0') <> '01' AND IsNull(BAJA,'0') <> '03'  AND   (NUMEMP like '" + exp + "%' or  ''='" + exp + "'  ) and ( NOMBRES like '%" + nombre.ToUpper() + "%' or ''='" + nombre.ToUpper() + "'  ) and ( APATERNO like '%" + apaterno.ToUpper() + "%'  or ''='" + apaterno.ToUpper() + "' ) and  (AMATERNO like '%" + amaterno.ToUpper() + "%' or ''='" + amaterno.ToUpper() + "'  )  ";
            var result = db.Database.SqlQuery<DHABIENTES>(query);
            var dhabientes = result.ToList();

            //Validar que tenga un folio de 15 dias atras o menos 

            //System.Diagnostics.Debug.WriteLine(dhabientes);

            var results1 = new List<CitaLabLista>();


            foreach (var dh in dhabientes)
            {
                //Buscar en tabla Lab_exp si tiene un registro
                var lab_exp = (from r in db2.Lab_exp
                               join medicos in db2.MEDICOS on r.medico_crea equals medicos.Numero into medicosX
                               from medicosIn in medicosX.DefaultIfEmpty()
                               join dhabiente in db2.DHABIENTES on r.expediente equals dhabiente.NUMEMP into dhabienteX
                               from dhabienteIn in dhabienteX.DefaultIfEmpty()
                               where r.expediente == dh.NUMEMP
                               where r.fecha_crea >= fecha_correcta
                               orderby r.fecha_crea descending
                               select new
                               {
                                   paciente = dhabienteIn.NOMBRES + " " + dhabienteIn.APATERNO + " " + dhabienteIn.AMATERNO,
                                   medico = medicosIn.Nombre + " " + medicosIn.Apaterno + " " + medicosIn.Amaterno,
                                   fecha = r.fecha_crea,
                                   fnac = dhabienteIn.FNAC,
                                   sexo = dhabienteIn.SEXO,
                               }).FirstOrDefault();


                //System.Diagnostics.Debug.WriteLine(lab_exp);

                //var fnaci = lab_exp.fnac;


                //System.Diagnostics.Debug.WriteLine(edad);

                if (lab_exp != null)
                {

                    var fnaci = lab_exp.fnac.ToString();
                    var split1 = fnaci.Split(' ');
                    var fechanac = split1[0];
                    var split2 = fechanac.Split('/');
                    //System.Diagnostics.Debug.WriteLine(split2[2]);

                    var anioFnac = Int16.Parse(split2[2]);
                    //var split2 = split1[1].Split('/');
                    //System.Diagnostics.Debug.WriteLine(split2[2]);

                    DateTime now = DateTime.Today;
                    int edad = now.Year - anioFnac;


                    var resultado = new CitaLabLista
                    {
                        numemp = dh.NUMEMP + "*" + lab_exp.paciente + "*" + lab_exp.medico + "*" + edad + "*" + lab_exp.sexo,
                        paciente = lab_exp.paciente,
                        fecha = string.Format("{0:yyyy-MM-dd}", lab_exp.fecha),
                    };

                    results1.Add(resultado);

                }
            }

            //System.Diagnostics.Debug.WriteLine(results1);

            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult CambiarEstadoANoContestar(bool NoContesto, string IdCita)
        {
            var valor = NoContesto ? 1 : 0;

            // usar using para cerrar la conexion con seguridad.
            using (SMDEVEntities33 db = new Models.SMDEVEntities33())
            {
                // marcar bloqueado a 1 para que se bloquee en la vista
                db.Database.ExecuteSqlCommand("UPDATE CITAS SET NoContesto = '" + valor + "' WHERE Id = '" + IdCita + "'");
            }

            return Json("OK");
        }

    }
}