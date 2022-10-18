using Microsoft.AspNet.Identity;
using Postal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        // GET: Tickets
        public ActionResult Index()
        {
            if (User.IsInRole("Tickets"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public class TicketsList
        {
            public string color { get; set; }
            public int id { get; set; }
            public string descripcion { get; set; }
            public string encargado { get; set; }
            public string estado { get; set; }
            public string fecha_registro { get; set; }
            public string boton { get; set; }
            public string usuario { get; set; }
            public int asignar { get; set; }
        }

        public JsonResult ListaTickets()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var usuario = User.Identity.GetUserName();
            var id_usuario = User.Identity.GetUserId();
            var fecha = DateTime.Now.ToString("2022-02-04T00:00:00.000");
            var fechaDT = DateTime.Parse(fecha);
            //Tickets
            var tickets = (from t in db.Tickets
                           where t.fecha_registro >= fechaDT
                           select new
                          {
                              id = t.id,
                              descripcion = t.descripcion,
                              encargado = t.usuario_encargado,
                              fecha_registro = t.fecha_registro,
                              estado = t.estado,
                              usuario_realiza = t.usuario_realiza,
                              usuario_encargado = t.usuario_encargado,
                              area = t.area,
                           })
                          .ToList();

            //System.Diagnostics.Debug.WriteLine(tickets);
            //System.Diagnostics.Debug.WriteLine(id_usuario);

            var results = new List<TicketsList>();

            foreach (var item in tickets)
            {
                var estado = "";
                var color = "";
                if (item.estado == 1)
                {
                    estado = "Nuevo";
                    color = "#bfbfbf";
                }
                else
                {
                    if (item.estado == 2)
                    {
                        estado = "Asignado";
                        color = "#ffd447";
                    }
                    else
                    {
                        if (item.estado == 3)
                        {
                            estado = "En seguimiento";
                            color = "#13326e";
                        }
                        else
                        {
                            if (item.estado == 4)
                            {
                                estado = "Cerrado";
                                color = "green";
                            }
                            else
                            {
                                estado = "Sin estado";
                            }
                        }
                    }
                }

                //Tomar si el ticket le pertenece al coordinador de esa area
                //System.Diagnostics.Debug.WriteLine(item.area);

                var area = (from a in db.TicketAreas
                            where a.id == item.area
                            select a).FirstOrDefault();

                //System.Diagnostics.Debug.WriteLine(area);
                //System.Diagnostics.Debug.WriteLine(id_usuario);

                var boton = "";

                if (area.id_coordinador == id_usuario && item.usuario_encargado == null)
                {                    
                    boton = "<button data-info='" + item.id + "' class='btn btn-asignar'>Asignar</button>";
                }
                else
                {
                    boton = "<button data-info='" + item.id + "' class='btn btn-asignar'>Ver más</button>";
                }

                //Buscar quien solicita el ticket
                var usuSol = "";

                var usuarioSol = (from a in db.MEDICOS
                            where a.Numero == item.usuario_realiza
                            select a).FirstOrDefault();

                if(usuarioSol != null)
                {
                    usuSol = usuarioSol.Nombre + " " + usuarioSol.Apaterno + " " + usuarioSol.Amaterno;
                }
                else
                {

                    var usuarioEnc = (from a in db.AspNetUsers
                                      where a.UserName == item.usuario_realiza
                                      select a).FirstOrDefault();

                    if(usuarioEnc.Name != null)
                    {
                        usuSol = usuarioEnc.Name;
                    }
                    else
                    {

                        string query = "SELECT NOMBRE AS NOMBRE FROM ACCESO WHERE USUARIO = '" + item.usuario_realiza + "'";
                        var result = db.Database.SqlQuery<Accesos>(query);
                        var res = result.FirstOrDefault();

                        if (res != null)
                        {
                            usuSol = res.NOMBRE;
                        }
                        else
                        {

                            usuSol = item.usuario_realiza;
                        }

                    }


                    
                }

                //System.Diagnostics.Debug.WriteLine(usuSol);

                var asignar = 0;


                var usuEnc = "";

                if(item.usuario_encargado != null)
                {
                    var usuarioEnc = (from a in db.AspNetUsers
                                      where a.Id == item.usuario_encargado
                                      select a).FirstOrDefault();

                    usuEnc = usuarioEnc.Name;

                }
                else
                {
                    usuEnc = "Sin asignar";
                }


                if (area.id_coordinador == id_usuario || item.usuario_realiza == usuario || item.usuario_encargado == id_usuario)
                {

                    //System.Diagnostics.Debug.WriteLine(id_usuario);

                    var result = new TicketsList
                    {
                        color = "<div style='width:10px; height:10px; border-radius:100%; margin-top:8px; background-color:" + color + "'></div>",
                        id = item.id,
                        descripcion = item.descripcion,
                        //encargado = item.encargado,
                        encargado = usuEnc,
                        estado = estado,
                        fecha_registro = string.Format("{0:dd/MM/yyyy}", item.fecha_registro),
                        boton = boton,
                        usuario = usuSol,
                    };

                    results.Add(result);
                }

            }

            return new JsonResult { Data = results, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public class TicketLista
        {
            public string opcion { get; set; }
        }

        public JsonResult DetallesTicket(int id)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var id_usuario = User.Identity.GetUserId();
            var usuario = User.Identity.GetUserName();
            //Tickets
            var ticket = (from t in db.Tickets
                          join areaTickets in db.TicketAreas on t.area equals areaTickets.id into areaX
                          from areaIn in areaX.DefaultIfEmpty()
                          where t.id == id
                          select new
                          {
                              id = t.id,
                              area = areaIn.area,
                              coordinador = areaIn.id_coordinador,
                              id_area = t.area,
                              descripcion = t.descripcion,
                              usuario_realiza = t.usuario_realiza,
                              usuario_encargado = t.usuario_encargado,
                              fecha_registro = t.fecha_registro,
                              fecha_cierre = t.fecha_cierre,
                              estado = t.estado,
                          })
                          .FirstOrDefault();

            //Buscar quien solicita el ticket
            var usuSol = "";

            var usuarioSol = (from a in db.MEDICOS
                              where a.Numero == ticket.usuario_realiza
                              select a).FirstOrDefault();

            if (usuarioSol != null)
            {
                usuSol = usuarioSol.Nombre + " " + usuarioSol.Apaterno + " " + usuarioSol.Amaterno;
            }
            else
            {

                var usuarioEnc = (from a in db.AspNetUsers
                                  where a.UserName == ticket.usuario_realiza
                                  select a).FirstOrDefault();

                if (usuarioEnc.Name != null)
                {
                    usuSol = usuarioEnc.Name;
                }
                else
                {

                    string query = "SELECT NOMBRE AS NOMBRE FROM ACCESO WHERE USUARIO = '" + ticket.usuario_realiza + "'";
                    var acceso = db.Database.SqlQuery<Accesos>(query);
                    var res = acceso.FirstOrDefault();

                    if (res != null)
                    {
                        usuSol = res.NOMBRE;
                    }
                    else
                    {
                        usuSol = ticket.usuario_realiza;
                    }

                }


            }

            var estado = "";
            if (ticket.estado == 1)
            {
                estado = "Nuevo";
            }
            else
            {
                if (ticket.estado == 2)
                {
                    estado = "En proceso";
                }
                else
                {
                    if (ticket.estado == 3)
                    {
                        estado = "Contestado";
                    }
                    else
                    {
                        if (ticket.estado == 4)
                        {
                            estado = "Finalizado";
                        }
                        else
                        {
                            estado = "Sin estado";
                        }
                    }
                }
            }

            //Tomar usuarios de Area
            var usuarios = (from a in db.AspNetUsers
                              where a.Area == ticket.id_area
                            select a).ToList();

            //Usuarios de areas
            StringBuilder sb = new StringBuilder();
            sb.Append("<option value='' selected disabled>Seleccionar encargado</option>");
            foreach (var item in usuarios)
            {
                if(item.Id == ticket.usuario_encargado)
                {
                    sb.Append("<option selected value='" + item.Id + "' > " + item.Name + " </option>");
                }
                else
                {
                    sb.Append("<option value='" + item.Id + "' > " + item.Name + " </option>");
                }
            }


            //Seguimiento
            var seguimientos = (from a in db.TicketSeguimiento
                            where a.id_ticket == id
                            select a).ToList();
                        
            StringBuilder seg = new StringBuilder();
            StringBuilder galeria = new StringBuilder();

            galeria.Append("<div id='gallery-" + id + "' class='hidden'>");

            foreach (var item in seguimientos)
            {
                
                var nombre = "";

                //Tomar el usuario
                var users = (from a in db.AspNetUsers
                             where a.Id == item.usuario
                             select a).FirstOrDefault();

                //Buscar nombre de usuario
                var usuarioName = (from a in db.MEDICOS
                                  where a.Numero == users.UserName
                                   select a).FirstOrDefault();

                if(usuarioName != null)
                {
                    nombre = usuarioName.Nombre + " " + usuarioName.Apaterno + " " + usuarioName.Amaterno;
                }
                else
                {
                    var usuarioName2 = (from a in db.AspNetUsers
                                       where a.Id == item.usuario
                                       select a).FirstOrDefault();

                    if(usuarioName2 != null)
                    {
                        nombre = usuarioName2.Name;
                    }
                }

                var fecha = string.Format("{0:dddd, dd MMMM yyyy HH:mm}", item.fecha, new CultureInfo("es-ES"));

                if (id_usuario == item.usuario)
                {
                    //System.Diagnostics.Debug.WriteLine(id_usuario);
                    if(item.imagen != null)
                    {
                        seg.Append("<div class='row m-0 mt-3'><div style='background-color:#fff4d9; border-radius:5px;' class='col-md-6 ml-auto p-3 text-left'><div class='row m-0'><div class='col p-0 text-left'><p class='m-0 mb-3'><b>" + nombre + "</b></p></div></div><a class='btn-gallery' href='#gallery-1'><img style='width:100%;' src='/SISM-Medico/Imagenes/tickets/" + item.imagen + "'></a><p class='m-0 mt-3'>" + item.seguimiento + "</p><p class='m-0 text-right mt-3'><em><b>" + fecha + "</b></em></p></div></div>");
                        galeria.Append("<a href='/Imagenes/tickets/" + item.imagen + "'>Image " + item.imagen + "</a>");
                    }
                    else
                    {
                        seg.Append("<div class='row m-0 mt-3'><div style='background-color:#fff4d9; border-radius:5px;' class='col-md-6 ml-auto p-3 text-left'><div class='row m-0'><div class='col p-0 text-left'><p class='m-0 mb-3'><b>" + nombre + "</b></p></div></div><p class='m-0 mt-3'>" + item.seguimiento + "</p><p class='m-0 text-right mt-3'><em><b>" + fecha + "</b></em></p></div></div>");
                    }
                }
                else
                {
                    if (item.imagen != null)
                    {
                        seg.Append("<div class='row m-0 mt-3'><div style='background-color:#e0f0ff; border-radius:5px;' class='col-md-6 mr-auto p-3 text-right'><div class='row m-0'><div class='col p-0 text-left'><p class='m-0 mb-3'><b>" + nombre + "</b></p></div></div><a class='btn-gallery' href='#gallery-" + item.id_ticket + "'><img style='width:100%;' src='/SISM-Medico/Imagenes/tickets/" + item.imagen + "'></a><p class='m-0 mt-3'>" + item.seguimiento + "</p><p class='m-0 text-right mt-3'><em><b>" + fecha + "</b></em></p></div></div>");
                        galeria.Append("<a href='/Imagenes/tickets/" + item.imagen + "'>Image " + item.imagen + "</a>");
                    }
                    else
                    {
                        seg.Append("<div class='row m-0 mt-3'><div style='background-color:#e0f0ff; border-radius:5px;' class='col-md-6 mr-auto p-3 text-right'><div class='row m-0'><div class='col p-0 text-left'><p class='m-0 mb-3'><b>" + nombre + "</b></p></div></div><p class='m-0 mt-3'>" + item.seguimiento + "</p><p class='m-0 text-right mt-3'><em><b>" + fecha + "</b></em></p></div></div>");
                    }
                }
            }

            galeria.Append("</div>");

            var botonUsuario = "";

            if(ticket.usuario_realiza == usuario)
            {
                botonUsuario = "Dar seguimiento";
            }
            else
            {
                if (ticket.coordinador == id_usuario)
                {
                    if(ticket.usuario_encargado == null)
                    {
                        botonUsuario = "Asignar";
                    }
                    else
                    {
                        botonUsuario = "Dar seguimiento";
                    }

                }
                else
                {
                    botonUsuario = "Dar seguimiento";
                }
            }

            var botonCerrar = "";

            if(ticket.usuario_realiza == usuario)
            {
                botonCerrar = "Cerrar";
            }
            else
            {
                botonCerrar = "";
            }


            var asignar = 0;

            if (ticket.coordinador == id_usuario)
            {
                asignar = 1;
            }
            else
            {
                asignar = 0;
            }
            //System.Diagnostics.Debug.WriteLine(seg.ToString());

            var result = new Object();

            result = new
            {
                id = ticket.id,
                id_usuario = id_usuario,
                usuario_realiza = usuSol,
                usuario = usuario,
                usuario2 = ticket.usuario_realiza,
                usuario3 = ticket.usuario_encargado,
                fecha = string.Format("{0:dd/MM/yyyy}", ticket.fecha_registro),
                fecha_cierre = ticket.fecha_cierre,
                descripcion = ticket.descripcion,
                estado = ticket.estado,
                opciones = sb.ToString(),
                segui = seg.ToString(),
                boton_usuario = botonUsuario,
                boton_cerrar = botonCerrar,
                asignar = asignar,
                galeria = galeria.ToString(),
            };

           //System.Diagnostics.Debug.WriteLine(result);


            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult AreasList()
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var usuario = User.Identity.GetUserName();
            //Tickets
            var areas = (from t in db.TicketAreas
                           where t.estado == 1
                           select new
                           {
                               id = t.id,
                               area = t.area,
                           })
                          .ToList();

            return new JsonResult { Data = areas, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult TiposList(int area)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var usuario = User.Identity.GetUserName();
            //Tickets
            var tipos = (from t in db.TicketTipo
                         where t.id_area == area
                         where t.estado == 1
                         select new
                         {
                             id = t.id,
                             tipo = t.descripcion,
                         })
                          .ToList();

            return new JsonResult { Data = tipos, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult GuardarTicket(int area, int tipo, string descripcion)
        {
            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var usuario = User.Identity.GetUserName();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var ip_realiza = Request.UserHostAddress;


            Tickets ticket = new Tickets();
            ticket.area = area;
            ticket.tipo = tipo;
            ticket.descripcion = descripcion;
            ticket.usuario_realiza = User.Identity.GetUserName();
            ticket.fecha_registro = fechaDT;
            ticket.ip_realiza = ip_realiza;
            ticket.estado = 1;
            db.Tickets.Add(ticket);
            db.SaveChanges();


            TempData["message_success"] = "Ticket generado con éxito";
            return Redirect(Request.UrlReferrer.ToString());
        }

        [HttpPost]
        public ActionResult AsignarTicket(int id, string usuario_encargado, string seguimiento, string fecha_estimada, HttpPostedFileBase file)
        {

            //System.Diagnostics.Debug.WriteLine(id);
            //System.Diagnostics.Debug.WriteLine(seguimiento);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var usuario = User.Identity.GetUserName();
            var id_usuario = User.Identity.GetUserId();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var fecha2 = DateTime.Now.ToString(fecha_estimada+ "T00:00:00.000");
            //var fechaDT2 = DateTime.Parse(fecha2);
            //System.Diagnostics.Debug.WriteLine(fechaDT2);
            var ip_realiza = Request.UserHostAddress;
            var secondsDT = fechaDT.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;

            //System.Diagnostics.Debug.WriteLine(usuario_encargado);

            var ticket = (from a in db.Tickets
                        where a.id == id
                        select a).FirstOrDefault();

            //System.Diagnostics.Debug.WriteLine(ticket.usuario_encargado);
            var nuevaAsignacion = "";

            //System.Diagnostics.Debug.WriteLine(usuario_encargado);
            //System.Diagnostics.Debug.WriteLine(ticket.usuario_encargado);

            if (ticket.usuario_encargado != usuario_encargado)
            {

                //Asignar a usuario
                if (fecha_estimada != null && fecha_estimada != "")
                {
                    db.Database.ExecuteSqlCommand("UPDATE Tickets SET usuario_encargado = '" + usuario_encargado + "', fecha_estimada = '" + fecha2 + "', estado = 2 WHERE id='" + ticket.id + "';");
                }
                else
                {
                    db.Database.ExecuteSqlCommand("UPDATE Tickets SET usuario_encargado = '" + usuario_encargado + "', estado = 2 WHERE id='" + ticket.id + "';");
                }

                var usuarioAsig = (from a in db.AspNetUsers
                                   where a.Id == usuario_encargado
                                   select a).FirstOrDefault();

                if(fecha_estimada != null && fecha_estimada != "")
                {
                    nuevaAsignacion = "<b>Ticket asignado a " + usuarioAsig.Name + "</b><br> <b> Fecha estimada:</b> " + fecha_estimada + "<br>";
                }
                else
                {
                    nuevaAsignacion = "<b>Ticket asignado a " + usuarioAsig.Name + "</b><br> ";
                }
                

                

                //System.Diagnostics.Debug.WriteLine(usuario_encargado);
                //Se asigno usuario
                //if (seguimiento != null && seguimiento != "")

                //System.Diagnostics.Debug.WriteLine(ticket.usuario_encargado);

                if (usuario_encargado != null && usuario_encargado != "")
                {
                    TicketSeguimiento sgmto = new TicketSeguimiento();
                    sgmto.id_ticket = id;
                    sgmto.seguimiento = nuevaAsignacion + seguimiento;
                    sgmto.usuario = id_usuario;
                    sgmto.fecha = fechaDT;
                    //sgmto.estado = 1;

                    if (file != null)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        String filenameFile = secondsDT + ext;
                        String pathFile = Server.MapPath("~/Imagenes/tickets/" + filenameFile);
                        file.SaveAs(pathFile);

                        sgmto.imagen = filenameFile;
                        //db.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET foto = '" + filenameFile + "' WHERE NUMEMP = '" + id + "'");
                    }

                    if(usuarioAsig.Email != null && usuarioAsig.Email != usuarioAsig.UserName)
                    {
                        //Se le envia un correo al email de el Coordinador
                        dynamic email = new Email("Tickets");
                        //email.To = "demian_hdzr@hotmail.com";
                        email.To = usuarioAsig.Email;
                        email.Subject = "SOI #" + ticket.id + " asignado";
                        email.Descripcion = nuevaAsignacion + seguimiento;
                        email.Send();
                    }
                    

                    db.TicketSeguimiento.Add(sgmto);
                    db.SaveChanges();
                    TempData["message_success"] = "Ticket asignado con éxito";
                }
                else
                {
                    //Se dio seguimiento
                    if (seguimiento != null && seguimiento != "")
                    {
                        TicketSeguimiento sgmto = new TicketSeguimiento();
                        sgmto.id_ticket = id;
                        sgmto.seguimiento = nuevaAsignacion + seguimiento;
                        sgmto.usuario = id_usuario;
                        sgmto.fecha = fechaDT;
                        //sgmto.estado = 1;

                        if (file != null)
                        {
                            var ext = Path.GetExtension(file.FileName);
                            String filenameFile = secondsDT + ext;
                            String pathFile = Server.MapPath("~/Imagenes/tickets/" + filenameFile);
                            file.SaveAs(pathFile);

                            sgmto.imagen = filenameFile;
                            //db.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET foto = '" + filenameFile + "' WHERE NUMEMP = '" + id + "'");
                        }


                        var usureaEmail = (from a in db.AspNetUsers
                                           where a.UserName == ticket.usuario_realiza
                                           select a).FirstOrDefault();

                        if (usureaEmail.Email != null && usureaEmail.Email != usureaEmail.UserName)
                        {
                            //Se le envia un correo al email de el Coordinador
                            dynamic email = new Email("Tickets");
                            //email.To = "demian_hdzr@hotmail.com";
                            email.To = usureaEmail.Email;
                            email.Subject = "Seguimiento de SOI #" + ticket.id;
                            email.Descripcion = nuevaAsignacion + seguimiento;
                            email.Send();
                        }


                        db.TicketSeguimiento.Add(sgmto);
                        db.SaveChanges();
                        TempData["message_success"] = "Seguimiento agregado con éxito";
                    }
                    else
                    {
                        TempData["message_warning"] = "Debes de generar un seguimiento";
                    }
                        
                }



            }
            else
            {
                
                db.Database.ExecuteSqlCommand("UPDATE Tickets SET estado = 3 WHERE id='" + ticket.id + "';");

                nuevaAsignacion = "";

                //Subir retroalimentacion
                if (seguimiento != null && seguimiento != "")
                {
                    TicketSeguimiento sgmto = new TicketSeguimiento();
                    sgmto.id_ticket = id;
                    sgmto.seguimiento = nuevaAsignacion + seguimiento;
                    sgmto.usuario = id_usuario;
                    sgmto.fecha = fechaDT;
                    //sgmto.estado = 2;

                    if (file != null)
                    {
                        var ext = Path.GetExtension(file.FileName);
                        String filenameFile = secondsDT + ext;
                        String pathFile = Server.MapPath("~/Imagenes/tickets/" + filenameFile);
                        file.SaveAs(pathFile);

                        sgmto.imagen = filenameFile;
                        //db.Database.ExecuteSqlCommand("UPDATE DHABIENTES SET foto = '" + filenameFile + "' WHERE NUMEMP = '" + id + "'");
                    }


                    var usureaEmail = (from a in db.AspNetUsers
                                       where a.UserName == ticket.usuario_realiza
                                       select a).FirstOrDefault();

                    if (usureaEmail.Email != null && usureaEmail.Email != usureaEmail.UserName)
                    {
                        //Se le envia un correo al email de el Coordinador
                        dynamic email = new Email("Tickets");
                        //email.To = "demian_hdzr@hotmail.com";
                        email.To = usureaEmail.Email;
                        email.Subject = "Seguimiento de SOI #" + ticket.id;
                        email.Descripcion = nuevaAsignacion + seguimiento;
                        email.Send();
                    }


                    db.TicketSeguimiento.Add(sgmto);
                    db.SaveChanges();
                    TempData["message_success"] = "Seguimiento agregado con éxito";
                }
                else
                {
                    TempData["message_warning"] = "Debes de generar un seguimiento";
                }

            }




            return Redirect(Request.UrlReferrer.ToString());
        }

        public JsonResult CerrarTicket(int id, string seguimiento)
        {
            //System.Diagnostics.Debug.WriteLine(id);

            Models.SMDEVEntities33 db = new Models.SMDEVEntities33();
            var fecha = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var fechaDT = DateTime.Parse(fecha);
            var id_usuario = User.Identity.GetUserId();

            //AGREGAR UN SEGUIMIENTO DE QUE YA SE CERRO
            TicketSeguimiento sgmto = new TicketSeguimiento();
            sgmto.id_ticket = id;
            if (seguimiento != null)
            {
                sgmto.seguimiento = "<b>Ticket cerrado</b><br>" + seguimiento;
            }
            else
            {
                sgmto.seguimiento = "<b>Ticket cerrado</b>";
            }
            sgmto.usuario = id_usuario;
            sgmto.fecha = fechaDT;
            //sgmto.estado = 2;
            db.TicketSeguimiento.Add(sgmto);
            db.SaveChanges();

            //Cerrar
            db.Database.ExecuteSqlCommand("UPDATE Tickets SET fecha_cierre = '" + fecha + "', estado = 4 WHERE id='" + id + "';");

            var result = "";

            return new JsonResult { Data = result, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            //TempData["message_success"] = "Ticket cerrado con éxito";
            //return Redirect(Request.UrlReferrer.ToString());
        }

    }
}