namespace VP.Totum.Url;

public class Program
{
	public static void Main(String[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddOpenTelemetry().UseAzureMonitor();
		builder.Services.AddTotumServices(builder.Configuration);

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}

internal static class TotumProgramExtension
{
	internal static IServiceCollection AddTotumServices(this IServiceCollection services, IConfiguration configuration)
	{
		services
			.AddDbContext<TotumDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("TotumDb")));

		return services;
	}
}
