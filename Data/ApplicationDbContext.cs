using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using myShopAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace myShopAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            Guid userId = Guid.NewGuid();
            Guid roleId = Guid.NewGuid();

            var hasher = new PasswordHasher<IdentityUser>();

            var adminUser = new IdentityUser<Guid>
            {
                Id = userId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@yopmail.com",
                NormalizedEmail = "ADMIN@YOPMAIL.COM",
                EmailConfirmed = true,
                PhoneNumber = "0980980980",
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null!, "Admin@123")
            };

            var adminRole = new IdentityRole<Guid>
            {
                Id = roleId,
                Name = "admin",
                NormalizedName = "ADMIN"
            };

            var adminUserRole = new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = userId
            };

            builder.Entity<IdentityUser<Guid>>().HasData(adminUser);
            builder.Entity<IdentityRole<Guid>>().HasData(adminRole);
            builder.Entity<IdentityUserRole<Guid>>().HasData(adminUserRole);

            // Beziehungen konfigurieren
            builder.Entity<Cart>()
                .HasMany(c => c.Items)
                .WithOne()
                .HasForeignKey(i => i.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }
    }
}