using CarbonTest1.Data.Interfaces;
using CarbonTest1.Models;
using CarbonTest1.Models.Dtos;
using CarbonTest1.Services.Interfaces;
using System.Threading.Tasks;

namespace CarbonTest1.Services
{
	public class ContactService : IContactService
	{
		private readonly IContactRepository _contactRepository;

		public ContactService(IContactRepository contactRepository)
		{
			_contactRepository = contactRepository;
		}

		public async Task<EntityResponse<Contact>> Add(Contact contactModel)
		{
			return await _contactRepository.AddAsync(contactModel, true);
		}

		public async Task<Contact> Get(int id)
		{
			return await _contactRepository.GetAsync(c => c.Id == id);
		}

		public async Task<EntityResponse<Contact>> Update(Contact contact)
		{
			var existingContact = await Get(contact.Id);
			existingContact.Firstname = contact.Firstname;
			existingContact.Lastname = contact.Lastname;
			await _contactRepository.UpdateAsync(existingContact, true);
			return new EntityResponse<Contact>(existingContact);
		}

		public async Task Delete(int id)
		{
			var contact = await Get(id);
			_contactRepository.Remove(contact);
		}

		public async Task<PagedList<Contact>> GetAll(PagedListQueryDto model)
		{
			if (string.IsNullOrEmpty(model.Search))
				return await _contactRepository.GetAllAsync(model.PageNumber, model.PageSize);

			//return await _contactRepository.GetAllAsync(model.PageNumber, model.PageSize,c => c.Firstname.ToLower().Contains(model.Search.ToLower()) || c.Lastname.ToLower().Contains(model.Search.ToLower()));
			
			//Add Search Criteria here
			return await _contactRepository.GetAllAsync(model.PageNumber, model.PageSize, c => c.Id.ToString().Contains(model.Search.ToLower()));
		}
	}
}
