using Domain.Models;
using Infrastructure.Database;
using Microsoft.Extensions.Logging;

namespace Data
{
    public static class SeedHelper
    {
        public static void Seed(RealDatabase db)
        {
            if (!db.Users.Any())
            {
                User harry = new User { UserName = "harrypotter", UserPass = BCrypt.Net.BCrypt.HashPassword("1234"), FirstName = "Harry", LastName = "Potter", Email = "harry@hogwarts.com", Phone = "123-456-7890", Address = "4 Privet Drive, Little Whinging, Surrey", Role = UserRole.Customer };
                User frodo = new User { UserName = "frodo", UserPass = BCrypt.Net.BCrypt.HashPassword("1234"), FirstName = "Frodo", LastName = "Baggins", Email = "frodo@middleearth.com", Phone = "987-654-3210", Address = "Bag End, Hobbiton, The Shire", Role = UserRole.Customer };
                User belle = new User { UserName = "belle", UserPass = BCrypt.Net.BCrypt.HashPassword("1234"), FirstName = "Belle", LastName = "Beaumont", Email = "belle@disney.com", Role = UserRole.StoreAdmin };
                User ariel = new User { UserName = "ariel", UserPass = BCrypt.Net.BCrypt.HashPassword("1234"), FirstName = "Ariel", LastName = "Triton", Email = "ariel@disney.com", Address = "Under the Sea, Atlantica", Role = UserRole.ProductAdmin };

                var users = new List<User> { harry, frodo, belle, ariel };
                db.Users.AddRange(users);
                db.SaveChanges();
            }
            var userDict = db.Users.ToDictionary(u => u.UserName);

            if (!db.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product { Name = "Apple", Price = 4.98m},
                    new Product { Name = "Milk", Price = 12.86m},
                    new Product { Name = "Bread", Price = 29.30m}
                };
                db.Products.AddRange(products);
                db.SaveChanges();

                foreach (var product in products)
                {
                    db.ProductDetail.Add(new ProductDetail { Product = product });
                }
                db.SaveChanges();
            }
            var productDict = db.Products.ToDictionary(p => p.Name);

            if (!db.Carts.Any())
            {
                var requiredUsers = new[] { "harrypotter", "frodo" };

                foreach (var userName in requiredUsers)
                {
                    if (!userDict.ContainsKey(userName))
                    {
                        throw new Exception($"User '{userName}' not found.");
                    }
                }

                var requiredProducts = new[] { "Apple", "Milk", "Bread" };
                foreach (var productName in requiredProducts)
                {
                    if (!productDict.ContainsKey(productName))
                    {
                        throw new Exception($"Product '{productName}' not found.");
                    }
                }

                var carts = new List<Cart>
                {
                    new Cart
                    {
                        UserId = userDict["harrypotter"].Id,
                        Items = new List<CartItem>
                        {
                            new CartItem { ProductId = productDict["Apple"].Id, Quantity = 4 },
                            new CartItem { ProductId = productDict["Milk"].Id, Quantity = 1 }
                        }
                    },
                    new Cart
                    {
                        UserId = userDict["frodo"].Id,
                        Items = new List<CartItem>
                        {
                            new CartItem { ProductId = productDict["Milk"].Id, Quantity = 2 },
                            new CartItem { ProductId = productDict["Bread"].Id, Quantity = 1 }
                        }
                    }
                };
                db.Carts.AddRange(carts);
                db.SaveChanges();
            }
            var cartDict = db.Carts.ToDictionary(c => c.UserId);

