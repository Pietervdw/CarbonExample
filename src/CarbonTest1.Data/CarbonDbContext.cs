using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarbonTest1.Models;

namespace CarbonTest1.Data
{
	public class CarbonDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
	{
	

		public CarbonDbContext(DbContextOptions<CarbonDbContext> options) : base(options) { }
		
		protected override void OnModelCreating(ModelBuilder builder)
		{
			CreateIdentityTables(builder);
		}

		private void CreateIdentityTables(ModelBuilder builder)
		{
			// See : https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1#identity-and-ef-core-migrations

			builder.Entity<ApplicationUser>(b =>
			{
				b.HasKey(u => u.Id);
				b.ToTable("Users");
			});

			builder.Entity<IdentityUserLogin<string>>(b =>
			{
				// Composite primary key consisting of the LoginProvider and the key to use
				// with that provider
				b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

				// Limit the size of the composite key columns due to common DB restrictions
				b.Property(l => l.LoginProvider).HasMaxLength(128);
				b.Property(l => l.ProviderKey).HasMaxLength(128);

				// Maps to the AspNetUserLogins table
				b.ToTable("UserLogins");
			});

			builder.Entity<IdentityUserToken<string>>(b =>
			{
				// Composite primary key consisting of the UserId, LoginProvider and Name
				b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

				// Limit the size of the composite key columns due to common DB restrictions
				b.Property(t => t.LoginProvider).HasMaxLength(128);
				b.Property(t => t.Name).HasMaxLength(128);

				// Maps to the AspNetUserTokens table
				b.ToTable("UserTokens");
			});

			builder.Entity<IdentityUserRole<string>>(b =>
			{
				// Primary key
				b.HasKey(r => new { r.UserId, r.RoleId });

				// Maps to the AspNetUserRoles table
				b.ToTable("UserRoles");
			});
		}
	}


}

