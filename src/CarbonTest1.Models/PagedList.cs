using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonTest1.Models
{
	public class PagedList<T>
	{
		public int TotalRows { get; set; }
		public int CurrentPage { get; set; }
		public int PerPage { get; set; }
		public int FilteredTotalRows { get; set; }
		public List<T> Items { get; set; }

		public PagedList(List<T> items, int totalRows, int currentPage, int perPage, int filteredTotalRows)
		{
			TotalRows = totalRows;
			CurrentPage = currentPage;
			PerPage = perPage;
			FilteredTotalRows = filteredTotalRows;

			Items = new List<T>();
			Items.AddRange(items);
		}

	}
}
