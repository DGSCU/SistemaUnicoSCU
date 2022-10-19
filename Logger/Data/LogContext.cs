using Microsoft.EntityFrameworkCore;

namespace Logger.Data
{
	class LogContext : DbContext
	{
		public LogContext()
		{
		}

		public LogContext(DbContextOptions<LogContext> options)
			: base(options)
		{
		}
		public virtual DbSet<LogTable> Log { get; set; }
		
		/*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(_connectionString);
			}
		}*/

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<LogTable>(entity =>
			{
				entity.ToTable("Log");
				entity.Property(e => e.EntityName)
					.HasMaxLength(128)
					.IsUnicode(false);
				entity.Property(e => e.Action)
					.HasMaxLength(128)
					.IsUnicode(false);
				entity.Property(e => e.TimeStamp)
					.HasColumnType("datetime")
					.HasDefaultValueSql("GETDATE()");
			});


		}

	}
}
