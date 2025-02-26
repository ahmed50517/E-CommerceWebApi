using E_Commerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Category> Categories { get; set; } // to create table Categories + do migration
        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) //seed data
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Honor",
                    Author = "Billy Spark",
                    Description = "Fortune of Time Description",
                    ISBN = "SWD1",
                    Price = 90,
                    CategoryId = 1,
                    IsAvailable = true,
                    Stock=50
                }, new Product
                {
                    Id = 2,
                    Title = "Beloved",
                    Author = "Nancy Hoover",
                    Description = "Dark Skies Description",
                    ISBN = "SWD2",
                    Price = 30,
                    CategoryId = 1,
                    IsAvailable=true,
                    Stock=40
                },
                new Product
                {
                    Id = 3,
                    Title = "Night",
                    Author = "Julian Button",
                    Description = "Vanish in the Sunset Description",
                    ISBN = "SWD3",
                    Price = 50,
                    CategoryId = 1,
                    IsAvailable=true,
                    Stock=25
                },
                new Product
                {
                    Id = 4,
                    Title = "Elsewhere",
                    Author = "Abby Muscles",
                    Description = "Cotton Candy Description",
                    ISBN = "HBK4",
                    Price = 65,
                    CategoryId = 2,
                    IsAvailable = true,
                    Stock=20
                },
                new Product
                {
                    Id = 5,
                    Title = "Island",
                    Author = "Ron Parker",
                    Description = "Rock in the Ocean Description",
                    ISBN = "OITKL5",
                    Price = 27,
                    CategoryId = 2,
                    IsAvailable = true,
                    Stock=5
                },
                new Product
                {
                    Id = 6,
                    Title = "Winter",
                    Author = "Laura Phantom",
                    Description = "Leaves And Wonders Description",
                    ISBN = "JGHGPVYOOR",
                    Price = 23,
                    CategoryId = 3,
                    IsAvailable = false,
                    Stock=0
                });

        }
    }
}


