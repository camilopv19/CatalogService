using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Hashes> Hashes { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the non-identity ID property in Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever(); // This prevents it from being an identity column
                                                                  // Other configurations for the entity
            });

            // Configure the non-identity ID property in Item
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever(); // This prevents it from being an identity column
                                                                  // Other configurations for the entity
            });
            // Configure the non-identity ID property in Hashes
            modelBuilder.Entity<Hashes>(entity =>
            {
                entity.HasKey(e => e.Hash);
                entity.Property(e => e.Hash).ValueGeneratedNever(); // This prevents it from being an identity column
                                                                  // Other configurations for the entity
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
