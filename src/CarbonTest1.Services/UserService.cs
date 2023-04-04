using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CarbonTest1.Data.Interfaces;
using CarbonTest1.Models;
using CarbonTest1.Models.Dtos;
using CarbonTest1.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CarbonTest1.Services
{
	public class UserService : IUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IUserRepository _userRepository;
		private readonly IConfiguration _configuration;

		public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
		 IUserRepository userRepository, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_userRepository = userRepository;
			_configuration = configuration;
		}

		public async Task<UserCreateResultDto> Create(ApplicationUser user, string password)
		{
			var result = new UserCreateResultDto();
			var userCreateResult = await _userManager.CreateAsync(user, password);
			result.Success = userCreateResult.Succeeded;
			result.UserId = user.Id;
			result.Errors = userCreateResult.Errors.Select(x => x.Description).ToList();

			return result;
		}

		public async Task<UserTokenResponseDto> GenerateAccessToken(AuthenticationRequestDto model)
		{
			var loginResult = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
			if (loginResult.Succeeded)
			{
				var user = await _userManager.FindByNameAsync(model.Username);

				if (user.IsActive)
				{
					var tokenResponse = new UserTokenResponseDto();
					var refreshToken = GenerateRefreshToken();
					tokenResponse.RefreshToken = refreshToken;

					tokenResponse.AccessToken = await GenerateAccessToken(user);
					user.RefreshToken = refreshToken;
					user.RefreshTokenExpires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Tokens:ExpiryMinutes"]));
					await _userManager.UpdateAsync(user);

					tokenResponse.ExpiresIn = Convert.ToInt32(_configuration["Tokens:ExpiryMinutes"]) * 60;
					return tokenResponse;
				}
			
			}
			return null;
		}

		public async Task<ApplicationUser> Get(string id)
		{
			return await _userRepository.GetAsync(u => u.Id == id);
		}

		public async Task<ApplicationUser> Update(UserUpdateDto model)
		{
			var user = await Get(model.Id);
			user.GivenName = model.GivenName;
			user.FamilyName = model.FamilyName;
			user.UserName = model.Username;
			user.NormalizedUserName = model.Username.ToUpper();
			user.Email = model.Email;
			user.NormalizedEmail = model.Email.ToUpper();
			user.Picture = model.Picture;
			await _userRepository.UpdateAsync(user);
			return user;
		}

		public async Task<UserTokenResponseDto> RefreshToken(string refreshToken)
		{
			var user = await _userRepository.GetByRefreshToken(refreshToken);
			if (user == null) return null;

			if (DateTime.UtcNow <= user.RefreshTokenExpires)
			{
				var tokenResponse = new UserTokenResponseDto();
				var newRefreshToken = GenerateRefreshToken();
				tokenResponse.RefreshToken = newRefreshToken;
				tokenResponse.AccessToken = await GenerateAccessToken(user);
				user.RefreshToken = newRefreshToken;
				user.RefreshTokenExpires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Tokens:ExpiryMinutes"]));
				await _userManager.UpdateAsync(user);

				tokenResponse.ExpiresIn = Convert.ToInt32(_configuration["Tokens:ExpiryMinutes"]) * 60;
				return tokenResponse;
			}
			return null;
		}

		private async Task<string> GenerateAccessToken(ApplicationUser user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_configuration["Tokens:Key"]);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Tokens:ExpiryMinutes"])),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			//Add any user specific claims here
			tokenDescriptor.Claims = new Dictionary<string, object>();
			tokenDescriptor.Claims.Add("sub", user.Id.ToString());
			tokenDescriptor.Claims.Add("given_name", user.GivenName);
			tokenDescriptor.Claims.Add("family_name", user.FamilyName);
			tokenDescriptor.Claims.Add("email", user.Email);
			tokenDescriptor.Claims.Add("picture", user.Picture);
            tokenDescriptor.Claims.Add("companyId", user.CompanyId);


			var roles = await _userManager.GetRolesAsync(user);
			tokenDescriptor.Claims.Add("roles", roles);

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		private string GenerateRefreshToken()
		{
			using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
			{
				var randomBytes = new byte[64];
				rngCryptoServiceProvider.GetBytes(randomBytes);
				var token = Convert.ToBase64String(randomBytes);

				return token.Replace("/", "_");
			}
		}

		public async Task<PagedList<ApplicationUser>> GetAll(PagedListQueryDto model)
		{
			if (string.IsNullOrEmpty(model.Search))
				return await _userRepository.GetAllAsync(model.PageNumber, model.PageSize);

			return await _userRepository.GetAllAsync(model.PageNumber, model.PageSize,
				c => c.GivenName.ToLower().Contains(model.Search.ToLower()) ||
				c.FamilyName.ToLower().Contains(model.Search.ToLower()) ||
				c.Picture.ToLower().Contains(model.Search.ToLower()) ||
				c.Email.ToLower().Contains(model.Search.ToLower()) ||
				c.UserName.ToLower().Contains(model.Search.ToLower()));
		}

	
	}
}
