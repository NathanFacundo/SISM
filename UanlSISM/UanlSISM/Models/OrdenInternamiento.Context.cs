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
    
    public partial class SMDEVEntities32 : DbContext
    {
        public SMDEVEntities32()
            : base("name=SMDEVEntities32")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DHABIENTES> DHABIENTES { get; set; }
        public virtual DbSet<DIAGNOSTICOS> DIAGNOSTICOS { get; set; }
        public virtual DbSet<MEDICOS> MEDICOS { get; set; }
        public virtual DbSet<Orden_Int> Orden_Int { get; set; }
        public virtual DbSet<prov_subrrogados> prov_subrrogados { get; set; }
    }
}
