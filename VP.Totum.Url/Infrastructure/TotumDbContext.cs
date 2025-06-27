namespace VP.Totum.Url.Infrastructure;

public class TotumDbContext : DbContext
{
	public TotumDbContext(DbContextOptions<TotumDbContext> options) : base(options)
	{
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		// Configure your entities here
		modelBuilder.Entity<ShortenedUrl>(entity =>
		{
			entity.ToTable(nameof(ShortenedUrl));
			entity.HasKey(e => e.Id);
			entity.Property(e => e.ShortCode).IsRequired().HasMaxLength(6);
			entity.Property(e => e.OriginalUrl).IsRequired().HasMaxLength(2048);
			entity.Property(e => e.CreatedAt).IsRequired();
			entity.Property(e => e.ExpirationDate).IsRequired(false);
			entity.Property(e => e.IsActive).HasDefaultValue(true);
		});

		//add index to Short code
		modelBuilder.Entity<ShortenedUrl>()
			.HasIndex(u => u.ShortCode)
			.IsUnique();
	}

	public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
}
