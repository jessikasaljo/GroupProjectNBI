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
                User harry = new User { UserName = "harrypotter", UserPass = "1234", FirstName = "Harry", LastName = "Potter", Email = "harry@hogwarts.com", Phone = "123-456-7890", Address = "4 Privet Drive, Little Whinging, Surrey", Role = UserRole.Customer };
                User frodo = new User { UserName = "frodo", UserPass = "1234", FirstName = "Frodo", LastName = "Baggins", Email = "frodo@middleearth.com", Phone = "987-654-3210", Address = "Bag End, Hobbiton, The Shire", Role = UserRole.Customer };
                User belle = new User { UserName = "belle", UserPass = "1234", FirstName = "Belle", LastName = "Beaumont", Email = "belle@disney.com", Role = UserRole.StoreAdmin };
                User ariel = new User { UserName = "ariel", UserPass = "1234", FirstName = "Ariel", LastName = "Triton", Email = "ariel@disney.com", Address = "Under the Sea, Atlantica", Role = UserRole.ProductAdmin };

                var users = new List<User> { harry, frodo, belle, ariel };
                db.Users.AddRange(users);
                db.SaveChanges();
            }

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
            }

            if (!db.Carts.Any())
            {
                var harry = db.Users.FirstOrDefault(u => u.UserName == "harrypotter");
                var frodo = db.Users.FirstOrDefault(u => u.UserName == "frodo");

                var carts = new List<Cart>
                {
                    new Cart
                    {
                        UserId = harry.Id,
                        Items = new List<CartItem>
                        {
                            new CartItem { ProductId = db.Products.FirstOrDefault(p => p.Name == "Apple").Id, Quantity = 4 },
                            new CartItem { ProductId = db.Products.FirstOrDefault(p => p.Name == "Milk").Id, Quantity = 1 }
                        }
                    },
                    new Cart
                    {
                        UserId = frodo.Id,
                        Items = new List<CartItem>
                        {
                            new CartItem { ProductId = db.Products.FirstOrDefault(p => p.Name == "Milk").Id, Quantity = 2 },
                            new CartItem { ProductId = db.Products.FirstOrDefault(p => p.Name == "Bread").Id, Quantity = 1 }
                        }
                    }
                };
                db.Carts.AddRange(carts);
                db.SaveChanges();
            }

            if (!db.Stores.Any())
            {
                var stores = new List<Store>
                {
                    new Store
                    {
                        Location = "Enchanted Forest",
                        StoreItems = new List<StoreItem>
                        {
                            new StoreItem { ProductId = db.Products.FirstOrDefault(p => p.Name == "Apple").Id, Quantity = 50 },
                            new StoreItem { ProductId = db.Products.FirstOrDefault(p => p.Name == "Bread").Id, Quantity = 20 }
                        }
                    },
                    new Store
                    {
                        Location = "Wonderland",
                        StoreItems = new List<StoreItem>
                        {
                            new StoreItem { ProductId = db.Products.FirstOrDefault(p => p.Name == "Milk").Id, Quantity = 30 },
                            new StoreItem { ProductId = db.Products.FirstOrDefault(p => p.Name == "Bread").Id, Quantity = 15 }
                        }
                    }
                };
                db.Stores.AddRange(stores);
                db.SaveChanges();
            }

            if (!db.Transactions.Any())
            {
                var store1 = db.Stores.FirstOrDefault(s => s.Location == "Enchanted Forest");
                var store2 = db.Stores.FirstOrDefault(s => s.Location == "Wonderland");

                var cart1 = db.Carts.FirstOrDefault(c => c.UserId == db.Users.FirstOrDefault(u => u.UserName == "harrypotter").Id);
                var cart2 = db.Carts.FirstOrDefault(c => c.UserId == db.Users.FirstOrDefault(u => u.UserName == "frodo").Id);

                var transactions = new List<Transaction>
                {
                    new Transaction { StoreId = store1.Id, CartId = cart1.Id, TransactionDate = DateTime.UtcNow },
                    new Transaction { StoreId = store2.Id, CartId = cart2.Id, TransactionDate = DateTime.UtcNow.AddDays(-1) }
                };
                db.Transactions.AddRange(transactions);
                db.SaveChanges();
            }

            if (!db.ProductDetail.Any())
            {
                var productDetails = new List<ProductDetail>
                {
                    new ProductDetail
                    {
                        ProductId = db.Products.FirstOrDefault(p => p.Name == "Apple").Id,
                        DetailInformation = new List<DetailInformation>
                        {
                            new DetailInformation { Title = "Description", Text = "Fresh red apple." },
                            new DetailInformation { Title = "Nutritional Facts", Text = "Rich in vitamins and antioxidants." },
                            new DetailInformation { Title = "Origin", Text = "Sourced from the magical orchards of the Enchanted Forest." }
                        }
                    },
                    new ProductDetail
                    {
                        ProductId = db.Products.FirstOrDefault(p => p.Name == "Milk").Id,
                        DetailInformation = new List<DetailInformation>
                        {
                            new DetailInformation { Title = "Volume", Text = "1 l."},
                            new DetailInformation { Title = "Nutritional Facts", Text = "High in calcium and protein." },
                            new DetailInformation { Title = "Storage Instructions", Text = "Keep refrigerated at 4°C." }
                        }
                    }
                };
                db.ProductDetail.AddRange(productDetails);
                db.SaveChanges();
            }
        }
    }
}