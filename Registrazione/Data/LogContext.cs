using System;
using System.Globalization;
using Logger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RegistrazioneSistemaUnico.Helpers;
using RegistrazioneSistemaUnico.Models;
namespace RegistrazioneSistemaUnico.Data
{
	public partial class LogContext : DbContext
	{
		public LogContext()
		{
		}

		public LogContext(DbContextOptions<LogContext> options)
			: base(options)
		{
		}


		public virtual DbSet<Log> Log { get; set; }
		public virtual DbSet<LogEvent> Event { get; set; }
		public virtual DbSet<LogLevel> Level { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Log>(entity =>
			{
				entity.HasKey(e => e.Id);
				
				entity
					.HasOne(e => e.LogLevel)
					.WithMany(e=>e.Logs)
					.HasForeignKey(e=>e.IdLevel);

				entity
					.HasOne(e => e.LogEvent)
					.WithMany(e=>e.Logs)
					.HasForeignKey(e=>e.IdEventType);

				entity
					.HasOne(e => e.Ente)
					.WithMany(e=>e.Logs)
					.HasForeignKey(e=>e.CodiceFiscaleEnte);
			});

			modelBuilder.Entity<LogEvent>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity
					.ToTable("LogEvent");
			});

			modelBuilder.Entity<LogLevel>(entity =>
			{
				entity.HasKey(e => e.Id);

				entity
					.ToTable("LogLevel");
			});

			modelBuilder.Entity<Ente>(entity =>
			{
				entity.HasKey(e => e.CodiceFiscale);

				entity
					.ToTable("VwDenominazione");
			});

			OnModelCreatingPartial(modelBuilder);
		}

		partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
	}
}
