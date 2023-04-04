using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CarbonTest1.Services;
using CarbonTest1.Services.Interfaces;
using CarbonTest1.Infrastructure.Startup;
using CarbonTest1.Data.Interfaces;
using CarbonTest1.Data.Repositories;

namespace CarbonTest1.Api
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddProjectCors();
			services.AddProjectDbContext(Configuration.GetConnectionString("EosConnection"));
			services.AddProjectIdentityAndAuthentication(Configuration["Tokens:Key"], Configuration["Tokens:Issuer"],Configuration.GetConnectionString("CarbonConnection"));
			services.ConfigureIdentityOptions();

			services.AddControllers();

			services.AddMvc()
				.ConfigureApiBehaviorOptions(options =>
				{
					options.SuppressModelStateInvalidFilter = true;
				}); //https://stackoverflow.com/questions/54942192

			ConfigureDependencyInjection(services);
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseCors("AllowSpecificOrigin");

			app.UseAuthentication();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private void ConfigureDependencyInjection(IServiceCollection services)
		{
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IUserRepository, UserRepository>();

			services.AddTransient<IContactService, ContactService>();
			services.AddTransient<IContactRepository, ContactRepository>();

		}

	}
}
