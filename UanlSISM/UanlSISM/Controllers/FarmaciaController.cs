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
    //[Authorize]
    public class FarmaciaController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        // GET: Farmacia
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Pantalla()
        {
            return View();
        }

        public ActionResult PantallaSEMAC()
        {
            return View();
        }

        public class RecetasList
        {
            public int Folio_Rcta { get; set; }
            public string Paciente { get; set; }
            public string Estado { get; set; }
            public string Usuario { get; set; }
            public string Tipo { get; set; }
            public int? TurnoFar { get; set; }
            public string Fecha { get; set; }
            public string Fecha_Kiosco { get; set; }
            public string Demora { get; set; }
            public string Boton { get; set; }
        }

        public JsonResult RecetasPantallaSEMAC()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var horaDemora = DateTime.Now.AddMinutes(-5).ToString("HH:mm");
            var horaDemoraDT = DateTime.Parse(horaDemora);

            var kioscoHora = DateTime.Now.AddHours(-4).ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var kioscoHoraDT = DateTime.Parse(kioscoHora);
            //var horaDemoraInt = Int16.Parse(horaDemora);
            //System.Diagnostics.Debug.WriteLine(horaDemoraInt);


            var recetas = (from r in db2.Receta_Exp
                           where r.Fecha == fecha_correcta
                           where r.unidad_medica == 3
                           where r.Dir_Ip.StartsWith("148.234.218")
                           where r.Hora_Rcta != null 
                           where r.Estado != "2"
                           && r.Estado != "3"
                           where r.Medico != "22019" || r.Medico != "02161" || r.Medico != "22016" || r.Medico != "22011"
                           select new
                           {
                               Folio_Rcta = r.Folio_Rcta,
                               Paciente = r.Expediente,
                               //Paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                               Estado = r.Estado,
                               //Usuario = usuarioIn.Usu_Nombre,
                               Tipo = r.Tipo,
                               TurnoFar = r.TurnoFar,
                               Hora_Rcta = r.Hora_Rcta,
                               Fecha_Kiosco = r.Fecha_Kiosco,

                           }).ToList();

            //System.Diagnostics.Debug.WriteLine(recetas);


            var results = new List<RecetasList>();

            foreach (var item in recetas)
            {

                var dhabiente = (from a in db.DHABIENTES
                                 where a.NUMEMP == item.Paciente
                                 //where a.hora_termino == null
                                 select a).FirstOrDefault();

                string queryCita = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + item.Paciente + "' and Fecha = '" + fecha + "'";
                var resultCita = db.Database.SqlQuery<Citas>(queryCita);
                var resCita = resultCita.FirstOrDefault();


                string querySAL = "select folio_rcta as folio_rcta, realiza as realiza, estado as estado from sal_cu WHERE folio_rcta = '" + item.Folio_Rcta + "'";
                var resultSAL = db2.Database.SqlQuery<Sal_CU>(querySAL);
                var resSAL = resultSAL.FirstOrDefault();

                //var fechaKiosco = string.Format("{0:HH:mm}", item.Fecha_Kiosco, new CultureInfo("es-ES"));
                //var fechaKioscoDT = DateTime.Parse(fechaKiosco);

                var fechaKiosco = string.Format("{0:HH:mm}", item.Hora_Rcta, new CultureInfo("es-ES"));
                var fechaKioscoDT = DateTime.Parse(fechaKiosco);
                //var fechaKioscoInt = Int16.Parse(fechaKiosco);
                //var demora = fechaKioscoDT - horaDemoraDT;

                //var demoraString = demora.ToString();
                //var demoraSplit = demoraString.Split(':');


                //System.Diagnostics.Debug.WriteLine(demoraSplit);

                string usuario = "";
                string estado = "";
                string demoraTxt = "";
                string citaTipo = "";
                string estadoRcta = "0";


                if (resSAL != null)
                { 
                
                    if(resSAL.estado == "1")
                    {
                        estadoRcta = "1";
                    }
                    else
                    {
                        estadoRcta = "0";
                    }

                }
                else
                {
                    estadoRcta = "0";
                }



                if (resCita != null)
                {
                    if (resCita.tipo == "13" || resCita.tipo == "12" || resCita.tipo == "01" || resCita.tipo == "02" || resCita.tipo == "08" || resCita.tipo == "09")
                    {
                        citaTipo = "Telefonica";
                    }
                    else
                    {
                        citaTipo = "Presencial";
                    }
                }
                else
                {
                    citaTipo = "Presencial";
                }


                if (fechaKioscoDT > horaDemoraDT)
                {

                    if (resSAL == null)
                    {
                        usuario = "Pendiente";
                        estado = "Pendiente";
                        var demora = fechaKioscoDT - horaDemoraDT;
                        var demoraString = demora.ToString();
                        var demoraSplit = demoraString.Split(':');
                        demoraTxt = demoraSplit[1];
                    }
                    else
                    {
                        if(resSAL.estado != "1")
                        {
                            var demora = fechaKioscoDT - horaDemoraDT;
                            var demoraString = demora.ToString();
                            var demoraSplit = demoraString.Split(':');
                            demoraTxt = demoraSplit[1];
                            estado = "Surtiendo";
                            usuario = resSAL.realiza;
                        }
                    }

                }
                else
                {
                    if (resSAL == null)
                    {
                        usuario = "Demora";
                        estado = "Demora";
                        demoraTxt = "00";
                    }
                    else
                    {
                        if (resSAL.estado != "1") 
                        {
                            usuario = resSAL.realiza;
                            estado = "Surtiendo";
                            demoraTxt = "00";
                        }     
                    }
                }


                if(estadoRcta != "1")
                {

                    var result = new RecetasList
                    {
                        Folio_Rcta = item.Folio_Rcta,
                        Paciente = dhabiente.NOMBRES + " " + dhabiente.APATERNO + " " + dhabiente.AMATERNO,
                        Estado = estado,
                        Usuario = usuario,
                        Tipo = citaTipo,
                        TurnoFar = item.TurnoFar,
                        Fecha = string.Format("{0:dddd, dd MMMM yyyy HH:mm}", item.Fecha_Kiosco, new CultureInfo("es-ES")),
                        Demora = demoraTxt + " minutos",
                    };

                    results.Add(result);

                }
            }



            //RECETAS DE RESURTIMIENTO
            var recetasRs = (from r in db.receta_exp_crn
                                 //where r.fecha_crea == fecha_correcta
                             join dhabiente in db.DHABIENTES on r.expediente equals dhabiente.NUMEMP into dhabX
                             from dhabIn in dhabX.DefaultIfEmpty()
                                 //join usu in db2.Usuario on r.UserFar equals usu.UsuarioId into usuarioX
                                 //from usuarioIn in usuarioX.DefaultIfEmpty()
                                 //where r.TurnoFar != null
                             where r.fecha_kiosco != null
                             where r.fecha_kiosco >= kioscoHoraDT
                             where r.terminada == 1
                             //where r.medico_crea != "22019" || r.medico_crea != "02161" || r.medico_crea != "22016" || r.medico_crea != "22011"
                             //where r.Estado != "2"
                             //where r.Estado != "3"
                             select new
                             {
                                 Folio_Rcta = r.folio_rc,
                                 Paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                 //Paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                                 Estado = r.terminada,
                                 //Usuario = "Resurtimiento",
                                 //Tipo = r.Tipo,
                                 //TurnoFar = r.TurnoFar,
                                 //Fecha = r.Fecha,
                                 Fecha_Kiosco = r.fecha_kiosco,

                             }).ToList();

            //System.Diagnostics.Debug.WriteLine(recetasRs);


            foreach (var item in recetasRs)
            {

                var dhabiente = (from a in db.DHABIENTES
                                 where a.NUMEMP == item.Paciente
                                 //where a.hora_termino == null
                                 select a).FirstOrDefault();

                string queryCita = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + item.Paciente + "' and Fecha = '" + fecha + "'";
                var resultCita = db.Database.SqlQuery<Citas>(queryCita);
                var resCita = resultCita.FirstOrDefault();

                var fechaKiosco = string.Format("{0:HH:mm}", item.Fecha_Kiosco, new CultureInfo("es-ES"));
                var fechaKioscoDT = DateTime.Parse(fechaKiosco);
                //var fechaKioscoInt = Int16.Parse(fechaKiosco);




                //System.Diagnostics.Debug.WriteLine(horaDemoraDT);
                //System.Diagnostics.Debug.WriteLine(fechaKioscoDT);
                //System.Diagnostics.Debug.WriteLine(demora);

                //System.Diagnostics.Debug.WriteLine(demoraSplit);

                string usuario = "Resurtimiento";
                string estado = "";
                string demoraTxt = "";
                string citaTipo = "";

                if (resCita != null)
                {
                    if (resCita.tipo == "13" || resCita.tipo == "12" || resCita.tipo == "01" || resCita.tipo == "02" || resCita.tipo == "08" || resCita.tipo == "09")
                    {
                        citaTipo = "Telefonica";
                    }
                    else
                    {
                        citaTipo = "Presencial";
                    }
                }
                else
                {
                    citaTipo = "Presencial";
                }



                if (fechaKioscoDT > horaDemoraDT)
                {
                    var demora = fechaKioscoDT - horaDemoraDT;
                    var demoraString = demora.ToString();
                    var demoraSplit = demoraString.Split(':');
                    demoraTxt = demoraSplit[1];
                    estado = "Pendiente";
                }
                else
                {
                    estado = "Demora";
                    demoraTxt = "00";
                }


                var result = new RecetasList
                {
                    Folio_Rcta = item.Folio_Rcta,
                    Paciente = item.Paciente,
                    Estado = estado,
                    Usuario = usuario,
                    Tipo = citaTipo,
                    TurnoFar = 1,
                    Fecha = string.Format("{0:dddd, dd MMMM yyyy HH:mm}", item.Fecha_Kiosco, new CultureInfo("es-ES")),
                    Demora = demoraTxt + " minutos",
                };

                results.Add(result);

            }


            //System.Diagnostics.Debug.WriteLine(results);

            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        public JsonResult RecetasPantallaCountSEMAC()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fecha_correcta = DateTime.Parse(fecha);

            var horaDemora = DateTime.Now.AddMinutes(-5).ToString("HH:mm");
            var horaDemoraDT = DateTime.Parse(horaDemora);

            var kioscoHora = DateTime.Now.AddHours(-4).ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var kioscoHoraDT = DateTime.Parse(kioscoHora);
            //var horaDemoraInt = Int16.Parse(horaDemora);
            //System.Diagnostics.Debug.WriteLine(horaDemoraInt);


            var recetas = (from r in db2.Receta_Exp
                           where r.Fecha == fecha_correcta
                           //join dhabiente in db.DHABIENTES on r.Expediente equals dhabiente.NUMEMP into dhabX
                           //from dhabIn in dhabX.DefaultIfEmpty()
                           //join usu in db2.Usuario on r.UserFar equals usu.UsuarioId into usuarioX
                           //from usuarioIn in usuarioX.DefaultIfEmpty()
                           //where r.TurnoFar != null
                           //where r.Fecha_Kiosco != null
                           //where r.Fecha_Kiosco >= kioscoHoraDT
                           where r.unidad_medica == 3
                           where r.Dir_Ip.StartsWith("148.234.218")
                           //where r.Estado == "7" 
                           where r.Estado != "2"
                           && r.Estado != "3"
                           where r.Medico != "22019" || r.Medico != "02161" || r.Medico != "22016" || r.Medico != "22011"
                           select new
                           {
                               Folio_Rcta = r.Folio_Rcta,
                               Paciente = r.Expediente,
                               //Paciente = dhabIn.NOMBRES + " " + dhabIn.APATERNO + " " + dhabIn.AMATERNO,
                               Estado = r.Estado,
                               //Usuario = usuarioIn.Usu_Nombre,
                               Tipo = r.Tipo,
                               TurnoFar = r.TurnoFar,
                               Hora_Rcta = r.Hora_Rcta,
                               Fecha_Kiosco = r.Fecha_Kiosco,

                           }).ToList();

            //System.Diagnostics.Debug.WriteLine(recetas);


            //var results = new List<RecetasList>();

            int demoraCount = 0;
            int pendienteCount = 0;
            int surtiendoCount = 0;

            foreach (var item in recetas)
            {

                var dhabiente = (from a in db.DHABIENTES
                                 where a.NUMEMP == item.Paciente
                                 //where a.hora_termino == null
                                 select a).FirstOrDefault();

                string queryCita = "select hora_recepcion as hora_recepcion, tipo as tipo, hr_consultorio as hr_consultorio, hr_llamado as hr_llamado from CITAS WHERE NumEmp = '" + item.Paciente + "' and Fecha = '" + fecha + "'";
                var resultCita = db.Database.SqlQuery<Citas>(queryCita);
                var resCita = resultCita.FirstOrDefault();

                string querySAL = "select folio_rcta as folio_rcta, realiza as realiza, estado as estado from sal_cu WHERE folio_rcta = '" + item.Folio_Rcta + "'";
                var resultSAL = db2.Database.SqlQuery<Sal_CU>(querySAL);
                var resSAL = resultSAL.FirstOrDefault();

                var fechaKiosco = string.Format("{0:HH:mm}", item.Hora_Rcta, new CultureInfo("es-ES"));
                var fechaKioscoDT = DateTime.Parse(fechaKiosco);
                //var fechaKioscoInt = Int16.Parse(fechaKiosco);
                //var demora = fechaKioscoDT - horaDemoraDT;

                //var demoraString = demora.ToString();
                //var demoraSplit = demoraString.Split(':');


                //System.Diagnostics.Debug.WriteLine(demoraSplit);
                string estadoRcta = "0";


                if (resSAL != null)
                {

                    if (resSAL.estado == "1")
                    {
                        estadoRcta = "1";
                    }
                    else
                    {
                        estadoRcta = "0";
                    }

                }
                else
                {
                    estadoRcta = "0";
                }


                if(estadoRcta != "1") {

                    if (fechaKioscoDT > horaDemoraDT)
                    {

                        if (resSAL == null)
                        {
                            pendienteCount++;
                        }
                        else
                        {
                            if(resSAL.realiza != "1")
                            {
                                surtiendoCount++;
                            }
                        }
                    }
                    else
                    {
                        demoraCount++;
                    }

                }







                /*var result = new RecetasList
                {

                };

                results.Add(result);*/

            }




                var result = new Object();

            result = new
            {
                pendiente = pendienteCount,
                surtiendo = surtiendoCount,
                demora = demoraCount,
            };


            //System.Diagnostics.Debug.WriteLine(results);

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }


        public ActionResult InventarioLista()
        {
            if (User.IsInRole("InventarioLista"))
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
            public int inv_ent { get; set; }
            public int inv_sal { get; set; }
            public int inv_act { get; set; }
            public int manejodisponible { get; set; }
            public string clave { get; set; }
            public string descripcion_grupo { get; set; }
            public string descripcion_sal { get; set; }
            public string presentacion { get; set; }
            public string clave_nivel { get; set; }
            public string usuario_registra { get; set; }
            public string sobranteInv2022 { get; set; }
            public string temporal { get; set; }
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
            public string sobranteInv2022 { get; set; }
            public string temporal { get; set; }
        }

        /*
        public JsonResult ListInvent()
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            string usuario = User.Identity.GetUserName();

            //System.Diagnostics.Debug.WriteLine(id);

            string query = "SELECT InvFarm_1.Usuario_Registra AS usuario_registra, InvFarm_1.Id AS id, InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, InvFarm_1.ManejoDisponible AS manejodisponible, Sustancia_1.Clave AS clave, Grupo_1.Descripcion_Grupo AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.Presentacion AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, Nivel_1.Descripcion_Nivel FROM SERVMED.dbo.Grupo AS Grupo_1 INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.Id_Grupo INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1)";
            var result = db.Database.SqlQuery<LstInv>(query);
            var res = result.ToList();

            //System.Diagnostics.Debug.WriteLine(res);
            
            var invLista = new List<LstInv2>();

            foreach (var item in res)
            {
                var boton = "";

                if (User.Identity.Name == "22017" || User.Identity.Name == "direccion" || User.Identity.Name == "coordinacion" || User.Identity.Name == "22011" || User.Identity.Name == "sistemas" || User.Identity.Name == "jorge")
                {
                    boton = "...";
                }
                else
                {
                    if (item.usuario_registra != null)
                    {
                        boton = "<button data-id='" + item.id + "' class='btn btn-subir' style='background-color:green !important;'>Actualizar</button>";
                    }
                    else
                    {
                        boton = "<button data-id='" + item.id + "' class='btn btn-subir'>Subir</button>";
                    }
                }

                

                var inventario = new LstInv2
                {
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
        */

        public JsonResult ListInvent()
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            string usuario = User.Identity.GetUserName();

            //System.Diagnostics.Debug.WriteLine(id);

            string query = "SELECT Sustancia_1.SobranteInv2022 AS sobranteInv2022, InvFarm_1.Id_Sustancia as id_sustancia, InvFarm_1.Usuario_Registra AS usuario_registra, " +
                "InvFarm_1.Id AS id, InvFarm_1.Inv_Sal AS sal, InvFarm_1.Inv_Act AS inv_act, InvFarm_1.ManejoDisponible AS manejodisponible, Sustancia_1.Clave AS clave, " +
                "Grupo_1.descripcion AS descripcion_grupo, Sal_1.Descripcion_Sal AS descripcion_sal, Sustancia_1.descripcion_21 AS presentacion, Nivel_1.Clave_Nivel AS clave_nivel, " +
                "Nivel_1.Descripcion_Nivel as descripcion_nivel " +
                "FROM SERVMED.dbo.grupo_21 AS Grupo_1 " +
                "INNER JOIN SERVMED.dbo.Sustancia AS Sustancia_1 ON Grupo_1.Id = Sustancia_1.id_grupo_21 " +
                "INNER JOIN SERVMED.dbo.Sal AS Sal_1 ON Sustancia_1.Id_Sal = Sal_1.Id " +
                "INNER JOIN SERVMED.dbo.Nivel AS Nivel_1 ON Sustancia_1.Id_Nivel = Nivel_1.Id " +
                "INNER JOIN SERVMED.dbo.InvFarm AS InvFarm_1 ON Sustancia_1.Id = InvFarm_1.Id_Sustancia " +
                "INNER JOIN SERVMED.dbo.Inventario AS Inventario_1 " +
                "ON InvFarm_1.InvFarmId = Inventario_1.id WHERE(Inventario_1.status = 1) AND(Inventario_1.tipo = 1) and Sustancia_1.descripcion_21 is not null and Sustancia_1.descripcion_21 != ''";
            
            
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
                var temporal = "";
                if (item.sobranteInv2022 == "2 ")
                {
                    //System.Diagnostics.Debug.WriteLine(item.presentacion);
                    temporal = "Temporal (Fuera de cuadro)";
                }
                else
                {
                    temporal = "Cuadro Básico";
                }

                var inventario = new LstInv2
                {
                    id_sustancia = item.id_sustancia,
                    clave = item.clave,
                    descripcion_sal = item.descripcion_sal,
                    presentacion = item.presentacion,
                    descripcion_grupo = item.descripcion_grupo,
                    manejodisponible = item.inv_act,
                    sobranteInv2022 = item.sobranteInv2022,
                    temporal = temporal,
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

            db.Database.ExecuteSqlCommand("UPDATE InvFarm SET Inv_Ini = " + total + ", Inv_Act = " + total + ", ManejoDisponible = " + total + ", Usuario_Registra = '" + usuario + "' WHERE Id = '" + id + "' ");

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

        public class InvFarm2
        {
            public int Id { get; set; }
            public Int16 InvFarmId { get; set; }
            public DateTime Inv_Fecha { get; set; }
            public int Id_Sustancia { get; set; }
            public int Inv_Ini { get; set; }
            public int Inv_Ent { get; set; }
            public int Inv_Sal { get; set; }
            public int Inv_Act { get; set; }
            public int Inv_Reorden { get; set; }
            public int ManejoDisponible { get; set; }
            public string Usuario_Registra { get; set; }
        }


        [HttpPost]
        public ActionResult GenerarInventario()
        {

            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var usuario = User.Identity.GetUserName();

            var today = DateTime.Today;
            DateTime? date = DateTime.Now;
            var mes = date.Value.ToString("MMMM", CultureInfo.CreateSpecificCulture("es"));
            var mes2 = mes.Substring(0, 3);
            var mesfinal = mes2.ToUpper();
            var year = today.Year;



            //Tomar el inventario que esta activo
            string query = "select id as id, nom_invent as nom_invent, status as status, tipo as tipo from Inventario WHERE status = 1 and tipo = 1";
            var result = db.Database.SqlQuery<Inventario>(query);
            var res = result.FirstOrDefault();


            //Crear nuevo inventario, se pondrá en tipo 3 por lo pronto
            db.Database.ExecuteSqlCommand("INSERT INTO Inventario (nom_invent, status, tipo) VALUES('Far" + mesfinal + year + "', 0, 3)");

            //Hacer lista del inventario que esta activo
            string query2 = "select Id as Id, InvFarmId as InvFarmId, Inv_Fecha as Inv_Fecha, Id_Sustancia as Id_Sustancia, Inv_Ini as Inv_Ini, Inv_Ent as Inv_Ent, Inv_Sal as Inv_Sal, Inv_Act as Inv_Act, Inv_Reorden as Inv_Reorden, ManejoDisponible as ManejoDisponible, Usuario_Registra as Usuario_Registra from InvFarm WHERE InvFarmId = " + res.id + " ";
            var result2 = db.Database.SqlQuery<InvFarm2>(query2);
            var res2 = result2.ToList();
            var sustancia = (from q in db.Sustancia
                             where q.descripcion_21 != null && q.descripcion_21 != ""
                             //where q.Clave.StartsWith("99")
                             select q).ToList();


            //Buscar idinventario con tipo = 3 el nuevo de ARRIBA QUE SE CREO
            string query3 = "select id as id, nom_invent as nom_invent, status as status, tipo as tipo from Inventario WHERE status = 0 and tipo = 3";
            var result3 = db.Database.SqlQuery<Inventario>(query3);
            var res3 = result3.FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(sustancia);


            //Insertar todos de nuevo pero con el nuevo Id del inventario y con Inv_Ini, Inv_Ent y Inv_Sal en 0
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:00");
            foreach (var item in sustancia)
            {
                db.Database.ExecuteSqlCommand("INSERT INTO InvFarm(InvFarmId, Inv_Fecha, Id_Sustancia, Inv_Ini, Inv_Ent, Inv_Sal, Inv_Act, Inv_Reorden, ManejoDisponible) VALUES(81, '" + fecha + "', " + item.Id + ", 0, 0, 0, 0, 0, 0)");
            }

            //Desactivar el inventario actual
            db.Database.ExecuteSqlCommand("UPDATE Inventario SET status = 0 WHERE Id = '" + res.id + "' ");

            //Activar el creado
            db.Database.ExecuteSqlCommand("UPDATE Inventario SET status = 1, tipo = 1 WHERE Id = '" + res3.id + "' ");


            TempData["message_success"] = "Inventario generado con éxito";
            return Redirect(Request.UrlReferrer.ToString());

        }


        public JsonResult InventarioActivo()
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            //Tomar el inventario que esta activo
            string query = "select id as id, nom_invent as nom_invent, status as status, tipo as tipo from Inventario WHERE status = 1 and tipo = 1";
            var result = db.Database.SqlQuery<Inventario>(query);
            var res = result.FirstOrDefault();
            return new JsonResult { Data = res, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult InhabilitarMedicamentos()
        {
            if (User.IsInRole("InhabilitarMedicamentos"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public JsonResult InhabilitarMedicamento(string clave)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            var fechaDT = DateTime.Parse(fecha);
            string usuario = User.Identity.GetUserName();
            var ip_realiza = Request.UserHostAddress;

            //Buscar medicamento en la tabla de Inhabilitados
            var inmed = (from a in db.InhabilitarMedicamentos
                         where a.clave == clave
                         select a).FirstOrDefault();

            if(inmed == null)
            {
                InhabilitarMedicamentos medicamento = new InhabilitarMedicamentos();
                medicamento.clave = clave;
                medicamento.usuario = User.Identity.GetUserName();
                medicamento.fecha = fechaDT;
                medicamento.ip_realiza = ip_realiza;
                db.InhabilitarMedicamentos.Add(medicamento);
                db.SaveChanges();

                var resultado = "¡Medicamento inhabilitado con éxito!";
                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            else
            {
                //Se vuelve a habilitar
                db.Database.ExecuteSqlCommand("DELETE FROM InhabilitarMedicamentos WHERE clave = " + clave + " ");

                var resultado = "¡Medicamento habilitado con éxito!";
                return new JsonResult { Data = resultado, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }


            
        }


        public JsonResult ListaInventario()
        {
            
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            Models.SERVMEDEntities4 db2 = new Models.SERVMEDEntities4();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            string usuario = User.Identity.GetUserName();

            var res = (from q in db2.InvFarm
                       join sust in db2.Sustancia on q.Id_Sustancia equals sust.Id into sustX
                       from sustIn in sustX.DefaultIfEmpty()
                       join grupo in db2.grupo_21 on sustIn.id_grupo_21 equals grupo.id into grupoX
                       from grupoIn in grupoX.DefaultIfEmpty()
                       where sustIn.descripcion_21 != null && sustIn.descripcion_21 != ""
                       where q.InvFarmId == 81
                       select new
                       {
                           //inv_act = invfarmIn.Inv_Act,
                           nivel_21 = sustIn.Nivel_21,
                           clave = sustIn.Clave,
                           presentacion = sustIn.descripcion_21,
                           descripcion_grupo = grupoIn.descripcion,
                           inv_act = q.Inv_Act,
                           id_sustancia = sustIn.Id,
                       })
                       .ToList();

            //System.Diagnostics.Debug.WriteLine(res);

            var invLista = new List<LstInv2>();

            foreach (var item in res)
            {
                var boton = "";

                var inmed = (from a in db2.InhabilitarMedicamentos
                             where a.clave == item.clave
                             select a).FirstOrDefault();

                if (inmed == null)
                {
                    boton = "<button data-texto='Inhabilitar' data-clave='" + item.clave + "' class='btn btn-boton'>Inhabilitar</button>";
                }
                else
                {
                    boton = "<button style='color: white !important; background-color: green !important; border: 2px solid green; width: 100%;' data-texto='Habilitar' data-clave='" + item.clave + "' class='btn btn-boton'>Deshabilitado</button>";
                }

                var inventario = new LstInv2
                {
                    id_sustancia = item.id_sustancia,
                    clave = item.clave,
                    //descripcion_sal = item.descripcion_sal,
                    presentacion = item.presentacion,
                    descripcion_grupo = item.descripcion_grupo,
                    manejodisponible = item.inv_act,
                    boton = boton,
                };

                invLista.Add(inventario);
            }

            return new JsonResult { Data = invLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            //System.Diagnostics.Debug.WriteLine(id);


            /*
            var medicamento = (from q in db2.InvFarm
                               //where q.Clave == clave
                               select new
                               {
                                   Id = q.Id,
                                   Clave = q.cl,
                                   Descripcion_Sal = q.Descripcion_Sal,
                                   Descripcion_Grupo = q.Descripcion_Grupo,
                                   Presentacion = q.Presentacion,
                                   Inv_Act = q.Inv_Act,
                               })
                              .ToList();

            //System.Diagnostics.Debug.WriteLine(res);

            var invLista = new List<LstInv2>();



            foreach (var item in medicamento)
            {
                var boton = "";

                //Buscar si esta inhabilitado el medicamento
                var inmed = (from a in db2.InhabilitarMedicamentos
                             where a.clave == item.Clave
                             select a).FirstOrDefault();

                var inmed = (from a in db2.InhabilitarMedicamentos
                             where a.clave == item.Clave
                             select a).FirstOrDefault();

                if (inmed == null)
                {
                    boton = "<button data-texto='Inhabilitar' data-clave='" + item.Clave + "' class='btn btn-boton'>Inhabilitar</button>";
                }
                else
                {
                    boton = "<button style='color: white !important; background-color: green !important; border: 2px solid green;' data-texto='Habilitar' data-clave='" + item.Clave + "' class='btn btn-boton'>Habilitar</button>";
                }

                



                var inventario = new LstInv2
                {
                    clave = item.Clave,
                    descripcion_sal = item.Descripcion_Sal,
                    descripcion_grupo = item.Descripcion_Grupo,
                    presentacion = item.Presentacion,
                    boton = boton,
                };

                invLista.Add(inventario);
            }


            return new JsonResult { Data = invLista, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            */
        }


        public ActionResult InventarioListaUER()
        {
            if (User.IsInRole("InventarioUER"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Authorize]
        public ActionResult SalidaRecetas()
        {
            return View();
        }

        public ActionResult SalidaRecetasCU()
        {
            return View();
        }


        public JsonResult ListaSalidaRecetas()
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);


            var receta_hoy = (from a in db.Receta_Exp
                              where a.unidad_medica == 1
                              where a.Hora_Rcta != null
                              where a.UserFar == null
                              where a.Fecha >= fechaDT
                              where a.Estado != "2" || a.Estado != "3"
                              where a.Expediente == "62759100"
                              select a).ToList();

            var results = new List<RecetasList>();

            foreach (var item in receta_hoy)
            {

                var dhab = (from a in db2.DHABIENTES
                            where a.NUMEMP == item.Expediente
                            select a).FirstOrDefault();

                var result = new RecetasList
                {
                    Paciente = dhab.NOMBRES + " " + dhab.APATERNO + " " + dhab.AMATERNO,
                    Folio_Rcta = item.Folio_Rcta,
                    Fecha = string.Format("{0:dddd, dd MMMM yyyy HH:mm}", item.Hora_Rcta, new CultureInfo("es-ES")),
                    Boton = "<button data-clave='" + item.Folio_Rcta + "' class='btn btn-boton'>Detalles</button>",
                };

                results.Add(result);

            }

            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult ListaSalidaRecetasCU()
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);


            var receta_hoy = (from a in db.Receta_Exp
                              where a.unidad_medica == 3
                              where a.Hora_Rcta != null
                              where a.UserFar == null
                              where a.Fecha >= fechaDT
                              where a.Estado != "2" || a.Estado != "3"
                              select a).ToList();

            var results = new List<RecetasList>();

            foreach (var item in receta_hoy)
            {

                var dhab = (from a in db2.DHABIENTES
                            where a.NUMEMP == item.Expediente
                            select a).FirstOrDefault();

                var result = new RecetasList
                {
                    Paciente = dhab.NOMBRES + " " + dhab.APATERNO + " " + dhab.AMATERNO,
                    Folio_Rcta = item.Folio_Rcta,
                    Fecha = string.Format("{0:dddd, dd MMMM yyyy HH:mm}", item.Hora_Rcta, new CultureInfo("es-ES")),
                    Boton = "<button data-clave='" + item.Folio_Rcta + "' class='btn btn-boton'>Detalles</button>",
                };

                results.Add(result);

            }

            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult DetalleReceta(int folioRcta)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();

            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            //actualizar estado de receta para que no lo puedan tomar
            var recetaExp = (from a in db.Receta_Exp
                             where a.Folio_Rcta == folioRcta
                             select a).FirstOrDefault();

            var mensaje = "";
            //System.Diagnostics.Debug.WriteLine(recetaExp);

            if (recetaExp != null)
            {
                //revisar si no la a tomado nadie
                if (recetaExp.UserFar == null)
                {

                    // darle USERFAR
                    string usuario = User.Identity.GetUserName();

                    //buscar usuario de farmacia
                    var usufarm = (from a in db.Usuario
                                   where a.Usu_User == usuario
                                   select a).FirstOrDefault();


                    if (usufarm != null)
                    {
                        db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET UserFar = '" + usufarm.UsuarioId + "' WHERE Folio_Rcta = " + folioRcta + " ");
                        mensaje = "";
                        //mensaje = "Editado";
                    }
                    else
                    {
                        mensaje = "Usuario no esta registrado en los usuarios de farmacia";
                    }

                    //db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET UserFar = '" + usuario + "' WHERE Folio_Rcta = " + folioRcta + " ");
                    
                }
                else
                {
                    //la receta ya fue tomada por alguien

                    string usuario = User.Identity.GetUserName();

                    var usufarm = (from a in db.Usuario
                                   where a.Usu_User == usuario
                                   select a).FirstOrDefault();

                    //System.Diagnostics.Debug.WriteLine(usufarm);

                    if (usufarm != null)
                    {

                        if (usufarm.UsuarioId == recetaExp.UserFar)
                        {
                            //la tomo el mismo usuario
                            //mensaje = "mismo usuario";
                            mensaje = "";
                        }
                        else
                        {
                            var usufarmAct = (from a in db.Usuario
                                           where a.UsuarioId == recetaExp.UserFar
                                           select a).FirstOrDefault();

                            mensaje = "Receta esta siendo surtida por" + usufarmAct.Usu_Nombre;
                        }
                    }
                    else
                    {
                        mensaje = "Usuario no esta registrado en los usuarios de farmacia";
                    }


                }

            }

            var recetaDts = (from a in db.Receta_Detalle
                             join sustancia in db.Sustancia on a.Codigo equals sustancia.Clave into sustanciaX
                             from sustanciaIn in sustanciaX.DefaultIfEmpty()
                             where a.Folio_Rcta == folioRcta
                             select new
                             {

                                 medicamento = sustanciaIn.descripcion_21,
                                 dosis = a.Dosis,
                                 cantidad = a.Cantidad,
                                 clave = a.Codigo,

                             }).ToList();

            //return new JsonResult { Data = recetaDts, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            var resultdata = new { Result1 = recetaDts, Result2 = mensaje };

            return new JsonResult { Data = resultdata, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult LiberarReceta(int folioRcta)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            //actualizar estado de receta para que no lo puedan tomar
            var recetaExp = (from a in db.Receta_Exp
                             where a.Folio_Rcta == folioRcta
                             select a).FirstOrDefault();

            var mensaje = "";

            if(recetaExp != null)
            {
                //db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET Estado = '7' WHERE Folio_Rcta = " + folioRcta + " ");
                db.Database.ExecuteSqlCommand("UPDATE Receta_Exp SET UserFar = null WHERE Folio_Rcta = " + folioRcta + " ");
                mensaje = "Receta liberada";
            }

            return new JsonResult { Data = mensaje, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            //return new JsonResult { Data = recetaExp, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult SalidaRecetasLinares()
        {
            return View();
        }


        public JsonResult ListaSalidaRecetasLin()
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            Models.SMDEVEntities33 db2 = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);


            var receta_hoy = (from a in db.Receta_Exp
                              where a.unidad_medica == 3
                              where a.Hora_Rcta != null
                              where a.Fecha >= fechaDT
                              select a).ToList();

            var results = new List<RecetasList>();

            foreach (var item in receta_hoy)
            {

                var dhab = (from a in db2.DHABIENTES
                            where a.NUMEMP == item.Expediente
                            select a).FirstOrDefault();

                var result = new RecetasList
                {
                    Paciente = dhab.NOMBRES + " " + dhab.APATERNO + " " + dhab.AMATERNO,
                    Folio_Rcta = item.Folio_Rcta,
                    Fecha = string.Format("{0:dddd, dd MMMM yyyy HH:mm}", item.Hora_Rcta, new CultureInfo("es-ES")),
                    Boton = "<button data-clave='" + item.Folio_Rcta + "' class='btn btn-boton'>Detalles</button>",
                };

                results.Add(result);

            }

            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult BuscarCodigoBarras(string codigoBarras)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();

            var codigoBar = (from a in db.CodigoBarras
                             join sustan in db.Sustancia on a.Id_Sustancia equals sustan.Id into susX
                             from susIn in susX.DefaultIfEmpty()
                             where a.Codigo_Barra == codigoBarras
                             where a.Codigo_Barra != ""
                             select new
                             {
                                 Clave = susIn.Clave,
                                 Nombre_Comercial = a.Nombre_Comercial,

                             }).FirstOrDefault();

            var result = new Object();

            var status = 0;

            //System.Diagnostics.Debug.WriteLine(codigoBar);

            if (codigoBar != null)
            {
                //Codigo encontrado
                status = 1;
                result = new
                {
                    status = status,
                    clave = codigoBar.Clave,
                    nombre = codigoBar.Nombre_Comercial,

                };

            }
            else
            {
                //Codigo no encontrado
                status = 0;
                result = new
                {
                    status = status,
                    clave = "",
                    nombre = "",

                };
            }

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public ActionResult SurtirReceta(int folioRcta)
        {
            Models.SERVMEDEntities4 db = new Models.SERVMEDEntities4();
            //Models.SERVMEDEntitiesPruebas db2 = new Models.SERVMEDEntitiesPruebas();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddT00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);

            /*
            var receta = (from a in db.Receta_Exp
                              where a.Folio_Rcta == folioRcta
                          select a).FirstOrDefault();
            */

            var recetaDts = (from a in db.Receta_Detalle
                             join sustancia in db.Sustancia on a.Codigo equals sustancia.Clave into sustanciaX
                             from sustanciaIn in sustanciaX.DefaultIfEmpty()
                             where a.Folio_Rcta == folioRcta
                             select new
                             {

                                 medicamento = sustanciaIn.descripcion_21,
                                 id_sustancia = sustanciaIn.Id,
                                 dosis = a.Dosis,
                                 cantidad = a.Cantidad,
                                 clave = a.Codigo,

                             }).ToList();

            foreach (var item in recetaDts)
            {
                
                //buscar inventario activo
                //string query = "select id as id, nom_invent as nom_invent, status as status, tipo as tipo from Inventario WHERE status = 1 and tipo = 1";
                string query = "select id as id, nom_invent as nom_invent, status as status, tipo as tipo from Inventario WHERE id = 48";
                var result = db.Database.SqlQuery<Inventario>(query);
                var res = result.FirstOrDefault();


                string query2 = "select Id as id, Id_Sustancia as id_sustancia, Inv_Ent as inv_ent, Inv_Sal as inv_sal, Inv_Act as inv_act from InvFarm WHERE InvFarmId = '" + res.id + "' and Id_Sustancia = '" + item.id_sustancia + "'  ";
                var result2 = db.Database.SqlQuery<LstInv>(query2);
                var res2 = result2.FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(item.cantidad);

                if (res2 != null)
                {
                    var cantidad = item.cantidad;
                    var actual = res2.inv_act - item.cantidad;
                    var salida = item.cantidad + res2.inv_sal;
                    //System.Diagnostics.Debug.WriteLine(res2);
                    //tomar cantidad y restar del inv_act e inv_sal
                    db.Database.ExecuteSqlCommand("UPDATE InvFarm SET Inv_Sal = "+ salida + ", Inv_Act = " + actual + " WHERE Id = " + res2.id + " ");

                }

                //System.Diagnostics.Debug.WriteLine(res);
                //buscar id_sustancia en tabla de inventario

            }

            //System.Diagnostics.Debug.WriteLine(receta);


            TempData["message_success"] = "Receta surtida con éxito";
            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}