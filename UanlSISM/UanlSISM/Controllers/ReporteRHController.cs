using System.Linq;
using System.Web.Mvc;
using UanlSISM.Models;

namespace UanlSISM.Controllers
{
    public class ReporteRHController : Controller
    {
        SISMEntities1 db = new SISMEntities1();
        // GET: ReporteRH
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ReporteRH(int SubCategoria)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdCategoriaSM == SubCategoria && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ReporteRH1(int Categoria)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdSubCategoria == Categoria && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ReporteRH2(int Dpto)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdDepartamento == Dpto && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ReporteRH3(int Puesto)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdPuestoSM == Puesto && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto


                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ReporteRH4(int UnidadMedica)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.UnidadMedica == UnidadMedica && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ReporteRH5(int Regimen)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdRegimen == Regimen && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult CategoriaSM()
        {
            var query = (from a in db.TblSubCategoriaSM
                         select new
                         {
                             a.IdSubCategoria,
                             a.SubCategoria

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult SubCategoriaSM(int IdSubCategoria)
        {
            var query = (from a in db.Tbl_CategoriaSM
                         where a.IdSubCategoria == IdSubCategoria
                         select new
                         {
                             a.IdCategoriaSM,
                             a.CategoriaSM

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Departamento()
        {
            var query = (from a in db.TblDepartamento
                         where a.Estatus == true
                         select new
                         {
                             a.IdDepartamento,
                             a.NomDepartamento

                         }).ToList();


            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult PuestoSM(int IdDpto)
        {
            var query = (from a in db.Tbl_PuestosSM
                         where a.IdDepartamento == IdDpto
                         select new
                         {
                             a.IdPuestoSM,
                             a.PuestoSM

                         }).ToList();


            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult UnidadMedica()
        {
            var query = (from a in db.Tbl_UnidadMedica
                         select new { 
                         
                             a.IdUnidadMed,
                             a.UnidadMedica
                         
                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult Regimen()
        {
            var query = (from a in db.TblRegimen
                         where a.Status_Regimen == true
                         select new
                         {
                             a.IdRegimen,
                             a.Regimen

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult RegimenCatSM(int CatSM, int Regimen)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdRegimen == Regimen && a.IdSubCategoria == CatSM && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult RegimenCatSMSubCatSM(int CatSM, int Regimen,int SubCatSM)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdRegimen == Regimen && a.IdSubCategoria == SubCatSM  && a.IdCategoriaSM == CatSM && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult RegimenCatSMSubCatSMDep(int CatSM, int Regimen, int SubCatSM, int Dpto)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdRegimen == Regimen && a.IdSubCategoria == SubCatSM && a.IdCategoriaSM == CatSM && a.IdDepartamento == Dpto && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult RegimenCatSMSubCatSMDepPSM(int CatSM, int Regimen, int SubCatSM, int Dpto, int PuestoSM)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdRegimen == Regimen && a.IdSubCategoria == SubCatSM && a.IdCategoriaSM == CatSM && a.IdDepartamento == Dpto && a.IdPuestoSM == PuestoSM && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult RegimenCatSMSubCatSMDepPSMUniMed(int CatSM, int Regimen, int SubCatSM, int Dpto, int PuestoSM, int UniMed)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdRegimen == Regimen && a.IdSubCategoria == SubCatSM && a.IdCategoriaSM == CatSM && a.IdDepartamento == Dpto && a.IdPuestoSM == PuestoSM && a.UnidadMedica == UniMed && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ReporteSinSubCat(int Regimen, int Categoria, int Dpto)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdRegimen == Regimen && a.IdSubCategoria == Categoria && a.IdDepartamento == Dpto && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult TablaReporte1(int Regimen)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdRegimen == Regimen && a.Emp_Estatus == true && a.Emp_Baja != true
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult TablaReporte3(int Regimen, int UniMed)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdRegimen == Regimen && a.Emp_Estatus == true && a.Emp_Baja != true && a.UnidadMedica == UniMed
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult TablaReporte4(int Categoria, int Dpto)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.IdSubCategoria == Categoria && a.Emp_Estatus == true && a.Emp_Baja != true && a.IdDepartamento == Dpto
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult TablaReporte5(int Dpto, int UniMed)
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.UnidadMedica == UniMed && a.Emp_Estatus == true && a.Emp_Baja != true && a.IdDepartamento == Dpto
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult GetEmpleadosActivos()
        {
            var query = (from a in db.TblEmpleado
                         join b in db.TblSubCategoriaSM on a.IdSubCategoria equals b.IdSubCategoria
                         join c in db.Tbl_CategoriaSM on a.IdCategoriaSM equals c.IdCategoriaSM
                         join d in db.TblDepartamento on a.IdDepartamento equals d.IdDepartamento
                         join e in db.Tbl_PuestosSM on a.IdPuestoSM equals e.IdPuestoSM
                         join f in db.Tbl_UnidadMedica on a.UnidadMedica equals f.IdUnidadMed
                         join g in db.TblRegimen on a.IdRegimen equals g.IdRegimen
                         where a.Emp_Estatus == true && a.Emp_Baja != true
                         orderby d.NomDepartamento ascending
                         select new
                         {
                             Regimen = g.Regimen,
                             Numero_Empleado = a.Emp_NumEmpAccess,
                             Nombre_Coompleto = a.Emp_Nombre + " " + a.Emp_APaterno + " " + a.Emp_AMaterno,
                             Categoria = b.SubCategoria,
                             SubCat = c.CategoriaSM,
                             Dpto = d.NomDepartamento,
                             Puesto = e.PuestoSM,
                             UniMed = f.UnidadMedica,
                             Ingreso_Neto = a.Emp_Ingresos,
                             Ingreso_Bruto = a.Emp_IngresoBruto

                         }).ToList();

            return new JsonResult { Data = query, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}