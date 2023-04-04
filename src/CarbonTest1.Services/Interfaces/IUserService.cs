using CarbonTest1.Models;
using CarbonTest1.Models.Dtos;
using System.Threading.Tasks;

namespace CarbonTest1.Services.Interfaces
{
	public interface IUserService
	{
		Task<UserCreateResultDto> Create(ApplicationUser user, string password);
		Task<ApplicationUser> Get(string id);
		Task<ApplicationUser> Update(UserUpdateDto model);
		Task<PagedList<ApplicationUser>> GetAll(PagedListQueryDto model);

		Task<UserTokenResponseDto> GenerateAccessToken(AuthenticationRequestDto model);
		Task<UserTokenResponseDto> RefreshToken(string refreshToken);
	}
}
