using IdentityCoreDemo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace IdentityCoreDemo.Data
{
    public class StoreContext : IdentityDbContext<AppUser>
    {

        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {


        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
