using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class LaboratorioController : Controller
    {
        public ActionResult Index(string expediente, string medico)
        {

            if (expediente == null || medico == null)
            {
                return RedirectToAction("Index", "Home");
            }

       
            if (User.IsInRole("SinAgenda") == false )
            {

                if (User.IsInRole("Subrogados") == true)
                {

                    Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();
                    Models.SMDEVEntities20 db2 = new Models.SMDEVEntities20();

                    var dhabiente = (from a in db2.DHABIENTES
                                     where a.NUMEMP == expediente
                                     select a).FirstOrDefault();


                    var listLabExpd = new Lab_Expediente();

                    var resultLabExp = (from labexp in db.Lab_exp
                                        where (labexp.expediente == expediente)
                                        && (labexp.medico_crea == medico)
                                        orderby labexp.fecha_crea descending
                                        select labexp).ToList();


                    listLabExpd.NUMEMP = dhabiente.NUMEMP;
                    listLabExpd.expediente = expediente;
                    listLabExpd.LabExpediente = resultLabExp;
                    listLabExpd.DHabiente = dhabiente;
                    //return View(listLabExpd);

                    if (dhabiente.NUMAFIL != "P72112")
                    {
                        return View(listLabExpd);
                    }
                    else
                    {
                        TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                        return RedirectToAction("SOAP/" + expediente, "ServiciosMedicos");
                    }



                }
                else
                {

                    var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                    Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                    string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio from CITAS WHERE NumEmp = '" + expediente + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                    var result = db4.Database.SqlQuery<Citas>(query);
                    var res = result.FirstOrDefault();

                    if (res != null)
                    {
                        Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();
                        Models.SMDEVEntities20 db2 = new Models.SMDEVEntities20();

                        var dhabiente = (from a in db2.DHABIENTES
                                         where a.NUMEMP == expediente
                                         select a).FirstOrDefault();


                        var listLabExpd = new Lab_Expediente();

                        var resultLabExp = (from labexp in db.Lab_exp
                                            where (labexp.expediente == expediente)
                                            && (labexp.medico_crea == medico)
                                            orderby labexp.fecha_crea descending
                                            select labexp).ToList();


                        listLabExpd.NUMEMP = dhabiente.NUMEMP;
                        listLabExpd.expediente = expediente;
                        listLabExpd.LabExpediente = resultLabExp;
                        listLabExpd.DHabiente = dhabiente;
                        //return View(listLabExpd);

                        if (dhabiente.NUMAFIL != "P72112")
                        {
                            return View(listLabExpd);
                        }
                        else
                        {
                            TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                            return RedirectToAction("SOAP/" + expediente, "ServiciosMedicos");
                        }


                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            else

            {
                Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();
                Models.SMDEVEntities20 db2 = new Models.SMDEVEntities20();

                var dhabiente = (from a in db2.DHABIENTES
                                 where a.NUMEMP == expediente
                                 select a).FirstOrDefault();


                var listLabExpd = new Lab_Expediente();

                var resultLabExp = (from labexp in db.Lab_exp
                                    where (labexp.expediente == expediente)
                                    && (labexp.medico_crea == medico)
                                    orderby labexp.fecha_crea descending
                                    select labexp).ToList();


                listLabExpd.NUMEMP = dhabiente.NUMEMP;
                listLabExpd.expediente = expediente;
                listLabExpd.LabExpediente = resultLabExp;
                listLabExpd.DHabiente = dhabiente;
                //return View(listLabExpd);

                if (dhabiente.NUMAFIL != "P72112")
                {
                    return View(listLabExpd);
                }
                else
                {
                    TempData["message_recetas_warning"] = "Paciente sin Servicio Médico, favor solo llenar Nota Médica";
                    return RedirectToAction("SOAP/" + expediente, "ServiciosMedicos");
                }


            }
        }


        public ActionResult Create(string expediente)
        {

            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabiente = (from a in db.DHABIENTES
                             where a.NUMEMP == expediente
                             select a).FirstOrDefault();
            return View(dhabiente);

        }



        public ActionResult Edit(string expediente, string folio)
        {
            int folio_lab = Convert.ToInt32(folio);

            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
            var dhabiente = (from a in db.DHABIENTES
                             where a.NUMEMP == expediente
                             select a).FirstOrDefault();



            Models.SMDEVEntitiesSoLab db2 = new Models.SMDEVEntitiesSoLab();
            var resultLabExp = (from labexp in db2.Lab_exp
                                where (labexp.Folio_lab == folio_lab)
                                select new { labexp }).FirstOrDefault();

            var resultlabDet = (from labDet in db2.Lab_detalle
                                join labCat in db2.lab_catalogo on labDet.id_servicio equals labCat.lab_id
                                where (labDet.Folio_lab == folio_lab)
                                orderby labDet.Folio_lab descending
                                select labCat).ToList();

            SolicitudLaboratorioExp labExpediente = new SolicitudLaboratorioExp();
            labExpediente.DerechoHabiente = dhabiente;
            labExpediente.LabExpediente = resultLabExp.labexp;
            labExpediente.LabDetalle = resultlabDet;

            return View(labExpediente);
        }


        public JsonResult GuardarSolicitudLaboratorioExp(string newSolLab, string labDetalle)
        {
            //System.Diagnostics.Debug.WriteLine(newSolLab);
            //System.Diagnostics.Debug.WriteLine(labDetalle);

            List<Lab_Detalle> labDet = JsonConvert.DeserializeObject<List<Lab_Detalle>>(labDetalle);
            Lab_exp NewLabExp = JsonConvert.DeserializeObject<Lab_exp>(newSolLab);

            Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();
            int folioLab;

            try
            {

                using (SqlCommand cmd = new SqlCommand("insert into lab_exp(expediente, medico_crea, fecha_crea, estado, urgente, observaciones) values('" + NewLabExp.expediente + "', '" + NewLabExp.medico_crea + "', GETDATE(), 0, '" + NewLabExp.urgente + "', '" + NewLabExp.observaciones + "')"))
                {

                    cmd.Connection = new SqlConnection(db.Database.Connection.ConnectionString);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = "SELECT SCOPE_IDENTITY()";
                    folioLab = Convert.ToInt32(cmd.ExecuteScalar());
                    cmd.Connection.Close();
                }

                foreach (var item in labDet)
                {


                    if (item.lab_id == "0381")
                    {
                        db.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + folioLab + "','" + item.lab_id + "','56A'," + item.lab_instrucciones + ")");
                        db.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + folioLab + "','" + item.lab_id + "','56M'," + item.lab_instrucciones + ")");
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + folioLab + "','" + item.lab_id + "','" + item.lab_lab + "'," + item.lab_instrucciones + ")");
                    }
                }

            }
            catch (Exception ex)
            {
                //System.Diagnostics.Debug.WriteLine("fsdfsd");

                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult ActualizarSolicitudLaboratorioExp(string folio, string labDetalle)
        {

            List<Lab_Detalle> labDet = JsonConvert.DeserializeObject<List<Lab_Detalle>>(labDetalle);


            Models.SMDEVEntitiesSoLab db = new Models.SMDEVEntitiesSoLab();
            int folioLab = Convert.ToInt32(folio);

            try
            {

                foreach (var item in labDet)
                {


                    if (item.lab_id == "0381")
                    {
                        db.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + folioLab + "','" + item.lab_id + "','56A'," + item.lab_instrucciones + ")");
                        db.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + folioLab + "','" + item.lab_id + "','56M'," + item.lab_instrucciones + ")");
                    }
                    else
                    {
                        db.Database.ExecuteSqlCommand("insert into lab_detalle (FOLIO_LAB, ID_SERVICIO, ID_LAB, id_indicaciones) values ('" + folioLab + "','" + item.lab_id + "','" + item.lab_lab + "'," + item.lab_instrucciones + ")");
                    }
                }

            }
            catch (Exception ex)
            {


                return new JsonResult { Data = ex.Message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

            return new JsonResult { Data = "1", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public ActionResult Citas()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GuardarCitas(Citas model)
        {
            //System.Diagnostics.Debug.WriteLine(hora);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            //var fechaDate = DateTime.Parse(fecha);

            //System.Diagnostics.Debug.WriteLine(model.Fecha);

            string query = "select Tipo as tipo from CITAS WHERE Fecha = '" + model.Fecha + "T00:00:00' and Tipo = '39' ";
            var result = db.Database.SqlQuery<Citas>(query);
            var res = result.Count();

            var EmpRealizo = User.Identity.GetUserName();

            //Si son menos de 100, en cierta fecha, que te permita agregar una cita 
            if (res < 3)
            {
                //var turnoFinal = hora + minuto;

                //Contar las citas que lleva realizadas el usuario de hoy
                string query2 = "select FRealizo as frealizo, EmpRealizo as emprealizo from CITAS WHERE EmpRealizo = '" + EmpRealizo + "' and FRealizo = '" + fecha + "' ";
                var result2 = db.Database.SqlQuery<Citas>(query2);
                var res2 = result2.Count();
                var countRes2 = res2 + 1;

                
                //Si va a editar
                if (model.modificar == 1)
                {
                    //System.Diagnostics.Debug.WriteLine(model.tipo);
                    db.Database.ExecuteSqlCommand("Update CITAS SET Fecha = '" + model.Fecha + "T00:00:00', Tipo = '" + model.tipo + "' WHERE NumEmp = '" + model.NumEmp + "' and REGXDIA = '" + model.regxdia + "' and Fecha = '" + model.Fecha + "T00:00:00' and EmpRealizo = '" + EmpRealizo + "'");
                    if(model.tipo == "00")
                    {
                        TempData["message_success"] = "Cita cancelada con éxito";
                    }
                    else
                    {
                        TempData["message_success"] = "Cita editada con éxito";
                    }
                }
                else
                {
                    //Se va a agregar
                    //Tipo es 39 para estudios de lab
                    db.Database.ExecuteSqlCommand("INSERT INTO Citas (NumEmp, Fecha, Tipo, EmpRealizo, FRealizo, REGXDIA) VALUES('" + model.NumEmp + "', '" + model.Fecha + "T00:00:00', '" + model.tipo + "', '" + EmpRealizo + "' , '" + fecha + "', '" + countRes2 + "')");
                    TempData["message_success"] = "Cita agendada con éxito";
                }


                //ACTUALIZAR TELEFONO DE DHABIENTES
                db.Database.ExecuteSqlCommand("Update DHABIENTES SET tel_consulta = '" + model.telefono + "' WHERE NumEmp = '" + model.NumEmp + "'");

                

            }
            else
            {
                //Si va a editar
                if (model.modificar == 1)
                {
                    //System.Diagnostics.Debug.WriteLine(model.tipo);
                    db.Database.ExecuteSqlCommand("Update CITAS SET Fecha = '" + model.Fecha + "T00:00:00', Tipo = '" + model.tipo + "' WHERE NumEmp = '" + model.NumEmp + "' and REGXDIA = '" + model.regxdia + "' and Fecha = '" + model.Fecha + "T00:00:00' and EmpRealizo = '" + EmpRealizo + "'");
                    if (model.tipo == "00")
                    {
                        TempData["message_success"] = "Cita cancelada con éxito";
                    }
                    else
                    {
                        TempData["message_success"] = "Cita editada con éxito";
                    }
                }
                else
                {
                    //Ya fueron las 100
                    TempData["message_success"] = "Ya hay 3 registros para el día " + model.Fecha;
                }

            }

            return Redirect(Request.UrlReferrer.ToString());
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

        public JsonResult TodasCitas()
        {
            var fecha = DateTime.Now.AddMonths(-6).ToString("yyyy-MM-ddT00:00:00.000");
            //var fecha_correcta = DateTime.Parse(fecha);
            //Models.SERVMEDEntities2 db = new Models.SERVMEDEntities2();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            var EmpRealizo = User.Identity.GetUserName();

            //System.Diagnostics.Debug.WriteLine(EmpRealizo);

            string query = "select CITAS.hora_recepcion as hora_recepcion, CITAS.NumEmp as NumEmp, CITAS.Fecha as start, CITAS.REGXDIA as regxdia, DHABIENTES.NOMBRES as Nombres, DHABIENTES.APATERNO as Apaterno, DHABIENTES.AMATERNO as Amaterno, TIPOSCONSULTA.Descripcion as tipo, CITAS.hr_consultorio as hr_consultorio, CASE WHEN len(DHABIENTES.tel_consulta) > 0 THEN '52' + DHABIENTES.tel_consulta ELSE '52' + AFILIADOS.tel_celular END AS telefono from CITAS INNER JOIN DHABIENTES ON DHABIENTES.NUMEMP = CITAS.NumEmp INNER JOIN TIPOSCONSULTA ON TIPOSCONSULTA.Clave = CITAS.Tipo INNER JOIN AFILIADOS ON AFILIADOS.num_contrato = DHABIENTES.NUMAFIL where CITAS.Tipo = '39' and CITAS.Fecha >= '" + fecha +"' ORDER BY Fecha DESC";
            var result = db.Database.SqlQuery<Citas>(query);
            var miscitas = result.ToList();

            //System.Diagnostics.Debug.WriteLine(miscitas);

            var results1 = new List<MisCitasList>();

            foreach (var cita in miscitas)
            {
                var estatus = "";

                if (cita.hora_recepcion == null)
                {
                    if(cita.tipo == "Cancelada")
                    {
                        estatus = "Cancelada";
                    }
                    else
                    {
                        estatus = "Programada";
                    }
                }
                else
                {
                    if (cita.tipo == "Cancelada")
                    {
                        estatus = "Cancelada";
                    }
                    else
                    {
                        estatus = "Registrada";
                    }
                }

                var resultado = new MisCitasList
                {
                    Numero_Medico = cita.NumEmp + "*" + cita.regxdia + "*" + string.Format("{0:yyyy-MM-ddT00:00:00.000}", cita.start),
                    Paciente = cita.Nombres + " " + cita.Apaterno + " " + cita.Amaterno + "<br><span><b>" + cita.telefono + "</b></span>",
                    //Medico = cita.Medico,
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



        public JsonResult TipoConsultas()
        {
            //Models.SERVMEDEntities2 db = new Models.SERVMEDEntities2();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var tipoConsulta = (from d in db2.TIPOSCONSULTA
                                where d.Clave == "00" || d.Clave == "39"
                                select new
                                {
                                    Clave = d.Clave,
                                    Descripcion = d.Descripcion,
                                })
                                .ToList();

            //System.Diagnostics.Debug.WriteLine(tipoConsulta);

            return new JsonResult { Data = tipoConsulta, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }



        public JsonResult GetCita(string numemp, int regxdia, string fecha)
        {
            //var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var EmpRealizo = User.Identity.GetUserName();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            string query = "select CITAS.Fecha as start, CITAS.NumEmp as NumEmp, CITAS.REGXDIA as regxdia, DHABIENTES.NOMBRES as Nombres, DHABIENTES.APATERNO as Apaterno, DHABIENTES.AMATERNO as Amaterno, DHABIENTES.SEXO as sexo, DHABIENTES.FNAC as fnac, TIPOSCONSULTA.Clave as tipo, CITAS.turno as turno from CITAS INNER JOIN DHABIENTES ON DHABIENTES.NUMEMP = CITAS.NumEmp INNER JOIN TIPOSCONSULTA ON TIPOSCONSULTA.Clave=CITAS.Tipo where CITAS.NumEmp = '" + numemp + "' and CITAS.EmpRealizo = '" + EmpRealizo + "' and CITAS.Fecha = '" + fecha + "'  and CITAS.REGXDIA = '" + regxdia + "'";
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




        public JsonResult RegistrarCita(string numemp, int regxdia, string fecha)
        {
            //var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var EmpRealizo = User.Identity.GetUserName();
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            string query = "select CITAS.Fecha as start, CITAS.NumEmp as NumEmp, CITAS.REGXDIA as regxdia, DHABIENTES.NOMBRES as Nombres, DHABIENTES.APATERNO as Apaterno, DHABIENTES.AMATERNO as Amaterno, DHABIENTES.SEXO as sexo, DHABIENTES.FNAC as fnac, TIPOSCONSULTA.Clave as tipo, CITAS.turno as turno from CITAS INNER JOIN DHABIENTES ON DHABIENTES.NUMEMP = CITAS.NumEmp INNER JOIN TIPOSCONSULTA ON TIPOSCONSULTA.Clave=CITAS.Tipo where CITAS.NumEmp = '" + numemp + "' and CITAS.EmpRealizo = '" + EmpRealizo + "' and CITAS.Fecha = '" + fecha + "'  and CITAS.REGXDIA = '" + regxdia + "' and CITAS.hora_recepcion is null";
            var result = db.Database.SqlQuery<Citas>(query);
            var cita = result.FirstOrDefault();

            var dhabiente = (from r in db.DHABIENTES
                             where r.NUMEMP == numemp
                             select new
                             {
                                 telefono = r.tel_consulta
                             }).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(cita);
            
            var mensaje = "";

            //Si es nulo entonces ya se registro 
            if (cita == null)
            {
                //TempData["cita_success"] = "Esta cita ya se registró";
                mensaje = "Esta cita ya se registró";
            }
            //Entonces hay que actalizar la hora_recepcion
            else
            {
                var hora_recepcion_mascinco = DateTime.Now.ToString("HH:mm");
                //var hora_recepcion_mascincoDT = DateTime.Parse(hora_recepcion_mascinco);

                db.Database.ExecuteSqlCommand("Update CITAS SET hora_recepcion = '" + hora_recepcion_mascinco + "' where NumEmp = '" + numemp + "' and EmpRealizo = '" + EmpRealizo + "' and Fecha = '" + fecha + "'  and REGXDIA = '" + regxdia + "' and hora_recepcion is null");
                //TempData["cita_success"] = "Cita registrada con éxito";
                mensaje = "Cita registrada con éxito";
            }


            //var mensaje  = "";

            //System.Diagnostics.Debug.WriteLine(cita.fnac);

            return new JsonResult { Data = mensaje, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



    }
}