            if (!db.Stores.Any())
            {
                var requiredProducts = new[] { "Apple", "Milk", "Bread" };
                foreach (var productName in requiredProducts)
                {
                    if (!productDict.ContainsKey(productName))
                    {
                        throw new Exception($"Product '{productName}' not found.");
                    }
                }

                var stores = new List<Store>
                {
                    new Store
                    {
                        Location = "Enchanted Forest",
                        StoreItems = new List<StoreItem>
                        {
                            new StoreItem { ProductId = productDict["Apple"].Id, Quantity = 50 },
                            new StoreItem { ProductId = productDict["Bread"].Id, Quantity = 20 }
                        }
                    },
                    new Store
                    {
                        Location = "Wonderland",
                        StoreItems = new List<StoreItem>
                        {
                            new StoreItem { ProductId = productDict["Milk"].Id, Quantity = 30 },
                            new StoreItem { ProductId = productDict["Bread"].Id, Quantity = 15 }
                        }
                    }
                };
                db.Stores.AddRange(stores);
                db.SaveChanges();
            }
            var storeDict = db.Stores.ToDictionary(s => s.Location);

            if (!db.Transactions.Any())
            {
                var requiredStores = new[] { "Enchanted Forest", "Wonderland" };
                foreach (var storeLocation in requiredStores)
                {
                    if (!storeDict.ContainsKey(storeLocation))
                    {
                        throw new Exception($"Store '{storeLocation}' not found.");
                    }
                }
                var requiredCarts = new[] { "harrypotter", "frodo" };
                foreach (var userName in requiredCarts)
                {
                    if (!userDict.ContainsKey(userName))
                    {
                        throw new Exception($"User '{userName}' not found.");
                    }
                }

                var store1 = storeDict["Enchanted Forest"];
                var store2 = storeDict["Wonderland"];

                var cart1 = cartDict[userDict["harrypotter"].Id];
                var cart2 = cartDict[userDict["frodo"].Id];

                var transactions = new List<Transaction>
                {
                    new Transaction { StoreId = store1.Id, CartId = cart1.Id, TransactionDate = DateTime.UtcNow },
                    new Transaction { StoreId = store2.Id, CartId = cart2.Id, TransactionDate = DateTime.UtcNow.AddDays(-1) }
                };
                db.Transactions.AddRange(transactions);
                db.SaveChanges();
            }
            var transactionDict = db.Transactions.ToDictionary(t => t.Id);

            if (!db.ProductDetail.Any())
            {
                var requiredProducts = new[] { "Apple", "Milk" };
                foreach (var productName in requiredProducts)
                {
                    if (!productDict.ContainsKey(productName))
                    {
                        throw new Exception($"Product '{productName}' not found.");
                    }
                }

                var productDetails = new List<ProductDetail>
                {
                    new ProductDetail
                    {
                        Product = productDict["Apple"]
                    },
                    new ProductDetail
                    {
                        Product = productDict["Milk"]
                    }
                };

                foreach(var productDetail in productDetails)
                {
                    db.ProductDetail.Add(productDetail);
                }
                db.SaveChanges();
            }
            var productDetailDict = db.ProductDetail.ToDictionary(pd => pd.ProductId);

            if (!db.DetailInformation.Any())
            {
                var firstTargetProduct = db.ProductDetail.FirstOrDefault(pd => pd.ProductId == productDict["Apple"].Id);
                var secondTargetProduct = db.ProductDetail.FirstOrDefault(pd => pd.ProductId == productDict["Milk"].Id);

                var detailInformationList = new List<DetailInformation>
                {
                    new DetailInformation { Title = "Description", Text = "Fresh red apple.", ProductDetail = productDetailDict[1] },
                    new DetailInformation { Title = "Nutritional Facts", Text = "Rich in vitamins and antioxidants.", ProductDetail = productDetailDict[1] },
                    new DetailInformation { Title = "Origin", Text = "Sourced from the magical orchards of the Enchanted Forest.", ProductDetail = productDetailDict[1] },
                    new DetailInformation { Title = "Volume", Text = "1 l.", ProductDetail = productDetailDict[2] },
                    new DetailInformation { Title = "Nutritional Facts", Text = "High in calcium and protein.", ProductDetail = productDetailDict[2] },
                    new DetailInformation { Title = "Storage Instructions", Text = "Keep refrigerated at 4°C.", ProductDetail = productDetailDict[2] }
                };

                foreach (var detailInformation in detailInformationList)
                {
                    db.DetailInformation.Add(detailInformation);
                }
                db.SaveChanges();
            }
        }
    }
}