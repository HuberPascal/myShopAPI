using Microsoft.EntityFrameworkCore;
using myShopAPI.Models;

namespace myShopAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart) // CartItem gehört zu einem Cart
                .WithMany(c => c.Items) // Ein Cart kann viele CartItems haben
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade); // Warenkorb-Löschung löscht auch CartItems
        }
    }
}
