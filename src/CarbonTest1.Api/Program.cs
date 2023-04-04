using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using CarbonTest1.Data;

namespace CarbonTest1.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var config = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: true)
				.AddCommandLine(args)
				.Build();

			var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
			GlobalDiagnosticsContext.Set("connectionString", config.GetConnectionString("DefaultConnection"));

			try
			{
				var host = CreateIWebHostBuilder(args);
				logger.Debug("Init Main");
				SeedDatabase(host);
				host.Run();
			}
			catch (Exception ex)
			{
				logger.Error(ex, "Stopped program because of exception");
				throw;
			}
			finally
			{
				LogManager.Shutdown();
			}

		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });


		public static IWebHost CreateIWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args).UseStartup<Startup>()
				.ConfigureLogging(logging =>
				{
					logging.ClearProviders();
					//logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
				})
				.UseNLog()
				.Build();

		private static void SeedDatabase(IWebHost host)
		{
			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					SeedData.Initialize(services);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred seeding the DB.");
				}
			}
		}

		
	}
}
