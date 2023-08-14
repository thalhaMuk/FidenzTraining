using FidenzLevelOne.Models;
using Microsoft.EntityFrameworkCore;

namespace FidenzLevelOne.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> customers { get; set; }
    }
}
