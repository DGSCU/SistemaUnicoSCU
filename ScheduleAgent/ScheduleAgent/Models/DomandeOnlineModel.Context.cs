﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScheduleAgent.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class DomandeOnlineContext : DbContext
    {
        public DomandeOnlineContext()
            : base("name=DomandeOnlineContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DomandaPartecipazione> DomandaPartecipazione { get; set; }
        public virtual DbSet<Progetto> SUSCN_DOL_PROGETTI_DISPONIBILI { get; set; }
        public virtual DbSet<Bando> Bando { get; set; }
    
        public virtual int SP_AggiornamentoDati()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_AggiornamentoDati");
        }
    
        public virtual int SP_AnnullaDomanda(Nullable<int> idDomanda)
        {
            var idDomandaParameter = idDomanda.HasValue ?
                new ObjectParameter("IdDomanda", idDomanda) :
                new ObjectParameter("IdDomanda", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SP_AnnullaDomanda", idDomandaParameter);
        }
    }
}
