
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
    public class KioscosController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        // GET: Kioscos
        public ActionResult KioscoCitas()
        {
            if (User.IsInRole("Kioscos"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public ActionResult KioscoRecetas()
        {
            if (User.IsInRole("Kioscos"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }


        public JsonResult ConfirmarCitaKiosco(string numemp)
        {
            //Base de datos
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();

            //System.Diagnostics.Debug.WriteLine(numemp);
            var ip_realiza = Request.UserHostAddress;
            //Buscar si tiene una cita
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00");
            var fecha_kiosco = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            string query = "select CITAS.fecha_kiosco as fecha_kiosco, CITAS.fecha_tarde as fecha_tarde, CITAS.Tipo as tipo, CITAS.turno as turno, CITAS.hora_recepcion as hora_recepcion, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as Medico, DHABIENTES.Nombres as Nombres, CITAS.turno as turno from CITAS INNER JOIN MEDICOS ON MEDICOS.Numero = CITAS.Medico INNER JOIN DHABIENTES ON DHABIENTES.NUMEMP = CITAS.NumEmp WHERE CITAS.NumEmp = '" + numemp + "' and CITAS.Fecha = '" + fecha + "' and CITAS.fecha_kiosco is null and CITAS.fecha_tarde is null ORDER BY CITAS.turno";
            var result = db.Database.SqlQuery<Citas>(query);
            var res = result.FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(res.fecha_kiosco);


            if (res != null)
            {
                //System.Diagnostics.Debug.WriteLine(res.tipo);
                //SI ES AGREGADO NO TOMES EN CUENTA SI LLEGA TARDE O NO
                if (res.tipo == "25" || res.tipo == "06")
                {
                    //Tomar la hora de la cita y validar que no se le haya pasado mas de 5 minutos
                    var turno1 = res.turno.Substring(0, 2);
                    var turno2 = res.turno.Substring(2, 2);
                    var turno = DateTime.Now.AddMinutes(-5).ToString(turno1 + ":" + turno2);
                    var turnoDT = DateTime.Parse(turno);

                    var hora_recepcion = DateTime.Now.ToString("HH:mm");
                    var hora_recepcionDT = DateTime.Parse(hora_recepcion);

                    //Si no tiene una hora de recepcion es decir que no la ha pasado
                    if (res.hora_recepcion == null || res.hora_recepcion == "" || res.hora_recepcion == "     " || res.hora_recepcion == " ")
                    {
                        db.Database.ExecuteSqlCommand("UPDATE CITAS SET hora_recepcion = '" + hora_recepcion + "', fecha_kiosco = '" + fecha_kiosco + "', ip_kiosco = '" + ip_realiza + "' WHERE NumEmp = '" + numemp + "' and Fecha = '" + fecha + "' AND turno = '" + res.turno + "'");

                        //var resultado = "<div class='row m-0'><div class='col-md-12 pt-4 pb-4'><h2>¡Hola <b>" + res.Nombres + "</b>!</h2><h4 class='mt-4'>tu cita con <b>" + res.Medico + "</b> a las <b>" + turno + "</b> ha sido confirmada, toma asiento en la sala de espera </h4><img src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>";

                        var resultado = new
                        {
                            html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + res.Nombres + "</b>!</h2><h4 class='mt-2 mt-2 mb-2'>tu cita con <b>" + res.Medico + "</b> a las <b>" + turno + "</b> ha sido confirmada, toma asiento en la sala de espera </h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>",
                            icon = "success",

                        };

                        return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    //ya la confirmo
                    else
                    {
                        //var resultado = "<div class='row m-0'><div class='col-md-12 pt-4 pb-4'><h2>¡Hola <b>" + res.Nombres + "</b>!</h2><h4 class='mt-4'>ya confirmaste tu cita, toma asiento en la sala de espera</h4><img src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>";

                        var resultado = new
                        {
                            html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + res.Nombres + "</b>!</h2><h4 class='mt-2 mt-2 mb-2'>ya confirmaste tu cita, toma asiento en la sala de espera</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>",
                            icon = "warning",

                        };


                        return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }

                }
                else
                {
                    //CITAS TELEFONICAS 
                    if (res.tipo == "12")
                    ///if (res.tipo == "13" || res.tipo == "25" || res.tipo == "12" || res.tipo == "01" || res.tipo == "02" || res.tipo == "08" || res.tipo == "09")
                    {
                        var turno1 = res.turno.Substring(0, 2);
                        var turno2 = res.turno.Substring(2, 2);
                        var turno = DateTime.Now.ToString(turno1 + ":" + turno2);

                        var resultado = new
                        {
                            html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + res.Nombres + "</b>!</h2><h4 class='mt-2 mt-2 mb-2'>tu cita con <b>" + res.Medico + "</b> a las <b>" + turno + "</b> es <b>TELEFÓNICA</b></h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/phone-call.png'></div></div>",
                            icon = "error",
                        };

                        return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        //CITAS NORMALES
                        if (res.hora_recepcion == null || res.hora_recepcion == "" || res.hora_recepcion == "     " || res.hora_recepcion == " ")
                        {
                            //Tomar la hora de la cita y validar que no se le haya pasado mas de 5 minutos
                            var turno1 = res.turno.Substring(0, 2);
                            var turno2 = res.turno.Substring(2, 2);
                            var turno = DateTime.Now.ToString(turno1 + ":" + turno2);
                            var turnoDT = DateTime.Parse(turno);

                            //System.Diagnostics.Debug.WriteLine(turno);


                            DateTime turnoCita = DateTime.Parse(turno);
                            DateTime turnoMax = turnoCita.AddMinutes(-5);
                            //DateTime turnoMax = turnoCita.AddHours(3);
                            DateTime turnoMin = turnoCita.AddMinutes(-40);

                            //System.Diagnostics.Debug.WriteLine(turnoMax);
                            //System.Diagnostics.Debug.WriteLine(turnoMin);

                            var hora_recepcion = DateTime.Now.ToString("HH:mm");
                            var hora_recepcionDT = DateTime.Parse(hora_recepcion);

                            //System.Diagnostics.Debug.WriteLine(hora_recepcionDT);


                            if (hora_recepcionDT <= turnoMax && hora_recepcionDT >= turnoMin)
                            {
                                //LLEGO A TIEMPO
                                //System.Diagnostics.Debug.WriteLine(turnoDT);
                                db.Database.ExecuteSqlCommand("UPDATE CITAS SET hora_recepcion = '" + hora_recepcion + "', fecha_kiosco = '" + fecha_kiosco + "', ip_kiosco = '" + ip_realiza + "' WHERE NumEmp = '" + numemp + "' and Fecha = '" + fecha + "' AND turno = '" + res.turno + "'");

                                //var resultado = "<div class='row m-0'><div class='col-md-12 pt-4 pb-4'><h2>¡Hola <b>" + res.Nombres + "</b>!</h2><h4 class='mt-4'>tu cita con <b>" + res.Medico + "</b> a las <b>" + turno + "</b> ha sido confirmada, toma asiento en la sala de espera </h4><img src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>";


                                var resultado = new
                                {
                                    html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + res.Nombres + "</b>!</h2><h4 class='mt-2 mb-2'>tu cita con <b>" + res.Medico + "</b> a las <b>" + turno + "</b> ha sido confirmada, toma asiento en la sala de espera </h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>",
                                    icon = "success",

                                };



                                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


                            }
                            else
                            {
                                //O LLEGO MUY TEMPRANO
                                if (hora_recepcionDT < turnoMin)
                                {
                                    //System.Diagnostics.Debug.WriteLine(hora_recepcionDT);
                                    //Buscar derecho habiente
                                    var dhabiente = (from d in db.DHABIENTES
                                                     where d.NUMEMP == numemp
                                                     select d).FirstOrDefault();

                                    var minutosRestantes = turnoMin - hora_recepcionDT;
                                    //System.Diagnostics.Debug.WriteLine(minutosRestantes);
                                    var minutosRestantesString = minutosRestantes.ToString();
                                    var split = minutosRestantesString.Split(':');
                                    var hora = Int16.Parse(split[0]);
                                    var minuto = Int16.Parse(split[1]);
                                    var horaTot = hora * 60;
                                    var minRes = horaTot + minuto;

                                    //var resultado = "<div class='row m-0'><div class='col-md-12 pt-4 pb-4'><h2>¡Hola <b>" + dhabiente.NOMBRES + "</b>!</h2><h4 class='mt-4'>asegurate de confirmar tu cita entre 40 a 5 minutos antes, vuelve a confirmar tu cita en <b>" + minRes + "</b> minutos </h4><img src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>";

                                    var resultado = new
                                    {
                                        html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + dhabiente.NOMBRES + "</b>!</h2><h4 class='mt-2 mb-2'>asegurate de confirmar tu cita entre 40 a 5 minutos antes, vuelve a confirmar tu cita en <b>" + minRes + "</b> minutos </h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>",
                                        icon = "error",

                                    };


                                    return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                                }
                                else
                                {
                                    //O LLEGO TARDE
                                    var dhabiente = (from d in db.DHABIENTES
                                                     where d.NUMEMP == numemp
                                                     select d).FirstOrDefault();

                                    //PONER FECHA DE QUE LLEGO TARDE y ESTATUS DE QUE LA CITA ESTA CANCELADA
                                    db.Database.ExecuteSqlCommand("UPDATE CITAS SET Tipo = '00', fecha_tarde = '" + fecha_kiosco + "', ip_kiosco = '" + ip_realiza + "' WHERE NumEmp = '" + numemp + "' and Fecha = '" + fecha + "' AND turno = '" + res.turno + "'");


                                    //var resultado = "<div class='row m-0'><div class='col-md-12 pt-4 pb-4'><h2>¡Hola <b>" + dhabiente.NOMBRES + "</b>!</h2><h4 class='mt-4'>tu cita con <b>" + res.Medico + "</b> a las <b>" + turno + "</b> ha expirado, reportate a citas para reagendar</h4><img src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>";

                                    var resultado = new
                                    {
                                        html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + dhabiente.NOMBRES + "</b>!</h2><h4 class='mt-2 mb-2'>tu cita con <b>" + res.Medico + "</b> a las <b>" + turno + "</b> ha expirado, reportate a citas para reagendar</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>",
                                        icon = "error",
                                    };

                                    return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                                }

                            }


                        }
                        else
                        {
                            //YA CONFIRMASTE LA CITA

                            //var resultado = "<div class='row m-0'><div class='col-md-12 pt-4 pb-4'><h2>¡Hola <b>" + res.Nombres + "</b>!</h2><h4 class='mt-4'>ya confirmaste tu cita, toma asiento en la sala de espera</h4><img src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>";

                            var resultado = new
                            {
                                html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + res.Nombres + "</b>!</h2><h4 class='mt-2 mb-2'>ya confirmaste tu cita, toma asiento en la sala de espera</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>",
                                icon = "warning",
                            };

                            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


                        }

                    }

                }


            }
            else
            {
                //Buscar si ya las registro, si ya tiene fecha_kiosco o fecha_tarde
                string query2 = "select CITAS.fecha_kiosco as fecha_kiosco, CITAS.fecha_tarde as fecha_tarde, CITAS.Tipo as tipo, CITAS.turno as turno, CITAS.hora_recepcion as hora_recepcion, MEDICOS.Nombre + ' ' + MEDICOS.Apaterno + ' ' + MEDICOS.Amaterno as Medico, DHABIENTES.Nombres as Nombres, CITAS.turno as turno from CITAS INNER JOIN MEDICOS ON MEDICOS.Numero = CITAS.Medico INNER JOIN DHABIENTES ON DHABIENTES.NUMEMP = CITAS.NumEmp WHERE CITAS.NumEmp = '" + numemp + "' and CITAS.Fecha = '" + fecha + "' ORDER BY CITAS.turno";
                var result2 = db.Database.SqlQuery<Citas>(query2);
                var res2 = result2.FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(res2.fecha_kiosco);

                if (res2 != null)
                {

                    //YA LA CONFIRMO
                    if (res2.fecha_kiosco != null)
                    {

                        //var resultado = "<div class='row m-0'><div class='col-md-12 pt-4 pb-4'><h2>¡Hola <b>" + res2.Nombres + "</b>!</h2><h4 class='mt-4'>ya confirmaste tu cita, toma asiento en la sala de espera</h4><img src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>";

                        var resultado = new
                        {
                            html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + res2.Nombres + "</b>!</h2><h4 class='mt-2 mb-2'>ya confirmaste tu cita, toma asiento en la sala de espera</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>",
                            icon = "warning",
                        };

                        return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                    }
                    //SI TIENE UNA FECHA TARDE QUE TE MARQUE QUE REAGENDE
                    else
                    {
                        var turno1 = res2.turno.Substring(0, 2);
                        var turno2 = res2.turno.Substring(2, 2);
                        var turno = DateTime.Now.ToString(turno1 + ":" + turno2);

                        //var resultado = "<div class='row m-0'><div class='col-md-12 pt-4 pb-4'><h2>¡Hola <b>" + res2.Nombres + "</b>!</h2><h4 class='mt-4'>tu cita con <b>" + res2.Medico + "</b> a las <b>" + turno + "</b> ha expirado, reportate a citas para reagendar</h4><img src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>";

                        var resultado = new
                        {
                            html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + res2.Nombres + "</b>!</h2><h4 class='mt-2 mb-2'>tu cita con <b>" + res2.Medico + "</b> a las <b>" + turno + "</b> ha expirado, reportate a citas para reagendar</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>",
                            icon = "error",
                        };


                        return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
                else
                {
                    var dhabiente = (from d in db.DHABIENTES
                                     where d.NUMEMP == numemp
                                     select d).FirstOrDefault();

                    //var resultado = "<div class='row m-0'><div class='col-md-12 pt-4 pb-4'><h2>¡Hola <b>" + dhabiente.NOMBRES + "</b>!</h2><h4 class='mt-4'>no tienes ninguna cita el día de hoy</h4><img src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>";

                    var resultado = new
                    {
                        html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + dhabiente.NOMBRES + "</b>!</h2><h4 class='mt-2 mb-2'>no tienes ninguna cita el día de hoy</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/iconos/sala-espera.png'></div></div>",
                        icon = "error",
                    };

                    return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

            }

        }


        public JsonResult ConfirmarRecetasKiosco(string numemp)
        {
            //Base de datos
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();
            //Base de datos prueba (aqui se guardan las recetas de HU)
            Models.FARMACIAHUEntities1 db3 = new Models.FARMACIAHUEntities1();


            //System.Diagnostics.Debug.WriteLine(numemp);

            //Buscar si tiene una cita
            var hora_rcta = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaMasCinco = DateTime.Now.AddDays(5).ToString("yyyy-MM-ddT00:00:00.000");
            var fechaMasCincoDT = DateTime.Parse(fechaMasCinco);
            var fechaMenosCinco = DateTime.Now.AddDays(-5).ToString("yyyy-MM-ddT00:00:00.000");
            var fechaMenosCincoDT = DateTime.Parse(fechaMenosCinco);
            //var fecha_kiosco = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var FechaMesMenos = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-ddTHH:mm:ss");
            var FechaMesMenosDT = DateTime.Parse(FechaMesMenos);

            var FechaMesMenos5Dias = DateTime.Now.AddMonths(-1).AddDays(-5).ToString("yyyy-MM-ddTHH:mm:ss");
            var FechaMesMenos5DiasDT = DateTime.Parse(FechaMesMenos5Dias);
            var FechaMesMas5Dias = DateTime.Now.AddMonths(-1).AddDays(5).ToString("yyyy-MM-ddTHH:mm:ss");
            var FechaMesMas5DiasDT = DateTime.Parse(FechaMesMas5Dias);



            //Dhabientes
            var dhabiente = (from r in db.DHABIENTES
                             where r.NUMEMP == numemp
                             select r).FirstOrDefault();

            //SI NO ENCUENTRAS NINGUN DERECHOHABIENTE
            if (dhabiente == null)
            {
                //NO EXISTE EL DHABIENTE
                var resultado = new
                {
                    html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola! Parece que tu numero de expediente no fue encontrado, vuelva a pasar la receta o teclea tu número de expediente</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/medicamento.png'></div></div>",
                    icon = "error",

                };

                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                //Contar recetas normales
                var recetasCount = (from r in db2.Receta_Exp
                                    where r.Expediente == numemp
                                    where r.Fecha >= FechaMesMenosDT
                                    where r.Estado == "7"
                                    where r.folio_rc == null
                                    select r).Count();

                //Insertar turnofar
                var turnoFar = (from r in db2.Receta_Exp
                                select r)
                               .OrderByDescending(g => g.TurnoFar)
                               .FirstOrDefault();

                var turnoFarRes = (from r in db.receta_exp_crn
                                   select r)
                               .OrderByDescending(g => g.turnofar)
                               .FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(turnoFar.TurnoFar);

                var turnoFarNuevo = turnoFar.TurnoFar + 1;

                var fecha_kiosco = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");


                //RECETAS NORMALES
                if (recetasCount > 0)
                {

                    //var fecha_kiosco = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
                    //Actualizar recetas normales 
                    db2.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET TurnoFar = '" + turnoFarNuevo + "', Fecha_Kiosco = '" + fecha_kiosco + "' WHERE Fecha >= '" + FechaMesMenos + "' and Expediente = '" + numemp + "' and Estado = '7' and folio_rc is null");

                }



                //----RESURTIMIENTO PRIMERA VEZ
                /*var recetasRsCount = (from r in db.receta_exp_crn
                                      where r.expediente == numemp
                                      where r.fecha_crea <= fechaMasCincoDT
                                      && r.fecha_crea >= fechaMenosCincoDT
                                      where r.terminada == 1
                                      where r.fecha_kiosco == null
                                      select r).Count();


                //Si si encuentra (primer evento)
                if(recetasRsCount > 0)
                {
                        //Enlista las recetas
                        var recetasRsList = (from r in db.receta_exp_crn
                                             where r.expediente == numemp
                                             where r.fecha_crea <= fechaMasCincoDT
                                             && r.fecha_crea >= fechaMenosCincoDT
                                             where r.terminada == 1
                                             where r.fecha_kiosco == null
                                             select r).ToList();

                        //System.Diagnostics.Debug.WriteLine(recetasRsList);

                        //Ponerle a todas las que encuentre la fecha de kiosco
                        foreach (var rctRsList in recetasRsList)
                        {

                            //Actualiza la fecha de kiosco
                            db.Database.ExecuteSqlCommand("UPDATE receta_exp_crn SET fecha_kiosco = '" + fecha_kiosco + "', turnofar = "+ turnoFarNuevo +" WHERE folio_rc = '" + rctRsList.folio_rc + "'");

                        }
                }




                //----RESURTIMMIENTO registrar segundo evento
                //Buscar si tiene resurtimiento de hace un mes para insertarlo en la tabla de farmacia de Recetas
                var recetasRsCount2 = (from r in db.receta_exp_crn
                                      where r.expediente == numemp
                                      where r.fecha_crea >= FechaMesMenos5DiasDT
                                      && r.fecha_crea <= FechaMesMas5DiasDT
                                      where r.terminada == 1
                                       //where r.fecha_kiosco != null
                                       select r).Count();

                if (recetasRsCount2 > 0) { 

                        var recetasRsList2 = (from r in db.receta_exp_crn
                                               where r.expediente == numemp
                                               where r.fecha_crea >= FechaMesMenos5DiasDT
                                               && r.fecha_crea <= FechaMesMas5DiasDT
                                               where r.terminada == 1
                                              //where r.fecha_kiosco != null
                                              select r).ToList();

                        //Por cada que encuentres
                        foreach(var rctRsLst2 in recetasRsList2)
                        {
                            //Inserta la receta en la tabla de farmacia

                            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                            var dir_ip = Request.UserHostAddress;

                            db2.Database.ExecuteSqlCommand("INSERT INTO Receta_Exp (Expediente, Medico, Fecha, Estado, Hora_Rcta, folio_rc, Dir_Ip, TurnoFar, Fecha_Kiosco) VALUES ('" + rctRsLst2.expediente + "', '" + rctRsLst2.medico_crea + "', '" + fecha + "', '7', '" + hora_rcta + "', '" + rctRsLst2.folio_rc + "', '" + dir_ip + "', '"+ turnoFarNuevo + "', '" + fecha_kiosco + "')");

                            //Tomar Folio de la ultima receta que se registro 
                            var folioReceta = (from r in db2.Receta_Exp
                                               where r.folio_rc == rctRsLst2.folio_rc
                                               where r.Expediente == numemp
                                               select r)
                                               .OrderBy(g => g.Folio_Rcta)
                                               .FirstOrDefault();

                            //Tomar el detalle de la receta
                            var recetasRsDts2 = (from r in db.receta_detalle_crn
                                             where r.folio_rc == rctRsLst2.folio_rc
                                             select r).ToList();


                            foreach (var rctRsDts2 in recetasRsDts2)
                            {
                            
                                //Insertar el detalle de la receta
                                db2.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES ('" + folioReceta.Folio_Rcta + "', '" + rctRsDts2.codigo + "', '" + rctRsDts2.cantidad + "', '" + rctRsDts2.dosis + "', '" + rctRsDts2.indicaciones + "')");
                            }

                        }


                }



                //---DE TRES EN ADELANETA DE RESURTIMIENTOS
                //Buscar en la tabla de recetas normales 
                var recetasRsCount3 = (from r in db2.Receta_Exp
                                       where r.Expediente == numemp
                                       where r.Fecha >= FechaMesMenos5DiasDT
                                       && r.Fecha <= FechaMesMas5DiasDT
                                       where r.Estado == "7"
                                       where r.folio_rc != null
                                       select r).Count();

                var resurtimientos3 = 0;

                if (recetasRsCount3 > 0)
                {
                    //Enlista las recetas que encuentres
                    var recetasRsList3 = (from r in db2.Receta_Exp
                                          where r.Expediente == numemp
                                          where r.Fecha >= FechaMesMenos5DiasDT
                                          && r.Fecha <= FechaMesMas5DiasDT
                                          where r.Estado == "7"
                                          where r.folio_rc != null
                                          select r).ToList();

                    //Por cada que encuentres
                    foreach (var rctRsLst3 in recetasRsList3)
                    {
                        //Buscar los eventos de esa receta con el folio_rc
                        var meses_surtir = (from r in db.receta_exp_crn
                                       where r.folio_rc == rctRsLst3.folio_rc
                                       select r).FirstOrDefault();

                        var eventos = recetasRsCount3 + 1;

                        if (eventos < meses_surtir.meses_surtir)
                        {
                            //Inserta la receta en la tabla de farmacia

                            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
                            var dir_ip = Request.UserHostAddress;

                            db2.Database.ExecuteSqlCommand("INSERT INTO Receta_Exp (Expediente, Medico, Fecha, Estado, Hora_Rcta, folio_rc, Dir_Ip, TurnoFar, Fecha_Kiosco) VALUES ('" + rctRsLst3.Expediente + "', '" + rctRsLst3.Medico + "', '" + fecha + "', '7', '" + hora_rcta + "', '" + rctRsLst3.folio_rc + "', '" + dir_ip + "', '" + turnoFarNuevo + "', '" + fecha_kiosco + "')");

                            //Tomar Folio de la ultima receta que se registro 
                            var folioReceta = (from r in db2.Receta_Exp
                                               where r.folio_rc == rctRsLst3.folio_rc
                                               where r.Expediente == numemp
                                               select r)
                                               .OrderBy(g => g.Folio_Rcta)
                                               .FirstOrDefault();

                            //Tomar el detalle de la receta
                            var recetasRsDts3 = (from r in db2.Receta_Detalle
                                                 where r.Folio_Rcta == folioReceta.Folio_Rcta
                                                 select r).ToList();


                            foreach (var rctRsDts3 in recetasRsDts3)
                            {

                                //Insertar el detalle de la receta
                                db2.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES ('" + folioReceta.Folio_Rcta + "', '" + rctRsDts3.Codigo + "', '" + rctRsDts3.Cantidad + "', '" + rctRsDts3.Dosis + "', '" + rctRsDts3.Indicaciones + "')");
                            }

                            resurtimientos3++;
                        }
                        

                    }

                }*/




                //---RECETAS DE HU
                //Buscar aparte si tiene recetas de HU
                string query = "select Expediente as Expediente, Medico as Medico, Fecha as Fecha, Estado as Estado, Hora_Rcta as Hora_Rcta from Receta_Exp WHERE Expediente = '" + numemp + "' and Fecha >= '" + FechaMesMenos + "' and Estado = '1'";
                var result = db3.Database.SqlQuery<Receta_Exp>(query);
                var recetasCountHU = result.Count();


                if (recetasCountHU > 0)
                {
                    //Enlista de las recetas de HU


                    string query2 = "select Folio_Rcta as Folio_Rcta, Expediente as Expediente, Medico as Medico, Fecha as Fecha, Estado as Estado, Hora_Rcta as Hora_Rcta, Dir_Ip as Dir_Ip from Receta_Exp WHERE Expediente = '" + numemp + "' and Fecha >= '" + FechaMesMenos + "' and Estado = '1'";
                    var result2 = db3.Database.SqlQuery<Receta_Exp>(query2);
                    var recetasListHU = result2.ToList();

                    //System.Diagnostics.Debug.WriteLine(recetasListHU);

                    //System.Diagnostics.Debug.WriteLine(dhabiente.NUMEMP);

                    //Por cada receta que encuentres del HU
                    foreach (var rctListHU in recetasListHU)
                    {
                        //Actualiza a estado 3 
                        db3.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Estado = '3' WHERE Folio_Rcta = '" + rctListHU.Folio_Rcta + "' and Expediente = '" + numemp + "'");

                        var fechaRcta = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");

                        //Inserta en la tabla de recetas de farmacia
                        db2.Database.ExecuteSqlCommand("INSERT INTO Receta_Exp (Expediente, Medico, Fecha, Estado, Hora_Rcta, folio_rcta_hu, Dir_Ip, Fecha_Kiosco, TurnoFar) VALUES ('" + dhabiente.NUMEMP + "', '" + rctListHU.Medico + "', '" + fechaRcta + "', '7', '" + hora_rcta + "', '" + rctListHU.Folio_Rcta + "', '" + rctListHU.Dir_Ip + "', '" + fecha_kiosco + "', '" + turnoFarNuevo + "')");


                        //DETALLES DE LA RECETA HU
                        string query3 = "select * from Receta_Detalle WHERE Folio_Rcta = '" + rctListHU.Folio_Rcta + "'";
                        var result3 = db3.Database.SqlQuery<Receta_Detalle>(query3);
                        var recetasDetallesListaHU = result3.ToList();


                        //DETALLES DE LA RECETA HU
                        foreach (var rctDtsListHU in recetasDetallesListaHU)
                        {


                            //TOMAR EL FOLIO DE LA ULTIMA RECETA GENERADA DE HU, QUE SE TOMARIA DEL INSERT DE ARRIBA
                            var folioReceta = (from r in db2.Receta_Exp
                                               where r.folio_rcta_hu == rctListHU.Folio_Rcta
                                               where r.Expediente == numemp
                                               select r)
                                              .OrderBy(g => g.Folio_Rcta)
                                              .FirstOrDefault();

                            //Insertar el detalle de la receta
                            db2.Database.ExecuteSqlCommand("INSERT INTO Receta_Detalle (Folio_Rcta, Codigo, Cantidad, Dosis, Indicaciones) VALUES ('" + folioReceta.Folio_Rcta + "', '" + rctDtsListHU.Codigo + "', '" + rctDtsListHU.Cantidad + "', '" + rctDtsListHU.Dosis + "', '" + rctDtsListHU.Indicaciones + "')");

                        }

                    }

                }



                //Sumar las recetas de SM y HU
                var recetasTotCount = recetasCountHU + recetasCount;
                var recetasRsCount = 0;
                var recetasRsCount2 = 0;
                var resurtimientos3 = 0;

                //Sumar las de resurtimiento
                var recetasTotCountRs = recetasRsCount + recetasRsCount2 + resurtimientos3;

                //TIENE NORMALES Y RESURTIMIENTO
                if (recetasTotCount > 0 && recetasTotCountRs > 0)
                {

                    var resultado = new
                    {
                        html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + dhabiente.NOMBRES + "</b>!</h2><h4 class='mt-2 mt-2 mb-2'>tienes <b>" + recetasTotCount + "</b> receta(s) activa(s) y <b> " + recetasTotCountRs + " </b> receta(s) de resurtimiento activa(s) pasa a farmacia por tus medicamentos</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/medicamento.png'></div></div>",
                        icon = "success",

                    };

                    return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    //SOLO TIENE NORMALES
                    if (recetasTotCount > 0)
                    {
                        var resultado = new
                        {
                            html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + dhabiente.NOMBRES + "</b>!</h2><h4 class='mt-2 mt-2 mb-2'>tienes <b>" + recetasTotCount + "</b> receta(s) activa(s), pasa a farmacia por tus medicamentos</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/medicamento.png'></div></div>",
                            icon = "success",

                        };

                        return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        if (recetasTotCountRs > 0)
                        {
                            var resultado = new
                            {
                                html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + dhabiente.NOMBRES + "</b>!</h2><h4 class='mt-2 mt-2 mb-2'>tienes <b> " + recetasTotCountRs + " </b> receta(s) de resurtimiento activa(s) pasa a farmacia por tus medicamentos</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/medicamento.png'></div></div>",
                                icon = "success",

                            };

                            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                        else
                        {
                            //No tiene recetas
                            var resultado = new
                            {
                                html = "<div class='row m-0'><div class='col-md-12 pt-0 pb-2'><h2>¡Hola <b>" + dhabiente.NOMBRES + "</b>!</h2><h4 class='mt-2 mt-2 mb-2'>no tienes ninguna receta activa para el día de hoy</h4><img style='width:70px;' src='http://148.234.143.203/SISM-Medico/Imagenes/medicamento.png'></div></div>",
                                icon = "warning",
                            };

                            return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }

                    }


                }



            }




        }


    }
}