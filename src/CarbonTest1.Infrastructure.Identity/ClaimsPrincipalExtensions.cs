using System;
using System.Linq;
using System.Security.Claims;

namespace CarbonTest1.Infrastructure.Identity
{
	public static class ClaimsPrincipalExtensions
	{
		public static string UserId(this ClaimsPrincipal principal)
		{
			if (principal == null)
				throw new ArgumentNullException(nameof(principal));

			var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
			if (claim != null)
				return claim.Value;

			return string.Empty;
		}

        public static int CompanyId(this ClaimsPrincipal principal)
        {
            if (principal == null)
                throw new ArgumentNullException(nameof(principal));

            var claim = principal.FindFirst("CompanyId");
            if (claim != null)
                return int.Parse(claim.Value);

            return 0;
        }

		public static string Email(this ClaimsPrincipal principal)
		{
			if (principal == null)
				throw new ArgumentNullException(nameof(principal));

			var claim = principal.FindFirst(ClaimTypes.Email);
			if (claim != null)
				return claim.Value;

			return string.Empty;
		}

		public static bool HasRole(this ClaimsPrincipal principal, string roleName)
		{
			if (principal == null)
				throw new ArgumentNullException(nameof(principal));

			var roles = principal.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
			if (roles != null)
				return roles.Any(r => r.Value.ToLower() == roleName.ToLower());

			return false;
		}

		public static string GetClaimValue(this ClaimsPrincipal principal, string claimName)
		{
			if (principal == null)
				throw new ArgumentNullException(nameof(principal));

			var claim = principal.FindFirst(claimName);
			if (claim != null)
				return claim.Value;

			return string.Empty;
		}
	}
}
