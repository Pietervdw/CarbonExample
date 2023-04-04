using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CarbonTest1.Models
{
	[Table("Logs")]
	public class Log : EntityBase
	{
		[MaxLength(50)]
		public string Application { get; set; }
		public DateTime DateLogged { get; set; }
		[MaxLength(50)]
		public string Level { get; set; }
		public string Message { get; set; }
		public string Logger { get; set; }
		public string CallSite { get; set; }
		public string Exception { get; set; }
	}
}
