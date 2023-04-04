using System.ComponentModel.DataAnnotations;

namespace CarbonTest1.Models
{
	public class EntityBase
	{
		[Key]
		public int Id { get; set; }
	}
}
