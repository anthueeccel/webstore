using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;

namespace WebStore.Infrastructure.Persistence
{
    internal class WebStoreDbContext(DbContextOptions<WebStoreDbContext> options) : DbContext(options)
    {
        internal DbSet<Brand> Brands { get; set; } = default!;
        internal DbSet<Category> Categories { get; set; } = default!;
        internal DbSet<Product> Products { get; set; } = default!;
        internal DbSet<Domain.Entities.WebStore> WebStores { get; set; } = default!;
        
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
        }
    }
}
