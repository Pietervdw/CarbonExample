using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonTest1.Models.Dtos
{
	public class UserCreateResultDto
	{
		public bool Success { get; set; }
		public string UserId { get; set; }
		public List<string> Errors { get; set; }
	}
}
