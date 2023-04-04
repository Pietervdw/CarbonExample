using Microsoft.AspNetCore.Identity;
using System;

namespace CarbonTest1.Models
{
	public class ApplicationUser : IdentityUser
	{
		public string GivenName { get; set; }
		public string FamilyName { get; set; }
		public string Picture { get; set; }
		public bool IsActive { get; set; }
        public int CompanyId { get; set; }

        public string RefreshToken { get; set; }
		public DateTime RefreshTokenExpires { get; set; }
	}
}
