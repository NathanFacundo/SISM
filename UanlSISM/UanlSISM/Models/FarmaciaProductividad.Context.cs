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
    
    public partial class SERVMEDEntities4 : DbContext
    {
        public SERVMEDEntities4()
            : base("name=SERVMEDEntities4")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Usuario> Usuario { get; set; }
        public virtual DbSet<Receta_Detalle> Receta_Detalle { get; set; }
        public virtual DbSet<Receta_Exp> Receta_Exp { get; set; }
        public virtual DbSet<InventarioConteo> InventarioConteo { get; set; }
        public virtual DbSet<InvFarm> InvFarm { get; set; }
        public virtual DbSet<CodigoBarras> CodigoBarras { get; set; }
        public virtual DbSet<InhabilitarMedicamentos> InhabilitarMedicamentos { get; set; }
        public virtual DbSet<Sustancia> Sustancia { get; set; }
        public virtual DbSet<unidadeslicitacion_21> unidadeslicitacion_21 { get; set; }
        public virtual DbSet<grupo_21> grupo_21 { get; set; }
        public virtual DbSet<Grupo> Grupo { get; set; }
        public virtual DbSet<Sal> Sal { get; set; }
        public virtual DbSet<MedicamentosControlados> MedicamentosControlados { get; set; }
        public virtual DbSet<DetalleOC> DetalleOC { get; set; }
        public virtual DbSet<OrdenCompra> OrdenCompra { get; set; }
    }
}
