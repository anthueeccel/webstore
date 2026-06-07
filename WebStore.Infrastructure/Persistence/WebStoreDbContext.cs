using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Persistence
{
    public class WebStoreDbContext(DbContextOptions<WebStoreDbContext> options) : DbContext(options)
    {
        public DbSet<Brand> Brands { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Domain.Entities.WebStore> WebStores { get; set; } = default!;
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>()
                .HasMany(p => p.Products);

            modelBuilder.Entity<Category>()
                .HasMany(p => p.Products);

            modelBuilder.Entity<Domain.Entities.WebStore>()
                .OwnsOne(a => a.Address);

            modelBuilder.Entity<Domain.Entities.WebStore>()
                .HasMany(p => p.Products)
                .WithOne()
                .HasForeignKey(p => p.WebStoreId);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price).HasPrecision(18, 2);
            });
        }
    }
}
