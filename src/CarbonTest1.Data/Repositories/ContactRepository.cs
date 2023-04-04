using Microsoft.AspNetCore.Http;
using CarbonTest1.Data.Interfaces;
using CarbonTest1.Infrastructure.Identity;
using CarbonTest1.Models;
using Microsoft.EntityFrameworkCore;

namespace CarbonTest1.Data.Repositories
{
	public class ContactRepository : RepositoryBase<Contact>, IContactRepository
	{
        public ContactRepository(EosDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
            if (httpContextAccessor.HttpContext.User.CompanyId() == 2)
            {
                context.Database.SetConnectionString("Server=(local);Database=EosDb;User Id=sa;Password=system;Connection Timeout=180;");
            }
            
        }

        
	}
}
