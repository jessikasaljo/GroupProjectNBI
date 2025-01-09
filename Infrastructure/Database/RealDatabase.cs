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
        public DbSet<Store> Stores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Store>()
                .HasIndex(s => s.Location)
                .IsUnique();
        }
    }
}
