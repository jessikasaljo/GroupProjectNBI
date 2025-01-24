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
                    new Product { Name = "Bread", Price = 29.30m }
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
                            new CartItem { ProductId = db.Products.First(p => p.Name == "Milk").Id, Quantity = 1 }
                        }
                    },
                    new Cart
                    {
                        UserId = db.Users.First(u => u.UserName == "frodo").Id,
                        Items = new List<CartItem>
                        {
                            new CartItem { ProductId = db.Products.First(p => p.Name == "Milk").Id, Quantity = 2 },
                            new CartItem { ProductId = db.Products.First(p => p.Name == "Bread").Id, Quantity = 1 }
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
                        StoreItems = new List<StoreItem>
                        {
                            new StoreItem { ProductId = db.Products.First(p => p.Name == "Apple").Id, Quantity = 50 },
                            new StoreItem { ProductId = db.Products.First(p => p.Name == "Bread").Id, Quantity = 20 }
                        }
                    },
                    new Store
                    {
                        Location = "Wonderland",
                        StoreItems = new List<StoreItem>
                        {
                            new StoreItem { ProductId = db.Products.First(p => p.Name == "Milk").Id, Quantity = 30 },
                            new StoreItem { ProductId = db.Products.First(p => p.Name == "Bread").Id, Quantity = 15 }
                        }
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
                db.ProductDetail.AddRange(new List<ProductDetail>
                {
                    new ProductDetail { Product = db.Products.First(p => p.Name == "Apple") },
                    new ProductDetail { Product = db.Products.First(p => p.Name == "Milk") }
                });
                db.SaveChanges();
            }

            if (!db.DetailInformation.Any())
            {
                var firstTargetProduct = db.ProductDetail.FirstOrDefault(pd => pd.Product.Name == "Apple");
                var secondTargetProduct = db.ProductDetail.FirstOrDefault(pd => pd.Product.Name == "Milk");
                if (firstTargetProduct == null || secondTargetProduct == null)
                {
                    throw new Exception("Product details not found.");
                }

                db.DetailInformation.AddRange(new List<DetailInformation>
                {
                    new DetailInformation { Title = "Description", Text = "Fresh red apple.", ProductDetail = firstTargetProduct },
                    new DetailInformation { Title = "Nutritional Facts", Text = "Rich in vitamins and antioxidants.", ProductDetail = firstTargetProduct },
                    new DetailInformation { Title = "Origin", Text = "Sourced from the magical orchards of the Enchanted Forest.", ProductDetail = firstTargetProduct },
                    new DetailInformation { Title = "Volume", Text = "1 l.", ProductDetail = secondTargetProduct },
                    new DetailInformation { Title = "Nutritional Facts", Text = "High in calcium and protein.", ProductDetail = secondTargetProduct },
                    new DetailInformation { Title = "Storage Instructions", Text = "Keep refrigerated at 4°C.", ProductDetail = secondTargetProduct }
                });
                db.SaveChanges();
            }
        }

    }
}