namespace CarbonTest1.Models.Dtos
{
	public class UserUpdateDto
	{
		public string Id { get; set; }
		public string GivenName { get; set; }
		public string FamilyName { get; set; }
		public string Email { get; set; }
		public string Username { get; set; }
		public string Picture { get; set; }
		public bool IsActive { get; set; }
	}
}
