using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Readzy.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace Readzy.DataAccess.Data
{

    public class ReadzyContext : IdentityDbContext<ApplicationUser>
    {
        #region Ctor-s
        public ReadzyContext() : base() { } //For DI declaration

        public ReadzyContext(DbContextOptions<ReadzyContext> options) : base(options) { } //For Connection Configuration 
        #endregion


        #region DBSets

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderHeader> OrderHeaders { get; set; }


        #endregion


        #region Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            //Category Entity Mapping
            modelBuilder.Entity<Category>(entityBuilder =>
            {
                entityBuilder.HasData(
                    new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                    new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                    new Category { Id = 3, Name = "History", DisplayOrder = 3 }
                    );
            });

            modelBuilder.Entity<Product>(entityBuilder => 
            {

                entityBuilder.HasData(
                     new Product
                     {
                         Id = 1,
                         Title = "Fortune of Time",
                         Author = "Billy Spark",
                         Price = 90,
                         ImageURL="",
                         Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                         ISBN = "SWD9999001",
                         CategoryId = 1
                     },
                    new Product
                    {
                        Id = 2,
                        Title = "Dark Skies",
                        Author = "Nancy Hoover",
                        Price = 30,
                        ImageURL = "",
                        Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                        ISBN = "SzD9999001",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Id = 3,
                        Title = "Vanish in the Sunset",
                        Author = "Julian Button",
                        Price = 50,
                        ImageURL = "",
                        Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                        ISBN = "SWD90999001",
                        CategoryId = 1
                    },
                    new Product
                    {
                        Id = 4,
                        Title = "Cotton Candy",
                        Author = "Abby Muscles",
                        Price = 65,
                        ImageURL = "",
                        Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                        ISBN = "SWD9199001",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Id = 5,
                        Title = "Rock in the Ocean",
                        Author = "Ron Parker",
                        Price = 27,
                        ImageURL = "",
                        Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                        ISBN = "Szh9999001",
                        CategoryId = 2
                    },
                    new Product
                    {
                        Id = 6,
                        Title = "Leaves and Wonders",
                        Author = "Laura Phantom",
                        Price = 23,
                        ImageURL = "",
                        Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                        ISBN = "SW102499001",
                        CategoryId = 3
                    }

                    );
            });

           
        }
        #endregion
    }
}
