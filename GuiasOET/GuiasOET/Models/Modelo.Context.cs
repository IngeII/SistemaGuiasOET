﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GuiasOET.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities1 : DbContext
    {
        public Entities1()
            : base("name=Entities1")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<GUIAS_COMPANIA> GUIAS_COMPANIA { get; set; }
        public virtual DbSet<GUIAS_CONFIGURACIONCORREO> GUIAS_CONFIGURACIONCORREO { get; set; }
        public virtual DbSet<GUIAS_EMPLEADO> GUIAS_EMPLEADO { get; set; }
        public virtual DbSet<GUIAS_ESTACION> GUIAS_ESTACION { get; set; }
        public virtual DbSet<GUIAS_RESERVACION> GUIAS_RESERVACION { get; set; }
        public virtual DbSet<GUIAS_ROLDIASLIBRES> GUIAS_ROLDIASLIBRES { get; set; }
        public virtual DbSet<GUIAS_TELEFONO> GUIAS_TELEFONO { get; set; }
        public virtual DbSet<GUIAS_ASIGNACION> GUIAS_ASIGNACION { get; set; }
        public virtual DbSet<GUIAS_TURNO> GUIAS_TURNO { get; set; }
    }
}
