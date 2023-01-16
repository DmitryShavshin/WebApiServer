using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiServer.Models;

namespace WebApiServer.Data
{
    public class DataContext: IdentityDbContext<User, Role, Guid>
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }


        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<Brand> Brands { get; set; }
    }
}
