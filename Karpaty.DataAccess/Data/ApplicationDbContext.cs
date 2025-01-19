using Karpaty.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Karpaty.Data
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<AplicationUser> AplicationUsers { get; set; }
        public DbSet<ShoppingCard> ShoppingCards { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData
                (
                new Category
                {
                    Id = 1,
                    Name = "Clothing",
                    DisplayNumber = 1
                },
                new Category
                {
                    Id = 2,
                    Name = "Attributes",
                    DisplayNumber = 2
                },
                new Category
                {
                    Id = 3,
                    Name = "Else",
                    DisplayNumber = 3
                }
                );


            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "T-Shirt",
                    Description = "Team logo t-shirt",
                    Price = 29.99,
                    Count = 100,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "Shorts",
                    Description = "Sports shorts",
                    Price = 19.99,
                    Count = 50,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 3,
                    Name = "Soccer Ball",
                    Description = "Official size soccer ball",
                    Price = 39.99,
                    Count = 30,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 4,
                    Name = "Cap",
                    Description = "Baseball cap with team logo",
                    Price = 15.99,
                    Count = 75,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 5,
                    Name = "Jacket",
                    Description = "Windbreaker jacket",
                    Price = 49.99,
                    Count = 20,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 6,
                    Name = "Water Bottle",
                    Description = "Reusable water bottle",
                    Price = 12.99,
                    Count = 100,
                    CategoryId = 2

                }
            );
        

        }
    }
}
