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
    
    public partial class SMDEVEntities28 : DbContext
    {
        public SMDEVEntities28()
            : base("name=SMDEVEntities28")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DIAGNOSTICOS> DIAGNOSTICOS { get; set; }
        public virtual DbSet<MEDICOS> MEDICOS { get; set; }
        public virtual DbSet<ESPECIALIDADES> ESPECIALIDADES { get; set; }
        public virtual DbSet<expediente> expediente { get; set; }
    }
}
