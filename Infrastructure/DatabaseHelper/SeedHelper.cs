using Domain.Models;
using Infrastructure.Database;

namespace Data
{
    public static class SeedHelper
    {
        public static void Seed(RealDatabase db)
        {
            if (!db.Users.Any())
            {
                db.Users.AddRange(new List<User>
                {
                    new User { UserName = "harrypotter", UserPass = BCrypt.Net.BCrypt.HashPassword("1234"), FirstName = "Harry", LastName = "Potter", Email = "harry@hogwarts.com", Phone = "123-456-7890", Address = "4 Privet Drive, Little Whinging, Surrey", Role = UserRole.Customer },
                    new User { UserName = "frodo", UserPass = BCrypt.Net.BCrypt.HashPassword("1234"), FirstName = "Frodo", LastName = "Baggins", Email = "frodo@middleearth.com", Phone = "987-654-3210", Address = "Bag End, Hobbiton, The Shire", Role = UserRole.Customer },
                    new User { UserName = "belle", UserPass = BCrypt.Net.BCrypt.HashPassword("1234"), FirstName = "Belle", LastName = "Beaumont", Email = "belle@disney.com", Role = UserRole.StoreAdmin },
                    new User { UserName = "ariel", UserPass = BCrypt.Net.BCrypt.HashPassword("1234"), FirstName = "Ariel", LastName = "Triton", Email = "ariel@disney.com", Address = "Under the Sea, Atlantica", Role = UserRole.ProductAdmin }
                });
                db.SaveChanges();
            }

            if (!db.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Apple", Price = 4.98m },
                    new Product { Name = "Milk", Price = 12.86m },
                    new Product { Name = "Bread", Price = 29.30m },
                    new Product { Name = "Cheese", Price = 45.60m },
                    new Product { Name = "Orange Juice", Price = 18.75m },
                    new Product { Name = "Eggs", Price = 22.50m },
                    new Product { Name = "Cashews", Price = 65.00m },
                    new Product { Name = "Rice", Price = 15.90m },
                    new Product { Name = "Pasta", Price = 13.50m },
                    new Product { Name = "Tomatoes", Price = 9.80m },
                    new Product { Name = "Potatoes", Price = 7.40m },
                    new Product { Name = "Onions", Price = 6.20m },
                    new Product { Name = "Carrots", Price = 8.30m },
                    new Product { Name = "Spinach", Price = 11.50m },
                    new Product { Name = "Bourbon Vanilla Paste", Price = 89.99m },
                    new Product { Name = "Tofu", Price = 20.00m },
                    new Product { Name = "Butter", Price = 34.20m },
                    new Product { Name = "Yoghurt", Price = 17.60m },
                    new Product { Name = "Honey", Price = 25.80m },
                    new Product { Name = "Coffee", Price = 49.99m }
                };

                db.Products.AddRange(products);
                db.SaveChanges();

                foreach (var product in products)
                {
                    db.ProductDetail.Add(new ProductDetail { Product = product });
                }
                db.SaveChanges();
            }

            if (!db.Carts.Any())
            {
                var requiredUsers = new[] { "harrypotter", "frodo" };

                foreach (var userName in requiredUsers)
                {
                    var user = db.Users.FirstOrDefault(u => u.UserName == userName);
                    if (user == null)
                    {
                        throw new Exception($"User '{userName}' not found.");
                    }
                }

                var requiredProducts = new[] { "Apple", "Milk", "Bread" };
                foreach (var productName in requiredProducts)
                {
                    var product = db.Products.FirstOrDefault(p => p.Name == productName);
                    if (product == null)
                    {
                        throw new Exception($"Product '{productName}' not found.");
                    }
                }

                db.Carts.AddRange(new List<Cart>
                {
                    new Cart
                    {
                        UserId = db.Users.First(u => u.UserName == "harrypotter").Id,
                        Items = new List<CartItem>
                        {
                            new CartItem { ProductId = db.Products.First(p => p.Name == "Apple").Id, Quantity = 4 },
                            new CartItem { ProductId = db.Products.First(p => p.Name == "Tofu").Id, Quantity = 1 },
                            new CartItem { ProductId = db.Products.First(p => p.Name == "Cheese").Id, Quantity = 2 }

                        }
                    },
                    new Cart
                    {
                        UserId = db.Users.First(u => u.UserName == "frodo").Id,
                        Items = new List<CartItem>
                        {
                            new CartItem { ProductId = db.Products.First(p => p.Name == "Milk").Id, Quantity = 2 },
                            new CartItem { ProductId = db.Products.First(p => p.Name == "Bread").Id, Quantity = 1 },
                            new CartItem { ProductId = db.Products.First(p => p.Name == "Honey").Id, Quantity = 1 },
                            new CartItem { ProductId = db.Products.First(p => p.Name == "Yoghurt").Id, Quantity = 1 }
                        }
                    }
                });
                db.SaveChanges();
            }

            if (!db.Stores.Any())
            {
                db.Stores.AddRange(new List<Store>
                {
                    new Store
                    {
                        Location = "Enchanted Forest",
                        StoreItems = db.Products.Take(15).Select(p => new StoreItem { ProductId = p.Id, Quantity = 20 }).ToList()
                    },
                    new Store
                    {
                        Location = "Wonderland",
                        StoreItems = db.Products.Skip(4).Take(15).Select(p => new StoreItem { ProductId = p.Id, Quantity = 20 }).ToList()
                    }
                });
                db.SaveChanges();
            }

            if (!db.Transactions.Any())
            {
                db.Transactions.AddRange(new List<Transaction>
                {
                    new Transaction
                    {
                        StoreId = db.Stores.First(s => s.Location == "Enchanted Forest").Id,
                        CartId = db.Carts.First(c => c.UserId == db.Users.First(u => u.UserName == "harrypotter").Id).Id,
                        TransactionDate = DateTime.UtcNow
                    },
                    new Transaction
                    {
                        StoreId = db.Stores.First(s => s.Location == "Wonderland").Id,
                        CartId = db.Carts.First(c => c.UserId == db.Users.First(u => u.UserName == "frodo").Id).Id,
                        TransactionDate = DateTime.UtcNow.AddDays(-1)
                    }
                });
                db.SaveChanges();
            }

            if (!db.ProductDetail.Any())
            {
                var productDetails = db.Products.Select(p => new ProductDetail { Product = p }).ToList();
                db.ProductDetail.AddRange(productDetails);
                db.SaveChanges();
            }

            if (!db.DetailInformation.Any())
            {
                var productDetails = db.ProductDetail.ToList();
                var detailInfo = new List<DetailInformation>();

                foreach (var productDetail in productDetails)
                {
                    detailInfo.AddRange(new List<DetailInformation>
                    {
                        new DetailInformation { Title = "Description", Text = $"Premium quality {productDetail.Product.Name}.", ProductDetail = productDetail },
                        new DetailInformation { Title = "Nutritional Facts", Text = "Packed with essential nutrients.", ProductDetail = productDetail },
                        new DetailInformation { Title = "Storage Instructions", Text = "Store in a cool, dry place.", ProductDetail = productDetail }
                    });
                }
                db.DetailInformation.AddRange(detailInfo);
                db.SaveChanges();
            }
        }
    }
}