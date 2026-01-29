using GameZoneErp.Shared.Entities;
using GameZoneErp.Shared.Enums;

namespace GameZoneErp.Server.Data
{
    public static class DbInitializer
    {
        public static void Initialize(GameZoneDbContext context)
        {
            // context.Database.EnsureCreated(); // Migration is used in Program.cs

            // Look for any users.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            // Seed Users
            var users = new User[]
            {
                new User { Name = "Admin", Role = UserRole.Admin, PasswordHash = "admin123" }, // In real app, hash this!
                new User { Name = "Manager", Role = UserRole.Manager, PasswordHash = "manager123" },
                new User { Name = "Cashier", Role = UserRole.Cashier, PasswordHash = "cashier123" }
            };
            context.Users.AddRange(users);

            // Seed Categories
            var categories = new Category[]
            {
                new Category { Name = "Boba" },
                new Category { Name = "Boost Discounts" },
                new Category { Name = "Party-on-Billiards" },
                new Category { Name = "Party-on-Bowling" },
                new Category { Name = "Raw Materials" },
                new Category { Name = "Video Games" }
            };
            context.Categories.AddRange(categories);
            context.SaveChanges(); // Save to get Ids

            // Seed Products
            // We need to fetch categories to get their IDs
            var bobaCat = context.Categories.First(c => c.Name == "Boba");
            var billiardsCat = context.Categories.First(c => c.Name == "Party-on-Billiards");
            var bowlingCat = context.Categories.First(c => c.Name == "Party-on-Bowling");

            var products = new Product[]
            {
                new Product { Name = "Classic Milk Tea", CategoryId = bobaCat.Id, CostPrice = 2.0m, SalePrice = 5.0m, StockQuantity = 100, Type = ProductType.Standard },
                new Product { Name = "Taro Milk Tea", CategoryId = bobaCat.Id, CostPrice = 2.0m, SalePrice = 5.5m, StockQuantity = 100, Type = ProductType.Standard },

                new Product { Name = "Billiards (1 Hour)", CategoryId = billiardsCat.Id, CostPrice = 0m, SalePrice = 10.0m, StockQuantity = 0, Type = ProductType.Service },
                new Product { Name = "Bowling (1 Game)", CategoryId = bowlingCat.Id, CostPrice = 0m, SalePrice = 15.0m, StockQuantity = 0, Type = ProductType.Service }
            };
            context.Products.AddRange(products);

            context.SaveChanges();
        }
    }
}
