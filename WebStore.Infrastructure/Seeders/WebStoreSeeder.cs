using WebStore.Domain.Entities;
using WebStore.Infrastructure.Persistence;

namespace WebStore.Infrastructure.Seeders
{
    internal class WebStoreSeeder(WebStoreDbContext dbContext) : IWebStoreSeeder
    {
        private readonly WebStoreDbContext _dbContext = dbContext;
        private readonly Guid AppleId = Guid.NewGuid();
        private readonly Guid SonyId = Guid.NewGuid();
        private readonly Guid PenguinId = Guid.NewGuid();
        private readonly Guid ManningId = Guid.NewGuid();
        private readonly Guid EletronictsId = Guid.NewGuid();
        private readonly Guid BooksId = Guid.NewGuid();

        public async Task Seed()
        {
            if (await _dbContext.Database.CanConnectAsync())
            {
                //if (!_dbContext.Brands.Any())
                //{
                //    var brands = GetBrands();
                //    _dbContext.Brands.AddRange(brands);
                //}
                //if (!_dbContext.Categories.Any())
                //{
                //    var categories = GetCategories();
                //    _dbContext.Categories.AddRange(categories);
                //}
                if (!_dbContext.WebStores.Any())
                {
                    var webStores = GetWebStores();
                    _dbContext.WebStores.AddRange(webStores);
                }
                await _dbContext.SaveChangesAsync();
            }
        }        

        private IEnumerable<Brand> GetBrands()
        {
            return
            [
                new Brand
                {
                    Id = AppleId,
                    Name = "Apple",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Brand
                {
                    Id = SonyId,
                    Name = "Sony",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Brand
                {
                    Id = PenguinId,
                    Name = "Penguin",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Brand
                {
                    Id = ManningId,
                    Name = "Manning",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            ];
        }

        private IEnumerable<Category> GetCategories()
        {
            return
            [
                new Category
                {
                    Id = EletronictsId,
                    Name = "Electronics",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Category
                {
                    Id = BooksId,
                    Name = "Books",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            ];
        }

        private IEnumerable<Domain.Entities.WebStore> GetWebStores()
        {
            var electronicsCategory = GetCategories().First(c => c.Name == "Electronics");
            var booksCategory = GetCategories().First(c => c.Name == "Books");

            var brandApple = GetBrands().First(b => b.Name == "Apple");
            var brandSony = GetBrands().First(b => b.Name == "Sony");
            var brandPenguin = GetBrands().First(b => b.Name == "Penguin");
            var brandManning = GetBrands().First(b => b.Name == "Manning");

            var techHavenId = Guid.NewGuid();
            var bookNookId = Guid.NewGuid();

            var webStores = new List<Domain.Entities.WebStore>
            {
                new() 
                {
                    Id = techHavenId,
                    Name = "Tech Haven",
                    Description = "Your one-stop shop for the latest electronics and gadgets.",
                    HasDelivery = true,
                    Address = new Address
                    {
                        Id = Guid.NewGuid(),
                        City = "Seattle",
                        Street = "123 Tech Ave",
                        PostalCode = "98101"
                    },
                    ContactPhoneNumber = "555-1234",
                    ContactEmail = "info@techhaven.com",
                    ExtraInfo = "Open 9am-9pm daily",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Id = Guid.NewGuid(),
                            Name = "iPhone 15 Pro",
                            Description = "Latest Apple smartphone with advanced features.",
                            Price = 1099.99m,
                            Category = electronicsCategory,
                            Brand = brandApple,
                            Model = "A3100",
                            ImageUrl = "https://example.com/iphone15pro.jpg",
                            WebStoreId = techHavenId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Product
                        {
                            Id = Guid.NewGuid(),
                            Name = "Sony WH-1000XM5",
                            Description = "Industry-leading noise canceling headphones.",
                            Price = 349.99m,
                            Category = electronicsCategory,
                            Brand = brandSony,
                            Model = "WH-1000XM5",
                            ImageUrl = "https://example.com/sonywh1000xm5.jpg",
                            WebStoreId = techHavenId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    }
                },
                new Domain.Entities.WebStore
                {
                    Id = bookNookId,
                    Name = "Book Nook",
                    Description = "A cozy place for book lovers.",
                    HasDelivery = false,
                    Address = new Address
                    {
                        Id = Guid.NewGuid(),
                        City = "Portland",
                        Street = "456 Reading Rd",
                        PostalCode = "97205"
                    },
                    ContactPhoneNumber = "555-5678",
                    ContactEmail = "contact@booknook.com",
                    ExtraInfo = "Specializes in rare and used books",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Products = new List<Product>
                    {
                        new Product
                        {
                            Id = Guid.NewGuid(),
                            Name = "The Great Gatsby",
                            Description = "Classic novel by F. Scott Fitzgerald.",
                            Price = 14.99m,
                            Category = booksCategory,
                            Brand = brandPenguin,
                            Model = "Paperback",
                            ImageUrl = "https://example.com/gatsby.jpg",
                            WebStoreId = bookNookId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        },
                        new Product
                        {
                            Id = Guid.NewGuid(),
                            Name = "C# in Depth",
                            Description = "Comprehensive guide to C# programming.",
                            Price = 49.99m,
                            Category = booksCategory,
                            Brand = brandManning,
                            Model = "4th Edition",
                            ImageUrl = "https://example.com/csharpindepth.jpg",
                            WebStoreId = bookNookId,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        }
                    }
                }
            };

            return webStores;
        }
    }
}
