using CarbonTest1.Models;
using Microsoft.EntityFrameworkCore;

namespace CarbonTest1.Data
{
    public class EosDbContext : DbContext
    {
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Log> Logs { get; set; }

        public EosDbContext(DbContextOptions<EosDbContext> options) : base(options) { }
    }
}