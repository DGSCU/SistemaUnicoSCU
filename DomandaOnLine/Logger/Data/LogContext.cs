using System.Data.Entity;

namespace Logger.Data
{
	class LogContext : DbContext
	{
		public LogContext()
			: base("Log")
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

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			var entity = modelBuilder.Entity<LogTable>();

			entity.ToTable("Log");
			entity.Property(e => e.EntityName)
				.HasMaxLength(128)
				.IsUnicode(false);
			entity.Property(e => e.Action)
				.HasMaxLength(128)
				.IsUnicode(false);
			entity.Property(e => e.TimeStamp)
				.HasColumnType("datetime");

		}

	}
}
