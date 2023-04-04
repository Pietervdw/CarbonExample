using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CarbonTest1.Models
{
	public class Contact : EntityBase
	{
		[MaxLength(10)]
		public string Firstname { get; set; }
		public string Lastname { get; set; }
	}
}
