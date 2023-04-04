using CarbonTest1.Models;
using System.Threading.Tasks;

namespace CarbonTest1.Data.Interfaces
{
	public interface IUserRepository : IRepositoryBase<ApplicationUser>
	{
		Task<ApplicationUser> GetByRefreshToken(string refreshToken);
	}
}
