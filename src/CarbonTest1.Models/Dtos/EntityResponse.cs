using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonTest1.Models.Dtos
{
	public class EntityResponse<T>
	{
		public EntityResponse() {}

		public EntityResponse(T entity, string errorMessage)
		{
			Entity = entity;
			HasError = true;

			Errors = new List<string>();
			Errors.Add(errorMessage);
		}

		public EntityResponse(T entity, List<string> errorMessages)
		{
			Entity = entity;
			HasError = true;

			Errors = errorMessages;
		}

		public EntityResponse(T entity) { Entity = entity; }

		public void AddError(string errorMessage)
		{
			if (Errors == null)
				Errors = new List<string>();

			Errors.Add(errorMessage);
			HasError = true;
		}

		public T Entity { get; set; }
		public bool HasError { get; set; }
		public List<string> Errors { get; set; }
	}
}
