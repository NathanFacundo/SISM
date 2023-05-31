using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UanlSISM.Models;
using System.Text.RegularExpressions;

namespace UanlSISM.Controllers
{
    public class RecepcionTriageController : Controller
    {
        SMDEVTriage db = new SMDEVTriage();
        SMDEVEntities33 db3 = new SMDEVEntities33();

        /**************************************************  PANTALLA RECEPCIÓN TRIAGE ***************************   INICIO  *******************/
        [Authorize]
        public ActionResult RecTriage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckTriage(TriageData triageData)
        {
            //if (triageData.Embarazo == "on" || triageData.DolorToracico == "on" || triageData.DeterioroNeurologico == "on")
            //    return Json(new { HexColor = "#FFFF00" });

            var redCondition = new TriageCondition("#FF0000",
                sistolica: new TriageRange(new TriageMinMax(0, 50), new TriageMinMax(200, 1000)),
                diastolica: new TriageRange(new TriageMinMax(0, 40), new TriageMinMax(120, 1000)),
                frecuenciaCarediaca: new TriageRange(new TriageMinMax(0, 40), new TriageMinMax(150, 1000)),
                frecuenciaRespiratoria: new TriageRange(new TriageMinMax(0, 10), new TriageMinMax(35, 100)),
                temperatura: new TriageRange(new TriageMinMax(0, 35), new TriageMinMax(40, 100)),
                sO2: new TriageRange(new TriageMinMax(0, 70)),
                glucosa: new TriageRange(new TriageMinMax(0, 40)),
                glasgow: new TriageRange(new TriageMinMax(3, 6)));

            var orangeCondition = new TriageCondition("#FF5733",
                sistolica: new TriageRange(new TriageMinMax(50, 70), new TriageMinMax(190, 200)),
                diastolica: new TriageRange(new TriageMinMax(40, 60), new TriageMinMax(100, 120)),
                frecuenciaCarediaca: new TriageRange(new TriageMinMax(40, 50), new TriageMinMax(130, 150)),
                frecuenciaRespiratoria: new TriageRange(new TriageMinMax(10, 15), new TriageMinMax(30, 40)),
                temperatura: new TriageRange(new TriageMinMax(35, 35.5), new TriageMinMax(39, 40)),
                sO2: new TriageRange(new TriageMinMax(70, 80)),
                glucosa: new TriageRange(new TriageMinMax(40, 50), new TriageMinMax(400, 1000)),
                glasgow: new TriageRange(new TriageMinMax(7, 10)));

            var yellowCondition = new TriageCondition("#FFFF00",
                sistolica: new TriageRange(new TriageMinMax(71, 100), new TriageMinMax(141, 189)),
                diastolica: new TriageRange(new TriageMinMax(61, 75)),
                frecuenciaCarediaca: new TriageRange(new TriageMinMax(51, 59), new TriageMinMax(95, 129)),
                frecuenciaRespiratoria: new TriageRange(new TriageMinMax(15, 18), new TriageMinMax(24, 29)),
                temperatura: new TriageRange(new TriageMinMax(35.6, 35.8), new TriageMinMax(38, 38.9)),
                sO2: new TriageRange(new TriageMinMax(81, 93)),
                glucosa: new TriageRange(new TriageMinMax(51, 60), new TriageMinMax(200, 400)),
                glasgow: new TriageRange(new TriageMinMax(10, 14)));

            var greenCondition = new TriageCondition("#008000",
                sistolica: new TriageRange(new TriageMinMax(131, 140)),
                diastolica: new TriageRange(new TriageMinMax(76, 85)),
                frecuenciaCarediaca: new TriageRange(new TriageMinMax(60, 95)),
                frecuenciaRespiratoria: new TriageRange(new TriageMinMax(19, 23)),
                temperatura: new TriageRange(new TriageMinMax(35.9, 36), new TriageMinMax(37.1, 37.9)),
                sO2: new TriageRange(new TriageMinMax(94, 96)),
                glucosa: new TriageRange(new TriageMinMax(61, 70), new TriageMinMax(130, 199)),
                glasgow: new TriageRange(new TriageMinMax(15, 16)));

            var blueCondition = new TriageCondition("#0000FF",
                sistolica: new TriageRange(new TriageMinMax(90, 130)),
                diastolica: new TriageRange(new TriageMinMax(70, 80)),
                frecuenciaCarediaca: new TriageRange(new TriageMinMax(61, 90)),
                frecuenciaRespiratoria: new TriageRange(new TriageMinMax(19, 20)),
                temperatura: new TriageRange(new TriageMinMax(36, 36.5), new TriageMinMax(36.6, 37)),
                sO2: new TriageRange(new TriageMinMax(97, 100)),
                glucosa: new TriageRange(new TriageMinMax(71, 129)),
                glasgow: new TriageRange(new TriageMinMax(15, 16)));

            List<TriageCondition> triageConditions = new List<TriageCondition>
            {
                redCondition,
                orangeCondition,
                yellowCondition,
                greenCondition,
                blueCondition
            };

            try
            {
                foreach (var condition in triageConditions)
                    condition.InCondition(triageData);

                return Json(new { HexColor = "#ffffff" });
            }
            catch (TriageException ex)
            {
                return Json(new { HexColor = ex.Condition.HexColor });
            }
        }

        public class BuscarDhabList
        {
            public string numemp { get; set; }
            public string numexp { get; set; }
            public string paciente { get; set; }
            public string edad { get; set; }
            public string sexo { get; set; }
            public string info { get; set; }
            public string fecha { get; set; }
        }

