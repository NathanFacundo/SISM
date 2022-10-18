using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class EmpleadosController : Controller
    {
        SISMEntities1 RH = new SISMEntities1();
        // GET: Empleados
        public ActionResult RegistroEmpleado(int Emp_NumEmp)
        {
            var query = (from a in RH.TblEmpleado
                         where a.Emp_NumEmp == Emp_NumEmp
                         select a
                         ).FirstOrDefault();

            return View(query);
        }
        public JsonResult GetEmpleado()
        {
            var query = (from a in RH.TblEmpleado
                         join b in RH.TblDepartamento on a.IdDepartamento equals b.IdDepartamento
                         join c in RH.TblPuesto on a.IdPuesto equals c.IdPuesto
                         orderby a.Emp_Nombre ascending
                         select new
                         {
                             Folio = a.Emp_NumEmp,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Completo = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Puesto = c.NombrePuesto,
                             Departamento = b.NomDepartamento

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public  ActionResult GetListaEmpleados()
        {
            return View();
        }
        public JsonResult GetPais()
        {
            var query = (from a in RH.TblPais
                         select new
                         {
                             a.IdPais,
                             a.NombrePais

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetEstados()
        {
            var query = (from a in RH.TblEstado
                         where a.activo == 1
                         select new
                         {
                             a.IdEstado,
                             a.nombre

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetMunicipio()
        {
            var query = (from a in RH.TblMunicipio
                         where a.activo == 1
                         select new
                         {
                             a.IdMunicipio,
                             a.nombre

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetColonia()
        {
            var query = (from a in RH.TblColonia
                         select new
                         {
                             a.IdColonia,
                             a.NombreColonia

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetEstudiosEmpleado(int Emp_NumEmp)
        {
            var query = (from a in RH.TblEstudiosEmpleado
                         join b in RH.TblNivelAcademico on a.IdNivel equals b.IdNivel
                         join c in RH.TblEmpleado on a.Emp_NumEmp equals c.Emp_NumEmp
                         join d in RH.TblCarreras on a.IdCarrera equals d.IdCarrera
                         join e in RH.TblEscuela on a.IdEscuela equals e.IdEscuela
                         where a.Emp_NumEmp == Emp_NumEmp
                         select new
                         {
                             Folio = a.IdCarreraEmpleado,
                             Nombre = c.Emp_Nombre + " " + c.Emp_APaterno + " " + c.Emp_AMaterno,
                             NivelAcademico = b.NivelAcademico,
                             Carrera = d.Descripcion,
                             Cedula = a.Emp_Cedula,
                             Fecha = a.FechaExpedicion,
                             Escuela = e.NomEscuela

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}