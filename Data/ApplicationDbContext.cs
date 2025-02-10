using Microsoft.EntityFrameworkCore;
using myShopAPI.Models;

namespace myShopAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }
    }
}
