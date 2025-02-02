﻿using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database
{
    public class RealDatabase : DbContext
    {
        public RealDatabase(DbContextOptions<RealDatabase> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetail { get; set; }
        public DbSet<DetailInformation> DetailInformation { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreItem> StoreItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<ProductDetail>()
                .HasOne(pd => pd.Product)
                .WithOne(p => p.ProductDetail)
                .HasForeignKey<ProductDetail>(pd => pd.ProductId);

            modelBuilder.Entity<DetailInformation>()
                .HasOne(di => di.ProductDetail)
                .WithMany(pd => pd.DetailInformation)
                .HasForeignKey(di => di.ProductId);

            modelBuilder.Entity<Store>()
                .HasIndex(s => s.Location)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

    }
}
