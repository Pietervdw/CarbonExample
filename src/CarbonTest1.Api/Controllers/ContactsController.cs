using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CarbonTest1.Infrastructure.Extensions;
using CarbonTest1.Infrastructure.Identity;
using CarbonTest1.Models;
using CarbonTest1.Models.Dtos;
using CarbonTest1.Services.Interfaces;

namespace CarbonTest1.Api.Controllers
{
	[ApiController]
	[Authorize()]
	[Route("contacts")]
	public class ContactsController : ControllerBase
	{
		private readonly IContactService _contactService;

		public ContactsController(IContactService contactService)
		{
			_contactService = contactService;
		}

		[HttpPost]
		public async Task<ActionResult> GetAll(PagedListQueryDto model)
		{
			var data = await _contactService.GetAll(model);
			return Ok(data);
		}

		[HttpPost("add")]
		public async Task<ActionResult> Add(Contact contact)
		{
			if (!ModelState.IsValid)
				return BadRequest(new EntityResponse<Contact>(contact, ModelState.Errors()));

			var entityResponse = await _contactService.Add(contact);
			return Ok(entityResponse);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult> Get(int id)
		{
            var contact = await _contactService.Get(id);
			return Ok(contact);
		}

		[HttpPatch()]
		public async Task<ActionResult> Update(Contact contactModel)
		{
			if (!ModelState.IsValid)
				return BadRequest(new EntityResponse<Contact>(contactModel, ModelState.Errors()));

			var entityResponse = await _contactService.Update(contactModel);
			return Ok(entityResponse);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			await _contactService.Delete(id);
			return Ok();
		}
	}
}
