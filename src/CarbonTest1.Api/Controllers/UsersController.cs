using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarbonTest1.Infrastructure.Extensions;
using CarbonTest1.Models;
using CarbonTest1.Models.Dtos;
using CarbonTest1.Services.Interfaces;
using System.Threading.Tasks;
using CarbonTest1.Infrastructure.Identity;

namespace CarbonTest1.Api.Controllers
{
	[ApiController]
	[Authorize(Roles = "Administrator")]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[AllowAnonymous]
		[HttpPost("token")]
		public async Task<ActionResult> GetToken(AuthenticationRequestDto model)
		{
			if (!string.IsNullOrEmpty(model.RefreshToken))
			{
				var refreshTokenResponse = await _userService.RefreshToken(model.RefreshToken);
				return Ok(refreshTokenResponse);
			}
			var response = await _userService.GenerateAccessToken(model);
			return Ok(response);
		}

		[HttpPost("users")]
		public async Task<ActionResult> GetAll(PagedListQueryDto model)
		{
			var data = await _userService.GetAll(model);
			return Ok(data);
		}

		[HttpPost("users/add")]
		public async Task<ActionResult> Add(UserCreateDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(new EntityResponse<ApplicationUser>(null, ModelState.Errors()));

			var entityResponse = await _userService.Create(new ApplicationUser
			{
				GivenName = model.GivenName,
				FamilyName = model.FamilyName,
				UserName = model.Username,
				Email = model.Email,
				Picture = model.Picture
			}, model.Password);
			return Ok(entityResponse);
		}

		[HttpPatch("users")]
		public async Task<ActionResult> Update(UserUpdateDto model)
		{
			if (!ModelState.IsValid)
				return BadRequest(new EntityResponse<ApplicationUser>(null, ModelState.Errors()));

			var entityResponse = await _userService.Update(model);
			return Ok(entityResponse);
		}

		[HttpGet("users/me")]
		public async Task<ActionResult> Me()
		{
			var user = await _userService.Get(User.UserId());
			return Ok(new
			{
				user = new
				{
					givenName = user.GivenName,
					familyName = user.FamilyName,
					picture = user.Picture,
					email = user.Email,
					refreshToken = user.RefreshToken
				}
			});
		}

	}
}
