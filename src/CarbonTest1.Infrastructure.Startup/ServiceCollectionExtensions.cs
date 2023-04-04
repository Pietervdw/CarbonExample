using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using CarbonTest1.Data;
using CarbonTest1.Models;
using System;
using System.Text;

namespace CarbonTest1.Infrastructure.Startup
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddProjectDbContext(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<EosDbContext>(options =>
			options.UseSqlServer(connectionString,
				sqlOptions =>
				{
					sqlOptions.CommandTimeout(10);
					sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
						maxRetryDelay: TimeSpan.FromSeconds(30),
						errorNumbersToAdd: null);
				}));

			return services;
		}

		public static IServiceCollection AddProjectIdentityAndAuthentication(this IServiceCollection services, string issuerSigningKey, string validIssuer, string connectionString)
        {
            services.AddEntityFrameworkSqlServer().AddDbContext<CarbonDbContext>(options =>
                options.UseSqlServer(connectionString,
                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(10);
                        sqlOptions.EnableRetryOnFailure(maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    }));

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<CarbonDbContext>()
				.AddRoles<IdentityRole>()
				.AddDefaultTokenProviders();

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			})
				.AddJwtBearer(cfg =>
				{
					cfg.TokenValidationParameters = new TokenValidationParameters()
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey)),
						ValidIssuer = validIssuer,
						ValidateIssuer = false,
						ValidateAudience = false,
						ClockSkew = TimeSpan.Zero
					};
				});

			return services;
		}

		public static IServiceCollection AddProjectCors(this IServiceCollection services)
		{
			services.AddCors(options =>
			{
				options.AddPolicy(name: "AllowSpecificOrigin",
					builder =>
					{
						builder.AllowAnyOrigin();
						builder.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod().Build();
					});
			});
			return services;
		}

		public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
		{
			services.Configure<IdentityOptions>(options =>
			{
				// Default Password settings.
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;

				options.User.RequireUniqueEmail = true;
			});

			return services;
		}
	}
}
