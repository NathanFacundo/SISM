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
    
    public partial class SERVMEDEntities9 : DbContext
    {
        public SERVMEDEntities9()
            : base("name=SERVMEDEntities9")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ReporteInventarios> ReporteInventarios { get; set; }
    
        public virtual ObjectResult<SP_MedicamentosTemporales_Result> SP_MedicamentosTemporales(Nullable<int> idUnidad)
        {
            var idUnidadParameter = idUnidad.HasValue ?
                new ObjectParameter("idUnidad", idUnidad) :
                new ObjectParameter("idUnidad", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_MedicamentosTemporales_Result>("SP_MedicamentosTemporales", idUnidadParameter);
        }
    
        public virtual ObjectResult<SP_ObtenerSustancias_Milton_Result> SP_ObtenerSustancias_Milton()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_ObtenerSustancias_Milton_Result>("SP_ObtenerSustancias_Milton");
        }
    
        public virtual ObjectResult<SP_Temporales_Result> SP_Temporales(Nullable<int> idUnidad)
        {
            var idUnidadParameter = idUnidad.HasValue ?
                new ObjectParameter("idUnidad", idUnidad) :
                new ObjectParameter("idUnidad", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SP_Temporales_Result>("SP_Temporales", idUnidadParameter);
        }
    }
}
