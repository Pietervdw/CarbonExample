using CarbonTest1.Models;
using CarbonTest1.Models.Dtos;
using System.Threading.Tasks;

namespace CarbonTest1.Services.Interfaces
{
	public interface IContactService
	{
		Task<EntityResponse<Contact>> Add(Contact contactModel);
		Task<Contact> Get(int id);
		Task<EntityResponse<Contact>> Update(Contact contactModel);
		Task Delete(int id);
		Task<PagedList<Contact>> GetAll(PagedListQueryDto model);
	}
}
