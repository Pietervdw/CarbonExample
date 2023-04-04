using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarbonTest1.Infrastructure.Extensions
{
	public static class ModelStateExtensions
	{
		public static List<string> Errors(this ModelStateDictionary modelState)
		{
			var errors = modelState
				.Where(a => a.Value.Errors.Count > 0)
				.SelectMany(x => x.Value.Errors)
				.Select(x => x.ErrorMessage)
				.ToList();
			return errors;
		}
	}
}
