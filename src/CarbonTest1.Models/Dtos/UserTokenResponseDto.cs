using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarbonTest1.Models.Dtos
{
	public class UserTokenResponseDto
	{
		public UserTokenResponseDto()
		{
			TokenType = "Bearer";
		}
		[JsonProperty(PropertyName = "tokenType")]
		public string TokenType { get; set; }
		[JsonProperty(PropertyName = "accessToken")]
		public string AccessToken { get; set; }
		[JsonProperty(PropertyName = "refreshToken")]
		public string RefreshToken { get; set; }
		[JsonProperty(PropertyName = "expiresIn")]
		public int ExpiresIn { get; set; }
	}
}
