﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UanlSISM.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SERVMEDEntities6 : DbContext
    {
        public SERVMEDEntities6()
            : base("name=SERVMEDEntities6")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ReporteInventarios> ReporteInventarios { get; set; }
        public virtual DbSet<Tbl_DetalleOC> Tbl_DetalleOC { get; set; }
        public virtual DbSet<Tbl_OrdenCompra> Tbl_OrdenCompra { get; set; }
        public virtual DbSet<Tbl_DetalleRequi> Tbl_DetalleRequi { get; set; }
        public virtual DbSet<Tbl_Requisicion> Tbl_Requisicion { get; set; }
        public virtual DbSet<Inventario_1> Inventario { get; set; }
        public virtual DbSet<InvAlmFarm> InvAlmFarm { get; set; }
        public virtual DbSet<InvFarm> InvFarm { get; set; }
        public virtual DbSet<Sustancia> Sustancia { get; set; }
    
        public virtual ObjectResult<SP_MedicamentosTemporales_Result> SP_MedicamentosTemporales(Nullable<int> idUnidad)
        {
            var idUnidadParameter = idUnidad.HasValue ?
                new ObjectParameter("idUnidad", idUnidad) :
                new ObjectParameter("idUnidad", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_MedicamentosTemporales_Result>("SP_MedicamentosTemporales", idUnidadParameter);
        }
    
        public virtual ObjectResult<SP_Temporales_Result> SP_Temporales(Nullable<int> idUnidad)
        {
            var idUnidadParameter = idUnidad.HasValue ?
                new ObjectParameter("idUnidad", idUnidad) :
                new ObjectParameter("idUnidad", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Temporales_Result>("SP_Temporales", idUnidadParameter);
        }
    
        public virtual ObjectResult<SP_Trazabilidad_Result1> SP_Trazabilidad(string fechaIn, string fechaFi, string claveMed)
        {
            var fechaInParameter = fechaIn != null ?
                new ObjectParameter("fechaIn", fechaIn) :
                new ObjectParameter("fechaIn", typeof(string));
    
            var fechaFiParameter = fechaFi != null ?
                new ObjectParameter("fechaFi", fechaFi) :
                new ObjectParameter("fechaFi", typeof(string));
    
            var claveMedParameter = claveMed != null ?
                new ObjectParameter("ClaveMed", claveMed) :
                new ObjectParameter("ClaveMed", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Trazabilidad_Result1>("SP_Trazabilidad", fechaInParameter, fechaFiParameter, claveMedParameter);
        }
    
        public virtual ObjectResult<SP_Trazabilidad_Completa_Result> SP_Trazabilidad_Completa(string fechaIn, string fechaFi, string claveMed)
        {
            var fechaInParameter = fechaIn != null ?
                new ObjectParameter("fechaIn", fechaIn) :
                new ObjectParameter("fechaIn", typeof(string));
    
            var fechaFiParameter = fechaFi != null ?
                new ObjectParameter("fechaFi", fechaFi) :
                new ObjectParameter("fechaFi", typeof(string));
    
            var claveMedParameter = claveMed != null ?
                new ObjectParameter("ClaveMed", claveMed) :
                new ObjectParameter("ClaveMed", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Trazabilidad_Completa_Result>("SP_Trazabilidad_Completa", fechaInParameter, fechaFiParameter, claveMedParameter);
        }
    
        public virtual ObjectResult<SP_Trazabilidad_VE_Result1> SP_Trazabilidad_VE(Nullable<int> idOC, Nullable<int> idSustancia)
        {
            var idOCParameter = idOC.HasValue ?
                new ObjectParameter("IdOC", idOC) :
                new ObjectParameter("IdOC", typeof(int));
    
            var idSustanciaParameter = idSustancia.HasValue ?
                new ObjectParameter("IdSustancia", idSustancia) :
                new ObjectParameter("IdSustancia", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Trazabilidad_VE_Result1>("SP_Trazabilidad_VE", idOCParameter, idSustanciaParameter);
        }
    
        public virtual ObjectResult<SP_Trazabilidad_OC_Result11> SP_Trazabilidad_OC(string fechaIn, string fechaFi, string claveMed)
        {
            var fechaInParameter = fechaIn != null ?
                new ObjectParameter("fechaIn", fechaIn) :
                new ObjectParameter("fechaIn", typeof(string));
    
            var fechaFiParameter = fechaFi != null ?
                new ObjectParameter("fechaFi", fechaFi) :
                new ObjectParameter("fechaFi", typeof(string));
    
            var claveMedParameter = claveMed != null ?
                new ObjectParameter("ClaveMed", claveMed) :
                new ObjectParameter("ClaveMed", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Trazabilidad_OC_Result11>("SP_Trazabilidad_OC", fechaInParameter, fechaFiParameter, claveMedParameter);
        }
    }
}
