using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonTest1.Models.Dtos
{
	public class UserCreateDto
	{
		public string Username { get; set; }
		public string Email { get; set; }
		public string GivenName { get; set; }
		public string FamilyName { get; set; }
		public string Picture { get; set; }
		public string Password { get; set; }
	}
}