        public JsonResult BuscarDhabientes(string numemp, string numexp, string nombre, string apaterno, string amaterno)
        {
            //SMDEVEntities16 db = new SMDEVEntities16();
            SMDEVEntities33 db2 = new SMDEVEntities33();
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaHoyDT = DateTime.Parse(fechaHoy);

            string query = " SELECT top 300  NUMEMP FROM DHABIENTES  WHERE FVIGENCIA != '1900-01-01T00:00:00' AND NUMAFIL IS NOT NULL AND IsNull(BAJA,'0') <> '01' AND IsNull(BAJA,'0') <> '03' AND (NUMAFIL like '" + numemp + "%' or  ''='" + numemp + "'  ) AND (NUMEMP like '" + numexp + "%' or  ''='" + numexp + "'  ) and ( NOMBRES like '%" + nombre.ToUpper() + "%' or ''='" + nombre.ToUpper() + "'  ) and ( APATERNO like '%" + apaterno.ToUpper() + "%'  or ''='" + apaterno.ToUpper() + "' ) and  (AMATERNO like '%" + amaterno.ToUpper() + "%' or ''='" + amaterno.ToUpper() + "'  )  ";
            var result = db.Database.SqlQuery<DHABIENTES>(query);
            var dhabientes = result.ToList();

            var results1 = new List<BuscarDhabList>();

            foreach (var dh in dhabientes)
            {
                //Buscar en tabla Lab_exp si tiene un registro
                var dhab = (from d in db2.DHABIENTES
                            where d.NUMEMP == dh.NUMEMP
                            //where r.fecha_crea >= fecha_correcta
                            //orderby r.fecha_crea descending
                            select new
                            {
                                paciente = d.NOMBRES + " " + d.APATERNO + " " + d.AMATERNO,
                                fnac = d.FNAC,
                                sexo = d.SEXO,
                                numexp = d.NUMEMP,
                                numemp = d.NUMAFIL,
                                fvigencia = d.FVIGENCIA,
                            }).FirstOrDefault();

                var edad = 0;
                var meses = 0;

                if (dhab != null)
                {
                    var today = DateTime.Today;
                    DateTime fnac = (DateTime)dhab.fnac;
                    int Years = 0;
                    int Months = 0;
                    int Days = 0;

                    if ((today.Year - fnac.Year) > 0 ||
                    (((today.Year - fnac.Year) == 0) && ((fnac.Month < today.Month) ||
                    ((fnac.Month == today.Month) && (fnac.Day <= today.Day)))))
                    {
                        int DaysInBdayMonth = DateTime.DaysInMonth(fnac.Year, fnac.Month);
                        int DaysRemain = today.Day + (DaysInBdayMonth - fnac.Day);

                        if (today.Month > fnac.Month)
                        {
                            Years = today.Year - fnac.Year;
                            Months = today.Month - (fnac.Month + 1) + Math.Abs(DaysRemain / DaysInBdayMonth);
                            Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                        }
                        else if (today.Month == fnac.Month)
                        {
                            if (today.Day >= fnac.Day)
                            {
                                Years = today.Year - fnac.Year;
                                Months = 0;
                                Days = today.Day - fnac.Day;
                            }
                            else
                            {
                                Years = (today.Year - 1) - fnac.Year;
                                Months = 11;
                                Days = DateTime.DaysInMonth(fnac.Year, fnac.Month) - (fnac.Day - today.Day);
                            }
                        }
                        else
                        {
                            Years = (today.Year - 1) - fnac.Year;
                            Months = today.Month + (11 - fnac.Month) + Math.Abs(DaysRemain / DaysInBdayMonth);
                            Days = (DaysRemain % DaysInBdayMonth + DaysInBdayMonth) % DaysInBdayMonth;
                        }
                    }

                    var vigencia = "";

                    if (fechaHoyDT > dhab.fvigencia)
                    {
                        vigencia = "Vigencia vencida";
                    }
                    else
                    {
                        vigencia = "Vigente";
                    }

                    //var tblt = (from d in db.TblTriage
                    //            where d.Expediente == dh.NUMEMP
                    //            select new
                    //            {
                    //                SV_PAdiastolica = d.SV_PAdiastolica,
                    //            }).FirstOrDefault();

                    var resultado = new BuscarDhabList
                    {
                        paciente = dhab.paciente,
                        numexp = dhab.numexp,
                        numemp = dhab.numemp,
                        edad = Years + " años con " + Months + " meses" + " y " + Days + " días",
                        sexo = dhab.sexo,
                        info = dhab.numexp + "*" + dhab.paciente + "*" + Years + " años con " + Months + " meses" + " y " + Days + " días" + "*" + dhab.sexo + "*" + vigencia + "*" + string.Format("{0:yyyy-MM-dd}", dhab.fvigencia),
                    };

                    results1.Add(resultado);
                }
            }

            return new JsonResult { Data = results1, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult Save(TriageData triageData)
        {
            try
            {
                // TODO: Logica para guardar en base de datos
                var nombreusuario = User.Identity.GetUserName();
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                var fechaDT = DateTime.Parse(fecha);
                var ip_realiza = Request.UserHostAddress;

                TblTriage triage = new TblTriage();

                if (triageData.ColorBoton == "#FF0000")
                {
                    triage.ColorConsulta = "Rojo";
                }
                else
                {
                    if (triageData.ColorBoton == "#FF5733")
                    {
                        triage.ColorConsulta = "Naranja";
                    }
                    else if (triageData.ColorBoton == "#FFFF00")
                    {
                        triage.ColorConsulta = "Amarrillo";
                    }
                    else if (triageData.ColorBoton == "#008000")
                    {
                        triage.ColorConsulta = "Verde";
                    }
                    else if (triageData.ColorBoton == "#0000FF")
                    {
                        triage.ColorConsulta = "Azul";
                    }
                }

                if (triageData.DolorToracico == "on")
                {
                    triage.ConsultaEspecial = "Dolor Toracico";
                }
                else
                {
                    if (triageData.Embarazo == "on")
                    {
                        triage.ConsultaEspecial = "Embarazo";
                    }
                    else if (triageData.DeterioroNeurologico == "on")
                    {
                        triage.ConsultaEspecial = "Deterioro Neurologico";
                    }
                }

                triage.UsuarioRegistra = nombreusuario;
                triage.Expediente = triageData.Expediente;
                triage.Descripcion = triageData.Descripcion;
                triage.Fecha = fechaDT;
                triage.ip_realiza = ip_realiza;

                triage.SV_PAsistolica = triageData.Sistolica.ToString();
                triage.SV_PAdiastolica = triageData.Diastolica.ToString();
                triage.SV_FC = triageData.FrecuenciaCardiaca.ToString();
                triage.SV_FR = triageData.FrecuenciaRespiratoria.ToString();
                triage.SV_Temperatura = triageData.Temperatura.ToString();
                triage.SV_SO2 = triageData.SO2.ToString();
                triage.SV_Glucosa = triageData.Glucosa.ToString();
                triage.SV_Glasgow = triageData.Glasgow.ToString();
                triage.SV_Peso = triageData.Peso.ToString();
                triage.SV_Talla = triageData.Talla.ToString();
                triage.NombrePaciente = Regex.Replace(triageData.Nombre.Trim(), @"\s+", " ");
                triage.EdadPaciente = triageData.Edad.ToString();
                triage.SexoPaciente = triageData.Sexo.ToString();
                triage.VigenciaPaciente = triageData.Vigencia.ToString();
                //Consulta nace como 0, o sea que el paciente está En Espera de ser llamado por el dr.
                triage.EstatusConsulta = "0";
                //triage.MedicoLlama = "en espera";

                db.TblTriage.Add(triage);
                //var status = true;
                db.SaveChanges();

                return Json(new { MENSAJE = "Succe: Los datos se guardaron con éxito" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }

            //if (status)
            //    return new HttpStatusCodeResult(HttpStatusCode.OK, "Se guardo la información");

            //return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Error al guardar los datos");
        }

        #region TriageConditions
        public class TriageRange
        {

            public TriageMinMax Minimum { get; set; }
            public TriageMinMax Maximum { get; set; }
            public double CurrentValue { get; set; }

            public TriageRange(TriageMinMax minimum, TriageMinMax maximum)
            {
                Minimum = minimum;
                Maximum = maximum;
            }

            public TriageRange(TriageMinMax minimum)
            {
                Minimum = minimum;
            }

            public bool InRange(double data)
            {
                CurrentValue = data;
                return (Minimum?.In(data) ?? false) || (Maximum?.In(data) ?? false);
            }

            public override string ToString()
            {
                List<string> strBuilder = new List<string>();
                if (Minimum != null)
                    strBuilder.Add(Minimum.ToString());
                if (Maximum != null)
                    strBuilder.Add(Maximum.ToString());
                return $"[{string.Join(",", strBuilder)}]: {CurrentValue}";
            }
        }

        public class TriageException : Exception
        {
            public TriageCondition Condition { get; set; }

            public TriageException(TriageCondition condition, string message) : base(message)
            {
                Condition = condition;
            }
        }


        public class TriageMinMax
        {
            public TriageMinMax(double minimum, double maximum)
            {
                Minimum = minimum;
                Maximum = maximum;
            }

            public double Minimum { get; set; }
            public double Maximum { get; set; }

            public bool In(double data)
            {
                if (data == 0)
                    return false;
                return Minimum <= data && data <= Maximum;
            }

            public override string ToString()
            {
                return $"[{Minimum}, {Maximum}]";
            }
        }

        public class TriageCondition
        {
            public string HexColor { get; set; }
            public TriageRange Sistolica { get; set; }
            public TriageRange Diastolica { get; set; }
            public TriageRange FrecuenciaCardiaca { get; set; }
            public TriageRange FrecuenciaRespiratoria { get; set; }
            public TriageRange Temperatura { get; set; }
            public TriageRange SO2 { get; set; }
            public TriageRange Glucosa { get; set; }
            public TriageRange Glasgow { get; set; }



            public TriageCondition(string hexColor, TriageRange sistolica, TriageRange diastolica, TriageRange frecuenciaCarediaca, TriageRange frecuenciaRespiratoria, TriageRange temperatura, TriageRange sO2, TriageRange glucosa, TriageRange glasgow)
            {
                HexColor = hexColor;
                Sistolica = sistolica;
                Diastolica = diastolica;
                FrecuenciaCardiaca = frecuenciaCarediaca;
                FrecuenciaRespiratoria = frecuenciaRespiratoria;
                Temperatura = temperatura;
                SO2 = sO2;
                Glucosa = glucosa;
                Glasgow = glasgow;
            }

            public void InCondition(TriageData data)
            {
                if (Sistolica.InRange(data.Sistolica))
                    throw new TriageException(this, "Dentro del rango " + nameof(Sistolica) + " " + Sistolica.ToString());
                if (Diastolica.InRange(data.Diastolica))
                    throw new TriageException(this, "Dentro del rango " + nameof(Diastolica) + " " + Diastolica.ToString());
                if (FrecuenciaCardiaca.InRange(data.FrecuenciaCardiaca))
                    throw new TriageException(this, "Dentro del rango " + nameof(FrecuenciaCardiaca) + " " + FrecuenciaCardiaca.ToString());
                if (FrecuenciaRespiratoria.InRange(data.FrecuenciaRespiratoria))
                    throw new TriageException(this, "Dentro del rango " + nameof(FrecuenciaRespiratoria) + " " + FrecuenciaRespiratoria.ToString());
                if (Temperatura.InRange(data.Temperatura))
                    throw new TriageException(this, "Dentro del rango " + nameof(Temperatura) + " " + Temperatura.ToString());
                if (SO2.InRange(data.SO2))
                    throw new TriageException(this, "Dentro del rango " + nameof(SO2) + " " + SO2.ToString());
                if (Glucosa.InRange(data.Glucosa))
                    throw new TriageException(this, "Dentro del rango " + nameof(Glucosa) + " " + Glucosa.ToString());
                if (Glasgow.InRange(data.Glasgow))
                    throw new TriageException(this, "Dentro del rango " + nameof(Glasgow) + " " + Glasgow.ToString());
            }
        }

        public class TriageData
        {
            public double Sistolica { get; set; }
            public double Diastolica { get; set; }
            public double FrecuenciaCardiaca { get; set; }
            public double FrecuenciaRespiratoria { get; set; }
            public double Temperatura { get; set; }
            public double SO2 { get; set; }
            public double Glucosa { get; set; }
            public double Glasgow { get; set; }
            public string Embarazo { get; set; }
            public string DolorToracico { get; set; }
            public string DeterioroNeurologico { get; set; }
            public string Descripcion { get; set; }
            public double Peso { get; set; }
            public double Talla { get; set; }
            public string Expediente { get; set; }
            public string ColorBoton { get; set; }
            public string Nombre { get; set; }
            public string Sexo { get; set; }
            public string Edad { get; set; }
            public string Vigencia { get; set; }

        }
        #endregion

        /****                                                 PANTALLA RECEPCIÓN TRIAGE ***************************   FIN  *******************/



        /**************************************************  PANTALLA MÉDICO URGENCIAS TRIAGE ***************************   INICIO  *******************/
        public ActionResult MedicoUrgenciasT()
        {
            //if (User.IsInRole("SinAgenda"))
            //{

            //}
            //else
            //{
            //    if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
            //    {
            //        return RedirectToAction("Citas", "ServiciosMedicos");
            //    }
            //    else
            //    {
            //        if (User.IsInRole("Subrogados"))
            //        {
            //            return RedirectToAction("Subrogados", "ServiciosMedicos");
            //        }
            //        else
            //        {
            //            return RedirectToAction("Index", "Home");
            //        }
            //    }

            //}

            return View();
        }

        [Authorize]
        public ActionResult MedicoUrgenciasT2()
        {
            return View();
        }

        int Respuesta;
        public JsonResult ValidaExistencia(int IdPac)
        {
            var RegistroTriage = (from a in db.TblTriage
                                  where a.Id == IdPac
                                  where a.EstatusConsulta == "3"
                                  select new
                                  {
                                      a
                                  }).ToList();

            return new JsonResult { Data = RegistroTriage, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult GuardarCons(int IdPac)
        {
            var Medico1 = User.Identity.GetUserName();
            //var ConsulMedico = (from a in db3.MEDICOS
            //                    where a.Numero == Medico1
            //                    //select new
            //                    //{
            //                    //    a.consultorio_mt
            //                    //}).FirstOrDefault();
            //                    select a).FirstOrDefault();

            try
            {
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                var fechaDT = DateTime.Parse(fecha);
                var ip_realiza = Request.UserHostAddress;

                var RegistroTriage = (from a in db.TblTriage
                                      where a.Id == IdPac
                                      select a).FirstOrDefault();
                
                RegistroTriage.EstatusConsulta = "3";
                
                RegistroTriage.MedicoLlama = Medico1;
                RegistroTriage.FechaLlamado = fechaDT;
                RegistroTriage.ip_realizaMedico = ip_realiza;

                var ip_consultorio = (from a in db0.INVENTARIO
                                      where a.DIRECCION_IP == ip_realiza
                                      select a).FirstOrDefault();

                if (ip_consultorio != null)
                    if (ip_consultorio.num_cons != null)
                        RegistroTriage.ConsultorioMedico = Convert.ToInt32(ip_consultorio.num_cons);

                db.SaveChanges();
                Respuesta = 1;

                return Json(new { MENSAJE = "Succe: Usted a llamado a consultorio al derechohabiente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            //return Respuesta;
        }

        public class TriageList
        {
            public int Id { get; set; }
            public string EstatusConsulta { get; set; }
            public string Fecha { get; set; }
            public string NombrePaciente { get; set; }
            public string Expediente { get; set; }
            public string ColorConsulta { get; set; }
            public string MedicoLlama { get; set; }
            public int ConsultorioMedico { get; set; }
        }

        [HttpPost]
        public ActionResult ObtenerPacientes()
        {
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaHoyDT = DateTime.Parse(fechaHoy);

            try
            {
                //EstatusConsulta = 0 ASÍ NACE UN REGISTRO DEL TRIAGE (PACIENTE) CUANDO RECIEN SE CREA
                //EstatusConsulta = 3 QUIERE DECIR QUE EL DR YA LLAMÓ AL PACIENTE AL CONSULTORIO
                var query = (from a in db.TblTriage
                             where a.Fecha >= fechaHoyDT
                             where a.EstatusConsulta == "0"
                             select new
                             {
                                 a.Id,
                                 a.EstatusConsulta,
                                 a.Fecha,
                                 //string.Format("{0:yyyy-MM-dd}", a.Fecha),
                                 a.NombrePaciente,
                                 a.Expediente,
                                 a.ColorConsulta,
                                 a.MedicoLlama
                             }).ToList();

                var results1 = new List<TriageList>();

                foreach (var q in query)
                {
                    var resultado = new TriageList
                    {
                        Id = q.Id,
                        EstatusConsulta = q.EstatusConsulta,
                        //Fecha = string.Format("{0:yyyy-MM-dd HH:mm:ss}", q.Fecha),
                        Fecha = string.Format("{0:d/M/yyyy HH:mm:ss}", q.Fecha),
                        NombrePaciente = q.NombrePaciente,
                        Expediente = q.Expediente,
                        ColorConsulta = q.ColorConsulta,
                        MedicoLlama = q.MedicoLlama
                    };
                    results1.Add(resultado);
                }
                return Json(new { MENSAJE = "FOUND", PACIENTES = results1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult MostrarModal(int Paciente)
        {
            //return BuscarDhabientes("", Expediente, "", "", "");
            try
            {
                var query = (from a in db.TblTriage
                             where a.Id == Paciente
                             select new
                             {
                                 a.Id,
                                 a.Descripcion,
                                 a.Expediente,
                                 a.ColorConsulta,
                                 a.SV_PAsistolica,
                                 a.SV_PAdiastolica,
                                 a.SV_FC,
                                 a.SV_FR,
                                 a.SV_Temperatura,
                                 a.SV_SO2,
                                 a.SV_Glucosa,
                                 a.SV_Glasgow,
                                 a.SV_Peso,
                                 a.SV_Talla,
                                 a.NombrePaciente,
                                 a.EdadPaciente,
                                 a.SexoPaciente,
                                 a.VigenciaPaciente,
                                 a.ConsultaEspecial
                             }).ToList();

                //return Json(new { query }, JsonRequestBehavior.AllowGet);
                return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //BOTÓN DE RECLAMAR
        [HttpPost]
        public ActionResult Guardar_TriageMedico(int IdRegistroTriage)
        {
            try
            {
                var Medico = User.Identity.GetUserName();
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                var fechaDT = DateTime.Parse(fecha);
                var ip_realiza = Request.UserHostAddress;

                var RegistroTriage = (from a in db.TblTriage
                                      where a.Id == IdRegistroTriage
                                      select a).FirstOrDefault();

                if (RegistroTriage.EstatusConsulta == "1")
                {
                    return Json(new { MENSAJE = "Found: El derechohabiente ya fue llamado por otro médico" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ConsulMedico = (from a in db3.MEDICOS
                                        where a.Numero == Medico
                                        select a).FirstOrDefault();

                    RegistroTriage.ConsultorioMedico = Convert.ToInt32(ConsulMedico.consultorio_mt);
                    RegistroTriage.MedicoLlama = Medico;
                    RegistroTriage.FechaLlamado = fechaDT;
                    RegistroTriage.ip_realizaMedico = ip_realiza;
                    RegistroTriage.EstatusConsulta = "1";

                    db.SaveChanges();

                    return Json(new { MENSAJE = "Succe: Usted a reclamado al derechohabiente" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult PasarAConsultorio_TriageMedico(int IdPac, string Med)
        {
            var Medico1 = User.Identity.GetUserName();

            if (Med == Medico1)
            {
                var RegistroTriage = (from a in db.TblTriage
                                      where a.Id == IdPac
                                      select a).FirstOrDefault();

                //  EstatusConsulta = 0 QUIERE DECIR QUE EL PACIENTE NO HA SIDO RECLAMADO POR UN DR
                //  EstatusConsulta = 3 QUIERE DECIR QUE EL DR YA LLAMÓ AL PACIENTE AL CONSULTORIO
                RegistroTriage.EstatusConsulta = "3";
                db.SaveChanges();

                return Json(new { MENSAJE = "Succe: Usted a llamado al paciente al consultorio" }, JsonRequestBehavior.AllowGet);

                //Aquí mandaremos a la vista HOJA FRONTAL pasando el paciente
                //return View();
            }
            else
            {
                return Json(new { MENSAJE = "Error: Usted no es el médico que reclamó al paciente." }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult BotonConsultar(int IdPac)
        {
            try
            {
                var MENSAJE = "";
                var Medico1 = User.Identity.GetUserName();
                var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
                var fechaDT = DateTime.Parse(fecha);
                var ip_realiza = Request.UserHostAddress;


                var RegistroTriage = (from a in db.TblTriage
                                      where a.Id == IdPac
                                      select a).FirstOrDefault();

                if (RegistroTriage.FechaLlamado != null)
                {
                    //MENSAJE = "Found: El derechohabiente ya fue llamado por otro médico";
                    return Json(new { MENSAJE = "Found: El derechohabiente ya fue llamado por otro médico" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var ConsulMedico = (from a in db3.MEDICOS
                                        where a.Numero == Medico1
                                        select a).FirstOrDefault();

                    RegistroTriage.EstatusConsulta = "3";
                    RegistroTriage.ConsultorioMedico = Convert.ToInt32(ConsulMedico.consultorio_mt);
                    RegistroTriage.MedicoLlama = Medico1;
                    RegistroTriage.FechaLlamado = fechaDT;
                    RegistroTriage.ip_realizaMedico = ip_realiza;
                    db.SaveChanges();

                    //MENSAJE = "Succe: Usted a llamado a consultorio al derechohabiente";
                    return Json(new { MENSAJE = "Succe: Usted a llamado a consultorio al derechohabiente" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        /****                                                  PANTALLA MÉDICO URGENCIAS TRIAGE ***************************   FIN  *******************/



        /**************************************************  PANTALLA LISTA ESPERA TRIAGE ***************************   INICIO  *******************/
        public ActionResult ListaEsperaT()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ObtenerPacientesLista()
        {
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaHoyDT = DateTime.Parse(fechaHoy);

            try
            {
                var query = (from a in db.TblTriage
                             where a.Fecha >= fechaHoyDT
                             where a.EstatusConsulta == "3"
                             select new
                             {
                                 a.Id,
                                 a.EstatusConsulta,
                                 a.Fecha,
                                 a.NombrePaciente,
                                 a.Expediente,
                                 a.ColorConsulta,
                                 a.MedicoLlama,
                                 a.ConsultorioMedico
                             }).ToList();

                var results1 = new List<TriageList>();

                foreach (var q in query)
                {
                    var resultado = new TriageList
                    {
                        Id = q.Id,
                        EstatusConsulta = q.EstatusConsulta,
                        //Fecha = string.Format("{0:yyyy-MM-dd HH:mm:ss}", q.Fecha),
                        Fecha = string.Format("{HH:mm:ss}", q.Fecha),
                        NombrePaciente = q.NombrePaciente,
                        Expediente = q.Expediente,
                        ColorConsulta = q.ColorConsulta,
                        MedicoLlama = q.MedicoLlama,
                        ConsultorioMedico = (int)q.ConsultorioMedico
                    };
                    results1.Add(resultado);
                }
                return Json(new { MENSAJE = "FOUND", PACIENTES = results1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public class ListCitasCU
        {
            public string info { get; set; }
            public string hora_recepcion { get; set; }
        }

        INVENTARIOEntities db0 = new INVENTARIOEntities();

        public JsonResult ListaConsultasTriage()
        {
            SMDEVEntities33 db = new SMDEVEntities33();
            SMDEVTriage db4 = new SMDEVTriage();
            

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;

            var query = (from a in db4.TblTriage
                         where a.EstatusConsulta == "0" ||
                               a.EstatusConsulta == "3"
                         where a.Fecha >= fechaDT
                         where a.PasarASoap == null
                         orderby a.FechaLlamado descending
                         select a).ToList();

            var results = new List<ListCitasCU>();

            foreach (var item in query)
            {
                var color = "";
                var color2 = "";
                var estatus = "";
                var hora = "";
                var consulta = "";

                var medico_info = (from a in db.MEDICOS
                                   where a.Numero == item.MedicoLlama
                                   select a).FirstOrDefault();

                var ip_consultorio = (from a in db0.INVENTARIO
                                      where a.DIRECCION_IP == item.ip_realizaMedico
                                      select a).FirstOrDefault();

                var consultorio = "";
                //if (ip_consultorio.num_cons == null)
                //{
                //    consultorio = "'sin número de consultorio'";
                //}
                //else
                //{
                //    consultorio = ip_consultorio.num_cons;
                //}

                if (ip_consultorio == null)
                {
                    consultorio = "'sin número de consultorio'";
                }
                else
                {
                    if(ip_consultorio.num_cons == null)
                    {
                        consultorio = "'sin número de consultorio'";
                    }
                    else
                    {
                        consultorio = ip_consultorio.num_cons;
                    }
                }

                if (medico_info == null)
                {
                    //color = "#FBC43D";
                    if (item.ColorConsulta == "Rojo")
                        color = "#FF0000";
                    if (item.ColorConsulta == "Naranja")
                        color = "#FF5733";
                    if (item.ColorConsulta == "Amarrillo")
                        color = "#E1BB0F";
                    if (item.ColorConsulta == "Verde")
                        color = "#008000";
                    if (item.ColorConsulta == "Azul")
                        color = "#0000FF";

                    color2 = "#F5EDD9";
                    estatus = "EN ESPERA";
                    var fe = string.Format("{0:HH:mm:ss}", item.Fecha);
                    hora = "Hora de recepción: " + fe + "";
                    consulta = "";
                }
                else
                {
                    //color = "green";
                    if (item.ColorConsulta == "Rojo")
                        color = "#FF0000";
                    if (item.ColorConsulta == "Naranja")
                        color = "#FF5733";
                    if (item.ColorConsulta == "Amarrillo")
                        color = "#E1BB0F";
                    if (item.ColorConsulta == "Verde")
                        color = "#008000";
                    if (item.ColorConsulta == "Azul")
                        color = "#0000FF";

                    color2 = "#ABE3AB";
                    estatus = "PASAR AL CONSULTORIO " + consultorio;
                    var fe1 = string.Format("{0:HH:mm:ss}", item.FechaLlamado);
                    hora = "Hora de llamado: " + fe1 + "";
                    consulta = "Médico: " + medico_info.Nombre + " " + medico_info.Apaterno;
                }

                var result = new ListCitasCU
                {
                    info = item.NombrePaciente + "*" + hora + "*" + color + "*" + color2 + "*" + estatus  + "*" + consulta,
                    //hora_recepcion = item.Fecha.ToString(),
                };
                results.Add(result);
            }
            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /****                                                  PANTALLA LISTA ESPERA TRIAGE ***************************   FIN  *******************/


        /**************************************************  PANTALLA SOAP TRIAGE ***************************   INICIO  *******************/

        public ActionResult TriageSoap(string Paciente)
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            try
            {
                var query = (from a in db.TblTriage
                             where a.Expediente == Paciente
                             where a.Fecha >= fechaDT
                             select new
                             {
                                 a.Id,
                                 a.Descripcion,
                                 a.Expediente,
                                 a.ColorConsulta,
                                 a.SV_PAsistolica,
                                 a.SV_PAdiastolica,
                                 a.SV_FC,
                                 a.SV_FR,
                                 a.SV_Temperatura,
                                 a.SV_SO2,
                                 a.SV_Glucosa,
                                 a.SV_Glasgow,
                                 a.SV_Peso,
                                 a.SV_Talla,
                                 a.NombrePaciente,
                                 a.EdadPaciente,
                                 a.SexoPaciente,
                                 a.VigenciaPaciente,
                                 a.ConsultaEspecial
                             }).ToList();

                return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GuardarNotaEvolucion(string ExpedientePaciente, string NotaEvolucionT)
        {
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            try
            {
                var query = (from a in db.TblTriage
                             where a.Expediente == ExpedientePaciente
                             where a.Fecha >= fechaDT
                             select
                                 a
                             ).FirstOrDefault();


                query.NotaEvolucion = NotaEvolucionT;
                db.SaveChanges();

                return Json(new { MENSAJE = "Succe: La nota se guardó correctamente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        //SOAP en servicios médicos
        public ActionResult SOAP(string id)
        {
            //LINEAS DE MILTON
            SMDEVTriage dbMilton = new SMDEVTriage();
            var fechaMilton = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
            var fechaHoyMilton = DateTime.Parse(fechaMilton);

            //var soapTriage = (from a in dbMilton.TblTriage
            //            where a.Id > 0 
            //            select a).FirstOrDefault();
            //ViewData["Triage"] = soapTriage;

            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Revisar si ya termino el SOAP 
                /*var fechaSOAP = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                var fechaSOAPDT = DateTime.Parse(fechaSOAP);
                var username = User.Identity.GetUserName();
                Models.SMDEVEntities33 db6 = new Models.SMDEVEntities33();
                var expRevisar = (from a in db6.expediente
                                  where a.numemp == id
                                  where a.medico == username
                                  where a.fecha == fechaSOAPDT
                                  where a.hora_termino != null
                                  select a).FirstOrDefault();

                if (expRevisar == null)
                {*/

                if (User.Identity.GetUserName() == "02290" || User.Identity.GetUserName() == "02305" || User.Identity.GetUserName() == "02294")
                {
                    return RedirectToAction("SOAP2/" + id, "ServiciosMedicos");
                }
                else
                {
                    if (User.IsInRole("ServiciosMedicos") || User.IsInRole("Urgencias"))
                    {
                        var fecha2 = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
                        Models.SMDEVEntities19 db4 = new Models.SMDEVEntities19();
                        string query = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "' and Tipo != '00'";
                        var result = db4.Database.SqlQuery<Citas>(query);
                        var res = result.FirstOrDefault();

                        //System.Diagnostics.Debug.WriteLine(res);

                        //LINEAS DE MILTON
                        //dbMilton.Database.ExecuteSqlCommand("UPDATE TblTriage SET PasarASoap = CONVERT(VARCHAR(5),getdate(),108) WHERE Expediente = '" + id + "' and Fecha = '" + fechaMilton + "'");
                        //var soap = (from a in dbMilton.TblTriage
                        //            where a.Expediente == id &&
                        //            a.Fecha >= fechaHoyMilton
                        //            select a).FirstOrDefault();

                        //if (soap.PasarASoap == null)
                        //{
                        //    soap.PasarASoap = "1";
                        //    dbMilton.SaveChanges();
                        //}

                        if (res != null)
                        {
                            //Si es presencial
                            if (res.tipo == "11")
                            {
                                //System.Diagnostics.Debug.WriteLine(res.hr_llamado);
                                //Si no ha llegado el paciente a recepcion o no lo haz llamado
                                if (res.hora_recepcion == null || res.hr_llamado == null)
                                {
                                    //System.Diagnostics.Debug.WriteLine(res.hora_recepcion);

                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " es presencial, haz clic en el boton llamar antes de iniciar la nota médica";
                                    return RedirectToAction("Citas", "ServiciosMedicos");
                                    //return RedirectToAction("Index", "Home");
                                }
                                //Si ya llego
                                else
                                {
                                    //Se va a actualizar citas
                                    var hr_consultorio = DateTime.Now.ToString("HH:mm");
                                    //System.Diagnostics.Debug.WriteLine(hr_consultorio);
                                    if (res.hr_consultorio == null)
                                    {
                                        db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "' and Tipo != '00' and fecha_kiosco is not null");
                                    }

                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                                    var diagnosticos = (from q in db2.DIAGNOSTICOS
                                                        where q.DescCompleta != null
                                                        where q.Clave != null
                                                        || q.Clave != ""
                                                        select new
                                                        {
                                                            CheckBox = "",
                                                            Clave = q.Clave,
                                                            DescCompleta = q.DescCompleta,
                                                            DesCorta = q.DesCorta
                                                        })
                                                        //.Skip(1)
                                                        .ToList();

                                    //System.Diagnostics.Debug.WriteLine(diagnosticos);

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
                                                         .OrderBy(g => g.descripcion)
                                                        .ToList();

                                    string jsonEsp = JsonConvert.SerializeObject(especialidades);
                                    string pathEsp = Server.MapPath("~/json/");
                                    System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                                    return View(dhabientes);
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
                                    db4.Database.ExecuteSqlCommand("UPDATE CITAS SET hr_consultorio = CONVERT(VARCHAR(5),getdate(),108) WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'");
                                }

                                Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                var dhabientes = (from a in db.DHABIENTES
                                                  where a.NUMEMP == id
                                                  select a).FirstOrDefault();

                                Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                                var diagnosticos = (from q in db2.DIAGNOSTICOS
                                                    where q.DescCompleta != null
                                                    where q.Clave != null
                                                    || q.Clave != ""
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
                                                      .OrderBy(g => g.descripcion)
                                                    .ToList();

                                string jsonEsp = JsonConvert.SerializeObject(especialidades);
                                string pathEsp = Server.MapPath("~/json/");
                                System.IO.File.WriteAllText(pathEsp + "especialidades.json", jsonEsp);


                                return View(dhabientes);


                            }

                        }
                        else
                        {
                            string query2 = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + id + "' and Medico = '" + User.Identity.GetUserName() + "' and Fecha = '" + fecha2 + "'";
                            var result2 = db4.Database.SqlQuery<Citas>(query2);
                            var res2 = result2.FirstOrDefault();

                            if (res2 != null)
                            {


                                if (res2.tipo == "00")
                                {
                                    Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                                    var dhabientes = (from a in db.DHABIENTES
                                                      where a.NUMEMP == id
                                                      select a).FirstOrDefault();

                                    TempData["message_warning"] = "La cita de " + dhabientes.NOMBRES + " " + dhabientes.APATERNO + " " + dhabientes.AMATERNO + " fue cancelada";
                                    return RedirectToAction("Citas", "ServiciosMedicos");
                                }
                                else
                                {
                                    //No tiene o llegaron tarde
                                    return RedirectToAction("Index", "Home");
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
                        if (User.IsInRole("SinAgenda") || User.IsInRole("Subrogados"))
                        {
                            //LINEAS DE MILTON
                            //dbMilton.Database.ExecuteSqlCommand("UPDATE TblTriage SET PasarASoap = CONVERT(VARCHAR(5),getdate(),108) WHERE Expediente = '" + id + "' and Fecha = '" + fechaMilton + "'");
                            //var soap = (from a in dbMilton.TblTriage
                            //            where a.Expediente == id &&
                            //            a.Fecha >= fechaHoyMilton
                            //            select a).FirstOrDefault();

                            //if (soap.PasarASoap == null)
                            //{
                            //    soap.PasarASoap = "1";
                            //    dbMilton.SaveChanges();
                            //}

                            Models.SMDEVEntities20 db = new Models.SMDEVEntities20();
                            var dhabientes = (from a in db.DHABIENTES
                                              where a.NUMEMP == id
                                              select a).FirstOrDefault();

                            Models.SMDEVEntities23 db2 = new Models.SMDEVEntities23();


                            var diagnosticos = (from q in db2.DIAGNOSTICOS
                                                where q.DescCompleta != null
                                                where q.Clave != null
                                                || q.Clave != ""
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
                                                  .OrderBy(g => g.descripcion)
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

                /*}
                else
                {
                    TempData["message_warning"] = "";
                    return RedirectToAction("Citas", "ServiciosMedicos");
                }*/

            }
        }

        /****                                                  PANTALLA SOAP TRIAGE ***************************   FIN  *******************/


        public ActionResult ObtenerPacientesModal()
        {
            var fechaHoy = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaHoyDT = DateTime.Parse(fechaHoy);

            try
            {
                var query = (from a in db.TblTriage
                             where a.Fecha >= fechaHoyDT
                             //where a.EstatusConsulta == "0"
                             where a.PasarASoap == null
                             where a.EstatusConsulta != "2"
                             select new
                             {
                                 a.Id,
                                 a.NombrePaciente,
                                 a.Fecha,
                             }).ToList();

                var results1 = new List<TriageList>();

                foreach (var q in query)
                {
                    var resultado = new TriageList
                    {
                        Id = q.Id,
                        NombrePaciente = q.NombrePaciente,
                        Fecha = string.Format("{0:d/M/yyyy hh:mm tt}", q.Fecha),
                    };
                    results1.Add(resultado);
                }

                return Json(new { MENSAJE = "FOUND", PAC = results1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EliminarPaciente(int Paciente)
        {
            try
            {
                var RegistroTriage = (from a in db.TblTriage
                                      where a.Id == Paciente
                                      select a).FirstOrDefault();

                RegistroTriage.EstatusConsulta = "2";
                
                db.SaveChanges();
                
                return Json(new { MENSAJE = "Succe: Se eliminó el paciente" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { MENSAJE = "Error: Error de sistema: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
    

}
