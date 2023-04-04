using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonTest1.Models.Dtos
{
	public class PagedListQueryDto
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public string Search { get; set; }
	}
}
