using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SIDWeb.Model;

namespace SIDWeb.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AppUser>? Users { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public void EnsureDatabaseUpToDate()
        {
            Database.EnsureCreated();
        }
    }
}
